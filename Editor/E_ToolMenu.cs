using UnityEditor;
using System.IO;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;
using System.Threading.Tasks;
using System.Net.Http;

namespace Parvez
{
    public static class E_ToolMenu
    {
        [MenuItem("Tools/Setup/CreateDefaultFolders")]
        public static void CreateDefaultFolders()
        {
            CreateDirectories("_Project", "Scripts", "Scenes", "Art");
            Refresh();
        }
        [MenuItem("Tools/Setup/LoadNewManifest")]
        public async static void LoadNewManifest()
        {
            var url = GetGistUrl("0b2813feb39d384d3fddab01cc28071b");
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }

        public static void CreateDirectories(string root, params string[] arg)
        {
            var fullPath = Combine(dataPath, root);
            foreach (var newDir in arg)
            {
                CreateDirectory(Combine(fullPath, newDir));
            }
        }
        public static string GetGistUrl(string id, string user = "belovedparvez") => $"https://gist.githubusercontent.com/{user}/{id}/raw";
        public static async Task<string> GetContents(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public static void ReplacePackageFile(string contents)
        {
            var existing = Combine(dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing, contents);
            UnityEditor.PackageManager.Client.Resolve();
        }
    }
}
