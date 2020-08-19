using System;
using System.Collections.Generic;

namespace Dating.Areas.Api.Models.Dating
{
    public class ViewMediaModel
    {
        public ViewMediaModel()
        {
            ScaledPics = new List<ViewMediaScaledPicInfo>();
        }

        public long AdMediaId;
        public long? AdId;
        public ViewFileType FileType;
        public DateTime DateCreated;
        public bool IsPrimary;
        public int Position;
        public string OriginalFileName;

        public List<ViewMediaScaledPicInfo> ScaledPics;
    }
}
