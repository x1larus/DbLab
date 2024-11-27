using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;

namespace DbLab.DalPg.Managers
{
    public class IncomeTypeManager : PostgresManagerBase
    {
        public async Task<List<IncomeTypeEntity>> ReadAll()
        {
            return await ExecuteCursorFunction(PgNames.FIncomeTypeReadAll, async reader => new IncomeTypeEntity
            {
                Id = await reader.GetFieldValueAsync<long>("id"),
                TypeName = await reader.GetFieldValueAsync<string>("type_name")
            });
        }
    }
}
