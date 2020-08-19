using AutoMapper;
using DatingCode.Config;
using DatingCode.Core;
using DatingStorage.Clients;
using DatingStorage.Orm;
using System;

namespace DatingStorage.Storages
{
    public class BaseStorage
    {
        public BaseStorage(ConfigValuesCollection configValues, IMapper mapper)
        {
            _configValues = configValues;
            _mapper = mapper;
            var pgConnectionString = _configValues.GetPgConnectionString();
            _pgDataReader = new PgDynamicDataReader();
            _storageHelper = new StorageHelper();
            _pgClinet = new PgClient(pgConnectionString, _pgDataReader, _storageHelper);
            _pgHelper = new PgClientHelper();
        }

        internal protected ConfigValuesCollection _configValues;
        internal protected PgClient _pgClinet;
        internal protected PgClientHelper _pgHelper;
        internal protected PgDynamicDataReader _pgDataReader;
        internal protected IMapper _mapper;
        internal protected StorageHelper _storageHelper;

        internal protected Maybe<TTo> MapMaybe<TFrom, TTo>(Maybe<TFrom> maybeItems)
        {
            if (maybeItems.Success)
            {
                try
                {
                    var mappedItems = _mapper.Map<TTo>(maybeItems.Value);
                    var result = Maybe<TTo>.NewSuccess(mappedItems);
                    return result;
                }
                catch (Exception ex)
                {
                    return Maybe<TTo>.NewFailure(ex.Message);
                }
            }
            else
            {
                return Maybe<TTo>.NewFailure(maybeItems.ErrorMessage);
            }
        }
    }
}
