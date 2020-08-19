using System;

namespace DatingCode.BusinessModels.Market
{
    public class AdEditInfo
    {
        public long? AdId;
        public long UserId;
        public int PlaceId;
        public string Name;
        public DateTime DateBorn;
        public int GenderId;
        public int? HeightCm;
        public int? WeightGr;
        public int? EyeColorId;
        public int? HairColorId;
        public int? HairLengthId;
        public int? RelationshipStatusId;
        public bool? HasKids;
        public int? EducationLevelId;
        public int? SmokingId;
        public int? AlcoholId;
        public int? ReligionId;
        public int? ZodiacSignId;
        public int? BodyTypeId;
        public int? EthnicGroupId;

        public long? MainPicId;
        public long[] PicsIds;
    }
}
