using System;
using System.Collections.Generic;

namespace DatingCode.Storage.Models.Media
{
    public class StorageMediaModel
    {
        public StorageMediaModel()
        {
            ScaledPics = new List<StoragePicScaledData>();
        }

        public long AdMediaId;
        public long AdId;
        public StorageFileType FileType;
        public DateTime DateCreated;
        public bool IsPrimary;
        public int Position;
        public string OriginalFileName;

        public List<StoragePicScaledData> ScaledPics;
    }
}
