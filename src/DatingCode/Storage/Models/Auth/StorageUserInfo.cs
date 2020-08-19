using System;
using System.Collections.Generic;

namespace DatingCode.Storage.Models.Auth
{
    public class StorageUserInfo
    {
        public StorageUserInfo()
        {
            Claims = new HashSet<StorageUserClaim>();
        }

        public long Id;
        public string Email;
        public string Login;
        public string PassSalt;
        public string PassHash;
        public DateTime DateCreated;
        public DateTime DateLastLogin;
        public int LocaleId;
        public int? ChosenPlaceId;

        public HashSet<StorageUserClaim> Claims;
    }
}
