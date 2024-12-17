using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;

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
    } 
}
