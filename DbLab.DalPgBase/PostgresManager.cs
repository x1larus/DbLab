using System.Text;
using Npgsql;
using NpgsqlTypes;

namespace DbLab.DalPgBase
{
    public abstract class PostgresManager
    {
        private const string CursorName = "ref";
        protected const int BaseTimeout = 30;

        protected List<T> ExecuteCursorFunction<T>(string funcName, Func<NpgsqlDataReader, T> mapper,
            int commandTimeout = BaseTimeout, params (string name, object value, NpgsqlDbType type)[] parameters)
        {
        }
    }
}
