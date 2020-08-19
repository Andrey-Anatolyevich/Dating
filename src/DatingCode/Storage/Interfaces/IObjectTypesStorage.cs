using DatingCode.Core;
using DatingCode.Storage.Models.Basics;
using System.Collections.Generic;

namespace DatingCode.Storage.Interfaces
{
    public interface IObjectTypesStorage
    {
        Maybe<IEnumerable<StorageObjectType>> GetAll();
        Result Update(StorageObjectType storageObjectType);
        Result<int> Create(string code);
    }
}
