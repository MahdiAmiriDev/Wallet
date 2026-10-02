
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface ICustomerClubCardService : IGenericService<CustomerClubCard, int>
    {

        IQueryable<CustomerClubCard> GetByCustomFilter(int? personId, int? customerClubId);
        /// <summary>
        /// Check Existence CustomerClubCard By Card Number
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub">مبتنی بر دیتابیس لوکال</param>
        /// <param name="cardNumber">شماره کارت</param>
        /// <param name="IsInstallmentsPaid">پرداخت اقساطی</param>
        /// <returns> true or false </returns>
        bool CheckExistenceByCardNumber(bool payWithWallet, bool isLocalClub, bool IsInstallmentsPaid, string cardNumber);
        /// <summary>
        /// Get CustomerClubCard By CustomerClubCard Code
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub"> مبتنی بر دیتابیس داخلی</param>
        /// <param name="code">کد</param>
        CustomerClubCard GetByCode(bool payWithWallet, bool isLocalClub, string cardNumber);
        /// <summary>
        /// Get CustomerClubCard By cardNumber
        /// </summary>
        /// <param name="cardNumber">شماره کارت</param>
        /// <returns>CustomerClub</returns>
        IQueryable<CustomerClubCard> GetByCardNumber(string cardNumber);

        ///// <summary>
        ///// UpdateCustomerClubCardAmountUsed
        ///// </summary>
        ///// <param name="customerClubCardId">شناسه کارت باشگاه مستریان</param>
        ///// <param name="price"></param>
        ///// <param name="saveAfterUpdate"></param>
        ///// <param name="amountUsed">مبلغ استفده شده</param>
        ///// <returns></returns>
        //Task<CustomerClubCard> UpdateCustomerClubCardAmountUsed(int customerClubCardId, decimal? price,
        //    bool saveAfterUpdate = false);
        /// <summary>
        /// با مشخصات موجود چک می کند که آیا شخصی با این مشخصات در باشگاه مشتریان پایگاه داده محلی وجود دارد یا نه
        /// </summary>
        /// <param name="customerClubVerifyParameter"></param>
        bool CheckExistenceByFilter(CustomerClubVerifyParameter customerClubVerifyParameter);

        /// <summary>
        /// Get ValidCard CustomerClubCard By CustomerClubCard Code
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub"> مبتنی بر دیتابیس داخلی</param>
        /// <param name="code">کد</param>
        Task<CustomerClubCard> GetValidCard(bool payWithWallet, bool isLocalClub, string code);
        
    }
}
