using Dating.Areas.Admin.Models.Objects;
using DatingCode.Business.Basics;
using DatingCode.BusinessModels.Basics;
using DatingCode.Infrastructure.Di;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Admin.Controllers
{
    public class ObjectsController : AdminBaseController
    {
        public ObjectsController() : base()
        {
            _objectTypesService = DiProxy.Get<IObjectTypesService>();
        }

        private IObjectTypesService _objectTypesService;

        [HttpGet]
        public IActionResult ObjectTypesGetAll()
        {
            var objectTypes = _objectTypesService.GetAll();
            var objectTypesJson = ToJson(objectTypes);
            return Ok(objectTypesJson);
        }

        [HttpPost]
        public IActionResult ObjectTypeCreateOrUpdate(CreateOrUpdateObjectTypeRequest model)
        {
            if (model.Id.HasValue)
            {
                var objectType = new ObjectType(model.Id.Value, model.Code);
                var updateResult = _objectTypesService.Update(objectType);
                if (!updateResult.Success)
                    return BadRequest();
            }
            else
            {
                var createResult = _objectTypesService.Create(model.Code);
                if (!createResult.Success)
                    return BadRequest();
            }

            return Ok();
        }
    }
}
