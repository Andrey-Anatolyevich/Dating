using DatingCode.Core;
using DatingCode.Storage.Models.Ads;
using System.Collections.Generic;

namespace DatingCode.Storage.Interfaces
{
    public interface IAdsStorage
    {
        Result<long> CreateAd(StorageAdEditInfo adCreationInfo);
        Result<IEnumerable<StorageAdInfo>> GetAdsByUser(long userId);
        Result<StorageAdInfo> GetAd(long adId);
        Result<IEnumerable<StorageAdInfo>> GetAds();
        Result UpdateAd(StorageAdEditInfo storageAdInfo);
    }
}
