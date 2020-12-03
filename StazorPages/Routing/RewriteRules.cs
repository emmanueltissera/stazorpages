using System;
using Microsoft.AspNetCore.Rewrite;
using StazorPages.Constants;

namespace StazorPages.Routing
{
    public class RewriteRules
    {
        public static void RewriteHtmlFileRequests(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            if (request.RouteValues.ContainsKey(RouteParameters.FilePath))
            {
                return;
            }

            var requestPath = request.Path.HasValue ? request.Path.Value:  string.Empty;
            requestPath = requestPath?.Trim('/');

            if (string.IsNullOrEmpty(requestPath))
            {
                context.Result = RuleResult.SkipRemainingRules;
                request.Path = "/index.html";
                return;
            }

            if (requestPath.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            context.Result = RuleResult.SkipRemainingRules;
            request.Path = $"/{requestPath}.html";
        }
    }
}