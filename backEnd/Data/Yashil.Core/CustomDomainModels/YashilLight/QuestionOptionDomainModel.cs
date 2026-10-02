namespace Yashil.Core.CustomDomainModels.YashilLight
{
    public class QuestionOptionDomainModel
    {
        /// <summary>
        /// شناسه گزینه سوال
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// سوال
        ///</summary>
        public int QuestionId { get; set; }
        /// <summary>
        /// متن گزینه
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// متن گزینه
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// متن گزینه
        /// </summary>
        public string SoundUrl { get; set; }
        /// <summary>
        /// آیا این گزینه توسط کاربر در هنگام شرکت در آزمون انتخاب شده است ؟
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// نوع محتوای گزینه سوال
        /// </summary>
        public int QuestionOptionContentType { get; set; }
    }
}
