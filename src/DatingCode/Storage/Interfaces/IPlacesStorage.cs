using DatingCode.Core;
using DatingCode.Storage.Models.Geo;
using System.Collections.Generic;

namespace DatingCode.Storage.Interfaces
{
    public interface IPlacesStorage
    {
        Maybe<List<StoragePlace>> GetAllPlaces();
        Maybe<StoragePlace> GetPlace(int id);
        Result UpdatePlace(StoragePlace place);
        Result<int> CreatePlace(StorageNewPlace mappedPlace);
    }
}
