using DatingStorage.Attirubtes;

namespace DatingStorage.Models.Auth
{
#pragma warning disable CS0649
    public class GetObjectTypeModel
    {
        [PgColumnName("id")]
        public int Id;

        [PgColumnName("code")]
        public string Code;
    }
}
