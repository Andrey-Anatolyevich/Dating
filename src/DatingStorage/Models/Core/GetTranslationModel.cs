using DatingStorage.Attirubtes;

namespace DatingStorage.Models.Core
{
#pragma warning disable CS0649
    public class GetTranslationModel
    {
        [PgColumnName("object_translation_id")]
        public int Id;

        [PgColumnName("object_id")]
        public int ObjectId;

        [PgColumnName("object_code")]
        public string ObjectCode;

        [PgColumnName("locale_id")]
        public int LocaleId;

        [PgColumnName("value")]
        public string Value;
    }
}
