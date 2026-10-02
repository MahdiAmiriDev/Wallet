using Microsoft.EntityFrameworkCore;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class FactorVerificationConfigurationRepository : GenericApplicationBasedRepository<FactorVerificationConfiguration, int>,
        IFactorVerificationConfigurationRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public FactorVerificationConfigurationRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        /// <summary>
        /// check is there any active factorVerificationConfiguration for a appropriate user based on applicationId
        /// if exist return true 
        /// </summary>
        /// <returns></returns>
        public bool CheckExistenceActiveFactorVerificationConfiguration()
        {
            return DbSet.Any(x => x.ApplicationId == _userPrincipal.ApplicationId && x.IsActive);
        }

        /// <summary>
        /// get the active factorVerification based on applicationId
        /// </summary>
        /// <returns></returns>
        public FactorVerificationConfiguration GetActiveFactorVerificationConfiguration()
        {
            return DbSet.FirstOrDefault(x => x.ApplicationId == _userPrincipal.ApplicationId && x.IsActive);
        }

        /// <summary>
        /// get the active factorVerification based on applicationId include FactorVerificationConfigurationIncludeRoles
        /// </summary>
        /// <returns></returns>
        public FactorVerificationConfiguration GetActiveFactorVerificationConfigurationIncludeRoles()
        {
            return DbSet.Include(x => x.FactorVerificationConfigurationRole).FirstOrDefault(x => x.ApplicationId == _userPrincipal.ApplicationId && x.IsActive);
        }

        public bool CheckExistenceActiveFactorVerification()
        {
            return DbSet.Any(x => x.ApplicationId == _userPrincipal.ApplicationId && x.IsActive);

        }


        public void DeactivateOldFactorVerificationConfigurations()
        {
            // get list of factorVerificationConfiguration with these condition
            // check applicationId
            //check activate
            var factorVerificationConfigurations = base.GetAll().Where(x => x.IsActive);
            // update each active factorVerificationConfiguration - deActive them
            foreach (var factorVerificationConfiguration in factorVerificationConfigurations)
            {
                factorVerificationConfiguration.IsActive = false;
            }
        }
    }
}
