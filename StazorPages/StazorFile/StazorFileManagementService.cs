using System.IO;
using System.Threading.Tasks;
using StazorPages.Constants;

namespace StazorPages.StazorFile
{
    public static class StazorFileManagementService
    {
        public static async Task SavePage(string response, string filePath)
        {
            var file = new FileInfo($"{DefaultFilePaths.StazorPageDirectory}/{filePath}");
            
            if (file.Directory != null)
            {
                Directory.CreateDirectory(file.Directory.ToString());
            }

            await using var streamWriter = file.CreateText();
            //await streamWriter.WriteLineAsync("@page");
            await streamWriter.WriteAsync(response);
            await streamWriter.FlushAsync();
        }

        public static void DeletePage(string filePath)
        {
            var file = new FileInfo($"{DefaultFilePaths.StazorPageDirectory}/{filePath}");

            if (!file.Exists)
            {
                return;
            }

            file.Delete();
            if (file.Directory != null && file.Directory.GetDirectories().Length == 0)
            {
                file.Directory.Delete();
            }
        }

        public static void EnsureStazorPageDirectory()
        {
            var directory = new DirectoryInfo(DefaultFilePaths.StazorPageDirectory);

            if (!directory.Exists)
            {
                directory.Create();
            }
        }
    }
}
