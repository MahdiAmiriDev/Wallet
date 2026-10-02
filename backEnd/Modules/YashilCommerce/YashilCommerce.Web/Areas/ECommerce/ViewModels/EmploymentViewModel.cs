namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class EmploymentViewModel
    {
        /// <summary>
        /// شناسه ی گروه دسته بندی اشتغال
        /// </summary>
        public int EmploymentCategoryGroupId { get; set; }


    }
    /// <summary>
    /// اطلاعات مربوط به سقف یارانه اشتغال
    /// </summary>
    public class EmploymentSubsidyInformationViewModel
    {
        /// <summary>
        /// سقف یارانه وزارت
        /// </summary>
        public int VezaratCeilingPerHekmatCard { get; set; }
        /// <summary>
        /// سقف یارانه ساتا
        /// </summary>
        public int SataCeilingPerHekmatCard { get; set; }
    }
}
