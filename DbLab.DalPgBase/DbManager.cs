using Npgsql;

namespace DbLab.DalPgBase
{
    public static class DbManager
    {
        private static readonly NpgsqlDataSource DataSource = NpgsqlDataSource.Create("Host=79.137.204.140;Port=5432;Username=db_lab_app;Password=db_lab_app;Database=db_lab");

        public static NpgsqlConnection GetConnection()
        {
            return DataSource.OpenConnection();
        }
    }
}
