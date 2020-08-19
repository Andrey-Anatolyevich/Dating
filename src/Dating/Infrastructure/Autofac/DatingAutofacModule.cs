using Autofac;
using DatingCode.Business.Basics;
using DatingCode.Business.Core;
using DatingCode.Business.Geo;
using DatingCode.Business.Dating;
using DatingCode.Business.Media;
using DatingCode.Business.Users;
using DatingCode.Config;
using DatingCode.Infrastructure.Autofac;
using DatingCode.Infrastructure.Config.Consts;
using DatingCode.Infrastructure.Di;
using DatingCode.Resources;
using DatingCode.Security.Crypto;
using DatingCode.Session;

namespace Dating.Infrastructure.Autofac
{
    public class DatingAutofacModule : AutofacProfileConfiger
    {
        public override void ConfigureMappings(ContainerBuilder builder)
        {
            builder.RegisterType<DiProxy>().AsSelf().SingleInstance();
            builder.RegisterType<ConfigFilesConsts>().AsSelf().SingleInstance();
            builder.RegisterType<ConfigSession>().AsSelf().SingleInstance();
            builder.RegisterType<AuthService>().AsSelf().SingleInstance();
            builder.RegisterType<Consts>().AsSelf().SingleInstance();
            builder.RegisterType<SessionOperator>().AsSelf().SingleInstance();
            builder.RegisterType<Sha256SaltedHasher>().AsSelf().SingleInstance();
            builder.RegisterType<UserInfoService>().As<IUserInfoService>().SingleInstance();
            builder.RegisterType<CachedResourceProvider>().As<IResourceProvider>().SingleInstance();
            builder.RegisterType<PlacesService>().As<IPlacesService>().SingleInstance();
            builder.RegisterType<ObjectTypesService>().As<IObjectTypesService>().SingleInstance();
            builder.RegisterType<ObjectsService>().As<IObjectsService>().SingleInstance();
            builder.RegisterType<AdsService>().As<IAdsService>().SingleInstance();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();
            builder.RegisterType<Localizer>().As<ILocalizer>().SingleInstance();
            builder.RegisterType<FilesService>().As<IFilesService>().SingleInstance();
        }
    }
}
