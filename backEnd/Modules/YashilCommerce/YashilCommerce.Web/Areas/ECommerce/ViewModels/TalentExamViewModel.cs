namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class TalentExamViewModel
    {
        /// <summary>
        /// کاربر آزمون را قبلا خریداری کرده است یا نه؟
        /// </summary>
        public bool HasUserPurchasedExam { get; set; }
        /// <summary>
        /// کاربر آزمون را قبلا شرکت کرده است یا نه؟
        /// </summary>
        public bool HasUserTakenExamBefore { get; set; }
        /// <summary>
        /// کاربر رزومه را قبلا پر کرده است یا نه؟
        /// </summary>
        public bool HasUserAlreadyFilledOutResume { get; set; }
        /// <summary>
        /// رزومه چند درصد پر شده است؟
        /// </summary>
        public int HowMuchIsResumeFilled { get; set; }
        /// <summary>
        /// در صورتی که درخواست نا موفق باشد پیام خطا در این پروپرتی قرار می گیرد
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// آدرسی که کاربر باید وارد آن شود تا وارد پنل سایت هیومکس شود
        /// </summary>
        public string Url { get; set; }
    }
}
