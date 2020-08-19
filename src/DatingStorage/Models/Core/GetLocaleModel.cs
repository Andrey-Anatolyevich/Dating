using DatingStorage.Attirubtes;

namespace DatingStorage.Models.Core
{
#pragma warning disable CS0649
    public class GetLocaleModel
    {
        [PgColumnName("locale_id")]
        public int Id;

        [PgColumnName("locale_code")]
        public string Code;

        [PgColumnName("comment")]
        public string Comment;
    }
}
