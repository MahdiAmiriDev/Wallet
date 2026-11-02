# راهنمای بیزینسی Wallet: اجرای دو سناریو و نقشهٔ متدها

مبنا: کد همین checkout در ۲۰۲۶-۱۱-۰۲. این سند راهنمای **رفتار پیاده‌شده** است؛ اجرای واقعی روی اپلیکیشن «شهرداری»، دیتابیس و درگاه در زمان نگارش انجام نشده است. در این سند «تخفیف Checkout» از «کوپن اعتبار کیف پول» جدا است. هیچ شماره کارت کامل، کد خام کوپن یا کلید امنیتی را در سند یا خروجی تست ثبت نکنید.

## پیش از شروع

1. در محیط تست شهرداری، مدیرِ دارای مجوز `WalletProgram`، کاربر `fa0912`، یک گیرندهٔ متعلق به او با کارت تست حکمت و یک محصول واقعی را آماده کنید. نام و شناسهٔ **واقعی** مرکز «ماز» و شهر «زاهدان» را در انتخابگر Scope تأیید کنید. نام گزینهٔ «نوع Program» از Lookup محیط می‌آید و از Source قابل تعیین قطعی نیست.
2. دو محصول/قلم سفارش برای کنترل Scope پیدا کنید: **مجاز** یعنی مرکز محصول **و** مرکز نمایندگی هر دو ماز و شهر آدرس نمایندگی زاهدان؛ **نامجاز** یعنی دست‌کم یکی از این سه شرط برقرار نباشد. شهر محل سکونت کاربر معیار این Scope نیست.
   Scope هنگام آماده‌سازی/رزرو سفارش سنجیده می‌شود؛ نمایش یک میلیون تومان در «کیف پول من» به‌تنهایی اجازهٔ خرج آن برای هر سفارش نیست.
3. مقدار `WalletSecurity:CardHmacKey` باید در محیط تست پیکربندی شده باشد. مقدارش را در این سند، درخواست‌ها یا Log قرار ندهید. ۵۰۰ کارت باید ۱۶رقمی، دارای checksum معتبر Luhn و یکتا در همان گروه باشند. کد، صادرکننده/حکمت‌بودن کارت را از BIN یا سرویس حکمت بررسی نمی‌کند؛ فهرست ۵۰۰ کارت را مالک دادهٔ حکمت باید تأیید کند.
4. برای آزمون مالی، Sandbox حکمت‌آنلاین یا وایب و امکان دیدن رکوردهای Payment/Reservation/Transaction در گزارش فقط‌خواندنی نیاز است. تست واقعی با پول/کارت واقعی انجام نشود.

## سناریو ۱: ۵۰۰ کارت، نفری ۱٬۰۰۰٬۰۰۰ تومان، ماز و زاهدان

**رفتار فعلی:** با روش `FixedForUser`، بودجهٔ `۵۰۰٬۰۰۰٬۰۰۰` و `TargetUserCount=۵۰۰`، سهم ثابت هر کاربر `۱٬۰۰۰٬۰۰۰` می‌شود. کارت‌های گروه فعال شرط Match هستند؛ Match از **کارت گیرندهٔ متعلق به خریدار واردشده** می‌خواند و اعتبار را به خریدار می‌دهد. هر خریدار در این Program حداکثر یک Grant می‌گیرد. بودجه سقف ۵۰۰ Grant یک‌میلیونی را می‌گذارد؛ رابطهٔ یک‌به‌یک کارت↔کاربر و محدودیت استفادهٔ انحصاری از هر کارت توسط یک خریدار در کد دیده نمی‌شود. همچنین اگر همان کاربر قبلاً از همین Program با مسیر دیگری اعتبار گرفته باشد، Match کارت می‌تواند Assignment موجود را بازیابی کند. برای الزام «دقیقاً همین ۵۰۰ شخص، هر کارت فقط یک شخص» نیاز به تصمیم/تغییر جداگانه است.

### الف) ساخت Program در پنل ادمین

مسیر: `walletPrograms` ← Program جدید. برای اجرای نمونه در تاریخ فعلی، بازهٔ زیر را بگذارید؛ اگر زمان اجرای واقعی از این بازه گذشته است، تاریخ آیندهٔ دقیق را قبل از ساخت ثبت کنید. **Program را پس از رسیدن زمان شروع فعال کنید**؛ فعال‌سازی پیش از شروع رد می‌شود.

