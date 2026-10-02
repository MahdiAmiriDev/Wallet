using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yashil.Core.CustomDtos.LocaleStringResource
{
    public  class LocaleStringResourceDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///نام منبع
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// زبان
        /// </summary>
        public string languageTitle {  get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        public string Value { get; set; }
    }
}
