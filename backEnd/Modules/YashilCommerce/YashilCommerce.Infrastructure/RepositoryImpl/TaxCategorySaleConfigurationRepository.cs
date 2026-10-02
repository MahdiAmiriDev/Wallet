using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    
    public class TaxCategorySaleConfigurationRepository : GenericApplicationBasedRepository<TaxCategorySaleConfiguration, int>, ITaxCategorySaleConfigurationRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		
        public TaxCategorySaleConfigurationRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        
        
        public IQueryable<TaxCategorySaleConfiguration> GetByCustomFilter( int? parentId, int? taxCategoryId, int? saleConfigurationId, int? factorReportType)
        {
            var query = GetAll(true);
            if(parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }
            
            if(taxCategoryId.HasValue)
            {
                query = query.Where(x => x.TaxCategoryId == taxCategoryId.Value);
            }
            
            if(saleConfigurationId.HasValue)
            {
                query = query.Where(x => x.SaleConfigurationId == saleConfigurationId.Value);
            }
            
            if(factorReportType.HasValue)
            {
                query = query.Where(x => x.FactorReportType == factorReportType.Value);
            }
            
            return query;
        }
         
    }
}      
