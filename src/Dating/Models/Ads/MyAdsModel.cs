using DatingCode.BusinessModels.Market;
using System.Collections.Generic;

namespace Dating.Models.Ads
{
    public class MyAdsModel : BaseViewModel
    {
        public MyAdsModel()
        {
            Ads = new List<MyAdsListItem>();
        }

        public List<MyAdsListItem> Ads;
    }
}
