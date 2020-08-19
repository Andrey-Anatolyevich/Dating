using DatingCode.BusinessModels.Basics;
using DatingCode.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DatingCode.Business.Basics
{
    internal class ObjectsServiceCache
    {
        public ObjectsServiceCache()
        {
            _allByType = new ConcurrentDictionary<int, IEnumerable<ObjectItem>>();
            _byId = new ConcurrentDictionary<int, ObjectItem>();
            _byTypeIdAndCode = new ConcurrentDictionary<Tuple<int, string>, ObjectItem>();
            _allObjectsLock = new object();
        }

        private ConcurrentDictionary<int, IEnumerable<ObjectItem>> _allByType;
        private ConcurrentDictionary<int, ObjectItem> _byId;
        private ConcurrentDictionary<Tuple<int, string>, ObjectItem> _byTypeIdAndCode;
        private object _allObjectsLock;

        internal Result<IEnumerable<ObjectItem>> GetAllOfType(int typeId)
        {
            if (_allByType.TryGetValue(typeId, out var foundObjs))
                return Result<IEnumerable<ObjectItem>>.NewSuccess(foundObjs);

            return Result<IEnumerable<ObjectItem>>.NewFailure($"No all objects of typeId: '{typeId}' are found.");
        }

        internal void SetAllOfType(int typeId, IEnumerable<ObjectItem> objects)
        {
            _allByType.AddOrUpdate(typeId, objects, (tId, objs) => { return objects; });
        }

        internal void InvalidateAllOfType(int typeId)
        {
            _allByType.TryRemove(typeId, out _);
        }

        internal void InvalidateObject(int objectId)
        {
            lock (_allObjectsLock)
            {
                if (_byId.TryRemove(objectId, out var obj))
                {
                    var typeId = obj.ObjectTypeId;
                    InvalidateAllOfType(typeId);
                    InvalidateByTypeIdAndCode(typeId, obj.ObjectCode);
                }
            }
        }

        internal Maybe<ObjectItem> GetById(int objectId)
        {
            if (_byId.TryGetValue(objectId, out var obj))
                return Maybe<ObjectItem>.NewSuccess(obj);

            return Maybe<ObjectItem>.NewFailure($"Can't find by {nameof(objectId)}: '{objectId}'.");
        }

        internal Maybe<ObjectItem> GetByTypeIdAndCode(int typeId, string code)
        {
            var tuple = new Tuple<int, string>(typeId, code);
            if (_byTypeIdAndCode.TryGetValue(tuple, out var item))
                return Maybe<ObjectItem>.NewSuccess(item);

            return Maybe<ObjectItem>.NewFailure($"Can't find by {nameof(typeId)}: '{typeId}' and {nameof(code)}: '{code}'.");
        }

        private void InvalidateByTypeIdAndCode(int typeId, string code)
        {
            var tuple = new Tuple<int, string>(typeId, code);
            _byTypeIdAndCode.TryRemove(tuple, out _);
        }

        internal void SetItem(ObjectItem objectItem)
        {
            var tuple = new Tuple<int, string>(objectItem.ObjectTypeId, objectItem.ObjectCode);
            lock (_allObjectsLock)
            {
                _byId.AddOrUpdate(objectItem.Id, objectItem, (id, obj) => objectItem);
                _byTypeIdAndCode.AddOrUpdate(tuple, objectItem, (tpl, obj) => objectItem);
            }
        }
    }
}
