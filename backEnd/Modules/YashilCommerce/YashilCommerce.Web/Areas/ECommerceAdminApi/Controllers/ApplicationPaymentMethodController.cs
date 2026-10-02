using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ApplicationPaymentMethodController : BaseController<ApplicationPaymentMethod, int, ApplicationPaymentMethodListViewModel, ApplicationPaymentMethodEditModel, ApplicationPaymentMethodSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationPaymentMethodService _applicationPaymentMethodService;

        public ApplicationPaymentMethodController(IApplicationPaymentMethodService applicationPaymentMethodService, IMapper mapper, IYashilLogger<ApplicationPaymentMethod> logger, IPermissionService permissionService) : base(applicationPaymentMethodService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _applicationPaymentMethodService = applicationPaymentMethodService;
        }
        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? paymentMethodType)
        {
            var applicationPaymentMethods = _applicationPaymentMethodService.GetByCustomFilter(paymentMethodType);
            return await DataSourceLoader.LoadAsync(applicationPaymentMethods.ProjectTo<ApplicationPaymentMethodListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetPaymentMethodsForSelect")]
        public async Task<LoadResult> GetPaymentMethodsForSelect(CustomDataSourceLoadOptions loadOptions, int? paymentMethodType)
        {
            var applicationPaymentMethods = _applicationPaymentMethodService.GetByCustomFilter(paymentMethodType);
            return await DataSourceLoader.LoadAsync(applicationPaymentMethods.ProjectTo<PaymentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetOnlineAndHekmatPaymentMethodsForSelect")]
        public async Task<LoadResult> GetOnlineAndHekmatPaymentMethodsForSelect(CustomDataSourceLoadOptions loadOptions)
        {
            var applicationPaymentMethods = _applicationPaymentMethodService.GetOnlineAndHekmatPaymentMethodsForSelect();
            return await DataSourceLoader.LoadAsync(applicationPaymentMethods.ProjectTo<PaymentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetNajaAndHekmatPaymentMethodsForSelect")]
        public async Task<LoadResult> GetNajaAndHekmatPaymentMethodsForSelect(CustomDataSourceLoadOptions loadOptions)
        {
            var applicationPaymentMethods = _applicationPaymentMethodService.GetNajaAndHekmatPaymentMethodsForSelect();
            return await DataSourceLoader.LoadAsync(applicationPaymentMethods.ProjectTo<PaymentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? paymentMethodType)
        {
            var applicationPaymentMethods = _applicationPaymentMethodService.GetByCustomFilter(paymentMethodType);
            return await DataSourceLoader.LoadAsync(applicationPaymentMethods.ProjectTo<ApplicationPaymentMethodSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}