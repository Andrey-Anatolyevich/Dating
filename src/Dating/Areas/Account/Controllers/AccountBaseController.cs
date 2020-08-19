using Dating.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Account.Controllers
{
    [Area("Account")]
    public abstract class AccountBaseController : BaseController
    {
        public AccountBaseController()
            : base()
        {
        }
    }
}
