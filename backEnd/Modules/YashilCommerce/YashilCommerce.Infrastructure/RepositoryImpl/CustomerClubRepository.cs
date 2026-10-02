using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;


namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CustomerClubRepository : GenericRepository<CustomerClub, int>, ICustomerClubRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public CustomerClubRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        /// <summary>
        /// باشگاه مشتریان مربوط به هر اپلیکیشنی را برمی گیرداند
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <returns></returns>
        public IQueryable<CustomerClub> GetAllByApplicationId(int applicationId)
        {
            return _context.ApplicationCustomerClub.Where(x => x.ApplicationId == applicationId).Select(x => x.CustomerClub).AsNoTracking();
        }

        /// <summary>
        /// Get CustomerClub By cardNumber
        /// </summary>
        /// <param name="cardNumber">شماره کارت</param>
        /// <returns>CustomerClub</returns>
        public CustomerClub GetByCardNumber(string cardNumber)
        {
            return _context.CustomerClubCard.Where(x => x.CardNumber == cardNumber && x.CustomerClub.Deleted != true)
                .Select(c => c.CustomerClub).FirstOrDefault();
        }

        public IQueryable<CustomerClub> FindById(int id, bool readOnly)
        {
            return GetAll(readOnly).Where(x => x.Id == id);
        }

        public IQueryable<CustomerClub> GetAllCustomerClubHasActiveContract(int applicationId, int productSalesPlanningId)
        {
            return DbSet.Where(x =>
                    x.SaleConfigurationAttribute.Any(c =>
                        c.IsActive && c.ApplicationId == applicationId
                        && c.SaleConfiguration.ProductSalesPlanning
                            .Any(psp=>psp.Id == productSalesPlanningId)
                    ))
                .AsNoTracking();
        }
    }
}
