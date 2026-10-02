using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface ICategoryCustomerClubService : IGenericService<CategoryCustomerClub,int>
    {
                 
        IQueryable<CategoryCustomerClub> GetByCustomFilter( int? customerClubId, int? categoryId, int? applicationId);
        /// <summary>
        /// این متد ابتدا بررسی میکند که 
        /// برای آیدی دسته بندی پاس داده شده categoryCustomerClub
        /// درج شده است یا خیر
        /// اگر درج نشده بود
        /// بازای تمام customerClub های موجود در سامانه
        /// برای دسته بندی مذکور CategoryCustomerClub درج میکند
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<string> CheckExistsCategoryCustomerClubAndAddIfNeededByCategoryIdAsync(int categoryId);
    }
}      
 