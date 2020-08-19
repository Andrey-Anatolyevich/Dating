using DatingCode.BusinessModels.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.BusinessModels.Auth
{
    public class UserInfo
    {
        public UserInfo()
        {
            Claims = new HashSet<UserClaim>();
        }

        public long Id;
        public string Email;
        public string Login;
        public HashSet<UserClaim> Claims;
        public string PassSalt;
        public string PassHash;
        public DateTime DateCreated;
        public DateTime DateLastLogin;
        public LocaleInfo Locale;
        public int? ChosenPlaceId;

        public bool IsAdmin { get { return Claims.Any(x => x == UserClaim.Admin); } }
    }
}
