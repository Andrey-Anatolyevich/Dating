using Dating.Models;
using DatingCode.Business.Geo;
using DatingCode.Infrastructure.Di;
using DatingCode.Session;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dating.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DiProxy proxy) 
            : base()
        {}

        public IActionResult Index()
        {
            var model = new BaseViewModel();
            FillBaseModel(model);
            return View(model);
        }

        public IActionResult PageNotFound()
        {
            var model = GetBaseModel();
            return View(model);
        }
    }
}
