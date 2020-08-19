using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Geo;
using DatingStorage.Models.Geo;
using Npgsql;
using System.Collections.Generic;

namespace DatingStorage.Storages
{
    public class PlacesStorage : BaseStorage, IPlacesStorage
    {
        public PlacesStorage(ConfigValuesCollection configValues, IMapper mapper)
            : base(configValues, mapper)
        { }

        public Maybe<List<StoragePlace>> GetAllPlaces()
        {
            var maybePlaces = _storageHelper.GetQueryMaybe(() =>
            {
                var table = _pgClinet.ExecuteQueryOnFunc("geo.place__get_all");
                var readObjects = _pgDataReader.ReadMany<GetPlacesModel>(table);
                var innerResult = _mapper.Map<List<StoragePlace>>(readObjects);
                return innerResult;
            });
            return maybePlaces;
        }

        public Maybe<StoragePlace> GetPlace(int placeId)
        {
            var maybePlaces = _storageHelper.GetQueryMaybe(() =>
            {
                var parameters = new List<NpgsqlParameter>();
                _pgHelper.AddParam(parameters, "p_place_id", NpgsqlTypes.NpgsqlDbType.Integer, placeId);
                var table = _pgClinet.ExecuteQueryOnFunc("geo.place__get", parameters);
                var readObjects = _pgDataReader.ReadSingle<GetPlacesModel>(table);
                var innerResult = _mapper.Map<StoragePlace>(readObjects);
                return innerResult;
            });
            return maybePlaces;
        }

        public Result UpdatePlace(StoragePlace place)
        {
            var result = _storageHelper.GetResult(() =>
            {
                var parameters = new List<NpgsqlParameter>();
                _pgHelper.AddParam(parameters, "p_place_id", NpgsqlTypes.NpgsqlDbType.Integer, place.Id);
                _pgHelper.AddParam(parameters, "p_parent_place_id", NpgsqlTypes.NpgsqlDbType.Integer, place.ParentPlaceId);
                _pgHelper.AddParam(parameters, "p_place_code", NpgsqlTypes.NpgsqlDbType.Varchar, place.PlaceCode);
                _pgHelper.AddParam(parameters, "p_place_type_id", NpgsqlTypes.NpgsqlDbType.Integer, (int)place.PlaceType);
                _pgHelper.AddParam(parameters, "p_is_enabled", NpgsqlTypes.NpgsqlDbType.Boolean, place.IsEnabled);
                _pgClinet.ExecuteNonQuery("geo.place__update", parameters);
            });
            return result;
        }

        public Result<int> CreatePlace(StorageNewPlace newPlace)
        {
            var maybePlaceId = _storageHelper.GetQueryMaybe(() =>
            {
                var parameters = new List<NpgsqlParameter>();
                _pgHelper.AddParam(parameters, "p_parent_place_id", NpgsqlTypes.NpgsqlDbType.Integer, newPlace.ParentPlaceId);
                _pgHelper.AddParam(parameters, "p_place_code", NpgsqlTypes.NpgsqlDbType.Varchar, newPlace.PlaceCode);
                _pgHelper.AddParam(parameters, "p_place_type_id", NpgsqlTypes.NpgsqlDbType.Integer, (int)newPlace.PlaceType);
                _pgHelper.AddParam(parameters, "p_is_enabled", NpgsqlTypes.NpgsqlDbType.Boolean, newPlace.IsEnabled);
                var newId = _pgClinet.ExecuteScalarFunc<int>("geo.place__create", parameters);
                return newId;
            });
            return Result<int>.NewSuccess(maybePlaceId.Value);
        }
    }
}
