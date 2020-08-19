namespace DatingCode.Storage.Models.Media
{
    public class StorageFileSaveModel
    {
        public StorageFileSaveModel(string name, StorageFileType fileType, byte[] bytes)
        {
            Name = name;
            FileType = fileType;
            Bytes = bytes;
        }

        public long UserId;
        public string Name;
        public StorageFileType FileType;
        public byte[] Bytes;
        public bool IsPrimary;
        public int Position;
    }
}
