using DbLab.DalPg.Managers;
using DbLab.DalPgBase;

namespace DbLab.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Host=79.137.204.140;Port=5432;Username=db_lab_app;Password=db_lab_app;Database=db_lab";
            DbManager.InitializeDb(connectionString);
        }
    }
}
