namespace DbLab.DalPg.Entities
{
    public class AccrualEntity
    {
        public long? Id { get; set; }

        public long? ParticipantId { get; set; }

        //public ParticipantEntity? ParticipantEntity { get; set; }

        public long? CategoryId { get; set; }

        //public CategoryEntity? CategoryEntity { get; set; }

        public decimal? Amount { get; set; }

        public string? Comment { get; set; }

        public DateTime? Date { get; set; }
    }
}
