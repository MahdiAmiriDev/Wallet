using System;

namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class CategoryCustomModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        /// <summary>
        ///         /// عنوان
        ///</summary>
        public string Title { get; set; }
        public string? CssClass { get; set; }
        /// <summary>
        ///         زمان تغییر
        ///</summary>
        public DateTime? ModificationDate { get; set; }

        /// <summary>
        /// آیا دسته بندی سطح اول است؟
        /// </summary>
        public bool IsLevelOneCategory { get; set; }
    }
}
