namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class ResumeFillHelpViewModel
    {
        /// <summary>
        /// کاربر رزومه را قبلا پر کرده است یا نه؟
        /// </summary>
        public bool HasUserAlreadyFilledOutResume { get; set; }
        /// <summary>
        /// پیام
        /// </summary>
        public string Message { get; set; }
    }
}
