
namespace Yashil.Core.CustomDtos.Representation
{
    public class RepresentationLocalDto
    {
        public int Id { get; set; }
        /// <summary>
        /// استان
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// شهر
        /// </summary>
        public string City { get; set; }
        /// <summary>
        ///         /// کد
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        ///         /// عنوان
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///         /// مؤسس
        ///</summary>
        public string Founder { get; set; }

        /// <summary>
        ///         /// تاریخ انقاضای مجوز
        ///</summary>
        public int LicenseExpireTime { get; set; }

    }

    public class RepresentationDetailDto
    {
        public int Id { get; set; }
        /// <summary>
        /// استان
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// شهر
        /// </summary>
        public string City { get; set; }
        /// <summary>
        ///         /// کد
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        ///       ///عنوان مرکز   
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        ///         /// عنوان
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///         /// مؤسس
        ///</summary>
        public string Founder { get; set; }

        /// <summary>
        ///         /// تاریخ انقاضای مجوز
        ///</summary>
        public int LicenseExpireTime { get; set; }
        /// <summary>
        ///        ///آدرس کامل   
        /// </summary>
        public string FullAddress { get; set; }

        /// <summary>
        ///         /// رایانامه (Email)
        ///</summary>
        public string Email { get; set; }
        /// <summary>
        ///         /// شماره تلفن
        ///</summary>
        public string Telephone { get; set; }
        /// <summary>
        ///         /// عرض جغرافیایی
        ///</summary>
        public double? Latitude { get; set; }
        /// <summary>
        ///         /// طول جغرافیایی
        ///</summary>
        public double? Longitude { get; set; }

    }
}
