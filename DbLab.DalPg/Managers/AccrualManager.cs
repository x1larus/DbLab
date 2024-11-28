using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class AccrualManager : PostgresManagerBase
    {
        public async Task<long> Write(AccrualEntity ent)
        {
            return await ExecuteFunction<long>(PgNames.FAccrualWrite,
                ("vp_id", ent.Id, NpgsqlDbType.Bigint),
                ("vp_id_accrual_category", ent.IdAccrualCategory, NpgsqlDbType.Bigint),
                ("vp_amount", ent.Amount, NpgsqlDbType.Numeric),
                ("vp_comment", ent.Comment, NpgsqlDbType.Varchar),
                ("vp_accrual_date", ent.AccrualDate, NpgsqlDbType.Date));
        }
    }
}
