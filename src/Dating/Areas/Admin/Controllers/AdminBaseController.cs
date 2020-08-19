using Dating.Controllers;
using DatingCode.Mvc.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Auth(allowedClaims: "Admin")]
    public abstract class AdminBaseController : BaseController
    {
        public AdminBaseController()
            : base()
        {
        }
    }
}