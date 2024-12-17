using DbLab.DalPg.Base;
using DbLab.DalPg.Entities;

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
    }
}
