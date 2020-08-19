using System.Collections.Generic;
using System.Linq;
using Dating.Models.DataApi;
using DatingCode.Business.Basics;
using DatingCode.Business.Core;
using DatingCode.BusinessModels.Geo;
using DatingCode.Infrastructure.Di;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dating.Controllers
{
    public class DataApiController : BaseController
    {
        public DataApiController()
            : base()
        {
            _objectTypesService = DiProxy.Get<IObjectTypesService>();
            _objectsService = DiProxy.Get<IObjectsService>();
            _translationService = DiProxy.Get<ILocalizationService>();

            _placeObjectTypeCode = "place_translation";
        }

        private IObjectTypesService _objectTypesService;
        private IObjectsService _objectsService;
        private ILocalizationService _translationService;

        private string _placeObjectTypeCode;

        public int PlacesObjectTypeId
        {
            get
            {
                if (!_PlacesObjectTypeId_init)
                {
                    _PlacesObjectTypeId = _objectTypesService.Get(code: _placeObjectTypeCode).Value.Id;
                    _PlacesObjectTypeId_init = true;
                }
                return _PlacesObjectTypeId;
            }
        }
        private int _PlacesObjectTypeId;
        private bool _PlacesObjectTypeId_init = false;

        public IActionResult GetTranslations(int localeId)
        {
            var allTranslationsForLocaleModels = GetTranslationsForLocale(localeId);
            var resultJson = ToJson(allTranslationsForLocaleModels);
            return Ok(resultJson);
        }

        public IActionResult GetLocaleWithTranslations()
        {
            var mbLocale = _localeService.GetLocale("EN");
            var translations = GetTranslationsForLocale(mbLocale.Value.Id);
            var resultModel = new GetLocaleWithTranslationsResponse()
            {
                LocaleId = mbLocale.Value.Id,
                Translations = translations
            };
            var result = ToJson(resultModel);
            return Ok(result);
        }

        public IActionResult GetEnabledPlaces()
        {
            var maybeCurrentLocationId = _sessionOperator.ChosenLocaleId_Get(HttpContext);
            if (!maybeCurrentLocationId.Success)
                return BadRequest();

            var maybeAllPlaces = _placesService.GetAllPlaces();
            if (!maybeAllPlaces.Success)
                return BadRequest();

            var allEnabledPlaces = maybeAllPlaces.Value.Where(x => x.IsEnabled).ToArray();
            var result = BuildCountriesCitiesTree(allEnabledPlaces, maybeCurrentLocationId.Value);
            var resultSerialized = JsonConvert.SerializeObject(result);
            return Ok(resultSerialized);
        }

        private List<TranslationInfoModel> GetTranslationsForLocale(int localeId)
        {
            var allTranslationsResult = _translationService.GetAllTranslations();
            if (!allTranslationsResult.Success)
                return null;

            var allTranslationsForLocale = allTranslationsResult.Value
                .Where(x => x.LocaleId == localeId)
                .ToList();

            var allTranslationsForLocaleModels = _mapper.Map<List<TranslationInfoModel>>(allTranslationsForLocale);
            return allTranslationsForLocaleModels;
        }

        private PlaceInfoJsonModel[] BuildCountriesCitiesTree(PlaceInfo[] allEnabledPlaces, int localeId)
        {
            var result = new List<PlaceInfoJsonModel>();

            var countries = allEnabledPlaces.Where(x => x.PlaceType == PlaceType.Country).ToArray();
            foreach (var country in countries)
            {
                var countryJModel = _mapper.Map<PlaceInfoJsonModel>(country);
                var maybePlaceTranslationObject = _objectsService.GetByTypeIdAndCode(PlacesObjectTypeId, country.PlaceCode);
                countryJModel.DisplayName = _localizer.ForObject(localeId, maybePlaceTranslationObject.Value.Id);

                FillCities(countryJModel, countryJModel, allEnabledPlaces, localeId);
                result.Add(countryJModel);
            }

            return result.ToArray();
        }

        private void FillCities(PlaceInfoJsonModel country, PlaceInfoJsonModel thePlace, PlaceInfo[] allEnabledPlaces, int localeId)
        {
            var thePlaceChildren = allEnabledPlaces.Where(x => x.ParentPlaceId == thePlace.Id).ToList();
            foreach (var thePlaceChild in thePlaceChildren)
            {
                var childJModel = _mapper.Map<PlaceInfoJsonModel>(thePlaceChild);
                if (thePlaceChild.PlaceType == PlaceType.City)
                {
                    var maybePlaceTranslationObject = _objectsService.GetByTypeIdAndCode(PlacesObjectTypeId, childJModel.PlaceCode);
                    childJModel.DisplayName = _localizer.ForObject(localeId, maybePlaceTranslationObject.Value.Id);
                    country.Children.Add(childJModel);
                }

                FillCities(country, childJModel, allEnabledPlaces, localeId);
            }
        }
    }
}
