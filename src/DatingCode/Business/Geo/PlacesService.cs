using AutoMapper;
using DatingCode.BusinessModels.Geo;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Geo;
using System;
using System.Collections.Generic;

namespace DatingCode.Business.Geo
{
    public class PlacesService : IPlacesService
    {
        public PlacesService(IMapper mapper, IPlacesStorage placesStorage)
        {
            _cache = new PlacesServiceCache();
            _mapper = mapper;
            _placesStorage = placesStorage;
        }

        private PlacesServiceCache _cache;
        private IMapper _mapper;
        private IPlacesStorage _placesStorage;

        public Maybe<IEnumerable<PlaceInfo>> GetAllPlaces()
        {
            var cachedAll = _cache.GetAll();
            if (cachedAll.Success)
                return cachedAll;

            var maybeStoragePlaces = _placesStorage.GetAllPlaces();
            if (!maybeStoragePlaces.Success)
                return Maybe<IEnumerable<PlaceInfo>>.NewFailure("Places storage returned no places.");

            var storageAllPlaces = maybeStoragePlaces.Value;
            var allPlaces = _mapper.Map<IEnumerable<PlaceInfo>>(storageAllPlaces);
            _cache.SetAll(allPlaces);
            return Maybe<IEnumerable<PlaceInfo>>.NewSuccess(allPlaces);
        }

        public Maybe<PlaceInfo> GetPlace(int placeId)
        {
            var cachedPlace = _cache.Get(placeId: placeId);
            if (cachedPlace.Success)
                return cachedPlace;

            var maybeStoragePlace = _placesStorage.GetPlace(placeId);
            if (!maybeStoragePlace.Success)
                return Maybe<PlaceInfo>.NewFailure(maybeStoragePlace.ErrorMessage);

            var place = _mapper.Map<PlaceInfo>(maybeStoragePlace.Value);
            _cache.Set(place);
            return Maybe<PlaceInfo>.NewSuccess(place);
        }

        public Result UpdatePlace(PlaceInfo place)
        {
            if (place == null)
                throw new ArgumentNullException(nameof(place));


            var mappedPlace = _mapper.Map<StoragePlace>(place);
            var updateResult = _placesStorage.UpdatePlace(mappedPlace);
            if (updateResult.Success)
                _cache.Set(place);

            return updateResult;
        }

        public Result<PlaceInfo> CreatePlace(NewPlace newPlace)
        {
            if (newPlace == null)
                return Result<PlaceInfo>.NewFailure($"{nameof(newPlace)} is NULL.");
            if (newPlace.PlaceType == PlaceType.Unknown)
                return Result<PlaceInfo>.NewFailure($"{nameof(newPlace.PlaceType)} == {PlaceType.Unknown}.");
            if (string.IsNullOrWhiteSpace(newPlace.PlaceCode))
                return Result<PlaceInfo>.NewFailure($"{nameof(newPlace.PlaceCode)} is Null/Empty/Whitespace.");


            var storagePlace = _mapper.Map<StorageNewPlace>(newPlace);
            var resultNewPlaceId = _placesStorage.CreatePlace(storagePlace);
            if (resultNewPlaceId.Success)
            {
                _cache.InvalidateAll();
                var maybeNewPlace = GetPlace(resultNewPlaceId.Value);
                return maybeNewPlace.ToResult();
            }
            else
                return Result<PlaceInfo>.NewFailure(resultNewPlaceId.ErrorMessage);
        }
    }
}
