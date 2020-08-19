namespace DatingCode.Storage.Models.Basics
{
    public struct StorageObjectItem
    {
        public StorageObjectItem(int id, int? parentId, int objectTypeId, string code, bool isEnabled)
        {
            Id = id;
            ParentId = parentId;
            ObjectTypeId = objectTypeId;
            ObjectCode = code;
            IsEnabled = isEnabled;
        }


        public int Id;
        public int? ParentId;
        public int ObjectTypeId;
        public string ObjectCode;
        public bool IsEnabled;
    }
}
