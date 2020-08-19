using DatingCode.Business.Basics;
using DatingCode.Config;
using System;

namespace DatingCode.Business.Core
{
    public class Localizer : ILocalizer
    {
        public Localizer(ILocalizationService localeAndTranslationService,
            IObjectTypesService objectTypesService,
            IObjectsService objectsService)
        {
            _objectsService = objectsService;
            _objectTypesService = objectTypesService;
            _localeAndTranslationService = localeAndTranslationService;
        }

        private IObjectsService _objectsService;
        private IObjectTypesService _objectTypesService;
        private ILocalizationService _localeAndTranslationService;

        private int _stringObjectTypeId
        {
            get
            {
                if (!__stringObjectTypeId_init)
                {
                    var maybeStringType = _objectTypesService.Get(code: ObjectTypeCodes.StringCode);
                    if (!maybeStringType.Success)
                        throw new Exception($"Can't find object type {ObjectTypeCodes.StringCode}");

                    __stringObjectTypeId = maybeStringType.Value.Id;
                    __stringObjectTypeId_init = true;
                }
                return __stringObjectTypeId;
            }
        }
        private int __stringObjectTypeId;
        private bool __stringObjectTypeId_init = false;

        public string ForString(int localeId, string templateCode)
        {
            var maybeObject = _objectsService.GetByTypeIdAndCode(typeId: _stringObjectTypeId, code: templateCode);
            if (maybeObject.Success)
            {
                var result = ForObject(localeId: localeId, objectId: maybeObject.Value.Id);
                return result;
            }

            return $"[{ObjectTypeCodes.StringCode}: '{templateCode}']";
        }

        public string ForObject(int localeId, int objectId)
        {
            var maybeTranslation = _localeAndTranslationService.GetTranslationForObject(objectId: objectId, localeId: localeId);
            if (maybeTranslation.Success)
                return maybeTranslation.Value.Value;

            return $"[ObjectId: {objectId}]";
        }
    }
}
