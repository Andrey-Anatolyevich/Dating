using DatingCode.BusinessModels.Market;
using DatingCode.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Dating
{
    public class AdsServiceCache
    {
        public AdsServiceCache()
        {
            _byUser = new ConcurrentDictionary<long, IEnumerable<AdInfo>>();
            _byId = new ConcurrentDictionary<long, AdInfo>();
            _allLock = new object();
        }

        private ConcurrentDictionary<long, IEnumerable<AdInfo>> _byUser;
        private ConcurrentDictionary<long, AdInfo> _byId;

        private List<AdInfo> _all;
        private volatile bool _allSet;
        private object _allLock;

        internal Maybe<IEnumerable<AdInfo>> GetAllAds()
        {
            if (!_allSet)
                return Maybe<IEnumerable<AdInfo>>.NewFailure("All ads are not available in cache.");

            lock (_allLock)
            {
                return Maybe<IEnumerable<AdInfo>>.NewSuccess(_all.AsEnumerable());
            }
        }

        internal void SetAllAds(IEnumerable<AdInfo> allAds)
        {
            lock (_allLock)
            {
                _all = allAds.ToList();
                _allSet = true;

                PutAdsInById(allAds);
            }
        }

        internal void InvalidateAll()
        {
            _allSet = false;
        }

        internal Maybe<IEnumerable<AdInfo>> GetUserAds(long userId)
        {
            if (_byUser.TryGetValue(userId, out var ads))
                return Maybe<IEnumerable<AdInfo>>.NewSuccess(ads);

            return Maybe<IEnumerable<AdInfo>>.NewFailure($"No data for user: '{userId}'.");
        }

        internal void SetUserAds(long userId, IEnumerable<AdInfo> ads)
        {
            _byUser.AddOrUpdate(userId, ads, (id, s) => ads);
            PutAdsInById(ads);
        }

        private void PutAdsInById(IEnumerable<AdInfo> ads)
        {
            foreach (var ad in ads)
                _byId.AddOrUpdate(ad.AdId, ad, (id, originalAd) => ad);
        }

        internal Maybe<AdInfo> GetAd(long adId)
        {
            if (_byId.TryGetValue(adId, out var ad))
                return Maybe<AdInfo>.NewSuccess(ad);

            return Maybe<AdInfo>.NewFailure($"No ad is found by id: '{adId}'.");
        }

        internal void SetAd(AdInfo ad)
        {
            _byId.AddOrUpdate(ad.AdId, ad, (id, s) => ad);
        }

        internal void InvalidateUserAds(long userId)
        {
            if (_byUser.TryRemove(userId, out var ads))
                foreach (var ad in ads)
                    _byId.TryRemove(ad.AdId, out _);

            _allSet = false;
        }
    }
}
