using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StazorPages.Routing
{
    public class RouteDataModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var key = bindingContext.ModelName;
            if (bindingContext.ActionContext.RouteData.Values.TryGetValue(key, out var value))
            {
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            return Task.CompletedTask;
        }
    }
}
