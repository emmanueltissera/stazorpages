using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
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
        /// <summary>
        /// Adds services needed to generate static HTML pages.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="env">The <see cref="IWebHostEnvironment"/>.</param>
        /// <returns>An <see cref="IServiceCollection"/>.</returns>
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

        /// <summary>
        /// Enables the generation of static HTML pages on the first request.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The <see cref="IWebHostEnvironment"/>.</param>
        /// <param name="useMiddleWare">Enables the generation of static HTML pages. Can be switched off for debugging purposes.</param>
        /// <returns>A reference to the <paramref name="app"/> after the operation has completed.</returns>
        [Obsolete("Use either UseStazorPages or UseStazorPagesInDevelopment methods by checking environment on the startup file")]
        public static IApplicationBuilder UseStazorPages(this IApplicationBuilder app, IWebHostEnvironment env, bool useMiddleWare = true)
        {
            UseRewriter(app);

            UseStaticFiles(app, env);

            if (useMiddleWare)
            {
                app.UseMiddleware<StazorMiddleware>();
            }

            return app;
        }


        /// <summary>
        /// Enables the generation of static HTML pages on the first request.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The <see cref="IWebHostEnvironment"/>.</param>
        /// <param name="useMiddleWare">Enables the generation of static HTML pages. Can be switched off for debugging purposes.</param>
        /// <returns>A reference to the <paramref name="app"/> after the operation has completed.</returns>
        /// <remarks>
        /// This should only be enabled in the Development environment as it deletes static pages at every startup.
        /// </remarks>
        public static IApplicationBuilder UseStazorPagesInDevelopment(this IApplicationBuilder app, IWebHostEnvironment env,  bool useMiddleWare = true)
        {
            UseRewriter(app);

            UseStaticFiles(app, env);

            if (useMiddleWare)
            {
                app.UseMiddleware<StazorMiddleware>();
            }
            
            // re-add this here to show detailed messages - TODO: Bubble up errors to middleware
            app.UseDeveloperExceptionPage();

            StazorFileManagementService.CleanStazorPageDirectory();
            
            return app;
        }

        private static void UseStaticFiles(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, DefaultFilePaths.StazorPageDirectory))
            });
        }

        private static void UseRewriter(IApplicationBuilder applicationBuilder)
        {
            var rewriteOptions = new RewriteOptions()
                .Add(RewriteRules.RewriteHtmlFileRequests);

            applicationBuilder.UseRewriter(rewriteOptions);
        }


        public static void MapStazorPages(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapDynamicControllerRoute<StazorRouteTransformer>("{**url}");
        }
    }
}
