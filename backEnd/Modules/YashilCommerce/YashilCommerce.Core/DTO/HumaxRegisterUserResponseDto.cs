namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// مقدار بازگشتی ار رجیستر در سایت هیومکس
    /// </summary>
    public class HumaxRegisterUserResponseDto
    {
        /// <summary>
        /// وضعیت درخواست
        /// true : موفق
        /// false : ناموفق
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// درصورتی که درخواست ناموفق باشد پیام خطا در این فیلد قرار می گیرد
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// آدرسی که کاربر باید وارد آن شود تا وارد پنل سایت هیومکس شود
        /// </summary>
        public string Url { get; set; }
    }
}
