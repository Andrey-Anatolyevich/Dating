using System.Collections.Generic;
using DatingCode.BusinessModels.Market;
using DatingCode.Core;

namespace DatingCode.Business.Media
{
    public interface IFilesService
    {
        Result<MediaModel> UploadPicture(UploadFileData value);
        Result<MediaModel> GetPic(long id);
        Result<IEnumerable<MediaModel>> GetAdMedia(long adId);
        Result SetAdPics(long adId, MediaModel[] pics);
    }
}
