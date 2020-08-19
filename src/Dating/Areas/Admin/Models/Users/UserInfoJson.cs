using System;

namespace Dating.Areas.Admin.Models.Users
{
    public class UserInfoJson
    {
        public long Id;
        public string Email;
        public string Login;
        public string Claims;
        public DateTime DateCreated;
        public DateTime DateLastLogin;
        public string Locale;
    }
}
