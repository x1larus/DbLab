using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Text;

namespace DbLab.DalPgBase
{
    /// <summary>
    /// Родитель для всех менеджеров
    /// </summary>
    public abstract class PostgresManager
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
            var query = new StringBuilder($"select {funcName}(");
            var queryParamsList = new List<NpgsqlParameter>();
            var result = new List<T>();

            // собираем параметры
            foreach (var inParam in parameters)
            {
                query.Append($"@{inParam.Name}, ");
                queryParamsList.Add(new NpgsqlParameter
                { ParameterName = inParam.Name, Value = inParam.Value, NpgsqlDbType = inParam.Type });
            }

            // добавляем курсорную переменную
            var cursorValue = Guid.NewGuid().ToString("N");
            query.Append($"@{CursorName})");
            queryParamsList.Add(new NpgsqlParameter { ParameterName = CursorName, Value = cursorValue, NpgsqlDbType = NpgsqlDbType.Refcursor });


            await using var connection = BaseBuilder.GetConnection();
            await connection.OpenAsync();

            // Делаем все в транзакции
            await using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            // Вызываем функцию
            await using (var cmd = new NpgsqlCommand(query.ToString(), connection, transaction))
            {
                cmd.Parameters.AddRange(queryParamsList.ToArray());
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    cursorValue = reader.GetValue(0).ToString();
                }
            }

            // Извлекаем данные из курсора и мапим
            await using (var cmd = new NpgsqlCommand($"fetch all in \"{cursorValue}\"", connection, transaction))
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(mapper(reader));
                }
                await reader.CloseAsync();
            }

            await transaction.CommitAsync();
            await connection.CloseAsync();
            return result;
        }
    }
}
