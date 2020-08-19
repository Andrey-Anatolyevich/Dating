using AutoMapper;
using DatingCode.BusinessModels.Basics;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Basics;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Basics
{
    public class ObjectsService : IObjectsService
    {
        public ObjectsService(IMapper mapper, IObjectsStorage objectTypesStorage)
        {
            _cache = new ObjectsServiceCache();
            _mapper = mapper;
            _objectsStorage = objectTypesStorage;
        }

        private ObjectsServiceCache _cache;
        private IMapper _mapper;
        private IObjectsStorage _objectsStorage;

        public IEnumerable<ObjectItem> AllOfType(int typeId)
        {
            Result<IEnumerable<ObjectItem>> resultAllOfTypeCached = _cache.GetAllOfType(typeId: typeId);
            if (resultAllOfTypeCached.Success)
            {
                return resultAllOfTypeCached.Value;
            }
            else
            {
                var storageResult = _objectsStorage.AllOfType(typeId);
                if (storageResult.Success)
                {
                    var storageObjects = storageResult.Value;
                    var objects = _mapper.Map<IEnumerable<ObjectItem>>(storageObjects);
                    _cache.SetAllOfType(typeId: typeId, objects: objects);
                    return objects;
                }
            }

            return Enumerable.Empty<ObjectItem>();
        }

        public IEnumerable<ObjectItem> ActiveOfType(int typeId)
        {
            var all = AllOfType(typeId);
            var result = all.Where(x => x.IsEnabled).ToArray();
            return result;
        }

        public Result Create(ObjectCreateInfo objCreate)
        {
            var storageCreateInfo = _mapper.Map<StorageObjectCreateInfo>(objCreate);
            var result = _objectsStorage.Create(storageCreateInfo);
            if (result.Success)
                _cache.InvalidateAllOfType(typeId: objCreate.TypeId);

            return result;
        }

        public Result Delete(int objectId)
        {
            var result = _objectsStorage.Delete(objectId);
            if (result.Success)
                _cache.InvalidateObject(objectId: objectId);

            return result;
        }

        public Result SetCode(int id, string code)
        {
            var result = _objectsStorage.SetCode(id, code);
            if (result.Success)
                _cache.InvalidateObject(objectId: id);

            return result;
        }

        public Maybe<ObjectItem> GetById(int objectId)
        {
            var maybeItemCached = _cache.GetById(objectId: objectId);
            if (maybeItemCached.Success)
                return maybeItemCached;

            var maybeStorageObject = _objectsStorage.GetById(objectId);
            if (!maybeStorageObject.Success)
                return Maybe<ObjectItem>.NewFailure(maybeStorageObject.ErrorMessage);

            var objectItem = _mapper.Map<ObjectItem>(maybeStorageObject.Value);
            _cache.SetItem(objectItem);
            return Maybe<ObjectItem>.NewSuccess(objectItem);
        }

        public Maybe<ObjectItem> GetByTypeIdAndCode(int typeId, string code)
        {
            var maybeItemCached = _cache.GetByTypeIdAndCode(typeId: typeId, code: code);
            if (maybeItemCached.Success)
                return maybeItemCached;

            var maybeStorageObject = _objectsStorage.GetByTypeAndCode(typeId: typeId, code: code);
            if (!maybeStorageObject.Success)
            {
                return Maybe<ObjectItem>.NewFailure($"Can't find Object by {nameof(typeId)}: '{typeId}' and {nameof(code)}: '{code}'.");
            }

            var objectItem = _mapper.Map<ObjectItem>(maybeStorageObject.Value);
            _cache.SetItem(objectItem);
            return Maybe<ObjectItem>.NewSuccess(objectItem);
        }
    }
}
