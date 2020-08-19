using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DatingCode.Mvc.Middleware
{
    public class NotFoundRedirectorMiddleware
    {
        public NotFoundRedirectorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public Task Invoke(HttpContext httpContext)
        {
            var task = _next(httpContext);
            task.GetAwaiter().GetResult();

            if(httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                httpContext.Request.Path = "/Home/PageNotFound";
                return _next(httpContext);
            }
            return task;
        }
    }
}
