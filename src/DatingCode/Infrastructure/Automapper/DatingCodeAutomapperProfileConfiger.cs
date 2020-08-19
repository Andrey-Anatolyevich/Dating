using AutoMapper;
using DatingCode.Basics;
using DatingCode.BusinessModels.Auth;
using DatingCode.BusinessModels.Basics;
using DatingCode.BusinessModels.Core;
using DatingCode.BusinessModels.Geo;
using DatingCode.BusinessModels.Market;
using DatingCode.Storage.Models.Ads;
using DatingCode.Storage.Models.Auth;
using DatingCode.Storage.Models.Basics;
using DatingCode.Storage.Models.Core;
using DatingCode.Storage.Models.Geo;
using DatingCode.Storage.Models.Media;

namespace DatingCode.Infrastructure.Automapper
{
    public class DatingCodeAutomapperProfileConfiger : AutomapperProfileConfiger
    {
        public override void ConfigureMappings(IProfileExpression profile)
        {
            profile.CreateMap<StorageUserInfo, UserInfo>()
                .ForMember(dst => dst.Locale, opt => opt.Ignore())
                .ReverseMap();
            profile.CreateMap<StorageUserClaim, UserClaim>().ReverseMap();
            profile.CreateMap<StorageLocaleInfo, LocaleInfo>().ReverseMap();

            profile.CreateMap<StoragePlace, PlaceInfo>().ReverseMap();
            profile.CreateMap<StoragePlaceType, PlaceType>().ReverseMap();

            profile.CreateMap<StorageObjectType, ObjectType>().ReverseMap();

            profile.CreateMap<StorageObjectItem, ObjectItem>().ReverseMap();

            profile.CreateMap<StorageAdEditInfo, AdEditInfo>()
                .ForMember(dst => dst.MainPicId, opt => opt.Ignore())
                .ForMember(dst => dst.PicsIds, opt => opt.Ignore())
                .ReverseMap();

            profile.CreateMap<StorageObjectCreateInfo, ObjectCreateInfo>().ReverseMap();

            profile.CreateMap<StorageAdInfo, AdInfo>().ReverseMap();
            profile.CreateMap<StorageMediaModel, MediaModel>().ReverseMap();
            profile.CreateMap<StoragePicScaledData, MediaScaledPicInfo>().ReverseMap();

            profile.CreateMap<StorageFileType, FileType>();

            profile.CreateMap<MediaScaledPicInfo, StoragePicScaledData>();
            profile.CreateMap<ImageType, StoragePicType>();

            profile.CreateMap<UploadFileData, StorageFileSaveModel>();

            profile.CreateMap<StorageTranslation, TranslationInfo>();
        }
    }
}
