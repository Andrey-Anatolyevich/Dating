using DatingCode.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace DatingCode.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthAttribute : Attribute, IAuthorizationFilter
    {
        public AuthAttribute(string allowedClaims = "")
        {
            _commaSeparatedClaims = allowedClaims;
        }

        private readonly string _commaSeparatedClaims;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var maybeUser = context.HttpContext.User.GetMyUser();
            if (maybeUser.Success)
            {
                if (string.IsNullOrWhiteSpace(_commaSeparatedClaims))
                    return;
                else
                {
                    var allowedClaimsLower = _commaSeparatedClaims
                        .Split(',', options: StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim().ToLower())
                        .ToArray();

                    var userHasAnyClaim = maybeUser.Value.Claims.Any(x => allowedClaimsLower.Any(y => y == x.ToString().ToLower()));
                    if (userHasAnyClaim)
                        return;
                }
            }

            context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
