using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace StazorPages.StazorFile
{
    public class StazorFileDetector
    {
        private readonly StaticFileOptions _staticFileOptions;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public StazorFileDetector(IOptions<StazorFileOptions> options, IWebHostEnvironment env)
        {
            _staticFileOptions = options.Value;
            _hostingEnvironment = env;
        }

        public bool FileExists(PathString path)
        {
            var baseUrl = _staticFileOptions.RequestPath;

            // get the relative path
            if(!path.StartsWithSegments(baseUrl, out var relativePath))
            {
                return false;
            }

            var fileProvider = _staticFileOptions.FileProvider ?? _hostingEnvironment.WebRootFileProvider;

            var file = fileProvider.GetFileInfo(relativePath.Value);
            return !file.IsDirectory && file.Exists;
        }
    }
}
