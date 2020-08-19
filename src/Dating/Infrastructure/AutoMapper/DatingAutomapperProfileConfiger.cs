using System;
using System.Linq;
using AutoMapper;
using Dating.Areas.Admin.Models.Geo;
using Dating.Areas.Admin.Models.Users;
using Dating.Areas.Api.Models.Auth;
using Dating.Areas.Api.Models.Dating;
using Dating.Models.Ads;
using Dating.Models.DataApi;
using DatingCode.Basics;
using DatingCode.BusinessModels.Auth;
using DatingCode.BusinessModels.Core;
using DatingCode.BusinessModels.Geo;
using DatingCode.BusinessModels.Market;
using DatingCode.Config;
using DatingCode.Infrastructure.Automapper;

namespace Dating.Infrastructure.AutoMapper
{
    public class DatingAutomapperProfileConfiger : AutomapperProfileConfiger
    {
        public override void ConfigureMappings(IProfileExpression profile)
        {
            profile.CreateMap<UserInfo, UserInfoJson>()
                .ForMember(dst => dst.Claims, opt => opt.MapFrom(src => string.Join(",", src.Claims.Select(x => x.ToString()))))
                .ForMember(dst => dst.Locale, opt => opt.MapFrom(src => src.Locale.Code));

            profile.CreateMap<PlaceInfo, EditPlaceInfo>().ReverseMap();

            profile.CreateMap<AdInfo, EditAdModel>()
                .ForMember(dst => dst.PicsIds, opt => opt.Ignore())
                .ForMember(dst => dst.MaxPicsAllowed, opt => opt.Ignore())
                .ForMember(dst => dst.PicIdsSeparator, opt => opt.Ignore())
                .ForMember(dst => dst.EyeColors, opt => opt.Ignore())
                .ForMember(dst => dst.HairColors, opt => opt.Ignore())
                .ForMember(dst => dst.HairLength, opt => opt.Ignore())
                .ForMember(dst => dst.Genders, opt => opt.Ignore())
                .ForMember(dst => dst.Places, opt => opt.Ignore())
                .ForMember(dst => dst.Localizer, opt => opt.Ignore())
                .ForMember(dst => dst.ShowAdminPanel, opt => opt.Ignore())
                .ForMember(dst => dst.CurrentUser, opt => opt.Ignore())
                .ForMember(dst => dst.CurrentLocale, opt => opt.Ignore())
                .ForMember(dst => dst.ExistingPics, opt => opt.Ignore())
                .ForMember(dst => dst.MainPicId,
                        opt => opt.MapFrom(src =>
                            src.AdMedia != null
                                && src.AdMedia.FirstOrDefault(x => x.IsPrimary) != null
                            ? src.AdMedia.First(x => x.IsPrimary).AdMediaId
                            : new Nullable<long>()))
                .ForMember(dst => dst.PicsIdsJoined,
                        opt => opt.MapFrom(src =>
                            src.AdMedia != null
                            ? string.Join(Consts.PicIdsSeparator, src.AdMedia.Select(x => x.AdMediaId))
                            : string.Empty));

            profile.CreateMap<AdInfo, AdDetailsResponse>()
                .ForMember(dst => dst.PicsJson, opt => opt.Ignore());
            profile.CreateMap<MediaModel, ViewMediaModel>();
            profile.CreateMap<FileType, ViewFileType>();
            profile.CreateMap<MediaScaledPicInfo, ViewMediaScaledPicInfo>()
                .ForMember(dst => dst.RelativePath, opt => opt.Ignore());
            profile.CreateMap<ImageType, ViewImageType>();

            profile.CreateMap<PlaceInfo, PlaceInfoJsonModel>()
                .ForMember(dst => dst.Children, opt => opt.Ignore())
                .ForMember(dst => dst.DisplayName, opt => opt.Ignore());
            profile.CreateMap<PlaceType, PlaceTypeJsonModel>();

            profile.CreateMap<UserInfo, SignInResponseModel>()
                .ForMember(dst => dst.LocaleCode, opt => opt.MapFrom(src => src.Locale.Code));

            profile.CreateMap<TranslationInfo, TranslationInfoModel>();
        }
    }
}
