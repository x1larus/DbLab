using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;
using NpgsqlTypes;

namespace DbLab.DalPg.Managers
{
    public class ParticipantManager : PostgresManagerBase
    {
        public async Task<List<ParticipantEntity>> ReadAll()
        {
            return await ExecuteCursorFunction("public.f$participants_read_all",
                async reader => new ParticipantEntity
                {
                    Id = await reader.GetFieldValueAsync<long?>("id"),
                    Name = await reader.GetFieldValueAsync<string?>("name"),
                    PhoneNumber = await reader.GetFieldValueAsync<string?>("phone_number"),
                    Expenses = await reader.GetFieldValueAsync<decimal?>("expenses"),
                    Income = await reader.GetFieldValueAsync<decimal?>("income"),
                    Balance = await reader.GetFieldValueAsync<decimal?>("balance")
                });
        }

        public async Task<long?> Write(ParticipantEntity ent)
        {
            return await ExecuteFunction<long?>("public.f$participants_write", ("vp_id", ent.Id, NpgsqlDbType.Bigint),
                ("vp_name", ent.Name, NpgsqlDbType.Varchar),
                ("vp_phone_number", ent.PhoneNumber, NpgsqlDbType.Varchar));
        }
    }
}
