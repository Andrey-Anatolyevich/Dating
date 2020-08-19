namespace DatingCode.Storage.Models.Geo
{
    public class StoragePlace
    {
        public int Id;
        public int? ParentPlaceId;
        public string PlaceCode;
        public StoragePlaceType PlaceType;
        public bool IsEnabled;
    }
}
