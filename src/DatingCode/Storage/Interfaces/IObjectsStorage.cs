using DatingCode.Core;
using DatingCode.Storage.Models.Basics;
using System.Collections.Generic;

namespace DatingCode.Storage.Interfaces
{
    public interface IObjectsStorage
    {
        Result<IEnumerable<StorageObjectItem>> AllOfType(int typeId);
        Result Create(StorageObjectCreateInfo storageCreateInfo);
        Result Delete(int objectId);
        Result SetCode(int id, string code);
        Maybe<StorageObjectItem> GetById(int objectId);
        Maybe<StorageObjectItem> GetByTypeAndCode(int typeId, string code);
    }
}
