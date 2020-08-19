using DatingCode.Core;
using DatingCode.Storage.Models.Core;
using System.Collections.Generic;

namespace DatingCode.Storage.Interfaces
{
    public interface ILocaleStorage
    {
        Maybe<StorageLocaleInfo> Get(int id);
        Maybe<StorageLocaleInfo> Get(string code);
        Maybe<IEnumerable<StorageLocaleInfo>> GetAll();
    }
}
