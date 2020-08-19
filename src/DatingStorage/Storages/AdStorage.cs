using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Ads;
using DatingStorage.Models.Market;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace DatingStorage.Storages
{
    public class AdStorage : BaseStorage, IAdsStorage
    {
        public AdStorage(ConfigValuesCollection configValues, IMapper mapper,
            IFilesInfoStorage filesInfoStorage)
            : base(configValues, mapper)
        {
            _filesInfoStorage = filesInfoStorage;
        }

        private IFilesInfoStorage _filesInfoStorage;

        public Result<long> CreateAd(StorageAdEditInfo adInfo)
        {
            var utcNow = DateTime.UtcNow;
            var newId = _pgClinet.NewCommand()
                .OnFunc("dating.ad__add")
                .WithParam("p_user_id", NpgsqlDbType.Bigint, adInfo.UserId)
                .WithParam("p_place_id", NpgsqlDbType.Integer, adInfo.PlaceId)
                .WithParam("p_is_active", NpgsqlDbType.Boolean, adInfo.IsActive)
                .WithParam("p_date_create", NpgsqlDbType.Timestamp, utcNow)
                .WithParam("p_date_last_modified", NpgsqlDbType.Timestamp, utcNow)
                .WithParam("p_name", NpgsqlDbType.Varchar, adInfo.Name)
                .WithParam("p_date_born", NpgsqlDbType.Timestamp, adInfo.DateBorn)
                .WithParam("p_gender_id", NpgsqlDbType.Integer, adInfo.GenderId)
                .WithParam("p_height_cm", NpgsqlDbType.Integer, adInfo.HeightCm)
                .WithParam("p_weight_gr", NpgsqlDbType.Integer, adInfo.WeightGr)
                .WithParam("p_eye_color_id", NpgsqlDbType.Integer, adInfo.EyeColorId)
                .WithParam("p_hair_color_id", NpgsqlDbType.Integer, adInfo.HairColorId)
                .WithParam("p_hair_length_id", NpgsqlDbType.Integer, adInfo.HairLengthId)
                .WithParam("p_relationship_status_id", NpgsqlDbType.Integer, adInfo.RelationshipStatusId)
                .WithParam("p_has_kids", NpgsqlDbType.Integer, adInfo.HasKids)
                .WithParam("p_education_level_id", NpgsqlDbType.Integer, adInfo.EducationLevelId)
                .WithParam("p_smoking_id", NpgsqlDbType.Integer, adInfo.SmokingId)
                .WithParam("p_alcohol_id", NpgsqlDbType.Integer, adInfo.AlcoholId)
                .WithParam("p_religion_id", NpgsqlDbType.Integer, adInfo.ReligionId)
                .WithParam("p_zodiac_sign_id", NpgsqlDbType.Integer, adInfo.ZodiacSignId)
                .WithParam("p_body_type_id", NpgsqlDbType.Integer, adInfo.BodyTypeId)
                .WithParam("p_ethnic_group_id", NpgsqlDbType.Integer, adInfo.EthnicGroupId)
                .QueryScalarResult<long>();
            return newId;
        }

        public Result UpdateAd(StorageAdEditInfo adInfo)
        {
            var utcNow = DateTime.UtcNow;
            var result = _pgClinet.NewCommand()
                .OnFunc("dating.ad__update")
                .WithParam("p_ad_id", NpgsqlDbType.Bigint, adInfo.AdId)
                .WithParam("p_user_id", NpgsqlDbType.Bigint, adInfo.UserId)
                .WithParam("p_place_id", NpgsqlDbType.Integer, adInfo.PlaceId)
                .WithParam("p_is_active", NpgsqlDbType.Boolean, adInfo.IsActive)
                .WithParam("p_date_create", NpgsqlDbType.Timestamp, utcNow)
                .WithParam("p_date_last_modified", NpgsqlDbType.Timestamp, utcNow)
                .WithParam("p_name", NpgsqlDbType.Varchar, adInfo.Name)
                .WithParam("p_date_born", NpgsqlDbType.Timestamp, adInfo.DateBorn)
                .WithParam("p_gender_id", NpgsqlDbType.Integer, adInfo.GenderId)
                .WithParam("p_height_cm", NpgsqlDbType.Integer, adInfo.HeightCm)
                .WithParam("p_weight_gr", NpgsqlDbType.Integer, adInfo.WeightGr)
                .WithParam("p_eye_color_id", NpgsqlDbType.Integer, adInfo.EyeColorId)
                .WithParam("p_hair_color_id", NpgsqlDbType.Integer, adInfo.HairColorId)
                .WithParam("p_hair_length_id", NpgsqlDbType.Integer, adInfo.HairLengthId)
                .WithParam("p_relationship_status_id", NpgsqlDbType.Integer, adInfo.RelationshipStatusId)
                .WithParam("p_has_kids", NpgsqlDbType.Integer, adInfo.HasKids)
                .WithParam("p_education_level_id", NpgsqlDbType.Integer, adInfo.EducationLevelId)
                .WithParam("p_smoking_id", NpgsqlDbType.Integer, adInfo.SmokingId)
                .WithParam("p_alcohol_id", NpgsqlDbType.Integer, adInfo.AlcoholId)
                .WithParam("p_religion_id", NpgsqlDbType.Integer, adInfo.ReligionId)
                .WithParam("p_zodiac_sign_id", NpgsqlDbType.Integer, adInfo.ZodiacSignId)
                .WithParam("p_body_type_id", NpgsqlDbType.Integer, adInfo.BodyTypeId)
                .WithParam("p_ethnic_group_id", NpgsqlDbType.Integer, adInfo.EthnicGroupId)
                .QueryVoidResult();
            return result;
        }

        public Result<IEnumerable<StorageAdInfo>> GetAdsByUser(long userId)
        {
            var result = _storageHelper.GetQueryResult(() =>
            {
                var dbUserAds = _pgClinet.NewCommand()
                    .OnFunc("dating.ad__get_by_user")
                    .WithParam("p_user_id", NpgsqlDbType.Bigint, userId)
                    .QueryMany<GetAdInfoModel>();

                var userStorageAds = _mapper.Map<IEnumerable<StorageAdInfo>>(dbUserAds);
                return userStorageAds;
            });

            if (!result.Success)
                return Result<IEnumerable<StorageAdInfo>>.NewFailure(result.ErrorMessage);

            foreach (var storageAd in result.Value)
            {
                var fillResult = FillStorageAdMedia(storageAd);
                if (!fillResult.Success)
                    return Result<IEnumerable<StorageAdInfo>>.NewFailure(fillResult.ErrorMessage);
            }

            return result;
        }

        public Result<StorageAdInfo> GetAd(long adId)
        {
            var maybeDbAd = _pgClinet.NewCommand()
                .OnFunc("dating.ad__get")
                .WithParam("p_ad_id", NpgsqlDbType.Bigint, adId)
                .QueryMaybeSingle<GetAdInfoModel>();

            if (!maybeDbAd.Success)
                return Result<StorageAdInfo>.NewFailure(maybeDbAd.ErrorMessage);

            var storageAd = _mapper.Map<StorageAdInfo>(maybeDbAd.Value);

            var fillResult = FillStorageAdMedia(storageAd);
            if (!fillResult.Success)
                return Result<StorageAdInfo>.NewFailure(fillResult.ErrorMessage);

            return Result<StorageAdInfo>.NewSuccess(storageAd);
        }

        public Result<IEnumerable<StorageAdInfo>> GetAds()
        {
            var result = _storageHelper.GetQueryResult(() =>
            {
                var dbAllAds = _pgClinet.NewCommand()
                    .OnFunc("dating.ad__get_all")
                    .QueryMany<GetAdInfoModel>();

                var storageAds = _mapper.Map<IEnumerable<StorageAdInfo>>(dbAllAds);
                return storageAds;
            });

            if (!result.Success)
                return result;

            foreach (var storageAd in result.Value)
            {
                var fillResult = FillStorageAdMedia(storageAd);
                if (!fillResult.Success)
                    return Result<IEnumerable<StorageAdInfo>>.NewFailure(fillResult.ErrorMessage);
            }

            return result;
        }

        private Result FillStorageAdMedia(StorageAdInfo storageAd)
        {
            var resultAdMedia = _filesInfoStorage.GetAdMedia(storageAd.AdId);
            if (!resultAdMedia.Success)
                return Result.NewFailure(resultAdMedia.ErrorMessage);
            storageAd.AdMedia = resultAdMedia.Value;
            return Result.NewSuccess();
        }
    }
}
