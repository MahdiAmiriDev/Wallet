using AutoMapper;
using System.Threading.Tasks;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class FactorVerificationConfigurationController : BaseController<FactorVerificationConfiguration, int, FactorVerificationConfigurationListViewModel, FactorVerificationConfigurationEditModel, FactorVerificationConfigurationSimpleViewModel>
    {
        private readonly IFactorVerificationConfigurationService _factorVerificationConfigurationService;

        public FactorVerificationConfigurationController(IFactorVerificationConfigurationService factorVerificationConfigurationService, IMapper mapper, IYashilLogger<FactorVerificationConfiguration> logger, IPermissionService permissionService) : base(factorVerificationConfigurationService, mapper, logger, permissionService)
        {
            _factorVerificationConfigurationService = factorVerificationConfigurationService;
        }


        protected override async Task CustomMapBeforeInsertAsync(FactorVerificationConfigurationEditModel editModel,
            FactorVerificationConfiguration entity)
        {
            DeActiveOldFactorVerificationConfigurations(editModel);
            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        protected override async Task CustomMapBeforeUpdateAsync(FactorVerificationConfigurationEditModel editModel, FactorVerificationConfiguration entity)
        {
            DeActiveOldFactorVerificationConfigurations(editModel);
            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }

        /// <summary>
        /// deactivate all old Active factorVerifications
        /// </summary>
        /// <param name="editModel"></param>
        private void DeActiveOldFactorVerificationConfigurations(FactorVerificationConfigurationEditModel editModel)
        {
            if (editModel.IsActive)
            {
                _factorVerificationConfigurationService.DeactivateOldFactorVerificationConfigurations();
            }
        }
    }
}