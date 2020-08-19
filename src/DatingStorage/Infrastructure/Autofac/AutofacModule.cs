using Autofac;
using DatingCode.Infrastructure.Autofac;
using DatingCode.Storage.Interfaces;
using DatingStorage.Storages;

namespace DatingStorage.Infrastructure.Autofac
{
    public class AutofacModule : AutofacProfileConfiger
    {
        public override void ConfigureMappings(ContainerBuilder builder)
        {
            builder.RegisterType<UserInfoStorage>().As<IUserInfoStorage>().SingleInstance();
            builder.RegisterType<PlacesStorage>().As<IPlacesStorage>().SingleInstance();
            builder.RegisterType<ObjectTypesStorage>().As<IObjectTypesStorage>().SingleInstance();
            builder.RegisterType<ObjectsStorage>().As<IObjectsStorage>().SingleInstance();
            builder.RegisterType<LocaleStorage>().As<ILocaleStorage>().SingleInstance();
            builder.RegisterType<TranslationStorage>().As<ITranslationStorage>().SingleInstance();
            builder.RegisterType<AdStorage>().As<IAdsStorage>().SingleInstance();
            builder.RegisterType<FilesStorage>().As<IFilesStorage>().SingleInstance();
            builder.RegisterType<FilesInfoStorage>().As<IFilesInfoStorage>().SingleInstance();
        }
    }
}
