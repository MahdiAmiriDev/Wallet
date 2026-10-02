using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.ServiceImpl;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class RepresentationController : BaseController<Representation,
        int, RepresentationListViewModel,
        RepresentationEditModel, RepresentationSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepresentationService _representationService;
        private readonly IAddressService _addressService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IFactorService _factorService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IAppConfigService _appConfigService;
        private readonly ApplicationConfiguration _applicationConfiguration;


        public RepresentationController(IRepresentationService representationService, IMapper mapper,
            IUserPrincipal userPrincipal, IAppConfigService appConfigService,
            ApplicationConfiguration applicationConfiguration,
            IAddressService addressService, IYashilLogger<Representation> logger,
            IPermissionService permissionService,
            IProductSalesPlanningService productSalesPlanningService,
            IFactorService factorService,
            IAdditionalUserPropService additionalUserPropService) : base(
            representationService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _userPrincipal = userPrincipal;
            _addressService = addressService;
            _productSalesPlanningService = productSalesPlanningService;
            _factorService = factorService;
            _additionalUserPropService = additionalUserPropService;
            _representationService = representationService;
            _appConfigService = appConfigService;
        }

        [HttpGet("GetByCompany")]
        public async Task<LoadResult> GetByCompany(CustomDataSourceLoadOptions loadOptions, int? companyId, bool showConfirmRepresentation = false)
        {
            if (!companyId.HasValue)
            {
                return null;
            }
            var representations = _representationService.GetByCustomFilter(null, companyId.Value, null, null, null, showConfirmRepresentation);


            return await DataSourceLoader.LoadAsync(representations.ProjectTo<RepresentationSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        [HttpGet("GetByProductSalesPlanningCompany")]
        public async Task<LoadResult> GetByProductSalesPlanningCompanyAsync(CustomDataSourceLoadOptions loadOptions, int? productSalesPlanningId)
        {
            if (!productSalesPlanningId.HasValue)
            {
                return null;
            }
            var representations =
                _representationService.GetByProductSalesPlanningCompany(productSalesPlanningId.Value);

            return await DataSourceLoader.LoadAsync(representations.ProjectTo<RepresentationSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetRepresentationList")]
        [YashilAuthorization("GetForSelect")]
        public async Task<List<RepresentationSimpleViewModel>> GetRepresentationListAsync(int companyId)
        {
            var userHasAccess = await _representationService.CheckUserAccessForCompanyAsync(companyId);

            if(!userHasAccess)
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Representation)}");

            var representations = _representationService
                .GetByCustomFilter(null, companyId, null, null, null, true);

            return await representations.ProjectTo<RepresentationSimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("GetByCompanyMainCategoryForList")]
        public async Task<LoadResult> GetByCompanyForList(CustomDataSourceLoadOptions loadOptions, int? companyId)
        {
            var representations = companyId.HasValue
                ? _representationService.GetByCompanyMainCategory(companyId.Value)
                : _representationService.GetAll(true);

            return await DataSourceLoader.LoadAsync(
                representations.ProjectTo<RepresentationListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task CustomMapBeforeUpdateAsync(RepresentationEditModel editModel, Representation entity)
        {
            if (entity.Address != null)
            {
                await _addressService.UpdateAsync(entity.Address, entity.Address.Id);
            }

            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? addressId,
            int? companyId, int? founderId, int? licenseType, int? ownershipType)
        {
            var representations =
                _representationService.GetByCustomFilter(addressId, companyId, founderId, licenseType, ownershipType);
            return await DataSourceLoader.LoadAsync(
                representations.ProjectTo<RepresentationListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? addressId,
            int? companyId, int? founderId, int? licenseType, int? ownershipType)
        {
            var representations =
                _representationService.GetByCustomFilter(addressId, companyId, founderId, licenseType, ownershipType);
            return await DataSourceLoader.LoadAsync(
                representations.ProjectTo<RepresentationSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(RepresentationEditModel editModel)
        {
            var representation = _representationService.Get(editModel.Id, true);
            if (editModel.FounderId == 0)
            {
                return "کاربر گرامی لطفا مؤسس را انتخاب کنید.";
            }
            if (representation.CompanyId != editModel.CompanyId)

            {
                var hasAnyProductSalesPlanning =
                    _productSalesPlanningService.CheckRepresentationHasAnyProductSalesPlanning(representation.Id);
                if (hasAnyProductSalesPlanning)
                {
                    return "کاربر گرامی با توجه به اینکه برای این نمایندگی برنامه ریزی فروش وجود دارد نمی توانید مرکز را تغییر دهید.";
                }
            }
            //بررسی میکنیم که کد ارسال شده از سمت کاربر با کد ذخیره شده از قبل یکسال هست یا خیر 
            // اگر یکسان نباشد باید بررسی کنیم که فاکتوری صادر نشده باشد
            // اگر فاکتور صادر شده باشد باید پیام خطا بدهیم
            // این کد در گزارش های کانورت کاربر دارد
            if (representation.Code != editModel.Code || representation.Title != editModel.Title)
            {
                var hasAnyFactor =
                    _factorService.CheckExistenceFactorForCompanyAndRepresentation(representation.CompanyId,
                        representation.Id);
                if (hasAnyFactor)
                {
                    return "کاربر گرامی با توجه به اینکه برای این نمایندگی فاکتور صادر شده است امکان تغییر کد یا عنوان آن وجود ندارد";
                }
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        protected override async Task<(bool isSuccess, string message)> AfterInsertAsync(RepresentationEditModel editModel, Representation entity)
        {
            await _productSalesPlanningService.UpdateProductSalesPlanningCalculatedPriceAsync();
            if (entity.Id > 0)
            {
                await _representationService.AddCategoriesForRepresentationAsync(entity.Id);
            }

            return (true, string.Empty);
        }

        protected override async Task UpdateAsync(Representation entity, RepresentationEditModel editModel, int entityId, List<string> props, bool modifyProps = true)
        {
            var representaion = await _representationService.GetAsync(entity.Id, true);

            if (representaion.VezaratSubsidyFactor != editModel.VezaratSubsidyFactor ||
                representaion.SataSubsidyFactor != editModel.SataSubsidyFactor ||
                representaion.Address.CityId != editModel.Address.CityId)
            {
                await _productSalesPlanningService.UpdateProductSalesPlanningCalculatedPriceAsync();
            }

            await base.UpdateAsync(entity, editModel, entityId, props, modifyProps);
        }

        protected override IQueryable<Representation> GetAll()
        {
            int? companyId = null;
            if (_userPrincipal.Id != 0)
            {
                var res = _additionalUserPropService.GetByUserId(_userPrincipal.Id);
                companyId = res.CompanyId;
            }
            if (companyId != null)
            {
                return _representationService.GetByCompanyId(companyId.Value, true);
            }

            return base.GetAll();
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            await _representationService.DeleteRepresentationCategoryByIdAsync(id);

            var (isAllowed, message) = await _representationService.AllowDeleteRepresntationAsync(id);
            if (!isAllowed)
            {
                return message;
            }
            return await base.CustomValidateBeforeDeleteAsync(id);
        }

    }
}