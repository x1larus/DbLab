using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using Npgsql;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class AccrualTypeManager : PostgresManagerBase
    {
        public async Task<List<AccrualTypeEntity>> ReadAll() =>
            await ExecuteCursorFunction(PgNames.FAccrualTypeReadAll, Map);

        public async Task<AccrualTypeEntity?> ReadById(long typeId) =>
            (await ExecuteCursorFunction(PgNames.FAccrualTypeReadById,
                Map,
                ("vp_id", typeId, NpgsqlDbType.Bigint))).FirstOrDefault();

        private static async Task<AccrualTypeEntity> Map(NpgsqlDataReader reader) => new AccrualTypeEntity
        {
            Id = await reader.GetFieldValueAsync<long>("id"),
            TypeName = await reader.GetFieldValueAsync<string>("type_name"),
            IsIncome = await reader.GetFieldValueAsync<bool>("is_income")
        };
    }
}
