using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Media;
using DatingStorage.Utils;
using System;
using System.IO;

namespace DatingStorage.Storages
{
    public class FilesStorage : IFilesStorage
    {
        public FilesStorage(ConfigValuesCollection configValues)
        {
            _configValues = configValues;
            _filesRootDir = _configValues.GetUserFilesDirectory();
            _fileNameProvider = new MediaFileNameProvider(filesRootDir: _filesRootDir);
        }

        private ConfigValuesCollection _configValues;
        private string _filesRootDir;
        private MediaFileNameProvider _fileNameProvider;

        public Result<StorageFileData> SaveFile(StorageFileSaveModel storageSaveFileModel)
        {
            if(storageSaveFileModel.UserId <=0)
                return Result<StorageFileData>.NewFailure($"{nameof(storageSaveFileModel.UserId)} is not set.");


            var resultExtension = GetFileExtensin(storageSaveFileModel.FileType);
            if (!resultExtension.Success)
                return Result<StorageFileData>.NewFailure(resultExtension.ErrorMessage);

            var relativeFilePathInfo = _fileNameProvider.GetNewFilePathInfo(
                userId: storageSaveFileModel.UserId,
                fileExtension: resultExtension.Value,
                reservationKey: out var reservationKey);

            try
            {
                SaveBytesToPath(rootDir: _filesRootDir, relativeFilePathInfo: relativeFilePathInfo, bytes: storageSaveFileModel.Bytes);

                var result = new StorageFileData()
                {
                    RelativeFilePathParts = relativeFilePathInfo.GetAllParts()
                };

                return Result<StorageFileData>.NewSuccess(result);
            }
            catch (Exception ex)
            {

                return Result<StorageFileData>.NewFailure(ex.Message);
            }
            finally
            {
                _fileNameProvider.Unlock(key: reservationKey);
            }
        }

        private Result<string> GetFileExtensin(StorageFileType fileType)
        {
            string fileExtension;
            switch (fileType)
            {
                case StorageFileType.Jpeg:
                    fileExtension = ".jpg";
                    break;
                default:
                    return Result<string>.NewFailure($"{nameof(StorageFileType)} '{fileType}' is not supported.");
            }
            return Result<string>.NewSuccess(fileExtension);
        }

        private void SaveBytesToPath(string rootDir, FilePathInfo relativeFilePathInfo, byte[] bytes)
        {
            if (!Directory.Exists(rootDir))
                throw new Exception($"Directory '{rootDir}' is missing.");


            MakeSureDirectoriesExist(rootDir: rootDir, subDirParts: relativeFilePathInfo.RelativePathParts);
            var fileRelativeDirectory = Path.Combine(relativeFilePathInfo.RelativePathParts);
            var fileFullDirectory = Path.Combine(rootDir, fileRelativeDirectory);
            var fileFullPath = Path.Combine(fileFullDirectory, relativeFilePathInfo.FileName);
            File.WriteAllBytes(fileFullPath, bytes);
        }

        private void MakeSureDirectoriesExist(string rootDir, string[] subDirParts)
        {
            string currentPath = rootDir;
            foreach (var subDirPart in subDirParts)
            {
                currentPath = Path.Combine(currentPath, subDirPart);
                if (!Directory.Exists(currentPath))
                    Directory.CreateDirectory(currentPath);
            }
        }
    }
}
