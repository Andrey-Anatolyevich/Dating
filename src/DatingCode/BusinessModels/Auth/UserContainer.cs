using DatingCode.BusinessModels.Auth;
using System.Security.Claims;

namespace DatingCode.BusinessModels.Users
{
    public class UserContainer : ClaimsPrincipal
    {
        public UserContainer(UserInfo cachedUserInfo)
        {
            MyUser = cachedUserInfo;
        }

        public UserInfo MyUser;
    }
}
