			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class FactorVerificationConfigurationRoleRepository : GenericApplicationBasedRepository<FactorVerificationConfigurationRole,int>, IFactorVerificationConfigurationRoleRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public FactorVerificationConfigurationRoleRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<FactorVerificationConfigurationRole> GetByCustomFilter( int? factorVerificationConfigurationId, int? roleId)
        {
            var query = GetAll(true);
            if (factorVerificationConfigurationId.HasValue)
            {
                query = query.Where(x =>
                    x.FactorVerificationConfigurationId == factorVerificationConfigurationId.Value);
            }

            if (roleId.HasValue)
            {
                query = query.Where(x => x.RoleId == roleId.Value);
            }
                  
          
            return query;
        }

    public IQueryable<FactorVerificationConfigurationRole> GetFactorVerificationConfigurationRoles(int factorVerificationConfigurationId)
    {
        return DbSet.Where(x =>
            x.FactorVerificationConfiguration.IsActive &&
            x.FactorVerificationConfigurationId == factorVerificationConfigurationId);
    }

    public bool CheckExistenceFactorVerificationConfigurationRole(int factorVerificationConfigurationId)
    {
        return DbSet.Any(x =>
            x.FactorVerificationConfiguration.IsActive &&
            x.FactorVerificationConfigurationId == factorVerificationConfigurationId);
        }
    }
}      
