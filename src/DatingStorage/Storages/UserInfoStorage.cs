using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Auth;
using DatingStorage.Models.Auth;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatingStorage.Storages
{
    public class UserInfoStorage : BaseStorage, IUserInfoStorage
    {
        public UserInfoStorage(ConfigValuesCollection configValues, IMapper mapper)
            : base(configValues, mapper)
        { }

        public Maybe<StorageUserInfo> GetUserInfoById(long userId)
        {
            var result = _storageHelper.GetQueryMaybe(() =>
            {
                var mbUserInfo = GetBasicUserInfoById(userId);
                if (mbUserInfo.Success)
                    mbUserInfo.Value.Claims = GetUserClaimsByUserId(userId);
                return mbUserInfo.Value;
            });
            return result;
        }

        public Maybe<StorageUserInfo> GetUserInfoByLogin(string login)
        {
            var maybeUser = _storageHelper.GetQueryMaybe(() =>
            {
                var mbUserInfo = GetBasicUserInfoByLogin(login);
                if (mbUserInfo.Success)
                    mbUserInfo.Value.Claims = GetUserClaimsByUserId(mbUserInfo.Value.Id);
                return mbUserInfo.Value;
            });
            return maybeUser;
        }

        public Result SetLastLoginDate(long userId, DateTime lastLoginDate)
        {
            var result = _storageHelper.GetResult(() =>
             {
                 _pgClinet.NewCommand()
                     .OnFunc("auth.user__set_last_login_date")
                     .WithParam("p_user_id", NpgsqlTypes.NpgsqlDbType.Integer, userId)
                     .WithParam("p_date_last_login", NpgsqlTypes.NpgsqlDbType.Timestamp, lastLoginDate)
                     .QueryVoid();
             });
            return result;
        }

        private Maybe<StorageUserInfo> GetBasicUserInfoById(long userId)
        {
            var parameters = new List<NpgsqlParameter>();
            _pgHelper.AddParam(parameters, "p_user_id", NpgsqlTypes.NpgsqlDbType.Integer, userId);
            var table = _pgClinet.ExecuteQueryOnFunc("auth.user__get_by_id", parameters);
            var userOrNone = _pgDataReader.ReadSingleOrNone<GetUserBasicInfoModel>(table);
            if (userOrNone != null)
            {
                var resultObject = _mapper.Map<StorageUserInfo>(userOrNone);
                return Maybe<StorageUserInfo>.NewSuccess(resultObject);
            }
            return Maybe<StorageUserInfo>.NewFailure($"User is not found by ID: '{userId}'.");
        }

        private Maybe<StorageUserInfo> GetBasicUserInfoByLogin(string login)
        {
            var parameters = new List<NpgsqlParameter>();
            _pgHelper.AddParam(parameters, "p_user_login", NpgsqlTypes.NpgsqlDbType.Varchar, login);
            var table = _pgClinet.ExecuteQueryOnFunc("auth.user__get_by_login", parameters);
            var userOrNone = _pgDataReader.ReadSingleOrNone<GetUserBasicInfoModel>(table);
            if (userOrNone != null)
            {
                var resultObject = _mapper.Map<StorageUserInfo>(userOrNone);
                return Maybe<StorageUserInfo>.NewSuccess(resultObject);
            }
            return Maybe<StorageUserInfo>.NewFailure($"User is not found by login: '{login}'.");
        }

        private HashSet<StorageUserClaim> GetUserClaimsByUserId(long userId)
        {
            var parameters = new List<NpgsqlParameter>();
            _pgHelper.AddParam(parameters, "p_user_id", NpgsqlTypes.NpgsqlDbType.Integer, userId);
            var table = _pgClinet.ExecuteQueryOnFunc("auth.user__get_claims", parameters);
            var readObjects = _pgDataReader.ReadMany<GetUserClaimsModel>(table);
            var pgUserClaims = readObjects.Select(x => x.ClaimCode).ToArray();
            var storageUserClaims = _mapper.Map<StorageUserClaim[]>(pgUserClaims);
            var result = storageUserClaims.ToHashSet();
            return result;
        }

        public Maybe<List<long>> GetAllIds()
        {
            var maybeAllIds = _storageHelper.GetQueryMaybe(() =>
            {
                var table = _pgClinet.ExecuteQueryOnFunc("auth.user__get_all_ids");
                var readObjects = _pgDataReader.ReadMany<GetUserIdModel>(table);
                var usersIds = readObjects.Select(x => x.Id).ToList();
                return usersIds;
            });
            return maybeAllIds;
        }
    }
}
