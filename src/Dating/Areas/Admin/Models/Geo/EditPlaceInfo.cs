using DatingCode.BusinessModels.Geo;

namespace Dating.Areas.Admin.Models.Geo
{
    public class EditPlaceInfo
    {
        public int Id { get; set; }
        public int? ParentPlaceId { get; set; }
        public string PlaceCode { get; set; }
        public PlaceType PlaceType { get; set; }
        public bool IsEnabled { get; set; }
    }
}
