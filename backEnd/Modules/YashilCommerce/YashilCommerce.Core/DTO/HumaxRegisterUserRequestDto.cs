namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// اطلاعات ارسالی برای رجیستر در سایت هیومکس 
    /// </summary>
    public class HumaxRegisterUserRequestDto
    {
        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// شماره موبایل / نام کاربری
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// ایمیل
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// کلید خصوصی
        /// </summary>
        public string APIKey { get; set; }
        /// <summary>
        ///  نام همکار
        /// </summary>
        public string PartnerName { get; set; }
    }
}
