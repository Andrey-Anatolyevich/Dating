using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using DatingCode.Storage.Models.Core;
using DatingStorage.Models.Core;
using System.Collections.Generic;

namespace DatingStorage.Storages
{
    public class LocaleStorage : BaseStorage, ILocaleStorage
    {
        public LocaleStorage(ConfigValuesCollection configValues, IMapper mapper)
            : base(configValues, mapper)
        { }

        public Maybe<StorageLocaleInfo> Get(int id)
        {
            var result = _storageHelper.GetQueryMaybe(() =>
            {
                var dbLocaleInfo = _pgClinet.NewCommand()
                    .OnFunc("core.locale__get_by_id")
                    .WithParam("p_id", NpgsqlTypes.NpgsqlDbType.Integer, id)
                    .QuerySingle<GetLocaleModel>();

                var storageLocaleInfo = _mapper.Map<StorageLocaleInfo>(dbLocaleInfo);
                return storageLocaleInfo;
            });
            return result;
        }

        public Maybe<StorageLocaleInfo> Get(string code)
        {
            var result = _storageHelper.GetQueryMaybe(() =>
            {
                var dbLocaleInfo = _pgClinet.NewCommand()
                    .OnFunc("core.locale__get_by_code")
                    .WithParam("p_code", NpgsqlTypes.NpgsqlDbType.Varchar, code)
                    .QuerySingle<GetLocaleModel>();

                var storageLocaleInfo = _mapper.Map<StorageLocaleInfo>(dbLocaleInfo);
                return storageLocaleInfo;
            });
            return result;
        }

        public Maybe<IEnumerable<StorageLocaleInfo>> GetAll()
        {
            var maybeItems = _pgClinet.NewCommand()
                    .OnFunc("core.locale__get_all")
                    .QueryMaybeMany<GetLocaleModel>();
            var result = MapMaybe<IEnumerable<GetLocaleModel>, IEnumerable<StorageLocaleInfo>>(maybeItems);
            return result;
        }
    }
}
