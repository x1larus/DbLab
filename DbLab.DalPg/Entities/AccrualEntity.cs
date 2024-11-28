namespace DbLab.DalPg.Entities
{
    public class AccrualEntity
    {
        public long? Id { get; set; }

        public long IdAccrualCategory { get; set; }

        public decimal Amount { get; set; }

        public string? Comment { get; set; }

        public DateOnly AccrualDate { get; set; }
    }
}
