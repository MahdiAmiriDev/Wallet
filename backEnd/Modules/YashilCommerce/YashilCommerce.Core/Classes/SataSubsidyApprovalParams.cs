namespace YashilCommerce.Core.Classes
{
    public class SataSubsidyApprovalData
    {
        /// <summary>
        /// توکن در این فیلد قرار می گیرد
        /// </summary>
        public string Access_token { get; set; }
        /// <summary>
        /// نوع توکن که از نوع  هست در این فیلد قرار می گیرد
        /// </summary>
        public string Token_type { get; set; }
        public string Refresh_token { get; set; }
        /// <summary>
        /// زمان انقضا توکن در این فیلد قرار می گیرد
        /// </summary>
        public int Expires_in { get; set; }
        public string Scope { get; set; }
    }

    public class SataSubsidyApprovalMessage
    {
        public SataSubsidyApprovalData Data { get; set; }
        public int Total { get; set; }
    }

    public class SataSubsidyApprovalLoginParams
    {
        public bool DevelopMode { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// نتیجه لاگین به سرویس ساتا
        /// </summary>
        public SataSubsidyApprovalMessage Message { get; set; }
    }
    public class SataCheckFamilyMembershipRoot
    {
        public bool DevelopMode { get; set; }
        /// <summary>
        /// وضعیت استعلام یارانه 
        /// اگر 0 باشد یعنی استعلام موفقیت آمیز بود
        /// اگر مخالف 0 باشد استعلام با موفقیت انجام نشده 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// نتیجه استعلام یارانه ساتا
        /// </summary>
        public SataCheckFamilyMembershipMessage Message { get; set; }
    }
    public class SataCheckFamilyMembershipParam
    {
        /// <summary>
        ///  کد ملی شخصی که برای استعلام دریافت یارانه به سرویس استعلام یارانه ارسال میشود
        /// </summary>
        public string nationalCode { get; set; }
        /// <summary>
        /// شماره شناسایی فرد
        /// </summary>
        public string identificationCode { get; set; }
    }

    public class SataCheckFamilyMembershipMessage
    {
        /// <summary>
        ///  نتیجه استعلام یارانه یعنی میتواند یارانه بگیرد یا خیر
        /// </summary>
        public bool Data { get; set; }
        public int Total { get; set; }
    }

}
