using AutoMapper;
using DatingCode.Business.Core;
using DatingCode.BusinessModels.Auth;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Users
{
    public class UserInfoService : IUserInfoService
    {
        public UserInfoService(IUserInfoStorage userInfoStorage,
            IMapper mapper, ILocalizationService localeService)
        {
            _cache = new UserInfoServiceCache();
            _userInfoStorage = userInfoStorage;
            _mapper = mapper;
            _localeService = localeService;

            _getAllLock = new object();
        }

        private UserInfoServiceCache _cache;
        private IUserInfoStorage _userInfoStorage;
        private IMapper _mapper;
        private ILocalizationService _localeService;

        private object _getAllLock;

        public Maybe<UserInfo> GetUserInfo(long userId)
        {
            var maybeUser = _cache.Get(userId: userId);
            if (maybeUser.Success)
                return Maybe<UserInfo>.NewSuccess(maybeUser.Value);

            var maybeStorageUser = _userInfoStorage.GetUserInfoById(userId: userId);
            if (!maybeStorageUser.Success)
                return Maybe<UserInfo>.NewFailure($"Couldn't find user by ID: {userId}.");

            var storageUser = maybeStorageUser.Value;
            var user = GetUserFromStorageUser(storageUser);
            _cache.Set(user);
            return Maybe<UserInfo>.NewSuccess(user);
        }

        public Maybe<UserInfo> GetUserInfo(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return Maybe<UserInfo>.NewFailure("Login is not provided.");

            var maybeUser = _cache.Get(login: login);
            if (maybeUser.Success)
                return Maybe<UserInfo>.NewSuccess(maybeUser.Value);

            var getUserInfoResult = _userInfoStorage.GetUserInfoByLogin(login: login);
            if (!getUserInfoResult.Success)
                return Maybe<UserInfo>.NewFailure($"Couldn't find user by login: {login}.");

            var storageUserInfo = getUserInfoResult.Value;
            var userInfo = GetUserFromStorageUser(storageUserInfo);
            _cache.Set(userInfo);
            return Maybe<UserInfo>.NewSuccess(userInfo);
        }

        public void SetLastLoginDate(long userId, DateTime lastLoginDate)
        {
            _userInfoStorage.SetLastLoginDate(userId: userId, lastLoginDate: lastLoginDate);
            _cache.Invalidate(userId: userId);
        }

        public IEnumerable<UserInfo> GetAll()
        {
            var maybeAllUsersCached = _cache.GetAll();
            if (maybeAllUsersCached.Success)
                return maybeAllUsersCached.Value;

            lock (_getAllLock)
            {
                var maybeAllStorageIds = _userInfoStorage.GetAllIds();
                if (!maybeAllStorageIds.Success)
                    throw new Exception(maybeAllStorageIds.ErrorMessage);

                var allCachedUsers = _cache.GetAllAvailable();
                var missingUsersIds = maybeAllStorageIds.Value
                    .Where(id => allCachedUsers.Any(x => x.Id == id) == false)
                    .ToList();

                var allUsers = new List<UserInfo>(allCachedUsers);
                foreach (var missingUserId in missingUsersIds)
                {
                    var maybeUser = GetUserInfo(userId: missingUserId);
                    if (maybeUser.Success)
                    {
                        allUsers.Add(maybeUser.Value);
                        _cache.Set(maybeUser.Value);
                    }
                }
                _cache.SetAll(allUsers);

                return allUsers;
            }
        }

        private UserInfo GetUserFromStorageUser(StorageUserInfo storageUser)
        {
            var user = _mapper.Map<UserInfo>(storageUser);
            var maybeLocale = _localeService.GetLocale(storageUser.LocaleId);
            if (maybeLocale.Success)
                user.Locale = maybeLocale.Value;

            return user;
        }
    }
}
