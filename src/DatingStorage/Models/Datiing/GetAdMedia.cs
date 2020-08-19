using DatingStorage.Attirubtes;
using System;

namespace DatingStorage.Models.Market
{
#pragma warning disable CS0649
    public class GetAdMedia
    {
        [PgColumnName("ad_media_id")]
        public long AdMediaId;

        [PgColumnName("ad_id")]
        public long? AdId;

        [PgColumnName("media_type_id")]
        public AdFileType FileType;

        [PgColumnName("date_create")]
        public DateTime DateCreated;

        [PgColumnName("is_primary")]
        public bool IsPrimary;

        [PgColumnName("position")]
        public int Position;

        [PgColumnName("original_file_name")]
        public string OriginalFileName;
    }
}
