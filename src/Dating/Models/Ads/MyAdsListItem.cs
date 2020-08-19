using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating.Models.Ads
{
    public class MyAdsListItem
    {
        public long AdId;
        public string Name;
        public DateTime LastModified;
        public string MainPicRelativeUrl;
        public int MediaCount;
        public bool IsActive;
    }
}
