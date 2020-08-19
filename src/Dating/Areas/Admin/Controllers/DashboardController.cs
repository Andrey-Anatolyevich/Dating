using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        public DashboardController() : base()
        {
        }

        public IActionResult Index()
        {
            var model = GetBaseModel();
            return View(model);
        }
    }
}