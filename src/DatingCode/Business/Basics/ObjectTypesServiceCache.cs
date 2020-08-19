using DatingCode.BusinessModels.Basics;
using DatingCode.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DatingCode.Business.Basics
{
    internal class ObjectTypesServiceCache
    {
        public ObjectTypesServiceCache()
        {
            _all = new List<ObjectType>();
            _allLock = new object();
            _byId = new ConcurrentDictionary<int, ObjectType>();
            _byCode = new ConcurrentDictionary<string, ObjectType>();
        }

        private bool _allSet;
        private List<ObjectType> _all;
        private object _allLock;
        private ConcurrentDictionary<int, ObjectType> _byId;
        private ConcurrentDictionary<string, ObjectType> _byCode;

        internal Maybe<IEnumerable<ObjectType>> GetAll()
        {
            lock (_allLock)
            {
                if (_allSet)
                    return Maybe<IEnumerable<ObjectType>>.NewSuccess(_all);

                return Maybe<IEnumerable<ObjectType>>.NewFailure("All are not set.");
            }
        }

        internal void SetAll(IEnumerable<ObjectType> allObjectTypes)
        {
            lock (_allLock)
            {
                _all.Clear();
                _all.AddRange(allObjectTypes);
                _allSet = true;
            }
        }

        internal void InvalidateAll()
        {
            lock (_allLock)
            {
                _all.Clear();
                _allSet = false;
            }
        }

        internal Maybe<ObjectType> Get(int id)
        {
            if (_byId.TryGetValue(id, out var value))
                return Maybe<ObjectType>.NewSuccess(value);

            return Maybe<ObjectType>.NewFailure($"Can't find by ID: '{id}'.");
        }

        internal Maybe<ObjectType> Get(string code)
        {
            if (_byCode.TryGetValue(code, out var value))
                return Maybe<ObjectType>.NewSuccess(value);

            return Maybe<ObjectType>.NewFailure($"Can't find by code: '{code}'.");
        }

        internal void Set(ObjectType objectType)
        {
            _byId.AddOrUpdate(objectType.Id, objectType, (id, val) => objectType);
            _byCode.AddOrUpdate(objectType.Code, objectType, (id, val) => objectType);
        }

        internal void Invalidate(ObjectType objectType)
        {
            _byId.TryRemove(objectType.Id, out _);
            _byCode.TryRemove(objectType.Code, out _);
            _allSet = false;
        }
    }
}
