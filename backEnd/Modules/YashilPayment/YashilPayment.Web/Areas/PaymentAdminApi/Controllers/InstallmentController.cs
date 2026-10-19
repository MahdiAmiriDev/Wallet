using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
	public class InstallmentController : BaseController<Installment ,int,InstallmentListViewModel, InstallmentEditModel,InstallmentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IInstallmentService _installmentService;
		private readonly IYashilLogger<Installment> _logger;
        private readonly IPermissionService _permissionService;
        public InstallmentController(IInstallmentService installmentService, IMapper mapper, IYashilLogger<Installment> logger, IPermissionService permissionService) : base(installmentService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _installmentService=installmentService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? paymentId, int? paymentStatus)
        {
            var installments = _installmentService.GetByCustomFilter(paymentId,paymentStatus);
            return await DataSourceLoader.LoadAsync(installments.ProjectTo<InstallmentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? paymentId, int? paymentStatus)
        {
            var installments = _installmentService.GetByCustomFilter(paymentId,paymentStatus);
            return await DataSourceLoader.LoadAsync(installments.ProjectTo<InstallmentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      