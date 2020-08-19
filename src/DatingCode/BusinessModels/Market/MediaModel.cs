using System;
using System.Collections.Generic;

namespace DatingCode.BusinessModels.Market
{
    public class MediaModel
    {
        public MediaModel()
        {
            ScaledPics = new List<MediaScaledPicInfo>();
        }

        public long AdMediaId;
        public long? AdId;
        public FileType FileType;
        public DateTime DateCreated;
        public bool IsPrimary;
        public int Position;
        public string OriginalFileName;

        public List<MediaScaledPicInfo> ScaledPics;
    }
}
