using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class IncomeManager : PostgresManagerBase
    {
        public async Task<List<IncomeEntity>> ReadAll()
        {
            return await ExecuteCursorFunction(PgNames.FIncomeReadAll, async reader => new IncomeEntity
            {
                Id = await reader.GetFieldValueAsync<long>("id"),
                Date = await reader.GetFieldValueAsync<DateTime>("date"),
                Summ = await reader.GetFieldValueAsync<decimal>("summ"),
                Comment = await reader.GetFieldValueAsync<string?>("comment"),
                IdIncomeType = await reader.GetFieldValueAsync<long>("id_income_type"),
                IncomeTypeName = await reader.GetFieldValueAsync<string?>("type_name")
            });
        }

        public async void Write(IncomeEntity ent)
        {
            var res = await ExecuteFunction<long?>(PgNames.FIncomeWrite,
                ("vp_id", ent.Id, NpgsqlDbType.Bigint),
                ("vp_date", ent.Date, NpgsqlDbType.Date),
                ("vp_summ", ent.Summ, NpgsqlDbType.Numeric),
                ("vp_comment", ent.Comment, NpgsqlDbType.Varchar),
                ("vp_id_income_type", ent.IdIncomeType, NpgsqlDbType.Bigint));
            ent.Id = res;
        }
    }
}
