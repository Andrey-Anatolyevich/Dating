using AutoMapper;
using DatingCode.Business.Basics;
using DatingCode.Business.Geo;
using DatingCode.Business.Media;
using DatingCode.BusinessModels.Geo;
using DatingCode.BusinessModels.Market;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Ads;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Dating
{
    public class AdsService : IAdsService
    {
        public AdsService(IMapper mapper, IAdsStorage adsStorage, IPlacesService placesService,
            IObjectTypesService objectTypesService, IObjectsService objectsService,
            IFilesService filesService)
        {
            _mapper = mapper;
            _adsStorage = adsStorage;
            _cache = new AdsServiceCache();
            _placesService = placesService;
            _objectsService = objectsService;
            _objectTypesService = objectTypesService;
            _filesService = filesService;

            _allAdsLock = new object();
        }

        private IMapper _mapper;
        private IAdsStorage _adsStorage;
        private AdsServiceCache _cache;
        private IPlacesService _placesService;
        private IObjectsService _objectsService;
        private IObjectTypesService _objectTypesService;
        private IFilesService _filesService;

        private object _allAdsLock;

        public Result EditAd(AdEditInfo adInfo)
        {
            // Should be split into Mapping and Validation as separate steps
            var resultStorageAdInfo = GetValidatedStorageAdEditInfo(adInfo);
            if (!resultStorageAdInfo.Success)
                return Result.NewFailure(resultStorageAdInfo.ErrorMessage);

            var resultStoragePics = GetValidatedStoragePics(adInfo);
            if (!resultStoragePics.Success)
                return Result.NewFailure(resultStoragePics.ErrorMessage);

            var storageAdInfo = resultStorageAdInfo.Value;
            long adId = 0;

            if (adInfo.AdId.HasValue)
            {
                var resultUpdateAd = _adsStorage.UpdateAd(storageAdInfo);
                if (!resultUpdateAd.Success)
                    return resultUpdateAd;

                adId = adInfo.AdId.Value;
            }
            else
            {
                var resultNewAdId = _adsStorage.CreateAd(storageAdInfo);
                if (!resultNewAdId.Success)
                    return Result.NewFailure(resultNewAdId.ErrorMessage);

                adId = resultNewAdId.Value;
            }

            // set mainPic in storage pics
            foreach (var sPic in resultStoragePics.Value)
                sPic.IsPrimary = adInfo.MainPicId == sPic.AdMediaId;

            var resultSetAdPics = _filesService.SetAdPics(adId: adId, pics: resultStoragePics.Value);
            if (!resultSetAdPics.Success)
                return resultSetAdPics;

            _cache.InvalidateUserAds(userId: adInfo.UserId);
            return Result.NewSuccess();
        }

        public Result<IEnumerable<AdInfo>> GetAds()
        {
            lock (_allAdsLock)
            {
                var maybeCachedAll = _cache.GetAllAds();
                if (maybeCachedAll.Success)
                    return maybeCachedAll.ToResult();

                var resultStorageAds = _adsStorage.GetAds();
                if (!resultStorageAds.Success)
                    return Result<IEnumerable<AdInfo>>.NewFailure("Couldn't get all ads from ads storage.");

                var allAds = _mapper.Map<IEnumerable<AdInfo>>(resultStorageAds.Value);
                foreach (var ad in allAds)
                {
                    var resultAdMedia = _filesService.GetAdMedia(ad.AdId);
                    if (!resultAdMedia.Success)
                        return Result<IEnumerable<AdInfo>>.NewFailure($"Failed to get ad media by {nameof(ad.AdId)}: '{ad.AdId}'.");

                    ad.AdMedia = resultAdMedia.Value.ToList();
                }

                _cache.SetAllAds(allAds);
                return Result<IEnumerable<AdInfo>>.NewSuccess(allAds);
            }
        }

        public Result<IEnumerable<AdInfo>> GetUserAds(long userId)
        {
            var maybeUserAds = _cache.GetUserAds(userId: userId);
            if (maybeUserAds.Success)
                return Result<IEnumerable<AdInfo>>.NewSuccess(maybeUserAds.Value);

            var resultStorageUserAds = _adsStorage.GetAdsByUser(userId: userId);
            if (!resultStorageUserAds.Success)
                return Result<IEnumerable<AdInfo>>.NewFailure(resultStorageUserAds.ErrorMessage);

            var userAds = _mapper.Map<IEnumerable<AdInfo>>(resultStorageUserAds.Value);
            _cache.SetUserAds(userId: userId, ads: userAds);
            return Result<IEnumerable<AdInfo>>.NewSuccess(userAds);
        }

        public Result<AdInfo> GetAd(long adId)
        {
            var maybeAd = _cache.GetAd(adId: adId);
            if (maybeAd.Success)
                return Result<AdInfo>.NewSuccess(maybeAd.Value);

            var resultStorageAd = _adsStorage.GetAd(adId: adId);
            if (!resultStorageAd.Success)
                return Result<AdInfo>.NewFailure(resultStorageAd.ErrorMessage);

            var foundAd = _mapper.Map<AdInfo>(resultStorageAd.Value);
            _cache.SetAd(ad: foundAd);
            return Result<AdInfo>.NewSuccess(foundAd);
        }

        private Result<StorageAdEditInfo> GetValidatedStorageAdEditInfo(AdEditInfo ad)
        {
            if (ad == null)
                Result<StorageAdEditInfo>.NewFailure($"{nameof(ad)} is NULL.");

            var result = new StorageAdEditInfo();
            result.AdId = ad.AdId;

            // UserId;
            //var resultUserAds = GetUserAds(ad.UserId);
            //if (!resultUserAds.Success)
            //    return Result<StorageAdEditInfo>.NewFailure($"Failed to receive user ads by user id: '{ad.UserId}'.");
            //if (resultUserAds.Value.Any())
            //    return Result<StorageAdEditInfo>.NewFailure("User already has an ad.");
            result.UserId = ad.UserId;

            //PlaceId;
            var maybePlace = _placesService.GetPlace(ad.PlaceId);
            if (!maybePlace.Success)
                return Result<StorageAdEditInfo>.NewFailure($"Can't find place by id: {ad.PlaceId}.");
            if (!maybePlace.Value.IsEnabled)
                return Result<StorageAdEditInfo>.NewFailure($"Place '{maybePlace.Value.PlaceCode}' is not enabled.");
            if (maybePlace.Value.PlaceType != PlaceType.City)
                return Result<StorageAdEditInfo>.NewFailure($"Place '{maybePlace.Value.PlaceCode}' is not of type '{PlaceType.City}'.");
            result.PlaceId = ad.PlaceId;

            //Name;
            result.Name = ad.Name;

            //DateBorn;
            if (ad.DateBorn < DateTime.Now.AddYears(-80)
                || ad.DateBorn > DateTime.Now.AddYears(-18))
                return Result<StorageAdEditInfo>.NewFailure($"{nameof(ad.DateBorn)} is invalid. (Too old or too young.");
            result.DateBorn = ad.DateBorn;

            //GenderId;
            if (!ObjectIsEnabledOfType(objectTypeCode: ObjectTypeCodes.Gender, objectId: ad.GenderId))
                return Result<StorageAdEditInfo>.NewFailure($"{nameof(ad.GenderId)} did not pass validation.");
            result.GenderId = ad.GenderId;

            //HeightCm;
            if (ad.HeightCm.HasValue && (ad.HeightCm.Value < 110 || ad.HeightCm > 250))
                return Result<StorageAdEditInfo>.NewFailure($"Height is invalid.");
            result.HeightCm = ad.HeightCm;

            //WeightGr;
            if (ad.WeightGr.HasValue && (ad.WeightGr < 30 * 1000 || ad.WeightGr > 200 * 1000))
                return Result<StorageAdEditInfo>.NewFailure($"Weight is invalid.");
            result.WeightGr = ad.WeightGr;

            //EyeColorId;
            if (ad.EyeColorId.HasValue && !ObjectIsEnabledOfType(objectTypeCode: ObjectTypeCodes.EyeColor, objectId: ad.EyeColorId.Value))
                return Result<StorageAdEditInfo>.NewFailure($"{nameof(ad.EyeColorId)} did not pass validation.");
            result.EyeColorId = ad.EyeColorId;

            //HairColorId;
            if (ad.HairColorId.HasValue && !ObjectIsEnabledOfType(objectTypeCode: ObjectTypeCodes.HairColor, objectId: ad.HairColorId.Value))
                return Result<StorageAdEditInfo>.NewFailure($"{nameof(ad.HairColorId)} did not pass validation.");
            result.HairColorId = ad.HairColorId;

            //HairLengthId;
            if (ad.HairLengthId.HasValue && !ObjectIsEnabledOfType(objectTypeCode: ObjectTypeCodes.HairLength, objectId: ad.HairLengthId.Value))
                return Result<StorageAdEditInfo>.NewFailure($"{nameof(ad.HairLengthId)} did not pass validation.");
            result.HairLengthId = ad.HairLengthId;

            return Result<StorageAdEditInfo>.NewSuccess(result);
        }

        private Result<MediaModel[]> GetValidatedStoragePics(AdEditInfo adInfo)
        {
            if (!adInfo.PicsIds.Any())
                return Result<MediaModel[]>.NewSuccess(Array.Empty<MediaModel>());


            var resultItems = new List<MediaModel>();
            foreach (var picId in adInfo.PicsIds)
            {
                var resultPic = _filesService.GetPic(id: picId);
                if (!resultPic.Success)
                    return Result<MediaModel[]>.NewFailure($"Can't find pic by ID: '{picId}'.");

                resultItems.Add(resultPic.Value);
            }

            return Result<MediaModel[]>.NewSuccess(resultItems.ToArray());
        }

        private bool ObjectIsEnabledOfType(string objectTypeCode, int objectId)
        {
            var maybeGenderType = _objectTypesService.Get(code: objectTypeCode);
            if (!maybeGenderType.Success)
                return false;
            var maybeGenderObject = _objectsService.GetById(objectId: objectId);
            if (!maybeGenderObject.Success)
                return false;
            if (maybeGenderObject.Value.ObjectTypeId != maybeGenderType.Value.Id)
                return false;
            if (!maybeGenderObject.Value.IsEnabled)
                return false;
            return true;
        }
    }
}
