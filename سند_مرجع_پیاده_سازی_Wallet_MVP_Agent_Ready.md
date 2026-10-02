# سند مرجع پیاده‌سازی Wallet MVP — Agent Ready
## هدف
این سند منبع مرجع برای درک کامل کسب‌وکار، مدل داده، قوانین، جریان‌ها و قرارداد پیاده‌سازی Wallet در فروشگاه آنلاین است. MVP شامل Program سازمانی، Card Group/Match و Coupon هشت‌رقمی است.
## مدل اصلی
`WalletProgram -> WalletScope -> WalletAssignment -> WalletReservation -> WalletTransaction`
`WalletProgram -> WalletCardGroup -> WalletCardMember`
`WalletProgram -> WalletCouponBatch -> WalletCoupon`
## جداول
### WalletProgram
- `Id` (bigint): کلید یکتا و داخلی Program.
- `Code` (nvarchar(50)): شناسه قابل گزارش و یکتا؛ مثال ORG-1405-001.
- `Title` (nvarchar(200)): عنوان مدیریتی Program.
- `ProgramType` (int): نوع Program؛ Organization، CardBased، CouponBased یا Mixed.
- `TotalAmount` (decimal(18,2)): بودجه کل Program.
- `AllocatedAmount` (decimal(18,2)): جمع اعتبار Grant/Assignment شده به کاربران.
- `ConsumedAmount` (decimal(18,2)): مصرف خالص قطعی فعلی؛ Capture افزایش و Refund کاهش می‌دهد.
- `ReservedAmount` (decimal(18,2)): جمع مبالغ Reservationهای باز.
- `ExpiredAmount` (decimal(18,2)): اعتبار منقضی‌شده برای گزارش و کنترل.
- `CurrencyCode` (char(3)): واحد پول واحد سامانه؛ مثال IRR.
- `AllocationMethod` (int): روش تخصیص مستقیم؛ FixedPerUser، EqualSplit یا Manual.
- `StartDate` (datetime2): زمان شروع اعتبار.
- `EndDate` (datetime2 nullable): زمان پایان اعتبار.
- `Status` (int): Draft، Active، Suspended، Closed.
- `Description` (nvarchar(1000)): توضیحات مدیریتی.
- `CreatedBy` (int): کاربر ثبت‌کننده.
- `CreationDate` (datetime2): زمان ثبت.
- `ModificationDate` (datetime2 nullable): آخرین ویرایش.
- `RowVersion` (rowversion): کنترل همروندی برای تغییرات حساس.

### WalletScope
- `Id` (bigint): شناسه Scope.
- `WalletProgramId` (bigint): Program مربوط.
- `ScopeType` (int): Company، Representation، City یا Product.
- `CompanyId` (int nullable): مرکز هدف در Scope مرکز یا محدودسازی شهر.
- `RepresentationId` (int nullable): نمایندگی هدف.
- `CityId` (int nullable): شهر هدف.
- `ProductId` (int nullable): محصول هدف.
- `IsActive` (bit): فعال یا غیرفعال بودن Rule.
- `CreatedBy` (int): ثبت‌کننده.
- `CreationDate` (datetime2): زمان ثبت.

### WalletAssignment
- `Id` (bigint): شناسه Assignment.
- `WalletProgramId` (bigint): Program منبع اعتبار.
- `UserId` (int): کاربر مالک اعتبار.
- `AssignedAmount` (decimal(18,2)): مبلغ اولیه اعتبار.
- `ConsumedAmount` (decimal(18,2)): مصرف قطعی از همین Assignment.
- `ReservedAmount` (decimal(18,2)): مبلغ رزروشده در سفارش‌های باز.
- `AvailableAmount` (computed): Assigned - Consumed - Reserved - Expired.
- `ExpiredAmount` (decimal(18,2)): اعتبار منقضی‌شده.
- `StartDate` (datetime2): شروع اعتبار Assignment.
- `EndDate` (datetime2 nullable): پایان اعتبار Assignment.
- `Status` (int): Active، Suspended، Exhausted، Cancelled.
- `SourceType` (int): Organization، Card، Coupon یا Manual.
- `SourceId` (bigint nullable): شناسه منبع ساختاریافته، مثل CardMemberId یا CouponId.
- `SourceRef` (nvarchar(100) nullable): مرجع متنی Trace، مثل COUPON-812.
- `CreatedBy` (int): ثبت‌کننده.
- `CreationDate` (datetime2): زمان ثبت.
- `RowVersion` (rowversion): کنترل موجودی.

