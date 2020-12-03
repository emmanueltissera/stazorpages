using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StazorPages.Constants;
using StazorPages.StazorFile;

namespace StazorPages.Middleware
{
    public class StazorMiddleware
    {
        private readonly RequestDelegate _next;

        public StazorMiddleware(RequestDelegate next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var saveResponse = context.Request.RouteValues.ContainsKey(RouteParameters.FilePath);

            if (saveResponse)
            {
                await InvokeSavePage(context);
            }
            else
            {
                await _next(context);
            }
        }

        public async Task InvokeSavePage(HttpContext context)
        {
            var response = context.Response;
            string responseBody;

            await using (var responseMemoryStream = new MemoryStream())
            {
                var originalResponseBodyReference = response.Body;
                response.Body = responseMemoryStream;

                await _next(context);

                response.Body.Seek(0, SeekOrigin.Begin);
                responseBody = await new StreamReader(response.Body).ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);

                await responseMemoryStream.CopyToAsync(originalResponseBodyReference);
            }

            if (response.StatusCode == StatusCodes.Status200OK)
            {
                await StazorFileManagementService.SavePage(responseBody, context.Request.RouteValues[RouteParameters.FilePath]?.ToString());
            }
        }
    }
}

