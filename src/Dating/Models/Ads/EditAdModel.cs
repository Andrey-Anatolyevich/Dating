using System;
using System.Collections.Generic;
using System.Linq;
using DatingCode.BusinessModels.Basics;
using DatingCode.BusinessModels.Geo;
using Newtonsoft.Json;

namespace Dating.Models.Ads
{
    public class EditAdModel : BaseViewModel
    {
        public EditAdModel()
        {
            EyeColors = Enumerable.Empty<ObjectItem>();
            HairColors = Enumerable.Empty<ObjectItem>();
            HairLength = Enumerable.Empty<ObjectItem>();
            Genders = Enumerable.Empty<ObjectItem>();
            Places = Enumerable.Empty<PlaceInfo>();
            ExistingPics = new List<EditAdPicInfoModel>();
        }

        public int MaxPicsAllowed;
        public string PicIdsSeparator;

        public long? AdId { get; set; }
        public int PlaceId { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        public DateTime DateBorn { get; set; }
        public int? HeightCm { get; set; }
        public int? WeightGr { get; set; }
        public int? EyeColorId { get; set; }
        public int? HairColorId { get; set; }
        public int? HairLengthId { get; set; }
        public int? AlcoholId { get; set; }
        public int? BodyTypeId { get; set; }
        public int? EducationLevelId { get; set; }
        public int? EthnicGroupId { get; set; }
        public bool? HasKids { get; set; }
        public int? RelationshipStatusId { get; set; }
        public int? ReligionId { get; set; }
        public int? SmokingId { get; set; }
        public int? ZodiacSignId { get; set; }

        public long? MainPicId { get; set; }
        public string PicsIdsJoined { get; set; }

        public long[] PicsIds
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PicsIdsJoined))
                    return Array.Empty<long>();

                var idsStrings = PicsIdsJoined.Split(";", StringSplitOptions.RemoveEmptyEntries);
                var ids = new List<long>();
                foreach (var idString in idsStrings)
                {
                    if (long.TryParse(idString, out var id))
                        ids.Add(id);
                }

                return ids.ToArray();
            }
        }

        public List<EditAdPicInfoModel> ExistingPics;
        public string ExistingPicsJson => JsonConvert.SerializeObject(ExistingPics);

        public IEnumerable<ObjectItem> EyeColors;
        public IEnumerable<ObjectItem> HairColors;
        public IEnumerable<ObjectItem> HairLength;
        public IEnumerable<ObjectItem> Genders;
        public IEnumerable<PlaceInfo> Places;
    }
}
