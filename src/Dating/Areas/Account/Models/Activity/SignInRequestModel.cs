using Dating.Models;

namespace Dating.Areas.Account.Models.Activity
{
    public class SignInRequestModel : BaseViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Error { get; internal set; }
    }
}
