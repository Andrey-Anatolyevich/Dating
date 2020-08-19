using System;
using System.Collections.Generic;
using System.Linq;
using Dating.Areas.Api.Models.Dating;
using DatingCode.Basics;
using DatingCode.Business.Dating;
using DatingCode.Business.Users;
using DatingCode.BusinessModels.Geo;
using DatingCode.BusinessModels.Market;
using DatingCode.Core;
using DatingCode.Extensions;
using DatingCode.Infrastructure.Di;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dating.Areas.Api.Controllers
{
    public class DatingController : ApiBaseController
    {
        public DatingController()
            : base()
        {
            _adsService = DiProxy.Get<IAdsService>();
            _usersService = DiProxy.Get<IUserInfoService>();
        }

        private IAdsService _adsService;
        private IUserInfoService _usersService;

        public IActionResult GetAdDetails(long id)
        {
            var ad = _adsService.GetAd(adId: id);
            //if (!ad.Success)
            //    return RedirectToAction(nameof(List));

            var adViewModel = _mapper.Map<AdDetailsResponse>(ad.Value);
            FillPicsUrlsIfAny(adViewModel);
            FillPicsJson(adViewModel);
            var resultBox = new AdDetailsBoxResponse();
            resultBox.ViewAdModel = adViewModel;

            return View(resultBox);
        }

        public IActionResult GetListItems(AdsListFilterRequest filter)
        {
            var allAdsResult = _adsService.GetAds();
            if (!allAdsResult.Success)
                return BadRequest("Something bad happened. Try again.");

            var allAdsFiltered = GetAdsFiltered(allAdsResult.Value, filter);
            var model = new GetAdResponse();
            model.Ads = allAdsFiltered;
            var result =  ToJson(model);
            return Ok(result);
        }

        private IEnumerable<AdsListItem> GetAdsFiltered(IEnumerable<AdInfo> ads, AdsListFilterRequest filter)
        {
            var result = GetAdsListItems(ads);
            if (filter.PlaceId.HasValue)
                result = result.Where(x => x.PlaceId == filter.PlaceId.Value);

            return result;
        }

        private void FillPicsUrlsIfAny(AdDetailsResponse adViewModel)
        {
            foreach (var media in adViewModel.AdMedia)
            {
                media.ScaledPics.ForEach(x => x.RelativePath = GetPartialPicUrl(x.PathParts));
            }
        }

        private void FillPicsJson(AdDetailsResponse adViewModel)
        {
            var jsonPicInfos = new List<AdPicInfoModelResponse>();
            foreach (var media in adViewModel.AdMedia)
            {
                var previewPic = media.ScaledPics.FirstOrDefault(x => x.PicType == ViewImageType.Preview_M);
                if (previewPic == null)
                    continue;

                var newPicInfo = new AdPicInfoModelResponse()
                {
                    id = previewPic.ScaledPicId,
                    isMain = media.IsPrimary,
                    selected = media.IsPrimary,
                    url = previewPic.RelativePath
                };
                jsonPicInfos.Add(newPicInfo);
            }
            adViewModel.PicsJson = JsonConvert.SerializeObject(jsonPicInfos);
        }

        private IEnumerable<AdsListItem> GetAdsListItems(IEnumerable<AdInfo> value)
        {
            var result = new List<AdsListItem>();
            foreach (var item in value)
            {
                var newItem = new AdsListItem();
                newItem.AdId = item.AdId;
                newItem.PlaceId = item.PlaceId;
                newItem.Name = item.Name;
                newItem.LastModified = item.DateLastModified;
                newItem.LastOnline = _usersService.GetUserInfo(item.UserId).Value?.DateLastLogin ?? new DateTime();
                newItem.Age = item.DateBorn.YearsToUtcNow();
                newItem.HeightCm = item.HeightCm;
                newItem.WeightGr = item.WeightGr;

                var firstPic = item.AdMedia.Where(x => x.IsPrimary && x.FileType == FileType.Jpeg).FirstOrDefault();
                if (firstPic != null)
                {
                    var smallestPic = firstPic.ScaledPics.Where(x => x.PicType == ImageType.Preview_M).FirstOrDefault();
                    if (smallestPic != null)
                    {
                        newItem.PicRelativeUrl = GetPartialPicUrl(smallestPic.PathParts);
                    }
                }

                result.Add(newItem);
            }
            return result;
        }

        private void FillFiltersModel(AdListFiltersModel filtersModel)
        {
            var maybeUser = GetMaybeMyUser();
            if (maybeUser.Success)
            {
                var nPlaceId = maybeUser.Value.ChosenPlaceId;
                if (nPlaceId.HasValue)
                {
                    var maybePlace = _placesService.GetPlace(nPlaceId.Value);
                    if (maybePlace.Success && maybePlace.Value.IsEnabled)
                        filtersModel.ChosenPlace = maybePlace;
                }
            }
            else
            {
                var maybePlaceId = _sessionOperator.ChosenLocationId_Get(HttpContext);
                if (maybePlaceId.Success)
                {
                    var maybePlace = _placesService.GetPlace(maybePlaceId.Value);
                    if (maybePlace.Success && maybePlace.Value.IsEnabled)
                    {
                        filtersModel.ChosenPlace = maybePlace;
                    }
                }
            }

            if (filtersModel.ChosenPlace == null)
                filtersModel.ChosenPlace = Maybe<PlaceInfo>.NewFailure("Place is not set.");

            filtersModel.AgeMin = 18;
            filtersModel.AgeMax = 59;
            filtersModel.AgeChosen = 25;
        }
    }
}
