			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class IssueParticipantRepository : GenericApplicationBasedRepository<IssueParticipant,int>, IIssueParticipantRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public IssueParticipantRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<IssueParticipant> GetByCustomFilter( int? issueId, int? userId)
        {
            var query= GetAll(true);
                         if ( issueId.HasValue)
                {
                    query = query.Where(x => x.IssueId == issueId.Value);
                }
                          if ( userId.HasValue)
                {
                    query = query.Where(x => x.UserId == userId.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
