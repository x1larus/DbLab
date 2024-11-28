namespace DbLab.DalPg
{
    internal static class PgNames
    {
        #region Functions

        public static string FAccrualTypeReadAll = "public.f$accrual_type_read_all";
        public static string FAccrualTypeReadById = "public.f$accrual_type_read_by_id";
        public static string FAccrualCategoryReadByType = "public.f$accrual_category_read_by_type";
        public static string FAccrualCategoryReadAll = "public.f$accrual_category_read_all";
        public static string FAccrualWrite = "public.f$accrual_write";

        #endregion
    }
}
