using Npgsql;

namespace DbLab.DalPgBase
{
    public static class DbManager
    {
        private static NpgsqlDataSource? DataSource;

        public static void InitializeDb(string connectionString)
        {
            DataSource = NpgsqlDataSource.Create(connectionString);
        }

        public static async ValueTask<NpgsqlConnection> GetConnectionAsync()
        {
            return await DataSource.OpenConnectionAsync();
        }
    }
}