| فیلد واقعی UI | مقدار تست |
|---|---|
| کد Program | `QA-MAZ-ZAH-500-20261102` (اگر قبلاً استفاده شده، پسوند یکتا) |
| عنوان | `اعتبار کارت حکمت ماز زاهدان ۵۰۰ نفر` |
| نوع Program | گزینهٔ متناسبِ موجود در Lookup شهرداری؛ مقدار انتخاب‌شده را در برگهٔ اجرا ثبت کنید |
| روش تخصیص | `FixedForUser` / سهم ثابت هر کاربر |
| کد ارز | `IRT`، پس از تأیید قرارداد ارز محیط شهرداری |
| بودجه کل | `۵۰۰٬۰۰۰٬۰۰۰` تومان |
| تعداد هدف کاربران | `۵۰۰` |
| مبلغ ثابت هر کاربر | `۱٬۰۰۰٬۰۰۰` تومان |
| شروع اعتبار | `۲۰۲۶-۱۱-۰۳ ۱۰:۰۰` به وقت نمایش UI |
| پایان اعتبار | `۲۰۲۶-۱۲-۰۳ ۱۰:۰۰` به وقت نمایش UI |
| توضیحات | `تست Wallet؛ ماز و زاهدان؛ ۵۰۰ کارت تأییدشده` |

1. «ذخیره» را بزنید. Program باید `Draft` باشد؛ `AllocatedAmount=۰` و `ReservedAmount=۰`.
2. در «محدوده مصرف Program» روی «افزودن محدوده» بزنید. ردیف اول: `عنوان Scope=QA-MAZ`، `نوع Scope=مرکز`، `هدف Scope=ماز`، `فعال=بله`، توضیحات خالی؛ «ثبت Scope».
3. ردیف دوم: `عنوان Scope=QA-ZAHEDAN`، `نوع Scope=شهر`، `هدف Scope=زاهدان`، `فعال=بله`، توضیحات خالی؛ «ثبت Scope». هر دو ردیف باید در جدول دیده شوند. بین این دو نوع شرط **AND** است و روی **همهٔ اقلام** سفارش اعمال می‌شود.
4. در «گروه کارت»، «گروه جدید» را بزنید: عنوان `QA-HEKMAT-500`، شروع `۲۰۲۶-۱۱-۰۳ ۱۰:۰۰`، پایان `۲۰۲۶-۱۲-۰۳ ۱۰:۰۰`؛ «ثبت گروه». گروه در `Draft` ساخته می‌شود.
5. گروه را انتخاب و در فیلد رمزدار «شماره کارت عضو» هر یک از **۵۰۰ کارت تستِ تأییدشده** را جداگانه وارد و «افزودن کارت» را بزنید. UI فعلی ورود یک‌به‌یک دارد؛ مسیر import گروهی در این بخش دیده نشد. پس از هر ثبت، فیلد پاک می‌شود و فهرست باید فقط مقدار ماسک‌شده نشان دهد. شمارهٔ تکراری در همان گروه باید رد شود. شمارندهٔ گروه پس از پایان دقیقاً `۵۰۰` باشد.
6. «فعال‌سازی گروه» را بزنید؛ وضعیت گروه `Active` شود. پس از زمان شروع Program، «فعال‌سازی / ادامه» را بزنید؛ Program `Active` شود. پیش از آزمون با ۵۰۰ کاربر، «بررسی مغایرت‌ها» را بزنید و نتیجه را ثبت کنید.

### ب) پایلوت با `fa0912` و آزمون مصرف

