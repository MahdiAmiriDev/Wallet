using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.AppEvents;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;
using YashilLight.Core.Dto;
using YashilLight.Core.Repositories;
using YashilUserManagement.Core.Services;


namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ProductSalesPlanningService : GenericService<ProductSalesPlanning, int>, IProductSalesPlanningService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IProductSalesPlanningRepository _productSalesPlanningRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IItemGroupService _itemGroupService;
        private readonly IDocTypeService _docTypeService;
        private readonly IYashilLogger<ProductSalesPlanning> _logger;
        private readonly IOrderItemService _orderItemService;
        private readonly IPermissionService _permissionService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly ISaleConfigurationAttributeService _saleConfigurationAttributeService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IAppConfigService _appConfigService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IMediaSessionViewingHistoryRepository _mediaSessionViewingHistoryRepository;

        public ProductSalesPlanningService(
            IUnitOfWork unitOfWork,
            IProductSalesPlanningRepository productSalesPlanningRepository,
            IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher,
            IAppDocumentService appDocumentService,
            IItemGroupService itemGroupService,
            IYashilLogger<ProductSalesPlanning> logger,
            IDocTypeService docTypeService,
            IOrderItemService orderItemService,
            IPermissionService permissionService,
            IAdditionalUserPropService additionalUserPropService,
            ISaleConfigurationAttributeService saleConfigurationAttributeService,
            IMediaSessionViewingHistoryRepository mediaSessionViewingHistoryRepository,
            ApplicationConfiguration applicationConfiguration, IAppConfigService appConfigService,
            IStaticCacheManager staticCacheManager)
            : base(unitOfWork, productSalesPlanningRepository,
                userPrincipal, eventPublisher)
        {
            _productSalesPlanningRepository = productSalesPlanningRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _appDocumentService = appDocumentService;
            _itemGroupService = itemGroupService;
            _logger = logger;
            _docTypeService = docTypeService;
            _orderItemService = orderItemService;
            _permissionService = permissionService;
            _additionalUserPropService = additionalUserPropService;
            _saleConfigurationAttributeService = saleConfigurationAttributeService;
            _applicationConfiguration = applicationConfiguration;
            _appConfigService = appConfigService;
            _staticCacheManager = staticCacheManager;
            _mediaSessionViewingHistoryRepository = mediaSessionViewingHistoryRepository;
        }

        public IQueryable<ProductSalesPlanning> GetByCustomFilter(int? productId, int? representationId,
            int? manageInventoryMethodType, int? productAvailabilityRangeType, int? deliveryDateType, int? published,
            int? confirmed)
        {
            return _productSalesPlanningRepository.GetByCustomFilter(productId, representationId,
                manageInventoryMethodType, productAvailabilityRangeType, deliveryDateType, published, confirmed);
        }

        public async Task<IQueryable<ProductSalesPlanning>> SearchProductSalesPlanningsAsync(string searchTitle,
            int? applicationGroupId, int? applicationId,
            int? categoryId, List<int> company, int? representationId, int? cityId, int? categoryGroupId,
            bool loadSubCategories,
            bool showIfHasPrice = false)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            //گروه دسته بندی اشتغال
            var employmentCategoryGroupId = await _appConfigService.GetValueByApplicationIdAsync(
                ApplicationConfigKey.YashilCommerceCategoryGroupEmploymentCategoryGroupId,
                _applicationConfiguration.ApplicationId);

            // در صورتی که گروه دسته بندی اشتغالی باشد 
            bool isEmploymentCategoryGroup = employmentCategoryGroupId == categoryGroupId;

            var query = _productSalesPlanningRepository
                .SearchProductSalesPlannings(
                    userAdditionalUserProp.CustomerClubId,
                    searchTitle,
                    applicationGroupId,
                    applicationId,
                    categoryId,
                    company,
                    representationId,
                    cityId,
                    categoryGroupId,
                    loadSubCategories,
                    isEmploymentCategoryGroup,
                    showIfHasPrice);
            return query.OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.Product.CreationDate);
        }

        public async Task<IQueryable<Company>> FindProductSalesPlanningsCompaniesAsync(string searchTitle,
            int? applicationId, int? categoryId, int? representationId, int? cityId, bool loadSubCategories,
            bool showIfHasPrice = false)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            return _productSalesPlanningRepository.FindProductSalesPlanningsCompanies(
                userAdditionalUserProp.CustomerClubId, searchTitle, applicationId, categoryId, representationId, cityId,
                loadSubCategories, showIfHasPrice);
        }

        public Task<int?> GetRepresentationIdByProductSalesPlanningIdAsync(int salesPlanningId, int applicationId)
        {
            return _productSalesPlanningRepository.GetRepresentationIdByProductSalesPlanningIdAsync(salesPlanningId,
                applicationId);
        }

        public IQueryable<ProductSalesPlanning> FindById(int pSpId, int? applicationId = null)
        {
            return _productSalesPlanningRepository.FindById(pSpId, applicationId);
        }

        /// <param name="filterByOrderItemRepresentationId">اگر برنامه ریزی دوره ای شناسه نمایندگی نداشته باشد لازم است
        /// در صورت فعال بودن این فیلتر و ارسال شناسه نمایندگی بین سفارش ها نیز بررسی می کند که اگر سفارشی با برای یک برناه
        /// ریزی دوره با شناسه این نمایندگی در ایتم سفارش ثبت شده باشد نیز جزو نیازمندی ما است</param>
        public async Task<IQueryable<ProductSalesPlanning>> GetByRepresentationFilterAsync(
            int applicationId,
            int? companyId,
            int? categoryId,
            int? representationId,
            bool showOnlyActiveProductSalesPlannings,
            bool doGetProductSalePlanningByRepresentation,
            bool? visibleIndividually = null,
            int? customerClubId = null,
            bool? showArchives = false,
            bool? filterByOrderItemRepresentationId = false,
            bool? includeHubRepresentationFilter = false)
        {
            customerClubId ??= (await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id))
                .CustomerClubId;
            return _productSalesPlanningRepository.GetByRepresentationFilter(
                applicationId,
                companyId,
                categoryId,
                representationId,
                showOnlyActiveProductSalesPlannings,
                doGetProductSalePlanningByRepresentation,
                customerClubId.Value,
                visibleIndividually, null, showArchives, filterByOrderItemRepresentationId, includeHubRepresentationFilter);
        }

        public Task UpdateProductSalesPlanningCalculatedPriceIfHasAccessToConfirmAsync(int productSalesPlanningId)
        {
            if (CanCurrentUserConfirmProductSalesPlanning())
            {
                return UpdateProductSalesPlanningCalculatedPriceAsync(productSalesPlanningId);
            }

            return Task.CompletedTask;
        }

        public Task UpdateProductSalesPlanningCalculatedPriceAsync(int? productSalesPlanningId = null)
        {
            var res =
                _productSalesPlanningRepository.UpdateProductSalesPlanningCalculatedPriceAsync(productSalesPlanningId);
            // هنگامی که UpdateProductSalesPlanningCalcedPrice صدا زده میشود
            // باید یک سری کش ها را پاک کنیم
            // لذا با publish/consumer میتوان این قضیه را هندل کرد
            // باید داخل consumer کش های مورد نظر را پاک کنیم
            _eventPublisher.EntityInsertedAsync<ProductSalesPlanning, int>(null);
            return res;
        }

        public Task<bool> CheckProductHasAnyProductSalesPlanningAsync(int productId)
        {
            return _productSalesPlanningRepository.CheckProductHasAnyProductSalesPlanningAsync(productId);
        }


        /// <summary>
        /// بررسی وجود برنامه ریزی محصول  بر اساس شناسه برنامه ریزی و شناسه برنامه
        /// </summary>
        /// <param name="productSalePlanningId"> شناسه برنامه ریزی محصول</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public async Task<bool> CheckExistsProductSalePlanningInApplicationAsync(int productSalePlanningId,
            int applicationId)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            return await _productSalesPlanningRepository.GetActiveProductSalePlanningById(
                    productSalePlanningId,
                    applicationId,
                    userAdditionalUserProp.CustomerClubId)
                .AnyAsync();
        }

        public async Task<bool> CheckExistsProductSalePlanningInApplicationAsync(int productSalePlanningId,
            int applicationId, bool considerZeroOrNullPriceValidation)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var query = _productSalesPlanningRepository.GetActiveProductSalePlanningById(
                    productSalePlanningId,
                    applicationId,
                    userAdditionalUserProp.CustomerClubId);

            if (considerZeroOrNullPriceValidation)
                query = query.Where(x => x.Price.HasValue && x.Price > 0);

            return await query.AnyAsync();
        }

        public IQueryable<ProductSalesPlanning> GetAllByApplicationId(int applicationId, bool readOnly)
        {
            return _productSalesPlanningRepository.GetAllByApplicationId(applicationId, readOnly);
        }

        public Task<AppDocument> GetProductSalesPlanningPicture(int productSalesPlanningId, int? applicationId)
        {
            applicationId ??= _userPrincipal.ApplicationId;
            return _appDocumentService.GetObjectDocumentsWithDocFormatByApplicationId(nameof(ProductSalesPlanning),
                    productSalesPlanningId,
                    (int)DocumentCategoryEnum.ProductSalesPlanningPicture, (int)DocFormatEnum.Image,
                    applicationId.Value)
                .FirstOrDefaultAsync();
        }

        public IQueryable<AppDocument> GetProductSalesPlanningAttachments(int productSalesPlanningId,
            int? applicationId)
        {
            applicationId ??= _userPrincipal.ApplicationId;
            return _appDocumentService.GetObjectDocumentsWithDocFormatByApplicationId(nameof(ProductSalesPlanning),
                productSalesPlanningId,
                (int)DocumentCategoryEnum.ProductSalesPlanningDocument, null, applicationId.Value);
        }

        /// <summary>
        /// لیست ایتم های مربوط به گروه را بر میگرداند
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<ProductSalesPlanning> GetProductSalesPlanningAssignedToGroup(int groupId,
            bool readOnly = false)
        {
            return _productSalesPlanningRepository.GetProductSalesPlanningAssignedToGroup(groupId, readOnly);
        }

        /// <summary>
        /// ایتم های گروهی با مقدار ایتم گروپ وارد شده را بر می گرداند
        /// </summary>
        /// <param name="itemGroupId"></param>
        /// <returns></returns>
        public Task<bool> ExistsAnyProductSalesPlanningWithThisItemGroupIdAsync(int itemGroupId)
        {
            return _productSalesPlanningRepository.ExistsAnyProductSalesPlanningWithThisItemGroupIdAsync(itemGroupId);
        }

        /// <summary>
        /// لیست ایتم گروه های گروه بندی نشده
        /// </summary>
        /// <param name="itemGroupSubId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IQueryable<ProductSalesPlanning> GetNotGroupedProductSalesPlanning(int itemGroupSubId,
            bool readOnly = false)
        {
            return _productSalesPlanningRepository.GetNotGroupedProductSalesPlanning(itemGroupSubId, readOnly);
        }

        /// <summary>
        /// جهت ایتم گروپ های که تا کنون گروه بندی نشده اند به گروه نهایی و ثبت نهایی کردن آن گروه
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<bool> AssignItemGroupForProductSalesPlanningAsync(int groupId)
        {
            var productSlaesPlanningGroup = await _itemGroupService.GetAsync(groupId, false);
            if (productSlaesPlanningGroup.ItemGroupSubId.HasValue)
            {
                var count = await _productSalesPlanningRepository.AssignItemGroupForProductSalesPlanningAsync(
                    productSlaesPlanningGroup.ItemGroupSubId.Value, groupId);
                if (count > 0)
                {
                    productSlaesPlanningGroup.ItemGroupStatusType = (int)ItemGroupStatusTypeEnum.Confirmed;
                    await _itemGroupService.UpdateAsync(productSlaesPlanningGroup, productSlaesPlanningGroup.Id, true);
                    return true;
                }
            }

            return false;
        }

        public async Task<IQueryable<ProductSalesPlanning>> GetTeacherProductSalesPlanningsAsync(int applicationId,
            int teacherId, int? companyId, int? categoryId, int? representationId, bool hierarchical,
            bool showOnlyActiveProductSalesPlannings, bool? visibleIndividually = null)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            return await _productSalesPlanningRepository.GetTeacherProductSalesPlanningsAsync(
                applicationId, teacherId, companyId, categoryId, representationId,
                hierarchical, showOnlyActiveProductSalesPlannings, userAdditionalUserProp.CustomerClubId,
                visibleIndividually);
        }

        public async Task<bool> ConvertProductSalesPlanningImageAsync()
        {
            var productSalesPlanningIds = GetAll(true).Select(x => x.Id);
            var docType = await _docTypeService.FindById((int)DocumentTypeEnum.ProductSalesPlanningImage).Select(x =>
                new
                {
                    x.ConvertToWebp,
                    x.MaxWidth,
                    x.MaxHeight,
                }).FirstOrDefaultAsync();
            if (docType == null)
            {
                _logger.InsertLog(LogLevel.Error,
                    new EventId((int)Events.GetEntityException, Events.GetEntityException.ToString()), nameof(DocType),
                    $"Get EntityException with id = {DocumentTypeEnum.ProductSalesPlanningImage}", null);
                return false;
            }

            try
            {
                foreach (var productSalesPlanningId in productSalesPlanningIds)
                {
                    var appDocument = await _appDocumentService.GetAll().Where(x => x.ObjectId == productSalesPlanningId
                        && x.DocTypeId == (int)DocumentTypeEnum.ProductSalesPlanningImage
                        && x.DocumentFile != null
                        && x.Extension != "webp").FirstOrDefaultAsync();
                    if (appDocument == null)
                    {
                        continue;
                    }

                    var stream = new MemoryStream(appDocument.DocumentFile);
                    // لود کردن عکس
                    var image = await Image.LoadAsync(stream);
                    // تغییر اندازه عکس
                    image.Mutate(c =>
                    {
                        c.Resize(new ResizeOptions()
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(docType.MaxWidth ?? image.Width, docType.MaxHeight ?? image.Height)
                        });
                    });
                    // تبدیل به فرمت webp 
                    var outStream = new MemoryStream(0);
                    await image.SaveAsWebpAsync(outStream);
                    appDocument.ContentType = "image/webp";
                    appDocument.Extension = "webp";
                    appDocument.DocumentFile = outStream.GetBuffer();

                    await _appDocumentService.UpdateAsync(appDocument, appDocument.Id);

                    stream.Close();
                    await stream.DisposeAsync();
                    outStream.Close();
                    await outStream.DisposeAsync();
                }

                await _appDocumentService.SaveChangeAsync();
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.UpdateException, "upload"),
                    nameof(AppDocument), $"error while updating psp image", e);
                return false;
            }

            return true;
        }

        public async Task<ProductSalesPlanning> ChangeProductSalesPlanningConfirmationIfNeededAsync(
            int productSalesPlanningId, bool saveAfterUpdate = false)
        {
            var productSalesPlanning = await GetAsync(productSalesPlanningId, false);
            if (productSalesPlanning == null)
            {
                return null;
            }

            productSalesPlanning.ModificationDate = DateTime.Now;

            var needAdminApproval = Convert.ToBoolean(CryptographyHelper.AesDecrypt(await _appConfigService.GetStringValueByApplicationIdAsync(ApplicationConfigKey
                  .GetAdminApprovalRequiredForProductSalesPlanning, _userPrincipal.ApplicationId)));
            var isLmsMode = await _appConfigService.GetBooleanValueAsync(ApplicationConfigKey.SiteConfigurationLmsMode);

            //   هست یا نه LMS بررسی اینکه سامانه در حالت
            if (!await _appConfigService.GetBooleanValueAsync(ApplicationConfigKey.SiteConfigurationLmsMode))
            {
                if (!CanCurrentUserConfirmProductSalesPlanning())
                {
                    productSalesPlanning.Confirmed = null;
                }
            }
            else
            {
                if (!CanCurrentUserConfirmProductSalesPlanning())
                {
                    //  می شود تا در انتظار تایید بالادستی باشد null درصورتی که نیاز به تایید ادمین یا بالادستی بود این مقدار
                    // در غیر اینصورت تایید می شود
                    productSalesPlanning.Confirmed = Convert.ToBoolean(CryptographyHelper.AesDecrypt(await _appConfigService.GetStringValueByApplicationIdAsync(ApplicationConfigKey
                     .GetAdminApprovalRequiredForProductSalesPlanning, _userPrincipal.ApplicationId))) ? null : true;
                }
            }

            return await UpdateAsync(productSalesPlanning, productSalesPlanning.Id, saveAfterUpdate);
        }

        public bool CanCurrentUserConfirmProductSalesPlanning() =>
            _permissionService.Authorize(SystemNameEnum.ProductSalesPlanning.ToString(),
                "ChangeProductSalesPlanningPublishState");

        /// <summary>
        /// در صورتی که برنامه ریزی محصول دارای درصد تخفیف باشد
        /// قیمت با تخفیف را با درصد تخفیف برنامه ریزی محصول محاسبه می کند.
        /// در غیر اینصورت درصد تخفیف باشگاه مشتریان کاربر لاگین کرده را دریافت
        /// و قیمت با تخفیف را با آن محاسبه می کند
        /// </summary>
        /// <param name="price">قیمت</param>
        /// <param name="productSalePlanningDiscountPercentage">درصد تخفیف برنامه ریزی دوره</param>
        /// <param name="saleConfigurationId">شناسه تنظیم فروش</param>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <returns></returns>
        public async Task<(decimal discountPercentage, decimal? calcedPrice)>
            GetCalcPriceInformationAsync(
                decimal? price,
                decimal? productSalePlanningDiscountPercentage,
                int saleConfigurationId,
                int customerClubId
            )
        {
            // TODO Cache saleConfigurationAttributes
            productSalePlanningDiscountPercentage ??= await _saleConfigurationAttributeService
                .GetCustomerClubSaleConfigurationAttribute(
                    customerClubId,
                    saleConfigurationId,
                    _applicationConfiguration.ApplicationId
                )
                .Select(x => x.DiscountPercentage).FirstOrDefaultAsync();

            return (productSalePlanningDiscountPercentage.Value,
                price * (1 - productSalePlanningDiscountPercentage.Value / 100));
        }

        /// <summary>
        /// برای سفارشات ثبت اولیه و و کامل ظرفیت باقی مانده باید به روز رسانی شود
        /// </summary>
        /// <param name="order">سفارش</param>
        /// <returns></returns>
        public async Task UpdateOrderProductSalesPlanningsCapacityAsync(Order order)
        {
            var orderItems =
                await _orderItemService.GetOrderItemsByOrderId(order.Id, true)
                    .Select(x => new
                    {
                        x.ProductSalesPlanning.MaxCapacity,
                        x.ProductSalesPlanningId,
                        x.Quantity
                    }).Where(x => x.MaxCapacity > 0).ToListAsync();

            foreach (var orderItem in orderItems)
            {
                await UpdateOrderProductSalesPlanningsCapacityAsync(orderItem.ProductSalesPlanningId);
            }
        }

        public async Task UpdateOrderProductSalesPlanningsCapacityAsync(int productSalesPlanningId)
        {
            var productSalesPlanning = await GetAsync(productSalesPlanningId, false);
            await UpdateAsync(productSalesPlanning, productSalesPlanning.Id);
        }

        public override async Task<ProductSalesPlanning> UpdateAsync(ProductSalesPlanning productSalesPlanning,
            object key, bool saveAfterUpdate = false, bool publishEvent = true)
        {
            // await _productSalesPlanningRepository.UpdateRemainingCapacity(productSalesPlanning.Id, _applicationConfiguration.ApplicationId);
            return await base.UpdateAsync(productSalesPlanning, key, saveAfterUpdate, publishEvent);
        }

        public override async Task<ProductSalesPlanning> UpdateAsync(ProductSalesPlanning productSalesPlanning,
            object key, List<string> props, bool modifyProps, bool saveAfterUpdate = false,
            bool publishEvent = true)
        {
            await _productSalesPlanningRepository.UpdateRemainingCapacity(productSalesPlanning.Id,
                _applicationConfiguration.ApplicationId);
            return await base.UpdateAsync(productSalesPlanning, key, props, modifyProps, saveAfterUpdate, publishEvent);
        }

        public async Task<(int? companyId, int? represenationId)> GetCompanyAndRepresentationIdAsync(
            int productSalePlaningId)
        {
            var res = await _productSalesPlanningRepository.GetAll(true).Where(x => x.Id == productSalePlaningId)
                .Select(x => new { companyId = x.Product.CompanyId, represenationId = x.RepresentationId })
                .FirstOrDefaultAsync();

            return (res?.companyId, res?.represenationId);
        }

        /// <summary>
        /// بیشترین فروش محصولات مرکز را
        /// بر اساس تعداد درخواست شده در پارامتر بر میگرداند
        /// </summary>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        public IQueryable<CompanyProductSalesPlanningDomainModel> GetBestSellers(int customerClubId)
        {
            return _productSalesPlanningRepository
                .GetBestSellers(_applicationConfiguration.ApplicationId, customerClubId);
        }


        /// <summary>
        ///محصولاتی که بیشترین تخفیف را دارند، بدون در نظر گرفتن مرکز بر می گرداند
        /// </summary>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        public IQueryable<CompanyProductSalesPlanningDomainModel> GetFullOfDiscounts(int customerClubId)
        {
            return _productSalesPlanningRepository
                .GetFullOfDiscounts(_applicationConfiguration.ApplicationId, customerClubId);
        }

        public IQueryable<CompanyProductSalesPlanningDomainModel> GetNewest(int customerClubId)
        {
            return _productSalesPlanningRepository.GetNewest(
                _applicationConfiguration.ApplicationId,
                customerClubId);
        }

        public Task<bool> ChecksProductSalesPlanningHaveCapacityAsync(int productSalesPlanningId)
        {
            return FindById(productSalesPlanningId)
                .AnyAsync(x =>
                    !x.RemainingCapacity.HasValue ||
                    x.RemainingCapacity > 0);
        }

        public Task<bool> HasDiscountPercentageChangedComparedToTheContractAsync(int productSalesPlanningId)
        {
            return _productSalesPlanningRepository.HasDiscountPercentageChangedComparedToTheContractAsync(
                productSalesPlanningId);
        }

        public async Task<IQueryable<ProductSalesPlanning>> GetUnFactoredProductSalesPlanningsAsync(
            int applicationId,
            int? companyId,
            int? representationId)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            return _productSalesPlanningRepository.GetByRepresentationFilter(
                applicationId,
                companyId,
                null,
                representationId,
                false,
                false,
                userAdditionalUserProp.CustomerClubId,
                null,
                true);
        }

        /// <summary>
        /// برسی می کند که آیا کاربر به برنامه ریزی دوره دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>bool</returns>
        public async Task<bool> ValidateUserHasAccessToProductSalesPlanningAsync(
            int productSalesPlanningId,
            int userId)
        {
            var productSalesPlanning =
                FindById(productSalesPlanningId, _applicationConfiguration.ApplicationId);

            return await productSalesPlanning
                .Select(x =>
                    x.ProductSalesPlanningStudent
                        .Any(c => c.StudentId == userId))
                .FirstOrDefaultAsync();
        }

        public bool CheckRepresentationHasAnyProductSalesPlanning(int representationId)
        {
            return _productSalesPlanningRepository.CheckRepresentationHasAnyProductSalesPlanning(representationId);
        }

        /// <summary>
        /// بررسی می کند که کاربر دوره آزمون استغال را خریده است
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> CheckUserPurchasedEmploymentTestCourse()
        {
            // دریافت شناسه دوره آزمون اشتغال
            var employmentTestProductId = await
                _appConfigService.GetValueByApplicationIdAsync(
                    ApplicationConfigKey.YashilCommerceEmploymentTestProductSalesPlanningId,
                    _applicationConfiguration.ApplicationId);

            return await _productSalesPlanningRepository.CheckCurrentUserPurchasesByProductSalesPlanningIdAsync(
                employmentTestProductId);
        }

        /// <summary>
        /// این متد بررسی می کند آیا کاربر دوره آزمون استعداد یابی را خریده و در آزمون استعداد یابی شرکت کرده است؟
        /// همچنین بررسی میکند که کاربر درحال خرید آن است منجر به اشتغال است یا نه؟ و آیا یارانه اشتغال به کاربر تعلق
        /// می گیرد یا نه؟
        /// در صورتی که کاربر لاگین نباشد
        /// null
        /// بر میگرداند
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns>EmploymentSubsidyDto || null</returns>
        public async Task<EmploymentSubsidyDto> CheckEmploymentSubsidyIsGrantedToCurrentUserAndPspIsEmployment(
            int productSalesPlanningId)
        {
            if (_userPrincipal.Id <= 0)
                return null;

            var additionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var isEmploymentProduct = await IsEmploymentProductOrNotAsync(productSalesPlanningId);

            return new EmploymentSubsidyDto
            {
                HasUserPurchasedExam = additionalUserProp.HasUserPurchasedEmploymentExam,
                HasUserTakenExamBefore = additionalUserProp.ParticipatedInhumaxTest,
                IsEmploymentProduct = isEmploymentProduct,
                IsEmploymentSubsidyGrantedToUser = additionalUserProp.HasUserPurchasedEmploymentExam.HasValue
                                                   && additionalUserProp.HasUserPurchasedEmploymentExam.Value
                                                   && additionalUserProp.ParticipatedInhumaxTest && isEmploymentProduct
            };
        }

        /// <summary>
        /// این متد بررسی می کند که آیا برنامه ریزی دوره مربوط به یک دوره منجر به اشتغال است یا نه؟
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns>bool</returns>
        public async Task<bool> IsEmploymentProductOrNotAsync(int productSalesPlanningId)
        {
            return await FindById(productSalesPlanningId)
                .AnyAsync(psp => psp.Product.ProductType == (int)ProductTypeEnum.Employment);
        }

        /// <summary>
        /// این متد عملیات جستجو براساس کلید در بین عنوان برنامه ریزی های دوره فعال را انجام می دهد
        /// </summary>
        /// <param name="searchKey">رشته کلید</param>
        /// <returns></returns>
        public async Task<IQueryable<ProductSalesPlanningSearchDto>> SearchInProductSalesPlanningTitleAsync(
            string searchKey)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            return _productSalesPlanningRepository.GetIsActiveProductSalesPlanningsByApplicationId(
                    _applicationConfiguration.ApplicationId,
                    userAdditionalUserProp.CustomerClubId
                )
                .Where(psp => psp.Product.Title.Contains(searchKey))
                .OrderBy(psp => psp.CreationDate)
                .Select(psp => new ProductSalesPlanningSearchDto
                {
                    ProductSalesPlanningId = psp.Id,
                    ProductSalesPlanningTitle = psp.Product.Title,
                    RepresentationId = psp.RepresentationId,
                    RepresentationTitle = psp.Representation.Title,
                    CompanyId = psp.Product.Company.Id,
                    CompanyTitle = psp.Product.Company.Title
                }).AsNoTracking().Take(5);
        }

        /// <summary>
        /// دریافت برنامه ریزی دوره
        /// productSalesPlanning
        /// مرتبط به شناسه تگ های ارسالی
        /// این سرویس برنامه ریزی دوره ای را که شرایط فعال بودن و نمایش در سامانه را دارا باشند 
        /// و به شناسه تگ های ارسالی مرتبط باشند را بر می گرداند
        /// </summary>
        /// <param name="tagIds">شناسه تگ ها</param>
        /// <param name="customerClubId">شناسه باشگاه مشتری</param>
        /// <returns>ProductSalesPlanning</returns>
        public IQueryable<ProductSalesPlanning> GetProductSalesPlanningRelatedToTagIds(List<int> tagIds,
            int customerClubId)
        {
            return _productSalesPlanningRepository.GetProductSalesPlanningRelatedToTagId(tagIds)
                .Where(_productSalesPlanningRepository
                    .GetProductSalePlanningIsActiveFilter(_applicationConfiguration.ApplicationId, customerClubId, null,
                        false))
                .Take(10);
        }


        /// <summary>
        /// این متد جهت دریافت تعداد برنامه ریزی دوره های فعال که
        /// بر اساس شناسه مرکز و شناسه
        /// categoryGroup
        /// گروه بندی شده اند کاربرد دارد
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <param name="companyIds">شناسه مراکز</param>
        /// <param name="representationId">شناسه نمایندگی</param>
        /// <param name="cityId">شناسه شهر</param>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <param name="query"></param>
        /// <returns><List<GroupedActiveProductSalesPlanningDto>></returns>
        public async Task<List<GroupedActiveProductSalesPlanningDto>> GetGroupedActiveProductSalesPlanningCountAsync(
            int? applicationId,
            int? categoryId,
            List<int> companyIds,
            int? representationId,
            int? cityId,
            int? categoryGroupId,
            IQueryable<ProductSalesPlanning> query)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<ProductSalesPlanning>.ActiveProductSalesPlanningCountGroupedByCompanyKey,
                applicationId, categoryId, companyIds, representationId, cityId, categoryGroupId);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                return await query.GroupBy(psp => new
                { psp.Product.CompanyId, psp.Product.DefaultCategory.CategoryGroupId })
                    .Select(x => new GroupedActiveProductSalesPlanningDto
                    {
                        CompanyId = x.Select(c => c.Product.CompanyId).FirstOrDefault(),
                        ProductSalesPlanningCount = x.Select(c => c.Id).Count()
                    }).ToListAsync();
            });
        }

        /// <summary>
        ///  برنامه ریزی فعال با فیلتر های
        /// تاریخ انتهای برنامه ریزی منقضی نشده باشد و ..
        /// و برنامه ریزی که دارای تنظیم فروش فعال با تاریخ منقضی نشده ..  
        /// </summary>
        /// <param name="applicationId">برنامه</param>
        /// <param name="customerClubId">باشگاه مشتریان یعنی برنامه ریزی دوره باید ویژگی تنظیم فروش فعال در این باشگاه داسته باشد</param>
        /// <param name="categoryGroupId">گروه دسته بندی محصول</param>
        /// <param name="isEmploymentCategoryGroup">آیا گروه دسته بندی محصول اشتغال است؟</param>
        /// <returns></returns>
        public Expression<Func<ProductSalesPlanning, bool>> GetProductSalePlanningIsActiveFilter(int? applicationId,
            int customerClubId, int? categoryGroupId,
            bool isEmploymentCategoryGroup)
        {
            return _productSalesPlanningRepository.GetProductSalePlanningIsActiveFilter(applicationId, customerClubId,
                categoryGroupId, isEmploymentCategoryGroup);
        }

        public async Task<(bool isHeld, string message)> CheckProductSalePlanningHasBeenHeld(int productSalesPlanningId)
        {
            var query = FindById(productSalesPlanningId);

            var productSalePlanningHasStudents = await query.AnyAsync(x => x.ProductSalesPlanningStudent.Any());
            if (productSalePlanningHasStudents)
            {
                return (true,
                    "کاربر گرامی، به دلیل اینکه به این برنامه ریزی دوره دانشجو اساین شده است، امکان اختصاص گروه وجود ندارد.");
            }

            var productSalePlanningHasExams = await query.AnyAsync(x =>
                x.ExamCategory.Any(ec => ec.Exam.Any(e => e.UserExam.Any())));
            if (productSalePlanningHasExams)
            {
                return (true,
                    "کاربر گرامی، به دلیل اینکه برای این برنامه ریزی دوره آزمون تعریف شده است، امکان اختصاص گروه وجود ندارد.");
            }

            var productSalePlanningHasVirtualClasses = await query.AnyAsync(x =>
                x.VirtualClass.Any(vc => vc.VirtualClassStudent.Any()));
            if (productSalePlanningHasVirtualClasses)
            {
                return (true,
                    "کاربر گرامی، به دلیل اینکه برای این برنامه ریزی دوره کلاس مجازی تعریف شده است، امکان اختصاص گروه وجود ندارد.");
            }

            return (false, string.Empty);
        }

        /// <summary>
        /// بررسی می کند که آیا کاربر به یک برنامه ریزی دوره دسترسی دارد یا نه؟
        /// باید مرکز کاربر و مرکز برنامه ریزی دوره ای که شناسه آن ارسال شده است یکی باشد.
        /// </summary>
        /// <param name="productSalesPlaningId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserHasAccessToProductSalesPlanning(int productSalesPlaningId, int userId)
        {
            var additionalUserProp = _additionalUserPropService.GetByUserId(_userPrincipal.Id);

            if (!additionalUserProp.CompanyId.HasValue)
                return true;

            var pspCompanyId = await FindById(productSalesPlaningId, _userPrincipal.ApplicationId)
                                            .Select(psp => psp.Product.CompanyId)
                                                .FirstOrDefaultAsync();

            if (pspCompanyId == additionalUserProp.CompanyId)
                return true;

            return false;
        }

        public ModifyPropertiesDescriptor GetPropertiesForApplyOrIgnoreUpdateProductSalesPlanning(ProductSalesPlanning entity,
            ProductSalesPlanningBaseEditModel editModel)
        {
            ModifyPropertiesDescriptor applyOrIgnoreUpdate = new ModifyPropertiesDescriptor();
            PropertyInfo[] properties = editModel.GetType().GetProperties();
            applyOrIgnoreUpdate.Properties = ((IEnumerable<PropertyInfo>)properties).Select<PropertyInfo, string>((Func<PropertyInfo, string>)(t => t.Name)).ToList<string>();
            applyOrIgnoreUpdate.UpdateSelectedFieldsOrIgnoreTheme = true;
            return applyOrIgnoreUpdate;
        }


        /// <summary>
        /// این متد بررسی می کند که تاریخ شروع و پایان یک آزمون بعد از تاریخ پایان دوره نباشد
        /// در صورتی که دوره شامل تاریخ پایان نباشد بررسی را انجام نمی دهد
        /// </summary>
        /// <param name="examCategoryId"></param>
        /// <param name="examStartDateInt"></param>
        /// <param name="examEndDateInt"></param>
        /// <returns></returns>
        public async Task<(bool isValid, string message)> ValidateProductSalesPlaningEndDateByExamCategoryIdAsync(int examCategoryId, int examStartDateInt, int examEndDateInt)
        {
            var productSalesPlanningExpirationInfo = await _productSalesPlanningRepository.GetProductSalesPlanningByExamCategoryId(examCategoryId)
                .Select(psp => new
                {
                    ConsiderEndDate = psp.ConsiderEndDate,
                    EndDate = psp.EndDate,
                    StartDateInt = psp.StartDate

                }).FirstOrDefaultAsync();

            if (productSalesPlanningExpirationInfo == null)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(ProductSalesPlanning),
                    "به هنگام ایجاد آزمون خطایی رخ داده است برنامه ریزی دوره ای برای شناسه دسته بندی آزمون ارسالی یافت نشد. ", null);

                return (false, "کاربر گرامی، به هنگام انجام عملیات خطای رخ داده است لطفا با مدیر سیستم تماس بگیرید.");
            }

            if (productSalesPlanningExpirationInfo.StartDateInt.HasValue &&
                productSalesPlanningExpirationInfo.StartDateInt.Value > examStartDateInt)
                return (false, "کاربر گرامی تاریخ شروع آزمون نمی تواند قبل از تاریخ شروع دوره باشد.");

            if (productSalesPlanningExpirationInfo.StartDateInt.HasValue &&
               productSalesPlanningExpirationInfo.StartDateInt.Value > examEndDateInt)
                return (false, "کاربر گرامی تاریخ پایان آزمون نمی تواند قبل از تاریخ شروع دوره باشد.");

            if (!productSalesPlanningExpirationInfo.ConsiderEndDate)
                return (true, string.Empty);

            if (!productSalesPlanningExpirationInfo.EndDate.HasValue)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(ProductSalesPlanning),
                    "به هنگام ایجاد آزمون خطایی رخ داده است برنامه ریزی دوره ای شامل تاریخ پایان می شود اما برای آن تاریخی در نظر گرفته نشده است. ", null);

                return (false, "کاربر گرامی، به هنگام انجام عملیات خطای رخ داده است لطفا با مدیر سیستم تماس بگیرید.");
            }



            if (examStartDateInt > productSalesPlanningExpirationInfo.EndDate)
                return (false, "کاربر گرامی تاریخ برگزاری آزمون نمی تواند بعد از تاریخ پایان دوره باشد");


            if (examEndDateInt > productSalesPlanningExpirationInfo.EndDate)
                return (false, "کاربر گرامی تاریخ پایان آزمون نمی تواند بعد از تاریخ پایان دوره باشد");


            return (true, string.Empty);
        }

        /// <summary>
        /// این متد جهت دریافت اطلاعات مربوط به مرکز و نمایندگی یک برنامه ریزی دوره کاربرد دارد
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(int companyId, int? representationId)> GetProductSalesPlanningCompanyAndRepresentationIdAsync(int productSalesPlanningId)
        {
            var pspInformation = await FindById(productSalesPlanningId, _userPrincipal.ApplicationId)
                .Select(psp => new
                {
                    companyId = psp.Product.CompanyId,
                    representationId = psp.RepresentationId,

                }).FirstOrDefaultAsync();

            if (pspInformation == null)
                throw new Exception($"prodcutSalesPlanning id is not valid pspId:{productSalesPlanningId}");

            return (pspInformation.companyId, pspInformation.representationId);
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر جاری به شناسه برنامه ریزی دوره ارسال شده دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> HasCurrentUserAccessToProductSalesPlanningAsync(int productSalesPlanningId)
        {
            if (_userPrincipal.Id == 0)
                throw new Exception("current user is not authenticated user id is 0");

            var additionalUserProp = await _additionalUserPropService.FindByUserId(_userPrincipal.Id).Select(aup => new
            {
                UserCompanyId = aup.CompanyId,
                UserRepresentationId = aup.RepresentationId

            }).FirstOrDefaultAsync();

            if (additionalUserProp == null)
                throw new Exception("additional user prop is equal to null");

            var pspInfo = await GetProductSalesPlanningCompanyAndRepresentationIdAsync(productSalesPlanningId);

            if (pspInfo.companyId == 0)
                throw new Exception($"productSalesPlanning with id {productSalesPlanningId} is not defined");

            if (pspInfo.representationId.HasValue && additionalUserProp.UserRepresentationId.HasValue &&
                pspInfo.representationId.Value == additionalUserProp.UserRepresentationId.Value)
                return true;

            if (!additionalUserProp.UserCompanyId.HasValue)
                return true;

            if (additionalUserProp.UserCompanyId.Value == pspInfo.companyId)
                return true;

            return false;
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر جاری به ایتم سفارش ارسال شده دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        public async Task<bool> HasCurrentUserAccessToOrderItemProductSalesPlanningAsync(int orderItemId)
        {
            var orderItem = await _orderItemService.GetAsync(orderItemId, true);

            if (orderItem == null || orderItem.ProductSalesPlanningId == 0)
                return false;


            if (!await HasCurrentUserAccessToProductSalesPlanningAsync(orderItem.ProductSalesPlanningId))
                return false;

            return true;
        }

        /// <summary>
        /// آیا دوره از نوع نیاز به تایید تامین کننده قبل از پرداخت است یا نه؟
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        public Task<bool> IsProductNeedToSupplierApprovalBeforePaymentAsync(int productSalesPlanningId)
        {
            return _productSalesPlanningRepository.GetAll(true, _applicationConfiguration.ApplicationId)
                .Where(x => x.Id == productSalesPlanningId)
                .AnyAsync(psp =>
                    psp.SaleConfiguration.RequireSupplierApprovalBeforePayment ||
                    psp.Product.RequireSupplierApprovalBeforePayment);
        }

        /// <summary>
        /// دریاتف برنامه ریزی دوره های از نوع
        /// نیاز به تایید تامین کننده قبل از پرداخت هستند
        /// و حداقل یک سفارش برای آن ها ثبت شده است.
        /// بر اساس شناسه مرکز و نمایندگی
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="applicationId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        public async Task<IQueryable<ProductSalesPlanning>> GetProductSalesPlanningThatHasAnyOrderWithWaitingForSupplierApprovalAsync(
            int? companyId, int applicationId, int? representationId = null)
        {
            var pspQuery = await GetByRepresentationFilterAsync(applicationId, companyId, null,
                representationId, true, false,
                true, filterByOrderItemRepresentationId: true);

            return pspQuery.Where(psp =>
                 psp.OrderItem.Any(oi => (
                     (oi.ProductSalesPlanning.Product.RequireSupplierApprovalBeforePayment ||
                                          (oi.ProductSalesPlanning.Product.Company.SaleConfiguration
                                              .Any(sc => sc.IsActive && sc.RequireSupplierApprovalBeforePayment ||
                                                         oi.ProductSalesPlanning.Representation.SaleConfiguration
                                                             .Any(sc => sc.IsActive && sc.RequireSupplierApprovalBeforePayment)))))));
        }

        /// <summary>
        /// دریافت برنامه ریزی دوره های مربوط به یک مرکز یا نمایندگی که
        /// گزینه
        /// RequireSupplierApprovalBeforePayment
        /// آن ها فعال باشد
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="applicationId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        public async Task<IQueryable<ProductSalesPlanning>> GetProductSalesPlanningWithRequireSupplierApprovalBeforePaymentAsync(
            int? companyId, int applicationId, int? representationId = null)
        {
            var pspQuery = await GetByRepresentationFilterAsync(applicationId, companyId, null,
                representationId, true, false,
                true);

            return pspQuery.Where(psp => psp.Product.RequireSupplierApprovalBeforePayment ||
                                  psp.Representation.SaleConfiguration.Any(sc =>
                                      sc.IsActive && sc.RequireSupplierApprovalBeforePayment) ||
                                  psp.Product.Company.SaleConfiguration.Any(sc =>
                                      sc.IsActive && sc.RequireSupplierApprovalBeforePayment));
        }

        /// <summary>
        /// این متد مقدار حذف برنامه ریزی دوره را برابر با صحیح می کند. رکورد در دریتابیس موجود است
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        public Task<int> Archive(int productSalesPlanningId)
        {
            return _productSalesPlanningRepository.ArchiveAsync(productSalesPlanningId);
        }

        /// <summary>
        ///  می کند false این متد مقدار حذف برنامه ریزی دوره را برابر با  
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        public Task<int> RestoreFromArchive(int productSalesPlanningId)
        {
            return _productSalesPlanningRepository.RestoreFromArchiveAsync(productSalesPlanningId);
        }

        public async Task<List<ProductSalesPlanningDto>> GetUserNotViewedMediaSessionProductInfoAsync(List<MediaSessionDto> mediaSessionDtos)
        {
            var sessionCodes = mediaSessionDtos.Select(x => x.Code).ToList();

            var viewedCodes = (await _mediaSessionViewingHistoryRepository
                .GetUserMediaSessionsCodesViewingHistory(sessionCodes, _userPrincipal.Id)
                .ToListAsync())
                .ToHashSet();

            var notViewedProductIds = mediaSessionDtos
                .Where(x => !viewedCodes.Contains(x.Code))
                .Select(x => x.ProductSalesPlanningId)
                .Distinct()
                .ToList();

            var result = await GetAll(true)
                .Where(x => notViewedProductIds.Contains(x.Id) && x.Product != null)
                .Select(psp => new ProductSalesPlanningDto
                {
                    Id = psp.Id,
                    ProductTitle = psp.Product.Title,
                })
                .ToListAsync();

            return result;
        }

        public Task UpdateProductSalesPlaningStartAndEndDateAsync(int productSalesPlanningId, int startDateInt, int endDateInt)
        {
            string startDateShamsiString = startDateInt.ToString().Insert(4, "/").Insert(7, "/");
            string endDateShamsiString = endDateInt.ToString().Insert(4, "/").Insert(7, "/");

            return GetAll(true)
                .Where(psp => psp.Id == productSalesPlanningId &&
                              (!psp.StartDate.HasValue ||
                               !psp.EndDate.HasValue ||
                               psp.EndDate.Value != endDateInt ||
                               psp.StartDate.Value != startDateInt))
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(c => c.StartDate, startDateInt)
                        .SetProperty(c => c.EndDate, endDateInt)
                        .SetProperty(c => c.StartDateString, startDateShamsiString)
                        .SetProperty(c => c.EndDateString, endDateShamsiString));
        }

        public IQueryable<ProductSalesPlanning> GetActiveProductSalesPlanningThatAtlastHasOnePsPStudent(int? companyId, int? representationId = null)
        {
            var ongoingProductSalesPlanningsQuery = GetAll(true);

            if (companyId is > 0)
                ongoingProductSalesPlanningsQuery = ongoingProductSalesPlanningsQuery.Where(x => x.Product.CompanyId == companyId.Value ||
                    (x.Product.Company.ParentId.HasValue &&
                     x.Product.Company.ParentId.Value == companyId));

            if (representationId is > 0)
                ongoingProductSalesPlanningsQuery = ongoingProductSalesPlanningsQuery
                    .Where(x =>
                        x.RepresentationId.HasValue && x.RepresentationId == representationId.Value);


            return ongoingProductSalesPlanningsQuery
                .Where(psp => psp.ProductSalesPlanningStudent.Any() &&
                                        (psp.HomeWorkCategory.Any(x => x.HomeWork.Any()) ||
                                                    psp.VirtualClass.Any() ||
                                                    psp.MediaCategory.Any(x => x.MediaSession.Any()) ||
                                                    psp.ExamCategory.Any(x => x.Exam.Any())
                                        )
                       );

        }


        public Task<List<string>> GetProductSalesPlanningRepresentationTitleListForFillInUploadedProductItemXlsValidationAsync(int companyId)
        {
            return GetAll(true)
                .Where(psp => psp.Product.CompanyId == companyId && psp.Product.VisibleIndividually &&
                              psp.Product.ApplicationId == _userPrincipal.ApplicationId && psp.Published)
                .Select(s => s.Representation.Title + "/#" + s.RepresentationId).Distinct().ToListAsync();
        }

        public IQueryable<ProductSalesPlanning> GetTeacherLatestActiveProductSalesPlanning(int teacherUserId, int? top)
        {
            var query = GetActiveProductSalesPlanningThatAtlastHasOnePsPStudent(null)
                .Where(x => x.ProductSalesPlanningTeacher.Any(pspTeacher => pspTeacher.TeacherUserId == teacherUserId))
                .OrderByDescending(x => x.Id);

            if (top is > 0)
                return query.Take(top.Value);

            return query.Take(5);
        }

        public IQueryable<ProductSalesPlanning> GetLatestActiveProductSalesPlanning(int? companyId, int? representationId, int? top)
        {
            return GetActiveProductSalesPlanningThatAtlastHasOnePsPStudent(companyId, representationId)
                .Take(top ?? 5);
        }

        public Task<List<ActiveProductSalesPlanningCountGroupedDto>> GetActiveProductSalesPlanningCountGroupByCompanyAsync(int? companyId)
        {
            return GetActiveProductSalesPlanningThatAtlastHasOnePsPStudent(companyId, null)
                .GroupBy(x => new
                {
                    x.Product.CompanyId,
                    x.Product.Company.Title

                }).Select(s => new ActiveProductSalesPlanningCountGroupedDto
                {
                    KeyTitle = s.Key.Title,
                    ActiveProductSalesPlanningCount = s.Count()

                }).ToListAsync();
        }

        public Task<List<ActiveProductSalesPlanningCountGroupedDto>> GetTeacherActiveProductSalesPlanningCountGroupByRepresentationAsync(int teacherUserId)
        {
            return GetTeacherLatestActiveProductSalesPlanning(teacherUserId, null)
                .GroupBy(x => new
                {
                    x.RepresentationId,
                    x.Representation.Title,
                }).Select(s => new ActiveProductSalesPlanningCountGroupedDto
                {
                    KeyTitle = s.Key.Title,
                    ActiveProductSalesPlanningCount = s.Count()

                }).ToListAsync();

        }

        public Task<bool> CheckExistProductSalePlanningBySaleConfigurationId(int saleConfigurationId)
        {
            return _productSalesPlanningRepository
                .FindBySaleConfigurationId(saleConfigurationId)
                .AnyAsync();

        }
    }
}