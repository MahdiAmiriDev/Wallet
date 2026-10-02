using System.Linq;
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

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ProductSalesPlanningDiscountController : BaseController<ProductSalesPlanningDiscount, int,
        ProductSalesPlanningDiscountListViewModel, ProductSalesPlanningDiscountEditModel,
        ProductSalesPlanningDiscountSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductSalesPlanningDiscountService _productSalesPlanningDiscountService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;

        public ProductSalesPlanningDiscountController(
            IProductSalesPlanningDiscountService productSalesPlanningDiscountService,
            IMapper mapper,
            IYashilLogger<ProductSalesPlanningDiscount> logger,
            IPermissionService permissionService, IProductSalesPlanningService productSalesPlanningService
        )
            : base(productSalesPlanningDiscountService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _productSalesPlanningService = productSalesPlanningService;
            _productSalesPlanningDiscountService = productSalesPlanningDiscountService;
        }

        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? productSalesPlanningId, int? customerClubId)
        {
            var productSalesPlanningDiscounts =
                _productSalesPlanningDiscountService.GetByCustomFilter(productSalesPlanningId, customerClubId);
            return await DataSourceLoader.LoadAsync(
                productSalesPlanningDiscounts.ProjectTo<ProductSalesPlanningDiscountListViewModel>(
                    _mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,
            int? productSalesPlanningId, int? customerClubId)
        {
            var productSalesPlanningDiscounts =
                _productSalesPlanningDiscountService.GetByCustomFilter(productSalesPlanningId, customerClubId);
            return await DataSourceLoader.LoadAsync(
                productSalesPlanningDiscounts.ProjectTo<ProductSalesPlanningDiscountSimpleViewModel>(
                    _mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(
            ProductSalesPlanningDiscountEditModel editModel)
        {
            var isValidDiscountPercentage = CheckDiscountPercentage(editModel.DiscountPercentage);
            if (!isValidDiscountPercentage.success)
            {
                return isValidDiscountPercentage.message;
            }

            var checkProductSalePlanningDiscount =
                await CheckExistProductSalePlanningDiscountAsync
                    (editModel.CustomerClubId, editModel.ProductSalesPlanningId);
            if (checkProductSalePlanningDiscount.isExists)
            {
                return checkProductSalePlanningDiscount.message;
            }

            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(ProductSalesPlanningDiscountEditModel editModel)
        {
            var isValidDiscountPercentage = CheckDiscountPercentage(editModel.DiscountPercentage);
            if (!isValidDiscountPercentage.success)
            {
                return isValidDiscountPercentage.message;
            }

            var productSalesPlanningDiscount = await _productSalesPlanningDiscountService
                .GetAsync(editModel.Id, true);
            if (productSalesPlanningDiscount.CustomerClubId != editModel.CustomerClubId)
            {
                var checkProductSalePlanningDiscount = await
                    CheckExistProductSalePlanningDiscountAsync
                        (editModel.CustomerClubId, editModel.ProductSalesPlanningId);
                if (checkProductSalePlanningDiscount.isExists)
                {
                    return checkProductSalePlanningDiscount.message;
                }
            }

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        /// <summary>
        /// وقتی تخفیف برای برنامه ریزی دوره ثبت می کنیم
        /// باید وضعیت انتشار برنامه ریزی دوره را به حالت ثبت اولیه برگردانیم
        /// به همین دلیل این متد را بازنویسی می کنیم
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override async Task<(bool isSuccess, string message)> AfterInsertAsync(ProductSalesPlanningDiscountEditModel editModel,
            ProductSalesPlanningDiscount entity)
        {
            await _productSalesPlanningService.ChangeProductSalesPlanningConfirmationIfNeededAsync(
                editModel.ProductSalesPlanningId, true);
            await _productSalesPlanningService
                .UpdateProductSalesPlanningCalculatedPriceIfHasAccessToConfirmAsync(editModel.ProductSalesPlanningId);

            return (true, string.Empty);
        }

        /// <summary>
        /// وقتی تخفیف برنامه ریزی دوره را به روز رسانی می کنیم
        /// باید وضعیت انتشار برنامه ریزی دوره را به حالت ثبت اولیه برگردانیم
        /// به همین دلیل این متد را بازنویسی می کنیم
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override async Task AfterUpdateAsync(ProductSalesPlanningDiscountEditModel editModel,
            ProductSalesPlanningDiscount entity)
        {
            await _productSalesPlanningService.ChangeProductSalesPlanningConfirmationIfNeededAsync(
                  editModel.ProductSalesPlanningId, true);
            await _productSalesPlanningService
                .UpdateProductSalesPlanningCalculatedPriceIfHasAccessToConfirmAsync(editModel.ProductSalesPlanningId);
        }


        /// <summary>
        /// بررسی می کند تخفیف برنامه ریزی محصول بر اساس باشگاه مشتریان وجود دارد
        /// </summary>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <param name="productSalePlanningId"></param>
        /// <returns></returns>
        private async Task<(bool isExists, string message)> CheckExistProductSalePlanningDiscountAsync
            (int customerClubId, int productSalePlanningId)
        {
            var isExists = await _productSalesPlanningDiscountService
                .CheckExistProductSalePlanningDiscountAsync(
                    customerClubId, productSalePlanningId);
            var message = isExists
                ? "کاربر گرامی برای این باشگاه مشتریان تخفیف برنامه ریزی دوره تعریف شده است"
                : string.Empty;
            return (isExists, message);
        }

        /// <summary>
        /// بررسی مقدار درصد تخفیف
        /// </summary>
        /// <param name="discountPercentage">درصد تخفیف</param>
        /// <returns></returns>
        private (bool success, string message) CheckDiscountPercentage
            (decimal discountPercentage)
        {
            return (discountPercentage >= 0 || discountPercentage <= 99)
                ? (true, string.Empty)
                : (false, "کاربر گرامی، مقدار درصد تخفیف باید بین 0 و 99  باشد");
        }

        public override async Task<IActionResult> DeleteEntity(int id)
        {
            var productSalesPlanningId =
                await _productSalesPlanningDiscountService.FindById(id)
                    .Select(x => x.ProductSalesPlanningId)
                    .FirstOrDefaultAsync();
            var res = await base.DeleteEntity(id);
            await _productSalesPlanningService.UpdateProductSalesPlanningCalculatedPriceAsync(productSalesPlanningId);
            return res;
        }
    }
}