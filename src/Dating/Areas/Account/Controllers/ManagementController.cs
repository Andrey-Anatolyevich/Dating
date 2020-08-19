using Dating.Areas.Account.Models.Management;
using DatingCode.Business.Users;
using DatingCode.Mvc.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Account.Controllers
{
    public class ManagementController : AccountBaseController
    {
        public ManagementController(IUserInfoService userService)
            : base()
        {
            _userService = userService;
        }

        private IUserInfoService _userService;

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegistrationModel();
            FillBaseModel(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistrationModel model)
        {
            FillBaseModel(model);
            return View(model);
        }

        [Auth]
        public IActionResult MyAccount()
        {
            var model = GetBaseModel();
            return View(model);
        }
    }
}