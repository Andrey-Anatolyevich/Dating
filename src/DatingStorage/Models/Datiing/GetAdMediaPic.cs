using DatingCode.Storage.Models.Media;
using DatingStorage.Attirubtes;

namespace DatingStorage.Models.Market
{
#pragma warning disable CS0649
    public class GetAdMediaPic
    {
        [PgColumnName("ad_media_pic_data_id")]
        public long ScaledPicId;

        [PgColumnName("ad_media_id")]
        public long AdMediaId;

        [PgColumnName("width")]
        public int Height;

        [PgColumnName("height")]
        public int Width;

        [PgColumnName("relative_path")]
        public string RelativePathPartsJoined;

        [PgColumnName("pic_type")]
        public StoragePicType PicType;
    }
}
