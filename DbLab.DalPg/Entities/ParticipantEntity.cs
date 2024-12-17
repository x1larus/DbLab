namespace DbLab.DalPg.Entities
{
    public class ParticipantEntity
    {
        public long? Id { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public decimal? Expenses { get; set; }

        public decimal? Income { get; set; }

        public decimal? Balance { get; set;}
    }
}
