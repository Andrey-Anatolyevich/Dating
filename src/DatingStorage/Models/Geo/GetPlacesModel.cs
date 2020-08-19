using DatingStorage.Attirubtes;

namespace DatingStorage.Models.Geo
{
#pragma warning disable CS0649
    public class GetPlacesModel
    {
        [PgColumnName("place_id")]
        public int Id;

        [PgColumnName("parent_place_id")]
        public int? ParentPlaceId;

        [PgColumnName("place_code")]
        public string PlaceCode;

        [PgColumnName("place_type_id")]
        public PgPlaceType PlaceType;

        [PgColumnName("is_enabled")]
        public bool IsEnabled;
    }
}
