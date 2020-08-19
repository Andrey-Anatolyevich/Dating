using DatingCode.Core;
using DatingCode.BusinessModels.Auth;
using DatingCode.Security.Crypto;
using System;

namespace DatingCode.Business.Users
{
    public class AuthService
    {
        public AuthService(IUserInfoService userInfoProvider, Sha256SaltedHasher saltedHasher)
        {
            if (userInfoProvider == null)
                throw new ArgumentNullException(nameof(userInfoProvider));
            if (saltedHasher == null)
                throw new ArgumentNullException(nameof(saltedHasher));


            _userInfoService = userInfoProvider;
            _saltedHasher = saltedHasher;
        }

        private IUserInfoService _userInfoService;
        private Sha256SaltedHasher _saltedHasher;


        public Maybe<UserInfo> UserLoginRequest(string login, string pass)
        {
            var maybeUser = GetUser(login, pass);
            if (maybeUser.Success)
                _userInfoService.SetLastLoginDate(userId: maybeUser.Value.Id, lastLoginDate: DateTime.UtcNow);

            return maybeUser;
        }

        private Maybe<UserInfo> GetUser(string login, string pass)
        {
            if (string.IsNullOrWhiteSpace(login))
                Maybe<UserInfo>.NewFailure($"{nameof(login)} is NULL / Empty / Whitespace.");
            if (string.IsNullOrWhiteSpace(pass))
                Maybe<UserInfo>.NewFailure($"{nameof(pass)} is NULL / Empty / Whitespace.");


            var maybeUser = _userInfoService.GetUserInfo(login: login);
            if (maybeUser.Success)
            {
                var user = maybeUser.Value;

                var currentHash = _saltedHasher.GetHashBytesString(pass, user.PassSalt);
                var userIsTheSame = currentHash.Equals(user.PassHash, StringComparison.OrdinalIgnoreCase);
                if (userIsTheSame)
                    return maybeUser;
            }

            return Maybe<UserInfo>.NewFailure("Can't find user by provided credentials.");
        }
    }
}
