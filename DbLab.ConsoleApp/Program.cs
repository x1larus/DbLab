using DbLab.DalPg.Managers;

namespace DbLab.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var manager = new TestTableManager();
            var data = manager.ReadAll();
            return;
        }
    }
}
