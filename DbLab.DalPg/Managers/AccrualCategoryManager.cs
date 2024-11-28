using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using Npgsql;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class AccrualCategoryManager : PostgresManagerBase
    {
        public async Task<List<AccrualCategoryEntity>> ReadByType(long idAccrualType) => await ExecuteCursorFunction(
            PgNames.FAccrualCategoryReadByType, Map, ("vp_id_accrual_type", idAccrualType, NpgsqlDbType.Bigint));

        public async Task<List<AccrualCategoryEntity>> ReadAll() =>
            await ExecuteCursorFunction(PgNames.FAccrualCategoryReadAll, Map);

        private static async Task<AccrualCategoryEntity> Map(NpgsqlDataReader reader) => new AccrualCategoryEntity
        {
            Id = await reader.GetFieldValueAsync<long>("id"),
            IdAccrualType = await reader.GetFieldValueAsync<long>("id_accrual_type"),
            CategoryName = await reader.GetFieldValueAsync<string>("category_name")
        };
    }
}
