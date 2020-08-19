using Dating.Mapping;
using Dating.Models.Ads;
using DatingCode.Business.Basics;
using DatingCode.Business.Dating;
using DatingCode.Business.Media;
using DatingCode.BusinessModels.Basics;
using DatingCode.BusinessModels.Geo;
using DatingCode.BusinessModels.Market;
using DatingCode.Config;
using DatingCode.Infrastructure.Di;
using DatingCode.Mvc.Attributes;
using DatingCode.Mvc.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dating.Controllers
{
    public class AdsController : BaseController
    {
        public AdsController()
            : base()
        {
            _objectTypesService = DiProxy.Get<IObjectTypesService>();
            _objectsService = DiProxy.Get<IObjectsService>();
            _adsService = DiProxy.Get<IAdsService>();
            _filesService = DiProxy.Get<IFilesService>();
            _uploadFilesHelper = new UploadFilesHelper(uploadFileMaxBytes: 30 * 1024 * 1024);
            _adMapper = new AdEditInfoMapper();
        }

        private IObjectTypesService _objectTypesService;
        private IObjectsService _objectsService;
        private IAdsService _adsService;
        private IFilesService _filesService;
        private UploadFilesHelper _uploadFilesHelper;
        private AdEditInfoMapper _adMapper;

        [Auth]
        public IActionResult MyAds()
        {
            var user = GetMyUser();
            var model = new MyAdsModel();
            FillBaseModel(model);
            var resultUserAds = _adsService.GetUserAds(userId: user.Id);
            var userAds = resultUserAds.Success ? resultUserAds.Value : Enumerable.Empty<AdInfo>();
            userAds = userAds.OrderByDescending(x => x.DateLastModified);
            foreach (var ad in userAds)
            {
                var myAd = new MyAdsListItem()
                {
                    AdId = ad.AdId,
                    Name = ad.Name,
                    LastModified = ad.DateLastModified,
                    MediaCount = ad.AdMedia.Count(),
                    IsActive = ad.IsActive
                };

                var scaledPic = ad.AdMedia.Where(x => x.IsPrimary && x.FileType == FileType.Jpeg).FirstOrDefault()?.ScaledPics.OrderBy(x => x.Size.Width).FirstOrDefault();
                if (scaledPic != null)
                    myAd.MainPicRelativeUrl = GetPartialPicUrl(scaledPic.PathParts);

                model.Ads.Add(myAd);
            }

            return View(model);
        }

        [Auth]
        [HttpGet]
        public IActionResult EditAd(long? adId)
        {
            var myUser = GetMyUser();
            var model = new EditAdModel();

            if (adId.HasValue)
            {
                var resultAd = _adsService.GetAd(adId: adId.Value);
                if (!resultAd.Success)
                    return BadRequest();
                if (resultAd.Value.UserId != myUser.Id)
                    return BadRequest();

                model = _mapper.Map<EditAdModel>(resultAd.Value);
            }
            else
            {
                model.DateBorn = DateTime.Today.AddYears(-25);
                model.HeightCm = 165;
                model.WeightGr = 55000;
            }

            FillBaseModel(model);
            FillPlaceAdCollections(model);
            FillPlaceAdPicsIfAny(model);

            return View(model);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAd(EditAdModel model)
        {
            var myUser = GetMyUser();

            var adInfo = _adMapper.GetAdEditInfo(myUser, model);
            if (!adInfo.Success)
            {
                FillBaseModel(model);
                FillPlaceAdCollections(model);
                FillPlaceAdPicsIfAny(model);
                return View(model);
            }

            var resultEditAd = _adsService.EditAd(adInfo.Value);
            if (!resultEditAd.Success)
            {
                FillBaseModel(model);
                FillPlaceAdCollections(model);
                FillPlaceAdPicsIfAny(model);
                return View(model);
            }

            return RedirectToAction(nameof(MyAds));
        }

        [Auth]
        [HttpPost]
        public IActionResult UploadPicture(long? adId)
        {
            var myUser = GetMyUser();
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || myUser == null)
                return BadRequest();

            if (adId.HasValue)
            {
                var resultTheAd = _adsService.GetAd(adId: adId.Value);
                if (!resultTheAd.Success)
                    return BadRequest();
                if (resultTheAd.Value.UserId != myUser.Id)
                    return BadRequest();
            }

            var mFileData = _uploadFilesHelper.GetFileData(userId: myUser.Id, adId: adId, file: file);
            if (!mFileData.Success)
                return BadRequest();

            var rUploadedPicData = _filesService.UploadPicture(value: mFileData.Value);
            if (!rUploadedPicData.Success)
                return BadRequest();

            var uploadedPic = rUploadedPicData.Value;

            if (uploadedPic.ScaledPics?.Any() != true)
                return BadRequest();

            var mostNarrowPic = uploadedPic.ScaledPics
                .OrderBy(x => x.Size.Width)
                .First();
            var picInfo = new UploadedPicInfo()
            {
                AdMediaId = uploadedPic.AdMediaId,
                RelativePicUrl = GetPartialPicUrl(mostNarrowPic.PathParts)
            };

            return Ok(picInfo);
        }

        private void FillPlaceAdCollections(EditAdModel model)
        {
            model.MaxPicsAllowed = 10;
            model.PicIdsSeparator = Consts.PicIdsSeparator;

            model.EyeColors = ActiveOfType(ObjectTypeCodes.EyeColor);
            model.HairColors = ActiveOfType(ObjectTypeCodes.HairColor);
            model.HairLength = ActiveOfType(ObjectTypeCodes.HairLength);

            model.Genders = ActiveOfType(ObjectTypeCodes.Gender);

            model.Places = _placesService.GetAllPlaces()
                .Value
                .Where(x => x.IsEnabled && x.PlaceType == PlaceType.City)
                .ToArray();
        }

        private IEnumerable<ObjectItem> ActiveOfType(string typeCode)
        {
            var eyeColorType = _objectTypesService.Get(typeCode);
            return _objectsService.ActiveOfType(eyeColorType.Value.Id);
        }

        private void FillPlaceAdPicsIfAny(EditAdModel model)
        {
            if (!model.AdId.HasValue)
                return;

            var adMedia = _filesService.GetAdMedia(adId: model.AdId.Value);
            if (adMedia.Success)
            {
                foreach (var media in adMedia.Value)
                {
                    if (media.FileType == FileType.Jpeg)
                    {
                        var smallestFileInfo = media.ScaledPics.OrderBy(x => x.Size.Width).FirstOrDefault();
                        if (smallestFileInfo == null)
                            continue;

                        var editPicModel = new EditAdPicInfoModel();
                        editPicModel.adMediaId = media.AdMediaId;
                        editPicModel.isMain = media.IsPrimary;
                        editPicModel.relativePicUrl = GetPartialPicUrl(smallestFileInfo.PathParts);
                        model.ExistingPics.Add(editPicModel);
                    }
                }
            }
        }
    }
}
