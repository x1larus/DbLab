using DbLab.DalPg.Base;

namespace DbLab.DalPg.Entities.View
{
    [ViewName("public.v$accrual")]
    public class VAccrualEntity
    {
        [ViewColumn("id")]
        public long? Id { get; set; }

        [ViewColumn("type_name")]
        public string? TypeName { get; set; }

        [ViewColumn("category_name")]
        public string? CategoryName { get; set; }

        [ViewColumn("amount")]
        public decimal? Amount { get; set; }

        [ViewColumn("comment")]
        public string? Comment { get; set; }

        [ViewColumn("accrual_date")]
        public DateTime? AccrualDate { get; set; }
        
        [ViewColumn("is_income")]
        public bool? IsIncome { get; set; }
    }
}
