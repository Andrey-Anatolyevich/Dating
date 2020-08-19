using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Media;
using DatingStorage.Models.Market;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Linq;

namespace DatingStorage.Storages
{
    public class FilesInfoStorage : BaseStorage, IFilesInfoStorage
    {
        public FilesInfoStorage(ConfigValuesCollection configValues, IMapper mapper)
            : base(configValues, mapper)
        { }

        public Result<StorageMediaModel> SaveMedia(StorageMediaCreateModel model)
        {
            if (model == null)
                return Result<StorageMediaModel>.NewFailure($"{nameof(model)} is NULL.");
            if (model.FileType != StorageFileType.Jpeg)
                return Result<StorageMediaModel>.NewFailure($"{nameof(model.FileType)} {model.FileType} is not supported.");
            if(model.ScaledPicData.Any(x=>x.PicType == StoragePicType.Unknown))
                return Result<StorageMediaModel>.NewFailure($"{nameof(StoragePicType)} {StoragePicType.Unknown} is not supported.");


            var resultTmpAdId = _pgClinet.NewCommand()
                .OnFunc("dating.ad_media__add")
                .WithParam("p_ad_id", NpgsqlDbType.Bigint, model.AdId)
                .WithParam("p_media_type_id", NpgsqlDbType.Integer, (int)model.FileType)
                .WithParam("p_date_create", NpgsqlDbType.Timestamp, model.DateCreated)
                .WithParam("p_is_primary", NpgsqlDbType.Boolean, model.IsPrimary)
                .WithParam("p_position", NpgsqlDbType.Integer, model.Position)
                .WithParam("p_original_file_name", NpgsqlDbType.Varchar, model.OriginalFileName)
                .QueryScalarResult<long>();

            if (!resultTmpAdId.Success)
                return Result<StorageMediaModel>.NewFailure(resultTmpAdId.ErrorMessage);

            var newId = resultTmpAdId.Value;

            switch (model.FileType)
            {
                case StorageFileType.Jpeg:
                    foreach (var picData in model.ScaledPicData)
                    {
                        var pathPartsJoined = string.Join(Consts.PathPartsSeparator, picData.PathParts);

                        var resultPicData = _pgClinet.NewCommand()
                            .OnFunc("dating.ad_media_pic_data__add")
                            .WithParam("p_ad_media_id", NpgsqlDbType.Bigint, newId)
                            .WithParam("p_width", NpgsqlDbType.Integer, picData.Size.Width)
                            .WithParam("p_height", NpgsqlDbType.Integer, picData.Size.Height)
                            .WithParam("p_relative_path", NpgsqlDbType.Varchar, pathPartsJoined)
                            .WithParam("p_pic_type", NpgsqlDbType.Varchar, picData.PicType.ToString())
                            .QueryVoidResult();

                        if (!resultPicData.Success)
                        {
                            RemoveMedia(newId);
                            return Result<StorageMediaModel>.NewFailure(resultPicData.ErrorMessage);
                        }
                    }
                    break;
            }

            var result = GetMedia(newId);
            return result;
        }

        public Result RemoveMedia(long mediaId)
        {
            var resultMedia = GetMedia(mediaId);
            if (!resultMedia.Success)
                return Result.NewFailure(resultMedia.ErrorMessage);


            var mediaTmp = resultMedia.Value;
            switch (mediaTmp.FileType)
            {
                case StorageFileType.Jpeg:
                    var resultRemovePics = _pgClinet.NewCommand()
                        .OnFunc("dating.ad_media_pic_data__remove_by_ad_media_id")
                        .WithParam("p_ad_media_id", NpgsqlDbType.Bigint, mediaId)
                        .QueryVoidResult();
                    if (!resultRemovePics.Success)
                        return resultRemovePics;
                    break;
                default:
                    return Result.NewFailure($"{nameof(mediaTmp.FileType)} '{mediaTmp.FileType}' is not supported.");
            }

            var resultRemoveMedia = _pgClinet.NewCommand()
                .OnFunc("dating.ad_media__remove")
                .WithParam("p_ad_media_id", NpgsqlDbType.Bigint, mediaId)
                .QueryVoidResult();

            return resultRemoveMedia;
        }

        public Result<StorageMediaModel> GetMedia(long mediaId)
        {
            var maybeMediaInfoTmp = _pgClinet.NewCommand()
                .OnFunc("dating.ad_media__get")
                .WithParam("p_ad_media_id", NpgsqlDbType.Bigint, mediaId)
                .QueryMaybeSingle<GetAdMedia>();

            if (!maybeMediaInfoTmp.Success)
                return Result<StorageMediaModel>.NewFailure(maybeMediaInfoTmp.ErrorMessage);

            var mediaInfo = maybeMediaInfoTmp.Value;
            var resultValue = _mapper.Map<StorageMediaModel>(mediaInfo);

            var fillResult = FillMediaInfo(resultValue);
            if (!fillResult.Success)
                return Result<StorageMediaModel>.NewFailure(fillResult.ErrorMessage);

            return Result<StorageMediaModel>.NewSuccess(resultValue);
        }

        public Result<List<StorageMediaModel>> GetAdMedia(long adId)
        {
            var maybeMediaInfos = _pgClinet.NewCommand()
                .OnFunc("dating.ad_media__get_by_ad_id")
                .WithParam("p_ad_id", NpgsqlDbType.Bigint, adId)
                .QueryMaybeMany<GetAdMedia>();

            if (!maybeMediaInfos.Success)
                return Result<List<StorageMediaModel>>.NewFailure(maybeMediaInfos.ErrorMessage);

            var adStorageMedias = _mapper.Map<List<StorageMediaModel>>(maybeMediaInfos.Value);

            foreach (var media in adStorageMedias)
            {
                var fillResult = FillMediaInfo(media);
                if (!fillResult.Success)
                    return Result<List<StorageMediaModel>>.NewFailure(fillResult.ErrorMessage);
            }

            return Result<List<StorageMediaModel>>.NewSuccess(adStorageMedias);
        }

        private Result FillMediaInfo(StorageMediaModel storageMedia)
        {
            switch (storageMedia.FileType)
            {
                case StorageFileType.Jpeg:
                    var maybeDbPicDetails = _pgClinet.NewCommand()
                        .OnFunc("dating.ad_media_pic_data__get_by_ad_media_id")
                        .WithParam("p_ad_media_id", NpgsqlDbType.Bigint, storageMedia.AdMediaId)
                        .QueryMaybeMany<GetAdMediaPic>();
                    if (!maybeDbPicDetails.Success)
                        return Result.NewFailure($"Couldn't load Pic details by mediaID: '{storageMedia.AdMediaId}'.");

                    storageMedia.ScaledPics = _mapper.Map<List<StoragePicScaledData>>(maybeDbPicDetails.Value);
                    break;
                default:
                    return Result.NewFailure($"{nameof(storageMedia.FileType)} '{storageMedia.FileType}' is not supported.");
            }

            return Result.NewSuccess();
        }

        public Result AssignMediaToAd(long adId, long adMediaId, bool isPrimary, int position)
        {
            var result = _pgClinet.NewCommand()
                .OnFunc("dating.ad_media__assign_to_ad")
                .WithParam("p_ad_media_id", NpgsqlDbType.Bigint, adMediaId)
                .WithParam("p_ad_id", NpgsqlDbType.Bigint, adId)
                .WithParam("p_is_primary", NpgsqlDbType.Boolean, isPrimary)
                .WithParam("p_position", NpgsqlDbType.Integer, position)
                .QueryVoidResult();
            return result;
        }
    }
}
