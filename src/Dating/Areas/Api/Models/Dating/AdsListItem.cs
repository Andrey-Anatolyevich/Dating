using System;

namespace Dating.Areas.Api.Models.Dating
{
    public class AdsListItem
    {
        public long AdId;
        public int PlaceId;
        public string Name;
        public string PicRelativeUrl;
        public DateTime LastModified;
        public DateTime LastOnline;
        public int? WeightGr;
        public int? HeightCm;
        public int Age;
    }
}
