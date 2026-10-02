using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    
    public class ProductTagsRepository : GenericApplicationBasedRepository<ProductTags, int>, IProductTagsRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		
        public ProductTagsRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        
        
        public IQueryable<ProductTags> GetByCustomFilter( int? parentId, int? productId, int? tagType)
        {
            var query = GetAll(true);
            if(parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }
            
            if(productId.HasValue)
            {
                query = query.Where(x => x.ProductId == productId.Value);
            }
            
            if(tagType.HasValue)
            {
                query = query.Where(x => x.TagType == tagType.Value);
            }
            
            return query;
        }

        /// <summary>
        /// دریافت تگ های مرتبط با برنامه ریزی دوره
        /// </summary>
        /// <param name="productSsalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns>ProductTags</returns>
        public IQueryable<ProductTags> GetByProductSalesPlanningId(int productSsalesPlanningId)
        {
            return DbSet.Where(ptag => ptag.Product.ProductSalesPlanning.Any(psp => psp.Id == productSsalesPlanningId));
        }
         
    }
}      