1. با `fa0912` وارد کلاینت شهرداری شوید. گیرنده‌ای که **مالک همین کاربر** است و کارت وی یکی از ۵۰۰ کارت ثبت‌شده است انتخاب کنید. اگر گیرنده/کارت قبلاً ثبت نشده، آن را از مسیر معمول کلاینت بسازید؛ در این سند شمارهٔ کارت ثبت نمی‌شود.
2. به «کیف پول من» بروید، از «تطبیق کارت گیرنده» همان گیرنده را انتخاب و «بررسی کارت» را بزنید. انتظار: Program بالا ظاهر شود و برای همان Program `Assigned=۱٬۰۰۰٬۰۰۰`، `Available=۱٬۰۰۰٬۰۰۰`، `Consumed=Reserved=Expired=۰` باشد. اجرای دوبارهٔ تطبیق نباید اعتبار را ۲ میلیون کند.
3. در پنل ادمین، ردیف Assignment همین کاربر را در Program پیدا کنید: Source `Card`، وضعیت `Active`، اعتبار ۱ میلیون؛ در «Grantهای Program» یک Grant یک‌میلیونی با کد داخلی `WalletCard:<ProgramId>:<UserId>`؛ `AllocatedAmount=۱٬۰۰۰٬۰۰۰`. هیچ PAN خام در رکورد یا خروجی مدیریتی نباشد.
4. با همین کاربر یک سفارش **فقط از قلم مجاز** بسازید. مبلغ نهایی قابل پرداخت را قبل از تأیید یادداشت کنید؛ برای تست Mixed باید `T>۱٬۰۰۰٬۰۰۰` باشد، مثلاً اگر محصول واقعی با قیمت نهایی `۱٬۵۰۰٬۰۰۰` در محیط موجود است، `W=۱٬۰۰۰٬۰۰۰` و سهم درگاه `G=۵۰۰٬۰۰۰` می‌شود. محصول/قیمت واقعی را جایگزین کنید؛ قیمت را حدس نزنید.
5. در Checkout، درگاه «حکمت‌آنلاین» یا «وایب»، گزینهٔ «کسر بخشی از مبلغ از کیف پول»، سپس Program ساخته‌شده را انتخاب کنید. «پرداخت و ثبت نهایی سفارش» را بزنید. پس از آماده‌سازی، انتظار: یک Payment با `Total=G` و یک Reservation با `Amount=W`، `ReservedAmount=W`، `Available=۰`؛ سفارش هنوز Complete نباشد. در Sandbox فقط `G` را پرداخت کنید.
6. پس از پاسخ قطعی موفق درگاه، Reservation باید `Captured`، Payment باید Paid، `ConsumedAmount=W`، `ReservedAmount=۰`، `Available=۰` و یک Ledger Capture منفی `−W` باشد. Order فقط بعد از Capture و بدهی صفر Complete شود. اگر درگاه قطعی ناموفق بود، Reservation باید Released و اعتبار قابل استفاده دوباره ۱ میلیون شود. نتیجهٔ نامعلوم نباید Release شود.
7. آزمایش منفی را با سفارش دارای قلم نامجاز از نظر ماز یا زاهدان تکرار کنید؛ Reserve باید رد شود و Payment/Reservation/مصرف جدید برای آن تلاش نباشد. آزمایش دیگر را با کارت خارج فهرست و کاربر تست جدا انجام دهید؛ نباید Grant بگیرد. کارت یکسان روی گیرندهٔ دو خریدار جدا را نیز در محیط کنترل‌شده امتحان کنید تا محدودیت بیزینسیِ ذکرشده آشکار شود.

### ج) اجرای کامل ۵۰۰ نفر

1. پیش از شروع، فهرست واقعی ۵۰۰ **کاربر** و نگاشت هرکدام به یک کارت از فهرست را خارج از سند، در مخزن امن دادهٔ تست تأیید کنید. خود Program چنین نگاشتی را نگهداری نمی‌کند. اگر هدف فقط ۵۰۰ کارت است، کد ممکن است به ۵۰۰ خریدار متفاوتِ دارای این کارت‌ها تا سقف بودجه Grant کند.
2. برای هر کاربر، Match را از حساب خودش اجرا کنید و نتیجهٔ همان Program را ثبت کنید. در ردیف ۱، ۲، ۴۹۹ و ۵۰۰ ماندهٔ ۱ میلیون و شمار Grantها را کنترل کنید. بعد از ۵۰۰ Grant موفق، `AllocatedAmount=۵۰۰٬۰۰۰٬۰۰۰` است. برای آزمون سقف بودجه، کاربر تست ۵۰۱ را با **یکی از همان ۵۰۰ کارت مجاز** و گیرندهٔ متعلق به خودش Match کنید؛ Grant او باید به علت بودجه رد شود. این آزمون هم‌زمان نشان می‌دهد یک‌به‌یک‌بودن کارت و کاربر در کد تضمین نشده است.
3. در پایان، «بررسی مغایرت‌ها» را بزنید؛ مجموع Assignmentها، Ledger Grant و `AllocatedAmount` باید با هم بخواند. در صورت هر اختلاف، تست Fail و اجرای گسترده متوقف شود.

