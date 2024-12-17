using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class CategoryManager : PostgresManagerBase
    {
        public async Task<List<CategoryEntity>> ReadAll()
        {
            return await ExecuteCursorFunction("public.f$categories_read_all", async reader => new CategoryEntity
            {
                Id = await reader.GetFieldValueAsync<long?>("id"),
                IsIncome = await reader.GetFieldValueAsync<bool?>("is_income"),
                Name = await reader.GetFieldValueAsync<string?>("name")
            });
        }

        public async Task<long?> Write(CategoryEntity ent)
        {
            return await ExecuteFunction<long?>("public.f$categories_write", ("vp_id", ent.Id, NpgsqlDbType.Bigint),
                ("vp_is_income", ent.IsIncome, NpgsqlDbType.Boolean), ("vp_name", ent.Name, NpgsqlDbType.Varchar));
        }
    } 
}
