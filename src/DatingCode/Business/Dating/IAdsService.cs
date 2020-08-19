using DatingCode.BusinessModels.Market;
using DatingCode.Core;
using System.Collections.Generic;

namespace DatingCode.Business.Dating
{
    public interface IAdsService
    {
        Result EditAd(AdEditInfo adInfo);
        Result<IEnumerable<AdInfo>> GetUserAds(long userId);
        Result<IEnumerable<AdInfo>> GetAds();
        Result<AdInfo> GetAd(long adId);
    }
}
