using System.Collections.Generic;

namespace DatingCode.BusinessModels.Market
{
    internal class SavedFileInfo
    {
        internal SavedFileInfo()
        {
            ScaledPicsInfo = new List<SavedScaledPicInfo>();
        }

        internal List<SavedScaledPicInfo> ScaledPicsInfo;

        internal void AddScaledPicInfo(SavedScaledPicInfo data)
        {
            ScaledPicsInfo.Add(data);
        }
    }
}
