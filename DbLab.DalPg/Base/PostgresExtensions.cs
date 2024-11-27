using Npgsql;

namespace DbLab.DalPg.Base
{
    public static class PostgresExtensions
    {
        public static T GetFieldValue<T>(this NpgsqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            return reader.GetFieldValue<T>(ordinal);
        }
    }
}
