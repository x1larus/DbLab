using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Text;

namespace DbLab.DalPgBase
{
    public abstract class PostgresManager
    {
        protected const string CursorName = "ref";
        protected const int BaseTimeout = 30;

        protected async Task<List<T>> ExecuteCursorFunction<T>(string funcName, Func<NpgsqlDataReader, T> mapper,
            int commandTimeout = BaseTimeout, params (string Name, object Value, NpgsqlDbType Type)[] parameters)
        {
            var query = new StringBuilder($"select {funcName}(");
            var inParamsList = new List<NpgsqlParameter>();

            foreach (var inParam in parameters)
            {
                query.Append($"@{inParam.Name}, ");
                inParamsList.Add(new NpgsqlParameter
                    { ParameterName = inParam.Name, Value = inParam.Value, NpgsqlDbType = inParam.Type });
            }

            var cursorValue = Guid.NewGuid().ToString("N");
            query.Append($"@{CursorName})");
            inParamsList.Add(new NpgsqlParameter
                { ParameterName = CursorName, Value = cursorValue, NpgsqlDbType = NpgsqlDbType.Refcursor });


            var list = new List<T>();
            await using var connection = BaseBuilder.GetConnection();
            connection.Open();

            await using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await using (var cmd = new NpgsqlCommand(query.ToString(), connection, transaction))
            {
                cmd.Parameters.AddRange(inParamsList.ToArray());
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cursorValue = reader.GetValue(0).ToString();
                }
                await reader.CloseAsync();
            }

            await using (var cmd = new NpgsqlCommand($"fetch all in \"{cursorValue}\"", connection, transaction))
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(mapper(reader));
                }
                await reader.CloseAsync();
            }

            await transaction.CommitAsync();
            await connection.CloseAsync();
            return list;
        }
    }
}
