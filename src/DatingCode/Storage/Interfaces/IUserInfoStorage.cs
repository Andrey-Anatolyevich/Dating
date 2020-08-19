using System;
using System.Collections.Generic;
using DatingCode.Core;
using DatingCode.Storage.Models.Auth;

namespace DatingCode.Storage.Interfaces
{
    public interface IUserInfoStorage
    {
        Maybe<StorageUserInfo> GetUserInfoById(long userId);
        Maybe<StorageUserInfo> GetUserInfoByLogin(string login);
        Result SetLastLoginDate(long userId, DateTime lastLoginDate);
        Maybe<List<long>> GetAllIds();
    }
}
