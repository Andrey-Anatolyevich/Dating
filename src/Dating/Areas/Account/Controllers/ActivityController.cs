using Dating.Areas.Account.Models.Activity;
using DatingCode.Business.Users;
using DatingCode.BusinessModels.Users;
using DatingCode.Mvc.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Account.Controllers
{
    public class ActivityController : AccountBaseController
    {
        public ActivityController(AuthService userService)
            : base()
        {
            _userService = userService;
        }

        private AuthService _userService;




    }
}