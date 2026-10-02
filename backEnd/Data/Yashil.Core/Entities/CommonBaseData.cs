using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CommonBaseData  :IBaseEntity<int>
    {
public CommonBaseData()
{
    AppEntityAttributeMapping = new HashSet<AppEntityAttributeMapping>();
    Application = new HashSet<Application>();
    ApplicationPaymentMethod = new HashSet<ApplicationPaymentMethod>();
    CardInfo = new HashSet<CardInfo>();
    CityCountryDivisionTypeNavigation = new HashSet<City>();
    CityCustomCategoryNavigation = new HashSet<City>();
    ClassRoomReservationClassRoomReservationRequestTypeNavigation = new HashSet<ClassRoomReservation>();
    ClassRoomReservationClassRoomReservationStatusTypeNavigation = new HashSet<ClassRoomReservation>();
    ClassRoomReservationSchedule = new HashSet<ClassRoomReservationSchedule>();
    Company = new HashSet<Company>();
    ContractContractStatusNavigation = new HashSet<Contract>();
    ContractContractTypeNavigation = new HashSet<Contract>();
    ContractPaymentMethod = new HashSet<ContractPaymentMethod>();
    ConversationConversationStatusTypeNavigation = new HashSet<Conversation>();
    ConversationConversationTypeNavigation = new HashSet<Conversation>();
    CorporationJobCoCooperationTypeNavigation = new HashSet<CorporationJobCo>();
    CorporationJobCoCustomGenderTypeNavigation = new HashSet<CorporationJobCo>();
    CorporationJobCoEducationGradeTypeNavigation = new HashSet<CorporationJobCo>();
    CorporationJobCoMilitaryServiceStatusTypeNavigation = new HashSet<CorporationJobCo>();
    CorporationJobCoSalaryTypeNavigation = new HashSet<CorporationJobCo>();
    CourseCertificateTypeNavigation = new HashSet<Course>();
    CourseEvaluationMethodNavigation = new HashSet<Course>();
    CourseSkillTypeNavigation = new HashSet<Course>();
    CreditOrCashPayment = new HashSet<CreditOrCashPayment>();
    CustomerClub = new HashSet<CustomerClub>();
    CustomerClubPaymentMethodType = new HashSet<CustomerClubPaymentMethodType>();
    Error = new HashSet<Error>();
    ExamExamDisplayResultTypeNavigation = new HashSet<Exam>();
    ExamExamTypeNavigation = new HashSet<Exam>();
    FactorFactorReportTypeNavigation = new HashSet<Factor>();
    FactorFactorReviewStatusTypeNavigation = new HashSet<Factor>();
    FactorGroup = new HashSet<FactorGroup>();
    FactorPaymentInstallment = new HashSet<FactorPaymentInstallment>();
    FactorVerification = new HashSet<FactorVerification>();
    HybridPaymentHybridPaymentStatusTypeNavigation = new HashSet<HybridPayment>();
    HybridPaymentPaymentMethodTypeNavigation = new HashSet<HybridPayment>();
    HybridPaymentRequest = new HashSet<HybridPaymentRequest>();
    HybridPaymentSaleTypeNavigation = new HashSet<HybridPayment>();
    Installment = new HashSet<Installment>();
    InverseParent = new HashSet<CommonBaseData>();
    IssueIssueStateTypeNavigation = new HashSet<Issue>();
    IssueIssueTypeNavigation = new HashSet<Issue>();
    IssuePriorityTypeNavigation = new HashSet<Issue>();
    IssueResolutionTypeNavigation = new HashSet<Issue>();
    IssueTypeItem = new HashSet<IssueTypeItem>();
    JobCoTags = new HashSet<JobCoTags>();
    Keyword = new HashSet<Keyword>();
    LoanBankTypeNavigation = new HashSet<Loan>();
    LoanLoanStatusNavigation = new HashSet<Loan>();
    MediaSession = new HashSet<MediaSession>();
    Message = new HashSet<Message>();
    ModifiedOrderItemModifiedOrderItemStatusTypeNavigation = new HashSet<ModifiedOrderItem>();
    ModifiedOrderItemModifiedOrderItemTypeNavigation = new HashSet<ModifiedOrderItem>();
    NewsStoreLanguageNavigation = new HashSet<NewsStore>();
    NewsStoreNewsTypeNavigation = new HashSet<NewsStore>();
    OrderMembershipTypeNavigation = new HashSet<Order>();
    OrderOrderStatusTypeNavigation = new HashSet<Order>();
    OrderRecipientGenderTypeNavigation = new HashSet<Order>();
    OrderRecipientRelationshipWithBuyer = new HashSet<Order>();
    OrderShippingStatusTypeNavigation = new HashSet<Order>();
    OrderSubsidyStatusTypeNavigation = new HashSet<Order>();
    OrderSubsidyTypeNavigation = new HashSet<Order>();
    PaymentCardTypeNavigation = new HashSet<Payment>();
    PaymentGatewayPaymentMethod = new HashSet<PaymentGatewayPaymentMethod>();
    PaymentPaymentMethodTypeNavigation = new HashSet<Payment>();
    PaymentPaymentStatusTypeNavigation = new HashSet<Payment>();
    PaymentSaleTypeNavigation = new HashSet<Payment>();
    PersonBankAccountBankTypeNavigation = new HashSet<PersonBankAccount>();
    PersonBankAccountDepositStatusNavigation = new HashSet<PersonBankAccount>();
    PersonEducationGradeNavigation = new HashSet<Person>();
    PersonGenderNavigation = new HashSet<Person>();
    ProductBasePriceBaseUnitTypeNavigation = new HashSet<Product>();
    ProductBasePriceUnitTypeNavigation = new HashSet<Product>();
    ProductDownloadActivationTypeNavigation = new HashSet<Product>();
    ProductProductTypeNavigation = new HashSet<Product>();
    ProductRecurringCyclePeriodTypeNavigation = new HashSet<Product>();
    ProductSalesModelTypeNavigation = new HashSet<Product>();
    ProductSalesPlanningAgeCategoryNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningConversation = new HashSet<ProductSalesPlanningConversation>();
    ProductSalesPlanningConversationMessage = new HashSet<ProductSalesPlanningConversationMessage>();
    ProductSalesPlanningCourseLevelNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningCourseStatusNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningCourseTypeNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningCustomGenderNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningDeliveryDateTypeNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningImplementationTypeNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningManageInventoryMethodTypeNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningProductAvailabilityRangeTypeNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningRunTypeNavigation = new HashSet<ProductSalesPlanning>();
    ProductSalesPlanningStudent = new HashSet<ProductSalesPlanningStudent>();
    ProductSalesPlanningTeacher = new HashSet<ProductSalesPlanningTeacher>();
    ProductSpecificationOption = new HashSet<ProductSpecificationOption>();
    ProductTags = new HashSet<ProductTags>();
    QuestionQuestionContentTypeNavigation = new HashSet<Question>();
    QuestionQuestionDifficultyDegreeTypeNavigation = new HashSet<Question>();
    QuestionQuestionOptionContentTypeNavigation = new HashSet<Question>();
    QuestionQuestionTypeNavigation = new HashSet<Question>();
    RecipientGenderTypeNavigation = new HashSet<Recipient>();
    RecipientMembershipTypeNavigation = new HashSet<Recipient>();
    RecipientRelationshipWithBuyerTypeNavigation = new HashSet<Recipient>();
    RecipientSubsidyStatusTypeNavigation = new HashSet<Recipient>();
    RecipientSubsidyTypeNavigation = new HashSet<Recipient>();
    RefundDetailRefundStatusTypeNavigation = new HashSet<RefundDetail>();
    RefundDetailRefundTypeNavigation = new HashSet<RefundDetail>();
    RefundPaymentTransactionItemRequestRefundPaymentTransactionItemRequestStatusNavigation = new HashSet<RefundPaymentTransactionItemRequest>();
    RefundPaymentTransactionItemRequestRefundPaymentTransactionItemRequestTypeNavigation = new HashSet<RefundPaymentTransactionItemRequest>();
    RepresentationEstablishedLicenseType = new HashSet<RepresentationEstablishedLicenseType>();
    RepresentationLicenseTypeNavigation = new HashSet<Representation>();
    RepresentationOwnershipTypeNavigation = new HashSet<Representation>();
    RepresentationTeacher = new HashSet<RepresentationTeacher>();
    SaleConfiguration = new HashSet<SaleConfiguration>();
    SaleConfigurationAttributeDiscountLimitationTypeNavigation = new HashSet<SaleConfigurationAttribute>();
    SaleConfigurationAttributeDiscountTypeNavigation = new HashSet<SaleConfigurationAttribute>();
    SaleConfigurationAttributeSaleTypeNavigation = new HashSet<SaleConfigurationAttribute>();
    ShoppingCartItem = new HashSet<ShoppingCartItem>();
    TaxCategorySaleConfiguration = new HashSet<TaxCategorySaleConfiguration>();
    Unit = new HashSet<Unit>();
    UsedSubsidy = new HashSet<UsedSubsidy>();
    UserExamExamResultTypeNavigation = new HashSet<UserExam>();
    UserExamExamStatusTypeNavigation = new HashSet<UserExam>();
    UserHomeWork = new HashSet<UserHomeWork>();
    ValidHekmatCard = new HashSet<ValidHekmatCard>();
    VirtualClass = new HashSet<VirtualClass>();
    VirtualClassRoomConfig = new HashSet<VirtualClassRoomConfig>();
    WalletProgramWalletProgramAllocationMethodTypeNavigation = new HashSet<WalletProgram>();
    WalletProgramWalletProgramStatusTypeNavigation = new HashSet<WalletProgram>();
    WalletProgramWalletProgramTypeNavigation = new HashSet<WalletProgram>();
    WalletScope = new HashSet<WalletScope>();
    YashilCms = new HashSet<YashilCms>();
    YashilScheduler = new HashSet<YashilScheduler>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// مقدار
///</summary>
public int Value { get; set; }
public string ExtendedProps { get; set; }
/// <summary>
///         /// نوع اطلاعات پايه
///</summary>
public int CommonBaseTypeId { get; set; }
/// <summary>
///         /// سیستمی
///</summary>
public bool IsSystemProp { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
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
///         /// سطح دسترسی
///</summary>
public int AccessLevelId { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }
public int? DisplayOrder { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual CommonBaseType CommonBaseType { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData Parent { get; set; }
public virtual ICollection<AppEntityAttributeMapping>
    AppEntityAttributeMapping { get; set; }
public virtual ICollection<Application>
    Application { get; set; }
public virtual ICollection<ApplicationPaymentMethod>
    ApplicationPaymentMethod { get; set; }
public virtual ICollection<CardInfo>
    CardInfo { get; set; }
public virtual ICollection<City>
    CityCountryDivisionTypeNavigation { get; set; }
public virtual ICollection<City>
    CityCustomCategoryNavigation { get; set; }
public virtual ICollection<ClassRoomReservation>
    ClassRoomReservationClassRoomReservationRequestTypeNavigation { get; set; }
public virtual ICollection<ClassRoomReservation>
    ClassRoomReservationClassRoomReservationStatusTypeNavigation { get; set; }
public virtual ICollection<ClassRoomReservationSchedule>
    ClassRoomReservationSchedule { get; set; }
public virtual ICollection<Company>
    Company { get; set; }
public virtual ICollection<Contract>
    ContractContractStatusNavigation { get; set; }
public virtual ICollection<Contract>
    ContractContractTypeNavigation { get; set; }
public virtual ICollection<ContractPaymentMethod>
    ContractPaymentMethod { get; set; }
public virtual ICollection<Conversation>
    ConversationConversationStatusTypeNavigation { get; set; }
public virtual ICollection<Conversation>
    ConversationConversationTypeNavigation { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCoCooperationTypeNavigation { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCoCustomGenderTypeNavigation { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCoEducationGradeTypeNavigation { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCoMilitaryServiceStatusTypeNavigation { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCoSalaryTypeNavigation { get; set; }
public virtual ICollection<Course>
    CourseCertificateTypeNavigation { get; set; }
public virtual ICollection<Course>
    CourseEvaluationMethodNavigation { get; set; }
public virtual ICollection<Course>
    CourseSkillTypeNavigation { get; set; }
public virtual ICollection<CreditOrCashPayment>
    CreditOrCashPayment { get; set; }
public virtual ICollection<CustomerClub>
    CustomerClub { get; set; }
public virtual ICollection<CustomerClubPaymentMethodType>
    CustomerClubPaymentMethodType { get; set; }
public virtual ICollection<Error>
    Error { get; set; }
public virtual ICollection<Exam>
    ExamExamDisplayResultTypeNavigation { get; set; }
public virtual ICollection<Exam>
    ExamExamTypeNavigation { get; set; }
public virtual ICollection<Factor>
    FactorFactorReportTypeNavigation { get; set; }
public virtual ICollection<Factor>
    FactorFactorReviewStatusTypeNavigation { get; set; }
public virtual ICollection<FactorGroup>
    FactorGroup { get; set; }
public virtual ICollection<FactorPaymentInstallment>
    FactorPaymentInstallment { get; set; }
public virtual ICollection<FactorVerification>
    FactorVerification { get; set; }
public virtual ICollection<HybridPayment>
    HybridPaymentHybridPaymentStatusTypeNavigation { get; set; }
public virtual ICollection<HybridPayment>
    HybridPaymentPaymentMethodTypeNavigation { get; set; }
public virtual ICollection<HybridPaymentRequest>
    HybridPaymentRequest { get; set; }
public virtual ICollection<HybridPayment>
    HybridPaymentSaleTypeNavigation { get; set; }
public virtual ICollection<Installment>
    Installment { get; set; }
public virtual ICollection<CommonBaseData>
    InverseParent { get; set; }
public virtual ICollection<Issue>
    IssueIssueStateTypeNavigation { get; set; }
public virtual ICollection<Issue>
    IssueIssueTypeNavigation { get; set; }
public virtual ICollection<Issue>
    IssuePriorityTypeNavigation { get; set; }
public virtual ICollection<Issue>
    IssueResolutionTypeNavigation { get; set; }
public virtual ICollection<IssueTypeItem>
    IssueTypeItem { get; set; }
public virtual ICollection<JobCoTags>
    JobCoTags { get; set; }
public virtual ICollection<Keyword>
    Keyword { get; set; }
public virtual ICollection<Loan>
    LoanBankTypeNavigation { get; set; }
public virtual ICollection<Loan>
    LoanLoanStatusNavigation { get; set; }
public virtual ICollection<MediaSession>
    MediaSession { get; set; }
public virtual ICollection<Message>
    Message { get; set; }
public virtual ICollection<ModifiedOrderItem>
    ModifiedOrderItemModifiedOrderItemStatusTypeNavigation { get; set; }
public virtual ICollection<ModifiedOrderItem>
    ModifiedOrderItemModifiedOrderItemTypeNavigation { get; set; }
public virtual ICollection<NewsStore>
    NewsStoreLanguageNavigation { get; set; }
public virtual ICollection<NewsStore>
    NewsStoreNewsTypeNavigation { get; set; }
public virtual ICollection<Order>
    OrderMembershipTypeNavigation { get; set; }
public virtual ICollection<Order>
    OrderOrderStatusTypeNavigation { get; set; }
public virtual ICollection<Order>
    OrderRecipientGenderTypeNavigation { get; set; }
public virtual ICollection<Order>
    OrderRecipientRelationshipWithBuyer { get; set; }
public virtual ICollection<Order>
    OrderShippingStatusTypeNavigation { get; set; }
public virtual ICollection<Order>
    OrderSubsidyStatusTypeNavigation { get; set; }
public virtual ICollection<Order>
    OrderSubsidyTypeNavigation { get; set; }
public virtual ICollection<Payment>
    PaymentCardTypeNavigation { get; set; }
public virtual ICollection<PaymentGatewayPaymentMethod>
    PaymentGatewayPaymentMethod { get; set; }
public virtual ICollection<Payment>
    PaymentPaymentMethodTypeNavigation { get; set; }
public virtual ICollection<Payment>
    PaymentPaymentStatusTypeNavigation { get; set; }
public virtual ICollection<Payment>
    PaymentSaleTypeNavigation { get; set; }
public virtual ICollection<PersonBankAccount>
    PersonBankAccountBankTypeNavigation { get; set; }
public virtual ICollection<PersonBankAccount>
    PersonBankAccountDepositStatusNavigation { get; set; }
public virtual ICollection<Person>
    PersonEducationGradeNavigation { get; set; }
public virtual ICollection<Person>
    PersonGenderNavigation { get; set; }
public virtual ICollection<Product>
    ProductBasePriceBaseUnitTypeNavigation { get; set; }
public virtual ICollection<Product>
    ProductBasePriceUnitTypeNavigation { get; set; }
public virtual ICollection<Product>
    ProductDownloadActivationTypeNavigation { get; set; }
public virtual ICollection<Product>
    ProductProductTypeNavigation { get; set; }
public virtual ICollection<Product>
    ProductRecurringCyclePeriodTypeNavigation { get; set; }
public virtual ICollection<Product>
    ProductSalesModelTypeNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningAgeCategoryNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningConversation>
    ProductSalesPlanningConversation { get; set; }
public virtual ICollection<ProductSalesPlanningConversationMessage>
    ProductSalesPlanningConversationMessage { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningCourseLevelNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningCourseStatusNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningCourseTypeNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningCustomGenderNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningDeliveryDateTypeNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningImplementationTypeNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningManageInventoryMethodTypeNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningProductAvailabilityRangeTypeNavigation { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanningRunTypeNavigation { get; set; }
public virtual ICollection<ProductSalesPlanningStudent>
    ProductSalesPlanningStudent { get; set; }
public virtual ICollection<ProductSalesPlanningTeacher>
    ProductSalesPlanningTeacher { get; set; }
public virtual ICollection<ProductSpecificationOption>
    ProductSpecificationOption { get; set; }
public virtual ICollection<ProductTags>
    ProductTags { get; set; }
public virtual ICollection<Question>
    QuestionQuestionContentTypeNavigation { get; set; }
public virtual ICollection<Question>
    QuestionQuestionDifficultyDegreeTypeNavigation { get; set; }
public virtual ICollection<Question>
    QuestionQuestionOptionContentTypeNavigation { get; set; }
public virtual ICollection<Question>
    QuestionQuestionTypeNavigation { get; set; }
public virtual ICollection<Recipient>
    RecipientGenderTypeNavigation { get; set; }
public virtual ICollection<Recipient>
    RecipientMembershipTypeNavigation { get; set; }
public virtual ICollection<Recipient>
    RecipientRelationshipWithBuyerTypeNavigation { get; set; }
public virtual ICollection<Recipient>
    RecipientSubsidyStatusTypeNavigation { get; set; }
public virtual ICollection<Recipient>
    RecipientSubsidyTypeNavigation { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailRefundStatusTypeNavigation { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetailRefundTypeNavigation { get; set; }
public virtual ICollection<RefundPaymentTransactionItemRequest>
    RefundPaymentTransactionItemRequestRefundPaymentTransactionItemRequestStatusNavigation { get; set; }
public virtual ICollection<RefundPaymentTransactionItemRequest>
    RefundPaymentTransactionItemRequestRefundPaymentTransactionItemRequestTypeNavigation { get; set; }
public virtual ICollection<RepresentationEstablishedLicenseType>
    RepresentationEstablishedLicenseType { get; set; }
public virtual ICollection<Representation>
    RepresentationLicenseTypeNavigation { get; set; }
public virtual ICollection<Representation>
    RepresentationOwnershipTypeNavigation { get; set; }
public virtual ICollection<RepresentationTeacher>
    RepresentationTeacher { get; set; }
public virtual ICollection<SaleConfiguration>
    SaleConfiguration { get; set; }
public virtual ICollection<SaleConfigurationAttribute>
    SaleConfigurationAttributeDiscountLimitationTypeNavigation { get; set; }
public virtual ICollection<SaleConfigurationAttribute>
    SaleConfigurationAttributeDiscountTypeNavigation { get; set; }
public virtual ICollection<SaleConfigurationAttribute>
    SaleConfigurationAttributeSaleTypeNavigation { get; set; }
public virtual ICollection<ShoppingCartItem>
    ShoppingCartItem { get; set; }
public virtual ICollection<TaxCategorySaleConfiguration>
    TaxCategorySaleConfiguration { get; set; }
public virtual ICollection<Unit>
    Unit { get; set; }
public virtual ICollection<UsedSubsidy>
    UsedSubsidy { get; set; }
public virtual ICollection<UserExam>
    UserExamExamResultTypeNavigation { get; set; }
public virtual ICollection<UserExam>
    UserExamExamStatusTypeNavigation { get; set; }
public virtual ICollection<UserHomeWork>
    UserHomeWork { get; set; }
public virtual ICollection<ValidHekmatCard>
    ValidHekmatCard { get; set; }
public virtual ICollection<VirtualClass>
    VirtualClass { get; set; }
public virtual ICollection<VirtualClassRoomConfig>
    VirtualClassRoomConfig { get; set; }
public virtual ICollection<WalletProgram>
    WalletProgramWalletProgramAllocationMethodTypeNavigation { get; set; }
public virtual ICollection<WalletProgram>
    WalletProgramWalletProgramStatusTypeNavigation { get; set; }
public virtual ICollection<WalletProgram>
    WalletProgramWalletProgramTypeNavigation { get; set; }
public virtual ICollection<WalletScope>
    WalletScope { get; set; }
public virtual ICollection<YashilCms>
    YashilCms { get; set; }
public virtual ICollection<YashilScheduler>
    YashilScheduler { get; set; }
    }
 }
