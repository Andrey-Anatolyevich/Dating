using DatingStorage.Attirubtes;

namespace DatingStorage.Models.Auth
{
#pragma warning disable CS0649
    public  class GetUserIdModel
    {
        [PgColumnName("user_id")]
        public long Id;
    }
}
