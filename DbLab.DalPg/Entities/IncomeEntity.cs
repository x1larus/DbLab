namespace DbLab.DalPg.Entities
{
    public class IncomeEntity
    {
        public long? Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Summ { get; set; }

        public string? Comment { get; set; }

        public long IdIncomeType { get; set; }

        public string? IncomeTypeName { get; set; }
    }
}
