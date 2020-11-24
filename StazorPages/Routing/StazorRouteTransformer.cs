using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using StazorPages.Constants;
using StazorPages.Exceptions;
using StazorPages.Services;
using StazorPages.StazorFile;

namespace StazorPages.Routing
{
    public class StazorRouteTransformer : DynamicRouteValueTransformer
    {
        private readonly IContentService _contentService;
        private readonly StazorFileDetector _stazorFileDetector;

        public StazorRouteTransformer(IContentService contentService, StazorFileDetector stazorFileDetector)
        {
            _contentService = contentService;
            _stazorFileDetector = stazorFileDetector;
        }

        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext,
            RouteValueDictionary values)
        {
            var url = values[RouteParameters.Url] as string;

            if (TryServeStazorPage(url, values, out var filePath))
            {
                return new ValueTask<RouteValueDictionary>(values);
            }
            
            if (TryServeKnownPaths(url))
            {
                return new ValueTask<RouteValueDictionary>(values);
            }

            ServeDynamicContent(url, filePath, values);

            return new ValueTask<RouteValueDictionary>(values);
        }

        private void ServeDynamicContent(string url, string filePath, RouteValueDictionary values)
        {
            try
            {
                var content = _contentService.GetContentByUrlSlug(url).Result;

                if (content == null)
                {
                    values[RouteParameters.Controller] = "Error";
                    values[RouteParameters.Action] = "Index";
                    values[RouteParameters.StatusCode] = 404;
                }
                else
                {
                    values[RouteParameters.Controller] = content.GetContentTypeAlias();
                    values[RouteParameters.Action] = "Index";
                    values[RouteParameters.Model] = content;
                    values[RouteParameters.FilePath] = filePath;
                }
            }
            catch (Exception ex)
            {
                throw new StazorPageContentRetrievalException("Could not retrieve content from content service.", ex);
            }
        }

        private bool TryServeStazorPage(string url, RouteValueDictionary values, out string filePath)
        {
            filePath = url;

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = "index.html";
            }
            else
            {
                filePath = filePath.Trim('/');
                filePath = $"{filePath}/index.html";
            }

            if (!_stazorFileDetector.FileExists($"/{filePath}"))
            {
                return false;
            }

            values[RouteParameters.Url] = filePath;
            return true;
        }

        private bool TryServeKnownPaths(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            return url.StartsWith("api") || url.StartsWith("error") || _stazorFileDetector.FileExists($"/{url}");
        }
    }
}
