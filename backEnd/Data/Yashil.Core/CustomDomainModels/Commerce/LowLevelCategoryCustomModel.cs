namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class LowLevelCategoryCustomModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        /// <summary>
        ///عنوان
        ///</summary>
        public string Title { get; set; }
        public string? CssClass { get; set; }
        /// <summary>
        /// عنوان پدر
        /// </summary>
        public string ParentTitle { get; set; }

    }
}