## سناریو ۲: ۳۰۰ کد تخفیف در Checkout

**قرارداد تأییدشده توسط کاربر:** ۳۰۰ کد با **مبلغ تخفیف ثابت ۵۰۰٬۰۰۰ تومان برای هر کد**؛ هر کد حداکثر یک‌بار مصرف می‌شود و پس از مصرف به همان یک کاربر نسبت داده می‌شود. مجموع ارزش اسمی ۳۰۰ کد `۱۵۰٬۰۰۰٬۰۰۰` تومان است. این جمله دربارهٔ استفادهٔ یک کاربر از چند **کد متفاوت** حکمی ندارد؛ این محدودیت جداگانه باید تعیین شود.

**وضعیت فعلی: قابل اجرای دقیق به‌شکل درخواستی نیست.** در Checkout فعلی فیلد واردکردن کد تخفیف و Action محاسبهٔ دوبارهٔ مبلغ نهایی با کد پیدا نشد. `WalletCouponBatch` کدهای ۸رقمی تولید می‌کند، ولی مشتری آن‌ها را در «کیف پول من» Claim می‌کند؛ Claim فوراً **اعتبار کیف پول** می‌دهد و `Order.Total` را به‌عنوان تخفیف کاهش نمی‌دهد. `ProductSalesPlanningDiscount` درصد تخفیف مرتبط با باشگاه مشتریان است، نه ۳۰۰ کد یکتا برای Checkout. بنابراین نمی‌توان برای خواستهٔ فعلی گام «کد را در Checkout بزنید و از مبلغ نهایی کم شود» را به‌عنوان تست قابلیت موجود نوشت.

اگر منظورتان **کوپن اعتبار کیف پول** باشد، مسیر فعلی این است: Program با `FixedForUser` بسازید؛ بودجه را `۱۵۰٬۰۰۰٬۰۰۰`، تعداد هدف را `۳۰۰` و مبلغ ثابت هر کاربر را `۵۰۰٬۰۰۰` تومان بگذارید؛ در «Batch کوپن»، تعداد `۳۰۰` و مبلغ هر کد `۵۰۰٬۰۰۰` تومان؛ Batch را بسازید، ۳۰۰ کد خام ۸رقمی را فقط در لحظهٔ ساخت در مسیر امن دریافت و Batch را فعال کنید. هر کاربر در «کیف پول من» یک کد را Claim می‌کند و اعتبار می‌گیرد؛ هر کد برای یک Claim موفق مصرف می‌شود و هر کاربر در Program ثابت فقط یک Grant می‌گیرد. سپس در Checkout فقط در پرداخت Mixed با حکمت‌آنلاین/وایب از اعتبار استفاده می‌کند. این **تخفیف قیمت سفارش نیست** و برای خریدی که کل بدهی را Wallet پوشش دهد، مسیر Mixed فعلی `W=T` را رد می‌کند.

برای سناریوی تخفیف واقعی، پیش از تست اجرایی/پیاده‌سازی این قواعد باقیمانده مشخص شوند: آیا یک کاربر می‌تواند چند کد متفاوت مصرف کند؛ محدودیت محصولات/شرکت/شهر؛ رفتار سفارش کمتر از `۵۰۰٬۰۰۰` تومان؛ زمان رزرو کد در Checkout و آزادسازی پس از شکست پرداخت؛ callback تکراری، لغو و Refund؛ ترتیب اعمال تخفیف نسبت به Wallet و هزینه‌ها. معیار پایهٔ تست پس از پیاده‌سازی: برای سفارش با مبلغ قابل تخفیف `T>۵۰۰٬۰۰۰`، پس از ورود کد معتبر، مبلغ نهایی `T−۵۰۰٬۰۰۰` شود؛ مثلاً برای `T=۱٬۵۰۰٬۰۰۰`، مبلغ نهایی `۱٬۰۰۰٬۰۰۰` تومان است. پرداخت ناموفق مصرف قطعی کد نسازد؛ پس از پرداخت موفق همان کد برای همان کاربر یا کاربر دیگر دوباره پذیرفته نشود. تا آن زمان ۳۰۰ کد Wallet را به نام «کد تخفیف Checkout» توزیع نکنید.

