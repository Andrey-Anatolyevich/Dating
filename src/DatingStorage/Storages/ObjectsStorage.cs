using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Basics;
using DatingStorage.Clients;
using DatingStorage.Models.Auth;
using DatingStorage.Orm;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;

namespace DatingStorage.Storages
{
    public class ObjectsStorage : BaseStorage, IObjectsStorage
    {
        public ObjectsStorage(ConfigValuesCollection configValues, IMapper mapper)
            : base(configValues, mapper)
        { }

        public Result<IEnumerable<StorageObjectItem>> AllOfType(int typeId)
        {
            var result = _storageHelper.GetQueryResult(() =>
            {
                var parameters = new List<NpgsqlParameter>();
                _pgHelper.AddParam(parameters, "p_object_type_id", NpgsqlDbType.Integer, typeId);
                var table = _pgClinet.ExecuteQueryOnFunc("core.object__get_all_of_type", parameters);
                var objectTypesReadModels = _pgDataReader.ReadMany<GetObjectModel>(table);
                var localResult = _mapper.Map<IEnumerable<StorageObjectItem>>(objectTypesReadModels);
                return localResult;
            });
            return result;
        }

        public Result Create(StorageObjectCreateInfo storageCreateInfo)
        {
            if (storageCreateInfo == null)
                return Result.NewFailure($"{nameof(storageCreateInfo)} is NULL.");


            var result = _storageHelper.GetResult(() =>
            {
                _pgClinet.NewCommand()
                    .OnFunc("core.object__create")
                    .WithParam("p_parent_object_id", NpgsqlDbType.Integer, storageCreateInfo.ParentId)
                    .WithParam("p_object_type_id", NpgsqlDbType.Integer, storageCreateInfo.TypeId)
                    .WithParam("p_object_code", NpgsqlDbType.Varchar, storageCreateInfo.Code)
                    .WithParam("p_is_enabled", NpgsqlDbType.Boolean, true)
                    .QueryVoid();
            });
            return result;
        }

        public Result Delete(int objectId)
        {
            var result = _storageHelper.GetResult(() =>
            {
                _pgClinet.NewCommand()
                    .OnFunc("core.object__delete")
                    .WithParam("p_object_id", NpgsqlDbType.Integer, objectId)
                    .QueryVoid();
            });
            return result;
        }

        public Maybe<StorageObjectItem> GetById(int objectId)
        {
            var maybeObject = _pgClinet.NewCommand()
                    .OnFunc("core.object__get_by_id")
                    .WithParam("p_object_id", NpgsqlDbType.Integer, objectId)
                    .QueryMaybeSingle<GetObjectModel>();
            var result = MapMaybe<GetObjectModel, StorageObjectItem>(maybeObject);
            return result;
        }

        public Maybe<StorageObjectItem> GetByTypeAndCode(int typeId, string code)
        {
            var maybeObject = _pgClinet.NewCommand()
                    .OnFunc("core.object__get_by_type_and_code")
                    .WithParam("p_type_id", NpgsqlDbType.Integer, typeId)
                    .WithParam("p_code", NpgsqlDbType.Varchar, code)
                    .QueryMaybeSingle<GetObjectModel>();
            var result = MapMaybe<GetObjectModel, StorageObjectItem>(maybeObject);
            return result;
        }

        public Result SetCode(int id, string code)
        {
            var result = _storageHelper.GetResult(() =>
            {
                _pgClinet.NewCommand()
                    .OnFunc("core.object__set_code")
                    .WithParam("p_object_id", NpgsqlDbType.Integer, id)
                    .WithParam("p_code", NpgsqlDbType.Varchar, code)
                    .QueryVoid();
            });
            return result;
        }
    }
}
