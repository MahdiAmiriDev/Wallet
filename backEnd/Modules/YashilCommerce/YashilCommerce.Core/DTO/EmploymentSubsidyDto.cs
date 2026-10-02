
namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// مدل تعلق گرفتن یارانه اشتغال به کاربر
    /// </summary>
    public class EmploymentSubsidyDto
    {
        /// <summary>
        /// آیا کاربرآزمون را خریده است؟
        /// </summary>
        public bool? HasUserPurchasedExam { get; set; }
        /// <summary>
        /// آیا کاریر در آزمون شرکت کرده است؟
        /// </summary>
        public bool? HasUserTakenExamBefore { get; set; }
        /// <summary>
        /// آیا دوره منجر به اشتغال است؟
        /// </summary>
        public bool? IsEmploymentProduct { get; set; }
        /// <summary>
        /// آیا یارانه اشتغال تعلق می گیرد یا نه؟
        /// </summary>
        public bool? IsEmploymentSubsidyGrantedToUser { get; set; }

    }
}
