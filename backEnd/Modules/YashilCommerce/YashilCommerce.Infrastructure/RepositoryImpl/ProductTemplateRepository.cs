using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class ProductTemplateRepository : GenericApplicationBasedRepository<ProductTemplate,int>, IProductTemplateRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public ProductTemplateRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
            

       
    }
}      
