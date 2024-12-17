using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class AccrualManager : PostgresManagerBase
    {
        public async Task<long?> Write(AccrualEntity ent)
        {
            return await ExecuteFunction<long?>("public.f$accruals_write", ("vp_id", ent.Id, NpgsqlDbType.Bigint),
                ("vp_category_id", ent.CategoryId, NpgsqlDbType.Bigint),
                ("vp_participant_id", ent.ParticipantId, NpgsqlDbType.Bigint),
                ("vp_amount", ent.Amount, NpgsqlDbType.Numeric),
                ("vp_date", ent.Date, NpgsqlDbType.Timestamp),
                ("vp_comment", ent.Comment, NpgsqlDbType.Varchar));
        }

        public async Task<List<AccrualEntity>> ReadAll(/*bool wc = true*/)
        {
            var res = await ExecuteCursorFunction("public.f$accruals_read_all", async reader => new AccrualEntity
            {
                Id = await reader.GetFieldValueAsync<long?>("id"),
                ParticipantId = await reader.GetFieldValueAsync<long?>("participant_id"),
                CategoryId = await reader.GetFieldValueAsync<long?>("category_id"),
                Amount = await reader.GetFieldValueAsync<decimal?>("amount"),
                Comment = await reader.GetFieldValueAsync<string?>("comment"),
                Date = await reader.GetFieldValueAsync<DateTime?>("date")
            });

            //if (!wc) return res;

            //var p = await new ParticipantManager().ReadAll();
            //var c = await new CategoryManager().ReadAll();

            //foreach (var x in res)
            //{
            //    x.ParticipantEntity = p.FirstOrDefault(el => el.Id == x.ParticipantId);
            //    x.CategoryEntity = c.FirstOrDefault(el => el.Id == x.CategoryId);
            //}

            return res;
        }
    }
}
