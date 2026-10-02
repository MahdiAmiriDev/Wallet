using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CategoryGroupRepository : GenericApplicationBasedRepository<CategoryGroup, int>, ICategoryGroupRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IProductSalesPlanningRepository _productSalesPlanningRepository;
        public CategoryGroupRepository(YashilAppDbContext context, IUserPrincipal userPrincipal, IProductSalesPlanningRepository productSalesPlanningRepository) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
            _productSalesPlanningRepository = productSalesPlanningRepository;
        }


        public IQueryable<CategoryGroup> GetCategoryGroupByApplicationId(int employmentCategoryGroupId,
            int applicationId,
            int customerClubId)
        {
            return _context.CategoryGroup.Where(ca =>
                    ca.ApplicationId == applicationId &&
                    (
                        _context.ProductSalesPlanning
                            .Where(sp =>
                                sp.Product.DefaultCategory.CategoryGroupId == ca.Id ||
                                (ca.Id == employmentCategoryGroupId &&
                                 sp.Product.ProductType == (int)ProductTypeEnum.Employment))
                            .Any(_productSalesPlanningRepository
                                .GetProductSalePlanningIsActiveFilter(
                                    applicationId,
                                    customerClubId,
                                    null,
                                    false))

                    )
                )
                .AsNoTracking();
        }

        public IQueryable<CategoryGroup> FindById(int id ,int? applicationId, bool readOnly = true)
        {
            return GetAll(readOnly, applicationId).Where(x => x.Id == id).AsNoTracking();
        }
    }
}
