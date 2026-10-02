using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface ICustomerClubRepository : IGenericRepository<CustomerClub,int>
    {
        /// <summary>
        /// Get CustomerClub By cardNumber
        /// </summary>
        /// <param name="cardNumber">شماره کارت</param>
        /// <returns>CustomerClub</returns>
        CustomerClub GetByCardNumber(string cardNumber);
        /// <summary>
        /// باشگاه مشتریان مربوط به هر اپلیکیشنی را برمی گیرداند
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <returns></returns>
        IQueryable<CustomerClub> GetAllByApplicationId(int applicationId);

        IQueryable<CustomerClub> FindById(int id, bool readOnly);
        /// <summary>
        /// دریافت باشگاه مشتریان دارای قرار داد فعال
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="productSalesPlanningId">شناسه برنامه</param>
        /// <returns></returns>
        IQueryable<CustomerClub> GetAllCustomerClubHasActiveContract(int applicationId, int productSalesPlanningId);
    }
}      
 