namespace DbLab.DalPg.Entities
{
    public class IncomeTypeEntity
    {
        public long Id { get; set; }

        public string TypeName { get; set; }

        public override string ToString() => $"{Id} - {TypeName}";
    }
}
