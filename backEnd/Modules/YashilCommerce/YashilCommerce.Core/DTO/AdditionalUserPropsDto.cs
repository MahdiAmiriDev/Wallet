namespace YashilCommerce.Core.DTO
{

    public class AdditionalUserPropsDto
    {
        public int CustomerClubId { get; set; }
        /// <summary>
        /// آیا پرداخت حکمت کارت دارد
        /// </summary>
        public bool HekmatBased { get; set; }
        /// <summary>
        /// مبتنی بر یارانه می باشد
        /// </summary>
        public bool SupportSubsidy { get; set; }
        /// <summary>
        /// مبتنی بر آدرس می باشد
        /// </summary>
        public bool AddressBased { get; set; }
        /// <summary>
        /// نوع ثبت نام
        /// </summary>
        public int RegistrationModeType { get; set; }
        /// <summary>
        /// آیا کاربر در آزمون استعدایابی شرکت کرده است
        /// </summary>
        public bool ParticipatedInhumaxTest { get; set; }
        /// <summary>
        /// آیا کاربر آزمون استعداد یابی شغلی را خریده است
        /// </summary>
        public bool? HasUserPurchasedEmploymentExam { get; set; }
        /// <summary>
        /// آیا کاربر امکان خرید اقساطی دارد یا خیر
        /// </summary>
        public bool InstallmentBased { get; set; }
        /// <summary>
        /// آیا رزومه را کامل پر کرده است
        /// </summary>
        public bool IsResumeFillOutEnough { get; set;}
    }
}
