using DatingCode.Basics;

namespace DatingCode.BusinessModels.Market
{
    public class SizedImageData
    {
        public SizedImageData(ImageType imgType, Dimention size, byte[] bytes)
        {
            ImgType = imgType;
            Size = size;
            Bytes = bytes;
        }

        public ImageType ImgType;
        public Dimention Size;
        public byte[] Bytes;
    }
}
