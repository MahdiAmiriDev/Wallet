			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class IssueTypeItemRepository : GenericApplicationBasedRepository<IssueTypeItem,int>, IIssueTypeItemRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public IssueTypeItemRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<IssueTypeItem> GetByCustomFilter( int? issueId, int? issueType)
        {
            var query= GetAll(true);
                         if ( issueId.HasValue)
                {
                    query = query.Where(x => x.IssueId == issueId.Value);
                }
                          if ( issueType.HasValue)
                {
                    query = query.Where(x => x.IssueType == issueType.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
