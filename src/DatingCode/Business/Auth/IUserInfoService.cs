using System;
using System.Collections.Generic;
using DatingCode.BusinessModels.Auth;
using DatingCode.Core;

namespace DatingCode.Business.Users
{
    public interface IUserInfoService
    {
        Maybe<UserInfo> GetUserInfo(long userId);
        Maybe<UserInfo> GetUserInfo(string login);
        IEnumerable<UserInfo> GetAll();
        void SetLastLoginDate(long userId, DateTime lastLoginDate);
    }
}
