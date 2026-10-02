using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ProductSalesPlanningDiscountRepository : GenericApplicationBasedRepository<ProductSalesPlanningDiscount, int>, IProductSalesPlanningDiscountRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public ProductSalesPlanningDiscountRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<ProductSalesPlanningDiscount> GetByCustomFilter(int? productSalesPlanningId, int? customerClubId)
        {
            var query = GetAll(true);
            if (productSalesPlanningId.HasValue)
            {
                query = query.Where(x => x.ProductSalesPlanningId == productSalesPlanningId.Value);
            }
            if (customerClubId.HasValue)
            {
                query = query.Where(x => x.CustomerClubId == customerClubId.Value);
            }


            return query;
        }

        public Task<bool> CheckExistProductSalePlanningDiscountAsync(int customerClubId, int productSalePlanningId)
        {
            return DbSet.AsNoTracking()
                .AnyAsync(x =>
                    x.CustomerClubId == customerClubId
                    && x.ProductSalesPlanningId == productSalePlanningId);
        }

        public IQueryable<ProductSalesPlanningDiscount> GetAllByApplicationId(int applicationId, bool readOnly)
        {
            var query = DbSet
                .Where(x => x.ApplicationId == applicationId);
            return readOnly ? query.AsNoTracking() : query;
        }

        public IQueryable<ProductSalesPlanningDiscount> FindById(int id)
        {
            return DbSet.Where(x => x.Id == id)
                .AsNoTracking();
        }
    }
}
