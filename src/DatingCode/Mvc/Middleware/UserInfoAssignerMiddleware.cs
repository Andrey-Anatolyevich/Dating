using DatingCode.Business.Users;
using DatingCode.BusinessModels.Users;
using DatingCode.Config;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DatingCode.Mvc.Middleware
{
    public class UserInfoAssignerMiddleware
    {
        public UserInfoAssignerMiddleware(RequestDelegate next, Consts consts)
        {
            _next = next;
            _userIdSessionkey = consts.SessionUserIdKey;
        }

        private string _userIdSessionkey;
        private readonly RequestDelegate _next;

        public Task Invoke(HttpContext httpContext, IUserInfoService userService)
        {
            var context = httpContext as DefaultHttpContext;

            var userIdString = context.Session.GetString(_userIdSessionkey);
            if (!string.IsNullOrWhiteSpace(userIdString))
            {
                var userId = long.Parse(userIdString);
                var maybeUser = userService.GetUserInfo(userId);
                if (maybeUser.Success)
                {
                    var userContainer = new UserContainer(maybeUser.Value);
                    httpContext.User = userContainer;
                }
            }

            return _next(httpContext);
        }
    }
}
