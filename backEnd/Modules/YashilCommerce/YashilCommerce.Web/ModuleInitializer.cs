using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Yashil.Common.SharedKernel.Module;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.RepositoryImpl;
using YashilCommerce.Infrastructure.ServiceImpl;
using Microsoft.Extensions.Configuration;
using Yashil.Common.Core.Interfaces;
using YashilCommerce.Infrastructure.Implementations;
using YashilCommerce.Web.DataFactory;
using System.Collections.Generic;
using Yashil.Common.Core.Classes;
using YashilCommerce.Core.Interfaces;

namespace YashilCommerce.Web
{
    public partial class ModuleInitializer : IModuleInitializer
    {
        private IConfiguration _configuration;

        public void OnStartup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, ITypeFinder typeFinder)
        {
            services.AddSingleton<PaymentTransactionUtil>();
            services.AddScoped<IHekmatApprovedTransactionService, HekmatApprovedTransactionService>();
            services.AddScoped<IHekmatApprovedTransactionRepository, HekmatApprovedTransactionRepository>();
            services.AddScoped<IPublicDataFactory, PublicDataFactory>();
            services.AddScoped<IRepresentationRepository, RepresentationRepository>();
            services.AddScoped<IRepresentationService, RepresentationService>();
            services.AddScoped<IRepresentationPersonRepository, RepresentationPersonRepository>();
            services.AddScoped<IRepresentationPersonService, RepresentationPersonService>();
            services.AddScoped<IAdditionalUserPropRepository, AdditionalUserPropRepository>();
            services.AddScoped<IAdditionalUserPropService, AdditionalUserPropService>();
            services.AddScoped<IRepresentationEstablishedLicenseTypeRepository, RepresentationEstablishedLicenseTypeRepository>();
            services.AddScoped<IRepresentationEstablishedLicenseTypeService, RepresentationEstablishedLicenseTypeService>();
            services.AddScoped<IPersonBankAccountRepository, PersonBankAccountRepository>();
            services.AddScoped<IPersonBankAccountService, PersonBankAccountService>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IProductTemplateRepository, ProductTemplateRepository>();
            services.AddScoped<IProductTemplateService, ProductTemplateService>();
            services.AddScoped<IGiftCardTypeRepository, GiftCardTypeRepository>();
            services.AddScoped<IGiftCardTypeService, GiftCardTypeService>();
            services.AddScoped<IRepresentationCategoryRepository, RepresentationCategoryRepository>();
            services.AddScoped<IRepresentationCategoryService, RepresentationCategoryService>();
            services.AddScoped<ITaxCategoryRepository, TaxCategoryRepository>();
            services.AddScoped<ITaxCategoryService, TaxCategoryService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerClubCardRepository, CustomerClubCardRepository>();
            services.AddScoped<ICustomerClubCardService, CustomerClubCardService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IWalletReservationRepository, WalletReservationRepository>();
            services.AddScoped<IWalletReservationService, WalletReservationService>();
            services.AddScoped<IProductSalesPlanningRepository, ProductSalesPlanningRepository>();
            services.AddScoped<IProductSalesPlanningService, ProductSalesPlanningService>();
            services.AddScoped<ICompanyMainCategoryRepository, CompanyMainCategoryRepository>();
            services.AddScoped<ICompanyMainCategoryService, CompanyMainCategoryService>();
            services.AddScoped<IDependentProductSaleRepository, DependentProductSaleRepository>();
            services.AddScoped<IDependentProductSaleService, DependentProductSaleService>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IContractPaymentMethodRepository, ContractPaymentMethodRepository>();
            services.AddScoped<IContractPaymentMethodService, ContractPaymentMethodService>();
            services.AddScoped<ISaleConfigurationRepository, SaleConfigurationRepository>();
            services.AddScoped<ISaleConfigurationService, SaleConfigurationService>();
            services.AddScoped<ISpecificationRepository, SpecificationRepository>();
            services.AddScoped<ISpecificationService, SpecificationService>();
            services.AddScoped<IProductSpecificationOptionRepository, ProductSpecificationOptionRepository>();
            services.AddScoped<IProductSpecificationOptionService, ProductSpecificationOptionService>();
            services.AddScoped<ISpecificationOptionRepository, SpecificationOptionRepository>();
            services.AddScoped<ISpecificationOptionService, SpecificationOptionService>();
            services.AddScoped<ICategorySpecificationRepository, CategorySpecificationRepository>();
            services.AddScoped<ICategorySpecificationService, CategorySpecificationService>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IProductTypeService, ProductTypeService>();
            services.AddScoped<IProductTypeSpecificationRepository, ProductTypeSpecificationRepository>();
            services.AddScoped<IProductTypeSpecificationService, ProductTypeSpecificationService>();
            services.AddScoped<ISubsidyRepository, SubsidyRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IOrderNoteRepository, OrderNoteRepository>();
            services.AddScoped<IOrderNoteService, OrderNoteService>();
            services.AddScoped<IAffiliateRepository, AffiliateRepository>();
            services.AddScoped<IAffiliateService, AffiliateService>();
            services.AddScoped<IRewardPointsHistoryRepository, RewardPointsHistoryRepository>();
            services.AddScoped<IRewardPointsHistoryService, RewardPointsHistoryService>();
            services.AddScoped<IShoppingCartItemRepository, ShoppingCartItemRepository>();
            services.AddScoped<IShoppingCartItemService, ShoppingCartItemService>();
            services.AddScoped<IRecipientRepository, RecipientRepository>();
            services.AddScoped<IRecipientService, RecipientService>();
            services.AddScoped<ICustomerClubRepository, CustomerClubRepository>();
            services.AddScoped<ICustomerClubService, CustomerClubService>();

            services.AddScoped<ICustomerClubPaymentMethodTypeRepository, CustomerClubPaymentMethodTypeRepository>();
            services.AddScoped<ICustomerClubPaymentMethodTypeService, CustomerClubPaymentMethodTypeService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IOrderTrakkingRepository, OrderTrakkingRepository>();
            services.AddScoped<IOrderTrakkingService, OrderTrakkingService>();
            services.AddScoped<IApplicationPaymentMethodRepository, ApplicationPaymentMethodRepository>();
            services.AddScoped<IApplicationPaymentMethodService, ApplicationPaymentMethodService>();
            services.AddScoped<IFactorItemRepository, FactorItemRepository>();
            services.AddScoped<IFactorItemService, FactorItemService>();
            services.AddScoped<IFactorRepository, FactorRepository>();
            services.AddScoped<IFactorService, FactorService>();
            services.AddScoped<IFactorPaymentRepository, FactorPaymentRepository>();
            services.AddScoped<IFactorPaymentService, FactorPaymentService>();
            services.AddScoped<IContentCategoryRepository, ContentCategoryRepository>();
            services.AddScoped<IContentCategoryService, ContentCategoryService>();
            services.AddScoped<IFactorVerificationRepository, FactorVerificationRepository>();
            services.AddScoped<IFactorVerificationService, FactorVerificationService>();
            services.AddScoped<IFactorVerificationConfigurationRepository, FactorVerificationConfigurationRepository>();
            services.AddScoped<IFactorVerificationConfigurationService, FactorVerificationConfigurationService>();
            services.AddScoped<IFactorVerificationConfigurationRoleRepository, FactorVerificationConfigurationRoleRepository>();
            services.AddScoped<IFactorVerificationConfigurationRoleService, FactorVerificationConfigurationRoleService>();
            services.AddScoped<IFactorGroupRepository, FactorGroupRepository>();
            services.AddScoped<IFactorGroupService, FactorGroupService>();

            services.AddScoped<IValidHekmatCardRepository, ValidHekmatCardRepository>();
            services.AddScoped<IValidHekmatCardService, ValidHekmatCardService>();
            services.AddScoped<IIssueRepository, IssueRepository>();
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IIssueParticipantRepository, IssueParticipantRepository>();
            services.AddScoped<IIssueParticipantService, IssueParticipantService>();
            services.AddScoped<IIssueTypeItemRepository, IssueTypeItemRepository>();
            services.AddScoped<IIssueTypeItemService, IssueTypeItemService>();
            services.AddScoped<IIssueLogWorkRepository, IssueLogWorkRepository>();
            services.AddScoped<IIssueLogWorkService, IssueLogWorkService>();
            services.AddScoped<IIssueCommentRepository, IssueCommentRepository>();
            services.AddScoped<IIssueCommentService, IssueCommentService>();
            services.AddScoped<IOrderDataFactory, OrderDataFactory>();
            services.AddScoped<ISubsidyService, SubsidyService>();
            services.AddScoped<IYashilSchedulerRepository, YashilSchedulerRepository>();
            services.AddScoped<IYashilSchedulerService, YashilSchedulerService>();
            services.AddScoped<IVocherTypeRepository, VocherTypeRepository>();
            services.AddScoped<IVocherTypeService, VocherTypeService>();
            services.AddScoped<IVocherRepository, VocherRepository>();
            services.AddScoped<IVocherService, VocherService>();
            ConfigureCustomServices(services, configuration, typeFinder);
            services.AddScoped<IModifiedOrderItemRepository, ModifiedOrderItemRepository>();
            services.AddScoped<IModifiedOrderItemService, ModifiedOrderItemService>();
            services.AddScoped<IRedirectUrlService, RedirectUrlService>();
            services.AddScoped<IItemGroupRepository, ItemGroupRepository>();
            services.AddScoped<IItemGroupService, ItemGroupService>();
            services.AddScoped<IRefundDetailRepository, RefundDetailRepository>();
            services.AddScoped<IRefundDetailService, RefundDetailService>();
            services.AddScoped<IRefundRepository, RefundRepository>();
            services.AddScoped<IRefundService, RefundService>();

            services.AddScoped<ICustomerClubPaymentInstallmentRepository, CustomerClubPaymentInstallmentRepository>();
            services.AddScoped<ICustomerClubPaymentInstallmentService, CustomerClubPaymentInstallmentService>();
            services.AddScoped<IUploadedProductItemRepository, UploadedProductItemRepository>();
            services.AddScoped<IUploadedProductItemService, UploadedProductItemService>();
            services.AddScoped<IFactorDeductionRepository, FactorDeductionRepository>();
            services.AddScoped<IFactorDeductionService, FactorDeductionService>();

            services.AddScoped<IHekmatCanceledTransactionRepository, HekmatCanceledTransactionRepository>();
            services.AddScoped<IHekmatCanceledTransactionService, HekmatCanceledTransactionService>();
            services.AddScoped<IHekmatEffectlessCanceledTransactionRepository, HekmatEffectlessCanceledTransactionRepository>();
            services.AddScoped<IHekmatEffectlessCanceledTransactionService, HekmatEffectlessCanceledTransactionService>();
            services.AddScoped<IHekmatEffectlessModifiedTransactionRepository, HekmatEffectlessModifiedTransactionRepository>();
            services.AddScoped<IHekmatEffectlessModifiedTransactionService, HekmatEffectlessModifiedTransactionService>();
            services.AddScoped<IHekmatModifiedTransactionRepository, HekmatModifiedTransactionRepository>();
            services.AddScoped<IHekmatModifiedTransactionService, HekmatModifiedTransactionService>();
            services.AddScoped<IUploadedProductItemService, UploadedProductItemService>();
            services.AddScoped<IUploadedFactorPaymentRepository, UploadedFactorPaymentRepository>();
            services.AddScoped<IUploadedFactorPaymentService, UploadedFactorPaymentService>();
            services.AddScoped<IUploadedVoucherRepository, UploadedVoucherRepository>();
            services.AddScoped<IUploadedVoucherService, UploadedVoucherService>();
            services.AddScoped<IUploadedVoucherRepository, UploadedVoucherRepository>();
            services.AddScoped<IUploadedVoucherService, UploadedVoucherService>();
            services.AddScoped<IHekmatTransactionRepository, HekmatTransactionRepository>();
            services.AddScoped<IHekmatTransactionService, HekmatTransactionService>();
            services.AddScoped<ICitySubsidyFactorRepository, CitySubsidyFactorRepository>();
            services.AddScoped<ICitySubsidyFactorService, CitySubsidyFactorService>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<ISaleConfigurationAttributeRepository, SaleConfigurationAttributeRepository>();
            services.AddScoped<ISaleConfigurationAttributeService, SaleConfigurationAttributeService>();
            services.AddScoped<ICategoryGroupRepository, CategoryGroupRepository>();
            services.AddScoped<ICategoryGroupService, CategoryGroupService>();
            services.AddScoped<ISubsidyGroupRepository, SubsidyGroupRepository>();
            services.AddScoped<ISubsidyGroupService, SubsidyGroupService>();
            services.AddScoped<IPreRegistrationRepository, PreRegistrationRepository>();
            services.AddScoped<IPreRegistrationService, PreRegistrationService>();
            services.AddScoped<ICategoryCustomerClubRepository, CategoryCustomerClubRepository>();
            services.AddScoped<ICategoryCustomerClubService, CategoryCustomerClubService>();
            services.AddScoped<IProductSalesPlanningDiscountRepository, ProductSalesPlanningDiscountRepository>();
            services.AddScoped<IProductSalesPlanningDiscountService, ProductSalesPlanningDiscountService>();
            services.AddScoped<IAppEntityCustomerClubRepository, AppEntityCustomerClubRepository>();
            services.AddScoped<IAppEntityCustomerClubService, AppEntityCustomerClubService>();
            services.AddScoped<ICompanyGroupRepository, CompanyGroupRepository>();
            services.AddScoped<ICompanyGroupService, CompanyGroupService>();
            services.AddScoped<IHumaxService, HumaxService>();
            services.AddScoped<ISiteVisitService,SiteVisitService>();
            services.AddScoped<ISiteVisitRepository,SiteVisitRepository>();
            services.AddScoped<IProductTagsService, ProductTagsService>();
            services.AddScoped<IProductTagsRepository, ProductTagsRepository>(); 
            services.AddScoped<ITaxCategorySaleConfigurationRepository, TaxCategorySaleConfigurationRepository>();
            services.AddScoped<ITaxCategorySaleConfigurationService, TaxCategorySaleConfigurationService>();
            services.AddScoped<IProductSalesPlanningItemGroupRepository, ProductSalesPlanningItemGroupRepository>();
            services.AddScoped<IProductSalesPlanningItemGroupService, ProductSalesPlanningItemGroupService>();
            services.AddScoped<IMaktabKhoneApiService, MaktabKhoneApiServiceService>();
            services.AddScoped<IUploadedCertificateRepository, UploadedCertificateRepository>();
            services.AddScoped<IUploadedCertificateService, UploadedCertificateService>();

            services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
            services.AddScoped<IProductGroupService, ProductGroupService>();
            services.AddScoped<IAdditionalUserPropCompanyGroupRepository, AdditionalUserPropCompanyGroupRepository>();
            services.AddScoped<IAdditionalUserPropCompanyGroupService, AdditionalUserPropCompanyGroupService>();


            services.AddScoped<IFactorPaymentInstallmentRepository, FactorPaymentInstallmentRepository>();
            services.AddScoped<IFactorPaymentInstallmentService, FactorPaymentInstallmentService>();
            services.AddScoped<IUploadedFactorInstallmentPaymentService, UploadedFactorInstallmentPaymentService>();
            services.AddScoped<IUploadedFactorInstallmentPaymentRepository, UploadedFactorInstallmentPaymentRepository>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        partial void ConfigureCustomServices(IServiceCollection services, IConfiguration configuration, ITypeFinder typeFinder);

        public List<YashilCustomResource> GetCustomResources()
        {
            return new List<YashilCustomResource>
            {
                new() { Title = "CoursePlanning", Description = "برنامه ريزي دوره" },
                new() { Title = "modifiedOrderItemAction", Description = "اعمال تغییر یا لغو سفارش" },
                new() { Title = "sataOrderItems", Description = "سفارشات با یارانه ساتا" }
            };
        }

        public YashilModuleInformation GetModuleInformation()
        {
            return new YashilModuleInformation() { DatabaseSchemaName = "shp" };
        }
    }
}
