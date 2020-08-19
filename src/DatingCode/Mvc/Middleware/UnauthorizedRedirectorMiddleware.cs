using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DatingCode.Mvc.Middleware
{
    public class UnauthorizedRedirectorMiddleware
    {
        public UnauthorizedRedirectorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public Task Invoke(HttpContext httpContext)
        {
            var task = _next(httpContext);
            task.GetAwaiter().GetResult();

            if(httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                httpContext.Request.Path = "/SignIn";
                return _next(httpContext);
            }
            return task;
        }
    }
}
