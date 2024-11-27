using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class IncomeManager : PostgresManagerBase
    {
        public List<IncomeEntity> ReadAll()
        {
            return ExecuteCursorFunction("public.income_get_all", reader => new IncomeEntity
            {
                Id = reader.GetFieldValue<long>("id"),
                Date = reader.GetFieldValue<DateTime>("date"),
                Summ = reader.GetFieldValue<decimal>("summ"),
                Comment = reader.GetFieldValue<string?>("comment"),
                IdIncomeType = reader.GetFieldValue<long>("id_income_type"),
                IncomeTypeName = reader.GetFieldValue<string?>("type_name")
            });
        }

        public void Write(IncomeEntity ent)
        {
            var res = ExecuteFunction<long?>("public.income_write",
                ("vp_id", ent.Id, NpgsqlDbType.Bigint),
                ("vp_date", ent.Date, NpgsqlDbType.Date),
                ("vp_summ", ent.Summ, NpgsqlDbType.Numeric),
                ("vp_comment", ent.Comment, NpgsqlDbType.Varchar),
                ("vp_id_income_type", ent.IdIncomeType, NpgsqlDbType.Bigint));
            ent.Id = res;
        }
    }
}
