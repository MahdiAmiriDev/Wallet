namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    /// <summary>
    /// این ویود مدل برای دریافت اطلاعات تصاویر تبلیغاتی سایت
    /// در صفحه ی هوم شامل 2 تصویر تبلیغاتی اول بعد از دسته بندی ها و 4 تصویر تبلیغ مهارت آموزی بعد از
    /// پرفروش ترین دوره ها کاربرد دارد و اطلاعات مربوط به آن ها را نگه  داری میکند
    /// </summary>
    public class HomePageAdvertisingImageViewModel
    {
        /// <summary>
        /// شناسه تصویر در حالت دکستاپ
        /// </summary>
        public int DeskTopModeAppDocumentId { get; set; }
        /// <summary>
        /// شناسه تصویر در حالت موبایل
        /// </summary>
        public int MobileModeAppDocumentId { get; set; }
        /// <summary>
        /// عنوان تصویر
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// زیر عنوان تصویر
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// آدرس یو ار ال تصویر
        /// </summary>
        public string Url { get; set; }

    }
}
