using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Admin.Controllers
{
    public class LocalesController : AdminBaseController
    {
        public LocalesController()
            : base()
        {
        }

        public IActionResult GetLocales()
        {
            var resultAllLocales = _localeService.GetAllLocales();
            var resultJson = ToJson(resultAllLocales.Value);
            return Ok(resultJson);
        }
    }
}
