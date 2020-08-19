using System.Collections.Generic;

namespace Dating.Models.DataApi
{
    public class PlaceInfoJsonModel
    {
        public PlaceInfoJsonModel()
        {
            Children = new List<PlaceInfoJsonModel>();
        }

        public int Id;
        public string PlaceCode;
        public string PlaceType;
        public string DisplayName;

        public List<PlaceInfoJsonModel> Children;
    }
}
