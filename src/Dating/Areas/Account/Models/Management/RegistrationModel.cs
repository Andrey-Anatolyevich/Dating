using Dating.Models;

namespace Dating.Areas.Account.Models.Management
{
    public class RegistrationModel : BaseViewModel
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
    }
}
