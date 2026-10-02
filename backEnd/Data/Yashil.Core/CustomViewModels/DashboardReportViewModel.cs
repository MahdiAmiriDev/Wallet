using System.Collections.Generic;
using Yashil.Common.Core.Attributes;

namespace Yashil.Core.CustomViewModels
{
    /// <summary>
    /// this Class Use For Present Report Or Dashboard  
    /// </summary>
    public class StoreCustomViewModel
    {
        public StoreCustomViewModel()
        {
            Groups = new List<int>();
        }

        public List<int> Groups { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
        [StringValidation] public string CssClass { get; set; }

        public byte[] Picture { get; set; }

        [StringValidation] public string Color { get; set; }

        [StringValidation] public string Description { get; set; }
    }
}