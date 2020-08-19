using DatingCode.Core;
using DatingCode.Storage.Models.Core;
using System.Collections.Generic;

namespace DatingCode.Storage.Interfaces
{
    public interface ITranslationStorage
    {
        Maybe<IEnumerable<StorageTranslation>> GetAll();
        Maybe<IEnumerable<StorageTranslation>> GetForObject(int objectId);
        Maybe<StorageTranslation> GetForObject(int objectId, int localeId);
        Result Create(int objectId, int localeId, string value);
        Result SetValue(int objectId, int localeId, string value);
    }
}
