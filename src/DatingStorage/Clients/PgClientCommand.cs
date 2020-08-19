using DatingCode.Core;
using DatingStorage.Orm;
using DatingStorage.Storages;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace DatingStorage.Clients
{
    internal class PgClientCommand
    {
        internal PgClientCommand(PgClient pgClinet
            , PgDynamicDataReader dynamicDataReader
            , StorageHelper storageHelper)
        {
            _pgClinet = pgClinet;
            _pgDataReader = dynamicDataReader;
            _storageHelper = storageHelper;

            Params = new List<NpgsqlParameter>();
        }

        private PgClient _pgClinet;
        private PgDynamicDataReader _pgDataReader;
        private StorageHelper _storageHelper;

        internal string FuncName;
        internal List<NpgsqlParameter> Params;

        internal PgClientCommand OnFunc(string funcName)
        {
            if (string.IsNullOrWhiteSpace(funcName))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(funcName));


            FuncName = funcName;
            return this;
        }

        internal PgClientCommand WithParam(string paramName, NpgsqlDbType paramType, object value)
        {
            if (string.IsNullOrWhiteSpace(paramName))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(paramName));


            var newParam = new NpgsqlParameter(parameterName: paramName, parameterType: paramType);
            newParam.Value = value ?? DBNull.Value;
            Params.Add(newParam);

            return this;
        }

        internal void QueryVoid()
        {
            _pgClinet.ExecuteNonQuery(FuncName, Params);
        }

        internal Result QueryVoidResult()
        {
            var result = _storageHelper.GetResult(() =>
            {
                _pgClinet.ExecuteNonQuery(FuncName, Params);
            });
            return result;
        }

        internal T QuerySingle<T>() where T : new()
        {
            var table = _pgClinet.ExecuteQueryOnFunc(FuncName, Params);
            T result = _pgDataReader.ReadSingle<T>(table);
            return result;
        }

        internal Result<T> QueryScalarResult<T>()
        {
            var result = _storageHelper.GetQueryResult<T>(() =>
            {
                var table = _pgClinet.ExecuteQueryOnFunc(FuncName, Params);
                T innerResult = _pgDataReader.ReadScalar<T>(table);
                return innerResult;
            });
            return result;
        }

        internal Maybe<IEnumerable<T>> QueryMaybeMany<T>() where T : new()
        {
            var result = _storageHelper.GetQueryMaybe<IEnumerable<T>>(() =>
            {
                return QueryMany<T>();
            });
            return result;
        }

        internal IEnumerable<T> QueryMany<T>() where T : new()
        {
            var table = _pgClinet.ExecuteQueryOnFunc(FuncName, Params);
            var innerResult = _pgDataReader.ReadMany<T>(table);
            return innerResult;
        }

        internal Maybe<T> QueryMaybeSingle<T>() where T : new()
        {
            var result = _storageHelper.GetQueryMaybe<T>(() =>
            {
                return QuerySingle<T>();
            });
            return result;
        }
    }
}
