using AutoMapper;
using Dating.Areas.Api.Models.Dating;
using Dating.Infrastructure.AutoMapper;
using Dating.Models.Ads;
using DatingCode.Basics;
using DatingCode.BusinessModels.Auth;
using DatingCode.BusinessModels.Geo;
using DatingCode.BusinessModels.Market;
using DatingCode.Infrastructure.Automapper;
using DatingCode.Storage.Models.Auth;
using DatingCode.Storage.Models.Geo;
using DatingCode.Storage.Models.Media;
using DatingStorage.Infrastructure.Automapper;
using DatingStorage.Models.Auth;
using DatingStorage.Models.Geo;
using System;
using Xunit;

namespace DatingTests.Code.Mapping
{
    public class MappingTests
    {
        [Fact]
        public void CanMap_UploadFileData_to_StorageFileSaveModel()
        {
            var mapper = GetMapper();

            var src = new UploadFileData(userId: 11, adId: 1, name: "name", fileType: FileType.Jpeg, bytes: new byte[] { });
            var dst = mapper.Map<StorageFileSaveModel>(src);
        }

        [Fact]
        public void CanMap_AdInfo_to_EditAdModel()
        {
            var mapper = GetMapper();

            var src = new AdInfo();
            var dst = mapper.Map<EditAdModel>(src);
        }

        [Fact]
        public void CanMap_AllEnums()
        {
            var mapper = GetMapper();

            EnumValuesAreTheSameAndCanMap<ImageType, StoragePicType>(mapper);
            EnumValuesAreTheSameAndCanMap<StorageUserClaim, UserClaim>(mapper);
            EnumValuesAreTheSameAndCanMap<StoragePlaceType, PlaceType>(mapper);
            EnumValuesAreTheSameAndCanMap<StorageFileType, FileType>(mapper);
            EnumValuesAreTheSameAndCanMap<FileType, ViewFileType>(mapper);
            EnumValuesAreTheSameAndCanMap<PgUserClaim, StorageUserClaim>(mapper);
            EnumValuesAreTheSameAndCanMap<PgPlaceType, StoragePlaceType>(mapper);
        }

        private void EnumValuesAreTheSameAndCanMap<enumSrc, enumDst>(IMapper mapper)
            where enumSrc : struct
            where enumDst : struct
        {
            if (!typeof(enumSrc).IsEnum)
                throw new Exception($"Type '{typeof(enumSrc).FullName}' is not Enum.");
            if (!typeof(enumDst).IsEnum)
                throw new Exception($"Type '{typeof(enumDst).FullName}' is not Enum.");


            var sourceEnumNames = Enum.GetNames(typeof(enumSrc));
            var targetEnumNames = Enum.GetNames(typeof(enumDst));

            foreach (var srcName in sourceEnumNames)
            {
                var srcEnumValue = Enum.Parse<enumSrc>(srcName);
                var dstEnumValue = mapper.Map<enumDst>(srcEnumValue);
            }

            Assert.Equal(sourceEnumNames, targetEnumNames);
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration((expr) =>
            {
                new DatingAutomapperProfileConfiger().ConfigureMappings(expr);
                new DatingCodeAutomapperProfileConfiger().ConfigureMappings(expr);
                new AutomapperProfile().ConfigureMappings(expr);
            });
            config.AssertConfigurationIsValid();

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
