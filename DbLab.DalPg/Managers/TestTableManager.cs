using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using DbLab.DalPgBase;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class TestTableManager : PostgresManagerBase
    {
        public List<TestTableEntity> ReadAll()
        {
            return ExecuteCursorFunction("public.test_table_read_all", reader => new TestTableEntity
            {
                Id = reader.GetFieldValue<long?>("id"),
                Data = reader.GetFieldValue<string>("data"),
                Type = reader.GetFieldValue<int?>("type")
            });
        }

        public List<TestTableEntity> ReadByType(int type)
        {
            return ExecuteCursorFunction("public.test_table_read_by_type", reader => new TestTableEntity
            {
                Id = reader.GetFieldValue<long?>("id"),
                Data = reader.GetFieldValue<string>("data"),
                Type = reader.GetFieldValue<int?>("type")
            }, ("vp_type", type, NpgsqlDbType.Integer));
        }
    }
}
