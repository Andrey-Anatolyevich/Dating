using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace DatingStorage.Clients
{
    public class PgClientHelper
    {
        internal void AddParam(List<NpgsqlParameter> parameters, string paramName, NpgsqlDbType type, object value)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(paramName))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(paramName));


            var newParam = new NpgsqlParameter(parameterName: paramName, parameterType: type);
            newParam.Value = value ?? DBNull.Value;
            parameters.Add(newParam);
        }
    }
}
