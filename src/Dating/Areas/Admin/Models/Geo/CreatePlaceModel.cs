using Dating.Models;
using System.Collections.Generic;

namespace Dating.Areas.Admin.Models.Geo
{
    public class CreatePlaceModel : BaseViewModel
    {
        public CreatePlaceModel()
        {
            PlaceInfo = new CreatePlaceInfo();
            ParentPlaces = new Dictionary<int, string>();
        }

        public CreatePlaceInfo PlaceInfo;
        public Dictionary<int, string> ParentPlaces;
    }
}
