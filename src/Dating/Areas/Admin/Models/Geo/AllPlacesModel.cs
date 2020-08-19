using Dating.Models;
using DatingCode.BusinessModels.Geo;
using System.Collections.Generic;

namespace Dating.Areas.Admin.Models.Geo
{
    public class AllPlacesModel : BaseViewModel
    {
        public AllPlacesModel()
        {
            AllPlaces = new List<PlaceInfo>();
            CurrentPlaceChildren = new List<PlaceInfo>();
            RootPlaces = new List<PlaceInfo>();
        }

        public List<PlaceInfo> AllPlaces;
        public string Error;
        public PlaceInfo CurrentPlace;
        public PlaceInfo ParentPlace;
        public List<PlaceInfo> CurrentPlaceChildren;
        public List<PlaceInfo> RootPlaces;
    }
}
