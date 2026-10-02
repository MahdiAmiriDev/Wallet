using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YashilCommerce.Core.DTO
{
    public class TopCategoryDto
    {
        /// <summary>
        /// تعداد دوره های این دسته بندی
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// عنوان دسته بندی
        /// </summary>
        public string Title { get; set; }
    }
}
