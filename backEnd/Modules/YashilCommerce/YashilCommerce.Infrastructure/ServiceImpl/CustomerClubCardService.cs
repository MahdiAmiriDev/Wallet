using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CustomerClubCardService : GenericService<CustomerClubCard, int>, ICustomerClubCardService
    {
        private readonly IUnitOfWork _unitOfWork; private readonly IEventPublisher _eventPublisher;
        private readonly ICustomerClubCardRepository _customerClubCardRepository;
        private readonly IUserPrincipal _userPrincipal;
        public CustomerClubCardService(IUnitOfWork unitOfWork,
           ICustomerClubCardRepository customerClubCardRepository,

           IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
           : base(unitOfWork, customerClubCardRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _customerClubCardRepository = customerClubCardRepository;
            _userPrincipal = userPrincipal; _eventPublisher = eventPublisher;
        }
        /// <summary>
        /// Check Existence CustomerClubCard By Card Number
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub">مبتنی بر دیتابیس لوکال</param>
        /// <param name="cardNumber">شماره کارت</param>
        /// <param name="IsInstallmentsPaid">پرداخت اقساطی</param>
        /// <returns> true or false </returns>
        public bool CheckExistenceByCardNumber(bool payWithWallet, bool isLocalClub, bool IsInstallmentsPaid, string cardNumber)
        {
            return _customerClubCardRepository.CheckExistenceByCardNumber(payWithWallet, isLocalClub, IsInstallmentsPaid, cardNumber);
        }

        public bool CheckExistenceByFilter(CustomerClubVerifyParameter customerClubVerifyParameter)
        {
            return _customerClubCardRepository.CheckExistenceByFilter(customerClubVerifyParameter);
        }

        /// <summary>
        /// Get CustomerClubCard By cardNumber
        /// </summary>
        /// <param name="cardNumber">شماره کارت</param>
        /// <returns>CustomerClub</returns>
        public IQueryable<CustomerClubCard> GetByCardNumber(string cardNumber)
        {
            return _customerClubCardRepository.GetByCardNumber(cardNumber);
        }

        /// <summary>
        /// Get CustomerClubCard By CustomerClubCard Code
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub"> مبتنی بر دیتابیس داخلی</param>
        /// <param name="code">کد</param>
        public CustomerClubCard GetByCode(bool payWithWallet, bool isLocalClub, string code)
        {
            return _customerClubCardRepository.GetByCode(payWithWallet, isLocalClub, code);
        }

        public IQueryable<CustomerClubCard> GetByCustomFilter(int? personId, int? customerClubId)
        {
            return _customerClubCardRepository.GetByCustomFilter(personId, customerClubId);
        }

        /// <summary>
        /// Get ValidCard CustomerClubCard By CustomerClubCard Code
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub"> مبتنی بر دیتابیس داخلی</param>
        /// <param name="code">کد</param>
        public Task<CustomerClubCard> GetValidCard(bool payWithWallet, bool isLocalClub, string code)
        {
            return _customerClubCardRepository.GetValidCard(payWithWallet, isLocalClub, code);
        }

        ///// <summary>
        ///// UpdateCustomerClubCardAmountUsed
        ///// </summary>
        ///// <param name="customerClubCardId">شناسه کارت باشگاه مستریان</param>
        ///// <param name="price"></param>
        ///// <param name="saveAfterUpdate"></param>
        ///// <returns></returns>
        //public Task<CustomerClubCard> UpdateCustomerClubCardAmountUsed(int customerClubCardId, decimal? price,
        //    bool saveAfterUpdate = false)
        //{
        //    var customerClubCard = _customerClubCardRepository.Get(customerClubCardId);
        //    customerClubCard.AmountUsed = customerClubCard.AmountUsed + price;
        //    customerClubCard.MaximumNumberPurchases = customerClubCard.MaximumNumberPurchases - 1;
        //    customerClubCard.ModificationDate = DateTime.Now;
        //    customerClubCard.ModifyBy = _userPrincipal.Id;
        //    return UpdateAsync(customerClubCard, customerClubCardId, saveAfterUpdate);
        //}
    }
}
