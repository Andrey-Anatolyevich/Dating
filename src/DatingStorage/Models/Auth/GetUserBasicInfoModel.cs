using DatingStorage.Attirubtes;
using System;

namespace DatingStorage.Models.Auth
{
#pragma warning disable CS0649
    public class GetUserBasicInfoModel
    {
        [PgColumnName("user_id")]
        public long Id;

        [PgColumnName("email")]
        public string Email;

        [PgColumnName("login")]
        public string Login;

        [PgColumnName("pass_hash")]
        public string PassHash;

        [PgColumnName("pass_salt")]
        public string PassSalt;

        [PgColumnName("date_created")]
        public DateTime DateCreated;

        [PgColumnName("date_last_login")]
        public DateTime? DateLastLogin;

        [PgColumnName("locale_id")]
        public int LocaleId;

        [PgColumnName("chosen_place_id")]
        public int? ChosenPlaceId;
    }
}
