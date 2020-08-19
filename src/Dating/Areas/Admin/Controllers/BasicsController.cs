using Dating.Areas.Admin.Models.Objects;
using DatingCode.Business.Basics;
using DatingCode.Business.Core;
using DatingCode.BusinessModels.Basics;
using DatingCode.Infrastructure.Di;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Dating.Areas.Admin.Controllers
{
    public class BasicsController : AdminBaseController
    {
        public BasicsController() : base()
        {
            _objectTypesService = DiProxy.Get<IObjectTypesService>();
            _objectsService = DiProxy.Get<IObjectsService>();
            _localizationService = DiProxy.Get<ILocalizationService>();
        }

        private IObjectTypesService _objectTypesService;
        private IObjectsService _objectsService;
        private ILocalizationService _localizationService;

        public IActionResult Objects()
        {
            var model = new ObjectsModel();
            FillBaseModel(model);
            model.ObjectTypes = _objectTypesService.GetAll();
            return View(model);
        }

        public IActionResult GetObjectsOfType(int typeId)
        {
            var objectsOfType = _objectsService.AllOfType(typeId);
            var jsonTreedObjects = JsonConvert.SerializeObject(objectsOfType);
            return Ok(jsonTreedObjects);
        }

        [HttpPost]
        public IActionResult ObjectCreate(int typeId, int? parentId, string code)
        {
            if (typeId <= 0)
                throw new ArgumentException($"{typeId} <= 0", nameof(typeId));
            if (parentId.HasValue && parentId <= 0)
                throw new ArgumentException($"{parentId} <= 0", nameof(parentId));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(code));


            var objCreate = new ObjectCreateInfo
            {
                TypeId = typeId,
                ParentId = parentId,
                Code = code
            };

            _objectsService.Create(objCreate);
            return Ok();
        }

        [HttpPost]
        public IActionResult ObjectDelete(int objectId)
        {
            if (objectId <= 0)
                throw new ArgumentException($"{objectId} <= 0", nameof(objectId));


            _objectsService.Delete(objectId);
            return Ok();
        }

        [HttpPost]
        public IActionResult ObjectSetCode(int id, string code)
        {
            if (id <= 0)
                throw new ArgumentException($"{id} <= 0", nameof(id));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(code));


            _objectsService.SetCode(id, code);
            return Ok();
        }

        [HttpPost]
        public IActionResult ObjectTranslationsPartial(int objectId)
        {
            var model = new ObjectTranslationsPartialModel();

            var maybeObject = _objectsService.GetById(objectId);
            if (!maybeObject.Success)
                return BadRequest();
            model.Object = maybeObject.Value;

            var resultLocales = _localeService.GetAllLocales();
            if (!resultLocales.Success)
                return BadRequest();
            model.Locales = resultLocales.Value;

            var resultTranslations = _localizationService.GetTranslationsForObject(objectId);
            if (!resultTranslations.Success)
                return BadRequest();
            model.Translations = resultTranslations.Value;

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult SetObjectTranslation(int objectId, int localeId, string value)
        {
            var result = _localizationService.SetTranslation(objectId, localeId, value);

            if (result.Success)
                return Ok();
            else
                return BadRequest();
        }
    }
}
