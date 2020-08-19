using DatingStorage.Attirubtes;
using System;

namespace DatingStorage.Models.Market
{
#pragma warning disable CS0649
    internal class GetAdInfoModel
    {
        [PgColumnName("ad_id")]
        public long AdId;

        [PgColumnName("user_id")]
        public long UserId;

        [PgColumnName("place_id")]
        public int PlaceId;

        [PgColumnName("is_active")]
        public bool IsActive;

        [PgColumnName("date_create")]
        public DateTime DateCreate;

        [PgColumnName("date_last_modified")]
        public DateTime DateLastModified;

        [PgColumnName("name")]
        public string Name;

        [PgColumnName("date_born")]
        public DateTime DateBorn;

        [PgColumnName("gender_id")]
        public int? GenderId;

        [PgColumnName("height_cm")]
        public int? HeightCm;

        [PgColumnName("weight_Gr")]
        public int? WeightGr;

        [PgColumnName("eye_color_id")]
        public int? EyeColorId;

        [PgColumnName("hair_color_id")]
        public int? HairColorId;

        [PgColumnName("hair_length_id")]
        public int? HairLengthId;

        [PgColumnName("relationship_status_id")]
        public int? RelationshipStatusId;

        [PgColumnName("has_kids")]
        public bool? HasKids;

        [PgColumnName("education_level_id")]
        public int? EducationLevelId;

        [PgColumnName("smoking_id")]
        public int? SmokingId;

        [PgColumnName("alcohol_id")]
        public int? AlcoholId;

        [PgColumnName("religion_id")]
        public int? ReligionId;

        [PgColumnName("zodiac_sign_id")]
        public int? ZodiacSignId;

        [PgColumnName("body_type_id")]
        public int? BodyTypeId;

        [PgColumnName("ethnic_group_id")]
        public int? EthnicGroupId;
    }
}
