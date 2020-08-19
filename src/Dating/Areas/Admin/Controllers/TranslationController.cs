using Dating.Areas.Admin.Models.Translation;
using DatingCode.Business.Core;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Admin.Controllers
{
    public class TranslationController : AdminBaseController
    {
        public TranslationController(ILocalizationService localizationService)
            : base()
        {
            _localizationService = localizationService;
        }

        private ILocalizationService _localizationService;

        public IActionResult TranslationsList()
        {
            var model = new TranslationsListModel();
            FillBaseModel(model);
            model.Translations = _localizationService.GetAllTranslations();

            return View(model);
        }
    }
}
