using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace DbLab.DalPgBase
{
    public abstract class PostgresManager
    {
        private const string CursorName = "ref";
        protected const int BaseTimeout = 30;

        protected List<T> ExecuteCursorFunction<T>(string funcName, Func<NpgsqlDataReader, T> mapper,
            int commandTimeout = BaseTimeout, params (string name, object value, NpgsqlDbType type)[] parameters)
        {
            var query = $"select {funcName}($1)";
            var list = new List<T>();
            var cursorValue = Guid.NewGuid().ToString("N");
            using var connection = BaseBuilder.GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            using (var cmd = new NpgsqlCommand(query, connection, transaction) { Parameters = { new NpgsqlParameter { Value = cursorValue, NpgsqlDbType = NpgsqlDbType.Refcursor} } })
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cursorValue = reader.GetValue(0).ToString();
                }
                reader.Close();

                cmd.CommandText = $"fetch all in {cursorValue}";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(mapper(reader));
                }
                reader.Close();
            }

            transaction.Commit();
            connection.Close();
            return list;
        }
    }
}
