using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface ITaxCategoryService : IGenericService<TaxCategory, int>
    {
        IQueryable<TaxCategory> FindById(int id);

        /// <summary>
        /// این متد
        ///TaxCategory
        /// هایی را بر میگردااند که
        /// برای آن ها در
        /// taxCategorySaleConfiguration
        /// مقداری درج نشده است
        /// </summary>
        /// <param name="saleConfigurationId"></param>
        /// <param name="taxCategorySaleConfigurationId"></param>
        /// <returns></returns>
        IQueryable<TaxCategory> GetTaxCategoryNotUsedInTaxCategorySaleConfiguration(int saleConfigurationId , int? taxCategorySaleConfigurationId);
    }
}
