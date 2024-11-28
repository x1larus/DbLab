namespace DbLab.DalPg.Entities
{
    public class AccrualTypeEntity
    {
        public long? Id { get; set; }

        public string? TypeName { get; set; }

        public bool? IsIncome { get; set; }

        public override string ToString() => TypeName;
    }
}
