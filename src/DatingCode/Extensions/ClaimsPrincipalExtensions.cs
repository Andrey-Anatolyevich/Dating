using DatingCode.BusinessModels.Auth;
using DatingCode.BusinessModels.Users;
using DatingCode.Core;
using System.Security.Claims;

namespace DatingCode.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Maybe<UserInfo> GetMyUser(this ClaimsPrincipal claimsPrincipal)
        {
            var userContainer = claimsPrincipal as UserContainer;
            var result = userContainer?.MyUser;
            return Maybe<UserInfo>.NewFromValue(result, "User is not set.");
        }
    }
}
