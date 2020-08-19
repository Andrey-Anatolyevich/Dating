using DatingCode.BusinessModels.Geo;
using DatingCode.Core;
using System.Collections.Generic;

namespace DatingCode.Business.Geo
{
    public interface IPlacesService
    {
        Maybe<IEnumerable<PlaceInfo>> GetAllPlaces();
        Maybe<PlaceInfo> GetPlace(int id);
        Result UpdatePlace(PlaceInfo place);
        Result<PlaceInfo> CreatePlace(NewPlace newPlace);
    }
}