## نقشهٔ سریع جریان بیزینسی

`Program(Draft) → Scope/گروه کارت → Program(Active) → Match کارت یا Claim کوپن → Assignment + Grant → Checkout/Eligibility → Prepare(Payment+Reservation) → Gateway → Capture یا Release → Refund/Expiry/Audit`

مبلغ آزاد Assignment: `Assigned − Consumed − Reserved − Expired`؛ `AllocatedAmount` Program مجموع Grantهای موفق است و نباید از `TotalAmount` بیشتر شود. Scopeها: AND بین نوع‌ها، OR بین چند مقدار از یک نوع؛ **همهٔ اقلام** باید مجاز باشند. در Mixed فقط حکمت‌آنلاین/وایب و `۰<W<T` پذیرفته می‌شود؛ `Payment.Total=G=T−W` و `Reservation.Amount=W`.

## وظیفهٔ متدهای بیزینسی Service

این جدول متدهای صرفاً فهرست/خواندن و CRUD عمومی Program، Scope و Assignment را حذف می‌کند. متدهایی با نام `Create` که **Grant مالی، تولید Batch یا ایجاد گروه امنیتی کارت** انجام می‌دهند، به‌دلیل اثر بیزینسی آمده‌اند. مسیر همهٔ Serviceهای Commerce زیر `backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/` است.

