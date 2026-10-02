namespace Yashil.Core.CustomDomainModels
{
    public class ProductSimpleDomainModel
    {
        /// <summary>
        /// شناسه محصول
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// کد اصلی محصول
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// عنوان دوره
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// توضیحات محصول
        /// </summary>
        public string FullDescription { get; set; }
        /// <summary>
        /// گروه عملی یا نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }
        /// <summary>
        /// نام کاربری دانشجو
        /// </summary>
        public string StudentUserName { get; set; }
        /// <summary>
        /// نام کاربری مربی یا استاد
        /// </summary>
        public string TeacherUserName { get; set; }
        /// <summary>
        /// وضعیت انتشار
        /// </summary>
        public string Published { get; set; }
        /// <summary>
        /// عنوان دسته بندی
        /// </summary>
        public string CategoryTitle { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public int StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public int EndDate { get; set; }
        /// <summary>
        /// حداکثر ظرفیت
        /// </summary>
        public int MaxCapacity { get; set; }
    }
}
