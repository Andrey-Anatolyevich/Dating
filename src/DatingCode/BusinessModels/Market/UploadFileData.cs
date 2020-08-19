namespace DatingCode.BusinessModels.Market
{
    public class UploadFileData
    {
        public UploadFileData(long userId, long? adId, string name, FileType fileType, byte[] bytes)
        {
            UserId = userId;
            AdId = adId;
            Name = name;
            FileType = fileType;
            Bytes = bytes;
        }

        public long UserId;
        public long? AdId;
        public string Name;
        public FileType FileType;
        public byte[] Bytes;
        public bool IsPrimary;
        public int Position;
    }
}
