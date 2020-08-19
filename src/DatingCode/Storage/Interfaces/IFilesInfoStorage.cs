using System.Collections.Generic;
using DatingCode.Core;
using DatingCode.Storage.Models.Media;

namespace DatingCode.Storage.Interfaces
{
    public interface IFilesInfoStorage
    {
        Result<StorageMediaModel> SaveMedia(StorageMediaCreateModel storageSaveMediaInfoModel);
        Result<StorageMediaModel> GetMedia(long mediaId);
        Result<List<StorageMediaModel>> GetAdMedia(long adId);
        Result RemoveMedia(long mediaId);
        Result AssignMediaToAd(long adId, long adMediaId, bool isPrimary, int position);
    }
}
