using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class YashilSchedulerRepository : GenericApplicationBasedRepository<YashilScheduler, int>,
        IYashilSchedulerRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public YashilSchedulerRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<YashilScheduler> GetByCustomFilter(int? occuranceType)
        {
            var query = GetAll(true);
            if (occuranceType.HasValue)
            {
                query = query.Where(x => x.OccuranceType == occuranceType.Value);
            }


            return query;
        }


    }
}