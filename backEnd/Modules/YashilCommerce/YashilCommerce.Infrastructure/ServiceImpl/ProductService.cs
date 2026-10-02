using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using YashilTms.Core.Services;
using System;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Yashil.Common.SharedKernel.Logging;
using Microsoft.Extensions.Logging;
using Yashil.Common.Infrastructure.AppEvents;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Enums;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using YashilDms.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ProductService : GenericService<Product, int>, IProductService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IProductRepository _productRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ICourseService _courseService;
        private readonly IYashilLogger<Product> _logger;
        private readonly IAppDocumentService _appDocumentService;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly IAppConfigService _appConfigService;
        private readonly ISpecificationOptionService _specificationOptionService;
        private readonly IAppEntityService _appEntityService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IConfiguration _configuration;




        public ProductService(IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IUserPrincipal userPrincipal,
            ICourseService courseService, IYashilLogger<Product> logger,
            IEventPublisher eventPublisher, IAppDocumentService appDocumentService,
            StoreConfiguration storeConfiguration, IAppConfigService appConfigService,
            ISpecificationOptionService specificationOptionService,
            IAppEntityService appEntityService, IProductSalesPlanningService productSalesPlanningService,
            IProductCategoryService productCategoryService, IAdditionalUserPropService additionalUserPropService, IConfiguration configuration)
            : base(unitOfWork, productRepository, userPrincipal, eventPublisher)
        {
            _productRepository = productRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _appDocumentService = appDocumentService;
            _storeConfiguration = storeConfiguration;
            _appConfigService = appConfigService;
            _specificationOptionService = specificationOptionService;
            _appEntityService = appEntityService;
            _productSalesPlanningService = productSalesPlanningService;
            _productCategoryService = productCategoryService;
            _additionalUserPropService = additionalUserPropService;
            _configuration = configuration;
            _courseService = courseService;
            _logger = logger;
        }

        public override async Task<Product> UpdateAsync(
            Product t,
            object key,
            List<string> props,
            bool modifyProps, bool saveAfterUpdate = false, bool publishEvent = true)
        {
            if (t.Course != null)
            {
                await _courseService.UpdateAsync(t.Course, t.Course.Id);
            }

            return await base.UpdateAsync(t, key, props, modifyProps, saveAfterUpdate, publishEvent);
        }

        public IQueryable<Product> GetByCustomFilter(string title, string code, int? categoryId, int? companyId,
            int? appEntityId, int? productTemplateId, int? giftCardTypeId, int? downloadActivationType,
            int? recurringCyclePeriodType, int? taxCategoryId, int? basePriceUnitType, int? basePriceBaseUnitType,
            bool? showArchives)
        {
            return _productRepository.GetByCustomFilter(title, code, categoryId, companyId, appEntityId,
                productTemplateId, giftCardTypeId, downloadActivationType, recurringCyclePeriodType, taxCategoryId,
                basePriceUnitType, basePriceBaseUnitType, showArchives);
        }

        public IQueryable<Product> GetByCategoryIdAndCompanyId(int categoryId, int? companyId, bool hierarchical = true)
        {
            return _productRepository.GetByCategoryIdAndCompanyId(categoryId, companyId, hierarchical);
        }

        public int GetProductCompanyId(int productId)
        {
            return _productRepository.GetProductCompanyId(productId);
        }

        public IQueryable<Product> GetCategoryProducts(int applicationId, int? categoryId, int? companyId, int? cityId,
            bool loadSubCategories, int count = 20)
        {
            return _productRepository.GetCategoryProducts(applicationId, categoryId, companyId, cityId,
                loadSubCategories, count);
        }

        public IQueryable<Product> GetRepresentationProductsByCategoryId(int? companyId, int? representationId,
            int? categoryId,
            bool hierarchical)
        {
            return _productRepository.GetRepresentationProductsByCategoryId(companyId, representationId, categoryId,
                hierarchical);
        }

        /// <summary>
        /// این متد برای ذخیره و یا به روز رسانی اطلاعات محصول بارگذاری شده از جدول 
        /// UploadedProductItem
        /// به جدول های 
        /// product  - ProductSalePlaning - DependentProductSale - ProductCategory
        /// میباشد
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public Task<int> AddDataFromUploadedProductItemAsync(int applicationId, int userId, int companyId)
        {
            try
            {
                var res = _productRepository.AddDataFromUploadedProductItemAsync(applicationId, userId, companyId);
                _eventPublisher.EntityInsertedAsync<Product, int>(null);
                _eventPublisher.EntityInsertedAsync<ProductSalesPlanning, int>(null);
                _eventPublisher.EntityInsertedAsync<Company, int>(null);
                _eventPublisher.EntityInsertedAsync<Category, int>(null);
                return res;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(1, Events.Insert.ToString()), nameof(Product),
                    "خطا در اجرای sp AddDataFromUploadedProductItem", e);
                return null;
            }
        }

        /// <summary>
        /// اطلاعات کامل محصول مربوط به مرکز را بر میگرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public IEnumerable<ProductDetailDomainModel> GetCompanyConfirmedAndPublishedProducts(int companyId)
        {
            return _productRepository.GetCompanyConfirmedAndPublishedProducts(companyId);
        }

        public Task<List<ProductSimpleDomainModel>> GetCompanyConfirmedAndPublishedProductSimpleDomainModelAsync(
            int companyId) =>
        _productRepository.GetCompanyConfirmedAndPublishedProductSimpleDomainModelAsync(companyId);

        /// <summary>
        /// بررسی می کند که کد محصول تکراری است یا خیر 
        /// </summary>
        /// <param name="code">کد محصول</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public bool IsProductCodeDuplicate(string code, int applicationId)
        {
            return _productRepository.IsProductCodeDuplicate(code, applicationId);
        }

        public async Task<string> SaveProductDetailPagesFileAndExtractToPath(IFormFile productDetailPagesFile)
        {
            // ذخیره کردن فایل در دیسک
            var uploadedDocumentResult = await _appDocumentService.SaveDocumentAsync(
                (int?)DocumentCategoryEnum.UploadProductDetailPages, null, null, null, null, (int)AppEntityEnum.Product,
                (int)DocumentTypeEnum.UploadProductDetailPages,
                null, (int)AppEntityEnum.UploadedVoucher, productDetailPagesFile);
            if (uploadedDocumentResult.appDocument == null)
            {
                return "بروز خطا در ذخیره سازی فایل";
            }

            // دریافت رکورد appDocument
            uploadedDocumentResult.appDocument =
                await _appDocumentService.GetAsync(uploadedDocumentResult.appDocument.Id, true);
            // دریافت ادرس فایل
            var path = _appDocumentService.GetFilePath(uploadedDocumentResult.appDocument);
            try
            {
                // اکسترکت کردن در آدرس مورد نظر
                ZipFile.ExtractToDirectory(path, _storeConfiguration.ProductDetailPagesPath, true);
                _logger.InsertLog(LogLevel.Information, (int)Events.Insert, nameof(Product),
                    $"UploadedProductDetailPagesFile inserted filename {productDetailPagesFile.Name}", null);
                return string.Empty;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(Product),
                    $"SaveProductDetailPagesFileAndExtractToPath error filename {productDetailPagesFile.Name}", e);
                return "بروز خطا در ذخیره سازی فایل";
            }
        }

        public Task<AppDocument> GetProductPicture(int productId, int applicationId)
        {
            return _appDocumentService.GetObjectDocumentsWithDocFormatByApplicationId(nameof(Product), productId,
                (int)DocumentCategoryEnum.ProductImage, (int)DocFormatEnum.Image, applicationId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// دریافت اطلاعات محصول براساس شناسه برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        public Task<Product> GetByproductSalesPlanningIdAsync(int productSalesPlanningId)
        {
            return _productRepository.GetByproductSalesPlanningIdAsync(productSalesPlanningId);
        }

        public IQueryable<Product> FindById(int productId, bool readOnly = false)
        {
            return _productRepository.FindById(productId, readOnly);
        }

        public string ValidateProductCodeBeforeInsert(string code)
        {
            // اگر کد محصولات اپلیکیشن ضروری باشد و کد محصول پر شده توسط کاربر مقدار نداشته باشد
            if (_storeConfiguration.RequireProductCode && string.IsNullOrEmpty(code))
            {
                return "کاربر گرامی کد محصول باید مقدار داشته باشد.";
            }

            // اگر کد بسته مقدار داشت
            if (!string.IsNullOrEmpty(code))
            {
                // بررسی می کند که کد محصول وارد شده تکراری است یا خیر 
                var isProductCodeDuplicate =
                    IsProductCodeDuplicate(code, _userPrincipal.ApplicationId);
                if (isProductCodeDuplicate)
                {
                    return "کاربر گرامی کد محصول وارد شده تکراری می باشد، لطفا کد محصول را تغییر دهید.";
                }
            }

            return string.Empty;
        }

        public async Task SetEntityDefaultPropsAsync(Product entity, int taxCategoryId, bool isTaxExempt)
        {
            if (isTaxExempt)
            {
                // دسته بندی معاف از مالیاتی پیش فرض دوره که معاف از مالیات ارزش افزوده می باشد
                entity.TaxCategoryId =
                    await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilDefaultTaxExemptTaxCategoryId);
            }

            if (entity.TaxCategoryId == 0)
            {
                // دسته بندی مالیاتی پیش فرض دوره که دارای مالیات ارزش افزوده می باشد
                entity.TaxCategoryId =
                    await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilDefaultCourseTaxCategoryId);
            }
            else
            {
                entity.TaxCategoryId = taxCategoryId;
            }

            var lmsApplicationId = _configuration.GetSection("ApplicationsId:Lms").Get<int>();

            if (lmsApplicationId == 0)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(SetEntityDefaultPropsAsync),
                    "مقدار appliactionId در appsetting برای سامانه Lms معتبر نمی باشد.", new Exception());

                throw new Exception("Invalid application Id for lms in appsetting");
            }


            if (_userPrincipal.ApplicationId == lmsApplicationId)
                entity.Published = true;

        }



        public void SetProductDefaultCategory(Product entity, List<int> categories)
        {
            if (categories.Count > 0)
            {
                entity.DefaultCategoryId = categories.FirstOrDefault();
            }
        }

        public void SetProductCategories(Product entity, List<int> categories)
        {
            SetProductDefaultCategory(entity, categories);
            foreach (var categoryId in categories)
            {
                entity.ProductCategory.Add(new ProductCategory
                {
                    CreateBy = _userPrincipal.Id,
                    CreatorOrganizationId = _userPrincipal.OrganizationId,
                    CreationDate = DateTime.Now,
                    ApplicationId = _userPrincipal.ApplicationId,
                    CategoryId = categoryId
                });
            }
        }

        public void SetProductSpecificationOption(Product entity, List<int> categories)
        {
            var specificationOptions = _specificationOptionService.GetByCategoryIds(categories).ToList();
            var specifications = specificationOptions.Select(x => x.SpecificationId);
            foreach (var specificationId in specifications)
            {
                var defaultSpecificationOption =
                    specificationOptions.FirstOrDefault(x => x.SpecificationId == specificationId && x.IsDefault) ??
                    specificationOptions.FirstOrDefault(x => x.SpecificationId == specificationId);

                if (defaultSpecificationOption != null)
                {
                    entity.ProductSpecificationOption.Add(new ProductSpecificationOption()
                    {
                        SpecificationOptionId = defaultSpecificationOption.Id,
                        AttributeType = 1084, //:TODO Fix This Id
                        CreateBy = _userPrincipal.Id,
                        CreatorOrganizationId = _userPrincipal.OrganizationId,
                        CreationDate = DateTime.Now,
                        ApplicationId = _userPrincipal.ApplicationId,
                    });
                }
            }
        }

        public void SetDependentProductSalePlanning(Product entity, int salesModelType, decimal minPrice,
            decimal maxPrice)
        {
            // check if the sales model for sale in all representation is selected or not
            if (salesModelType == (int)SalesModelTypes.SalesAtAllRepresentations)
            {
                // if min and max product are equal the sales planing price must be equal to them
                // otherwise it should be 0 and the representation will edit it
                var price = maxPrice == minPrice ? maxPrice : 0;
                // create new sales planing instance
                var productSalesPlanning = CreateProductSalesPlanningObject(null, Convert.ToInt32(price));
                // add sales planning instance to te company
                entity.ProductSalesPlanning.Add(productSalesPlanning);
                // if the product is group all product in the group should have just one sales planing
                // check the product in group has sales planing or not
                // if not add a sales planing to them
            }
        }

        public ProductSalesPlanning CreateProductSalesPlanningObject(int? productId, int price)
        {
            var psp = new ProductSalesPlanning
            {
                CreateBy = _userPrincipal.Id,
                CreatorOrganizationId = _userPrincipal.OrganizationId,
                CreationDate = DateTime.Now,
                ApplicationId = _userPrincipal.ApplicationId,
                Price = Convert.ToInt32(price),
                RunType = (int)RunTypeEnum.Class
            };
            if (productId.HasValue)
            {
                psp.ProductId = productId.Value;
            }

            return psp;
        }

        public async Task CustomMapBeforeInsertAsync(Product entity, List<int> categories, int taxCategoryId,
            bool isTaxExempt, int salesModelType, decimal minPrice, decimal maxPrice, int? appEntityId)
        {
            await SetEntityDefaultPropsAsync(entity, taxCategoryId, isTaxExempt);

            SetProductCategories(entity, categories);
            SetProductSpecificationOption(entity, categories);
            SetDependentProductSalePlanning(entity, salesModelType, minPrice, maxPrice);
            if (entity.Course != null)
            {
                var courseAppEntityId = await _appEntityService.GetIdByTitleAsync("Course");
                var salesModeTypeString =
                    await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilDefaultSalesModelTypeId);

                //اگر در کانفیگ DefaultSalesModeType
                //مقدار داشته باشیم و کاربر نیز مقداری را وارد نکرده باشد بنابراین مقدار 
                //salesModeType
                //را از کانفیگ خوانده و در غیر این صورت از مقدار وارد شده توسط کاربر میخوانیم
                if (!string.IsNullOrEmpty(salesModeTypeString) && salesModelType == 0 &&
                    int.TryParse(salesModeTypeString, out var salesModeTypeInt))
                {
                    entity.SalesModelType = salesModeTypeInt;

                    //اگر در کانفیگ مقداری ست نشده باشد و سمت کاربر هم مقداری ست نشده باشد 
                    //Exception throw
                    //می شود
                }
                else if (salesModelType == 0)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(Product),
                        "در هنگام ست کردن مقدار salesModeType خطای رخ داده است . یا کاربر مقدار را ست نکرده است یا مقدار پیش فرض کانفیگ صحیح نمی بشاد",
                        null);
                    throw new Exception("Error occurred in setting sales mode type");
                }

                entity.AppEntityId = courseAppEntityId;
                entity.Course.CreateBy = _userPrincipal.Id;
                entity.Course.CreationDate = DateTime.Now;
                entity.Course.ApplicationId = _userPrincipal.ApplicationId;
                entity.Course.CreatorOrganizationId = _userPrincipal.OrganizationId;
            }
            else
            {
                entity.Course = new Course
                {
                    CreateBy = _userPrincipal.Id,
                    CreationDate = DateTime.Now,
                    ApplicationId = _userPrincipal.ApplicationId,
                    CreatorOrganizationId = _userPrincipal.OrganizationId
                };
                if (!appEntityId.HasValue)
                {
                    var productAppEntityId = await _appEntityService.GetIdByTitleAsync("Product");
                    entity.AppEntityId = productAppEntityId;
                }
            }
        }

        /// <summary>
        /// آیا محصولی برای مرکز تعریف شده است یا خیر ؟؟ 
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public Task<bool> IsExistProductForCompanyAsync(int companyId)
        {
            return _productRepository.GetByCompanyId(companyId)
                .AnyAsync();
        }




        public override async Task DeleteAsync(object id, bool saveAfterDelete = false, bool publishEvent = true,
            bool logical = false)
        {
            await DeleteProductCategoryAsync((int)id);
            await base.DeleteAsync(id, saveAfterDelete, publishEvent, logical);
        }

        /// <summary>
        /// وابستگی های محصول را حذف می کند
        /// با توجه به بیزینس فقط اگر برای محصول دسته بندی تعریف شده باشد
        /// و دیگر وابستگی برای محصول نباشد
        /// دسته بندی محصول را حذف می کند
        /// تا بتواند محصول را حذف کند
        /// </summary>
        /// <param name="productId">محصول</param>
        /// <returns></returns>
        private async Task DeleteProductCategoryAsync(int productId)
        {
            // تمظیم فروش  فعال مرکز را بر می گرداند 
            var productCategories = _productCategoryService.GetProductCategories(productId);
            foreach (var productCategory in productCategories)
            {
                await _productCategoryService.DeleteAsync(productCategory);
            }
        }

        public async Task<(bool isAllowed, string message)> IsAllowedDeleteProductAsync(int productId)
        {
            // آیا برای محصول برنامه ریزی انجام شده است یا خیر 
            var isExistProduct =
                await _productSalesPlanningService.CheckProductHasAnyProductSalesPlanningAsync(productId);
            if (isExistProduct)
            {
                return (false, "کاربر گرامی، برای این دوره برنامه ریزی انجام شده، امکان حذف دوره وجود ندارد.");
            }

            return (true, string.Empty);
        }

        public Task<bool> ExistProductByCategoryIdAsync(int categoryId)
        {
            return _productRepository.ExistProductByCategoryIdAsync(categoryId);
        }

        /// <summary>
        /// دریافت شناسه
        /// salesModeType
        /// مربوط به یک Product
        /// </summary>
        /// <param name="productId">شناسه مرکز</param>
        /// <returns></returns>
        public Task<int> GetSalesModeTypeIdAsync(int productId)
        {
            return FindById(productId, true).Select(p => p.SalesModelType).FirstOrDefaultAsync();
        }

        /// <summary>
        /// این متد مقدار حذف برنامه ریزی دوره را برابر با صحیح می کند. رکورد در دریتابیس موجود است
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Archive(int id)
        {
            return _productRepository.ArchiveAsync(id);
        }

        /// <summary>
        /// می کند false این متد مقدار حذف برنامه ریزی دوره را برابر با 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> RestoreFromArchiveAsync(int id)
        {
            return _productRepository.RestoreFromArchiveAsync(id);
        }


        public Task UpdateProductSalesModeTypeAsync(int productId, SalesModelTypeEnum newSalesModelType)
        {
            return GetAll(false)
                .Where(x => x.Id == productId)
                .ExecuteUpdateAsync(x
                    => x.SetProperty(c => c.SalesModelType, (int)newSalesModelType));

        }


        public IQueryable<ProductMainCategoryView> GetProductMainCategoryView() =>
            _productRepository.GetProductMainCategoryView();

        public async Task<int> GetProductCountBasedOnCurrentUserCompanyAndRepresentationAsync()
        {
            var additionalUserProp = await _additionalUserPropService
                .GetByCustomFilter(_userPrincipal.Id, _userPrincipal.ApplicationId)
                .Select(aup => new { aup.RepresentationId, aup.CompanyId })
                .FirstOrDefaultAsync();

            var query = GetAll(true);

            if (additionalUserProp?.CompanyId is > 0)
                query = query.Where(x => x.CompanyId == additionalUserProp.CompanyId);


            return await query.CountAsync();
        }



        public Task<List<string>> GetCompanyProductListForFillInUploadedProductItemXlsValidationAsync(int companyId)
        {
            return _productRepository.GetAll(true)
                .Where(p => p.CompanyId == companyId && p.VisibleIndividually)
                .Select(s => s.Title + "/" + s.Code + "#" + s.Id).ToListAsync();
        }

    }
}