### WalletReservation
- `Id` (bigint): شناسه Reservation.
- `WalletAssignmentId` (bigint): Assignment منبع.
- `UserId` (int): کاربر صاحب اعتبار.
- `OrderId` (int): سفارش.
- `Amount` (decimal(18,2)): مبلغ رزروشده.
- `Status` (int): Reserved، Captured، Released، Expired.
- `IdempotencyKey` (nvarchar(100)): کلید یکتای Reserve/عملیات.
- `ExpiresAt` (datetime2): پایان مهلت رزرو.
- `CreatedAt` (datetime2): زمان ایجاد.
- `CapturedAt` (datetime2 nullable): زمان قطعی شدن.
- `ReleasedAt` (datetime2 nullable): زمان آزاد شدن.
- `RowVersion` (rowversion): کنترل وضعیت Reservation.

### WalletTransaction
- `Id` (bigint): شناسه Ledger.
- `WalletAssignmentId` (bigint): Assignment مربوط.
- `UserId` (int): کاربر.
- `OrderId` (int nullable): سفارش مرتبط.
- `OrderItemId` (int nullable): آیتم سفارش در صورت نیاز به مصرف سطح آیتم.
- `ReservationId` (bigint nullable): Reservation مرتبط.
- `TransactionType` (int): Grant، Capture، Refund، Expire، Adjustment.
- `Amount` (decimal(18,2)): مبلغ علامت‌دار: Credit مثبت و Debit منفی.
- `PreviousBalance` (decimal(18,2) nullable): مانده قبل از رخداد برای Audit سریع.
- `NewBalance` (decimal(18,2) nullable): مانده بعد از رخداد.
- `ExternalReferenceType` (nvarchar(50) nullable): نوع مرجع بیرونی مثل PaymentGateway.
- `ExternalReferenceId` (nvarchar(100) nullable): شناسه سیستم بیرونی.
- `OccurredAt` (datetime2): زمان قطعی وقوع رخداد Wallet.
- `CreatedAt` (datetime2): زمان ثبت رکورد.
- `Description` (nvarchar(500) nullable): توضیح.
- `CreatedBy` (int nullable): عامل ثبت؛ System/کاربر.

### WalletCardGroup
- `Id` (bigint): شناسه گروه کارت.
- `WalletProgramId` (bigint): Program مرتبط.
- `Title` (nvarchar(200)): عنوان گروه.
- `StartDate` (datetime2): شروع اعتبار گروه.
- `EndDate` (datetime2 nullable): پایان اعتبار گروه.
- `Status` (int): Draft، Active، Suspended، Closed.
- `CreatedBy` (int): ثبت‌کننده.
- `CreationDate` (datetime2): زمان ثبت.

### WalletCardMember
- `Id` (bigint): شناسه عضو کارت.
- `WalletCardGroupId` (bigint): گروه کارت.
- `CardFingerprint` (char(64)): HMAC شماره کارت نرمال‌شده؛ برای Match امن.
- `MaskedCard` (varchar(19)): نمایش محدود مانند **** **** **** 1234.
- `Status` (int): Active، Blocked، Closed.
- `CreatedAt` (datetime2): زمان ثبت.

