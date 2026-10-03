
namespace YashilCommerce.Core.Enums
{
    public enum HekmatRequestResult
    {
        /// <summary>
        /// array list returned from Hekmat Request position 0 for result
        /// </summary>
        StatusResult = 0,

        /// <summary>
        /// position 1 for transactionCode
        /// </summary>
        TransactionIdResult = 1,
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ECommerceApplicationActions
    {
        AssignToGroup,
        ExportToExcel
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ECommerceEvents
    {
        AccessDeniedAssignToGroup = 100,
        AccessDeniedExportToExcel = 101,
        ExportToExcel = 102,
        AccessDeniedExportToPdf = 103,
        ExportToPdf = 104,
        AccessDeniedGetLimitedInformation = 105,
        AccessDeniedGetUserOrderItems = 106,
        AccessDeniedGetSataSubsidyBasedOrderItemFullInfoView = 107,
        AccessDeniedGetVezaratSubsidyBasedOrderItemFullInfoView = 108,
        AccessDeniedGetOrderItemFullInfo = 109,
        AccessDeniedGetByCustomFilterForList = 110,
        AccessDeniedGetAwaitingSataSubsidyApproval = 111,
        AccessDeniedGetByCustomFilterForFactorGroupList = 112,
        AccessDeniedGetCurrentUserUnFactoredOrderItems = 113,
        AccessDeniedGetForConfirmList = 114,
        AccessDeniedGetTransactionsForSendToHekmatAsync = 115,
        AccessDeniedGetHekmatTransactionItem = 116,
        AccessDeniedUpdateHekmatTransaction = 117,
        AccessDeniedUploadHekmatTransactionFileAsync = 118,
        AccessDeniedGetTransactionFilesAsync = 119,
        AccessDeniedGetIssueAssigneeByFiler = 120,
        AccessDeniedModifiedOrderGetByCustomFilterForModify = 121,
        AccessDeniedModifiedOrderGetByCustomFilterForModifyAction = 122,
        AccessDeniedCancelModification = 123,
        AccessDeniedChangeOrderItemToItSelfAsync = 124,
        AccessDeniedGetPersonBankAccountInformation = 125,
        AccessDeniedGetUploadedProductDetailPagesFileAsync = 126,
        AccessDeniedGetHekmatUploadedTransactionFiles = 127,
        AccessDeniedGetHekmatUploadedRefundFiles = 128,
        AccessDeniedUploadHekmatTransactionFile = 129,
        AccessDeniedUploadHekmatRefundFile = 130,
        AccessDeniedGetRefundDetailFullInfo = 131,
        AccessDeniedConfirmSubsidyBySata = 132,
        AccessDeniedVerifySataSubsidy = 133,
        AccessDeniedRejectionSataSubsidy = 134,
        AccessDeniedGetUploadedFactorPaymentFiles = 135,
        AccessDeniedGetUploadedProductItemTransactionFilesAsync = 136,
        AccessDeniedGetUploadedVoucherTransactionFilesAsync = 137,
        HumaxServiceRegisterError = 138
    }


    /// <summary>
    /// 
    /// </summary>
    public enum ArchiveModeEnum
    {
        Archived = 1,
        NotArchived = 2,
        All = 3
    }
    /// <summary>
    /// انواع تخفیف 
    /// </summary>
    public enum DiscountTypeEnum
    {
        /// <summary>
        /// اختصاص به مجموع سفارش
        /// </summary>
        AssignedToOrderTotal = 2293,

        /// <summary>
        /// اختصاص به محصولات
        /// </summary>
        AssignedToProduct = 2294,

        /// <summary>
        /// اختصاص به دسته بندی
        /// </summary>
        AssignedToCategories = 2295,

        /// <summary>
        /// اختصاص به مرکز
        /// </summary>
        AssignedToCompany = 2296,

        /// <summary>
        /// اختصاص به ارسال
        /// </summary>
        AssignedToShipping = 2297,

        /// <summary>
        ///  اختصاص به مجموع زیر مجموعه سفارش
        /// </summary>
        AssignedToOrderSubTotal = 2298
    }

    /// <summary>
    /// انواع محدودیت تخفیف
    /// </summary>
    public enum DiscountLimitationTypEnum
    {
        /// <summary>
        /// نا محدود
        /// </summary>
        Unlimited = 2299,

        /// <summary>
        /// N بار
        /// </summary>
        NTimesOnly = 2300,

        /// <summary>
        /// N برای هر مشتری
        /// </summary>
        NTimesPerCustomer = 2301
    }
    /// <summary>
    /// نوع درخواست تراکنش برای درگاه پرداخت  
    /// </summary>
    public enum SendTransactionTypeEnum
    {
        VerifyTransaction = 1,
        Reverse = 2
    }

    public enum SaminPurchaseTypeEnum
    {
        /// <summary>
        /// تراکنش اعتباری
        /// </summary>
        Credit = 1,
        /// <summary>
        /// تراکنش اقساطی
        /// </summary>
        Installment = 2,
        /// <summary>
        /// تراکنش نقدی	
        /// </summary>
        Debit = 3
    }

    public enum ProductTemplateIdEnum
    {
        Book = 1,
        Test = 2,
        Group = 3
    }



    public enum VoucherTemplateIdEnum
    {
        HasLinkAddress = 1,
        NotHasLinkAddress = 2
    }
    /// <summary>
    /// مربوط به نوع محصول که میتواند
    /// اشتغال یا سایر باشد
    /// </summary>
    public enum ProductTypeEnum
    {
        /// <summary>
        /// اشتغال
        /// </summary>
        Employment = 2882,
        /// <summary>
        /// سایر
        /// </summary>
        Other = 2883
    }

    /// <summary>
    /// مربوط به نوع محصول که میتواند
    /// اشتغال یا سایر باشد
    /// </summary>
    public enum SalesModelTypeEnum
    {
        /// <summary>
        /// فروش در تمامی نمایندگی ها
        /// </summary>
        SalesInAllRepresentation = 1085,
        /// <summary>
        /// فروش در نمایندگی های منتخب
        /// </summary>
        SalesInSelectedRepresentation = 1086,
        /// <summary>
        /// برنامه ریزی فروش توسط نمایندگی
        /// </summary>
        SalesPlanningByRepresentation = 1087
    }

    public enum InstallmentPaymentStatusTypeEnum
    {
        Unpaid = 3156,
        Paying = 3157,
        Paid = 3158
    }

    public enum WalletReservationStatusType
    {
        Reserved = 3184,
        Released = 3185,
        Expired = 3186
    }
}