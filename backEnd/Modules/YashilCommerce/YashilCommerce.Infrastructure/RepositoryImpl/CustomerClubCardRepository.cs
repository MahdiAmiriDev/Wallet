using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CustomerClubCardRepository : GenericRepository<CustomerClubCard, int>, ICustomerClubCardRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public CustomerClubCardRepository(YashilAppDbContext context, IUserPrincipal userPrincipal
            , ApplicationConfiguration applicationConfiguration
            ) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }
        /// <summary>
        /// Check Existence CustomerClubCard By Card Number
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub">مبتنی بر دیتابیس لوکال</param>
        /// <param name="cardNumber">شماره کارت</param>
        /// <param name="isInstallmentsPaid">پرداخت اقساطی</param>
        /// <returns> true or false </returns>
        public bool CheckExistenceByCardNumber(bool payWithWallet, bool isLocalClub, bool isInstallmentsPaid, string cardNumber)
        {
            return DbSet.Any(x => x.CardNumber == cardNumber);
        }
        /// <summary>
        /// Get CustomerClubCard By CustomerClubCard Code
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub"> مبتنی بر دیتابیس داخلی</param>
        /// <param name="code">کد</param>
        public CustomerClubCard GetByCode(bool payWithWallet, bool isLocalClub, string code)
        {
            return DbSet.FirstOrDefault(x => x.CustomerClub.PayWithWallet == payWithWallet && x.CustomerClub.IsLocalClub == isLocalClub && x.Code == code);
        }

        public IQueryable<CustomerClubCard> GetByCustomFilter(int? personId, int? customerClubId)
        {
            var query = GetAll(true);
            if (personId.HasValue)
            {
                query = query.Where(x => x.CustomerClubCardPersonId == personId.Value);
            }
            if (customerClubId.HasValue)
            {
                query = query.Where(x => x.CustomerClubId == customerClubId.Value);
            }


            return query;
        }

        /// <summary>
        /// Get CustomerClubCard By cardNumber
        /// </summary>
        /// <param name="cardNumber">شماره کارت</param>
        /// <returns>CustomerClub</returns>
        public IQueryable<CustomerClubCard> GetByCardNumber(string cardNumber)
        {
            return _context.CustomerClubCard.Where(x => x.CardNumber == cardNumber && x.CustomerClub.Deleted != true &&
            x.CustomerClub.ApplicationCustomerClub.Any(c => c.ApplicationId == _applicationConfiguration.ApplicationId));
        }


        /// <summary>
        /// Get ValidCard CustomerClubCard By CustomerClubCard Code
        /// </summary>
        /// <param name="payWithWallet">پرداخت با کیف پول</param>
        /// <param name="isLocalClub"> مبتنی بر دیتابیس داخلی</param>
        /// <param name="code">کد</param>
        public Task<CustomerClubCard> GetValidCard(bool payWithWallet, bool isLocalClub, string code)
        {
            return DbSet.FirstOrDefaultAsync(x => x.CustomerClub.PayWithWallet == payWithWallet &&
            x.CustomerClub.IsLocalClub == isLocalClub && x.Code == code && x.CustomerClubId == 5);
        }

        
        public bool CheckExistenceByFilter(CustomerClubVerifyParameter customerClubVerifyParameter)
        {
            if (customerClubVerifyParameter.CustomerClubId.HasValue)
            {
                var customerClub = _context.CustomerClub.Include(x => x.AdditionalUserProp).FirstOrDefault(x => x.Id == customerClubVerifyParameter.CustomerClubId);
                if (customerClub != null)
                {
                    var query = DbSet.Where(x => x.CustomerClubId == customerClubVerifyParameter.CustomerClubId);
                    if (customerClub.HasCustomCode)
                    {
                        query = query.Where(x => x.CardNumber == customerClubVerifyParameter.CardNumber);
                    }
                    if (customerClub.HasNationalCode)
                    {
                        query = query.Where(x => x.CustomerClubCardPerson != null && x.CustomerClubCardPerson.NationalCode == customerClubVerifyParameter.NationalCode);
                    }
                    if (customerClub.HasMobileNumber)
                    {
                        query = query.Where(x => x.CustomerClubCardPerson.MobileNumber == customerClubVerifyParameter.MobileNumber);
                    }

                    return query.Count() > 0;
                    
                }
            }
            return false;
        }
    }
}
