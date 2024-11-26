using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Text;

namespace DbLab.DalPgBase
{
    /// <summary>
    /// Родитель для всех менеджеров
    /// </summary>
    public abstract class PostgresManagerBase
    {
        /// <summary>
        /// Выполнить функцию, возвращающую курсор
        /// </summary>
        /// <typeparam name="T">Cущность, в которую будет мапиться результат</typeparam>
        /// <param name="funcName">Имя функции</param>
        /// <param name="mapper">Функция-мапер/></param>
        /// <param name="parameters">Входные параметры функции. ПОРЯДОК ВАЖЕН!</param>
        /// <returns></returns>
        protected List<T> ExecuteCursorFunction<T>(string funcName, Func<NpgsqlDataReader, T> mapper, params (string Name, object? Value, NpgsqlDbType Type)[] parameters)
        {
            var query = $"select {funcName}{CreateParametersQuery(parameters)}";
            var result = new List<T>();

            using var connection = DbManager.GetConnection();

            // Делаем все в транзакции
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            string? cursorValue = "";
            // Вызываем функцию
            using (var cmd = new NpgsqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddRange(CreateParameters(parameters));
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cursorValue = reader.GetValue(0).ToString();
                }
            }

            // Извлекаем данные из курсора и мапим
            using (var cmd = new NpgsqlCommand($"fetch all in \"{cursorValue}\"", connection, transaction))
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(mapper(reader));
                }
                reader.Close();
            }

            transaction.Commit();
            return result;
        }

        protected async ValueTask<T?> ExecuteFunction<T>(string funcName,
            params (string Name, object? Value, NpgsqlDbType Type)[] parameters)
        {
            var query = $"select {funcName}{CreateParametersQuery(parameters)}";
            await using var connection = DbManager.GetConnection();
            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddRange(CreateParameters(parameters));
            await using var reader = await cmd.ExecuteReaderAsync();

            T? res = default;

            while (await reader.ReadAsync())
            {
                res = reader.GetFieldValue<T>(0);
            }

            return res;
        }

        #region private

        private static NpgsqlParameter[] CreateParameters((string Name, object? Value, NpgsqlDbType Type)[] parameters)
        {
            return parameters.Select(el => new NpgsqlParameter { ParameterName = el.Name, Value = el.Value ?? DBNull.Value, NpgsqlDbType = el.Type }).ToArray();
        }

        private static string CreateParametersQuery((string Name, object? Value, NpgsqlDbType Type)[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return "()";

            var res = new StringBuilder("(");

            for (var i = 0; i < parameters.Length; ++i)
            {
                res.Append($"@{parameters[i].Name}");
                res.Append(i != parameters.Length - 1 ? "," : "");
            }
            res.Append(')');

            return res.ToString();
        }

        #endregion
    }
}
