using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Home.Director.Controller.Filters
{
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private readonly Guid _apiKey;
        public ApiKeyAuthAttribute(IConfiguration configuration)
        {
            _apiKey = configuration.GetValue<Guid>("AllowedSecret");
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
