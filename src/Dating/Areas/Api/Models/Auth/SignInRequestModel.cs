using System.Diagnostics;

namespace Dating.Areas.Api.Models.Auth
{
    [DebuggerDisplay("Login: '{Login}' Pass: '{Password}'")]
    public class SignInRequestModel
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
