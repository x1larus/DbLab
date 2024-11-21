using DbLab.DalPg.Entities;
using DbLab.DalPgBase;

namespace DbLab.DalPg.Managers
{
    public class TestTableManager : PostgresManager
    {
        public List<TestTableEntity> ReadAll()
        {
            return ExecuteCursorFunction("public.test_table_read_all", reader => new TestTableEntity
            {
                Id = reader.GetFieldValue<long?>("id"),
                Data = reader.GetFieldValue<string>("data")
            });
        }
    }
}
