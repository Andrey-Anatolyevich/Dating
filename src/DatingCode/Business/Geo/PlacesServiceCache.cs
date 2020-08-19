using DatingCode.BusinessModels.Geo;
using DatingCode.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DatingCode.Business.Geo
{
    public class PlacesServiceCache
    {
        public PlacesServiceCache()
        {
            _allLock = new object();
            _all = new List<PlaceInfo>();
            _byId = new ConcurrentDictionary<int, PlaceInfo>();
        }

        private object _allLock;
        private bool _allSet;
        private List<PlaceInfo> _all;
        private ConcurrentDictionary<int, PlaceInfo> _byId;

        internal Maybe<IEnumerable<PlaceInfo>> GetAll()
        {
            lock (_allLock)
            {
                if (_allSet)
                    return Maybe<IEnumerable<PlaceInfo>>.NewSuccess(_all);

                return Maybe<IEnumerable<PlaceInfo>>.NewFailure("All not set.");
            }
        }

        internal void SetAll(IEnumerable<PlaceInfo> allPlaces)
        {
            lock (_allLock)
            {
                foreach (var place in allPlaces)
                    Set(place);

                _all.Clear();
                _all.AddRange(allPlaces);
                _allSet = true;
            }
        }

        internal void InvalidateAll()
        {
            lock (_allLock)
            {
                _allSet = false;
            }
        }

        internal Maybe<PlaceInfo> Get(int placeId)
        {
            if (_byId.TryGetValue(placeId, out var value))
                return Maybe<PlaceInfo>.NewSuccess(value);

            return Maybe<PlaceInfo>.NewFailure($"Can't find by ID: '{placeId}'.");
        }

        internal void Set(PlaceInfo place)
        {
            _byId.AddOrUpdate(place.Id, place, (id, pls) => place);
            InvalidateAll();
        }
    }
}