### WalletCouponBatch
- `Id` (bigint): شناسه Batch.
- `WalletProgramId` (bigint): Program مقصد.
- `Title` (nvarchar(200)): عنوان کمپین.
- `Quantity` (int): تعداد کد؛ مثال 100.
- `CodeLength` (tinyint): در MVP دقیقاً 8 رقم.
- `AmountPerCoupon` (decimal(18,2)): اعتبار هر کد؛ مثال 10M.
- `StartDate` (datetime2): شروع اعتبار.
- `EndDate` (datetime2 nullable): پایان اعتبار.
- `Status` (int): Draft، Active، Closed، Cancelled.
- `CreatedBy` (int): ثبت‌کننده.
- `CreatedAt` (datetime2): زمان ایجاد.

### WalletCoupon
- `Id` (bigint): شناسه کد.
- `WalletCouponBatchId` (bigint): Batch مادر.
- `CodeHash` (char(64)): HMAC/Hash کد برای Match.
- `MaskedCode` (varchar(12)): نمایش محدود کد.
- `UserId` (int nullable): کاربر Claim کننده.
- `Status` (int): Available، Claimed، Expired، Cancelled.
- `ClaimedAt` (datetime2 nullable): زمان Claim.
- `CreatedAt` (datetime2): زمان تولید.

## Transaction Types
`Grant`, `Capture`, `Refund`, `Expire`, `Adjustment`. `Reserve` و `Release` Transaction قطعی نیستند و در Reservation مدیریت می‌شوند. `Status` و `Direction` در WalletTransaction وجود ندارند. `Amount` علامت‌دار است.
## Statuses
- Program: Draft/Active/Suspended/Closed
- Assignment: Active/Suspended/Exhausted/Cancelled
- Reservation: Reserved/Captured/Released/Expired
- CardGroup: Draft/Active/Suspended/Closed
- CardMember: Active/Blocked/Closed
- CouponBatch: Draft/Active/Closed/Cancelled
- Coupon: Available/Claimed/Expired/Cancelled
## Duplicate Policy
فقط یک Assignment فعال برای `(UserId, WalletProgramId)`. اگر Card و Coupon هر دو یک Program را Eligibility کنند، Assignment دوم ایجاد نشود.
## Coupon
`Quantity=100`, `CodeLength=8`, `AmountPerCoupon` قابل تنظیم. کد با RNG امن تولید و CodeHash یکتا ذخیره می‌شود. Claim اتمیک است. Coupon پس از Claim در `Claimed` باقی می‌ماند؛ مصرف واقعی در Assignment/Transaction ثبت می‌شود.
## Card
PAN خام در Wallet ذخیره نمی‌شود. شماره Normalize و سپس HMAC-SHA256 می‌شود. `CardFingerprint` برای Match و `MaskedCard` برای نمایش است.
## Financial Flow
`Eligibility -> Reserve -> Payment -> Success/Capture OR Failure/Release`. `Capture` یک Ledger Debit قطعی ایجاد می‌کند. `Refund` یک Ledger Credit جدید ایجاد می‌کند.
## Invariants
1. Available منفی نشود.
2. Allocated از Total بیشتر نشود.
3. Reserved+Consumed+Expired از Assigned بیشتر نشود.
4. Reserve/Capture/Release/Claim idempotent باشند.
5. Concurrency از double-spend جلوگیری کند.
6. Ledger حذف/overwrite نشود.
## Implementation Services
WalletProgramService, WalletScopeService, WalletAssignmentService, WalletEligibilityService, WalletCardService, WalletCouponService, WalletReservationService, WalletSettlementService, WalletLedgerService.
## MVP Decision Log
- WalletTransaction.Status: حذف.
- WalletTransaction.Direction: حذف.
- WalletTransaction.CorrelationId: حذف در MVP.
- WalletCoupon.Used: حذف.
- WalletCardMember.ValidUntil و LastMatchedAt: حذف در MVP مگر نیاز جدید.
- `SourceType + SourceId` مرجع ساختاریافته، `SourceRef` فقط Trace.
## Integration Assumption
`UserId`, `OrderId`, `OrderItemId`, `CompanyId`, `RepresentationId`, `CityId`, `ProductId`, `CreatedBy` در جداول موجود پروژه integer هستند. FKهای خارجی تا زمان دیدن Schema واقعی نباید حدس زده شوند.
