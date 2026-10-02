using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yashil.Core.CustomViewModels
{
    /// <summary>
    /// مدل مورد استفاده در مواقعی که به 
    /// Id , Title
    /// </summary>
    public class OptionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string  CategoryTitle { get; set; }
    }
}