| سرویس / متد | وظیفه و اثر اصلی | نکتهٔ تغییر/تست |
|---|---|---|
| `WalletProgramService.ChangeStatusAsync` | گذار Draft/Active/Suspended/Closed/Expired؛ برای Active بودجه، تاریخ و سهم ثابت را اعتبارسنجی می‌کند؛ Closed با Reservation باز رد می‌شود. | تغییر قواعد Lifecycle همین‌جاست؛ گذار با RowVersion کنترل می‌شود. |
| `WalletProgramService.ExpireDueProgramsAsync` | Assignmentهای سررسید را پردازش و Program فعال/معلقِ پایان‌یافته را Expired می‌کند. | Job هر دقیقه آن را صدا می‌زند؛ نتیجهٔ نامعلوم درگاه با Reservation باز را زود منقضی نکنید. |
| `WalletProgramService.ExpireDueAssignmentAsync` / `ExpireAssignmentWithinTransactionAsync` | ماندهٔ آزاد را به Expired منتقل، Reservationهای مجاز را Expired و Ledger بدهکار انقضا ثبت می‌کند؛ در تراکنش Serializable. | انتظار ماندهٔ صفر و فقط یک Ledger انقضا؛ Payment باز/موفق باعث تعویق می‌شود. |
| `WalletEligibilityService.IsOrderEligibleAsync` | از OrderItem، مرکز نمایندگی و محصول، شهر آدرس نمایندگی، نمایندگی و محصول را می‌خواند؛ AND/OR و `items.All` را اعمال می‌کند. | برای شرط ماز+زاهدان نقطهٔ اصلی است؛ شهر کاربر در این منطق نیست. |
| `WalletGrantService.CreateAsync` / `CreateWithinTransactionAsync` | مبلغ مثبت و کلید تکرار را کنترل؛ بودجه Program و AssignedAmount را اتمیک افزایش و Grant Credit ثبت می‌کند. در FixedForUser مبلغ باید برابر سهم باشد و برای هر کاربر یک بار Grant شود. | Replay همان کلید بی‌اثر؛ کلید یکسان با دادهٔ دیگر تعارض. |
| `WalletBenefitsService.CreateCardGroupAsync` | گروه کارت را فقط روی Program Draft می‌سازد. | پیش از فعال‌سازی Program انجام دهید. |
| `WalletBenefitsService.AddCardAsync` | کارت ۱۶رقمی Luhn معتبر را نرمال، HMAC و ماسک می‌کند؛ عضو تکراری همان گروه را رد می‌کند. | صادرکنندهٔ حکمت را بررسی نمی‌کند؛ PAN خام را ذخیره نکنید. |
| `WalletBenefitsService.ActivateCardGroupAsync` | گروه Draft دارای عضو را Active می‌کند. | Match فقط گروه Active در بازه را می‌بیند. |
| `WalletBenefitsService.MatchRecipientCardAsync` | کارت گیرندهٔ متعلق به خریدار و Application جاری را با عضو گروه فعال تطبیق می‌دهد؛ Assignment را ایجاد/بازیابی می‌کند؛ برای FixedForUser، Grant یک‌باره می‌دهد و برای Manual فقط Assignment صفرمانده. | Grant به خریدار، نه گیرنده، تعلق دارد؛ شرط «کارت فقط برای یک خریدار» اینجا برقرار نیست. |
| `WalletBenefitsService.CreateCouponBatchAsync` | برای Program ثابتِ Draft/Active با مبلغ مساوی سهم کاربر، کدهای یکتای ۸رقمی می‌سازد؛ فقط Hash/Mask ذخیره و کد خام را در پاسخ نخست برمی‌گرداند. | Replay کد خام ندارد؛ `AttemptKey` هویت ساخت Batch است. |
| `WalletBenefitsService.ActivateCouponBatchAsync` / `CancelCouponBatchAsync` | Batch کاملِ Draft را در بازه و با Program فعال، Active می‌کند؛ یا Batch Draft با پاسخ گم‌شده و کدهایش را Cancelled می‌کند. | کد گم‌شده بازیابی نمی‌شود؛ Cancel و ایجاد Batch تازه. |
| `WalletBenefitsService.ClaimCouponAsync` | کد ۸رقمی را HMAC و بررسی می‌کند؛ Coupon Available/Batch Active را Claim و Grant را در یک تراکنش ثبت می‌کند؛ همان کاربر Replay می‌گیرد. | Claim قبل از Checkout است و قیمت Order را کم نمی‌کند. |
| `WalletBenefitsService.EnsureAssignmentAsync` / `GrantOnceAsync` | Assignment فعال یکتا را می‌سازد/بازیابی و Grant FixedForUser را با کنترل بودجه و Ledger دقیقاً یک بار انجام می‌دهد. | مسیر مشترک Card و Coupon؛ تغییرش هر دو را تحت اثر می‌گذارد. |
| `WalletOrderPreparationService.PrepareAsync` | برای Order خود کاربر و روش مجاز، Scope/مانده را بررسی؛ `W=min(T,Available)`، `G=T−W`؛ Payment و Reservation یک تلاش Mixed را در تراکنش/قفل مشترک می‌سازد یا همان تلاش را Replay می‌کند. | `۰<W<T`؛ برای G اعشاری رد می‌شود؛ Order با تلاش باز Conflict. |
| `WalletReservationService.ReserveAsync` | مسیر رزرو مستقل با Payment موجود؛ مالکیت Order/Assignment، Scope، مبلغ و مانده را می‌سنجد و ReservedAmount را بالا می‌برد. | کلید داخلی Paymentمحور است؛ `ExpiresAt` ایجاد+۳۰ دقیقه؛ Reserve Ledger مصرف نمی‌سازد. |
| `WalletSettlementService.CaptureAsync` / `SettleAsync` | فقط برای Payment Paid، Reservation کامل را Captured؛ Reserved را کم، Consumed را زیاد و Capture Debit منفی در Ledger ثبت می‌کند. | Replay کلید همان تراکنش را می‌دهد؛ Capture جزئی در این مسیر نیست. |
| `WalletSettlementService.ReleaseAsync` / `SettleAsync` | فقط پس از Payment قطعی `Unsuccessful/Cancel/Void`، Reservation را Released و ReservedAmount را آزاد می‌کند؛ Ledger مصرف نمی‌سازد. | timeout/نتیجهٔ نامعلوم اجازه Release نمی‌دهد. |
| `WalletRefundService.RefundAsync` / `RefundWithinTransactionAsync` | برای Capture معتبر، Refund کامل یا جزئی را تا سقف مبلغ Capture اعمال؛ Consumed را کم و Ledger Credit فرزند Capture می‌افزاید. | API سهم Wallet را برمی‌گرداند؛ Refund درگاه/Order را به‌تنهایی انجام نمی‌دهد. |
| `CustomerWalletBalanceService.GetCurrentUserBalanceAsync` | ماندهٔ Programهای کاربر جاری را بدون اثر مالی می‌سازد؛ اعتبار قابل استفاده فقط برای Program/Assignment فعال و در بازه. | UI «کیف پول من» و Lookup برنامهٔ Checkout از آن تغذیه می‌شوند. |
| `WalletAuditService.AuditProgramAsync` | Assignment، Reservation و Ledger همان Application را با جمع‌ها و وضعیت‌ها تطبیق و مغایرت‌ها را گزارش می‌کند. | قبل/بعد از اجرای ۵۰۰ نفر استفاده کنید؛ گزارش صرفاً خواندنی است. |

