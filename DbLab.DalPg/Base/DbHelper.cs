using Npgsql;

namespace DbLab.DalPg.Base
{
    public static class DbHelper
    {
        //todo: спрятать строку подключения
        private static NpgsqlDataSource DataSource { get; set; } = NpgsqlDataSource.Create("Host=79.137.204.140;Port=5432;Username=db_lab_app;Password=db_lab_app;Database=db_lab");

        public static async ValueTask<NpgsqlConnection> CreateOpenedConnectionAsync() => await DataSource.OpenConnectionAsync();
    }
}
