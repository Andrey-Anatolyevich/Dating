using System.Collections.Generic;

namespace Dating.Areas.Api.Models.Auth
{
    public class SignInResponseModel
    {
        public string Email;
        public string Login;
        public List<string> Claims;
        public string LocaleCode;
        public int? ChosenPlaceId;
    }
}