### Helperهای مؤثر بر قواعد بیزینسی

| محل / متد | نقش |
|---|---|
| `WalletProgramService.ValidFixedAllocation` | سهم ثابت را با تقسیم بودجه بر تعداد هدف و بریدن تا دو رقم اعشار مقایسه می‌کند؛ برای ۵۰۰ نفر و ۵۰۰ میلیون، دقیقاً ۱ میلیون. |
| `WalletProgramService.HasDateConflictAsync` / `AllowedTransition` | کوتاه‌کردن بازه در حضور تخصیص/رزرو و گذار غیرمجاز وضعیت را منع می‌کنند. |
| `WalletScopeService.ValidateAsync` / `LockDraftProgramAsync` | وجود هدف Scope در Application و تک‌هدف‌بودن هر ردیف را بررسی؛ تغییر Scope را به Draft محدود می‌کنند. |
| `WalletBenefitsService.ActiveProgramAsync` | Program فعال، متعلق به Application و در بازه را برای Card/Coupon پیدا می‌کند. |
| `WalletBenefitsService.LockAsync` | قفل SQL Server برای گروه/کد/کاربر در تراکنش؛ محافظ رقابت Claim و Match. |
| `WalletBenefitsService.TryKey` / `Fingerprint` / `SameHash` / `TryNormalizeCard` | کلید HMAC معتبر را می‌گیرند، Hash و مقایسهٔ امن انجام می‌دهند و کارت ۱۶رقمی Luhn را نرمال می‌کنند. |
| `WalletOrderPreparationService.LockAsync` / `FindAttemptAsync` / `ReplayOrConflict` | تلاش و Order را قفل، تلاش قبلی را پیدا و تطابق درخواست تکراری را کنترل می‌کنند. |
| `WalletReservationService.Matches` / `Replay` | رزرو قبلی را با Payment، Assignment، کاربر و مبلغ تطبیق و پاسخ تکراری می‌سازند. |
| `WalletSettlementService.SettleWithinTransactionAsync` | وضعیت Payment/Reservation، سازگاری Ledger و تغییر مانده‌های Program/Assignment را در تراکنش نهایی کنترل می‌کند. |
| `WalletRefundService.RefundWithinTransactionAsync` | مجموع Refundهای پیشین Capture را می‌سنجد و Refund جدید را تا سقف باقی‌مانده ثبت می‌کند. |
| `WalletAuditService.CheckAmount` / `AddIssue` | اختلاف مبلغ یا رابطهٔ مالی را به خروجی ممیزی اضافه می‌کنند؛ خودشان داده را اصلاح نمی‌کنند. |

متدهای `Result`/`Failure` فقط DTO پاسخ می‌سازند و متدهای `GetAll`/`GetAsync`/`FindAsync` و عملیات CRUD عمومی در این سند شرح داده نشده‌اند.

## Actionهای بیزینسی Controller و اتصال درگاه

Actionهای صرفاً GET فهرست/جزئیات و CRUD عمومی اینجا نیامده‌اند. مسیر Controllerهای ادمین: `backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/`؛ کلاینت: `.../Areas/ECommerce/Controllers/`.

