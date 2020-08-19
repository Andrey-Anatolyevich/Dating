using DatingCode.Basics;

namespace DatingCode.BusinessModels.Market
{
    internal class SavedScaledPicInfo
    {
        internal SavedScaledPicInfo(Dimention dimention, string[] filePathParts, ImageType imgType)
        {
            Dimention = dimention;
            FilePathParts = filePathParts;
            ImgType = imgType;
        }

        internal Dimention Dimention;
        internal string[] FilePathParts;
        internal ImageType ImgType;
    }
}
