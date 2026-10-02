using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.CustomViewModels.News
{
    public class NewsDetailViewModel 
    {
        /// <summary>
        /// شناسه خبر
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان خبر
        /// </summary>
        [StringValidation] public string Title { get; set; }
        /// <summary>
        /// خلاصه خبر
        /// </summary>
        public string Led { get; set; }
        /// <summary>
        ///         /// متن خبر
        ///</summary>
        public string Body { get; set; }
        /// <summary>
        ///         /// نوع خبر
        ///</summary>
        public int NewsType { get; set; }
        /// <summary>
        ///         /// زیرتیتر
        ///</summary>
        public string Deck { get; set; }
        /// <summary>
        ///         /// روتیتر
        ///</summary>
        public string Kicker { get; set; }
        /// <summary>
        ///         /// خلاصه تیتر(سوتیتر)
        ///</summary>
        public string SubTitle { get; set; }
        /// <summary>
        ///         /// کلمات کلیدی
        ///</summary>
        public string KeywordStr { get; set; }
    }
}
