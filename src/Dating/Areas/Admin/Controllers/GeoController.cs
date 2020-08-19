using Dating.Areas.Admin.Models.Geo;
using DatingCode.BusinessModels.Geo;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Dating.Areas.Admin.Controllers
{
    public class GeoController : AdminBaseController
    {
        public GeoController() : base()
        {
        }

        public IActionResult GetPlaces()
        {
            var maybePlaces = _placesService.GetAllPlaces();
            if (!maybePlaces.Success)
                return BadRequest();

            var json = ToJson(maybePlaces.Value);
            return Ok(json);
        }

        [HttpPost]
        public IActionResult SetPlaceState(int placeId, bool isEnabled)
        {
            var mbPlace = _placesService.GetPlace(placeId);
            if (!mbPlace.Success)
                return BadRequest();

            mbPlace.Value.IsEnabled = isEnabled;
            var updatePlaceResult = _placesService.UpdatePlace(mbPlace.Value);
            if (!updatePlaceResult.Success)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        public IActionResult CreatePlace()
        {
            var model = new CreatePlaceModel();
            FillBaseModel(model);
            FillCreatePlaceModel(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult CreatePlace(CreatePlaceInfo placeInfo)
        {
            var newPlace = _mapper.Map<NewPlace>(placeInfo);
            var createPlaceResult = _placesService.CreatePlace(newPlace);
            if (createPlaceResult.Success)
                return RedirectToAction("Places", new { id = createPlaceResult.Value.Id });

            var model = new CreatePlaceModel();
            FillBaseModel(model);
            FillCreatePlaceModel(model);
            model.PlaceInfo = placeInfo;
            return View(model);
        }

        private void FillCreatePlaceModel(CreatePlaceModel model)
        {
            var maybeAllPlaces = _placesService.GetAllPlaces();
            if (maybeAllPlaces.Success)
                model.ParentPlaces = maybeAllPlaces.Value.ToDictionary(x => x.Id, x => x.PlaceCode);
        }

        [HttpGet]
        public IActionResult EditPlace(int id)
        {
            var maybePlace = _placesService.GetPlace(id);
            if (maybePlace.Success)
            {
                var model = new EditPlaceModel();
                model.PlaceInfo = _mapper.Map<EditPlaceInfo>(maybePlace.Value);
                FillBaseModel(model);
                return View(model);
            }
            else
            {
                return RedirectToAction("Places");
            }
        }

        [HttpPost]
        public IActionResult EditPlace(EditPlaceInfo placeInfo)
        {
            var place = _mapper.Map<PlaceInfo>(placeInfo);
            var result = _placesService.UpdatePlace(place);
            if (!result.Success)
            {
                var model = new EditPlaceModel();
                FillBaseModel(model);
                model.PlaceInfo = placeInfo;
                return View(model);
            }

            return RedirectToAction("Places", new { id = placeInfo.Id });
        }
    }
}
