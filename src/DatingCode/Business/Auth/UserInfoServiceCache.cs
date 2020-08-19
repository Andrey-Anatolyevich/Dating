using DatingCode.BusinessModels.Auth;
using DatingCode.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Users
{
    public class UserInfoServiceCache
    {
        public UserInfoServiceCache()
        {
            _singleLock = new object();
            _byId = new ConcurrentDictionary<long, UserInfo>();
            _byLoginLower = new ConcurrentDictionary<string, UserInfo>();

            _all = new List<UserInfo>();
            _allLock = new object();
        }

        private readonly object _singleLock;
        private ConcurrentDictionary<long, UserInfo> _byId;
        private ConcurrentDictionary<string, UserInfo> _byLoginLower;

        private List<UserInfo> _all;
        private bool _allSet;
        private object _allLock;

        internal Maybe<UserInfo> Get(long userId)
        {
            if (_byId.TryGetValue(userId, out var userInfo))
                return Maybe<UserInfo>.NewSuccess(userInfo);

            return Maybe<UserInfo>.NewFailure($"Couldn't find User by {nameof(userId)}: '{userId}'.");
        }

        internal Maybe<UserInfo> Get(string login)
        {
            if (_byLoginLower.TryGetValue(login?.ToLower(), out var userInfo))
                return Maybe<UserInfo>.NewSuccess(userInfo);

            return Maybe<UserInfo>.NewFailure($"Couldn't find User by {nameof(login)}: '{login}'.");
        }

        internal Result Set(UserInfo user)
        {
            if (user == null)
                return Result.NewFailure($"Parameter {nameof(user)} is NULL.");

            lock (_singleLock)
            {
                _byId.AddOrUpdate(user.Id, user, (id, usr) => user);
                _byLoginLower.AddOrUpdate(user.Login.ToLower(), user, (id, usr) => user);
            }
            lock (_allLock)
            {
                _all.RemoveAll(x => x.Id == user.Id);
                _all.Add(user);
            }
            return Result.NewSuccess();
        }

        internal void Invalidate(long userId)
        {
            lock (_singleLock)
            {
                if (_byId.TryRemove(userId, out var user))
                    _byLoginLower.TryRemove(user.Login.ToLower(), out var userByLogin);
            }

            lock (_allLock)
            {
                _all.RemoveAll(x => x.Id == userId);
                _allSet = false;
            }
        }

        internal Maybe<IEnumerable<UserInfo>> GetAll()
        {
            lock (_allLock)
            {
                if (_allSet)
                    return Maybe<IEnumerable<UserInfo>>.NewSuccess(_all);

                return Maybe<IEnumerable<UserInfo>>.NewFailure("All is not set.");
            }
        }

        internal void SetAll(IEnumerable<UserInfo> allUsers)
        {
            lock (_allLock)
            {
                _all.Clear();
                _all.AddRange(allUsers);
                _allSet = true;
            }
        }

        internal IEnumerable<UserInfo> GetAllAvailable()
        {
            lock (_allLock)
            {
                return _all.AsEnumerable();
            }
        }
    }
}
