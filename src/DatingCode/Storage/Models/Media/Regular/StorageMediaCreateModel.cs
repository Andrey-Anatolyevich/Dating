using DatingCode.Storage.Models.Basics;
using System;
using System.Collections.Generic;

namespace DatingCode.Storage.Models.Media
{
    public class StorageMediaCreateModel
    {
        public StorageMediaCreateModel()
        {
            ScaledPicData = new List<StoragePicScaledData>();
        }

        public long? AdId;
        public StorageFileType FileType;
        public DateTime DateCreated;
        public bool IsPrimary;
        public int Position;
        public string OriginalFileName;

        public List<StoragePicScaledData> ScaledPicData;
    }
}
