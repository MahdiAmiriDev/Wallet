using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class User  :IBaseEntity<int>,IApplicationBasedEntity
    {
public User()
{
    AccessLevelCreateByNavigation = new HashSet<AccessLevel>();
    AccessLevelModifyByNavigation = new HashSet<AccessLevel>();
    ActiveStorageAttachmentCreateByNavigation = new HashSet<ActiveStorageAttachment>();
    ActiveStorageAttachmentModifyByNavigation = new HashSet<ActiveStorageAttachment>();
    ActiveStorageBlobsCreateByNavigation = new HashSet<ActiveStorageBlobs>();
    ActiveStorageBlobsModifyByNavigation = new HashSet<ActiveStorageBlobs>();
    ActivityCreateByNavigation = new HashSet<Activity>();
    ActivityLogCreateByNavigation = new HashSet<ActivityLog>();
    ActivityLogModifyByNavigation = new HashSet<ActivityLog>();
    ActivityLogUser = new HashSet<ActivityLog>();
    ActivityModifyByNavigation = new HashSet<Activity>();
    AdditionalUserPropCompanyGroupCreateByNavigation = new HashSet<AdditionalUserPropCompanyGroup>();
    AdditionalUserPropCompanyGroupModifyByNavigation = new HashSet<AdditionalUserPropCompanyGroup>();
    AdditionalUserPropCreateByNavigation = new HashSet<AdditionalUserProp>();
    AdditionalUserPropModifyByNavigation = new HashSet<AdditionalUserProp>();
    AddressCreateByNavigation = new HashSet<Address>();
    AddressModifyByNavigation = new HashSet<Address>();
    AffiliateCreateByNavigation = new HashSet<Affiliate>();
    AffiliateModifyByNavigation = new HashSet<Affiliate>();
    AppActionCreateByNavigation = new HashSet<AppAction>();
    AppActionModifyByNavigation = new HashSet<AppAction>();
    AppConfigCreateByNavigation = new HashSet<AppConfig>();
    AppConfigModifyByNavigation = new HashSet<AppConfig>();
    AppDocumentCreateByNavigation = new HashSet<AppDocument>();
    AppDocumentModifyByNavigation = new HashSet<AppDocument>();
    AppEntityAttributeCreateByNavigation = new HashSet<AppEntityAttribute>();
    AppEntityAttributeMappingCreateByNavigation = new HashSet<AppEntityAttributeMapping>();
    AppEntityAttributeMappingModifyByNavigation = new HashSet<AppEntityAttributeMapping>();
    AppEntityAttributeModifyByNavigation = new HashSet<AppEntityAttribute>();
    AppEntityAttributeValueCreateByNavigation = new HashSet<AppEntityAttributeValue>();
    AppEntityAttributeValueModifyByNavigation = new HashSet<AppEntityAttributeValue>();
    AppEntityCustomerClubCreateByNavigation = new HashSet<AppEntityCustomerClub>();
    AppEntityCustomerClubModifyByNavigation = new HashSet<AppEntityCustomerClub>();
    ApplicationCreateByNavigation = new HashSet<Application>();
    ApplicationCustomerClubCreateByNavigation = new HashSet<ApplicationCustomerClub>();
    ApplicationCustomerClubModifyByNavigation = new HashSet<ApplicationCustomerClub>();
    ApplicationModifyByNavigation = new HashSet<Application>();
    ApplicationPaymentMethodCreateByNavigation = new HashSet<ApplicationPaymentMethod>();
    ApplicationPaymentMethodModifyByNavigation = new HashSet<ApplicationPaymentMethod>();
    ArInternalMetadataCreateByNavigation = new HashSet<ArInternalMetadata>();
    ArInternalMetadataModifyByNavigation = new HashSet<ArInternalMetadata>();
    CardAmountHistoryCreateByNavigation = new HashSet<CardAmountHistory>();
    CardAmountHistoryModifyByNavigation = new HashSet<CardAmountHistory>();
    CardInfoCreateByNavigation = new HashSet<CardInfo>();
    CardInfoModifyByNavigation = new HashSet<CardInfo>();
    CategoryCreateByNavigation = new HashSet<Category>();
    CategoryCustomerClubCreateByNavigation = new HashSet<CategoryCustomerClub>();
    CategoryCustomerClubModifyByNavigation = new HashSet<CategoryCustomerClub>();
    CategoryGroupCreateByNavigation = new HashSet<CategoryGroup>();
    CategoryGroupModifyByNavigation = new HashSet<CategoryGroup>();
    CategoryModifyByNavigation = new HashSet<Category>();
    CategorySpecificationCreateByNavigation = new HashSet<CategorySpecification>();
    CategorySpecificationModifyByNavigation = new HashSet<CategorySpecification>();
    CertificateCreateByNavigation = new HashSet<Certificate>();
    CertificateModifyByNavigation = new HashSet<Certificate>();
    CityCreateByNavigation = new HashSet<City>();
    CityModifyByNavigation = new HashSet<City>();
    CitySubsidyFactorCreateByNavigation = new HashSet<CitySubsidyFactor>();
    CitySubsidyFactorModifyByNavigation = new HashSet<CitySubsidyFactor>();
    ClassRoomCreateByNavigation = new HashSet<ClassRoom>();
    ClassRoomModifyByNavigation = new HashSet<ClassRoom>();
    ClassRoomReservationCreateByNavigation = new HashSet<ClassRoomReservation>();
    ClassRoomReservationModifyByNavigation = new HashSet<ClassRoomReservation>();
    ClassRoomReservationScheduleCreateByNavigation = new HashSet<ClassRoomReservationSchedule>();
    ClassRoomReservationScheduleModifyByNavigation = new HashSet<ClassRoomReservationSchedule>();
    ClassSessionCreateByNavigation = new HashSet<ClassSession>();
    ClassSessionModifyByNavigation = new HashSet<ClassSession>();
    CommonBaseDataCreateByNavigation = new HashSet<CommonBaseData>();
    CommonBaseDataModifyByNavigation = new HashSet<CommonBaseData>();
    CommonBaseTypeCreateByNavigation = new HashSet<CommonBaseType>();
    CommonBaseTypeModifyByNavigation = new HashSet<CommonBaseType>();
    CompanyCreateByNavigation = new HashSet<Company>();
    CompanyGroupCreateByNavigation = new HashSet<CompanyGroup>();
    CompanyGroupModifyByNavigation = new HashSet<CompanyGroup>();
    CompanyMainCategoryCreateByNavigation = new HashSet<CompanyMainCategory>();
    CompanyMainCategoryModifyByNavigation = new HashSet<CompanyMainCategory>();
    CompanyModifyByNavigation = new HashSet<Company>();
    ContentCategoryCreateByNavigation = new HashSet<ContentCategory>();
    ContentCategoryModifyByNavigation = new HashSet<ContentCategory>();
    ContractCreateByNavigation = new HashSet<Contract>();
    ContractModifyByNavigation = new HashSet<Contract>();
    ContractPaymentMethodCreateByNavigation = new HashSet<ContractPaymentMethod>();
    ContractPaymentMethodModifyByNavigation = new HashSet<ContractPaymentMethod>();
    ConversationCreateByNavigation = new HashSet<Conversation>();
    ConversationModifyByNavigation = new HashSet<Conversation>();
    ConversationRecipientCreateByNavigation = new HashSet<ConversationRecipient>();
    ConversationRecipientModifyByNavigation = new HashSet<ConversationRecipient>();
    ConversationRecipientToUser = new HashSet<ConversationRecipient>();
    ConversationSeenCreateByNavigation = new HashSet<ConversationSeen>();
    ConversationSeenModifyByNavigation = new HashSet<ConversationSeen>();
    ConversationSenderNavigation = new HashSet<Conversation>();
    CorporationCreateByNavigation = new HashSet<Corporation>();
    CorporationJobCoCreateByNavigation = new HashSet<CorporationJobCo>();
    CorporationJobCoModifyByNavigation = new HashSet<CorporationJobCo>();
    CorporationModifyByNavigation = new HashSet<Corporation>();
    CourseCreateByNavigation = new HashSet<Course>();
    CourseModifyByNavigation = new HashSet<Course>();
    CreditOrCashPaymentCreateByNavigation = new HashSet<CreditOrCashPayment>();
    CreditOrCashPaymentModifyByNavigation = new HashSet<CreditOrCashPayment>();
    CurrencyCreateByNavigation = new HashSet<Currency>();
    CurrencyModifyByNavigation = new HashSet<Currency>();
    CustomerClubCardCreateByNavigation = new HashSet<CustomerClubCard>();
    CustomerClubCardModifyByNavigation = new HashSet<CustomerClubCard>();
    CustomerClubCardPersonCreateByNavigation = new HashSet<CustomerClubCardPerson>();
    CustomerClubCardPersonModifyByNavigation = new HashSet<CustomerClubCardPerson>();
    CustomerClubCreateByNavigation = new HashSet<CustomerClub>();
    CustomerClubModifyByNavigation = new HashSet<CustomerClub>();
    CustomerClubPaymentInstallmentCreateByNavigation = new HashSet<CustomerClubPaymentInstallment>();
    CustomerClubPaymentInstallmentModifyByNavigation = new HashSet<CustomerClubPaymentInstallment>();
    CustomerClubPaymentMethodTypeCreateByNavigation = new HashSet<CustomerClubPaymentMethodType>();
    CustomerClubPaymentMethodTypeModifyByNavigation = new HashSet<CustomerClubPaymentMethodType>();
    DashboardConnectionStringCreateByNavigation = new HashSet<DashboardConnectionString>();
    DashboardConnectionStringModifyByNavigation = new HashSet<DashboardConnectionString>();
    DashboardGroupDashboardCreateByNavigation = new HashSet<DashboardGroupDashboard>();
    DashboardGroupDashboardModifyByNavigation = new HashSet<DashboardGroupDashboard>();
    DashboardStoreCreateByNavigation = new HashSet<DashboardStore>();
    DashboardStoreModifyByNavigation = new HashSet<DashboardStore>();
    DependentProductSaleCreateByNavigation = new HashSet<DependentProductSale>();
    DependentProductSaleModifyByNavigation = new HashSet<DependentProductSale>();
    DocFormatCreateByNavigation = new HashSet<DocFormat>();
    DocFormatModifyByNavigation = new HashSet<DocFormat>();
    DocTypeCreateByNavigation = new HashSet<DocType>();
    DocTypeModifyByNavigation = new HashSet<DocType>();
    DocumentCategoryCreateByNavigation = new HashSet<DocumentCategory>();
    DocumentCategoryModifyByNavigation = new HashSet<DocumentCategory>();
    ErrorCreateByNavigation = new HashSet<Error>();
    ErrorModifyByNavigation = new HashSet<Error>();
    ExamCategoryCreateByNavigation = new HashSet<ExamCategory>();
    ExamCategoryModifyByNavigation = new HashSet<ExamCategory>();
    ExamCreateByNavigation = new HashSet<Exam>();
    ExamModifyByNavigation = new HashSet<Exam>();
    ExamQuestionAnswerCreateByNavigation = new HashSet<ExamQuestionAnswer>();
    ExamQuestionAnswerModifyByNavigation = new HashSet<ExamQuestionAnswer>();
    ExamQuestionCreateByNavigation = new HashSet<ExamQuestion>();
    ExamQuestionModifyByNavigation = new HashSet<ExamQuestion>();
    FactorConfirmByNavigation = new HashSet<Factor>();
    FactorCreateByNavigation = new HashSet<Factor>();
    FactorDeductionCreateByNavigation = new HashSet<FactorDeduction>();
    FactorDeductionModifyByNavigation = new HashSet<FactorDeduction>();
    FactorGroupCreateByNavigation = new HashSet<FactorGroup>();
    FactorGroupItemCreateByNavigation = new HashSet<FactorGroupItem>();
    FactorGroupItemModifyByNavigation = new HashSet<FactorGroupItem>();
    FactorGroupModifyByNavigation = new HashSet<FactorGroup>();
    FactorItemCreateByNavigation = new HashSet<FactorItem>();
    FactorItemModifyByNavigation = new HashSet<FactorItem>();
    FactorModifyByNavigation = new HashSet<Factor>();
    FactorPaymentCreateByNavigation = new HashSet<FactorPayment>();
    FactorPaymentInstallmentCreateByNavigation = new HashSet<FactorPaymentInstallment>();
    FactorPaymentInstallmentModifyByNavigation = new HashSet<FactorPaymentInstallment>();
    FactorPaymentModifyByNavigation = new HashSet<FactorPayment>();
    FactorVerificationConfigurationCreateByNavigation = new HashSet<FactorVerificationConfiguration>();
    FactorVerificationConfigurationModifyByNavigation = new HashSet<FactorVerificationConfiguration>();
    FactorVerificationConfigurationRoleCreateByNavigation = new HashSet<FactorVerificationConfigurationRole>();
    FactorVerificationConfigurationRoleModifyByNavigation = new HashSet<FactorVerificationConfigurationRole>();
    FactorVerificationCreateByNavigation = new HashSet<FactorVerification>();
    FactorVerificationModifyByNavigation = new HashSet<FactorVerification>();
    FactorVerificationVerifyByNavigation = new HashSet<FactorVerification>();
    FeatureCreateByNavigation = new HashSet<Feature>();
    FeatureModifyByNavigation = new HashSet<Feature>();
    FileTransferCreateByNavigation = new HashSet<FileTransfer>();
    FileTransferModifyByNavigation = new HashSet<FileTransfer>();
    GiftCardTypeCreateByNavigation = new HashSet<GiftCardType>();
    GiftCardTypeModifyByNavigation = new HashSet<GiftCardType>();
    HekmatBlackListPhoneCreateByNavigation = new HashSet<HekmatBlackListPhone>();
    HekmatBlackListPhoneModifyByNavigation = new HashSet<HekmatBlackListPhone>();
    HekmatPaymentGatewayModifiedTransactionCreateByNavigation = new HashSet<HekmatPaymentGatewayModifiedTransaction>();
    HekmatPaymentGatewayModifiedTransactionModifyByNavigation = new HashSet<HekmatPaymentGatewayModifiedTransaction>();
    HekmatPaymentGatewayTransactionBulkRequestCategoryCreateByNavigation = new HashSet<HekmatPaymentGatewayTransactionBulkRequestCategory>();
    HekmatPaymentGatewayTransactionBulkRequestCategoryModifyByNavigation = new HashSet<HekmatPaymentGatewayTransactionBulkRequestCategory>();
    HekmatPaymentGatewayTransactionBulkRequestCreateByNavigation = new HashSet<HekmatPaymentGatewayTransactionBulkRequest>();
    HekmatPaymentGatewayTransactionBulkRequestDetailCreateByNavigation = new HashSet<HekmatPaymentGatewayTransactionBulkRequestDetail>();
    HekmatPaymentGatewayTransactionBulkRequestDetailModifyByNavigation = new HashSet<HekmatPaymentGatewayTransactionBulkRequestDetail>();
    HekmatPaymentGatewayTransactionBulkRequestModifyByNavigation = new HashSet<HekmatPaymentGatewayTransactionBulkRequest>();
    HekmatPaymentGatewayTransactionCreateByNavigation = new HashSet<HekmatPaymentGatewayTransaction>();
    HekmatPaymentGatewayTransactionModifyByNavigation = new HashSet<HekmatPaymentGatewayTransaction>();
    HekmatTerminalCreateByNavigation = new HashSet<HekmatTerminal>();
    HekmatTerminalModifyByNavigation = new HashSet<HekmatTerminal>();
    HekmatTransactionCreateByNavigation = new HashSet<HekmatTransaction>();
    HekmatTransactionModifyByNavigation = new HashSet<HekmatTransaction>();
    HomeWorkCategoryCreateByNavigation = new HashSet<HomeWorkCategory>();
    HomeWorkCategoryModifyByNavigation = new HashSet<HomeWorkCategory>();
    HomeWorkCreateByNavigation = new HashSet<HomeWork>();
    HomeWorkModifyByNavigation = new HashSet<HomeWork>();
    HybridPaymentCreateByNavigation = new HashSet<HybridPayment>();
    HybridPaymentModifyByNavigation = new HashSet<HybridPayment>();
    HybridPaymentRequestCreateByNavigation = new HashSet<HybridPaymentRequest>();
    HybridPaymentRequestModifyByNavigation = new HashSet<HybridPaymentRequest>();
    InstallmentCreateByNavigation = new HashSet<Installment>();
    InstallmentModifyByNavigation = new HashSet<Installment>();
    InvitationCreateByNavigation = new HashSet<Invitation>();
    InvitationModifyByNavigation = new HashSet<Invitation>();
    IssueAssigneeNavigation = new HashSet<Issue>();
    IssueCommentCreateByNavigation = new HashSet<IssueComment>();
    IssueCommentModifyByNavigation = new HashSet<IssueComment>();
    IssueCreateByNavigation = new HashSet<Issue>();
    IssueIssueOwnerNavigation = new HashSet<Issue>();
    IssueLogWorkCreateByNavigation = new HashSet<IssueLogWork>();
    IssueLogWorkModifyByNavigation = new HashSet<IssueLogWork>();
    IssueModifyByNavigation = new HashSet<Issue>();
    IssueParticipantCreateByNavigation = new HashSet<IssueParticipant>();
    IssueParticipantModifyByNavigation = new HashSet<IssueParticipant>();
    IssueParticipantUser = new HashSet<IssueParticipant>();
    IssueTypeItemCreateByNavigation = new HashSet<IssueTypeItem>();
    IssueTypeItemModifyByNavigation = new HashSet<IssueTypeItem>();
    ItemGroupCreateByNavigation = new HashSet<ItemGroup>();
    ItemGroupModifyByNavigation = new HashSet<ItemGroup>();
    JobCoCategoryCreateByNavigation = new HashSet<JobCoCategory>();
    JobCoCategoryModifyByNavigation = new HashSet<JobCoCategory>();
    JobCoCreateByNavigation = new HashSet<JobCo>();
    JobCoModifyByNavigation = new HashSet<JobCo>();
    JobCoSkillCreateByNavigation = new HashSet<JobCoSkill>();
    JobCoSkillModifyByNavigation = new HashSet<JobCoSkill>();
    JobCoTagsCreateByNavigation = new HashSet<JobCoTags>();
    JobCoTagsModifyByNavigation = new HashSet<JobCoTags>();
    JobCoUserCreateByNavigation = new HashSet<JobCoUser>();
    JobCoUserJobApplicantUserNavigation = new HashSet<JobCoUser>();
    JobCoUserModifyByNavigation = new HashSet<JobCoUser>();
    JobCreateByNavigation = new HashSet<Job>();
    JobModifyByNavigation = new HashSet<Job>();
    KeywordCreateByNavigation = new HashSet<Keyword>();
    KeywordModifyByNavigation = new HashSet<Keyword>();
    LanguageCreateByNavigation = new HashSet<Language>();
    LanguageModifyByNavigation = new HashSet<Language>();
    LoanCreateByNavigation = new HashSet<Loan>();
    LoanGuarantorCreateByNavigation = new HashSet<LoanGuarantor>();
    LoanGuarantorModifyByNavigation = new HashSet<LoanGuarantor>();
    LoanModifyByNavigation = new HashSet<Loan>();
    LocaleStringResourceCreateByNavigation = new HashSet<LocaleStringResource>();
    LocaleStringResourceModifyByNavigation = new HashSet<LocaleStringResource>();
    MainNewsCreateByNavigation = new HashSet<MainNews>();
    MainNewsModifyByNavigation = new HashSet<MainNews>();
    MediaCategoryCreateByNavigation = new HashSet<MediaCategory>();
    MediaCategoryModifyByNavigation = new HashSet<MediaCategory>();
    MediaCommentCreateByNavigation = new HashSet<MediaComment>();
    MediaCommentModifyByNavigation = new HashSet<MediaComment>();
    MediaKeyMomentCreateByNavigation = new HashSet<MediaKeyMoment>();
    MediaKeyMomentModifyByNavigation = new HashSet<MediaKeyMoment>();
    MediaKeyMomentUser = new HashSet<MediaKeyMoment>();
    MediaManufacturerCreateByNavigation = new HashSet<MediaManufacturer>();
    MediaManufacturerModifyByNavigation = new HashSet<MediaManufacturer>();
    MediaSessionCreateByNavigation = new HashSet<MediaSession>();
    MediaSessionModifyByNavigation = new HashSet<MediaSession>();
    MediaSessionViewingHistoryCreateByNavigation = new HashSet<MediaSessionViewingHistory>();
    MediaSessionViewingHistoryModifyByNavigation = new HashSet<MediaSessionViewingHistory>();
    MenuCreateByNavigation = new HashSet<Menu>();
    MenuModifyByNavigation = new HashSet<Menu>();
    MerchantInfoCreateByNavigation = new HashSet<MerchantInfo>();
    MerchantInfoModifyByNavigation = new HashSet<MerchantInfo>();
    MerchantInfoUser = new HashSet<MerchantInfo>();
    MerchantTerminalInfoCreateByNavigation = new HashSet<MerchantTerminalInfo>();
    MerchantTerminalInfoModifyByNavigation = new HashSet<MerchantTerminalInfo>();
    ModifiedOrderItemAssigneeNavigation = new HashSet<ModifiedOrderItem>();
    ModifiedOrderItemCreateByNavigation = new HashSet<ModifiedOrderItem>();
    ModifiedOrderItemModifyByNavigation = new HashSet<ModifiedOrderItem>();
    NajaDemandCreateByNavigation = new HashSet<NajaDemand>();
    NajaDemandModifyByNavigation = new HashSet<NajaDemand>();
    NajaPaymentCreateByNavigation = new HashSet<NajaPayment>();
    NajaPaymentModifyByNavigation = new HashSet<NajaPayment>();
    NajaRefundCreateByNavigation = new HashSet<NajaRefund>();
    NajaRefundModifyByNavigation = new HashSet<NajaRefund>();
    NewsKeywordCreateByNavigation = new HashSet<NewsKeyword>();
    NewsKeywordModifyByNavigation = new HashSet<NewsKeyword>();
    NewsStoreCreateByNavigation = new HashSet<NewsStore>();
    NewsStoreModifyByNavigation = new HashSet<NewsStore>();
    OrderChangePermissionCreateByNavigation = new HashSet<OrderChangePermission>();
    OrderChangePermissionModifyByNavigation = new HashSet<OrderChangePermission>();
    OrderCreateByNavigation = new HashSet<Order>();
    OrderItemCreateByNavigation = new HashSet<OrderItem>();
    OrderItemDependentProductSaleCreateByNavigation = new HashSet<OrderItemDependentProductSale>();
    OrderItemDependentProductSaleModifyByNavigation = new HashSet<OrderItemDependentProductSale>();
    OrderItemDetailCreateByNavigation = new HashSet<OrderItemDetail>();
    OrderItemDetailModifyByNavigation = new HashSet<OrderItemDetail>();
    OrderItemModifyByNavigation = new HashSet<OrderItem>();
    OrderItemPaymentCreateByNavigation = new HashSet<OrderItemPayment>();
    OrderItemPaymentModifyByNavigation = new HashSet<OrderItemPayment>();
    OrderModifyByNavigation = new HashSet<Order>();
    OrderNoteCreateByNavigation = new HashSet<OrderNote>();
    OrderNoteModifyByNavigation = new HashSet<OrderNote>();
    OrderTrakkingCreateByNavigation = new HashSet<OrderTrakking>();
    OrderTrakkingModifyByNavigation = new HashSet<OrderTrakking>();
    OrderTrakkingReferenceToNavigation = new HashSet<OrderTrakking>();
    OrderUser = new HashSet<Order>();
    OrganizationCreateByNavigation = new HashSet<Organization>();
    OrganizationModifyByNavigation = new HashSet<Organization>();
    OrganizationVirtualClassRoomConfigCreateByNavigation = new HashSet<OrganizationVirtualClassRoomConfig>();
    OrganizationVirtualClassRoomConfigModifyByNavigation = new HashSet<OrganizationVirtualClassRoomConfig>();
    PasswordBlackListCreateByNavigation = new HashSet<PasswordBlackList>();
    PasswordBlackListModifyByNavigation = new HashSet<PasswordBlackList>();
    PasswordHistoryCreateByNavigation = new HashSet<PasswordHistory>();
    PasswordHistoryModifyByNavigation = new HashSet<PasswordHistory>();
    PaymentCreateByNavigation = new HashSet<Payment>();
    PaymentGatewayCreateByNavigation = new HashSet<PaymentGateway>();
    PaymentGatewayModifyByNavigation = new HashSet<PaymentGateway>();
    PaymentGatewayPaymentMethodCreateByNavigation = new HashSet<PaymentGatewayPaymentMethod>();
    PaymentGatewayPaymentMethodModifyByNavigation = new HashSet<PaymentGatewayPaymentMethod>();
    PaymentModifyByNavigation = new HashSet<Payment>();
    PaymentTransactionItemRefundCreateByNavigation = new HashSet<PaymentTransactionItemRefund>();
    PaymentTransactionItemRefundModifyByNavigation = new HashSet<PaymentTransactionItemRefund>();
    PersianCalendarCreateByNavigation = new HashSet<PersianCalendar>();
    PersianCalendarModifyByNavigation = new HashSet<PersianCalendar>();
    PersianDateMonthCreateByNavigation = new HashSet<PersianDateMonth>();
    PersianDateMonthModifyByNavigation = new HashSet<PersianDateMonth>();
    Person = new HashSet<Person>();
    PersonBankAccountCreateByNavigation = new HashSet<PersonBankAccount>();
    PersonBankAccountModifyByNavigation = new HashSet<PersonBankAccount>();
    PersonEconomicProfileCreateByNavigation = new HashSet<PersonEconomicProfile>();
    PersonEconomicProfileModifyByNavigation = new HashSet<PersonEconomicProfile>();
    PersonEducationCreateByNavigation = new HashSet<PersonEducation>();
    PersonEducationModifyByNavigation = new HashSet<PersonEducation>();
    PersonPostCreateByNavigation = new HashSet<PersonPost>();
    PersonPostModifyByNavigation = new HashSet<PersonPost>();
    PostCreateByNavigation = new HashSet<Post>();
    PostModifyByNavigation = new HashSet<Post>();
    PreRegistrationCreateByNavigation = new HashSet<PreRegistration>();
    PreRegistrationModifyByNavigation = new HashSet<PreRegistration>();
    PreRegistrationUser = new HashSet<PreRegistration>();
    ProductCategoryCreateByNavigation = new HashSet<ProductCategory>();
    ProductCategoryModifyByNavigation = new HashSet<ProductCategory>();
    ProductCreateByNavigation = new HashSet<Product>();
    ProductGroupCreateByNavigation = new HashSet<ProductGroup>();
    ProductGroupModifyByNavigation = new HashSet<ProductGroup>();
    ProductModifyByNavigation = new HashSet<Product>();
    ProductSalesPlanningConversationCreateByNavigation = new HashSet<ProductSalesPlanningConversation>();
    ProductSalesPlanningConversationMessageCreateByNavigation = new HashSet<ProductSalesPlanningConversationMessage>();
    ProductSalesPlanningConversationMessageModifyByNavigation = new HashSet<ProductSalesPlanningConversationMessage>();
    ProductSalesPlanningConversationModifyByNavigation = new HashSet<ProductSalesPlanningConversation>();
    ProductSalesPlanningCreateByNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningDetailCreateByNavigation = new HashSet<ProductSalesPlanningDetail>();
    ProductSalesPlanningDetailModifyByNavigation = new HashSet<ProductSalesPlanningDetail>();
    ProductSalesPlanningDiscountCreateByNavigation = new HashSet<ProductSalesPlanningDiscount>();
    ProductSalesPlanningDiscountModifyByNavigation = new HashSet<ProductSalesPlanningDiscount>();
    ProductSalesPlanningItemGroupCreateByNavigation = new HashSet<ProductSalesPlanningItemGroup>();
    ProductSalesPlanningItemGroupModifyByNavigation = new HashSet<ProductSalesPlanningItemGroup>();
    ProductSalesPlanningModifyByNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningReviewCreateByNavigation = new HashSet<ProductSalesPlanningReview>();
    ProductSalesPlanningReviewModifyByNavigation = new HashSet<ProductSalesPlanningReview>();
    ProductSalesPlanningStudentCreateByNavigation = new HashSet<ProductSalesPlanningStudent>();
    ProductSalesPlanningStudentModifyByNavigation = new HashSet<ProductSalesPlanningStudent>();
    ProductSalesPlanningStudentStudent = new HashSet<ProductSalesPlanningStudent>();
    ProductSalesPlanningTeacherCreateByNavigation = new HashSet<ProductSalesPlanningTeacher>();
    ProductSalesPlanningTeacherModifyByNavigation = new HashSet<ProductSalesPlanningTeacher>();
    ProductSalesPlanningTeacherTeacherUser = new HashSet<ProductSalesPlanningTeacher>();
    ProductSpecificationOptionCreateByNavigation = new HashSet<ProductSpecificationOption>();
    ProductSpecificationOptionModifyByNavigation = new HashSet<ProductSpecificationOption>();
    ProductTagsCreateByNavigation = new HashSet<ProductTags>();
    ProductTagsModifyByNavigation = new HashSet<ProductTags>();
    ProductTemplateCreateByNavigation = new HashSet<ProductTemplate>();
    ProductTemplateModifyByNavigation = new HashSet<ProductTemplate>();
    ProductTypeCreateByNavigation = new HashSet<ProductType>();
    ProductTypeModifyByNavigation = new HashSet<ProductType>();
    ProductTypeSpecificationCreateByNavigation = new HashSet<ProductTypeSpecification>();
    ProductTypeSpecificationModifyByNavigation = new HashSet<ProductTypeSpecification>();
    ProjectCreateByNavigation = new HashSet<Project>();
    ProjectModifyByNavigation = new HashSet<Project>();
    QuestionCategoryCreateByNavigation = new HashSet<QuestionCategory>();
    QuestionCategoryModifyByNavigation = new HashSet<QuestionCategory>();
    QuestionCreateByNavigation = new HashSet<Question>();
    QuestionModifyByNavigation = new HashSet<Question>();
    QuestionOptionCreateByNavigation = new HashSet<QuestionOption>();
    QuestionOptionModifyByNavigation = new HashSet<QuestionOption>();
    RecipientCreateByNavigation = new HashSet<Recipient>();
    RecipientModifyByNavigation = new HashSet<Recipient>();
    RecipientPersonCreateByNavigation = new HashSet<RecipientPerson>();
    RecipientPersonModifyByNavigation = new HashSet<RecipientPerson>();
    RefundCreateByNavigation = new HashSet<Refund>();
    RefundDetailArchiveByNavigation = new HashSet<RefundDetail>();
    RefundDetailAssigneeNavigation = new HashSet<RefundDetail>();
    RefundDetailCreateByNavigation = new HashSet<RefundDetail>();
    RefundDetailModifyByNavigation = new HashSet<RefundDetail>();
    RefundDetailRefundAmountSetByNavigation = new HashSet<RefundDetail>();
    RefundModifyByNavigation = new HashSet<Refund>();
    RefundPaymentTransactionItemRequestCreateByNavigation = new HashSet<RefundPaymentTransactionItemRequest>();
    RefundPaymentTransactionItemRequestModifyByNavigation = new HashSet<RefundPaymentTransactionItemRequest>();
    ReportConnectionStringCreateByNavigation = new HashSet<ReportConnectionString>();
    ReportConnectionStringModifyByNavigation = new HashSet<ReportConnectionString>();
    ReportGroupReportCreateByNavigation = new HashSet<ReportGroupReport>();
    ReportGroupReportModifyByNavigation = new HashSet<ReportGroupReport>();
    ReportStoreApplicationCreateByNavigation = new HashSet<ReportStoreApplication>();
    ReportStoreApplicationModifyByNavigation = new HashSet<ReportStoreApplication>();
    ReportStoreCreateByNavigation = new HashSet<ReportStore>();
    ReportStoreModifyByNavigation = new HashSet<ReportStore>();
    RepresentationCategoryCreateByNavigation = new HashSet<RepresentationCategory>();
    RepresentationCategoryModifyByNavigation = new HashSet<RepresentationCategory>();
    RepresentationCreateByNavigation = new HashSet<Representation>();
    RepresentationEstablishedLicenseTypeCreateByNavigation = new HashSet<RepresentationEstablishedLicenseType>();
    RepresentationEstablishedLicenseTypeModifyByNavigation = new HashSet<RepresentationEstablishedLicenseType>();
    RepresentationModifyByNavigation = new HashSet<Representation>();
    RepresentationPersonCreateByNavigation = new HashSet<RepresentationPerson>();
    RepresentationPersonModifyByNavigation = new HashSet<RepresentationPerson>();
    RepresentationTeacherCreateByNavigation = new HashSet<RepresentationTeacher>();
    RepresentationTeacherModifyByNavigation = new HashSet<RepresentationTeacher>();
    ResourceAttributeCreateByNavigation = new HashSet<ResourceAttribute>();
    ResourceAttributeModifyByNavigation = new HashSet<ResourceAttribute>();
    ResourceAttributeRoleCreateByNavigation = new HashSet<ResourceAttributeRole>();
    ResourceAttributeRoleModifyByNavigation = new HashSet<ResourceAttributeRole>();
    ResourceCreateByNavigation = new HashSet<Resource>();
    ResourceModifyByNavigation = new HashSet<Resource>();
    RewardPointsHistoryCreateByNavigation = new HashSet<RewardPointsHistory>();
    RewardPointsHistoryModifyByNavigation = new HashSet<RewardPointsHistory>();
    RoleCreateByNavigation = new HashSet<Role>();
    RoleDashboardCreateByNavigation = new HashSet<RoleDashboard>();
    RoleDashboardModifyByNavigation = new HashSet<RoleDashboard>();
    RoleModifyByNavigation = new HashSet<Role>();
    RoleReportCreateByNavigation = new HashSet<RoleReport>();
    RoleReportModifyByNavigation = new HashSet<RoleReport>();
    RollCallCreateByNavigation = new HashSet<RollCall>();
    RollCallModifyByNavigation = new HashSet<RollCall>();
    SaleConfigurationAttributeCreateByNavigation = new HashSet<SaleConfigurationAttribute>();
    SaleConfigurationAttributeModifyByNavigation = new HashSet<SaleConfigurationAttribute>();
    SaleConfigurationCreateByNavigation = new HashSet<SaleConfiguration>();
    SaleConfigurationModifyByNavigation = new HashSet<SaleConfiguration>();
    ServiceCreateByNavigation = new HashSet<Service>();
    ServiceModifyByNavigation = new HashSet<Service>();
    ShoppingCartItemCreateByNavigation = new HashSet<ShoppingCartItem>();
    ShoppingCartItemModifyByNavigation = new HashSet<ShoppingCartItem>();
    SiteVisitCreateByNavigation = new HashSet<SiteVisit>();
    SiteVisitModifyByNavigation = new HashSet<SiteVisit>();
    SkillCreateByNavigation = new HashSet<Skill>();
    SkillModifyByNavigation = new HashSet<Skill>();
    SkyRoomUserCreateByNavigation = new HashSet<SkyRoomUser>();
    SkyRoomUserModifyByNavigation = new HashSet<SkyRoomUser>();
    SpecificationCreateByNavigation = new HashSet<Specification>();
    SpecificationModifyByNavigation = new HashSet<Specification>();
    SpecificationOptionCreateByNavigation = new HashSet<SpecificationOption>();
    SpecificationOptionModifyByNavigation = new HashSet<SpecificationOption>();
    StructureInfoCreateByNavigation = new HashSet<StructureInfo>();
    StructureInfoModifyByNavigation = new HashSet<StructureInfo>();
    SubsidyCreateByNavigation = new HashSet<Subsidy>();
    SubsidyGroupCreateByNavigation = new HashSet<SubsidyGroup>();
    SubsidyGroupModifyByNavigation = new HashSet<SubsidyGroup>();
    SubsidyModifyByNavigation = new HashSet<Subsidy>();
    SurveyOptionCreateByNavigation = new HashSet<SurveyOption>();
    SurveyOptionModifyByNavigation = new HashSet<SurveyOption>();
    TaxCategoryCreateByNavigation = new HashSet<TaxCategory>();
    TaxCategoryModifyByNavigation = new HashSet<TaxCategory>();
    TaxCategorySaleConfigurationCreateByNavigation = new HashSet<TaxCategorySaleConfiguration>();
    TaxCategorySaleConfigurationModifyByNavigation = new HashSet<TaxCategorySaleConfiguration>();
    UnitCreateByNavigation = new HashSet<Unit>();
    UnitModifyByNavigation = new HashSet<Unit>();
    UploadedBulkPasswordResetCreateByNavigation = new HashSet<UploadedBulkPasswordReset>();
    UploadedBulkPasswordResetModifyByNavigation = new HashSet<UploadedBulkPasswordReset>();
    UploadedCertificateCreateByNavigation = new HashSet<UploadedCertificate>();
    UploadedCertificateModifyByNavigation = new HashSet<UploadedCertificate>();
    UploadedFactorInstallmentPaymentCreateByNavigation = new HashSet<UploadedFactorInstallmentPayment>();
    UploadedFactorInstallmentPaymentModifyByNavigation = new HashSet<UploadedFactorInstallmentPayment>();
    UploadedProductItemCreateByNavigation = new HashSet<UploadedProductItem>();
    UploadedProductItemModifyByNavigation = new HashSet<UploadedProductItem>();
    UploadedQuestionCreateByNavigation = new HashSet<UploadedQuestion>();
    UploadedQuestionModifyByNavigation = new HashSet<UploadedQuestion>();
    UploadedUserCreateByNavigation = new HashSet<UploadedUser>();
    UploadedUserModifyByNavigation = new HashSet<UploadedUser>();
    UploadedVoucherCreateByNavigation = new HashSet<UploadedVoucher>();
    UploadedVoucherModifyByNavigation = new HashSet<UploadedVoucher>();
    UsedSubsidyCreateByNavigation = new HashSet<UsedSubsidy>();
    UsedSubsidyModifyByNavigation = new HashSet<UsedSubsidy>();
    UserApplicationCreateByNavigation = new HashSet<UserApplication>();
    UserApplicationModifyByNavigation = new HashSet<UserApplication>();
    UserApplicationUser = new HashSet<UserApplication>();
    UserDashboardCreateByNavigation = new HashSet<UserDashboard>();
    UserDashboardModifyByNavigation = new HashSet<UserDashboard>();
    UserDashboardUser = new HashSet<UserDashboard>();
    UserExamCreateByNavigation = new HashSet<UserExam>();
    UserExamModifyByNavigation = new HashSet<UserExam>();
    UserGroupCreateByNavigation = new HashSet<UserGroup>();
    UserGroupMappingCreateByNavigation = new HashSet<UserGroupMapping>();
    UserGroupMappingModifyByNavigation = new HashSet<UserGroupMapping>();
    UserGroupMappingUser = new HashSet<UserGroupMapping>();
    UserGroupModifyByNavigation = new HashSet<UserGroup>();
    UserHomeWorkCreateByNavigation = new HashSet<UserHomeWork>();
    UserHomeWorkModifyByNavigation = new HashSet<UserHomeWork>();
    UserReportCreateByNavigation = new HashSet<UserReport>();
    UserReportModifyByNavigation = new HashSet<UserReport>();
    UserReportUser = new HashSet<UserReport>();
    UserRoleCreateByNavigation = new HashSet<UserRole>();
    UserRoleModifyByNavigation = new HashSet<UserRole>();
    UserRoleUser = new HashSet<UserRole>();
    UserValidIpAddressCreateByNavigation = new HashSet<UserValidIpAddress>();
    UserValidIpAddressModifyByNavigation = new HashSet<UserValidIpAddress>();
    UserValidIpAddressUser = new HashSet<UserValidIpAddress>();
    ValidHekmatCardCreateByNavigation = new HashSet<ValidHekmatCard>();
    ValidHekmatCardModifyByNavigation = new HashSet<ValidHekmatCard>();
    VersionInfoCreateByNavigation = new HashSet<VersionInfo>();
    VersionInfoModifyByNavigation = new HashSet<VersionInfo>();
    VirtualClassCreateByNavigation = new HashSet<VirtualClass>();
    VirtualClassModifyByNavigation = new HashSet<VirtualClass>();
    VirtualClassRecordingDetailCreateByNavigation = new HashSet<VirtualClassRecordingDetail>();
    VirtualClassRecordingDetailModifyByNavigation = new HashSet<VirtualClassRecordingDetail>();
    VirtualClassRoomConfigCreateByNavigation = new HashSet<VirtualClassRoomConfig>();
    VirtualClassRoomConfigModifyByNavigation = new HashSet<VirtualClassRoomConfig>();
    VirtualClassStudentCreateByNavigation = new HashSet<VirtualClassStudent>();
    VirtualClassStudentModifyByNavigation = new HashSet<VirtualClassStudent>();
    VirtualClassTeacherCreateByNavigation = new HashSet<VirtualClassTeacher>();
    VirtualClassTeacherModifyByNavigation = new HashSet<VirtualClassTeacher>();
    VocherCreateByNavigation = new HashSet<Vocher>();
    VocherModifyByNavigation = new HashSet<Vocher>();
    VocherTypeCreateByNavigation = new HashSet<VocherType>();
    VocherTypeModifyByNavigation = new HashSet<VocherType>();
    WalletAssignment = new HashSet<WalletAssignment>();
    WalletCardGroupCreateByNavigation = new HashSet<WalletCardGroup>();
    WalletCardGroupModifyByNavigation = new HashSet<WalletCardGroup>();
    WalletCardMemberCreateByNavigation = new HashSet<WalletCardMember>();
    WalletCardMemberModifyByNavigation = new HashSet<WalletCardMember>();
    WalletCouponBatchCreateByNavigation = new HashSet<WalletCouponBatch>();
    WalletCouponBatchModifyByNavigation = new HashSet<WalletCouponBatch>();
    WalletCouponCreateByNavigation = new HashSet<WalletCoupon>();
    WalletCouponModifyByNavigation = new HashSet<WalletCoupon>();
    WalletCouponUser = new HashSet<WalletCoupon>();
    WalletProgramCreateByNavigation = new HashSet<WalletProgram>();
    WalletProgramModifyByNavigation = new HashSet<WalletProgram>();
    WalletReservationCreateByNavigation = new HashSet<WalletReservation>();
    WalletReservationModifyByNavigation = new HashSet<WalletReservation>();
    WalletReservationUser = new HashSet<WalletReservation>();
    WalletScopeCreateByNavigation = new HashSet<WalletScope>();
    WalletScopeModifyByNavigation = new HashSet<WalletScope>();
    WalletTransactionCreateByNavigation = new HashSet<WalletTransaction>();
    WalletTransactionModifyByNavigation = new HashSet<WalletTransaction>();
    WalletTransactionUser = new HashSet<WalletTransaction>();
    WarehouseCreateByNavigation = new HashSet<Warehouse>();
    WarehouseModifyByNavigation = new HashSet<Warehouse>();
    YashilCmsCreateByNavigation = new HashSet<YashilCms>();
    YashilCmsModifyByNavigation = new HashSet<YashilCms>();
    YashilDataProviderCreateByNavigation = new HashSet<YashilDataProvider>();
    YashilDataProviderModifyByNavigation = new HashSet<YashilDataProvider>();
    YashilDayOfWeekCreateByNavigation = new HashSet<YashilDayOfWeek>();
    YashilDayOfWeekModifyByNavigation = new HashSet<YashilDayOfWeek>();
    YashilHekmatTerminalCreateByNavigation = new HashSet<YashilHekmatTerminal>();
    YashilHekmatTerminalModifyByNavigation = new HashSet<YashilHekmatTerminal>();
    YashilHekmatTerminalSupplierCreateByNavigation = new HashSet<YashilHekmatTerminalSupplier>();
    YashilHekmatTerminalSupplierModifyByNavigation = new HashSet<YashilHekmatTerminalSupplier>();
    YashilHekmatTerminalSupplierUser = new HashSet<YashilHekmatTerminalSupplier>();
    YashilSchedulerCreateByNavigation = new HashSet<YashilScheduler>();
    YashilSchedulerModifyByNavigation = new HashSet<YashilScheduler>();
 }

/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// نام کاربری
///</summary>
public string UserName { get; set; }
/// <summary>
///         /// ایمیل
///</summary>
public string Email { get; set; }
/// <summary>
///         /// کلمه عبور
///</summary>
public byte[] Password { get; set; }
/// <summary>
///         /// فعال
///</summary>
public bool IsActive { get; set; }
/// <summary>
///         /// سازمان
///</summary>
public int OrganizationId { get; set; }
public byte[] PasswordSalt { get; set; }
/// <summary>
///         /// ایجاد کننده
///</summary>
public int CreateBy { get; set; }
/// <summary>
///         /// ویرایش کننده
///</summary>
public int? ModifyBy { get; set; }
/// <summary>
///         /// زمان ایجاد
///</summary>
public DateTime CreationDate { get; set; }
/// <summary>
///         /// زمان تغییر
///</summary>
public DateTime? ModificationDate { get; set; }
/// <summary>
///         /// برنامه
///</summary>
public int ApplicationId { get; set; }
/// <summary>
///         /// سطح دسترسی
///</summary>
public int AccessLevelId { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
public int CreatorOrganizationId { get; set; }
public int PersonId { get; set; }
public DateTime? LastLoginDate { get; set; }
public DateTime? LastChangePasswordDate { get; set; }
public int FailLoginTryCount { get; set; }
public bool LockDueToFailLoginNumber { get; set; }
public DateTime? LockDueToFailLoginNumberDateTime { get; set; }
/// <summary>
///         /// نیاز به ارسال پیامک برای احراز هویت
///</summary>
public bool NeedToSendSmsForAuthentication { get; set; }
public int UserGroupId { get; set; }
public int? LastSendLogId { get; set; }
public string Code { get; set; }
/// <summary>
///         /// کد خصوصی
///</summary>
public string PrivateKey { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual Organization Organization { get; set; }
    public virtual Person PersonNavigation { get; set; }
    public virtual AdditionalUserProp AdditionalUserPropUser { get; set; }
public virtual ICollection<AccessLevel>
    AccessLevelCreateByNavigation { get; set; }
public virtual ICollection<AccessLevel>
    AccessLevelModifyByNavigation { get; set; }
public virtual ICollection<ActiveStorageAttachment>
    ActiveStorageAttachmentCreateByNavigation { get; set; }
public virtual ICollection<ActiveStorageAttachment>
    ActiveStorageAttachmentModifyByNavigation { get; set; }
public virtual ICollection<ActiveStorageBlobs>
    ActiveStorageBlobsCreateByNavigation { get; set; }
public virtual ICollection<ActiveStorageBlobs>
    ActiveStorageBlobsModifyByNavigation { get; set; }
public virtual ICollection<Activity>
    ActivityCreateByNavigation { get; set; }
public virtual ICollection<ActivityLog>
    ActivityLogCreateByNavigation { get; set; }
public virtual ICollection<ActivityLog>
    ActivityLogModifyByNavigation { get; set; }
public virtual ICollection<ActivityLog>
    ActivityLogUser { get; set; }
public virtual ICollection<Activity>
    ActivityModifyByNavigation { get; set; }
public virtual ICollection<AdditionalUserPropCompanyGroup>
    AdditionalUserPropCompanyGroupCreateByNavigation { get; set; }
public virtual ICollection<AdditionalUserPropCompanyGroup>
    AdditionalUserPropCompanyGroupModifyByNavigation { get; set; }
public virtual ICollection<AdditionalUserProp>
    AdditionalUserPropCreateByNavigation { get; set; }
public virtual ICollection<AdditionalUserProp>
    AdditionalUserPropModifyByNavigation { get; set; }
public virtual ICollection<Address>
    AddressCreateByNavigation { get; set; }
public virtual ICollection<Address>
    AddressModifyByNavigation { get; set; }
public virtual ICollection<Affiliate>
    AffiliateCreateByNavigation { get; set; }
public virtual ICollection<Affiliate>
    AffiliateModifyByNavigation { get; set; }
public virtual ICollection<AppAction>
    AppActionCreateByNavigation { get; set; }
public virtual ICollection<AppAction>
    AppActionModifyByNavigation { get; set; }
public virtual ICollection<AppConfig>
    AppConfigCreateByNavigation { get; set; }
public virtual ICollection<AppConfig>
    AppConfigModifyByNavigation { get; set; }
public virtual ICollection<AppDocument>
    AppDocumentCreateByNavigation { get; set; }
public virtual ICollection<AppDocument>
    AppDocumentModifyByNavigation { get; set; }
public virtual ICollection<AppEntityAttribute>
    AppEntityAttributeCreateByNavigation { get; set; }
public virtual ICollection<AppEntityAttributeMapping>
    AppEntityAttributeMappingCreateByNavigation { get; set; }
public virtual ICollection<AppEntityAttributeMapping>
    AppEntityAttributeMappingModifyByNavigation { get; set; }
public virtual ICollection<AppEntityAttribute>
    AppEntityAttributeModifyByNavigation { get; set; }
public virtual ICollection<AppEntityAttributeValue>
    AppEntityAttributeValueCreateByNavigation { get; set; }
public virtual ICollection<AppEntityAttributeValue>
    AppEntityAttributeValueModifyByNavigation { get; set; }
public virtual ICollection<AppEntityCustomerClub>
    AppEntityCustomerClubCreateByNavigation { get; set; }
public virtual ICollection<AppEntityCustomerClub>
    AppEntityCustomerClubModifyByNavigation { get; set; }
public virtual ICollection<Application>
    ApplicationCreateByNavigation { get; set; }
public virtual ICollection<ApplicationCustomerClub>
    ApplicationCustomerClubCreateByNavigation { get; set; }
public virtual ICollection<ApplicationCustomerClub>
    ApplicationCustomerClubModifyByNavigation { get; set; }
public virtual ICollection<Application>
    ApplicationModifyByNavigation { get; set; }
public virtual ICollection<ApplicationPaymentMethod>
    ApplicationPaymentMethodCreateByNavigation { get; set; }
public virtual ICollection<ApplicationPaymentMethod>
    ApplicationPaymentMethodModifyByNavigation { get; set; }
public virtual ICollection<ArInternalMetadata>
    ArInternalMetadataCreateByNavigation { get; set; }
public virtual ICollection<ArInternalMetadata>
    ArInternalMetadataModifyByNavigation { get; set; }
public virtual ICollection<CardAmountHistory>
    CardAmountHistoryCreateByNavigation { get; set; }
public virtual ICollection<CardAmountHistory>
    CardAmountHistoryModifyByNavigation { get; set; }
public virtual ICollection<CardInfo>
    CardInfoCreateByNavigation { get; set; }
public virtual ICollection<CardInfo>
    CardInfoModifyByNavigation { get; set; }
public virtual ICollection<Category>
    CategoryCreateByNavigation { get; set; }
public virtual ICollection<CategoryCustomerClub>
    CategoryCustomerClubCreateByNavigation { get; set; }
public virtual ICollection<CategoryCustomerClub>
    CategoryCustomerClubModifyByNavigation { get; set; }
public virtual ICollection<CategoryGroup>
    CategoryGroupCreateByNavigation { get; set; }
public virtual ICollection<CategoryGroup>
    CategoryGroupModifyByNavigation { get; set; }
public virtual ICollection<Category>
    CategoryModifyByNavigation { get; set; }
public virtual ICollection<CategorySpecification>
    CategorySpecificationCreateByNavigation { get; set; }
public virtual ICollection<CategorySpecification>
    CategorySpecificationModifyByNavigation { get; set; }
public virtual ICollection<Certificate>
    CertificateCreateByNavigation { get; set; }
public virtual ICollection<Certificate>
    CertificateModifyByNavigation { get; set; }
public virtual ICollection<City>
    CityCreateByNavigation { get; set; }
public virtual ICollection<City>
    CityModifyByNavigation { get; set; }
public virtual ICollection<CitySubsidyFactor>
    CitySubsidyFactorCreateByNavigation { get; set; }
public virtual ICollection<CitySubsidyFactor>
    CitySubsidyFactorModifyByNavigation { get; set; }
public virtual ICollection<ClassRoom>
    ClassRoomCreateByNavigation { get; set; }
public virtual ICollection<ClassRoom>
    ClassRoomModifyByNavigation { get; set; }
public virtual ICollection<ClassRoomReservation>
    ClassRoomReservationCreateByNavigation { get; set; }
public virtual ICollection<ClassRoomReservation>
    ClassRoomReservationModifyByNavigation { get; set; }
public virtual ICollection<ClassRoomReservationSchedule>
    ClassRoomReservationScheduleCreateByNavigation { get; set; }
public virtual ICollection<ClassRoomReservationSchedule>
    ClassRoomReservationScheduleModifyByNavigation { get; set; }
public virtual ICollection<ClassSession>
    ClassSessionCreateByNavigation { get; set; }
public virtual ICollection<ClassSession>
    ClassSessionModifyByNavigation { get; set; }
public virtual ICollection<CommonBaseData>
    CommonBaseDataCreateByNavigation { get; set; }
public virtual ICollection<CommonBaseData>
    CommonBaseDataModifyByNavigation { get; set; }
public virtual ICollection<CommonBaseType>
    CommonBaseTypeCreateByNavigation { get; set; }
public virtual ICollection<CommonBaseType>
    CommonBaseTypeModifyByNavigation { get; set; }
public virtual ICollection<Company>
    CompanyCreateByNavigation { get; set; }
public virtual ICollection<CompanyGroup>
    CompanyGroupCreateByNavigation { get; set; }
public virtual ICollection<CompanyGroup>
    CompanyGroupModifyByNavigation { get; set; }
public virtual ICollection<CompanyMainCategory>
    CompanyMainCategoryCreateByNavigation { get; set; }
public virtual ICollection<CompanyMainCategory>
    CompanyMainCategoryModifyByNavigation { get; set; }
public virtual ICollection<Company>
    CompanyModifyByNavigation { get; set; }
public virtual ICollection<ContentCategory>
    ContentCategoryCreateByNavigation { get; set; }
public virtual ICollection<ContentCategory>
    ContentCategoryModifyByNavigation { get; set; }
public virtual ICollection<Contract>
    ContractCreateByNavigation { get; set; }
public virtual ICollection<Contract>
    ContractModifyByNavigation { get; set; }
public virtual ICollection<ContractPaymentMethod>
    ContractPaymentMethodCreateByNavigation { get; set; }
public virtual ICollection<ContractPaymentMethod>
    ContractPaymentMethodModifyByNavigation { get; set; }
public virtual ICollection<Conversation>
    ConversationCreateByNavigation { get; set; }
public virtual ICollection<Conversation>
    ConversationModifyByNavigation { get; set; }
public virtual ICollection<ConversationRecipient>
    ConversationRecipientCreateByNavigation { get; set; }
public virtual ICollection<ConversationRecipient>
    ConversationRecipientModifyByNavigation { get; set; }
public virtual ICollection<ConversationRecipient>
    ConversationRecipientToUser { get; set; }
public virtual ICollection<ConversationSeen>
    ConversationSeenCreateByNavigation { get; set; }
public virtual ICollection<ConversationSeen>
    ConversationSeenModifyByNavigation { get; set; }
public virtual ICollection<Conversation>
    ConversationSenderNavigation { get; set; }
public virtual ICollection<Corporation>
    CorporationCreateByNavigation { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCoCreateByNavigation { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCoModifyByNavigation { get; set; }
public virtual ICollection<Corporation>
    CorporationModifyByNavigation { get; set; }
public virtual ICollection<Course>
    CourseCreateByNavigation { get; set; }
public virtual ICollection<Course>
    CourseModifyByNavigation { get; set; }
public virtual ICollection<CreditOrCashPayment>
    CreditOrCashPaymentCreateByNavigation { get; set; }
public virtual ICollection<CreditOrCashPayment>
    CreditOrCashPaymentModifyByNavigation { get; set; }
public virtual ICollection<Currency>
    CurrencyCreateByNavigation { get; set; }
public virtual ICollection<Currency>
    CurrencyModifyByNavigation { get; set; }
public virtual ICollection<CustomerClubCard>
    CustomerClubCardCreateByNavigation { get; set; }
public virtual ICollection<CustomerClubCard>
    CustomerClubCardModifyByNavigation { get; set; }
public virtual ICollection<CustomerClubCardPerson>
    CustomerClubCardPersonCreateByNavigation { get; set; }
public virtual ICollection<CustomerClubCardPerson>
    CustomerClubCardPersonModifyByNavigation { get; set; }
public virtual ICollection<CustomerClub>
    CustomerClubCreateByNavigation { get; set; }
public virtual ICollection<CustomerClub>
    CustomerClubModifyByNavigation { get; set; }
public virtual ICollection<CustomerClubPaymentInstallment>
    CustomerClubPaymentInstallmentCreateByNavigation { get; set; }
public virtual ICollection<CustomerClubPaymentInstallment>
    CustomerClubPaymentInstallmentModifyByNavigation { get; set; }
public virtual ICollection<CustomerClubPaymentMethodType>
    CustomerClubPaymentMethodTypeCreateByNavigation { get; set; }
public virtual ICollection<CustomerClubPaymentMethodType>
    CustomerClubPaymentMethodTypeModifyByNavigation { get; set; }
public virtual ICollection<DashboardConnectionString>
    DashboardConnectionStringCreateByNavigation { get; set; }
public virtual ICollection<DashboardConnectionString>
    DashboardConnectionStringModifyByNavigation { get; set; }
public virtual ICollection<DashboardGroupDashboard>
    DashboardGroupDashboardCreateByNavigation { get; set; }
public virtual ICollection<DashboardGroupDashboard>
    DashboardGroupDashboardModifyByNavigation { get; set; }
public virtual ICollection<DashboardStore>
    DashboardStoreCreateByNavigation { get; set; }
public virtual ICollection<DashboardStore>
    DashboardStoreModifyByNavigation { get; set; }
public virtual ICollection<DependentProductSale>
    DependentProductSaleCreateByNavigation { get; set; }
public virtual ICollection<DependentProductSale>
    DependentProductSaleModifyByNavigation { get; set; }
public virtual ICollection<DocFormat>
    DocFormatCreateByNavigation { get; set; }
public virtual ICollection<DocFormat>
    DocFormatModifyByNavigation { get; set; }
public virtual ICollection<DocType>
    DocTypeCreateByNavigation { get; set; }
public virtual ICollection<DocType>
    DocTypeModifyByNavigation { get; set; }
public virtual ICollection<DocumentCategory>
    DocumentCategoryCreateByNavigation { get; set; }
public virtual ICollection<DocumentCategory>
    DocumentCategoryModifyByNavigation { get; set; }
public virtual ICollection<Error>
    ErrorCreateByNavigation { get; set; }
public virtual ICollection<Error>
    ErrorModifyByNavigation { get; set; }
public virtual ICollection<ExamCategory>
    ExamCategoryCreateByNavigation { get; set; }
public virtual ICollection<ExamCategory>
    ExamCategoryModifyByNavigation { get; set; }
public virtual ICollection<Exam>
    ExamCreateByNavigation { get; set; }
public virtual ICollection<Exam>
    ExamModifyByNavigation { get; set; }
public virtual ICollection<ExamQuestionAnswer>
    ExamQuestionAnswerCreateByNavigation { get; set; }
public virtual ICollection<ExamQuestionAnswer>
    ExamQuestionAnswerModifyByNavigation { get; set; }
public virtual ICollection<ExamQuestion>
    ExamQuestionCreateByNavigation { get; set; }
public virtual ICollection<ExamQuestion>
    ExamQuestionModifyByNavigation { get; set; }
public virtual ICollection<Factor>
    FactorConfirmByNavigation { get; set; }
public virtual ICollection<Factor>
    FactorCreateByNavigation { get; set; }
public virtual ICollection<FactorDeduction>
    FactorDeductionCreateByNavigation { get; set; }
public virtual ICollection<FactorDeduction>
    FactorDeductionModifyByNavigation { get; set; }
public virtual ICollection<FactorGroup>
    FactorGroupCreateByNavigation { get; set; }
public virtual ICollection<FactorGroupItem>
    FactorGroupItemCreateByNavigation { get; set; }
public virtual ICollection<FactorGroupItem>
    FactorGroupItemModifyByNavigation { get; set; }
public virtual ICollection<FactorGroup>
    FactorGroupModifyByNavigation { get; set; }
public virtual ICollection<FactorItem>
    FactorItemCreateByNavigation { get; set; }
public virtual ICollection<FactorItem>
    FactorItemModifyByNavigation { get; set; }
public virtual ICollection<Factor>
    FactorModifyByNavigation { get; set; }
public virtual ICollection<FactorPayment>
    FactorPaymentCreateByNavigation { get; set; }
public virtual ICollection<FactorPaymentInstallment>
    FactorPaymentInstallmentCreateByNavigation { get; set; }
public virtual ICollection<FactorPaymentInstallment>
    FactorPaymentInstallmentModifyByNavigation { get; set; }
public virtual ICollection<FactorPayment>
    FactorPaymentModifyByNavigation { get; set; }
public virtual ICollection<FactorVerificationConfiguration>
    FactorVerificationConfigurationCreateByNavigation { get; set; }
public virtual ICollection<FactorVerificationConfiguration>
    FactorVerificationConfigurationModifyByNavigation { get; set; }
public virtual ICollection<FactorVerificationConfigurationRole>
    FactorVerificationConfigurationRoleCreateByNavigation { get; set; }
public virtual ICollection<FactorVerificationConfigurationRole>
    FactorVerificationConfigurationRoleModifyByNavigation { get; set; }
public virtual ICollection<FactorVerification>
    FactorVerificationCreateByNavigation { get; set; }
public virtual ICollection<FactorVerification>
    FactorVerificationModifyByNavigation { get; set; }
public virtual ICollection<FactorVerification>
    FactorVerificationVerifyByNavigation { get; set; }
public virtual ICollection<Feature>
    FeatureCreateByNavigation { get; set; }
public virtual ICollection<Feature>
    FeatureModifyByNavigation { get; set; }
public virtual ICollection<FileTransfer>
    FileTransferCreateByNavigation { get; set; }
public virtual ICollection<FileTransfer>
    FileTransferModifyByNavigation { get; set; }
public virtual ICollection<GiftCardType>
    GiftCardTypeCreateByNavigation { get; set; }
public virtual ICollection<GiftCardType>
    GiftCardTypeModifyByNavigation { get; set; }
public virtual ICollection<HekmatBlackListPhone>
    HekmatBlackListPhoneCreateByNavigation { get; set; }
public virtual ICollection<HekmatBlackListPhone>
    HekmatBlackListPhoneModifyByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayModifiedTransaction>
    HekmatPaymentGatewayModifiedTransactionCreateByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayModifiedTransaction>
    HekmatPaymentGatewayModifiedTransactionModifyByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequestCategory>
    HekmatPaymentGatewayTransactionBulkRequestCategoryCreateByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequestCategory>
    HekmatPaymentGatewayTransactionBulkRequestCategoryModifyByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequest>
    HekmatPaymentGatewayTransactionBulkRequestCreateByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequestDetail>
    HekmatPaymentGatewayTransactionBulkRequestDetailCreateByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequestDetail>
    HekmatPaymentGatewayTransactionBulkRequestDetailModifyByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequest>
    HekmatPaymentGatewayTransactionBulkRequestModifyByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransaction>
    HekmatPaymentGatewayTransactionCreateByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransaction>
    HekmatPaymentGatewayTransactionModifyByNavigation { get; set; }
public virtual ICollection<HekmatTerminal>
    HekmatTerminalCreateByNavigation { get; set; }
public virtual ICollection<HekmatTerminal>
    HekmatTerminalModifyByNavigation { get; set; }
public virtual ICollection<HekmatTransaction>
    HekmatTransactionCreateByNavigation { get; set; }
public virtual ICollection<HekmatTransaction>
    HekmatTransactionModifyByNavigation { get; set; }
public virtual ICollection<HomeWorkCategory>
    HomeWorkCategoryCreateByNavigation { get; set; }
public virtual ICollection<HomeWorkCategory>
    HomeWorkCategoryModifyByNavigation { get; set; }
public virtual ICollection<HomeWork>
    HomeWorkCreateByNavigation { get; set; }
public virtual ICollection<HomeWork>
    HomeWorkModifyByNavigation { get; set; }
public virtual ICollection<HybridPayment>
    HybridPaymentCreateByNavigation { get; set; }
public virtual ICollection<HybridPayment>
    HybridPaymentModifyByNavigation { get; set; }
public virtual ICollection<HybridPaymentRequest>
    HybridPaymentRequestCreateByNavigation { get; set; }
public virtual ICollection<HybridPaymentRequest>
    HybridPaymentRequestModifyByNavigation { get; set; }
public virtual ICollection<Installment>
    InstallmentCreateByNavigation { get; set; }
public virtual ICollection<Installment>
    InstallmentModifyByNavigation { get; set; }
public virtual ICollection<Invitation>
    InvitationCreateByNavigation { get; set; }
public virtual ICollection<Invitation>
    InvitationModifyByNavigation { get; set; }
public virtual ICollection<Issue>
    IssueAssigneeNavigation { get; set; }
public virtual ICollection<IssueComment>
    IssueCommentCreateByNavigation { get; set; }
public virtual ICollection<IssueComment>
    IssueCommentModifyByNavigation { get; set; }
public virtual ICollection<Issue>
    IssueCreateByNavigation { get; set; }
public virtual ICollection<Issue>
    IssueIssueOwnerNavigation { get; set; }
public virtual ICollection<IssueLogWork>
    IssueLogWorkCreateByNavigation { get; set; }
public virtual ICollection<IssueLogWork>
    IssueLogWorkModifyByNavigation { get; set; }
public virtual ICollection<Issue>
    IssueModifyByNavigation { get; set; }
public virtual ICollection<IssueParticipant>
    IssueParticipantCreateByNavigation { get; set; }
public virtual ICollection<IssueParticipant>
    IssueParticipantModifyByNavigation { get; set; }
public virtual ICollection<IssueParticipant>
    IssueParticipantUser { get; set; }
public virtual ICollection<IssueTypeItem>
    IssueTypeItemCreateByNavigation { get; set; }
public virtual ICollection<IssueTypeItem>
    IssueTypeItemModifyByNavigation { get; set; }
public virtual ICollection<ItemGroup>
    ItemGroupCreateByNavigation { get; set; }
public virtual ICollection<ItemGroup>
    ItemGroupModifyByNavigation { get; set; }
public virtual ICollection<JobCoCategory>
    JobCoCategoryCreateByNavigation { get; set; }
public virtual ICollection<JobCoCategory>
    JobCoCategoryModifyByNavigation { get; set; }
public virtual ICollection<JobCo>
    JobCoCreateByNavigation { get; set; }
public virtual ICollection<JobCo>
    JobCoModifyByNavigation { get; set; }
public virtual ICollection<JobCoSkill>
    JobCoSkillCreateByNavigation { get; set; }
public virtual ICollection<JobCoSkill>
    JobCoSkillModifyByNavigation { get; set; }
public virtual ICollection<JobCoTags>
    JobCoTagsCreateByNavigation { get; set; }
public virtual ICollection<JobCoTags>
    JobCoTagsModifyByNavigation { get; set; }
public virtual ICollection<JobCoUser>
    JobCoUserCreateByNavigation { get; set; }
public virtual ICollection<JobCoUser>
    JobCoUserJobApplicantUserNavigation { get; set; }
public virtual ICollection<JobCoUser>
    JobCoUserModifyByNavigation { get; set; }
public virtual ICollection<Job>
    JobCreateByNavigation { get; set; }
public virtual ICollection<Job>
    JobModifyByNavigation { get; set; }
public virtual ICollection<Keyword>
    KeywordCreateByNavigation { get; set; }
public virtual ICollection<Keyword>
    KeywordModifyByNavigation { get; set; }
public virtual ICollection<Language>
    LanguageCreateByNavigation { get; set; }
public virtual ICollection<Language>
    LanguageModifyByNavigation { get; set; }
public virtual ICollection<Loan>
    LoanCreateByNavigation { get; set; }
public virtual ICollection<LoanGuarantor>
    LoanGuarantorCreateByNavigation { get; set; }
public virtual ICollection<LoanGuarantor>
    LoanGuarantorModifyByNavigation { get; set; }
public virtual ICollection<Loan>
    LoanModifyByNavigation { get; set; }
public virtual ICollection<LocaleStringResource>
    LocaleStringResourceCreateByNavigation { get; set; }
public virtual ICollection<LocaleStringResource>
    LocaleStringResourceModifyByNavigation { get; set; }
public virtual ICollection<MainNews>
    MainNewsCreateByNavigation { get; set; }
public virtual ICollection<MainNews>
    MainNewsModifyByNavigation { get; set; }
public virtual ICollection<MediaCategory>
    MediaCategoryCreateByNavigation { get; set; }
public virtual ICollection<MediaCategory>
    MediaCategoryModifyByNavigation { get; set; }
public virtual ICollection<MediaComment>
    MediaCommentCreateByNavigation { get; set; }
public virtual ICollection<MediaComment>
    MediaCommentModifyByNavigation { get; set; }
public virtual ICollection<MediaKeyMoment>
    MediaKeyMomentCreateByNavigation { get; set; }
public virtual ICollection<MediaKeyMoment>
    MediaKeyMomentModifyByNavigation { get; set; }
public virtual ICollection<MediaKeyMoment>
    MediaKeyMomentUser { get; set; }
public virtual ICollection<MediaManufacturer>
    MediaManufacturerCreateByNavigation { get; set; }
public virtual ICollection<MediaManufacturer>
    MediaManufacturerModifyByNavigation { get; set; }
public virtual ICollection<MediaSession>
    MediaSessionCreateByNavigation { get; set; }
public virtual ICollection<MediaSession>
    MediaSessionModifyByNavigation { get; set; }
public virtual ICollection<MediaSessionViewingHistory>
    MediaSessionViewingHistoryCreateByNavigation { get; set; }
public virtual ICollection<MediaSessionViewingHistory>
    MediaSessionViewingHistoryModifyByNavigation { get; set; }
public virtual ICollection<Menu>
    MenuCreateByNavigation { get; set; }
public virtual ICollection<Menu>
    MenuModifyByNavigation { get; set; }
public virtual ICollection<MerchantInfo>
    MerchantInfoCreateByNavigation { get; set; }
public virtual ICollection<MerchantInfo>
    MerchantInfoModifyByNavigation { get; set; }
public virtual ICollection<MerchantInfo>
    MerchantInfoUser { get; set; }
public virtual ICollection<MerchantTerminalInfo>
    MerchantTerminalInfoCreateByNavigation { get; set; }
public virtual ICollection<MerchantTerminalInfo>
    MerchantTerminalInfoModifyByNavigation { get; set; }
public virtual ICollection<ModifiedOrderItem>
    ModifiedOrderItemAssigneeNavigation { get; set; }
public virtual ICollection<ModifiedOrderItem>
    ModifiedOrderItemCreateByNavigation { get; set; }
public virtual ICollection<ModifiedOrderItem>
    ModifiedOrderItemModifyByNavigation { get; set; }
public virtual ICollection<NajaDemand>
    NajaDemandCreateByNavigation { get; set; }
public virtual ICollection<NajaDemand>
    NajaDemandModifyByNavigation { get; set; }
public virtual ICollection<NajaPayment>
    NajaPaymentCreateByNavigation { get; set; }
public virtual ICollection<NajaPayment>
    NajaPaymentModifyByNavigation { get; set; }
public virtual ICollection<NajaRefund>
    NajaRefundCreateByNavigation { get; set; }
public virtual ICollection<NajaRefund>
    NajaRefundModifyByNavigation { get; set; }
public virtual ICollection<NewsKeyword>
    NewsKeywordCreateByNavigation { get; set; }
public virtual ICollection<NewsKeyword>
    NewsKeywordModifyByNavigation { get; set; }
public virtual ICollection<NewsStore>
    NewsStoreCreateByNavigation { get; set; }
public virtual ICollection<NewsStore>
    NewsStoreModifyByNavigation { get; set; }
public virtual ICollection<OrderChangePermission>
    OrderChangePermissionCreateByNavigation { get; set; }
public virtual ICollection<OrderChangePermission>
    OrderChangePermissionModifyByNavigation { get; set; }
public virtual ICollection<Order>
    OrderCreateByNavigation { get; set; }
public virtual ICollection<OrderItem>
    OrderItemCreateByNavigation { get; set; }
public virtual ICollection<OrderItemDependentProductSale>
    OrderItemDependentProductSaleCreateByNavigation { get; set; }
public virtual ICollection<OrderItemDependentProductSale>
    OrderItemDependentProductSaleModifyByNavigation { get; set; }
public virtual ICollection<OrderItemDetail>
    OrderItemDetailCreateByNavigation { get; set; }
public virtual ICollection<OrderItemDetail>
    OrderItemDetailModifyByNavigation { get; set; }
public virtual ICollection<OrderItem>
    OrderItemModifyByNavigation { get; set; }
public virtual ICollection<OrderItemPayment>
    OrderItemPaymentCreateByNavigation { get; set; }
public virtual ICollection<OrderItemPayment>
    OrderItemPaymentModifyByNavigation { get; set; }
public virtual ICollection<Order>
    OrderModifyByNavigation { get; set; }
public virtual ICollection<OrderNote>
    OrderNoteCreateByNavigation { get; set; }
public virtual ICollection<OrderNote>
    OrderNoteModifyByNavigation { get; set; }
public virtual ICollection<OrderTrakking>
    OrderTrakkingCreateByNavigation { get; set; }
public virtual ICollection<OrderTrakking>
    OrderTrakkingModifyByNavigation { get; set; }
public virtual ICollection<OrderTrakking>
    OrderTrakkingReferenceToNavigation { get; set; }
public virtual ICollection<Order>
    OrderUser { get; set; }
public virtual ICollection<Organization>
    OrganizationCreateByNavigation { get; set; }
public virtual ICollection<Organization>
    OrganizationModifyByNavigation { get; set; }
public virtual ICollection<OrganizationVirtualClassRoomConfig>
    OrganizationVirtualClassRoomConfigCreateByNavigation { get; set; }
public virtual ICollection<OrganizationVirtualClassRoomConfig>
    OrganizationVirtualClassRoomConfigModifyByNavigation { get; set; }
public virtual ICollection<PasswordBlackList>
    PasswordBlackListCreateByNavigation { get; set; }
public virtual ICollection<PasswordBlackList>
    PasswordBlackListModifyByNavigation { get; set; }
public virtual ICollection<PasswordHistory>
    PasswordHistoryCreateByNavigation { get; set; }
public virtual ICollection<PasswordHistory>
    PasswordHistoryModifyByNavigation { get; set; }
public virtual ICollection<Payment>
    PaymentCreateByNavigation { get; set; }
public virtual ICollection<PaymentGateway>
    PaymentGatewayCreateByNavigation { get; set; }
public virtual ICollection<PaymentGateway>
    PaymentGatewayModifyByNavigation { get; set; }
public virtual ICollection<PaymentGatewayPaymentMethod>
    PaymentGatewayPaymentMethodCreateByNavigation { get; set; }
public virtual ICollection<PaymentGatewayPaymentMethod>
    PaymentGatewayPaymentMethodModifyByNavigation { get; set; }
public virtual ICollection<Payment>
    PaymentModifyByNavigation { get; set; }
public virtual ICollection<PaymentTransactionItemRefund>
    PaymentTransactionItemRefundCreateByNavigation { get; set; }
public virtual ICollection<PaymentTransactionItemRefund>
    PaymentTransactionItemRefundModifyByNavigation { get; set; }
public virtual ICollection<PersianCalendar>
    PersianCalendarCreateByNavigation { get; set; }
public virtual ICollection<PersianCalendar>
    PersianCalendarModifyByNavigation { get; set; }
public virtual ICollection<PersianDateMonth>
    PersianDateMonthCreateByNavigation { get; set; }
public virtual ICollection<PersianDateMonth>
    PersianDateMonthModifyByNavigation { get; set; }
public virtual ICollection<Person>
    Person { get; set; }
public virtual ICollection<PersonBankAccount>
    PersonBankAccountCreateByNavigation { get; set; }
public virtual ICollection<PersonBankAccount>
    PersonBankAccountModifyByNavigation { get; set; }
public virtual ICollection<PersonEconomicProfile>
    PersonEconomicProfileCreateByNavigation { get; set; }
public virtual ICollection<PersonEconomicProfile>
    PersonEconomicProfileModifyByNavigation { get; set; }
public virtual ICollection<PersonEducation>
    PersonEducationCreateByNavigation { get; set; }
public virtual ICollection<PersonEducation>
    PersonEducationModifyByNavigation { get; set; }
public virtual ICollection<PersonPost>
    PersonPostCreateByNavigation { get; set; }
public virtual ICollection<PersonPost>
    PersonPostModifyByNavigation { get; set; }
public virtual ICollection<Post>
    PostCreateByNavigation { get; set; }
public virtual ICollection<Post>
    PostModifyByNavigation { get; set; }
public virtual ICollection<PreRegistration>
    PreRegistrationCreateByNavigation { get; set; }
public virtual ICollection<PreRegistration>
    PreRegistrationModifyByNavigation { get; set; }
public virtual ICollection<PreRegistration>
    PreRegistrationUser { get; set; }
public virtual ICollection<ProductCategory>
    ProductCategoryCreateByNavigation { get; set; }
public virtual ICollection<ProductCategory>
    ProductCategoryModifyByNavigation { get; set; }
public virtual ICollection<Product>
    ProductCreateByNavigation { get; set; }
public virtual ICollection<ProductGroup>
    ProductGroupCreateByNavigation { get; set; }
public virtual ICollection<ProductGroup>
    ProductGroupModifyByNavigation { get; set; }
public virtual ICollection<Product>
    ProductModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningConversation>
    ProductSalesPlanningConversationCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningConversationMessage>
    ProductSalesPlanningConversationMessageCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningConversationMessage>
    ProductSalesPlanningConversationMessageModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningConversation>
    ProductSalesPlanningConversationModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningDetail>
    ProductSalesPlanningDetailCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningDetail>
    ProductSalesPlanningDetailModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningDiscount>
    ProductSalesPlanningDiscountCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningDiscount>
    ProductSalesPlanningDiscountModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningItemGroup>
    ProductSalesPlanningItemGroupCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningItemGroup>
    ProductSalesPlanningItemGroupModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningReview>
    ProductSalesPlanningReviewCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningReview>
    ProductSalesPlanningReviewModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningStudent>
    ProductSalesPlanningStudentCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningStudent>
    ProductSalesPlanningStudentModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningStudent>
    ProductSalesPlanningStudentStudent { get; set; }
public virtual ICollection<ProductSalesPlanningTeacher>
    ProductSalesPlanningTeacherCreateByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningTeacher>
    ProductSalesPlanningTeacherModifyByNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningTeacher>
    ProductSalesPlanningTeacherTeacherUser { get; set; }
public virtual ICollection<ProductSpecificationOption>
    ProductSpecificationOptionCreateByNavigation { get; set; }
public virtual ICollection<ProductSpecificationOption>
    ProductSpecificationOptionModifyByNavigation { get; set; }
public virtual ICollection<ProductTags>
    ProductTagsCreateByNavigation { get; set; }
public virtual ICollection<ProductTags>
    ProductTagsModifyByNavigation { get; set; }
public virtual ICollection<ProductTemplate>
    ProductTemplateCreateByNavigation { get; set; }
public virtual ICollection<ProductTemplate>
    ProductTemplateModifyByNavigation { get; set; }
public virtual ICollection<ProductType>
    ProductTypeCreateByNavigation { get; set; }
public virtual ICollection<ProductType>
    ProductTypeModifyByNavigation { get; set; }
public virtual ICollection<ProductTypeSpecification>
    ProductTypeSpecificationCreateByNavigation { get; set; }
public virtual ICollection<ProductTypeSpecification>
    ProductTypeSpecificationModifyByNavigation { get; set; }
public virtual ICollection<Project>
    ProjectCreateByNavigation { get; set; }
public virtual ICollection<Project>
    ProjectModifyByNavigation { get; set; }
public virtual ICollection<QuestionCategory>
    QuestionCategoryCreateByNavigation { get; set; }
public virtual ICollection<QuestionCategory>
    QuestionCategoryModifyByNavigation { get; set; }
public virtual ICollection<Question>
    QuestionCreateByNavigation { get; set; }
public virtual ICollection<Question>
    QuestionModifyByNavigation { get; set; }
public virtual ICollection<QuestionOption>
    QuestionOptionCreateByNavigation { get; set; }
public virtual ICollection<QuestionOption>
    QuestionOptionModifyByNavigation { get; set; }
public virtual ICollection<Recipient>
    RecipientCreateByNavigation { get; set; }
public virtual ICollection<Recipient>
    RecipientModifyByNavigation { get; set; }
public virtual ICollection<RecipientPerson>
    RecipientPersonCreateByNavigation { get; set; }
public virtual ICollection<RecipientPerson>
    RecipientPersonModifyByNavigation { get; set; }
public virtual ICollection<Refund>
    RefundCreateByNavigation { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailArchiveByNavigation { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailAssigneeNavigation { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailCreateByNavigation { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailModifyByNavigation { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailRefundAmountSetByNavigation { get; set; }
public virtual ICollection<Refund>
    RefundModifyByNavigation { get; set; }
public virtual ICollection<RefundPaymentTransactionItemRequest>
    RefundPaymentTransactionItemRequestCreateByNavigation { get; set; }
public virtual ICollection<RefundPaymentTransactionItemRequest>
    RefundPaymentTransactionItemRequestModifyByNavigation { get; set; }
public virtual ICollection<ReportConnectionString>
    ReportConnectionStringCreateByNavigation { get; set; }
public virtual ICollection<ReportConnectionString>
    ReportConnectionStringModifyByNavigation { get; set; }
public virtual ICollection<ReportGroupReport>
    ReportGroupReportCreateByNavigation { get; set; }
public virtual ICollection<ReportGroupReport>
    ReportGroupReportModifyByNavigation { get; set; }
public virtual ICollection<ReportStoreApplication>
    ReportStoreApplicationCreateByNavigation { get; set; }
public virtual ICollection<ReportStoreApplication>
    ReportStoreApplicationModifyByNavigation { get; set; }
public virtual ICollection<ReportStore>
    ReportStoreCreateByNavigation { get; set; }
public virtual ICollection<ReportStore>
    ReportStoreModifyByNavigation { get; set; }
public virtual ICollection<RepresentationCategory>
    RepresentationCategoryCreateByNavigation { get; set; }
public virtual ICollection<RepresentationCategory>
    RepresentationCategoryModifyByNavigation { get; set; }
public virtual ICollection<Representation>
    RepresentationCreateByNavigation { get; set; }
public virtual ICollection<RepresentationEstablishedLicenseType>
    RepresentationEstablishedLicenseTypeCreateByNavigation { get; set; }
public virtual ICollection<RepresentationEstablishedLicenseType>
    RepresentationEstablishedLicenseTypeModifyByNavigation { get; set; }
public virtual ICollection<Representation>
    RepresentationModifyByNavigation { get; set; }
public virtual ICollection<RepresentationPerson>
    RepresentationPersonCreateByNavigation { get; set; }
public virtual ICollection<RepresentationPerson>
    RepresentationPersonModifyByNavigation { get; set; }
public virtual ICollection<RepresentationTeacher>
    RepresentationTeacherCreateByNavigation { get; set; }
public virtual ICollection<RepresentationTeacher>
    RepresentationTeacherModifyByNavigation { get; set; }
public virtual ICollection<ResourceAttribute>
    ResourceAttributeCreateByNavigation { get; set; }
public virtual ICollection<ResourceAttribute>
    ResourceAttributeModifyByNavigation { get; set; }
public virtual ICollection<ResourceAttributeRole>
    ResourceAttributeRoleCreateByNavigation { get; set; }
public virtual ICollection<ResourceAttributeRole>
    ResourceAttributeRoleModifyByNavigation { get; set; }
public virtual ICollection<Resource>
    ResourceCreateByNavigation { get; set; }
public virtual ICollection<Resource>
    ResourceModifyByNavigation { get; set; }
public virtual ICollection<RewardPointsHistory>
    RewardPointsHistoryCreateByNavigation { get; set; }
public virtual ICollection<RewardPointsHistory>
    RewardPointsHistoryModifyByNavigation { get; set; }
public virtual ICollection<Role>
    RoleCreateByNavigation { get; set; }
public virtual ICollection<RoleDashboard>
    RoleDashboardCreateByNavigation { get; set; }
public virtual ICollection<RoleDashboard>
    RoleDashboardModifyByNavigation { get; set; }
public virtual ICollection<Role>
    RoleModifyByNavigation { get; set; }
public virtual ICollection<RoleReport>
    RoleReportCreateByNavigation { get; set; }
public virtual ICollection<RoleReport>
    RoleReportModifyByNavigation { get; set; }
public virtual ICollection<RollCall>
    RollCallCreateByNavigation { get; set; }
public virtual ICollection<RollCall>
    RollCallModifyByNavigation { get; set; }
public virtual ICollection<SaleConfigurationAttribute>
    SaleConfigurationAttributeCreateByNavigation { get; set; }
public virtual ICollection<SaleConfigurationAttribute>
    SaleConfigurationAttributeModifyByNavigation { get; set; }
public virtual ICollection<SaleConfiguration>
    SaleConfigurationCreateByNavigation { get; set; }
public virtual ICollection<SaleConfiguration>
    SaleConfigurationModifyByNavigation { get; set; }
public virtual ICollection<Service>
    ServiceCreateByNavigation { get; set; }
public virtual ICollection<Service>
    ServiceModifyByNavigation { get; set; }
public virtual ICollection<ShoppingCartItem>
    ShoppingCartItemCreateByNavigation { get; set; }
public virtual ICollection<ShoppingCartItem>
    ShoppingCartItemModifyByNavigation { get; set; }
public virtual ICollection<SiteVisit>
    SiteVisitCreateByNavigation { get; set; }
public virtual ICollection<SiteVisit>
    SiteVisitModifyByNavigation { get; set; }
public virtual ICollection<Skill>
    SkillCreateByNavigation { get; set; }
public virtual ICollection<Skill>
    SkillModifyByNavigation { get; set; }
public virtual ICollection<SkyRoomUser>
    SkyRoomUserCreateByNavigation { get; set; }
public virtual ICollection<SkyRoomUser>
    SkyRoomUserModifyByNavigation { get; set; }
public virtual ICollection<Specification>
    SpecificationCreateByNavigation { get; set; }
public virtual ICollection<Specification>
    SpecificationModifyByNavigation { get; set; }
public virtual ICollection<SpecificationOption>
    SpecificationOptionCreateByNavigation { get; set; }
public virtual ICollection<SpecificationOption>
    SpecificationOptionModifyByNavigation { get; set; }
public virtual ICollection<StructureInfo>
    StructureInfoCreateByNavigation { get; set; }
public virtual ICollection<StructureInfo>
    StructureInfoModifyByNavigation { get; set; }
public virtual ICollection<Subsidy>
    SubsidyCreateByNavigation { get; set; }
public virtual ICollection<SubsidyGroup>
    SubsidyGroupCreateByNavigation { get; set; }
public virtual ICollection<SubsidyGroup>
    SubsidyGroupModifyByNavigation { get; set; }
public virtual ICollection<Subsidy>
    SubsidyModifyByNavigation { get; set; }
public virtual ICollection<SurveyOption>
    SurveyOptionCreateByNavigation { get; set; }
public virtual ICollection<SurveyOption>
    SurveyOptionModifyByNavigation { get; set; }
public virtual ICollection<TaxCategory>
    TaxCategoryCreateByNavigation { get; set; }
public virtual ICollection<TaxCategory>
    TaxCategoryModifyByNavigation { get; set; }
public virtual ICollection<TaxCategorySaleConfiguration>
    TaxCategorySaleConfigurationCreateByNavigation { get; set; }
public virtual ICollection<TaxCategorySaleConfiguration>
    TaxCategorySaleConfigurationModifyByNavigation { get; set; }
public virtual ICollection<Unit>
    UnitCreateByNavigation { get; set; }
public virtual ICollection<Unit>
    UnitModifyByNavigation { get; set; }
public virtual ICollection<UploadedBulkPasswordReset>
    UploadedBulkPasswordResetCreateByNavigation { get; set; }
public virtual ICollection<UploadedBulkPasswordReset>
    UploadedBulkPasswordResetModifyByNavigation { get; set; }
public virtual ICollection<UploadedCertificate>
    UploadedCertificateCreateByNavigation { get; set; }
public virtual ICollection<UploadedCertificate>
    UploadedCertificateModifyByNavigation { get; set; }
public virtual ICollection<UploadedFactorInstallmentPayment>
    UploadedFactorInstallmentPaymentCreateByNavigation { get; set; }
public virtual ICollection<UploadedFactorInstallmentPayment>
    UploadedFactorInstallmentPaymentModifyByNavigation { get; set; }
public virtual ICollection<UploadedProductItem>
    UploadedProductItemCreateByNavigation { get; set; }
public virtual ICollection<UploadedProductItem>
    UploadedProductItemModifyByNavigation { get; set; }
public virtual ICollection<UploadedQuestion>
    UploadedQuestionCreateByNavigation { get; set; }
public virtual ICollection<UploadedQuestion>
    UploadedQuestionModifyByNavigation { get; set; }
public virtual ICollection<UploadedUser>
    UploadedUserCreateByNavigation { get; set; }
public virtual ICollection<UploadedUser>
    UploadedUserModifyByNavigation { get; set; }
public virtual ICollection<UploadedVoucher>
    UploadedVoucherCreateByNavigation { get; set; }
public virtual ICollection<UploadedVoucher>
    UploadedVoucherModifyByNavigation { get; set; }
public virtual ICollection<UsedSubsidy>
    UsedSubsidyCreateByNavigation { get; set; }
public virtual ICollection<UsedSubsidy>
    UsedSubsidyModifyByNavigation { get; set; }
public virtual ICollection<UserApplication>
    UserApplicationCreateByNavigation { get; set; }
public virtual ICollection<UserApplication>
    UserApplicationModifyByNavigation { get; set; }
public virtual ICollection<UserApplication>
    UserApplicationUser { get; set; }
public virtual ICollection<UserDashboard>
    UserDashboardCreateByNavigation { get; set; }
public virtual ICollection<UserDashboard>
    UserDashboardModifyByNavigation { get; set; }
public virtual ICollection<UserDashboard>
    UserDashboardUser { get; set; }
public virtual ICollection<UserExam>
    UserExamCreateByNavigation { get; set; }
public virtual ICollection<UserExam>
    UserExamModifyByNavigation { get; set; }
public virtual ICollection<UserGroup>
    UserGroupCreateByNavigation { get; set; }
public virtual ICollection<UserGroupMapping>
    UserGroupMappingCreateByNavigation { get; set; }
public virtual ICollection<UserGroupMapping>
    UserGroupMappingModifyByNavigation { get; set; }
public virtual ICollection<UserGroupMapping>
    UserGroupMappingUser { get; set; }
public virtual ICollection<UserGroup>
    UserGroupModifyByNavigation { get; set; }
public virtual ICollection<UserHomeWork>
    UserHomeWorkCreateByNavigation { get; set; }
public virtual ICollection<UserHomeWork>
    UserHomeWorkModifyByNavigation { get; set; }
public virtual ICollection<UserReport>
    UserReportCreateByNavigation { get; set; }
public virtual ICollection<UserReport>
    UserReportModifyByNavigation { get; set; }
public virtual ICollection<UserReport>
    UserReportUser { get; set; }
public virtual ICollection<UserRole>
    UserRoleCreateByNavigation { get; set; }
public virtual ICollection<UserRole>
    UserRoleModifyByNavigation { get; set; }
public virtual ICollection<UserRole>
    UserRoleUser { get; set; }
public virtual ICollection<UserValidIpAddress>
    UserValidIpAddressCreateByNavigation { get; set; }
public virtual ICollection<UserValidIpAddress>
    UserValidIpAddressModifyByNavigation { get; set; }
public virtual ICollection<UserValidIpAddress>
    UserValidIpAddressUser { get; set; }
public virtual ICollection<ValidHekmatCard>
    ValidHekmatCardCreateByNavigation { get; set; }
public virtual ICollection<ValidHekmatCard>
    ValidHekmatCardModifyByNavigation { get; set; }
public virtual ICollection<VersionInfo>
    VersionInfoCreateByNavigation { get; set; }
public virtual ICollection<VersionInfo>
    VersionInfoModifyByNavigation { get; set; }
public virtual ICollection<VirtualClass>
    VirtualClassCreateByNavigation { get; set; }
public virtual ICollection<VirtualClass>
    VirtualClassModifyByNavigation { get; set; }
public virtual ICollection<VirtualClassRecordingDetail>
    VirtualClassRecordingDetailCreateByNavigation { get; set; }
public virtual ICollection<VirtualClassRecordingDetail>
    VirtualClassRecordingDetailModifyByNavigation { get; set; }
public virtual ICollection<VirtualClassRoomConfig>
    VirtualClassRoomConfigCreateByNavigation { get; set; }
public virtual ICollection<VirtualClassRoomConfig>
    VirtualClassRoomConfigModifyByNavigation { get; set; }
public virtual ICollection<VirtualClassStudent>
    VirtualClassStudentCreateByNavigation { get; set; }
public virtual ICollection<VirtualClassStudent>
    VirtualClassStudentModifyByNavigation { get; set; }
public virtual ICollection<VirtualClassTeacher>
    VirtualClassTeacherCreateByNavigation { get; set; }
public virtual ICollection<VirtualClassTeacher>
    VirtualClassTeacherModifyByNavigation { get; set; }
public virtual ICollection<Vocher>
    VocherCreateByNavigation { get; set; }
public virtual ICollection<Vocher>
    VocherModifyByNavigation { get; set; }
public virtual ICollection<VocherType>
    VocherTypeCreateByNavigation { get; set; }
public virtual ICollection<VocherType>
    VocherTypeModifyByNavigation { get; set; }
public virtual ICollection<WalletAssignment>
    WalletAssignment { get; set; }
public virtual ICollection<WalletCardGroup>
    WalletCardGroupCreateByNavigation { get; set; }
public virtual ICollection<WalletCardGroup>
    WalletCardGroupModifyByNavigation { get; set; }
public virtual ICollection<WalletCardMember>
    WalletCardMemberCreateByNavigation { get; set; }
public virtual ICollection<WalletCardMember>
    WalletCardMemberModifyByNavigation { get; set; }
public virtual ICollection<WalletCouponBatch>
    WalletCouponBatchCreateByNavigation { get; set; }
public virtual ICollection<WalletCouponBatch>
    WalletCouponBatchModifyByNavigation { get; set; }
public virtual ICollection<WalletCoupon>
    WalletCouponCreateByNavigation { get; set; }
public virtual ICollection<WalletCoupon>
    WalletCouponModifyByNavigation { get; set; }
public virtual ICollection<WalletCoupon>
    WalletCouponUser { get; set; }
public virtual ICollection<WalletProgram>
    WalletProgramCreateByNavigation { get; set; }
public virtual ICollection<WalletProgram>
    WalletProgramModifyByNavigation { get; set; }
public virtual ICollection<WalletReservation>
    WalletReservationCreateByNavigation { get; set; }
public virtual ICollection<WalletReservation>
    WalletReservationModifyByNavigation { get; set; }
public virtual ICollection<WalletReservation>
    WalletReservationUser { get; set; }
public virtual ICollection<WalletScope>
    WalletScopeCreateByNavigation { get; set; }
public virtual ICollection<WalletScope>
    WalletScopeModifyByNavigation { get; set; }
public virtual ICollection<WalletTransaction>
    WalletTransactionCreateByNavigation { get; set; }
public virtual ICollection<WalletTransaction>
    WalletTransactionModifyByNavigation { get; set; }
public virtual ICollection<WalletTransaction>
    WalletTransactionUser { get; set; }
public virtual ICollection<Warehouse>
    WarehouseCreateByNavigation { get; set; }
public virtual ICollection<Warehouse>
    WarehouseModifyByNavigation { get; set; }
public virtual ICollection<YashilCms>
    YashilCmsCreateByNavigation { get; set; }
public virtual ICollection<YashilCms>
    YashilCmsModifyByNavigation { get; set; }
public virtual ICollection<YashilDataProvider>
    YashilDataProviderCreateByNavigation { get; set; }
public virtual ICollection<YashilDataProvider>
    YashilDataProviderModifyByNavigation { get; set; }
public virtual ICollection<YashilDayOfWeek>
    YashilDayOfWeekCreateByNavigation { get; set; }
public virtual ICollection<YashilDayOfWeek>
    YashilDayOfWeekModifyByNavigation { get; set; }
public virtual ICollection<YashilHekmatTerminal>
    YashilHekmatTerminalCreateByNavigation { get; set; }
public virtual ICollection<YashilHekmatTerminal>
    YashilHekmatTerminalModifyByNavigation { get; set; }
public virtual ICollection<YashilHekmatTerminalSupplier>
    YashilHekmatTerminalSupplierCreateByNavigation { get; set; }
public virtual ICollection<YashilHekmatTerminalSupplier>
    YashilHekmatTerminalSupplierModifyByNavigation { get; set; }
public virtual ICollection<YashilHekmatTerminalSupplier>
    YashilHekmatTerminalSupplierUser { get; set; }
public virtual ICollection<YashilScheduler>
    YashilSchedulerCreateByNavigation { get; set; }
public virtual ICollection<YashilScheduler>
    YashilSchedulerModifyByNavigation { get; set; }
    }
 }
