using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface ITaxCategorySaleConfigurationService : IGenericService<TaxCategorySaleConfiguration, int>
    {
        IQueryable<TaxCategorySaleConfiguration> GetByCustomFilter( int? parentId, int? taxCategoryId, int? saleConfigurationId, int? factorReportType);



        /// <summary>
        /// دریافت دسته بندی مالیاتی که قبلا برای تنظیم فروش مقداری درج نشده
        /// </summary>
        /// <param name="saleConfigurationId"></param>
        /// <param name="taxCategoryId"></param>
        /// <returns></returns>
        Task<bool> CheckAnyTaskCategorySaleConfigurationInsertedBefore(int taxCategoryId, int saleConfigurationId);
    }
}
