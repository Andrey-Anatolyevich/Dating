using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Basics;
using DatingStorage.Models.Auth;
using Npgsql;
using System.Collections.Generic;

namespace DatingStorage.Storages
{
    public class ObjectTypesStorage : BaseStorage, IObjectTypesStorage
    {
        public ObjectTypesStorage(ConfigValuesCollection configValues, IMapper mapper)
            : base(configValues, mapper)
        {}

        public Maybe<IEnumerable<StorageObjectType>> GetAll()
        {
            var result = _storageHelper.GetQueryMaybe(() =>
            {
                var table = _pgClinet.ExecuteQueryOnFunc("core.object_type__get_all");
                var objectTypesReadModels = _pgDataReader.ReadMany<GetObjectTypeModel>(table);
                var localResult = _mapper.Map<IEnumerable<StorageObjectType>>(objectTypesReadModels);
                return localResult;
            });
            return result;
        }

        public Result Update(StorageObjectType storageObjectType)
        {
            return _storageHelper.GetResult(() => {
                var parameters = new List<NpgsqlParameter>();
                _pgHelper.AddParam(parameters, "p_id", NpgsqlTypes.NpgsqlDbType.Integer, storageObjectType.Id);
                _pgHelper.AddParam(parameters, "p_code", NpgsqlTypes.NpgsqlDbType.Varchar, storageObjectType.Code);
                _pgClinet.ExecuteNonQuery("core.object_type__update", parameters);
            });
        }

        public Result<int> Create(string code)
        {
            return _storageHelper.GetQueryResult(() => {
                var parameters = new List<NpgsqlParameter>();
                _pgHelper.AddParam(parameters, "p_code", NpgsqlTypes.NpgsqlDbType.Varchar, code);
                var resultInner = _pgClinet.ExecuteScalarFunc<int>("core.object_type__create", parameters);
                return resultInner;
            });
        }
    }
}
