using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface ICustomerClubService : IGenericService<CustomerClub, int>
    {
        /// <summary>
        /// Get CustomerClub By cardNumber
        /// </summary>
        /// <param name="cardNumber">شماره کارت</param>
        /// <returns>CustomerClub</returns>
        CustomerClub GetByCardNumber(string cardNumber);

        /// <summary>
        /// بررسی می کند آیا کاربر در باشگاه مشتریان عضو است یا نه؟
        /// </summary>
        /// <param name="customerClubVerifyParameter"></param>
        /// <returns></returns>
        Task<bool> VerifyCustomerAsync(CustomerClubVerifyParameter customerClubVerifyParameter);

        /// <summary>
        /// موجود بودن کاربر در باشگاه مشتریان را بررسی میکند
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="nationalCode"></param>
        /// <param name="ServiceUrl"></param>
        /// <returns></returns>
        Task<bool> CheckExistKarafarinCustomerClub(string mobile, string nationalCode, string ServiceUrl);
        /// <summary>
        /// مشخص می کند که ایا پرداخت های این کارت بر اساس به صورت آفلاین و از کیف پول است یا نه؟
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        PayWithWalletDomainModel GetWalletPaymentInfoByCardNumber(string cardNumber);
        /// <summary>
        /// باشگاه مشتریان مربوط به هر اپلیکیشنی را برمی گیرداند
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <returns></returns>
        public IQueryable<CustomerClub> GetAllByApplicationId(int applicationId);

        IQueryable<CustomerClub> FindById(int id, bool readOnly = false);
        /// <summary>
        /// دریافت باشگاه مشتریان دارای قرار داد فعال
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="productSalesPlanningId">شناسه برنامه</param>
        /// <returns></returns>
        IQueryable<CustomerClub> GetAllCustomerClubHasActiveContract(int applicationId,int productSalesPlanningId);
    }
}      
 