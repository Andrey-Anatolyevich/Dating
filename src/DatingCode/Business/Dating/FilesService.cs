using AutoMapper;
using DatingCode.Basics;
using DatingCode.Business.Basics;
using DatingCode.BusinessModels.Market;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Media.Utils;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Media
{
    public class FilesService : IFilesService
    {
        public FilesService(IMapper mapper,
            IFilesStorage filesStorage,
            IFilesInfoStorage filesInfoStorage,
            IObjectTypesService objectTypesService,
            IObjectsService objectsService)
        {
            _mapper = mapper;
            _filesBinaryStorage = filesStorage;
            _filesInfoStorage = filesInfoStorage;
            _objectTypesService = objectTypesService;
            _objectsService = objectsService;
            _imgConvUtil = new ImagesConversionUtil();
        }

        private ImagesConversionUtil _imgConvUtil;
        private IFilesStorage _filesBinaryStorage;
        private IFilesInfoStorage _filesInfoStorage;
        private IMapper _mapper;
        private IObjectTypesService _objectTypesService;
        private IObjectsService _objectsService;

        public ResizeOption[] ResizeOptions
        {
            get
            {
                if (!_PicsSizes_init)
                {
                    var picSizeObjType = _objectTypesService.Get(code: ObjectTypeCodes.DatingPicSize);
                    var sizeObjects = _objectsService.ActiveOfType(picSizeObjType.Value.Id);
                    var resizeDimentions = new List<ResizeOption>();
                    resizeDimentions.Add(new ResizeOption(resizeType: ResizeOperationType.Raw, imgType: ImageType.Raw));
                    foreach (var obj in sizeObjects)
                    {
                        var sizeParts = obj.ObjectCode.Split("|");
                        var width = int.Parse(sizeParts[0]);
                        var height = int.Parse(sizeParts[1]);
                        var resizeType = Enum.Parse<ResizeOperationType>(sizeParts[2]);
                        var imageType = Enum.Parse<ImageType>(sizeParts[3]);
                        var newSize = new Dimention(width: width, height: height);
                        var resizeDimentiion = new ResizeOption(size: newSize, resizeType: resizeType, imgType: imageType);
                        resizeDimentions.Add(resizeDimentiion);
                    }
                    _PicsSizes = resizeDimentions.ToArray();
                    _PicsSizes_init = true;
                }
                return _PicsSizes;
            }
        }
        private ResizeOption[] _PicsSizes;
        private bool _PicsSizes_init = false;

        public Result<MediaModel> UploadPicture(UploadFileData imageData)
        {
            if (imageData == null)
                return Result<MediaModel>.NewFailure($"Parameter '{nameof(imageData)}' is NULL.");


            try
            {
                var savedFile = new SavedFileInfo();
                foreach (var option in ResizeOptions)
                {
                    var rScaledImage = _imgConvUtil.GetScaledJpegBytes(imageData.Bytes, option);
                    if (!rScaledImage.Success)
                        return Result<MediaModel>.NewFailure(rScaledImage.ErrorMessage);

                    var scaledImage = rScaledImage.Value;

                    var storageSaveFileModel = _mapper.Map<StorageFileSaveModel>(imageData);
                    storageSaveFileModel.Bytes = scaledImage.Bytes;
                    var rStorageFileData = _filesBinaryStorage.SaveFile(storageSaveFileModel: storageSaveFileModel);
                    if (!rStorageFileData.Success)
                        return Result<MediaModel>.NewFailure(rStorageFileData.ErrorMessage);

                    var imgType = _mapper.Map<ImageType>(option.OperationType);
                    var scaledPicInfo = new SavedScaledPicInfo(
                        dimention: scaledImage.Size,
                        filePathParts: rStorageFileData.Value.RelativeFilePathParts,
                        imgType: imgType);
                    savedFile.AddScaledPicInfo(data: scaledPicInfo);
                }

                // Save the pic data to table.
                var storageSaveMediaModel = GetStorageSaveMediaModel(imageData, savedFile);
                var resultStorageSaveFile = _filesInfoStorage.SaveMedia(storageSaveMediaModel);
                if (!resultStorageSaveFile.Success)
                    return Result<MediaModel>.NewFailure(resultStorageSaveFile.ErrorMessage);

                var result = _mapper.Map<MediaModel>(resultStorageSaveFile.Value);

                return Result<MediaModel>.NewSuccess(result);
            }
            catch (Exception ex)
            {
                return Result<MediaModel>.NewFailure(ex.ToString());
            }
        }

        private StorageMediaCreateModel GetStorageSaveMediaModel(UploadFileData imageData, SavedFileInfo savedFile)
        {
            var storageSaveMediaModel = new StorageMediaCreateModel()
            {
                AdId = imageData.AdId,
                FileType = StorageFileType.Jpeg,
                IsPrimary = imageData.IsPrimary,
                DateCreated = DateTime.UtcNow,
                OriginalFileName = imageData.Name,
                Position = imageData.Position
            };
            foreach (var picData in savedFile.ScaledPicsInfo)
            {
                storageSaveMediaModel.ScaledPicData.Add(new StoragePicScaledData()
                {
                    Size = picData.Dimention,
                    PathParts = picData.FilePathParts,
                    PicType = _mapper.Map<StoragePicType>(picData.ImgType)
                });
            }

            return storageSaveMediaModel;
        }

        public Result<MediaModel> GetPic(long id)
        {
            var resultStorageMedia = _filesInfoStorage.GetMedia(id);
            if (!resultStorageMedia.Success)
                return Result<MediaModel>.NewFailure(resultStorageMedia.ErrorMessage);

            var resultMediaModel = _mapper.Map<MediaModel>(resultStorageMedia.Value);
            return Result<MediaModel>.NewSuccess(resultMediaModel);
        }

        public Result<IEnumerable<MediaModel>> GetAdMedia(long adId)
        {
            var resultStorageAdMedia = _filesInfoStorage.GetAdMedia(adId: adId);
            if (!resultStorageAdMedia.Success)
                return Result<IEnumerable<MediaModel>>.NewFailure(resultStorageAdMedia.ErrorMessage);

            var adMedia = _mapper.Map<IEnumerable<MediaModel>>(resultStorageAdMedia.Value);
            return Result<IEnumerable<MediaModel>>.NewSuccess(adMedia);
        }

        public Result SetAdPics(long adId, MediaModel[] pics)
        {
            var resultCurrentAdMedia = GetAdMedia(adId);
            if (!resultCurrentAdMedia.Success)
                return Result.NewFailure(resultCurrentAdMedia.ErrorMessage);

            var currentAdMedia = resultCurrentAdMedia.Value;
            var currentAdMediaToDelete = currentAdMedia
                .Where(x => !pics.Any(y => y.AdMediaId == x.AdMediaId))
                .ToArray();

            foreach (var adMediaToDelete in currentAdMediaToDelete)
            {
                var resultRemove = _filesInfoStorage.RemoveMedia(adMediaToDelete.AdMediaId);
                if (!resultRemove.Success)
                    return resultRemove;
            }

            foreach (var pic in pics)
            {
                var resultAssign = _filesInfoStorage.AssignMediaToAd(adId: adId, adMediaId: pic.AdMediaId, isPrimary: pic.IsPrimary, position: pic.Position);
                if (!resultAssign.Success)
                    return resultAssign;
            }

            return Result.NewSuccess();
        }
    }
}
