using DatingCode.Core;
using DatingCode.Storage.Models.Media;

namespace DatingCode.Storage.Interfaces
{
    public interface IFilesStorage
    {
        Result<StorageFileData> SaveFile(StorageFileSaveModel storageSaveFileModel);
    }
}
