using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Core;
using DatingStorage.Models.Core;
using NpgsqlTypes;
using System.Collections.Generic;

namespace DatingStorage.Storages
{
    public class TranslationStorage : BaseStorage, ITranslationStorage
    {
        public TranslationStorage(ConfigValuesCollection configValues, IMapper mapper)
            : base(configValues, mapper)
        { }

        public Result Create(int objectId, int localeId, string value)
        {
            var result = _pgClinet.NewCommand()
                .OnFunc("core.object_translation__create")
                .WithParam("p_object_id", NpgsqlDbType.Integer, objectId)
                .WithParam("p_locale_id", NpgsqlDbType.Integer, localeId)
                .WithParam("p_value", NpgsqlDbType.Varchar, value)
                .QueryVoidResult();
            return result;
        }

        public Maybe<IEnumerable<StorageTranslation>> GetAll()
        {
            var maybeItems = _pgClinet.NewCommand()
                    .OnFunc("core.object_translation__get_all")
                    .QueryMaybeMany<GetTranslationModel>();
            var result = MapMaybe<IEnumerable<GetTranslationModel>, IEnumerable<StorageTranslation>>(maybeItems);
            return result;
        }

        public Maybe<IEnumerable<StorageTranslation>> GetForObject(int objectId)
        {
            var maybeItems = _pgClinet.NewCommand()
                    .OnFunc("core.object_translation__get_for_object")
                    .WithParam("p_object_id", NpgsqlDbType.Integer, objectId)
                    .QueryMaybeMany<GetTranslationModel>();
            var result = MapMaybe<IEnumerable<GetTranslationModel>, IEnumerable<StorageTranslation>>(maybeItems);
            return result;
        }

        public Maybe<StorageTranslation> GetForObject(int objectId, int localeId)
        {
            var maybeTranslation = _pgClinet.NewCommand()
                    .OnFunc("core.object_translation__get_for_object")
                    .WithParam("p_object_id", NpgsqlDbType.Integer, objectId)
                    .WithParam("p_locale_id", NpgsqlDbType.Integer, localeId)
                    .QueryMaybeSingle<GetTranslationModel>();
            var result = MapMaybe<GetTranslationModel, StorageTranslation>(maybeTranslation);
            return result;
        }

        public Result SetValue(int objectId, int localeId, string value)
        {
            var result = _pgClinet.NewCommand()
                .OnFunc("core.object_translation__set_value")
                .WithParam("p_object_id", NpgsqlDbType.Integer, objectId)
                .WithParam("p_locale_id", NpgsqlDbType.Integer, localeId)
                .WithParam("p_value", NpgsqlDbType.Varchar, value)
                .QueryVoidResult();
            return result;
        }
    }
}
