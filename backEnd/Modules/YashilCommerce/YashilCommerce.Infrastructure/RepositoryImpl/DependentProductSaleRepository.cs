using System.Collections.Generic;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class DependentProductSaleRepository : GenericApplicationBasedRepository<DependentProductSale, int>, IDependentProductSaleRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public DependentProductSaleRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<DependentProductSale> GetByCustomFilter(int? mainProduct, int? relatedProduct)
        {
            var query = GetAll(true);
            if (mainProduct.HasValue)
            {
                query = query.Where(x => x.MainProductSalePlanningId == mainProduct.Value);
            }
            if (relatedProduct.HasValue)
            {
                query = query.Where(x => x.RelatedProductSalePlanningId == relatedProduct.Value);
            }


            return query;
        }

        public void UpdateMainProductDependentProducts(List<int> dependentProducts, int mainProductSalePlanningId)
        {
            var dependentProductSales = DbSet.Where(x => x.MainProductSalePlanningId == mainProductSalePlanningId);

        }
    }
}
