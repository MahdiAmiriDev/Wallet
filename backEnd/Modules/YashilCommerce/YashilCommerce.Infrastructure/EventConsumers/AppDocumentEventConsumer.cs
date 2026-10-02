using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Caching;
using Yashil.Common.Infrastructure.AppEvents;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.EventConsumers
{
    public class AppDocumentEventConsumer :
        IConsumer<EntityInsertedEvent<AppDocument, int>>,
        IConsumer<EntityDeletedEvent<AppDocument, int>>
    {


        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICategoryService _categoryService;
        private readonly ICompanyService _companyService;
        private readonly ICategoryCustomerClubService _categoryCustomerClubService;

        public AppDocumentEventConsumer(IProductSalesPlanningService productSalesPlanningService,
            IStaticCacheManager staticCacheManager, ICategoryService categoryService, ICompanyService companyService,
            ICategoryCustomerClubService categoryCustomerClubService)
        {
            _productSalesPlanningService = productSalesPlanningService;
            _staticCacheManager = staticCacheManager;
            _categoryService = categoryService;
            _companyService = companyService;
            _categoryCustomerClubService = categoryCustomerClubService;
        }

        public async Task HandleEventAsync(EntityInsertedEvent<AppDocument, int> eventMessage)
        {
            await UpdateCategoryAndClearCacheAsync(eventMessage.Entity);
            await UpdateProductSalesPlanningConfirmationAndClearCacheAsync(eventMessage.Entity);
            await UpdateCompanyAndClearCacheAsync(eventMessage.Entity);
            await UpdateCategoryCustomerClubAndClearCacheAsync(eventMessage.Entity);
            await ClearYashilCmsAdvertisingImageCacheAsync(eventMessage.Entity.ObjectId);
        }



        public void HandleEvent(EntityInsertedEvent<AppDocument, int> eventMessage)
        {
            UpdateCategoryAndClearCacheAsync(eventMessage.Entity).Wait();
            UpdateProductSalesPlanningConfirmationAndClearCacheAsync(eventMessage.Entity).Wait();
            UpdateCompanyAndClearCacheAsync(eventMessage.Entity).Wait();
            UpdateCategoryCustomerClubAndClearCacheAsync(eventMessage.Entity).Wait();
            ClearYashilCmsAdvertisingImageCacheAsync(eventMessage.Entity.ObjectId).Wait();
        }

        private async Task UpdateProductSalesPlanningConfirmationAndClearCacheAsync(AppDocument appDocument)
        {

            List<int> acceptedDocumentTypeIdsForPsp = new List<int>()
            {
                (int)DocumentTypeEnum.ProductSalesPlanningImage,
                (int)DocumentTypeEnum.ProductSalesPlanningPdfAttachments,
                (int)DocumentTypeEnum.CourseGuide,
                (int)DocumentTypeEnum.ProductSalesPlanningVideo,
                (int)DocumentTypeEnum.ProductSalesPlanningPdfAttachmentFile,
                (int)DocumentTypeEnum.ProductSalesPlanningZipFile
            };

            if (acceptedDocumentTypeIdsForPsp.Any(acceptedDocTypeId => appDocument.DocTypeId == acceptedDocTypeId))
            {
                await _productSalesPlanningService.ChangeProductSalesPlanningConfirmationIfNeededAsync(
                    (int)appDocument.ObjectId, true);

                await _staticCacheManager.RemoveByPrefixAsync(YashilModelCacheDefaults<ProductSalesPlanning>
                    .ProductSalesPlanningByFilterPrefixCacheKey);
            }
        }

        private async Task UpdateCompanyAndClearCacheAsync(AppDocument appDocument)
        {
            if (appDocument.DocTypeId == (int)DocumentTypeEnum.CompanyIcon)
            {
                var company = await _companyService.GetAsync(appDocument.ObjectId, false);
                company.ModificationDate = DateTime.Now;
                await _companyService.UpdateAsync(company, company.Id, true);
                await _staticCacheManager.RemoveByPrefixAsync(YashilModelCacheDefaults<Category>.Prefix);
            }
        }

        private async Task UpdateCategoryAndClearCacheAsync(AppDocument appDocument)
        {
            if (appDocument.DocTypeId == (int)DocumentTypeEnum.CategoryIcon)
            {

                var category = await _categoryService.GetAsync((int)appDocument.ObjectId, false);
                category.ModificationDate = DateTime.Now;
                await _categoryService.UpdateAsync(category, category.Id, true);
                await _staticCacheManager.RemoveByPrefixAsync(YashilModelCacheDefaults<Category>
                    .FirstLevelCategoriesPrefixCacheKey);
            }
        }

        private async Task UpdateCategoryCustomerClubAndClearCacheAsync(AppDocument appDocument)
        {
            if (appDocument.DocTypeId == (int)DocumentTypeEnum.CategorySlider)
            {
                var categoryCustomerClub =
                    await _categoryCustomerClubService.GetAsync((int)appDocument.ObjectId, false);
                categoryCustomerClub.ModificationDate = DateTime.Now;
                await _categoryCustomerClubService.UpdateAsync(categoryCustomerClub, categoryCustomerClub.Id, true);
                await _staticCacheManager.RemoveByPrefixAsync(YashilModelCacheDefaults<YashilCms>
                    .MainPageSliderLevelTwo.ToString());
            }
        }

        /// <summary>
        /// این متد جهت پاک کردن کش مربوط به تصاویر تبلیغاتی سایت که
        /// در صفحه ی اصلی سامانه  نمایش داده می شوند کاربرد دارد
        /// </summary>
        /// <returns></returns>
        Task ClearYashilCmsAdvertisingImageCacheAsync(long objectId)
        {

            return Task.WhenAll(
                _staticCacheManager.RemoveAsync(YashilModelCacheDefaults<YashilCms>.HomePageCommercialImageInfoKey),
                _staticCacheManager.RemoveAsync(YashilModelCacheDefaults<YashilCms>.HomePageLearningSkillImageInfoKey),
                _staticCacheManager.RemoveAsync(YashilModelCacheDefaults<YashilCms>
                    .HomePageTopRightAdvertisingImageInfoKey),
                _staticCacheManager.RemoveAsync(
                    YashilModelCacheDefaults<YashilCms>.HomePageTopRightSliderAdvertisingImageKey, objectId));
        }

        public async Task HandleEventAsync(EntityDeletedEvent<AppDocument, int> eventMessage)
        {
            await UpdateCategoryAndClearCacheAsync(eventMessage.Entity);
            await UpdateProductSalesPlanningConfirmationAndClearCacheAsync(eventMessage.Entity);
            await UpdateCompanyAndClearCacheAsync(eventMessage.Entity);
            await UpdateCategoryCustomerClubAndClearCacheAsync(eventMessage.Entity);
            await ClearYashilCmsAdvertisingImageCacheAsync(eventMessage.Entity.ObjectId);
        }

        public void HandleEvent(EntityDeletedEvent<AppDocument, int> eventMessage)
        {

        }

    }
}
