namespace YashilCommerce.Core.Classes
{
    /// <summary>
    /// نتیجه اعتبار سنجی گروه بندی فاکتور
    /// </summary>
    public class FactorGroupValidateResultParam
    {
        /// <summary>
        /// گروه بندی فاکتور معتبر می باشد
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// پیام خطا در صورت وجود خطا
        /// </summary>
        public string Message { get; set; }
        
    }
}
