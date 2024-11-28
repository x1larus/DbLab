namespace DbLab.DalPg.Entities
{
    public class AccrualCategoryEntity
    {
        public long? Id { get; set; }

        public long IdAccrualType { get; set; }

        public string CategoryName { get; set; }

        public override string ToString() => CategoryName;
    }
}
