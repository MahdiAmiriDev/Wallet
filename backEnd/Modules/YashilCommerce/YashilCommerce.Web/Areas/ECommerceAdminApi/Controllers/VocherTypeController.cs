using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilUserManagement.Core.Dto;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class VocherTypeController : BaseController<VocherType, int, VocherTypeListViewModel, VocherTypeEditModel, VocherTypeSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IVocherTypeService _vocherTypeService;
        private readonly ICompanyService _companyService;
        private readonly IActivityLogService _activityLogService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IYashilLogger<VocherType> _logger;

        public VocherTypeController(IVocherTypeService vocherTypeService,
        IMapper mapper,
        IYashilLogger<VocherType> logger,
        IPermissionService permissionService,
        ICompanyService companyService,
        IActivityLogService activityLogService,
        IUserPrincipal userPrincipal) : base(vocherTypeService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _companyService = companyService;
            _vocherTypeService = vocherTypeService;
            _activityLogService = activityLogService;
            _userPrincipal = userPrincipal;
            _logger = logger;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? companyId)
        {
            var vocherTypes = _vocherTypeService.GetByCustomFilter(companyId);
            return await DataSourceLoader.LoadAsync(vocherTypes.ProjectTo<VocherTypeListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? companyId)
        {
            var vocherTypes = _vocherTypeService.GetByCustomFilter(companyId);
            return await DataSourceLoader.LoadAsync(vocherTypes.ProjectTo<VocherTypeSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCompanyId")]
        public async Task<LoadResult> GetByCompany(int? companyId, CustomDataSourceLoadOptions loadOptions)
        {
            if (!companyId.HasValue)
            {
                return null;
            }
            var representations = _vocherTypeService.GetByCustomFilter(companyId.Value, true);

            return await DataSourceLoader.LoadAsync(representations.ProjectTo<VocherTypeSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task CustomMapBeforeInsertAsync(VocherTypeEditModel editModel, VocherType entity)
        {
            await SetVoucherTemplateId(entity, editModel.CompanyId);
            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        protected override async Task CustomMapBeforeUpdateAsync(VocherTypeEditModel editModel, VocherType entity)
        {
            await SetVoucherTemplateId(entity, editModel.CompanyId);
            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }
        /// <summary>
        /// در صورتی که  فیلد
        /// VoucherUrlPrefix
        /// مرکز دارای مقدار باشد
        /// VoucherTemplateId  = 1
        ///  می شود
        /// در غیر اینصورت
        /// VoucherTemplateId  = 2
        ///  می شود
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private async Task SetVoucherTemplateId(VocherType entity, int companyId)
        {
            var voucherUrlPrefix = await _companyService.FindById(companyId)
                .Select(x => x.VoucherUrlPrefix)
                .FirstOrDefaultAsync();

            entity.VoucherTemplateId = string.IsNullOrEmpty(voucherUrlPrefix)
                ? (int)VoucherTemplateIdEnum.NotHasLinkAddress
                : (int)VoucherTemplateIdEnum.HasLinkAddress;
        }

        /// <summary>
        /// لیست تمامی ووچرهای مربوط به این ووچرتایپ 
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        [HttpGet("GetVochersList")]
        public async Task<LoadResult> GetVochersListAsync(CustomDataSourceLoadOptions loadOptions, int vocherTypeId)
        {
            var list = _vocherTypeService.GetVochersList(vocherTypeId);
            return await DataSourceLoader.LoadAsync(list.ProjectTo<VocherTypeVochersListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// حذف کوپن های تخصیص نیافته یک نوع مشخص 
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        [HttpPost("DeleteNotAssignedVochers")]
        [YashilAuthorization(ActionName = "DeleteNotAssignedVochers")]
        public async Task<ResultStatus<int>> DeleteNotAssignedVochersAsync(VocherTypeSimpleEditModel vocherTypeEditModel)
        {
            string successMessage = "کوپن های تخصیص نیافته با موفقیت حذف شدند";
            string noItemFoundMessage = "کوپن تخصیص نیافته ای پیدا نشد";
            string errorMessage = "متاسفانه خطایی در هنگام پردازش رخ داده است. دوباره سعی کنید";

            try
            {

                var result =await _vocherTypeService.DeleteNotAssignedVochersAsync(vocherTypeEditModel.Id);
                if(result == 0)
                {
                    return new ResultStatus<int>(false, noItemFoundMessage);
                }

                if (result == -1)   
                {
                    return new ResultStatus<int>(false, errorMessage);
                }

                await _activityLogService.AddActivityLogAsync(new AddActivityLogDto()
                {
                    Description = "حذف کپن های تخصیص نیافته",
                    ApplicationId = _userPrincipal.ApplicationId,
                    AppActionTitle = nameof(DeleteNotAssignedVochersAsync),
                    ObjectId = vocherTypeEditModel.Id,
                    PreviousRecordContent = JsonConvert.SerializeObject(result),
                    ResourceTitle = nameof(result),
                    UserId = _userPrincipal.Id
                });

                return new ResultStatus<int>(true, successMessage);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(VocherType),
                    "بروز خطا به هنگام حذف کپن های تخصیص نیافته",
                    e);
                return new ResultStatus<int>(false, errorMessage);
            }
        }
    }
}