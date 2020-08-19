using DatingCode.Infrastructure.Config.Consts;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DatingCode.Mvc.Middleware
{
    public class SessionPinnerMiddleware
    {
        public SessionPinnerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public Task Invoke(HttpContext httpContext, ConfigSession configSession)
        {
            var context = httpContext as DefaultHttpContext;
            if(context != null
                && context.Session != null)
            {
                context.Session.SetString(configSession.SessionPinKey, configSession.SessionPinValue);
            }

            return _next(httpContext);
        }
    }
}
