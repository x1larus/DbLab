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
        protected const string CursorName = "ref";

        /// <summary>
        /// Выполнить функцию, возвращающую курсор
        /// </summary>
        /// <typeparam name="T">Cущность, в которую будет мапиться результат</typeparam>
        /// <param name="funcName">Имя функции</param>
        /// <param name="mapper">Функция-мапер/></param>
        /// <param name="parameters">Входные параметры функции. ПОРЯДОК ВАЖЕН!</param>
        /// <returns></returns>
        protected async Task<List<T>> ExecuteCursorFunction<T>(string funcName, Func<NpgsqlDataReader, T> mapper, params (string Name, object Value, NpgsqlDbType Type)[] parameters)
        {
            var query = $"select {funcName}{CreateParametersQuery(parameters)}";
            var result = new List<T>();

            await using var connection = await DbManager.GetConnectionAsync();

            // Делаем все в транзакции
            await using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            string? cursorValue = "";
            // Вызываем функцию
            await using (var cmd = new NpgsqlCommand(query.ToString(), connection, transaction))
            {
                cmd.Parameters.AddRange(CreateParameters(parameters));
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    cursorValue = reader.GetValue(0).ToString();
                }
            }

            // Извлекаем данные из курсора и мапим
            await using (var cmd = new NpgsqlCommand($"fetch all in \"{cursorValue}\"", connection, transaction))
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(mapper(reader));
                }
                await reader.CloseAsync();
            }

            await transaction.CommitAsync();
            return result;
        }

        #region private

        private static NpgsqlParameter[] CreateParameters((string Name, object Value, NpgsqlDbType Type)[] parameters)
        {
            return parameters.Select(el => new NpgsqlParameter { ParameterName = el.Name, Value = el.Value, NpgsqlDbType = el.Type }).ToArray();
        }

        private static string CreateParametersQuery((string Name, object Value, NpgsqlDbType Type)[] parameters)
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
