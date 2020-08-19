using DatingCode.BusinessModels.Market;

namespace DatingCode.Basics
{
    public class ResizeOption
    {
        public ResizeOption(ResizeOperationType resizeType, ImageType imgType)
        {
            OperationType = resizeType;
            ImgType = imgType;
        }

        public ResizeOption(ResizeOperationType resizeType, ImageType imgType, Dimention size)
            : this(resizeType, imgType)
        {
            Size = size;
        }

        public Dimention Size;
        public ResizeOperationType OperationType;
        public ImageType ImgType;
    }
}
