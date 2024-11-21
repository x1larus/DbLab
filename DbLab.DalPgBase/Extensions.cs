using Npgsql;

namespace DbLab.DalPgBase
{
    public static class Extensions
    {
        public static T GetFieldValue<T>(this NpgsqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            return reader.GetFieldValue<T>(ordinal);
        }
    }
}