| Controller / Action | نقش در جریان |
|---|---|
| `WalletProgramController.ChangeStatus` | درخواست گذار وضعیت را با مجوز مدیریتی به Service می‌دهد. |
| `WalletGrantController.PostEntity` | درخواست Grant مالی را به Service می‌سپارد و خطا/تعارض را به پاسخ HTTP تبدیل می‌کند. |
| `WalletCardController.CreateGroup` / `AddCard` / `ActivateGroup` | ساخت گروه Draft، ثبت کارت امن، فعال‌سازی گروه؛ برای سناریوی ۵۰۰ کارت. |
| `WalletCouponBatchController.Create` / `Activate` / `Cancel` | تولید Batch با `Cache-Control: no-store`، فعال‌سازی پس از دریافت کدها، لغو Draft. |
| `MyWalletController.MatchRecipientCard` | فرم «کیف پول من» را به Card Match و نمایش پیام وصل می‌کند. |
| `MyWalletController.MatchCheckoutRecipientCard` | Card Match در Checkout؛ نتیجه و فهرست Programهای قابل انتخاب را JSON می‌دهد. |
| `MyWalletController.ClaimCoupon` | Claim کد کوپن در «کیف پول من»؛ در Checkout فراخوانی نمی‌شود. |
| `MyWalletController.Programs` / `Index` | برنامه‌های قابل انتخاب Mixed و ماندهٔ فعلی کاربر را نشان می‌دهد؛ تغییر مالی ندارد، ولی نقطهٔ ورود بیزینسی UI است. |
| `OrderController` (AdminApi)، شاخهٔ `SaveShoppingCartToOrder` | هنگام انتخاب Wallet، ابتدا Match کارت گیرنده و سپس `PrepareAsync` را صدا می‌زند؛ فقط برای Vibe/HekmatOnline و بدون نیاز به تأیید یارانه، به درگاه هدایت می‌کند. |
| `WalletController.Reserve` | API آماده‌سازی مستقل Mixed؛ فقط Vibe/HekmatOnline را می‌پذیرد و `PaymentId/ReservationId/RedirectUrl` می‌دهد. |
| `WalletSettlementController.Capture` / `Release` | نقاط مدیریتی تسویهٔ Reservation؛ اعتبارسنجی مالی در Service است. |
| `WalletRefundController.PostEntity` | Refund سهم Wallet بر اساس CaptureTransactionId، مبلغ و Code. |
| `WalletAuditController.GetEntity` | گزارش مغایرت Program را برای بررسی مدیریتی برمی‌گرداند. |

در ماژول Payment، `VibeService.CreateOrderAsync` از Payment آماده‌شدهٔ Mixed استفاده می‌کند؛ `VerifyVibeOrderAsync` به `VerifyMixedAsync` و سپس `FinishMixedCaptureAsync` می‌رسد. در حکمت، `HekmatOnlinePaymentGatewayService.GenerateMixedTokenAsync` تلاش Mixed موجود را به توکن وصل می‌کند؛ `VerifyPaymentTransactionAsync`/`VerifyMixedAsync` نتیجه را بررسی و `FinishMixedCaptureAsync` سهم Wallet را Capture می‌کند. تغییر ترتیب `Paid → Capture → بدهی صفر → Complete` یا منطق نتیجهٔ نامعلوم باید در همین شاخه‌ها و Serviceهای Settlement با هم بررسی شود.

`WalletPaymentController` و Action قدیمی `OrderController.PayWithWallet` نیز در Source وجود دارند، اما مسیر پرداخت `WalletProgram/Reservation` مورد بحث این سند، شاخهٔ Mixed بالاست. پیش از تغییر آن Action قدیمی، مدل و منطق `CustomerClub` را جدا بررسی کنید.

## مسیرهای اصلی برای کدخوانی

- UI ادمین: `clientApp/ecommerce/wallet-program/wallet-program-detail.component.html` و `.ts`.
- UI کلاینت: `backEnd/Yashil.WebHost/Views/MyWallet/Index.cshtml` و `Views/Order/_OrderSummaryForPay.cshtml`.
- Scope/مالی/مزایا: فایل‌های هم‌نام Serviceهای جدول در `backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/`.
- Controllerهای Wallet: `backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/Wallet*.cs` و `Areas/ECommerce/Controllers/MyWalletController.cs`.
- Gateway: `backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs` و `HekmatOnlinePaymentGatewayService.cs`.
