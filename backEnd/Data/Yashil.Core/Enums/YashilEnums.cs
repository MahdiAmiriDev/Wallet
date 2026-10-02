namespace Yashil.Core.Enums
{
    public enum GenderEnum
    {
        /// <summary>
        /// مرد
        /// </summary>
        Man = 26,
        /// <summary>
        /// زن
        /// </summary>
        Female = 27
    }
    /// <summary>
    /// نوع عودت وجه
    /// </summary>
    public enum RefundTypeEnum
    {
        /// <summary>
        /// ناجا
        /// </summary>
        Naja = 1215,
        /// <summary>
        /// آنلاین
        /// </summary>
        Online = 1216,
        /// <summary>
        /// حکمت
        /// </summary>
        Hekmat = 1217
    }
    /// <summary>
    /// وضعیت عودت وجه
    /// </summary>
    public enum RefundStatusTypeEnum
    {
        /// <summary>
        /// ثبت اوایه
        /// </summary>
        Register = 1212,
        /// <summary>
        /// ارسال برای اصلاح قیمت
        /// </summary>
        SendForModifyPrice = 1213,
        /// <summary>
        /// انجام شد
        /// </summary>
        Done = 1214
    }
    public enum CacheTime
    {
        OneHour = 60,
        TwoHour = 120,
        SixHour = 360,
        TwelveHour = 720,
        TwentyFourHour = 1440,
        OneMonth = 43200,
        Default = 360,
    }
    public enum OrderListType
    {
        WaitingForSubsidyOrderrViewModel = 1
    }
    public enum AccessLevelEnum
    {
        /// <summary>
        /// دسترسی عادی
        /// </summary>
        Normal = 1,

        /// <summary>
        /// محرمانه
        /// </summary>
        Confidential = 2,

        /// <summary>
        /// خیلی محرمانه
        /// </summary>
        VeryConfidential = 3,

        /// <summary>
        /// سری
        /// </summary>
        Secret = 4,

        /// <summary>
        /// بکلی سری
        /// </summary>
        TotallySecret = 5
    }
    public enum NewsTypeEnum
    {
        Simple = 1069,
        Gallery = 1075,
        Movie = 1072,
        Notification = 1074,
        Pictorial = 1071
    }
    public enum ImplementationTypeEnum
    {
        /// <summary>
        /// آنلاین
        /// </summary>
        Online = 11,
        /// <summary>
        /// کلاس حضوری
        /// </summary>
        AttendanceClass = 12,
        /// <summary>
        /// حضوری و آنلاین
        /// </summary>
        Mixed = 1192
    }
    public enum CommonBaseTypeEnum
    {
        CustomGender = 2,
        CourseType = 3,
        AgeCategory = 4,
        ImplementationType = 5,
        CertificateType = 6,
        SkillType = 7,
        CustomCategory = 8,
        CountryDivisionType = 11,
        Gender = 12,
        EducationGrade = 13,
        EducationalResources = 14,
        RunType = 15,
        CourseEquipment = 16,
        EvaluationMethod = 17,
        CooperationType = 18,
        LicenseType = 19,
        OwnershipType = 20,
        CourseStatus = 24,
        FileFormat = 25,
        EstablishedLicenseType = 29,
        NewsType = 1026,
        Job = 1027,
        Language = 1031,
        BankType = 1032,
        ContractType = 1033,
        ContractStatus = 1034,
        AttributeType = 1036,
        SalesModelType = 1037,
        PagePositionType = 1042,
        OrderStatusType = 1043,
        PaymentStatusType = 1044,
        ShippingStatusType = 1045,
        SaleType = 1046,
        SubsidyType = 1047,
        MembershipType = 1048,
        CardType = 1049,
        ShoppingCartType = 1050,
        DepositStatus = 1116,
        PaymentMethodType = 1052,
        ConversationStatusType = 1116,
        ConversationType = 1117,
        ConversationBoxType = 1118,
        QuestionType = 1119,
        QuestionDifficultyDegreeType = 1122,
        EducationalFieldType = 3131,
        JobCoCooperationType = 3135,
        SalaryType = 3136,
        MilitaryServiceStatusType = 3137,
        RelationshipWithBuyer = 3139
    }
    public enum PagePositionTypeEnum
    {
        /// <summary>
        /// اسلایدر سطح یک
        /// </summary>
        MainPageSliderLevelOne = 2306,
        /// <summary>
        /// اسلایدر سطح دو
        /// </summary>
        MainPageSliderLevelTwo = 1088,
        /// <summary>
        /// توضیحات صفحه ثبت نام
        /// </summary>
        RegisterPageDescription = 1146,
        /// <summary>
        /// توضیحات پیش فاکتور
        /// </summary>
        OrderSummeryDescription = 1145,
        /// <summary>
        /// توضیحات مربوط به انتظار برای تایید یارانه حکمت
        /// </summary>
        OrderSummeryWaitingForsubsidyConfirm = 1155,
        /// <summary>
        /// توضیحات مربوط به انتظار برای تایید پرداخت حکمت
        /// </summary>
        OrderSummeryWaitingForHekmatConfirm = 1159,
        /// <summary>
        /// توضیحات مربوط به دلیل عدم امکان پرداخت
        /// </summary>
        OrderSummeryReasonsForInabilityToPay = 1156,
        /// <summary>
        ///توضیحات مربوط به دریافت کالا
        /// </summary>
        OrderSummeryDescriptionOfReceivingProduct = 1193,
        /// <summary>
        /// صفحه درباره ما
        /// </summary>
        SiteAbout = 1190,
        /// <summary>
        /// صفحه تماس با ما
        /// </summary>
        SiteContactUs = 1219,
        /// <summary>
        /// معرفی شرکت دارنده وب سایت
        /// </summary>
        CorporationAbout = 1224,
        /// <summary>
        /// معرفی ویژگی های فوتر وب سایت
        /// </summary>
        SiteFooterFeature = 1225,
        // متن توافقنامه سایت در صفحه ورودهای موفق و ناموفق سایت
        UserAgreementConfirmation = 1255,
        /// <summary>
        /// تصویر اول مربوط به تبلیغات تجاری صفحه اصلی
        /// </summary>
        FirstImageOfCommercialAdvertisingInHomePage = 2932,
        /// <summary>
        /// تصویر دوم مربوط به تبلیغات تجاری صفحه اصلی
        /// </summary>
        SecondImageOfCommercialAdvertisingInHomePage = 2933,
        /// <summary>
        /// تصویر اول مربوط به مهارت آموزی در صفحه اصلی
        /// </summary>
        FirstImageOfLearningSkillInHomePage = 2934,
        /// <summary>
        /// تصویر دوم مربوط به مهارت آموزی در صفحه اصلی
        /// </summary>
        SecondImageOfLearningSkillInHomePage = 2935,
        /// <summary>
        /// تصویر سوم مربوط به مهارت آموزی در صفحه اصلی
        /// </summary>
        ThirdImageOfLearningSkillInHomePage = 2936,
        /// <summary>
        /// تصویر چهارم مربوط به مهارت آموزی در صفحه اصلی
        /// </summary>
        FourthImageOfLearningSkillInHomePage = 2937,
        /// <summary>
        /// تصاویر تبلیغاتی بالا سمت راست صفحه اصلی
        /// سامانه باقرالعلوم
        /// </summary>
        HomePageTopRightSliderAdvertisingImage = 2938,
        /// <summary>
        /// محتوای صفحه ی ارتباط با ما
        /// </summary>
        ContactUs = 2939,

        /// <summary>
        /// گالری عکس صفحه اصلی
        /// </summary>
        MainPageGalleryImage = 3147,

    }
    public enum ShoppingCartTypeEnum
    {
        /// <summary>
        /// سبد خرید
        /// </summary>
        ShoppingCart = 1114,
        /// <summary>
        /// لیست علاقه مندی ها
        /// </summary>
        WishList = 1115
    }
    public enum ListViewMode
    {
        Card = 1,
        List = 2,
        ListWithScroll = 3,
        ListWithPagination = 4
    }
    public enum CompanyViewMode
    {
        Horizontal = 1,
        VerticalSingleSelection = 2,
        VerticalMultipleSelection = 3
    }
    public enum FilterContainerViewMode
    {
        DesktopMode = 1,
        MobileMode = 2,

    }
    public enum CartType
    {
        /// <summary>
        ///    حکمت
        /// </summary>
        Hekmat = 1112,
        /// <summary>
        /// بنیاد
        /// </summary>
        Bonyad = 1113,
        /// <summary>
        /// شتاب
        /// </summary>
        Shetab = 1116
    }
    public enum OrderStatusTypesEnum
    {
        /// <summary>
        /// ثبت اولیه
        /// </summary>
        InitialRegistration = 1089,
        /// <summary>
        /// در حال بررسی پرداخت حکمت
        /// تراکنش های حکمت به صورت آفلاین به سمت حکمت ارسال و نتیجه آن برمی گیردد
        /// مدت زمانی طول میکشد و سفارش در این وضعیت می ماند تا نتیجه تراکنش مشخض شود  
        /// </summary>
        Processing = 1090,
        /// <summary>
        /// کامل
        /// </summary>
        Complete = 1091,
        /// <summary>
        /// مرجوعی  :  سفارش کامل با پرداخت موفق لغو شده است
        /// انصراف بعد از تایید نهایی و پرداخت
        /// </summary>
        Refund = 1092,
        /// <summary>
        /// انصراف کاربر از خرید-در این حالت کاربر از فرم سفارش دکمه انصراف خرید را زده است
        /// </summary>
        Canceled = 1124,
        /// <summary>
        /// لغو تراکنش توسط سیستم بدلیل گذشت مدت زمان مشخص و عدم پرداخت توسط کاربر 
        /// </summary>
        CancelBySystem = 1144,
        /// <summary>
        /// پرداخت ناموفق
        /// </summary>
        UnsuccessfulPayment = 1128,
        /// <summary>
        /// وقتی کاربر کالایی را تغییر میدهد و منتظر تایید تامین کننده برای تغییر قرار میگیرد
        /// </summary>
        WaitingForSupplierApprovalBeforeEditOrder = 1130,
        /// <summary>
        /// وقتی کاربر کالایی را  مرجوع  می کند و منتظر تایید تامین کننده برای تغییر قرار میگیرد
        /// </summary>
        WaitingForSupplierApprovalBeforereRefundOrder = 1143,
        /// <summary>
        /// پرداخت ناقص
        /// </summary>
        IncompletePayment = 1131,
        /// <summary>
        /// نیاز به تایید تایمن کننده برای پرداخت
        /// </summary>
        RequiredSupplierApprovalBeforePayment = 3108,

        /// <summary>
        /// منتظر پرداخت
        /// </summary>
        WaitingForPayment = 3109,
        /// <summary>
        /// انصراف از پرداخت
        /// </summary>
        CancelFromPayment = 1133,
        /// <summary>
        /// پرداخت ناقص نقدی اقساطی
        /// </summary>
        IncompleteCashAndInstallmentPay = 3112,
        /// <summary>
        /// رد شده توسط تامین کننده
        /// </summary>
        RejectBySupplier = 3127

    }
    public enum ShippingStatusTypesEnum
    {
        /// <summary>
        /// حمل و نقل مورد نیاز نیست
        /// </summary>
        NoShippingRequired = 1099,
        /// <summary>
        /// هنوز ارسال نشده است
        /// </summary>
        NotBeenSentYet = 1100,
        /// <summary>
        /// حمل و نقل جزئی
        /// </summary>
        PartialShipping = 1101,
        /// <summary>
        /// ارسال شده
        /// </summary>
        Sent = 1102,
        /// <summary>
        /// تحویل داده شده
        /// </summary>
        Delivered = 1103
    }
    public enum AppEntityEnum
    {
        Resource = 64,
        Application = 65,
        AppAction = 66,
        Organization = 67,
        ResourceAppAction = 68,
        RoleResourceAction = 69,
        Role = 70,
        ReportConnectionString = 71,
        DashboardGroupDashboard = 72,
        DocFormat = 73,
        ReportGroup = 74,
        AppConfig = 75,
        DashboardGroup = 76,
        DashboardConnectionString = 77,
        sysdiagrams = 78,
        User = 79,
        AppEntity = 80,
        ReportStore = 81,
        UserDashboard = 82,
        AccessLevel = 83,
        RoleDashboard = 84,
        RoleReport = 85,
        YashilDataProvider = 86,
        DocumentCategory = 87,
        YashilConnectionString = 88,
        Schemas = 89,
        AppDocument = 90,
        UserReport = 91,
        DashboardStore = 92,
        ReportGroupReport = 93,
        Menu = 94,
        UserRole = 95,
        DocType = 96,
        Post = 97,
        City = 98,
        Person = 99,
        CommonBaseData = 100,
        Course = 102,
        Representation = 104,
        RepresentationPerson = 105,
        CommonBaseType = 106,
        AppEntityAttribute = 110,
        AppEntityAttributeMapping = 111,
        AppEntityAttributeValue = 112,
        AdditionalUserProp = 113,
        CoursePlanning = 1116,
        Job = 2115,
        MainNews = 2117,
        Service = 2118,
        Keyword = 2119,
        NewsStore = 2120,
        NewsKeyword = 2121,
        RepresentationTeacher = 2122,
        RepresentationEstablishedLicenseType = 2124,
        PersonBankAccount = 2125,
        CoursePlanningSchedule = 2126,
        Contract = 2127,
        Address = 2128,
        ProductTemplate = 2130,
        GiftCardType = 2132,
        RepresentationCategory = 2134,
        TaxCategory = 2135,
        Product = 2137,
        Order = 2138,
        ProductSalesPlanning = 2139,
        CompanyMainCategory = 2140,
        DependentProductSale = 2141,
        ProductCategory = 2143,
        Warehouse = 2144,
        Category = 2145,
        Company = 2146,
        ContractPaymentMethod = 2147,
        SaleConfiguration = 2148,
        Specification = 2149,
        SpecificationOption = 2151,
        CategorySpecification = 2153,
        ProductType = 2154,
        ProductTypeSpecification = 2155,
        ProductSpecificationOption = 2156,
        ApplicationPaymentMethod = 2158,
        PasswordHistory = 2159,
        YashilCms = 2160,
        LogEvents = 2161,
        FanCity = 2163,
        PasswordBlackList = 2164,
        CreditOrCashPayment = 2165,
        Installment = 2166,
        OrderNote = 2167,
        OrderItem = 2168,
        CardInfo = 2169,
        CardAmountHistory = 2170,
        Subsidy = 2171,
        Affiliate = 2172,
        RewardPointsHistory = 2173,
        ShoppingCartItem = 2174,
        Recipient = 2175,
        CustomerClub = 2176,
        CustomerClubCard = 2177,
        CustomerClubPaymentMethodType = 2178,
        Payment = 2179,
        OrderTrakking = 2181,
        FactorItem = 2183,
        Factor = 2184,
        FactorPayment = 2185,
        ContentCategory = 2186,
        ApplicationCustomerClub = 2187,
        FactorVerification = 2188,
        FactorVerificationConfiguration = 2189,
        FactorVerificationConfigurationRole = 2190,
        CardNumberOrganization = 2191,
        CitySubsidyFactor = 2192,
        CustomerClubPaymentInstallment = 2193,
        CustomerClubPaymentInstallmentOldSite = 2194,
        DayNumber = 2195,
        Error = 2196,
        FactorGroup = 2197,
        FaniStandardCategory = 2198,
        Issue = 2199,
        IssueComment = 2200,
        IssueLogWork = 2201,
        IssueParticipant = 2202,
        IssueTypeItem = 2203,
        ItemGroup = 2204,
        Message = 2205,
        ModifiedOrderItem = 2206,
        MonthNumber = 2207,
        OrderChangePermission = 2208,
        OrderItemDependentProductSale = 2209,
        OrginalCityNames = 2210,
        PersianDateMonth = 2211,
        ProductSalesPlanningStudent = 2212,
        productTemp = 2213,
        ProductTest = 2214,
        ReportStoreApplication = 2217,
        TempUserRoleMapping = 2218,
        tmpCustomerClubPaymentInstallmentOld = 2219,
        UsedSubsidy = 2221,
        UserApplication = 2222,
        ValidHekmatCard = 2223,
        Vocher = 2224,
        VocherType = 2225,
        YashilScheduler = 2226,
        YearNumber = 2227,
        CustomerClubRefund = 2228,
        MerchantInfo = 2229,
        MerchantTerminalInfo = 2230,
        OrderItemFullInfo = 2231,
        Refund = 2232,
        RefundDetail = 2233,
        RefundWD = 2234,
        TmpRefund = 2235,
        RefundDetailForPayment = 2240,
        FactorDeduction = 2241,
        HekmatApprovedTransaction = 2242,
        HekmatCanceledTransaction = 2243,
        HekmatEffectlessCanceledTransaction = 2244,
        HekmatEffectlessModifiedTransaction = 2245,
        HekmatModifiedTransaction = 2246,
        Naja = 2247,
        OldSiteRefundDetail = 2248,
        PersonRefundedBankAccountInformation = 2249,
        UploadedProductItem = 2251,
        UploadedFactorPayment = 2252,
        UploadedVoucher = 3277,
        Conversation = 3278,
        HekmatTransaction = 3285,
        UploadedQuestion = 6306,
        UploadedUser = 6308,
        MediaSession = 4302,
        UserHomeWork = 6334,
        Certificate = 6337,
        UploadedCertificate = 6386,
        UploadedBulkPasswordReset = 6387,
        HekmatPaymentGatewayTransactionBulkRequestDetail = 6397,
        HekmatPaymentGatewayTransactionBulkRequestCategory = 6396,
        HekmatPaymentGatewayTransactionBulkRequest = 6395,
        UploadedFactorInstallmentPayment = 7412

    }
    public enum PaymentStatusTypesEnum
    {
        ///// <summary>
        ///// ثبت اولیه
        ///// </summary>
        //InitialRegistration = 1093,
        /// <summary>
        /// ارسال به درگاه پرداخت
        /// صفحه در گاه باز شده است
        /// </summary>
        SendToPaymentGateway = 1094,
        /// <summary>
        /// متد  درگاه باز شده است و کاربر مشخصات وارد کرده و متد
        /// verify
        /// سمت پذیرنده فرا خوانی شده است 
        /// </summary>
        WaitingResponseFromPaymentGateway = 1123,
        /// <summary>
        /// پرداخت با موفقیت انجام شد
        /// </summary>
        Paid = 1095,
        /// <summary>
        ///بازپرداخت جزئی
        /// </summary>
        PartialRefund = 1096,
        /// <summary>
        /// بازپرداخت کل
        /// </summary>
        Refund = 1097,
        /// <summary>
        /// کاربر دکمه انصراف را در صفحه درگاه پرداخت میزند
        /// یا به صفحه درگاه را باز کرده و کاری انجام نداده است
        /// </summary>
        Cancel = 1098,
        /// <summary>
        /// پرداخت ناموفق 
        /// </summary>
        Unsuccessful = 1125,
        /// <summary>
        /// انصراف پرداخت گروه بندی نشده به دلیل  تغییر قیمت سفارش فقط در پرداخت های ناجا
        /// </summary>
        UnGroupedCancellation = 1218,
        /// <summary>
        /// پرداخت ناقص نقدی اقساطی
        /// </summary>
        IncompleteCashAndInstallmentPay = 3111,
        /// <summary>
        /// ابطال تراکنش
        /// </summary>
        Void = 3209

    }

    public enum MembershipTypesEnum
    {
        /// <summary>
        /// وزارت
        /// </summary>
        Vezarat = 1108,
        /// <summary>
        /// ساتا
        /// </summary>
        Sata = 1109,
        /// <summary>
        /// سایر
        /// </summary>
        Others = 1111
    }

    public enum SubsidyTypesEnum
    {
        /// <summary>
        /// وزارت
        /// </summary>
        Vezarat = 1107,
        /// <summary>
        /// ساتا
        /// </summary>
        Sata = 1106,
    }

    public enum PaymentMethodTypesEnum
    {
        /// <summary>
        /// حکمت
        /// </summary>
        Hekmat = 1120,
        /// <summary>
        /// درگاه پرداخت شتابی
        /// </summary>
        BankInternetPaymentGateway = 1121,
        /// <summary>
        /// ثمین
        /// </summary>
        Samin = 1122,
        /// <summary>
        /// کیف پول : پرداخت از طریق دیتابیس لوکال و سقف مبلغ خرید برای باشگاه مشتریان
        /// مبتنی بر پرداخت کیف پول در صورت داشتن سقف مبلغ خرید و باقمانده مبلغ خرید
        /// </summary>
        Wallet = 1126,
        /// <summary>
        /// فن آوا کارت(شتاب)
        /// </summary>
        Ansar = 1129,
        /// <summary>
        /// سامان
        /// </summary>
        Saman = 1127,
        /// <summary>
        /// صادرات
        /// </summary>
        Saderat = 2309,
        /// <summary>
        /// پرداخت نقدی اقساطی
        /// </summary>
        Hybrid = 3110,
        /// <summary>
        /// درگاه پرداخت ایران کیش
        /// </summary>
        IranKish = 3146,
        /// <summary>
        /// درگاه اعتباری وایب
        /// </summary>
        Vibe = 3160,
        /// <summary>
        /// درگاه پرداخت آنلاین حکمت
        /// </summary>
        HekmatOnline = 3162,

    }
    /// <summary>
    /// باشگاه مشتریان
    /// آیدی های جدول customerClub
    /// </summary>
    public enum CustomerClubEnum
    {
        /// <summary>
        /// سینا
        /// </summary>
        Sina = 1,
        /// <summary>
        /// بیمه کارافرین
        /// </summary>
        KarAfarin = 4,
        /// <summary>
        /// ناجا
        /// </summary>
        Naja = 5,
        /// <summary>
        /// انستیتو صاایران
        /// </summary>
        InstituteOfSaIran = 6,
        /// <summary>
        /// بانک ملت و شرکت های تابعه
        /// </summary>
        Mellat = 7
    }

    /// <summary>
    /// sales model types
    /// the product sales planning type
    /// </summary>
    public enum SalesModelTypes
    {
        /// <summary>
        /// sales planning for all representations
        /// </summary>
        SalesAtAllRepresentations = 1085,
        /// <summary>
        /// sales planing for selected representations
        /// </summary>
        SalesAtSelectedRepresentations = 1086,
        /// <summary>
        /// sales plan by representations
        ///  te representations plan the sale
        /// </summary>
        SalesByRepresentations = 1087

    }

    public enum ApplicationInstallmentBasedEnum
    {
        /// <summary>
        /// پرداخت نقدی
        /// </summary>
        CashPaymentOnly = 1,
        /// <summary>
        /// پرداخت اقسلطی
        /// </summary>
        InstallmentPaymentOnly = 2,
        /// <summary>
        /// نقدی و اقساطی
        /// </summary>
        BothType = 3
    }

    public enum SubsidyStatusTypesEnum
    {
        /// <summary>
        ///ثبت اولیه
        /// </summary>
        InitialRegistration = 1117,
        /// <summary>
        ///تایید شد
        /// </summary>
        Confirmed = 1118,
        /// <summary>
        /// تایید نشد
        /// </summary>
        Rejected = 1119
    }
    /// <summary>
    /// Registration mode
    /// used for config input fields in registration
    /// </summary>
    public enum RegistrationModeEnum
    {
        /// <summary>
        /// take minimal input fields
        ///  old Value  = 0
        /// </summary>
        Minimal = 2302,
        /// <summary>
        /// take some of them
        /// Old Value  = 1
        /// </summary>
        Medium = 2303,
        /// <summary>
        /// take all of them
        /// old Value = 2
        /// </summary>
        Full = 2304
    }
    /// <summary>
    /// used for password recovery
    /// if it was email in registration form we get email 
    /// </summary>
    public enum PasswordResetRecoveryModeEnum
    {
        /// <summary>
        /// recover password with mobile and sms
        /// </summary>
        Mobile = 0,
        /// <summary>
        /// recover password with email
        /// </summary>
        Email = 1
    }
    /// <summary>
    /// نوع کارتی که با آن پرداخت انجام میشود
    /// </summary>
    public enum CardTypeEnum
    {
        /// <summary>
        /// کارت انصار
        /// </summary>
        Ansar = 1112,
        /// <summary>
        ///  کارت حکمت
        /// </summary>
        Hekmat = 1113,
        /// <summary>
        ///  کارت سپه
        /// </summary>
        Sepah = 1116
    }
    /// <summary>
    /// وضعیت فاکتور
    /// </summary>
    public enum FactorReviewStatusTypeEnum
    {
        /// <summary>
        /// ثبت اولیه
        /// منتظر تایید مدیر سایت
        /// </summary>
        WaitingForConfirmAdmin = 1134,
        /// <summary>
        /// منتظر بارگذاری فایل
        /// </summary>
        WaitingForTheFileToUpload = 1258,
        /// <summary>
        /// تایید شده
        /// </summary>
        Confirmed = 1132,
        /// <summary>
        /// در حال بررسی
        /// </summary>
        Processing = 1136,
        /// <summary>
        /// رد شده
        /// </summary>
        Rejected = 1135

    }
    /// <summary>
    /// گزارش فاکتور 1056
    /// </summary>
    public enum FactorReportTypeEnum
    {
        /// <summary>
        /// با جزییات
        /// </summary>
        FactorReportWithDetail = 1140,
        /// <summary>
        /// بدون جزییات
        /// </summary>
        FactorReportWithoutDetail = 1141
    }
    /// <summary>
    /// وضعیت بررسی فاکتور
    /// 1055
    /// </summary>
    public enum FactorConfirmationTypeEnum
    {
        /// <summary>
        /// ثبت اولیه
        /// </summary>
        Initial = 1137,
        /// <summary>
        /// تایید شده
        /// </summary>
        Confirmed = 1138,
        /// <summary>
        /// رد شده
        /// </summary>
        Rejected = 1139
    }
    /// <summary>
    /// دسته بندی اسناد
    /// </summary>
    public enum DocumentCategoryEnum
    {
        ProductSalesPlanningDocument = 12,
        CourseImage = 13,
        IdentityDocument = 14,
        MainNewsImage = 15,
        VideoNewsImage = 16,
        Album = 17,
        Movie = 18,
        FounderDocument = 19,
        ContractDocument = 20,
        ProductImage = 23,
        CompanyImage = 24,
        RepresentationImage = 25,
        MainIcon = 26,
        ProductCategoryImage = 27,
        BasicCourseDocument = 28,
        CourseImage2 = 29,
        MainImage = 40,
        CompanyLogo = 41,
        FactorDocument = 42,
        FileCategoryContent = 43,
        UploadProductDetailPages = 52,
        ConversationAttachment = 1101,
        YashilHekmatTerminalIcon = 55,
        MediaDocument = 56,
        MediaSessionFullVideo = 1058,
        MediaSessionScormAndOtherContent = 1064,
        UploadQuestion = 1061,
        UploadUser = 1062,
        ProductSalesPlanningPicture = 1067,
        HomeWorkDocument = 1092,
        HomeWorkImage = 1093,
        UserHomeWork = 1106,
        ResumeCertificate = 1107,
        CategoryGroupImage = 1108,
        HekmatPaymentGatewayTransactionBulkRequest = 1112,
        UserAttachmentAnswer = 1114,
    }
    /// <summary>
    /// انواع سند
    /// </summary>
    public enum DocumentTypeEnum
    {
        /// <summary>
        /// فایل های پیوست
        /// ProductSalesPlanning
        /// 
        /// </summary>
        ProductSalesPlanningPdfAttachments = 1,
        CourseGuide = 2,
        CourseImage = 3,
        NationalCard = 4,
        FirstPageIdentityCard = 5,
        MainImageNews = 6,
        Image = 7,
        Image2 = 8,
        Movie = 9,
        FounderNationalCode = 10,
        FounderLicenseImage = 11,
        ContractImage = 13,
        NationalCardLicenseHolderImage = 14,
        ActivityLicenseImage = 15,
        ProductImage = 16,
        CompanyIcon = 17,
        RepresentationImage = 18,
        Icon = 19,
        CategoryIcon = 20,
        CategoryMenuIcon = 50,
        Movie2 = 21,
        CourseIcon = 22,
        CmsImage = 23,
        SliderMobileImage = 1049,
        CompanyMobileLogo = 25,
        CompanyDesktopLogo = 27,
        FactorDocument = 28,
        Content = 29,
        HekmatApprovedTransaction = 31,
        HekmatRefundedTransactions = 32,
        UploadProductItem = 33,
        UploadFactorPayments = 34,
        UploadVoucher = 35,
        UploadProductDetailPages = 36,
        Conversation = 37,
        HekmatTransaction = 38,
        ProductSalesPlanningVideo = 39,
        ProductSalesPlanningImage = 40,
        MediaDocument = 41,
        MediaFullVideo = 1039,
        MediaImage = 1042,
        UploadQuestion = 1043,
        UploadedUser = 1044,
        Scorm = 1045,
        Pdf = 1046,
        PowerPoint = 1048,
        Word = 1049,
        CategorySlider = 1047,
        YashilHekmatTerminalIcon = 1048,
        QuestionImage = 1052,
        HomeWorkFile = 1051,
        HomeWorkImage = 1050,
        QuestionVideo = 1053,
        QuestionPdf = 1054,
        QuestionSound = 1057,
        QuestionOptionImage = 1055,
        QuestionOptionVideo = 1056,
        QuestionOptionSound = 1058,
        ProductSalesPlanningPdfAttachmentFile = 1061,
        UserHomeWorkFile = 1072,
        ProductSalesPlanningZipFile = 1073,
        CategorySliderMobileMode = 1075,
        ResumeCertificate = 1076,
        ResumeLanguageCertificate = 1077,
        ResumeDegreeOfEducation = 1078,
        CategoryGroupDesktopModeImage = 1079,
        CategoryGroupMobileModeImage = 1080,
        CorporationImage = 1081,
        CorporationBackgroundImage = 1082,
        UploadedCertificate = 1083,
        UploadedBulkPasswordReset = 1084,
        HekmatPaymentGatewayTransactionBulkRequest = 1085,
        UploadedExamFile = 1086,
        UnitHomePageImage = 1090,
        UnitDetailImage = 1089,
        UnitProjectImage = 1091,
        UploadedNewsFile = 1092,
        UploadedFactorInstallmentPayment = 1093
    }
    /// <summary>
    /// نوع پیام
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        ///  پیام احراز هویت : پیامی برای ارسال کد تایید در هنگام ورود در سایت یا ثبت نام در سایت به شماره موبایل کاربر ارسال میشود 
        /// </summary>
        Authenticationessage = 1147
    }

    /// <summary>
    /// نوع مسئله 
    /// </summary>
    public enum IssueTypeEnum
    {
        /// <summary>
        /// مشکل
        /// </summary>
        Problem = 1184,
        /// <summary>
        /// کار
        /// </summary>
        Task = 1148,
        /// <summary>
        /// خطا
        /// </summary>
        Error = 1149,
        /// <summary>
        /// نامشخص بودن وضعیت رهگیری
        /// </summary>
        UnknownOfTrackingStatus = 1166,
        /// <summary>
        /// خطا در ثبت پایه/مقطع تحصیلی
        /// </summary>
        ErrorRegisteringBase = 1167,
        /// <summary>
        /// خطا در انتخاب رشته/گرایش تحصیلی
        /// </summary>
        ErrorInChoosingFieldOfStudy = 1168,
        /// <summary>
        /// خطا در انتخاب نوع کالا
        /// </summary>
        ErrorSelectingProductType = 1169,
        /// <summary>
        /// خطا در ثبت مشخصات پرداخت
        /// </summary>
        ErrorRegisteringPaymentDetails = 1170,
        /// <summary>
        /// مشکل مشاهده اطلاعات سایت
        /// </summary>
        DifficultyViewingSiteInformation = 1171,
        /// <summary>
        /// خطا در انتخاب مرکز
        /// </summary>
        ErrorSelectingCompany = 1172,
        /// <summary>
        /// خطا در انتخاب نمایندگی
        /// </summary>
        ErrorSelectingRepresentation = 1173,
        /// <summary>
        /// اطلاع  رسانی نامناسب نمایندگی
        /// </summary>
        InappropriateInformationRepresentation = 1174,
        /// <summary>
        /// برخورد نامناسب نمایندگی
        /// </summary>
        ImproperRepresentation = 1175,
        /// <summary>
        /// تخلف نمایندگی
        /// </summary>
        RepresentationViolation = 1176,
        /// <summary>
        /// تأخیر در تحویل  محصول
        /// </summary>
        DelayInProductDelivery = 1177,
        /// <summary>
        /// نقص در  محصول تحویلی
        /// </summary>
        DefectsInTheDeliveredProduct = 1178,
        /// <summary>
        /// شکایت
        /// </summary>
        Complaint = 1179,
        /// <summary>
        /// انصراف
        /// </summary>
        Cancel = 1180,
        /// <summary>
        /// عودت وجه
        /// </summary>
        Refund = 1253
    }
    /// <summary>
    ///  نوع نتیجه 
    /// </summary>
    public enum ResolutionType
    {
        /// <summary>
        /// انجام شد
        /// </summary>
        Done = 1150,
        /// <summary>
        /// انجام نشد
        /// </summary>
        Failed = 1151,
        /// <summary>
        /// تکراری است
        /// </summary>
        Duplicate = 1152,
        /// <summary>
        /// غیر قابل انجام است
        /// </summary>
        Impossible = 1153,

    }
    /// <summary>
    /// اولویت
    /// </summary>
    public enum PriorityTypeEnum
    {
        /// <summary>
        /// بالاترین
        /// </summary>
        Highest = 1154,
        /// <summary>
        /// بالا
        /// </summary>
        High = 1185,
        /// <summary>
        /// متوسط
        /// </summary>
        Medium = 1186,
        /// <summary>
        /// پایین
        /// </summary>
        Low = 1157,
        /// <summary>
        /// پایین ترین
        /// </summary>
        Lowest = 1158,
    }
    /// <summary>
    /// وضعیت مسئله
    /// </summary>
    public enum IssueStateType
    {
        /// <summary>
        /// شروع و تایید
        /// </summary>
        StartAndApprove = 1187,
        /// <summary>
        /// انجام دادن
        /// </summary>
        ToDo = 1160,
        /// <summary>
        /// در حال انجام
        /// </summary>
        IsDoing = 1161,
        /// <summary>
        /// به منظور بررسی
        /// </summary>
        ToVerify = 1162,
        /// <summary>
        /// در حال بررسی
        /// </summary>
        IsVerifying = 1163,
        /// <summary>
        /// انجام شده
        /// </summary>
        Done = 1164,
        /// <summary>
        /// بستن
        /// </summary>
        Close = 1165

    }
    /// <summary>
    /// وضعیت گروه بندی فاکتور
    /// base type id = 1063
    /// </summary>
    public enum FactorGroupStatusTypeEnum
    {
        // ثبت اولیه
        Initial = 1182,
        //ثبت نهایی
        Confirmed = 1183
    }
    /// <summary>
    /// وضعیت گروه بندی آیتم
    /// base type id = 1063
    /// </summary>
    public enum ItemGroupStatusTypeEnum
    {
        // ثبت اولیه
        Initial = 1207,
        //ثبت نهایی
        Confirmed = 1208
    }
    /// <summary>
    /// نام منابع مربوط به فاکتور
    /// </summary>
    public enum FactorResourceNameEnum
    {
        SupervisorFactor,
        FactorVerification
    }


    /// <summary>
    /// نام منابع مربوط به فاکتور
    /// </summary>
    public enum ResourceTypeEnum
    {
        List = 1,
        Edit = 2
    }
    /// <summary>
    /// نوع اصلاح آیتم سفارش : لغو سفارش یا تغییر سفارش
    /// </summary>
    public enum ModifiedOrderItemTypeEnum
    {
        // لغو سفارش
        CancelOrderItem = 1195,

        // تغییر سفارش
        ChangeOrderItem = 1196,

        // تغییر تعداد سفارش
        ModifyQuantity = 1201
    }
    /// <summary>
    /// وضعیت اصلاح آیتم سفارش : 
    /// ثبت توسط تامین کننده یا
    /// رد توسط کارشناس یا crm
    /// اصلاح سفارش انجام شد
    /// </summary>
    public enum ModifiedOrderItemStatusTypeEnum
    {
        // ثبت توسط تامین کننده
        RegisteredBySupplier = 1197,
        // تایید توسط کارشناس crm
        ApprovedByCrmExpert = 1198,
        // رد توسط کارشناس crm
        RejectedByCrmExpert = 1199,
        // اصلاح سفارش انجام شد
        Completed = 1200
    }
    public enum RunTypeEnum
    {
        Workshop = 31,//	کارگاه
        Event = 32,//	رويداد
        camp = 33,//	کمپ
        Class = 34,//	کلاسي
        SeminarConference = 35,//	سمينار/همايش
        Online = 36,//	آموزش از راه دور
        OtherItems = 37,//	ساير موارد
    }

    /// <summary>
    /// وضعیت پیام
    /// </summary>
    public enum ConversationStatusTypeEnum
    {
        /// <summary>
        /// ثبت اولیه
        /// </summary>
        InitialRegistration = 1260,
        /// <summary>
        /// ارسال شده
        /// </summary>
        Sent = 1261,
        /// <summary>
        /// پایان مکالمه
        /// </summary>
        Ended = 1290
    }
    /// <summary>
    /// نوع پیام
    /// </summary>
    public enum ConversationTypeEnum
    {
        /// <summary>
        ///پیام متنی
        /// </summary>
        TextMessage = 1262,
        /// <summary>
        /// ایمیل
        /// </summary>
        EmailMessage = 1263,
        /// <summary>
        /// Sms
        /// </summary>
        Sms = 1264

    }
    /// <summary>
    /// نوع صندوق پیام
    /// </summary>
    public enum ConversationBoxTypeEnum
    {
        /// <summary>
        ///صندوق دریافتی 
        /// </summary>
        Inbox = 1265,
        /// <summary>
        /// صندوق ارسالی
        /// </summary>
        SentBox = 1266,
        /// <summary>
        /// صندوق پیش نویس
        /// </summary>
        DraftBox = 1267

    }
    /// <summary>
    /// فرمت سندها
    /// </summary>
    public enum DocFormatEnum
    {
        /// <summary>
        /// تصویر
        /// </summary>
        Image = 1,
        /// <summary>
        /// pdf
        /// </summary>
        Pdf = 2,
        /// <summary>
        /// تصویر قابل برش
        /// </summary>
        CropImage = 3,
        /// <summary>
        /// فایل zip
        /// </summary>
        ZipArchive = 100,
        /// <summary>
        /// فیلم
        /// </summary>
        Movie = 103,
        /// <summary>
        /// Scorm
        /// </summary>
        Scorm = 106,
        /// <summary>
        /// سند office
        /// </summary>
        Office = 110
    }
    public enum QuestionTypeEnum
    {
        /// <summary>
        /// سوالات چند گزینه ای
        /// </summary>
        MultipleChoiceQuestion = 1268,
        /// <summary>
        /// سوالات تشریحی
        /// </summary>
        DescriptiveQuestion = 1269,
        /// <summary>
        /// سوالات ارسالی
        /// </summary>
        SubmissionBasedQuestion = 3137
    }
    /// <summary>
    /// 3122
    /// وضعیت درخواست پرداخت ترکیبی
    /// </summary>
    public enum HybridPaymentRequestStatusTypeEnum
    {
        /// <summary>
        /// ثبت اولیه
        /// </summary>
        InitialRegistration = 2275,

        /// <summary>
        /// درحال بررسی
        /// </summary>
        Processing = 2276,
        /// <summary>
        /// کل قیمت مربوط به کالا پرداخت شده باشد
        /// </summary>
        Complete = 2277,

        /// <summary>
        /// مرجوعی  :  سفارش کامل با پرداخت موفق لغو شده است
        /// انصراف بعد از تایید نهایی و پرداخت
        /// </summary>
        Refund = 2278,

        /// <summary>
        /// در صورتی که کاربر در صفحه پرداخت ترکیبی بود و دکمه انصراف را زده باشد  
        /// </summary>
        Canceled = 2279,

        /// <summary>
        /// لغو تراکنش توسط سیستم بدلیل گذشت مدت زمان مشخص و عدم پرداخت توسط کاربر 
        /// </summary>
        CancelBySystem = 2280,
        /// <summary>
        /// پرداخت ناموفق 
        /// </summary>
        Unsuccessful = 2281,

        /// <summary>
        /// پرداخت ناقص
        /// </summary>
        IncompletePayment = 2284
    }
    /// <summary>
    /// 3123
    /// وضعیت پرداخت در پرداخت ترکیبی
    /// باید تمامی استیت های payment را داشته باشد
    /// </summary>
    public enum HybridPaymentStatusTypeEnum
    {
        ///// <summary>
        ///// ثبت اولیه
        ///// </summary>
        // InitialRegistration = 2285,
        /// <summary>
        /// ارسال به درگاه پرداخت
        /// صفحه در گاه باز شده است
        /// </summary>
        SendToPaymentGateway = 2286,
        /// <summary>
        /// متد  درگاه باز شده است و کاربر مشخصات وارد کرده و متد
        /// verify
        /// سمت پذیرنده فرا خوانی شده است 
        /// </summary>
        WaitingResponseFromPaymentGateway = 2287,
        /// <summary>
        /// پرداخت با موفقیت انجام شد
        /// </summary>
        Paid = 2288,
        /// <summary>
        ///بازپرداخت جزئی
        /// </summary>
        //PartialRefund = 2289,
        /// <summary>
        /// بازپرداخت کل
        /// </summary>
        Refund = 2290,
        /// <summary>
        /// کاربر دکمه انصراف را در صفحه درگاه پرداخت میزند
        /// یا به صفحه درگاه را باز کرده و کاری انجام نداده است
        /// </summary>
        Cancel = 2291,
        /// <summary>
        /// پرداخت ناموفق 
        /// </summary>
        Unsuccessful = 2292,
    }

    public enum SystemNameEnum
    {
        ProductSalesPlanning
    }

    public enum CustomGenderTypeEnum
    {
        /// <summary>
        /// جنسیت مرد
        /// </summary>
        Male = 2,
        /// <summary>
        /// جنسیت زن
        /// </summary>
        Female = 3,
        /// <summary>
        /// جنسیت فرقی نمی کند
        /// </summary>
        WithoutGender = 4
    }
    /// <summary>
    /// برای بررسی نوع ولیدیشن نام کاربری که براساس کلید کانفیگ مشخص شده است استفاده می شود
    /// </summary>
    public enum UserNameValidationMode
    {
        /// <summary>
        /// بر اساس کد ملی
        /// </summary>
        ByNationalCode = 1,
        /// <summary>
        /// بر اساس شماره موبایل
        /// </summary>
        ByMobileNumber = 2,
        /// <summary>
        /// سایر روش ها
        /// </summary>
        ByOtherMethod = 3,
        /// <summary>
        /// بر اساس ایمیل
        /// </summary>
        ByEmail = 4
    }
    public enum RoleEnum
    {
        /// <summary>
        /// شناسه نقش فراگیر یا دانشجو
        /// </summary>
        Student = 1201,
        Admin = 98,
        EducationManager = 1203,
        SystemAdmin = 1204,
        Teacher = 1202,
        LocalAdmin = 103
    }
}