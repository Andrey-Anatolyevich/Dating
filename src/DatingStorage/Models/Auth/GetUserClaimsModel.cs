using DatingStorage.Attirubtes;

namespace DatingStorage.Models.Auth
{
#pragma warning disable CS0649
    public class GetUserClaimsModel
    {
        [PgColumnName("claim_id")]
        public int ClaimId;

        [PgColumnName("claim_code")]
        public PgUserClaim ClaimCode;
    }
}
