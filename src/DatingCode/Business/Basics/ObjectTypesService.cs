using AutoMapper;
using DatingCode.BusinessModels.Basics;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Basics;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Basics
{
    public class ObjectTypesService : IObjectTypesService
    {
        public ObjectTypesService(IMapper mapper, IObjectTypesStorage objectTypesStorage)
        {
            _cache = new ObjectTypesServiceCache();
            _mapper = mapper;
            _objectTypesStorage = objectTypesStorage;
        }

        private ObjectTypesServiceCache _cache;
        private IMapper _mapper;
        private IObjectTypesStorage _objectTypesStorage;

        public IEnumerable<ObjectType> GetAll()
        {
            Maybe<IEnumerable<ObjectType>> objectTypesCache = _cache.GetAll();
            if (objectTypesCache.Success)
                return objectTypesCache.Value;

            var storageResult = _objectTypesStorage.GetAll();
            if (!storageResult.Success)
                return Enumerable.Empty<ObjectType>();

            var allStorageObjectTypes = storageResult.Value;
            var allObjectTypes = _mapper.Map<IEnumerable<ObjectType>>(allStorageObjectTypes);
            _cache.SetAll(allObjectTypes);
            return allObjectTypes;
        }

        public Maybe<ObjectType> Get(int id)
        {
            var cacheResult = _cache.Get(id: id);
            if (cacheResult.Success)
                return cacheResult;

            var allObjectTypes = GetAll();
            var objectTypesById = allObjectTypes.Where(x => x.Id == id).ToArray();
            if (!objectTypesById.Any())
                return Maybe<ObjectType>.NewFailure($"Can't find {nameof(ObjectType)} by {nameof(id)} '{id}'.");

            var resultObjectType = objectTypesById.First();
            _cache.Set(objectType: resultObjectType);
            return Maybe<ObjectType>.NewSuccess(resultObjectType);
        }

        public Maybe<ObjectType> Get(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Maybe<ObjectType>.NewFailure($"'{code}' is NULL / Empty / Whitespace.");

            var cacheResult = _cache.Get(code: code);
            if (cacheResult.Success)
                return cacheResult;

            var allObjectTypes = GetAll();
            var objectTypesById = allObjectTypes.Where(x => x.Code == code).ToArray();
            if (!objectTypesById.Any())
                return Maybe<ObjectType>.NewFailure($"Can't find {nameof(ObjectType)} by {nameof(code)} '{code}'.");

            var resultObjectType = objectTypesById.First();
            _cache.Set(objectType: resultObjectType);
            return Maybe<ObjectType>.NewSuccess(resultObjectType);
        }

        public Result Update(ObjectType objectType)
        {
            if (string.IsNullOrWhiteSpace(objectType.Code))
                return Result.NewFailure($"'{nameof(objectType.Code)}' has no value.");

            var storageObjectType = _mapper.Map<StorageObjectType>(objectType);
            var updateResult = _objectTypesStorage.Update(storageObjectType);
            _cache.Invalidate(objectType);
            return updateResult;
        }

        public Result<int> Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result<int>.NewFailure($"'{nameof(code)}' has no value.");

            var updateResult = _objectTypesStorage.Create(code);
            _cache.InvalidateAll();
            return updateResult;
        }
    }
}
