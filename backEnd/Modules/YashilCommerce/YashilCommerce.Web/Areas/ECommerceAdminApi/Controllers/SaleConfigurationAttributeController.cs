using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilCommerce.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class SaleConfigurationAttributeController : BaseController<SaleConfigurationAttribute, int, SaleConfigurationAttributeListViewModel, SaleConfigurationAttributeEditModel, SaleConfigurationAttributeSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISaleConfigurationAttributeService _saleConfigurationAttributeService;

        public SaleConfigurationAttributeController(ISaleConfigurationAttributeService saleConfigurationAttributeService,
        IMapper mapper,
        IYashilLogger<SaleConfigurationAttribute> logger,
        IPermissionService permissionService) : base(saleConfigurationAttributeService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _saleConfigurationAttributeService = saleConfigurationAttributeService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? discountType, int? discountLimitationType, int? saleConfigurationId, int? customerClubId)
        {
            var saleConfigurationAttributes = _saleConfigurationAttributeService.GetByCustomFilter(discountType, discountLimitationType, saleConfigurationId, customerClubId);
            return await DataSourceLoader.LoadAsync(saleConfigurationAttributes.ProjectTo<SaleConfigurationAttributeListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? discountType, int? discountLimitationType, int? saleConfigurationId, int? customerClubId)
        {
            var saleConfigurationAttributes = _saleConfigurationAttributeService.GetByCustomFilter(discountType, discountLimitationType, saleConfigurationId, customerClubId);
            return await DataSourceLoader.LoadAsync(saleConfigurationAttributes.ProjectTo<SaleConfigurationAttributeSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(SaleConfigurationAttributeEditModel editModel)
        {
            var checkExistsSaleConfigurationAttributeForCustomerClub
               = await CheckExistsSaleConfigurationAttributeForCustomerClub
               (editModel.SaleConfigurationId, editModel.CustomerClubId);
            if (checkExistsSaleConfigurationAttributeForCustomerClub.isExists)
            {
                return checkExistsSaleConfigurationAttributeForCustomerClub.message;
            }

            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(SaleConfigurationAttributeEditModel editModel)
        {
            var saleConfigurationAttribute = await _saleConfigurationAttributeService
                .GetAsync(editModel.Id, true);
            if (saleConfigurationAttribute.CustomerClubId != editModel.CustomerClubId)
            {
                var checkExistsSaleConfigurationAttributeForCustomerClub
                = await CheckExistsSaleConfigurationAttributeForCustomerClub
                (editModel.SaleConfigurationId, editModel.CustomerClubId);
                if (checkExistsSaleConfigurationAttributeForCustomerClub.isExists)
                {
                    return checkExistsSaleConfigurationAttributeForCustomerClub.message;
                }
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        /// <summary>
        /// بررسی می کند برای این تنظیم فروش باشگاه مشتریان درج شده است یا خیر
        /// </summary>
        /// <param name="saleConfigurationId">تنظیم فروش</param>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        private async Task<(bool isExists, string message)> CheckExistsSaleConfigurationAttributeForCustomerClub(
            int saleConfigurationId,
            int customerClubId)
        {
            var isExists = await _saleConfigurationAttributeService.GetByCustomFilter
               (null, null,
                   saleConfigurationId,
                   customerClubId)
               .AnyAsync();
            var message = isExists
                ? "کاربر گرامی، برای این باشگاه مشتریان ویژگی تنظیم درج شده است."
                : string.Empty;
            return (isExists, message);



        }
        protected override Task CustomMapBeforeInsertAsync(SaleConfigurationAttributeEditModel editModel, SaleConfigurationAttribute entity)
        {
            SetDiscountTypeData(entity);
            return base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        protected override Task CustomMapBeforeUpdateAsync(SaleConfigurationAttributeEditModel editModel, SaleConfigurationAttribute entity)
        {
            SetDiscountTypeData(entity);
            return base.CustomMapBeforeUpdateAsync(editModel, entity);
        }
        /// <summary>
        /// ثبت مقدار اولیه برای نوع تخفیف و نوع محدودیت تخفیف
        /// </summary>
        /// <param name="entity"></param>
        private void SetDiscountTypeData(SaleConfigurationAttribute entity)
        {
            entity.DiscountType = (int)DiscountTypeEnum.AssignedToOrderTotal;
            entity.DiscountLimitationType = (int)DiscountLimitationTypEnum.Unlimited;
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            if (id > 0)
            {
                return "کاربر گرامی، ویژگی تنظیم فروش برای باشگاه مشتریان را نمی توان حذف کرد. ";
            }
            return await base.CustomValidateBeforeDeleteAsync(id);

        }
    }
}