using DatingStorage.Attirubtes;

namespace DatingStorage.Models.Auth
{
#pragma warning disable CS0649
    public class GetObjectModel
    {
        [PgColumnName("object_id")]
        public int Id;

        [PgColumnName("parent_object_id")]
        public int? ParentId;

        [PgColumnName("object_type_id")]
        public int ObjectTypeId;

        [PgColumnName("object_code")]
        public string ObjectCode;

        [PgColumnName("is_enabled")]
        public bool IsEnabled;
    }
}
