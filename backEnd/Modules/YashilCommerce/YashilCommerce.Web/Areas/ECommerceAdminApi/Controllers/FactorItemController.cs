using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class FactorItemController : BaseController<FactorItem, int, FactorItemListViewModel, FactorItemEditModel, FactorItemSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFactorItemService _factorItemService;
        private readonly IYashilLogger<FactorItem> _logger;
        private readonly IFactorService _factorService;
        private readonly IFactorVerificationService _factorVerificationService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IRepresentationService _representationService;
        private readonly IRefundDetailService _refundDetailService;

        public FactorItemController(IFactorItemService factorItemService,
            IMapper mapper, IYashilLogger<FactorItem> logger,
            IPermissionService permissionService, IFactorService factorService,
            IFactorVerificationService factorVerificationService,
          IUserPrincipal userPrincipal,
            IRepresentationService representationService,
            IRefundDetailService refundDetailService) : base(factorItemService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorItemService = factorItemService;
            _logger = logger;
            _factorService = factorService;
            _factorVerificationService = factorVerificationService;
            _userPrincipal = userPrincipal;
            _representationService = representationService;
            _refundDetailService = refundDetailService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int factorId, int? orderItemId)
        {

            await CheckUserAccessForFactorId(factorId);

            var factorItemDetailStatisticViews = _factorItemService.GetFactorItemListStatistic(factorId);
            return await DataSourceLoader.LoadAsync(factorItemDetailStatisticViews.ProjectTo<FactorItemDetailStatisticViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }



        /// <summary>
        /// دریافت لیست آیتمهای سفارش موجود در یک گروه بندی فاکتور
        /// این متد برای زمانی است ک گروه بندی فاکتور ها انجام شده و مالی میخواهد ریز ایتمهای موجود در آن را مشاهده کند.
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorGroupFactorItemForList")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetFactorGroupFactorItemForList(CustomDataSourceLoadOptions loadOptions, int factorGroupId)
        {

            var factorGroupFactorItems = _factorItemService.GetFactorGroupFactorItems(factorGroupId);
            return await DataSourceLoader.LoadAsync(factorGroupFactorItems.ProjectTo<FactorItemDetailFullViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// برای دریافت لیست آیتم های فاکتور نشده بر اساس کاربر لاگین شده
        /// این متد در هنگام صدور فاکتور مورد استفاده قرار میگیرد و در انجا کاربر لاگین شده با توجه به مرکز و نمایندگی خود آیستمهای فاکتور نشده را مشاهده میکند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="productSalesPlanningIds">برنامه ریزی دوره های فاکتور نشده انتخابی</param>
        /// <returns></returns>
        [HttpPost("GetCurrentUserUnFactoredOrderItems")]
        [YashilAuthorization(ActionName = "GetCurrentUserUnFactoredOrderItems")]
        public async Task<LoadResult> GetCurrentUserUnFactoredOrderItems(

            CustomDataSourceLoadOptions loadOptions, int?[] productSalesPlanningIds = null)
        {

            var currentUserUnFactoredOrderItems =
                    _factorItemService.GetCurrentUserUnFactoredOrderItems(productSalesPlanningIds);
            return await DataSourceLoader.LoadAsync(currentUserUnFactoredOrderItems
                .ProjectTo<UnFactoredOrderItemViewModel>(_mapper.ConfigurationProvider), loadOptions);

        }

        /// <summary>
        /// برای نمایش آیتم های فاکتور نشده با فیلتر مرکز و نمایندگی
        /// این متد در فرم ایتم های فاکتور نشده مورد استفاده قرار میگیرد
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetUnFactoredOrderItems")]
        [YashilAuthorization(ActionName = "GetUnFactoredOrderItems")]
        public async Task<LoadResult> GetUnFactoredOrderItems(int? companyId,
            int? representationId,
            CustomDataSourceLoadOptions loadOptions)
        {


            bool userHasAccessForCompanyAndRepresentation = await _representationService.CheckUserAccessForCompanyAndRepresentationAsync(companyId, representationId, _userPrincipal.Id);

            if (!userHasAccessForCompanyAndRepresentation)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "GetUnFactoredOrderItems"),
                    nameof(Factor), "GetUnFactoredOrderItems Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Factor)}");
            }

            IQueryable<UnFactoredOrderItemView> unFactoredOrderItemViews = null;

            if (companyId != null)
            {
                unFactoredOrderItemViews = _factorItemService.GetUnFactoredOrderItems(companyId.Value, representationId);
            }

            return await DataSourceLoader.LoadAsync(unFactoredOrderItemViews.ProjectTo<UnFactoredOrderItemViewModel>(_mapper.ConfigurationProvider), loadOptions);

        }

        /// <summary>
        ///  بررسی می کند که آیا سایر کسورات در فاکتور تامین کننده لحاظ شده است یا خیر؟
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("CheckOtherDeductionsAreIncludedInTheSupplierInvoice")]
        public async Task<bool> CheckOtherDeductionsAreIncludedInTheSupplierInvoice(int factorId)
        {
            await CheckUserAccessForFactorId(factorId);

            return await _factorItemService.CheckOtherDeductionsAreIncludedInTheSupplierInvoice(factorId);
        }
        /// <summary>
        /// دریافت لیست آیتم های فاکتوری که محاسبه قیمت آن بدرستی انجام نشده است
        /// در فرم بررسی فاکتور مورد استفاده قرار میگیرد
        /// sp-checkIsInvalidfactorItem را استفاده میکند.
        /// </summary>
        /// <param name="factorVerificationId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetInvalidFactorItemList")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<List<InvalidFactorItemViewModel>> GetInvalidFactorItemAsync(int factorVerificationId)
        {

            var factorVerification = await _factorVerificationService.GetAsync(factorVerificationId, true);
            if (factorVerification == null)
            {
                throw new Exception($" GetAsync not found : {nameof(InvalidFactorItemViewModel)}");
            }

            await CheckUserAccessForFactorId(factorVerification.FactorId);

            var invalidFactorItems = _factorItemService.GetInvalidFactorItem(factorVerification.FactorId);
            return _mapper.Map<List<InvalidFactorItemViewModel>>(invalidFactorItems);

        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(FactorItemEditModel editModel)
        {
            if (editModel.Id != 0)
            {
                var factorItem = _factorItemService.Get(editModel.Id, true);
                if (factorItem != null)
                {
                    var factor = _factorService.Get(factorItem.FactorId, true);
                    if (factor != null)
                    {
                        if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload)
                        {
                            return await base.CustomValidateBeforeUpdateAsync(editModel);
                        }
                        return "این آیتم اجازه ویرایش ندارد.";
                    }
                    return "فاکتوری برای این آیتم وجود ندارد.";
                }
                return "چنین آیتمی وجود ندارد.";
            }
            return "چنین آیتمی وجود ندارد.";
        }

        /// <summary>
        /// the user can delete the factorItem  if the factor was initial step or rejected
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var factorItem = _factorItemService.Get(id, true);
            if (factorItem != null)
            {
                var factor = _factorService.Get(factorItem.FactorId, true);
                if (factor != null)
                {
                    if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload)
                    {
                        return await base.CustomValidateBeforeDeleteAsync(id);
                    }
                    else if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Rejected)
                    {
                        return "فاکتور این آیتم رد شده است. لطفا فاکتور را حذف و درخواست فاکتور جدیدی ثبت نمایید.";
                    }
                    else
                    {
                        return "این آیتم اجازه ویرایش ندارد.";
                    }
                }
                else
                {
                    return "فاکتوری برای این آیتم وجود ندارد.";
                }
            }
            else
            {
                return "چنین آیتمی وجود ندارد.";
            }


        }

        private async Task CheckUserAccessForFactorId(int factorId)
        {
            bool userHasAccessForFactor = await _factorService.CheckUserAccessForFactorId(factorId);
            if (!userHasAccessForFactor)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "GetByCustomFilterForList"),
                    nameof(Factor), "GetByCustomFilterForList Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Factor)}");
            }
        }

        /// <summary>
        /// دریافت لیست پرداخت های مربوط به هر آیتم فاکتور
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorId"></param>
        /// <returns></returns>

        [HttpGet("GetFactorItemPayment")]
        [YashilAuthorization(ActionName = "FactorItemPayment")]
        public async Task<LoadResult> GetFactorItemPaymentAsync(CustomDataSourceLoadOptions loadOptions, int factorId)
        {
            var factorItems = _factorItemService.GetFactorItems(factorId);
            var result = await DataSourceLoader.LoadAsync(factorItems.ProjectTo<FactorItemPaymentsViewModel>(_mapper.ConfigurationProvider), loadOptions);
            //شد flat برای نمایش درست دیتا در پنل مقدار بدست آمد
            var flattenedData = result.data.Cast<FactorItemPaymentsViewModel>().SelectMany(x => x.FactorOrderItemPaymentList).ToList();
            return new LoadResult
            {
                data = flattenedData,
                totalCount = flattenedData.Count
            };
        }

        /// <summary>
        ///  دریافت خلاصه پرداخت آیتم های فاکتور برای اساس درصد 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorItemPaymentSummary")]
        [YashilAuthorization(ActionName = "FactorItemPaymentSummary")]
        public async Task<LoadResult> GetFactorItemPaymentSummaryAsync(CustomDataSourceLoadOptions loadOptions, int factorId)
        {
            var factorItems = _factorItemService.GetFactorItemPaymentSummary(factorId);
            return await DataSourceLoader.LoadAsync(factorItems.ProjectTo<FactorItemSummaryViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        /// <summary>
        /// دریافت گزارش فاکتورهایی که یارانه گرفته و عودت وجه رخ داده است
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorId"></param>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        [HttpGet("GetRefundedFactorItemDetails")]
        [YashilAuthorization(ActionName = "RefundedFactorItemDetails")]
        public async Task<LoadResult> GetRefundedFactorItemDetailsAsync(CustomDataSourceLoadOptions loadOptions, int factorId, int? membershipId)
        {
            var factorItems = _factorItemService.GetFactorItemsByMembershipType(factorId, membershipId);
            return await DataSourceLoader.LoadAsync(
                factorItems.ProjectTo<FactorItemRefundedViewModel>(
                _mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetFactorItemPropertiesThatHaveNotEffectInFactorPrice")]
        //[YashilAuthorization("GetFactorItemPropertiesThatHaveNotEffectInFactorPrice")]
        public async Task<LoadResult> GetFactorItemPropertiesThatHaveNotEffectInFactorPriceAsync(CustomDataSourceLoadOptions loadOptions, int? factorId)
        {
            if (factorId is null or <= 0)
                throw new ArgumentNullException(nameof(factorId));


            return await DataSourceLoader.LoadAsync(_factorItemService.GetFactorItems(factorId.Value)
                .ProjectTo<FactorItemPropertiesThatHaveNotEffectInFactorPriceViewModel>(_mapper.ConfigurationProvider), loadOptions);

        }

        /// <summary>
        /// بروزرسانی مقدار
        /// insurancePercentage
        /// insurancePrice
        /// مربوط به فاکتور ایتم ها بر اساس
        /// مقدار
        /// insurancePercentage
        /// در تنظیم فروش شناسه فاکتور ارسالی
        /// نکته مقدار
        /// insurancePrice
        /// بر اساس محاسباتی که طبق فرمول در ادامه وجود دارد بروز می شود
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("UpdateFactorItemsInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentage")]
        [YashilAuthorization("UpdateFactorItemsInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentage")]
        public async Task<ResultStatus<int>> UpdateFactorItemsInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentageAsync(int? factorId)
        {
            if (factorId is null or <= 0)
                throw new ArgumentNullException(nameof(factorId));

            try
            {
                await _factorItemService
                    .UpdateAllFactorItemInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentageAndPriceAsync(
                        factorId.Value);

                return new ResultStatus<int>(true, "عملیات موفقیت آمیز انجام شد.");
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(UpdateFactorItemsInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentageAsync),
                    $"بروز خطا به هنگام بروزرسانی مقادیر درصد بیمه ایتم های سفارش  برای شناسه فاکتور : {factorId}", e);

                return new ResultStatus<int>(false, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");

            }
        }

        [HttpGet("UpdateFactorItemsOtherDeductionsPercentageAndPriceFromOrderItemSaleConfiguration")]
        [YashilAuthorization("UpdateFactorItemsOtherDeductionsPercentageAndPriceFromOrderItemSaleConfiguration")]
        public async Task<ResultStatus<int>> UpdateFactorItemsOtherDeductionsPercentageAndPriceFromOrderItemSaleConfigurationAsync(int? factorId)
        {
            if (factorId is null or <= 0)
                throw new ArgumentNullException(nameof(factorId));

            try
            {
                await _factorItemService
                    .UpdateFactorItemsOtherDeductionsPercentageAndPriceFromOrderItemSaleConfigurationAsync(
                        factorId.Value);

                return new ResultStatus<int>(true, "عملیات موفقیت آمیز انجام شد.");
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(UpdateFactorItemsOtherDeductionsPercentageAndPriceFromOrderItemSaleConfigurationAsync),
                    $"بروز خطا بهنگام بروزرسانی مقدار سایز کسورات مربوط به ایتم های سفارش فاکتور با شناسه :{factorId}", e);

                return new ResultStatus<int>(false, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");

            }
        }


        /// <summary>
        /// بروزرسانی شامل بیمه بودن ایتم های مربوط به یک فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("UpdateFactorItemIsIncludeInsuranceFromProductCategory")]
        [YashilAuthorization("UpdateFactorItemIsIncludeInsuranceFromProductCategory")]
        public async Task<ResultStatus<int>> UpdateFactorItemIsIncludeInsuranceFromProductCategoryAsync(int? factorId)
        {
            if (factorId is null or <= 0)
                throw new ArgumentNullException(nameof(factorId));

            try
            {
                await _factorItemService
                    .UpdateFactorItemIsIncludeInsuranceFromProductCategoryAsync(
                        factorId.Value);

                return new ResultStatus<int>(true, "عملیات موفقیت آمیز انجام شد.");
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(UpdateFactorItemIsIncludeInsuranceFromProductCategoryAsync),
                    $"بروز خطا بهنگام بروزرسانی مقدار شامل بیمه بودن ایتم های سفارش فاکتور با شناسه :{factorId}", e);

                return new ResultStatus<int>(false, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");

            }
        }
    }
}