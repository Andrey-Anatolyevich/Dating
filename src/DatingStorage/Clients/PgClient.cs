using DatingStorage.Orm;
using DatingStorage.Storages;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace DatingStorage.Clients
{
    public class PgClient
    {
        public PgClient(string pgSqlConnectionString, PgDynamicDataReader pgDynamicDataReader, StorageHelper storageHelper)
        {
            _connectionString = pgSqlConnectionString;
            _pgDynamicDataReader = pgDynamicDataReader;
            _storageHelper = storageHelper;
        }

        private readonly string _connectionString;
        private PgDynamicDataReader _pgDynamicDataReader;
        private StorageHelper _storageHelper;

        internal void ExecuteNonQuery(string funcName, List<NpgsqlParameter> parameters = null)
        {
            ReportCall(funcName);


            using (var conn = GetNewOpenConnection())
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = funcName;

                if (parameters != null)
                    parameters.ForEach(param => cmd.Parameters.Add(param));

                cmd.ExecuteNonQuery();
            }
        }

        internal DataTable ExecuteQueryOnFunc(string funcName, List<NpgsqlParameter> parameters = null)
        {
            ReportCall(funcName);

            var result = new DataTable();
            using (var conn = GetNewOpenConnection())
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = funcName;

                if (parameters != null)
                    parameters.ForEach(param => cmd.Parameters.Add(param));

                using (var pgSqlReader = cmd.ExecuteReader())
                {
                    result.Load(pgSqlReader);
                }
            }
            return result;
        }

        internal T ExecuteScalarFunc<T>(string funcName, List<NpgsqlParameter> parameters = null)
        {
            ReportCall(funcName);

            using (var conn = GetNewOpenConnection())
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = funcName;

                if (parameters != null)
                    parameters.ForEach(param => cmd.Parameters.Add(param));

                object resultObject = cmd.ExecuteScalar();
                var result = (T)resultObject;
                return result;
            }
        }

        private NpgsqlConnection GetNewOpenConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        internal PgClientCommand NewCommand()
        {
            return new PgClientCommand(this, _pgDynamicDataReader, _storageHelper);
        }

        private void ReportCall(string command)
        {
            var bg = Console.BackgroundColor;
            var fg = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Thread.Sleep(10);
            Console.WriteLine($"PgClient reports: Storage call: {command}");
            Thread.Sleep(10);

            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
        }
    }
}
