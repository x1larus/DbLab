using Npgsql;

namespace DbLab.DalPgBase
{
    public static class BaseBuilder
    {
        private static readonly string ConnectionString = "Host=79.137.204.140;Port=5432;Username=db_lab_app;Password=db_lab_app;Database=db_lab";
        public static readonly NpgsqlDataSource DataSource = NpgsqlDataSource.Create(ConnectionString);

        public static NpgsqlConnection GetConnection()
        {
            return DataSource.CreateConnection();
        }
    }
}
