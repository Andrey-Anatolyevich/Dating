using DatingCode.BusinessModels.Market;
using DatingCode.Core;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Mime;

namespace DatingCode.Mvc.Helpers
{
    public class UploadFilesHelper
    {
        public UploadFilesHelper(int uploadFileMaxBytes)
        {
            UploadFileMaxLengthBytes = uploadFileMaxBytes;
        }

        private int UploadFileMaxLengthBytes;

        public Result<UploadFileData> GetFileData(long userId, long? adId, IFormFile file)
        {
            if (file == null)
                return Result<UploadFileData>.NewFailure($"Passed parameter {nameof(file)} is NULL.");

            var fileLengthString = file.Length.ToString();
            if (!int.TryParse(fileLengthString, out var length))
                return Result<UploadFileData>.NewFailure($"File is too long. File length: '{fileLengthString}' bytes.");

            if (length > UploadFileMaxLengthBytes)
                return Result<UploadFileData>.NewFailure($"File length: '{length}' is more than allowed: '{UploadFileMaxLengthBytes}'.");


            var fileTypes = new byte[length];
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                ms.Position = 0;
                ms.Read(fileTypes, 0, length);
            }

            var fileType = GetFileType(file.ContentType);

            var fileData = new UploadFileData(
                userId: userId,
                adId: adId,
                name: file.FileName,
                fileType: fileType,
                bytes: fileTypes);
            return Result<UploadFileData>.NewSuccess(fileData);
        }

        private FileType GetFileType(string contentType)
        {
            switch (contentType)
            {
                case MediaTypeNames.Image.Jpeg:
                    return FileType.Jpeg;
                default: return FileType.Unknown;
            }
        }
    }
}
