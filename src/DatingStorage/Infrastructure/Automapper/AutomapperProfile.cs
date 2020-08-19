using AutoMapper;
using DatingCode.Basics;
using DatingCode.Infrastructure.Automapper;
using DatingCode.Storage.Models.Ads;
using DatingCode.Storage.Models.Auth;
using DatingCode.Storage.Models.Basics;
using DatingCode.Storage.Models.Core;
using DatingCode.Storage.Models.Geo;
using DatingCode.Storage.Models.Media;
using DatingStorage.Models.Auth;
using DatingStorage.Models.Core;
using DatingStorage.Models.Geo;
using DatingStorage.Models.Market;
using System;

namespace DatingStorage.Infrastructure.Automapper
{
    public class AutomapperProfile : AutomapperProfileConfiger
    {
        public override void ConfigureMappings(IProfileExpression profile)
        {
            profile.CreateMap<GetUserBasicInfoModel, StorageUserInfo>()
                .ForMember(dst => dst.Claims, opt => opt.Ignore());
            profile.CreateMap<PgUserClaim, StorageUserClaim>();

            profile.CreateMap<GetPlacesModel, StoragePlace>();
            profile.CreateMap<PgPlaceType, StoragePlaceType>();

            profile.CreateMap<GetObjectTypeModel, StorageObjectType>();

            profile.CreateMap<GetObjectModel, StorageObjectItem>();

            profile.CreateMap<GetAdInfoModel, StorageAdInfo>()
                .ForMember(dst => dst.AdMedia, opt => opt.Ignore());

            profile.CreateMap<GetAdMedia, StorageMediaModel>()
                .ForMember(dst => dst.ScaledPics, opt => opt.Ignore());
            profile.CreateMap<GetAdMediaPic, StoragePicScaledData>()
                .ForMember(dst => dst.PathParts
                    , opt => opt.MapFrom(src =>
                        src.RelativePathPartsJoined.Split(Consts.PathPartsSeparator, StringSplitOptions.RemoveEmptyEntries)))
                .ForMember(dst => dst.Size, opt => opt.MapFrom(src => new Dimention(src.Width, src.Height)));

            profile.CreateMap<GetLocaleModel, StorageLocaleInfo>();

            profile.CreateMap<GetTranslationModel, StorageTranslation>();
        }
    }
}
