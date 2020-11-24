using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using StazorPages.Constants;
using StazorPages.Middleware;
using StazorPages.StazorFile;

namespace StazorPages.Routing
{
    public static class StazorRoutingStartup
    {
        public static IServiceCollection AddStazorPages(this IServiceCollection services, IWebHostEnvironment env)
        {
            StazorFileManagementService.EnsureStazorPageDirectory();

            services.Configure<StazorFileOptions>(
                options =>
                {
                    options.FileProvider = new PhysicalFileProvider(
                        Path.Combine(env.ContentRootPath, DefaultFilePaths.StazorPageDirectory));
                });

            services.AddSingleton<StazorRouteTransformer>();

            services.AddSingleton<StazorFileDetector>();

            return services;
        }

        public static IApplicationBuilder UseStazorPages(this IApplicationBuilder app, IWebHostEnvironment env, bool useMiddleWare = true)
        {
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, DefaultFilePaths.StazorPageDirectory))
            });

            if (useMiddleWare)
            {
                app.UseMiddleware<StazorMiddleware>();
            }

            // re-add this here to show detailed messages - TODO: Bubble up errors to middleware
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }

        public static void MapStazorPages(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapDynamicControllerRoute<StazorRouteTransformer>("{**url}");
        }
    }
}
