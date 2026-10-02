using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Organization  :IBaseEntity<int>
    {
public Organization()
{
    ActiveStorageAttachment = new HashSet<ActiveStorageAttachment>();
    ActiveStorageBlobs = new HashSet<ActiveStorageBlobs>();
    Activity = new HashSet<Activity>();
    ActivityLog = new HashSet<ActivityLog>();
    AdditionalUserProp = new HashSet<AdditionalUserProp>();
    AdditionalUserPropCompanyGroup = new HashSet<AdditionalUserPropCompanyGroup>();
    Address = new HashSet<Address>();
    Affiliate = new HashSet<Affiliate>();
    AppConfig = new HashSet<AppConfig>();
    AppDocument = new HashSet<AppDocument>();
    AppEntityAttribute = new HashSet<AppEntityAttribute>();
    AppEntityAttributeMapping = new HashSet<AppEntityAttributeMapping>();
    AppEntityAttributeValue = new HashSet<AppEntityAttributeValue>();
    AppEntityCustomerClub = new HashSet<AppEntityCustomerClub>();
    ApplicationCustomerClub = new HashSet<ApplicationCustomerClub>();
    ApplicationPaymentMethod = new HashSet<ApplicationPaymentMethod>();
    ArInternalMetadata = new HashSet<ArInternalMetadata>();
    CardAmountHistory = new HashSet<CardAmountHistory>();
    CardInfo = new HashSet<CardInfo>();
    Category = new HashSet<Category>();
    CategoryCustomerClub = new HashSet<CategoryCustomerClub>();
    CategoryGroup = new HashSet<CategoryGroup>();
    CategorySpecification = new HashSet<CategorySpecification>();
    CitySubsidyFactor = new HashSet<CitySubsidyFactor>();
    ClassRoom = new HashSet<ClassRoom>();
    ClassRoomReservation = new HashSet<ClassRoomReservation>();
    ClassRoomReservationSchedule = new HashSet<ClassRoomReservationSchedule>();
    ClassSession = new HashSet<ClassSession>();
    CommonBaseData = new HashSet<CommonBaseData>();
    CommonBaseType = new HashSet<CommonBaseType>();
    Company = new HashSet<Company>();
    CompanyGroup = new HashSet<CompanyGroup>();
    CompanyMainCategory = new HashSet<CompanyMainCategory>();
    ContentCategory = new HashSet<ContentCategory>();
    Contract = new HashSet<Contract>();
    ContractPaymentMethod = new HashSet<ContractPaymentMethod>();
    Conversation = new HashSet<Conversation>();
    ConversationRecipient = new HashSet<ConversationRecipient>();
    ConversationSeen = new HashSet<ConversationSeen>();
    Corporation = new HashSet<Corporation>();
    CorporationJobCo = new HashSet<CorporationJobCo>();
    Course = new HashSet<Course>();
    CreditOrCashPayment = new HashSet<CreditOrCashPayment>();
    Currency = new HashSet<Currency>();
    CustomerClub = new HashSet<CustomerClub>();
    CustomerClubCard = new HashSet<CustomerClubCard>();
    CustomerClubCardPerson = new HashSet<CustomerClubCardPerson>();
    CustomerClubPaymentInstallment = new HashSet<CustomerClubPaymentInstallment>();
    CustomerClubPaymentMethodType = new HashSet<CustomerClubPaymentMethodType>();
    DashboardGroup = new HashSet<DashboardGroup>();
    DashboardStore = new HashSet<DashboardStore>();
    DependentProductSale = new HashSet<DependentProductSale>();
    DocType = new HashSet<DocType>();
    DocumentCategory = new HashSet<DocumentCategory>();
    Error = new HashSet<Error>();
    Exam = new HashSet<Exam>();
    ExamCategory = new HashSet<ExamCategory>();
    ExamQuestion = new HashSet<ExamQuestion>();
    ExamQuestionAnswer = new HashSet<ExamQuestionAnswer>();
    Factor = new HashSet<Factor>();
    FactorDeduction = new HashSet<FactorDeduction>();
    FactorGroup = new HashSet<FactorGroup>();
    FactorGroupItem = new HashSet<FactorGroupItem>();
    FactorItem = new HashSet<FactorItem>();
    FactorPayment = new HashSet<FactorPayment>();
    FactorPaymentInstallment = new HashSet<FactorPaymentInstallment>();
    FactorVerification = new HashSet<FactorVerification>();
    FactorVerificationConfiguration = new HashSet<FactorVerificationConfiguration>();
    FactorVerificationConfigurationRole = new HashSet<FactorVerificationConfigurationRole>();
    Feature = new HashSet<Feature>();
    FileTransfer = new HashSet<FileTransfer>();
    GiftCardType = new HashSet<GiftCardType>();
    HekmatBlackListPhone = new HashSet<HekmatBlackListPhone>();
    HekmatPaymentGatewayModifiedTransaction = new HashSet<HekmatPaymentGatewayModifiedTransaction>();
    HekmatPaymentGatewayTransaction = new HashSet<HekmatPaymentGatewayTransaction>();
    HekmatPaymentGatewayTransactionBulkRequest = new HashSet<HekmatPaymentGatewayTransactionBulkRequest>();
    HekmatPaymentGatewayTransactionBulkRequestCategory = new HashSet<HekmatPaymentGatewayTransactionBulkRequestCategory>();
    HekmatTerminal = new HashSet<HekmatTerminal>();
    HomeWork = new HashSet<HomeWork>();
    HomeWorkCategory = new HashSet<HomeWorkCategory>();
    HybridPayment = new HashSet<HybridPayment>();
    HybridPaymentRequest = new HashSet<HybridPaymentRequest>();
    Installment = new HashSet<Installment>();
    InverseParent = new HashSet<Organization>();
    Invitation = new HashSet<Invitation>();
    Issue = new HashSet<Issue>();
    IssueComment = new HashSet<IssueComment>();
    IssueLogWork = new HashSet<IssueLogWork>();
    IssueParticipant = new HashSet<IssueParticipant>();
    IssueTypeItem = new HashSet<IssueTypeItem>();
    ItemGroup = new HashSet<ItemGroup>();
    Job = new HashSet<Job>();
    JobCo = new HashSet<JobCo>();
    JobCoCategory = new HashSet<JobCoCategory>();
    JobCoSkill = new HashSet<JobCoSkill>();
    JobCoTags = new HashSet<JobCoTags>();
    JobCoUser = new HashSet<JobCoUser>();
    Keyword = new HashSet<Keyword>();
    Language = new HashSet<Language>();
    Loan = new HashSet<Loan>();
    LoanGuarantor = new HashSet<LoanGuarantor>();
    LocaleStringResource = new HashSet<LocaleStringResource>();
    MainNews = new HashSet<MainNews>();
    MediaCategory = new HashSet<MediaCategory>();
    MediaComment = new HashSet<MediaComment>();
    MediaKeyMoment = new HashSet<MediaKeyMoment>();
    MediaManufacturer = new HashSet<MediaManufacturer>();
    MediaSession = new HashSet<MediaSession>();
    MediaSessionViewingHistory = new HashSet<MediaSessionViewingHistory>();
    MerchantInfo = new HashSet<MerchantInfo>();
    MerchantTerminalInfo = new HashSet<MerchantTerminalInfo>();
    ModifiedOrderItem = new HashSet<ModifiedOrderItem>();
    NewsKeyword = new HashSet<NewsKeyword>();
    NewsStore = new HashSet<NewsStore>();
    Order = new HashSet<Order>();
    OrderChangePermission = new HashSet<OrderChangePermission>();
    OrderItem = new HashSet<OrderItem>();
    OrderItemDependentProductSale = new HashSet<OrderItemDependentProductSale>();
    OrderItemDetail = new HashSet<OrderItemDetail>();
    OrderItemPayment = new HashSet<OrderItemPayment>();
    OrderNote = new HashSet<OrderNote>();
    OrderTrakking = new HashSet<OrderTrakking>();
    OrganizationVirtualClassRoomConfigCreatorOrganization = new HashSet<OrganizationVirtualClassRoomConfig>();
    PasswordHistory = new HashSet<PasswordHistory>();
    Payment = new HashSet<Payment>();
    PaymentGateway = new HashSet<PaymentGateway>();
    PaymentGatewayPaymentMethod = new HashSet<PaymentGatewayPaymentMethod>();
    PaymentTransactionItemRefund = new HashSet<PaymentTransactionItemRefund>();
    Person = new HashSet<Person>();
    PersonBankAccount = new HashSet<PersonBankAccount>();
    PersonEconomicProfile = new HashSet<PersonEconomicProfile>();
    PersonEducation = new HashSet<PersonEducation>();
    PersonPost = new HashSet<PersonPost>();
    Post = new HashSet<Post>();
    PreRegistration = new HashSet<PreRegistration>();
    Product = new HashSet<Product>();
    ProductCategory = new HashSet<ProductCategory>();
    ProductGroup = new HashSet<ProductGroup>();
    ProductSalesPlanningConversation = new HashSet<ProductSalesPlanningConversation>();
    ProductSalesPlanningConversationMessage = new HashSet<ProductSalesPlanningConversationMessage>();
    ProductSalesPlanningDetail = new HashSet<ProductSalesPlanningDetail>();
    ProductSalesPlanningDiscount = new HashSet<ProductSalesPlanningDiscount>();
    ProductSalesPlanningItemGroup = new HashSet<ProductSalesPlanningItemGroup>();
    ProductSalesPlanningReview = new HashSet<ProductSalesPlanningReview>();
    ProductSalesPlanningStudent = new HashSet<ProductSalesPlanningStudent>();
    ProductSalesPlanningTeacher = new HashSet<ProductSalesPlanningTeacher>();
    ProductSpecificationOption = new HashSet<ProductSpecificationOption>();
    ProductTags = new HashSet<ProductTags>();
    ProductTemplate = new HashSet<ProductTemplate>();
    ProductType = new HashSet<ProductType>();
    ProductTypeSpecification = new HashSet<ProductTypeSpecification>();
    Project = new HashSet<Project>();
    Question = new HashSet<Question>();
    QuestionCategory = new HashSet<QuestionCategory>();
    QuestionOption = new HashSet<QuestionOption>();
    Recipient = new HashSet<Recipient>();
    RecipientPerson = new HashSet<RecipientPerson>();
    Refund = new HashSet<Refund>();
    RefundDetail = new HashSet<RefundDetail>();
    RefundPaymentTransactionItemRequest = new HashSet<RefundPaymentTransactionItemRequest>();
    ReportGroup = new HashSet<ReportGroup>();
    ReportStore = new HashSet<ReportStore>();
    ReportStoreApplication = new HashSet<ReportStoreApplication>();
    Representation = new HashSet<Representation>();
    RepresentationCategory = new HashSet<RepresentationCategory>();
    RepresentationEstablishedLicenseType = new HashSet<RepresentationEstablishedLicenseType>();
    RepresentationPerson = new HashSet<RepresentationPerson>();
    RepresentationTeacher = new HashSet<RepresentationTeacher>();
    ResourceAttribute = new HashSet<ResourceAttribute>();
    ResourceAttributeRole = new HashSet<ResourceAttributeRole>();
    RewardPointsHistory = new HashSet<RewardPointsHistory>();
    RollCall = new HashSet<RollCall>();
    SaleConfiguration = new HashSet<SaleConfiguration>();
    SaleConfigurationAttribute = new HashSet<SaleConfigurationAttribute>();
    Service = new HashSet<Service>();
    SiteVisit = new HashSet<SiteVisit>();
    Skill = new HashSet<Skill>();
    SkyRoomUser = new HashSet<SkyRoomUser>();
    Specification = new HashSet<Specification>();
    SpecificationOption = new HashSet<SpecificationOption>();
    StructureInfo = new HashSet<StructureInfo>();
    Subsidy = new HashSet<Subsidy>();
    SubsidyGroup = new HashSet<SubsidyGroup>();
    SurveyOption = new HashSet<SurveyOption>();
    TaxCategory = new HashSet<TaxCategory>();
    TaxCategorySaleConfiguration = new HashSet<TaxCategorySaleConfiguration>();
    Unit = new HashSet<Unit>();
    UsedSubsidy = new HashSet<UsedSubsidy>();
    UserApplication = new HashSet<UserApplication>();
    UserCreatorOrganization = new HashSet<User>();
    UserExam = new HashSet<UserExam>();
    UserGroup = new HashSet<UserGroup>();
    UserGroupMapping = new HashSet<UserGroupMapping>();
    UserHomeWork = new HashSet<UserHomeWork>();
    UserOrganization = new HashSet<User>();
    UserValidIpAddress = new HashSet<UserValidIpAddress>();
    ValidHekmatCard = new HashSet<ValidHekmatCard>();
    VersionInfo = new HashSet<VersionInfo>();
    VirtualClassRecordingDetail = new HashSet<VirtualClassRecordingDetail>();
    VirtualClassRoomConfig = new HashSet<VirtualClassRoomConfig>();
    VirtualClassStudent = new HashSet<VirtualClassStudent>();
    VirtualClassTeacher = new HashSet<VirtualClassTeacher>();
    Vocher = new HashSet<Vocher>();
    VocherType = new HashSet<VocherType>();
    WalletCardGroup = new HashSet<WalletCardGroup>();
    WalletCardMember = new HashSet<WalletCardMember>();
    WalletCoupon = new HashSet<WalletCoupon>();
    WalletCouponBatch = new HashSet<WalletCouponBatch>();
    WalletProgram = new HashSet<WalletProgram>();
    WalletReservation = new HashSet<WalletReservation>();
    WalletScope = new HashSet<WalletScope>();
    WalletTransaction = new HashSet<WalletTransaction>();
    Warehouse = new HashSet<Warehouse>();
    YashilCms = new HashSet<YashilCms>();
    YashilConnectionString = new HashSet<YashilConnectionString>();
    YashilHekmatTerminal = new HashSet<YashilHekmatTerminal>();
    YashilHekmatTerminalSupplier = new HashSet<YashilHekmatTerminalSupplier>();
    YashilScheduler = new HashSet<YashilScheduler>();
 }

/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// فعال
///</summary>
public bool IsActive { get; set; }
/// <summary>
///         /// سازمان پدر
///</summary>
public int? ParentId { get; set; }
public long? UniqueId { get; set; }
public string CodePath { get; set; }
/// <summary>
///         /// نوع سازمان
///</summary>
public int? Type { get; set; }
/// <summary>
///         /// استان
///</summary>
public int? ProvinceId { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
public int CreatorOrganizationId { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Organization Parent { get; set; }
    public virtual OrganizationVirtualClassRoomConfig OrganizationVirtualClassRoomConfigOrganization { get; set; }
public virtual ICollection<ActiveStorageAttachment>
    ActiveStorageAttachment { get; set; }
public virtual ICollection<ActiveStorageBlobs>
    ActiveStorageBlobs { get; set; }
public virtual ICollection<Activity>
    Activity { get; set; }
public virtual ICollection<ActivityLog>
    ActivityLog { get; set; }
public virtual ICollection<AdditionalUserProp>
    AdditionalUserProp { get; set; }
public virtual ICollection<AdditionalUserPropCompanyGroup>
    AdditionalUserPropCompanyGroup { get; set; }
public virtual ICollection<Address>
    Address { get; set; }
public virtual ICollection<Affiliate>
    Affiliate { get; set; }
public virtual ICollection<AppConfig>
    AppConfig { get; set; }
public virtual ICollection<AppDocument>
    AppDocument { get; set; }
public virtual ICollection<AppEntityAttribute>
    AppEntityAttribute { get; set; }
public virtual ICollection<AppEntityAttributeMapping>
    AppEntityAttributeMapping { get; set; }
public virtual ICollection<AppEntityAttributeValue>
    AppEntityAttributeValue { get; set; }
public virtual ICollection<AppEntityCustomerClub>
    AppEntityCustomerClub { get; set; }
public virtual ICollection<ApplicationCustomerClub>
    ApplicationCustomerClub { get; set; }
public virtual ICollection<ApplicationPaymentMethod>
    ApplicationPaymentMethod { get; set; }
public virtual ICollection<ArInternalMetadata>
    ArInternalMetadata { get; set; }
public virtual ICollection<CardAmountHistory>
    CardAmountHistory { get; set; }
public virtual ICollection<CardInfo>
    CardInfo { get; set; }
public virtual ICollection<Category>
    Category { get; set; }
public virtual ICollection<CategoryCustomerClub>
    CategoryCustomerClub { get; set; }
public virtual ICollection<CategoryGroup>
    CategoryGroup { get; set; }
public virtual ICollection<CategorySpecification>
    CategorySpecification { get; set; }
public virtual ICollection<CitySubsidyFactor>
    CitySubsidyFactor { get; set; }
public virtual ICollection<ClassRoom>
    ClassRoom { get; set; }
public virtual ICollection<ClassRoomReservation>
    ClassRoomReservation { get; set; }
public virtual ICollection<ClassRoomReservationSchedule>
    ClassRoomReservationSchedule { get; set; }
public virtual ICollection<ClassSession>
    ClassSession { get; set; }
public virtual ICollection<CommonBaseData>
    CommonBaseData { get; set; }
public virtual ICollection<CommonBaseType>
    CommonBaseType { get; set; }
public virtual ICollection<Company>
    Company { get; set; }
public virtual ICollection<CompanyGroup>
    CompanyGroup { get; set; }
public virtual ICollection<CompanyMainCategory>
    CompanyMainCategory { get; set; }
public virtual ICollection<ContentCategory>
    ContentCategory { get; set; }
public virtual ICollection<Contract>
    Contract { get; set; }
public virtual ICollection<ContractPaymentMethod>
    ContractPaymentMethod { get; set; }
public virtual ICollection<Conversation>
    Conversation { get; set; }
public virtual ICollection<ConversationRecipient>
    ConversationRecipient { get; set; }
public virtual ICollection<ConversationSeen>
    ConversationSeen { get; set; }
public virtual ICollection<Corporation>
    Corporation { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCo { get; set; }
public virtual ICollection<Course>
    Course { get; set; }
public virtual ICollection<CreditOrCashPayment>
    CreditOrCashPayment { get; set; }
public virtual ICollection<Currency>
    Currency { get; set; }
public virtual ICollection<CustomerClub>
    CustomerClub { get; set; }
public virtual ICollection<CustomerClubCard>
    CustomerClubCard { get; set; }
public virtual ICollection<CustomerClubCardPerson>
    CustomerClubCardPerson { get; set; }
public virtual ICollection<CustomerClubPaymentInstallment>
    CustomerClubPaymentInstallment { get; set; }
public virtual ICollection<CustomerClubPaymentMethodType>
    CustomerClubPaymentMethodType { get; set; }
public virtual ICollection<DashboardGroup>
    DashboardGroup { get; set; }
public virtual ICollection<DashboardStore>
    DashboardStore { get; set; }
public virtual ICollection<DependentProductSale>
    DependentProductSale { get; set; }
public virtual ICollection<DocType>
    DocType { get; set; }
public virtual ICollection<DocumentCategory>
    DocumentCategory { get; set; }
public virtual ICollection<Error>
    Error { get; set; }
public virtual ICollection<Exam>
    Exam { get; set; }
public virtual ICollection<ExamCategory>
    ExamCategory { get; set; }
public virtual ICollection<ExamQuestion>
    ExamQuestion { get; set; }
public virtual ICollection<ExamQuestionAnswer>
    ExamQuestionAnswer { get; set; }
public virtual ICollection<Factor>
    Factor { get; set; }
public virtual ICollection<FactorDeduction>
    FactorDeduction { get; set; }
public virtual ICollection<FactorGroup>
    FactorGroup { get; set; }
public virtual ICollection<FactorGroupItem>
    FactorGroupItem { get; set; }
public virtual ICollection<FactorItem>
    FactorItem { get; set; }
public virtual ICollection<FactorPayment>
    FactorPayment { get; set; }
public virtual ICollection<FactorPaymentInstallment>
    FactorPaymentInstallment { get; set; }
public virtual ICollection<FactorVerification>
    FactorVerification { get; set; }
public virtual ICollection<FactorVerificationConfiguration>
    FactorVerificationConfiguration { get; set; }
public virtual ICollection<FactorVerificationConfigurationRole>
    FactorVerificationConfigurationRole { get; set; }
public virtual ICollection<Feature>
    Feature { get; set; }
public virtual ICollection<FileTransfer>
    FileTransfer { get; set; }
public virtual ICollection<GiftCardType>
    GiftCardType { get; set; }
public virtual ICollection<HekmatBlackListPhone>
    HekmatBlackListPhone { get; set; }
public virtual ICollection<HekmatPaymentGatewayModifiedTransaction>
    HekmatPaymentGatewayModifiedTransaction { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransaction>
    HekmatPaymentGatewayTransaction { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequest>
    HekmatPaymentGatewayTransactionBulkRequest { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequestCategory>
    HekmatPaymentGatewayTransactionBulkRequestCategory { get; set; }
public virtual ICollection<HekmatTerminal>
    HekmatTerminal { get; set; }
public virtual ICollection<HomeWork>
    HomeWork { get; set; }
public virtual ICollection<HomeWorkCategory>
    HomeWorkCategory { get; set; }
public virtual ICollection<HybridPayment>
    HybridPayment { get; set; }
public virtual ICollection<HybridPaymentRequest>
    HybridPaymentRequest { get; set; }
public virtual ICollection<Installment>
    Installment { get; set; }
public virtual ICollection<Organization>
    InverseParent { get; set; }
public virtual ICollection<Invitation>
    Invitation { get; set; }
public virtual ICollection<Issue>
    Issue { get; set; }
public virtual ICollection<IssueComment>
    IssueComment { get; set; }
public virtual ICollection<IssueLogWork>
    IssueLogWork { get; set; }
public virtual ICollection<IssueParticipant>
    IssueParticipant { get; set; }
public virtual ICollection<IssueTypeItem>
    IssueTypeItem { get; set; }
public virtual ICollection<ItemGroup>
    ItemGroup { get; set; }
public virtual ICollection<Job>
    Job { get; set; }
public virtual ICollection<JobCo>
    JobCo { get; set; }
public virtual ICollection<JobCoCategory>
    JobCoCategory { get; set; }
public virtual ICollection<JobCoSkill>
    JobCoSkill { get; set; }
public virtual ICollection<JobCoTags>
    JobCoTags { get; set; }
public virtual ICollection<JobCoUser>
    JobCoUser { get; set; }
public virtual ICollection<Keyword>
    Keyword { get; set; }
public virtual ICollection<Language>
    Language { get; set; }
public virtual ICollection<Loan>
    Loan { get; set; }
public virtual ICollection<LoanGuarantor>
    LoanGuarantor { get; set; }
public virtual ICollection<LocaleStringResource>
    LocaleStringResource { get; set; }
public virtual ICollection<MainNews>
    MainNews { get; set; }
public virtual ICollection<MediaCategory>
    MediaCategory { get; set; }
public virtual ICollection<MediaComment>
    MediaComment { get; set; }
public virtual ICollection<MediaKeyMoment>
    MediaKeyMoment { get; set; }
public virtual ICollection<MediaManufacturer>
    MediaManufacturer { get; set; }
public virtual ICollection<MediaSession>
    MediaSession { get; set; }
public virtual ICollection<MediaSessionViewingHistory>
    MediaSessionViewingHistory { get; set; }
public virtual ICollection<MerchantInfo>
    MerchantInfo { get; set; }
public virtual ICollection<MerchantTerminalInfo>
    MerchantTerminalInfo { get; set; }
public virtual ICollection<ModifiedOrderItem>
    ModifiedOrderItem { get; set; }
public virtual ICollection<NewsKeyword>
    NewsKeyword { get; set; }
public virtual ICollection<NewsStore>
    NewsStore { get; set; }
public virtual ICollection<Order>
    Order { get; set; }
public virtual ICollection<OrderChangePermission>
    OrderChangePermission { get; set; }
public virtual ICollection<OrderItem>
    OrderItem { get; set; }
public virtual ICollection<OrderItemDependentProductSale>
    OrderItemDependentProductSale { get; set; }
public virtual ICollection<OrderItemDetail>
    OrderItemDetail { get; set; }
public virtual ICollection<OrderItemPayment>
    OrderItemPayment { get; set; }
public virtual ICollection<OrderNote>
    OrderNote { get; set; }
public virtual ICollection<OrderTrakking>
    OrderTrakking { get; set; }
public virtual ICollection<OrganizationVirtualClassRoomConfig>
    OrganizationVirtualClassRoomConfigCreatorOrganization { get; set; }
public virtual ICollection<PasswordHistory>
    PasswordHistory { get; set; }
public virtual ICollection<Payment>
    Payment { get; set; }
public virtual ICollection<PaymentGateway>
    PaymentGateway { get; set; }
public virtual ICollection<PaymentGatewayPaymentMethod>
    PaymentGatewayPaymentMethod { get; set; }
public virtual ICollection<PaymentTransactionItemRefund>
    PaymentTransactionItemRefund { get; set; }
public virtual ICollection<Person>
    Person { get; set; }
public virtual ICollection<PersonBankAccount>
    PersonBankAccount { get; set; }
public virtual ICollection<PersonEconomicProfile>
    PersonEconomicProfile { get; set; }
public virtual ICollection<PersonEducation>
    PersonEducation { get; set; }
public virtual ICollection<PersonPost>
    PersonPost { get; set; }
public virtual ICollection<Post>
    Post { get; set; }
public virtual ICollection<PreRegistration>
    PreRegistration { get; set; }
public virtual ICollection<Product>
    Product { get; set; }
public virtual ICollection<ProductCategory>
    ProductCategory { get; set; }
public virtual ICollection<ProductGroup>
    ProductGroup { get; set; }
public virtual ICollection<ProductSalesPlanningConversation>
    ProductSalesPlanningConversation { get; set; }
public virtual ICollection<ProductSalesPlanningConversationMessage>
    ProductSalesPlanningConversationMessage { get; set; }
public virtual ICollection<ProductSalesPlanningDetail>
    ProductSalesPlanningDetail { get; set; }
public virtual ICollection<ProductSalesPlanningDiscount>
    ProductSalesPlanningDiscount { get; set; }
public virtual ICollection<ProductSalesPlanningItemGroup>
    ProductSalesPlanningItemGroup { get; set; }
public virtual ICollection<ProductSalesPlanningReview>
    ProductSalesPlanningReview { get; set; }
public virtual ICollection<ProductSalesPlanningStudent>
    ProductSalesPlanningStudent { get; set; }
public virtual ICollection<ProductSalesPlanningTeacher>
    ProductSalesPlanningTeacher { get; set; }
public virtual ICollection<ProductSpecificationOption>
    ProductSpecificationOption { get; set; }
public virtual ICollection<ProductTags>
    ProductTags { get; set; }
public virtual ICollection<ProductTemplate>
    ProductTemplate { get; set; }
public virtual ICollection<ProductType>
    ProductType { get; set; }
public virtual ICollection<ProductTypeSpecification>
    ProductTypeSpecification { get; set; }
public virtual ICollection<Project>
    Project { get; set; }
public virtual ICollection<Question>
    Question { get; set; }
public virtual ICollection<QuestionCategory>
    QuestionCategory { get; set; }
public virtual ICollection<QuestionOption>
    QuestionOption { get; set; }
public virtual ICollection<Recipient>
    Recipient { get; set; }
public virtual ICollection<RecipientPerson>
    RecipientPerson { get; set; }
public virtual ICollection<Refund>
    Refund { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetail { get; set; }
public virtual ICollection<RefundPaymentTransactionItemRequest>
    RefundPaymentTransactionItemRequest { get; set; }
public virtual ICollection<ReportGroup>
    ReportGroup { get; set; }
public virtual ICollection<ReportStore>
    ReportStore { get; set; }
public virtual ICollection<ReportStoreApplication>
    ReportStoreApplication { get; set; }
public virtual ICollection<Representation>
    Representation { get; set; }
public virtual ICollection<RepresentationCategory>
    RepresentationCategory { get; set; }
public virtual ICollection<RepresentationEstablishedLicenseType>
    RepresentationEstablishedLicenseType { get; set; }
public virtual ICollection<RepresentationPerson>
    RepresentationPerson { get; set; }
public virtual ICollection<RepresentationTeacher>
    RepresentationTeacher { get; set; }
public virtual ICollection<ResourceAttribute>
    ResourceAttribute { get; set; }
public virtual ICollection<ResourceAttributeRole>
    ResourceAttributeRole { get; set; }
public virtual ICollection<RewardPointsHistory>
    RewardPointsHistory { get; set; }
public virtual ICollection<RollCall>
    RollCall { get; set; }
public virtual ICollection<SaleConfiguration>
    SaleConfiguration { get; set; }
public virtual ICollection<SaleConfigurationAttribute>
    SaleConfigurationAttribute { get; set; }
public virtual ICollection<Service>
    Service { get; set; }
public virtual ICollection<SiteVisit>
    SiteVisit { get; set; }
public virtual ICollection<Skill>
    Skill { get; set; }
public virtual ICollection<SkyRoomUser>
    SkyRoomUser { get; set; }
public virtual ICollection<Specification>
    Specification { get; set; }
public virtual ICollection<SpecificationOption>
    SpecificationOption { get; set; }
public virtual ICollection<StructureInfo>
    StructureInfo { get; set; }
public virtual ICollection<Subsidy>
    Subsidy { get; set; }
public virtual ICollection<SubsidyGroup>
    SubsidyGroup { get; set; }
public virtual ICollection<SurveyOption>
    SurveyOption { get; set; }
public virtual ICollection<TaxCategory>
    TaxCategory { get; set; }
public virtual ICollection<TaxCategorySaleConfiguration>
    TaxCategorySaleConfiguration { get; set; }
public virtual ICollection<Unit>
    Unit { get; set; }
public virtual ICollection<UsedSubsidy>
    UsedSubsidy { get; set; }
public virtual ICollection<UserApplication>
    UserApplication { get; set; }
public virtual ICollection<User>
    UserCreatorOrganization { get; set; }
public virtual ICollection<UserExam>
    UserExam { get; set; }
public virtual ICollection<UserGroup>
    UserGroup { get; set; }
public virtual ICollection<UserGroupMapping>
    UserGroupMapping { get; set; }
public virtual ICollection<UserHomeWork>
    UserHomeWork { get; set; }
public virtual ICollection<User>
    UserOrganization { get; set; }
public virtual ICollection<UserValidIpAddress>
    UserValidIpAddress { get; set; }
public virtual ICollection<ValidHekmatCard>
    ValidHekmatCard { get; set; }
public virtual ICollection<VersionInfo>
    VersionInfo { get; set; }
public virtual ICollection<VirtualClassRecordingDetail>
    VirtualClassRecordingDetail { get; set; }
public virtual ICollection<VirtualClassRoomConfig>
    VirtualClassRoomConfig { get; set; }
public virtual ICollection<VirtualClassStudent>
    VirtualClassStudent { get; set; }
public virtual ICollection<VirtualClassTeacher>
    VirtualClassTeacher { get; set; }
public virtual ICollection<Vocher>
    Vocher { get; set; }
public virtual ICollection<VocherType>
    VocherType { get; set; }
public virtual ICollection<WalletCardGroup>
    WalletCardGroup { get; set; }
public virtual ICollection<WalletCardMember>
    WalletCardMember { get; set; }
public virtual ICollection<WalletCoupon>
    WalletCoupon { get; set; }
public virtual ICollection<WalletCouponBatch>
    WalletCouponBatch { get; set; }
public virtual ICollection<WalletProgram>
    WalletProgram { get; set; }
public virtual ICollection<WalletReservation>
    WalletReservation { get; set; }
public virtual ICollection<WalletScope>
    WalletScope { get; set; }
public virtual ICollection<WalletTransaction>
    WalletTransaction { get; set; }
public virtual ICollection<Warehouse>
    Warehouse { get; set; }
public virtual ICollection<YashilCms>
    YashilCms { get; set; }
public virtual ICollection<YashilConnectionString>
    YashilConnectionString { get; set; }
public virtual ICollection<YashilHekmatTerminal>
    YashilHekmatTerminal { get; set; }
public virtual ICollection<YashilHekmatTerminalSupplier>
    YashilHekmatTerminalSupplier { get; set; }
public virtual ICollection<YashilScheduler>
    YashilScheduler { get; set; }
    }
 }
