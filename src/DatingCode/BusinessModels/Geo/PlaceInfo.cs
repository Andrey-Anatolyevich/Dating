namespace DatingCode.BusinessModels.Geo
{
    public class PlaceInfo
    {
        public int Id;
        public int? ParentPlaceId;
        public string PlaceCode;
        public PlaceType PlaceType;
        public bool IsEnabled;
    }
}
