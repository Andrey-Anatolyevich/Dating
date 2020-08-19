using DatingCode.BusinessModels.Geo;
using DatingCode.Core;

namespace Dating.Areas.Api.Models.Dating
{
    public class AdListFiltersModel
    {
        public Maybe<PlaceInfo> ChosenPlace;
        public int AgeMin;
        public int AgeMax;
        public int? AgeChosen;
    }
}
