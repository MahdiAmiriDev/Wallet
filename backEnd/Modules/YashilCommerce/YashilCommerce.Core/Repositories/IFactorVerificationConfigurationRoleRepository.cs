			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IFactorVerificationConfigurationRoleRepository : IGenericRepository<FactorVerificationConfigurationRole,int>
    {
		          
        IQueryable<FactorVerificationConfigurationRole> GetByCustomFilter( int? factorVerificationConfigurationId, int? roleId);


        IQueryable<FactorVerificationConfigurationRole> GetFactorVerificationConfigurationRoles(int factorVerificationConfigurationId);
        bool CheckExistenceFactorVerificationConfigurationRole(int factorVerificationConfigurationId);
    }
}      
 