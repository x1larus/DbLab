using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;

namespace DbLab.DalPg.Managers
{
    public class IncomeTypeManager : PostgresManagerBase
    {
        public List<IncomeTypeEntity> ReadAll()
        {
            return ExecuteCursorFunction("public.f$income_type_read_all", reader => new IncomeTypeEntity
            {
                Id = reader.GetFieldValue<long>("id"),
                TypeName = reader.GetFieldValue<string>("type_name")
            });
        }
    }
}
