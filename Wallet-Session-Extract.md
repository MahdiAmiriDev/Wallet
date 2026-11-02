## user
# AGENTS.md instructions for P:\Projects\walletProject\Wallet\backEnd

<INSTRUCTIONS>
# Codex Repository Guide

## Purpose
This repository contains the Wallet work built on an existing Yashil commerce application. Treat the existing code and repository documentation as the source of truth; do not invent missing architecture or database relationships.

## Start here
- `docs/codex/README.md` — Codex map and when to read each document.
- `docs/codex/architecture.md` — observed backend structure and boundaries.
- `docs/codex/frontend.md` — observed Angular structure and conventions.
- `docs/codex/wallet.md` — Wallet domain facts and invariants.
- `سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md` — detailed Wallet business reference.
- `docs/specifications/TEMPLATE.md` — template for new feature specifications.
- `.agents/skills/` — reusable task workflows; load only the relevant skill.

## Working rules
1. Inspect the existing implementation and a similar feature before changing code.
2. Prefer existing project patterns over introducing new abstractions or libraries.
3. Read only documentation relevant to the current task; do not load the whole knowledge base by default.
4. Keep changes scoped to the requested feature. Avoid unrelated refactoring.
5. Do not guess external/missing modules, schema relationships, configuration values, or business rules. State uncertainty and inspect references first.
6. For financial/wallet changes, preserve idempotency, concurrency protection, ledger integrity, and the documented Wallet invariants.
7. Never store or expose secrets, credentials, raw card PANs, or sensitive payment data in code or documentation.
8. Before finishing, build/test the affected area when the repository provides a valid command for it, then inspect the diff for unrelated changes.

## Planning
For non-trivial work, first produce a concise implementation plan identifying affected files, existing patterns, risks, and verification steps. Do not implement until the task's required approval point is satisfied.

## Scope note
The supplied archive references additional Yashil modules from `Yashil.WebHost`, but those modules are not present in the supplied source archive. Do not assume their implementation details; treat them as external dependencies unless their source is later provided.

## Current technology signals
- Backend: .NET 8 / ASP.NET Core / EF Core / SQL Server / gRPC in the supplied projects.
- Backend Wallet code is currently integrated mainly through `Yashil.Core` entities and `YashilCommerce` Core/Infrastructure/Web layers.
- Frontend: Angular/TypeScript with existing shared base components and feature modules.

## What not to do by default
Do not introduce MCP, multi-agent orchestration, SDK automation, headless workflows, or other advanced Codex infrastructure unless explicitly requested.

</INSTRUCTIONS>
<environment_context>
  <cwd>P:\Projects\walletProject\Wallet\backEnd</cwd>
  <shell>powershell</shell>
  <current_date>2026-10-02</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user

# Files pasted by the user:

## "# مرحله اول — شناخت پروژه (Project Onboarding) تو در این مرحله به عنوان یک **Se…": C:\Users\pc\.codex/attachments/d82d1a7b-a7bf-42dd-a639-77b5f4f33372/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن پیوست را می‌خوانم و سپس ساختار و مستندات مرتبط مخزن را بررسی می‌کنم تا خروجی مرحلهٔ شناخت پروژه بر پایهٔ شواهد موجود باشد.

---

## assistant
راهنمای مخزن و نقشه‌های معماری را خواندم. اکنون نمونه‌های واقعی Backend و Angular و بخش‌های کلیدی سند Wallet را با آن‌ها تطبیق می‌دهم؛ وضعیت Git فعلاً پاک است.

---

## assistant
یک اختلاف مهم پیدا شد: سند Wallet جریان کامل Reserve/Capture و سرویس‌های اختصاصی را تعریف می‌کند، اما در کدهای بررسی‌شده فعلاً Entityهای Wallet دیده می‌شوند و سرویس‌های نام‌برده‌شده پیدا نشدند. این را در گزارش به‌عنوان فاصلهٔ «طرح مستند» و «پیاده‌سازی قابل مشاهده» ثبت می‌کنم.

---

## assistant
# Project Understanding Report

### 1. نمای کلی معماری

**واقعیت:** مخزن شامل `backEnd` و `clientApp` است. Backend موجود از [Yashil.Core](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Yashil.Core.csproj)، سه لایهٔ Commerce یعنی [Core](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/YashilCommerce.Core.csproj)، [Infrastructure](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/YashilCommerce.Infrastructure.csproj) و [Web](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/YashilCommerce.Web.csproj)، و [WebHost](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Yashil.WebHost.csproj) تشکیل شده است. پروژه‌ها `net8.0` هستند. WebHost به Commerce و چند ماژول دیگر ارجاع می‌دهد.

**برداشت مبتنی بر کد:** مسیر رایج Feature در Commerce، قرارداد در Core، پیاده‌سازی در Infrastructure و Controller و ViewModel در Web است؛ مرز لایه‌ها کاملاً مستقل نیست، چون Web مستقیماً به Infrastructure نیز ارجاع دارد.

### 2. Backend

**واقعیت:** `Yashil.Core` محل Entityها و مدل‌های مشترک است. نمونهٔ Commerce، [IHekmatTransactionService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IHekmatTransactionService.cs) و [HekmatTransactionService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/HekmatTransactionService.cs) است. Repositoryها از EF Core و `YashilAppDbContext` استفاده می‌کنند؛ WebHost آن را با SQL Server و `IUnitOfWork` ثبت می‌کند ([Startup.cs](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:465)). ثبت سرویس‌های Commerce در [ModuleInitializer.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs) انجام می‌شود.

**واقعیت:** Controller نمونه از `BaseController`، AutoMapper، ViewModel، اعتبارسنجی سفارشی و `[YashilAuthorization]` استفاده می‌کند ([CategoryController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/CategoryController.cs)). WebHost احراز هویت، مجوزدهی، رسیدگی به خطا، Serilog و gRPC را پیکربندی می‌کند. یک نمونهٔ Job مبتنی بر Quartz نیز وجود دارد ([UpdateExpiredOrderStatusJob.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/JobImpl/UpdateExpiredOrderStatusJob.cs))؛ ثبت و اجرای آن از این نمونه به‌تنهایی قابل تأیید نیست.

### 3. Frontend

**واقعیت:** Angular به Featureهایی مانند `payment` و `ecommerce` و بخش `shared` تقسیم شده است. هر Feature ماژول، Route و Componentهای فهرست/جزئیات دارد؛ Routeهای نمونه با `AuthGuard` محافظت شده‌اند ([payment-routing.module.ts](P:/Projects/walletProject/Wallet/clientApp/payment/payment-routing.module.ts)). Component فهرست از `Selectable` و `BaseList` استفاده می‌کند ([payment-method-list.component.ts](P:/Projects/walletProject/Wallet/clientApp/payment/payment-method/payment-method-list.component.ts)). ارتباط API از `HttpClient` و سرویس عمومی `/api` انجام می‌شود ([generic-data.service.ts](P:/Projects/walletProject/Wallet/clientApp/shared/base/services/generic-data.service.ts)).

### 4. Conventionهای مهم توسعه

**واقعیت:** طبق [AGENTS.md](P:/Projects/walletProject/Wallet/AGENTS.md)، پیش از تغییر باید نمونهٔ مشابه بررسی شود، الگوهای موجود رعایت شوند، تغییر محدود به Feature بماند و برای کار غیرساده ابتدا طرح اجرا ارائه شود. برای Feature جدید، [قالب Specification](P:/Projects/walletProject/Wallet/docs/specifications/TEMPLATE.md) بخش‌های رفتار فعلی، قواعد، اثر بر داده/API/UI و معیار پذیرش را مشخص می‌کند. در کد، قرارداد سرویس/Repository، ثبت DI، ViewModel، AutoMapper و اعتبارسنجی Controller الگوهای قابل مشاهده‌اند.

### 5. Wallet Domain

**واقعیت مستند:** مدل MVP شامل `WalletProgram → WalletScope → WalletAssignment → WalletReservation → WalletTransaction` و شاخه‌های CardGroup/CardMember و CouponBatch/Coupon است. جریان تعریف‌شده `Eligibility → Reserve → Payment → Capture` در موفقیت یا `Release` در شکست است؛ Refund باید یک Credit جدید در Ledger بسازد. سند، Coupon هشت‌رقمی با Claim اتمیک و تطبیق کارت با HMAC و مقدار ماسک‌شده را تعریف می‌کند ([مرجع Wallet](P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md)).

**واقعیت کد:** Entityهای این مدل در [Entities](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletProgram.cs) وجود دارند، اما سرویس‌های MVP نام‌برده‌شده در سند در Commerce پیدا نشدند. [WalletPaymentController](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/WalletPaymentController.cs) به پرداخت مبتنی بر CustomerClubCard مربوط است؛ شواهدی برای یکسان دانستن آن با جریان MVP وجود ندارد. قاعدهٔ مشخصی برای ارتباط Prepayment یا Discount با Wallet MVP در منابع بررسی‌شده تأیید نشد.

### 6. Testing

**واقعیت:** Test Project برای .NET و فایل Solution در نسخهٔ موجود پیدا نشد. چند فایل Angular `*.spec.ts` با الگوی Jasmine/TestBed دیده می‌شود، از جمله [generic-detail.service.spec.ts](P:/Projects/walletProject/Wallet/clientApp/shared/base/services/generic-detail.service.spec.ts). `package.json`، `angular.json` و پیکربندی اجرای تست Frontend در مسیرهای بررسی‌شده پیدا نشد؛ بنابراین دستور تست قابل اتکایی برای این نسخه مشخص نیست. به‌دلیل خواندنی بودن این مرحله، Build یا Test اجرا نشد.

### 7. محدودیت‌ها و Invariantهای مهم

**واقعیت مستند:** ماندهٔ قابل استفاده نباید منفی شود؛ تخصیص نباید از بودجهٔ Program فراتر رود؛ مجموع رزروشده، مصرف‌شده و منقضی‌شده نباید از Assignment بیشتر شود. Reserve، Capture، Release و Claim باید idempotent باشند، همروندی باید جلوی خرج دوباره را بگیرد، Ledger نباید حذف یا بازنویسی شود و برای هر `(UserId, WalletProgramId)` فقط یک Assignment فعال مجاز است. PAN خام و دادهٔ حساس پرداخت نباید ذخیره شود ([wallet.md](P:/Projects/walletProject/Wallet/docs/codex/wallet.md)). **ناشناخته:** اجرای این قیود در پایگاه داده یا سرویس‌ها با کد موجود تأیید نشده است.

### 8. اختلاف مستندات و کد

**واقعیت:** مرجع MVP برای شناسهٔ اصلی Wallet نوع `bigint` و برای `WalletTransaction` مبلغ علامت‌دار بدون `Direction` تعریف می‌کند؛ Entityهای فعلی شناسهٔ `int` دارند و [WalletTransaction.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:67) دارای `WalletTransactionDirectionType` است. [WalletAssignment.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletAssignment.cs) نیز `SourceId` و `SourceRef` مستند را ندارد. سند، سرویس‌های Wallet و جریان تسویه را شرح می‌دهد، اما پیاده‌سازی آن‌ها در Commerce موجود مشاهده نشد. این اختلاف‌ها نیازمند تعیین تکلیف در طرح Feature بعدی هستند.

### 9. Unknowns / Missing Context

**ناشناخته:** سورس بسیاری از ماژول‌های ارجاع‌شده از WebHost، از جمله `Yashil.Infrastructure` که Commerce به آن وابسته است، در این نسخه نیست؛ در نتیجه جزئیات `YashilAppDbContext`، تنظیمات واقعی EF/Schema، Indexها، FKها و اجرای قیود Wallet قابل تأیید نیست. پیکربندی Build/Test کامل Frontend نیز موجود نیست. وضعیت Git در پایان بررسی پاک بود؛ هیچ فایل، Configuration یا History تغییر نکرد.

---

## user
# مرحله دوم — آماده‌سازی فرآیند توسعه Feature

در مرحله قبل، Repository را بررسی کردی و یک `Project Understanding Report` ارائه دادی.

اکنون بر اساس همان شناخت، بررسی کن که آیا ساختار فعلی Repository و فایل‌های راهنمای Codex برای شروع توسعه Featureهای واقعی کافی است یا خیر.

## هدف

هدف این مرحله **طراحی یا پیاده‌سازی Feature نیست**.

فقط باید مشخص کنیم از این مرحله به بعد، وقتی یک Feature جدید به تو داده می‌شود، چه Workflowای باید دنبال شود.

---

## Scope

فقط موارد زیر را بررسی کن:

- `AGENTS.md`
- `docs/codex/`
- `.codex/skills/`
- نتیجه `Project Understanding Report` مرحله قبل

در صورت نیاز فقط برای تأیید یک نکته مشخص، به یک یا دو فایل Source مراجعه کن.

**Repository را دوباره به صورت گسترده بررسی نکن.**

---

## مواردی که باید مشخص کنی

### 1. Feature Development Workflow

یک Workflow عملی و کوتاه برای توسعه Feature پیشنهاد بده که شامل این مراحل باشد:

1. دریافت Feature و فهم Requirement
2. بررسی Business Rules و Specification
3. پیدا کردن Implementation مشابه در Repository
4. تعیین Project / Layer / Fileهای درگیر
5. تهیه Plan در صورت نیاز
6. Implementation
7. Test / Build مرتبط
8. Review تغییرات و بررسی Scope

Workflow باید متناسب با همین Repository باشد، نه یک Workflow عمومی.

---

### 2. Plan قبل از Implementation

مشخص کن چه نوع Featureهایی نیاز به Plan و تأیید من دارند.

برای مثال:

- Feature ساده
- Feature متوسط
- Feature پیچیده
- تغییر Business Rule
- تغییر معماری

برای Plan یک ساختار کوتاه و عملی پیشنهاد بده.

---

### 3. قوانین Implementation

بر اساس Conventionهای واقعی Repository مشخص کن که Codex هنگام توسعه Feature باید چه اصولی را رعایت کند.

تمرکز روی موارد عملی باشد، مانند:

- استفاده از Patternهای موجود
- حداقل تغییر لازم
- عدم ایجاد Abstraction غیرضروری
- عدم تغییر خارج از Scope
- عدم اختراع Business Rule
- رعایت Layerهای موجود
- استفاده از Implementationهای مشابه

---

### 4. Testing و Review

مشخص کن بعد از Implementation چه بررسی‌هایی باید انجام شود.

فقط مواردی را پیشنهاد بده که با ساختار واقعی پروژه قابل انجام هستند.

---

## Database Scope

Database، Schema، `DbContext`، Migration، Index و FK خارج از Scope این Workflow هستند، مگر اینکه در یک Feature آینده **صراحتاً** درخواست شوند.

Entityهای موجود که از Database Scaffold شده‌اند را به عنوان Source موجود در نظر بگیر و صرفاً به دلیل تفاوت با مستندات، پیشنهاد بازطراحی آنها را نده.

---

## Wallet Scope

در این مرحله هیچ Wallet Feature را پیاده‌سازی نکن.

همچنین:

- Business Rule جدید تعریف نکن.
- Entity جدید ایجاد نکن.
- Service جدید ایجاد نکن.
- Wallet MVP را دوباره تحلیل نکن.

Wallet فقط به عنوان یکی از Feature Domainهای آینده در نظر گرفته شود.

---

## تغییر فایل‌ها

در این مرحله:

- Source Code را تغییر نده.
- Feature پیاده‌سازی نکن.
- Database را تغییر نده.
- Package نصب نکن.
- Git History را تغییر نده.
- Commit ایجاد نکن.

اگر واقعاً نیاز به تغییر در یکی از فایل‌های راهنمای Codex وجود دارد، **ابتدا فقط آن تغییر را به صورت پیشنهاد ارائه بده و خودت فایل را تغییر نده.**

---

# خروجی

یک گزارش کوتاه با عنوان:

**Development Workflow Review**

ارائه بده و فقط شامل این موارد باشد:

### 1. Workflow پیشنهادی

مراحل استاندارد توسعه Feature در این Repository.

### 2. Plan Criteria

چه زمانی باید قبل از Implementation از من تأیید Plan گرفته شود.

### 3. Implementation Rules

مهم‌ترین قوانین اجرایی Codex.

### 4. Testing & Review

مراحل بررسی بعد از Implementation.

### 5. Existing Structure Assessment

آیا ساختار فعلی `AGENTS.md`، `docs/codex/` و `.codex/skills/` برای شروع توسعه کافی است؟

یکی از این دو نتیجه را صریحاً اعلام کن:

- **Ready for Feature Development**
- **Changes Recommended**

اگر `Changes Recommended` بود، فقط تغییرات ضروری را پیشنهاد بده و دلیل هرکدام را کوتاه توضیح بده.

## شرط پایان

پس از ارائه گزارش متوقف شو.

هیچ فایل یا کدی را تغییر نده و هیچ Featureای پیاده‌سازی نکن.


---

## assistant
فقط فایل‌های راهنمای تعیین‌شده و نتیجهٔ گزارش قبلی را مبنا می‌گیرم. به‌ویژه مسیر skillها و نقطهٔ تأیید Plan را بررسی می‌کنم تا Workflow پیشنهادی با ساختار همین مخزن سازگار باشد.

---

## assistant
# Development Workflow Review

### 1. Workflow پیشنهادی

1. Requirement و معیار پذیرش Feature را روشن کن.
2. Specification و فقط مستندات مرتبط در [docs/codex](P:/Projects/walletProject/Wallet/docs/codex/README.md) را بخوان؛ برای قواعد نامشخص حدس نزن.
3. یک پیاده‌سازی مشابه در Commerce یا Angular پیدا کن.
4. Project، لایه و فایل‌های درگیر را مشخص کن.
5. در صورت احراز معیارهای بخش بعد، Plan کوتاه ارائه بده و تأیید بگیر.
6. Feature را با الگوهای موجود پیاده‌سازی کن.
7. Build/Test مرتبطی را که دستور معتبر آن در مخزن موجود است اجرا کن.
8. Diff و وضعیت Git را از نظر صحت و خروج از Scope بررسی کن.

### 2. Plan Criteria

- **ساده:** تغییر محدود در یک بخش، با الگوی مشابه و Rule روشن؛ Plan و تأیید جداگانه لازم نیست.
- **متوسط:** تغییر چند لایه، قرارداد API یا رفتار قابل مشاهده؛ Plan و تأیید پیش از Implementation لازم است.
- **پیچیده، مالی، تغییر Business Rule یا معماری:** Plan و تأیید صریح لازم است. اگر Rule یا وابستگی بیرونی نامعلوم باشد، همان ابهام باید پیش از اجرای بخش وابسته تعیین تکلیف شود.

ساختار Plan: **هدف و معیار پذیرش؛ نمونهٔ مشابه؛ فایل‌ها و لایه‌های درگیر؛ مراحل تغییر؛ ریسک و ابهام؛ روش راستی‌آزمایی.** این معیارها اجرای بند «non-trivial work» در [AGENTS.md](P:/Projects/walletProject/Wallet/AGENTS.md) را مشخص می‌کنند.

### 3. Implementation Rules

از قراردادهای Core، پیاده‌سازی‌های Infrastructure، ثبت DI و الگوهای Controller/ViewModel و AutoMapper موجود استفاده کن. در Angular، نزدیک‌ترین Module، Route، Component و سرویس مشابه را مبنا بگیر. کمترین تغییر لازم را انجام بده؛ abstraction، وابستگی یا refactor خارج از Scope اضافه نکن. Business Rule، رفتار ماژول‌های غایب و رابطهٔ داده‌ای را حدس نزن. Entityهای Scaffold شده صرفاً به‌دلیل تفاوت با سند بازطراحی نمی‌شوند. Database و Schema فقط با درخواست صریح Feature وارد Scope می‌شوند.

### 4. Testing & Review

Build پروژهٔ درگیر را فقط وقتی وابستگی‌ها و دستور معتبر آن موجود است اجرا کن. برای Angular، تا زمان وجود پیکربندی Build/Test، دستور npm فرضی پیشنهاد نکن. تست مرتبط موجود را در صورت قابلیت اجرا به کار ببر؛ نبود Test Project را گزارش کن. در پایان، `git diff` و `git status` را برای تغییرات ناخواسته، فایل‌های خارج از Scope و دادهٔ حساس بررسی کن.

### 5. Existing Structure Assessment

**Ready for Feature Development.** [AGENTS.md](P:/Projects/walletProject/Wallet/AGENTS.md) قواعد پایه و نقطهٔ Plan را دارد و [docs/codex](P:/Projects/walletProject/Wallet/docs/codex/README.md) مسیر مطالعهٔ تدریجی را مشخص می‌کند. مسیر `.codex/skills/` در این مخزن وجود ندارد؛ مسیر معرفی‌شده در راهنمای مخزن `.agents/skills/` است و وجود آن تأیید شد. برای Featureهای وابسته به ماژول‌های غایب یا تنظیمات Build/Test ناموجود، همان محدودیت باید در Plan آن Feature ثبت شود. هیچ فایلی تغییر نکرد.

---

## user
الان می‌خواهم Workflow توسعه Feature در این Repository را قبل از شروع پیاده‌سازی بررسی کنی.

هدف این مرحله فقط بررسی آمادگی زیرساخت فعلی Codex برای شروع توسعه Feature است.

### محدوده کار

فقط موارد زیر را بررسی کن:

1. `AGENTS.md`
2. ساختار `Skills`
3. ساختار `Specifications`
4. نحوه سازمان‌دهی Context و Documentation
5. نحوه استفاده از این موارد در Workflow توسعه Feature
6. اینکه آیا دستورالعمل‌ها و ساختار فعلی برای شروع یک Feature جدید واضح، قابل اجرا و کم‌مصرف از نظر Context هستند یا خیر.

### نکات مهم

- Repository را برای تحلیل مجدد معماری Wallet اسکن نکن.
- مستندات Business و Wallet را دوباره مطالعه نکن.
- وارد Implementation نشو.
- هیچ فایل Source Code را تغییر نده.
- هیچ Database، DbContext، Schema، Index، FK یا Migration را بررسی یا تغییر نده.
- Git را بررسی یا تنظیم نکن.
- Skill یا Specification جدید ایجاد نکن.
- هدف این مرحله پیدا کردن ایرادهای احتمالی در خود Workflow است، نه توسعه Feature.

### معیار بررسی

بررسی کن که آیا Workflow فعلی این مسیر را به‌صورت واضح و کم‌هزینه پشتیبانی می‌کند:

User Request\
→ Specification\
→ Relevant Skills / Context\
→ Repository Inspection\
→ Implementation Plan\
→ Implementation\
→ Self-check / Tests\
→ Final Report

به‌خصوص بررسی کن:

- آیا Codex می‌تواند برای هر Feature فقط Context موردنیاز را Load کند؟
- آیا مرز بین Specification، Skill و Repository Knowledge واضح است؟
- آیا احتمال Context Bloat یا مطالعه غیرضروری Repository وجود دارد؟
- آیا مراحل قبل از Implementation به اندازه کافی مشخص هستند؟
- آیا بعد از Implementation مرحله Self-check و Validation مشخص است؟
- آیا دستورالعمل‌ها احتمال رفتارهای خارج از Scope را کاهش می‌دهند؟
- آیا Developer برای شروع یک Feature جدید می‌داند دقیقاً چه چیزی باید به Codex بدهد؟
- آیا Codex می‌داند قبل از تغییر Code چه چیزهایی را باید بررسی کند و چه چیزهایی را نباید بررسی کند؟

### خروجی مورد انتظار

فقط یک گزارش کوتاه ارائه بده و در آن مشخص کن:

1. وضعیت فعلی Workflow:
   - Ready for Feature Development
   - یا Changes Recommended
2. مواردی که درست و کافی هستند.
3. حداکثر مواردی که واقعاً نیاز به اصلاح دارند، همراه با دلیل.
4. اگر اصلاحی ضروری است، دقیقاً بگو:
   - کدام فایل/بخش باید تغییر کند
   - چه نوع تغییری لازم است
   - چرا

در این مرحله هیچ تغییری را خودت اعمال نکن.

تمرکز اصلی:\
**کمترین Context و Token مصرفی، بدون کاهش کیفیت توسعه و بدون ایجاد پیچیدگی غیرضروری.**


---

## assistant
ساختار راهنماها، Skillها و Specificationها را فقط از نظر مسیر استفاده و هزینهٔ Context بررسی می‌کنم. به کد، مستندات Wallet، Database یا Git وارد نمی‌شوم.

---

## assistant
# Development Workflow Review

**وضعیت: Changes Recommended.** ساختار فعلی سبک است، اما سه ابهام کوچک می‌تواند مسیر شروع Feature را تغییر دهد.

### موارد کافی

- [AGENTS.md](P:/Projects/walletProject/Wallet/AGENTS.md) بررسی نمونهٔ مشابه، تغییر محدود و راستی‌آزمایی نهایی را الزام می‌کند.
- [docs/codex/README.md](P:/Projects/walletProject/Wallet/docs/codex/README.md) بارگذاری تدریجی Context را مشخص می‌کند. Skillها نیز کوتاه و تخصصی‌اند و Self-check دارند.
- [قالب Specification](P:/Projects/walletProject/Wallet/docs/specifications/TEMPLATE.md) برای ثبت Rule، معیار پذیرش و خارج از Scope مناسب است. نیازی نیست برای هر درخواست حتماً فایل Specification جدید ساخته شود.

### اصلاحات ضروری پیشنهادی

1. **[AGENTS.md](P:/Projects/walletProject/Wallet/AGENTS.md)، بخش Planning:** ورودی حداقلی Feature‌ (رفتار مطلوب، معیار پذیرش و محدودیت‌ها) و معیار عملی «non-trivial» را در چند خط مشخص کند: تغییر چند لایه یا قرارداد، Rule مالی/کسب‌وکار، یا معماری نیازمند Plan و تأیید پیش از پیاده‌سازی است. اکنون نقطهٔ تأیید برای درخواست‌های متوسط روشن نیست.

2. **[docs/codex/README.md](P:/Projects/walletProject/Wallet/docs/codex/README.md)، بخش Source-of-truth order:** روشن کند کد منبع شناخت *رفتار موجود* است و درخواست کاربر و Specification تأییدشده منبع *رفتار مطلوب* هستند؛ اختلاف باید گزارش شود. ترتیب خطی فعلی که Task Prompt را آخر می‌گذارد، این دو نقش را مخلوط می‌کند.

3. **[wallet-feature/SKILL.md](P:/Projects/walletProject/Wallet/.agents/skills/wallet-feature/SKILL.md)، گام ۲:** الزام بررسی Schema را به زمانی محدود کند که Feature آینده صراحتاً Database را در Scope قرار می‌دهد. این تغییر، Workflow را با محدوده‌ای که شما تعیین کرده‌اید هماهنگ می‌کند.

با همین اصلاحات متنی، مسیر `Request → Specification → Context/Skill مرتبط → نمونهٔ واقعی → Plan در صورت نیاز → Implementation → Self-check/Build → گزارش` روشن می‌شود، بدون افزودن ساختار یا Context دائمی. مسیر واقعی Skillها `.agents/skills/` است؛ `.codex/skills/` در این مخزن وجود ندارد. هیچ فایلی تغییر نکرد.

---

## user
بر اساس گزارش Review قبلی، فقط سه اصلاح متنی زیر را اعمال کن.

### 1. AGENTS.md — Planning

در بخش Planning، یک دستورالعمل کوتاه اضافه کن که مشخص کند:

Feature حداقل باید شامل این موارد باشد:

- رفتار مطلوب
- Acceptance Criteria
- محدودیت‌ها و Scope

همچنین معیار عملی برای `non-trivial` بودن Feature مشخص کن.

Feature را non-trivial در نظر بگیر اگر حداقل یکی از موارد زیر وجود داشته باشد:

- تغییر در چند لایه یا Contract
- تغییر یا اضافه شدن Business/Financial Rule
- تغییر معماری یا Design
- تغییراتی که نیاز به تصمیم‌گیری بین چند مسیر پیاده‌سازی دارند

برای Featureهای non-trivial:

- ابتدا Plan تهیه شود.
- قبل از Implementation، Plan تأیید شود.

برای تغییرات ساده و local، از ایجاد Plan غیرضروری خودداری کن.

### 2. docs/codex/README.md — Source of Truth

بخش Source-of-truth را طوری اصلاح کن که نقش منابع کاملاً مشخص باشد:

- Repository Code:\
  منبع حقیقت برای رفتار موجود سیستم است.
- User Request:\
  منبع رفتار مورد انتظار Feature است.
- Approved Specification:\
  منبع دقیق و رسمی رفتار مطلوب، Business Rules و Acceptance Criteria است؛ در صورتی که Specification برای Feature وجود داشته باشد.

اگر بین رفتار موجود Code و رفتار مطلوب Request/Specification اختلاف وجود داشت:

- اختلاف را شناسایی کن.
- آن را گزارش کن.
- بدون تصمیم‌گیری خودسرانه، رفتار مطلوب را از User/Specification مشخص کن.

این بخش نباید باعث شود Codex برای هر Feature تمام Repository یا تمام Documentation را مطالعه کند.

### 3. .agents/skills/wallet-feature/SKILL.md — Schema Inspection

گام مربوط به بررسی Database Schema را اصلاح کن.

بررسی Schema، Table، Column، FK، Index و سایر جزئیات Database فقط زمانی انجام شود که Feature به‌صورت مستقیم یا غیرمستقیم Database را در Scope داشته باشد.

در Featureهایی که Database در Scope نیست:

- Schema را بررسی نکن.
- Context مربوط به Database را Load نکن.

### محدودیت‌های مهم

- هیچ Source Code را تغییر نده.
- هیچ فایل جدیدی ایجاد نکن.
- هیچ Skill یا Specification جدیدی ایجاد نکن.
- فقط همین سه فایل را ویرایش کن.
- ساختار Repository را تغییر نده.
- هیچ Package، Dependency یا Configuration را تغییر نده.

### بعد از اعمال تغییرات

هر سه فایل را دوباره بررسی کن و مطمئن شو:

1. دستورالعمل جدید با سایر دستورالعمل‌ها تناقض ندارد.
2. Context غیرضروری به Workflow اضافه نشده است.
3. Workflow همچنان Progressive و کم‌مصرف باقی مانده است.
4. مرز بین Request، Specification و Existing Code واضح است.
5. Featureهای ساده مجبور به Planning غیرضروری نمی‌شوند.
6. Database فقط زمانی وارد Context می‌شود که Scope آن را ایجاب کند.

در پایان فقط گزارش بده:

- چه فایل‌هایی تغییر کردند.
- خلاصه هر تغییر.
- نتیجه Self-check.

هیچ Feature واقعی را پیاده‌سازی نکن.


---

## assistant
فقط همین سه فایل راهنما را ویرایش می‌کنم. متن را کوتاه نگه می‌دارم و سپس هر سه فایل را دوباره می‌خوانم تا مرز Scope و شرط Plan را بررسی کنم.

---

## assistant
سه فایل تغییر کرد:

- [AGENTS.md](P:/Projects/walletProject/Wallet/AGENTS.md): ورودی حداقلی Feature، معیارهای `non-trivial` و لزوم تأیید Plan پیش از پیاده‌سازی آن مشخص شد. تغییرات ساده و محلی به Plan غیرضروری نیاز ندارند.
- [docs/codex/README.md](P:/Projects/walletProject/Wallet/docs/codex/README.md): نقش کد موجود، درخواست کاربر و Specification تأییدشده از هم تفکیک شد.
- [wallet-feature/SKILL.md](P:/Projects/walletProject/Wallet/.agents/skills/wallet-feature/SKILL.md): بارگذاری Context و بررسی جزئیات Database به Featureهایی محدود شد که Database مستقیم یا غیرمستقیم در Scope آن‌هاست.

**نتیجهٔ Self-check:** هر سه فایل دوباره خوانده شدند. دستورالعمل‌های جدید با Workflow تدریجی سازگارند، Context اضافی را الزامی نمی‌کنند و مرز Planning و Database Scope را روشن نگه می‌دارند. هیچ Feature یا فایل دیگری تغییر نکرد.

---

## user
فاز Pilot Feature را شروع کن.

هدف این مرحله این است که Workflow و Harness فعلی Repository را با یک Feature واقعی از Wallet آزمایش کنیم.

### مرحله 1 — انتخاب Feature

ابتدا Documentation و Specificationهای موجود Repository را بررسی کن و یک Feature واقعی و مناسب برای Pilot انتخاب کن.

Feature انتخابی باید:

- واقعی و موردنیاز سیستم Wallet باشد.
- به اندازه کافی non-trivial باشد تا Workflow را واقعاً آزمایش کند.
- چند بخش از معماری موجود را درگیر کند.
- در صورت نیاز Business Rule داشته باشد.
- ولی آن‌قدر بزرگ نباشد که کل Wallet را وارد یک تغییر واحد کند.

Feature ساختگی ایجاد نکن و Feature را فقط بر اساس حدس انتخاب نکن.

### مرحله 2 — Context Discovery

پس از انتخاب Feature، فقط Context موردنیاز همان Feature را پیدا و مطالعه کن.

طبق Workflow موجود:

- Repository Code منبع رفتار موجود است.
- User Request / Specification منبع رفتار مطلوب است.
- Skill مرتبط را پیدا و استفاده کن.
- Context غیرمرتبط را Load نکن.
- کل Repository را بدون دلیل اسکن نکن.

### مرحله 3 — تحلیل

برای Feature انتخاب‌شده مشخص کن:

1. Feature چیست؟
2. Business Goal آن چیست؟
3. رفتار مطلوب چیست؟
4. Acceptance Criteria چیست؟
5. چه بخش‌هایی از سیستم در Scope هستند؟
6. چه بخش‌هایی خارج از Scope هستند؟
7. چه قسمت‌هایی از Code موجود مرتبط هستند؟
8. آیا Database در Scope است؟
9. آیا Feature طبق معیارهای AGENTS.md، non-trivial محسوب می‌شود؟
10. چه ابهام‌ها یا Conflictهایی بین Documentation و Code موجود وجود دارد؟

### مرحله 4 — Plan

اگر Feature non-trivial است، یک Implementation Plan ارائه بده.

Plan باید شامل موارد زیر باشد:

- مراحل پیاده‌سازی
- فایل‌ها/لایه‌های احتمالی که باید تغییر کنند
- تغییرات احتمالی Database در صورت وجود
- Business Rules مرتبط
- Validation و Test Strategy
- ریسک‌ها یا ابهامات

### قوانین مهم

در این مرحله:

- هیچ Source Code را تغییر نده.
- هیچ Migration ایجاد نکن.
- هیچ Database را تغییر نده.
- هیچ Feature دیگری را بررسی یا پیاده‌سازی نکن.
- هیچ فایل جدیدی ایجاد نکن.
- فقط Analysis و Plan ارائه بده.

### خروجی

در پایان گزارش را با این ساختار ارائه کن:

## Selected Pilot Feature

نام و توضیح کوتاه Feature

## Why This Feature

چرا این Feature برای Pilot مناسب است.

## Scope

### In Scope

...

### Out of Scope

...

## Required Context

فقط Contextهایی که واقعاً برای Implementation لازم هستند.

## Existing Code

نمونه‌ها و بخش‌های مرتبط موجود در Repository.

## Specification / Business Rules

رفتار مورد انتظار و Rules.

## Acceptance Criteria

...

## Database Impact

اگر ندارد، صریحاً بنویس:\
`No Database impact identified.`

## Ambiguities / Conflicts

...

## Classification

Simple / Non-trivial

## Implementation Plan

فقط اگر Non-trivial است.

### مهم

در این مرحله منتظر تأیید من برای Implementation بمان.

حتی اگر Plan کامل به نظر می‌رسد، بدون تأیید من وارد Implementation نشو.


---

## assistant
برای Pilot، Feature را از قواعد ثبت‌شدهٔ Wallet انتخاب می‌کنم و سپس فقط مسیر همان Feature را در کد بررسی می‌کنم. از Skillهای `wallet-feature` و `backend-feature` برای تحلیل و Plan استفاده می‌کنم؛ در این مرحله هیچ پیاده‌سازی انجام نمی‌شود.

---

## assistant
Feature «ایجاد رزرو اعتبار Wallet برای سفارش» را به‌عنوان گزینهٔ Pilot بررسی می‌کنم: در سند مرجع به‌صورت مستقل آمده و هم‌زمان قرارداد API، منطق مالی و ثبت داده را درگیر می‌کند. حالا وجود پیاده‌سازی فعلی و یک الگوی مشابه در Commerce را به‌صورت محدود بررسی می‌کنم.

---

## assistant
## Selected Pilot Feature

**ایجاد رزرو اعتبار Wallet برای یک سفارش (Reserve).** این عملیات پیش از پرداخت، مبلغی از اعتبار یک `WalletAssignment` را در `WalletReservation` رزرو می‌کند.

## Why This Feature

Reserve در [مرجع Wallet MVP](P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md) تعریف شده است. هم‌زمان قرارداد سرویس، منطق مالی، دسترسی به داده و API را درگیر می‌کند و idempotency و همروندی را می‌آزماید، بدون آنکه اجرای پرداخت و تسویه را وارد همین تغییر کند.

## Scope

### In Scope

- ایجاد Reservation برای Assignment و سفارش مشخص.
- کنترل ماندهٔ قابل استفاده، idempotency و جلوگیری از خرج دوباره.
- ثبت اتمیک Reservation و به‌روزرسانی مبلغ رزروشدهٔ Assignment و Program.
- اعتبارسنجی ورودی و دسترسی به سفارش در نقطهٔ ورود.

### Out of Scope

Eligibility اولیه و ایجاد Assignment، اجرای پرداخت، Capture، Release، Refund، Ledger قطعی، Coupon، Card و UI.

## Required Context

بخش‌های `WalletAssignment`، `WalletReservation`، Financial Flow و Invariants از مرجع MVP؛ [wallet.md](P:/Projects/walletProject/Wallet/docs/codex/wallet.md)؛ Skillهای [wallet-feature](P:/Projects/walletProject/Wallet/.agents/skills/wallet-feature/SKILL.md) و [backend-feature](P:/Projects/walletProject/Wallet/.agents/skills/backend-feature/SKILL.md)؛ و مسیر مشابه سرویس و API در Commerce. در `docs/specifications` فقط قالب وجود دارد و Specification تأییدشدهٔ مخصوص Reserve پیدا نشد.

## Existing Code

[WalletAssignment.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletAssignment.cs) مبالغ `Assigned`، `Consumed`، `Reserved` و `Expired` را دارد. [WalletReservation.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs) شامل `OrderId`، `Amount`، `IdempotencyKey`، وضعیت و `RowVersion` است. در Commerce، سرویس یا ثبت DI برای Reserve پیدا نشد و [WalletController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs) خالی است. الگوی لایه‌ها، تراکنش و کنترل دسترسی را می‌توان در [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs)، [HekmatTransactionService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/HekmatTransactionService.cs) و [PaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/PaymentController.cs) دید.

## Specification / Business Rules

طبق مرجع، Reserve پیش از Payment انجام می‌شود، باید idempotent باشد و نباید ماندهٔ قابل استفاده را منفی کند. مجموع `Reserved + Consumed + Expired` نباید از `Assigned` بیشتر شود؛ درخواست‌های هم‌زمان نباید به خرج دوباره منجر شوند. Reserve تراکنش قطعی Ledger نیست. این‌ها قواعد مستند هستند؛ قرارداد دقیق API و رفتار خطا هنوز مشخص نشده‌اند.

## Acceptance Criteria

**معیارهای استخراج‌شده از سند، برای تأیید در Specification Pilot:**

- رزرو موفق یک Reservation با وضعیت `Reserved` ایجاد می‌کند و مبالغ رزروشدهٔ مرتبط را به‌طور سازگار افزایش می‌دهد.
- تکرار عملیات با کلید idempotency، رزرو یا کسر اعتبار دوم ایجاد نمی‌کند.
- درخواست‌های هم‌زمان نمی‌توانند مانده را منفی کنند.
- شکست عملیات، تغییر مالی نیمه‌کاره باقی نمی‌گذارد.
- برای Reserve رکورد `WalletTransaction` قطعی ایجاد نمی‌شود.

## Database Impact

**بله.** این Feature به درج Reservation و تغییر مانده‌های ذخیره‌شده نیاز دارد. وجود و تنظیم واقعی Schema، یکتایی `IdempotencyKey` و رفتار `RowVersion` در سورس موجود قابل تأیید نیست؛ سورس `Yashil.Infrastructure` در این نسخه غایب است. نیاز به Index یا تغییر Schema باید پس از بررسی Schema واقعی تعیین شود؛ در این مرحله Migration پیشنهاد قطعی نمی‌شود.

## Ambiguities / Conflicts

- Entityهای فعلی شناسهٔ `int` دارند، در حالی که مرجع MVP برای Wallet شناسهٔ `bigint` نوشته است. این Pilot بازطراحی Entityهای Scaffold شده را فرض نمی‌کند.
- سند `AvailableAmount` را computed توصیف می‌کند؛ Entity آن را `decimal?` نشان می‌دهد. منبع قابل اتکای محاسبه و کنترل اتمیک باید روشن شود.
- محدودهٔ یکتایی کلید idempotency، مدت اعتبار رزرو، رفتار تکرار کلید با ورودی متفاوت، و بررسی وضعیت/تاریخ Assignment و Program مشخص نیست.
- Entity دارای `OrderItemId` است، اما سناریوی رزرو در سند در سطح سفارش تعریف شده است. استفاده از آن نیاز به تصمیم صریح دارد.
- نقطهٔ فراخوانی Reserve در جریان سفارش/پرداخت و قرارداد API هنوز مشخص نشده‌اند.

## Classification

**Non-trivial** — چند لایه و قرارداد جدید، Rule مالی، همروندی و چند تصمیم پیاده‌سازی را درگیر می‌کند.

## Implementation Plan

1. پیش از کدنویسی، Specification کوتاه Pilot را با قرارداد فراخوانی، قواعد ابهام‌دار بالا و Acceptance Criteria نهایی تأیید کنیم.
2. Schema و نگاشت واقعی موجود را فقط برای Assignment، Program و Reservation بررسی کنیم؛ سازوکار اتمیک و یکتایی را بر همان مبنا تعیین کنیم.
3. قراردادهای لازم را در Commerce Core و پیاده‌سازی محدود سرویس/Repository را در Infrastructure اضافه کنیم؛ Reserve و تغییر مبالغ در یک واحد اتمیک انجام شود.
4. endpoint مجاز، اعتبارسنجی ورودی و ثبت DI را مطابق الگوی Commerce اضافه کنیم.
5. idempotency، ماندهٔ ناکافی، شکست میانی و درخواست هم‌زمان را با تست‌های مرتبط بررسی کنیم؛ Build بخش درگیر را در صورت فراهم بودن وابستگی‌ها اجرا و Diff را از نظر Scope بازبینی کنیم.

**ریسک اصلی:** بدون Schema/نگاشت واقعی و تصمیم دربارهٔ قرارداد idempotency، تضمین همروندی قابل تأیید نیست. برای ورود به Implementation منتظر تأیید Plan و تعیین تکلیف این ابهام‌ها می‌مانم.

---

## user

# Files pasted by the user:

## "بر اساس Analysis و Plan قبلی برای Pilot Feature با عنوان: **Wallet Reserve — ای…": C:\Users\pc\.codex/attachments/edb92cf1-c503-4802-bac2-f134dedfd482/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن پیوست را می‌خوانم و آن را با Plan قبلی تطبیق می‌دهم تا مرحلهٔ بعد را دقیقاً در همان محدوده پیش ببرم.

---

## assistant
در این مرحله Specification را فقط تا حدی قطعی می‌کنم که سند و کد پشتیبانی کنند. مواردی مثل Scope کلید idempotency یا قرارداد API اگر شواهد کافی نداشته باشند، به‌صورت تصمیمِ نیازمند تأیید باقی می‌مانند.

---

## assistant
شواهد فعلی نشان می‌دهد مدل دادهٔ Reserve در Entityها وجود دارد، اما مسیر اجرایی آن در Commerce هنوز دیده نمی‌شود. نمونهٔ تراکنش و به‌روزرسانی شرطی در Repository هست؛ بررسی می‌کنم این الگوها چه چیزی را ثابت می‌کنند و کدام تضمین‌ها همچنان به Schema واقعی وابسته‌اند.

---

## assistant
## 1. Resolved Decisions

موارد زیر از [مرجع Wallet MVP](P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md) قابل تعیین‌اند:

- Reserve بین Eligibility و Payment قرار می‌گیرد. هر Reservation به یک `WalletAssignment`، یک `User` و یک `Order` ارجاع دارد و مبلغ، کلید idempotency و زمان انقضا دارد.
- وضعیت اولیهٔ رزرو موفق `Reserved` است. `CreatedAt` و `ExpiresAt` در مدل مستند لازم‌اند. Reserve یک `WalletTransaction` قطعی ایجاد نمی‌کند.
- فرمول مستند ماندهٔ قابل استفادهٔ Assignment برابر `Assigned − Consumed − Reserved − Expired` است؛ پس `Expired` کسر می‌شود. Program نیز `ReservedAmount` را به‌عنوان مجموع رزروهای باز دارد، اما فیلد `AvailableAmount` مستقل برای آن تعریف نشده است.
- Reserve باید idempotent باشد و درخواست‌های هم‌زمان نباید مانده را منفی کنند.

کد موجود این رفتار را **پیاده‌سازی یا تضمین‌شده نشان نمی‌دهد**. [WalletReservation.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs) و [WalletAssignment.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletAssignment.cs) مدل‌های مرتبط را دارند، ولی در Commerce سرویس Reserve پیدا نشد.

## 2. Decision Required

### الف) فراخواننده، قرارداد و Authorization

- **Question:** چه بخشی Reserve را فراخوانی می‌کند؛ سرویس داخلی کافی است یا API لازم است؟ شناسهٔ کاربر و زمان انقضا را چه کسی تعیین می‌کند؟
- **Why it matters:** قرارداد ورودی/خروجی و محل کنترل دسترسی به این انتخاب وابسته است.
- **Available options:** فراخوانی داخلی از جریان سفارش/پرداخت؛ یا endpoint در Commerce Web. `UserId` از هویت معتبر گرفته شود یا در قرارداد داخلی ارسال شود.
- **Evidence:** سند فقط ترتیب `Eligibility → Reserve → Payment` را می‌گوید. [WalletController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs) خالی است. [PaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/PaymentController.cs) نمونهٔ Authorization در Web است؛ متد دسترسی به سفارش در `OrderService` وجود دارد، ولی قاعدهٔ دسترسی به *اعتبار همان کاربر* برای Reserve تعریف نشده است.
- **Recommended decision for approval:** ابتدا فراخوانندهٔ واقعی را مشخص کنید. قرارداد سرویس با `AssignmentId`، `OrderId`، `Amount` و `IdempotencyKey` و خروجی شامل شناسه، وضعیت، مبلغ و زمان انقضای Reservation پیشنهاد می‌شود. endpoint و شیوهٔ تعیین `UserId`/انقضا پس از تعیین فراخواننده تأیید شوند؛ در هر حالت دسترسی کاربر به Assignment و Order در مسیر اجرایی کنترل شود.

### ب) Scope و رفتار Idempotency

- **Question:** کلید در چه محدوده‌ای یکتا است و تکرار آن با ورودی یکسان یا متفاوت چه نتیجه‌ای دارد؟
- **Why it matters:** بدون قرارداد و قید یکتایی قابل اتکا، retry و race ممکن است اعتبار را دوبار رزرو کنند.
- **Available options:** کلید یکتای سراسری؛ یا کلید مرکب با Order/Assignment. برای ورودی متفاوت، رد درخواست یا تعریف عملیات جدید.
- **Evidence:** سند `IdempotencyKey` را «کلید یکتای Reserve/عملیات» می‌نامد، اما Scope و پاسخ retry را تعیین نمی‌کند. Entity فقط property آن را دارد؛ Unique Constraint در منابع موجود تأیید نشد.
- **Recommended decision for approval:** یکتایی سراسری کلید برای Reserve؛ تکرار همان کلید با همان ورودی، بازگرداندن همان Reservation بدون تغییر مجدد مبالغ؛ تکرار با ورودی متفاوت، خطای تعارض. تضمین یکتایی در Database پس از تأیید Schema واقعی بررسی و اعمال شود.

### پ) سطح Order و تعداد Reservationها

- **Question:** آیا Reserve فقط در سطح Order است و آیا چند Reservation برای یک Order مجاز است؟
- **Why it matters:** بر انتخاب کلید یکتا، مبلغ رزرو و قرارداد API اثر دارد.
- **Available options:** یک Reservation برای هر Order؛ چند Reservation برای Assignmentهای متفاوت؛ یا رزرو سطح OrderItem.
- **Evidence:** سند برای `WalletReservation`، `OrderId` را تعریف می‌کند و `OrderItemId` را فقط برای `WalletTransaction` اختیاری می‌داند. Entity فعلی Reservation یک `OrderItemId` اختیاری نیز دارد. Navigationهای collection در [Order.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/Order.cs) و Assignment امکان نمایش چند رکورد را می‌دهند، اما مجاز بودن آن‌ها یا دلیل وجود `OrderItemId` را ثابت نمی‌کنند.
- **Recommended decision for approval:** Pilot در سطح Order باشد و `OrderItemId` را استفاده نکند. مجاز بودن چند Reservation برای یک Order، به‌ویژه از چند Assignment، صریحاً تعیین شود.

### ت) منبع مانده و تضمین همروندی

- **Question:** آیا `AvailableAmount` واقعاً ستون computed است؟ چه سازوکاری رزرو و تغییر مبالغ Assignment/Program را اتمیک می‌کند؟
- **Why it matters:** محاسبهٔ درست به‌تنهایی جلوی خرج دوباره را نمی‌گیرد.
- **Available options:** `RowVersion` با optimistic concurrency؛ به‌روزرسانی شرطی اتمیک؛ transaction همراه با قید یکتایی؛ یا ترکیب آن‌ها.
- **Evidence:** سند `AvailableAmount` را computed و `RowVersion` را ابزار همروندی معرفی می‌کند. Entity، `AvailableAmount` را property قابل تنظیم و `RowVersion` را `byte[]` نشان می‌دهد؛ این نمایش، تنظیم واقعی Database را ثابت یا رد نمی‌کند. در Commerce نمونهٔ transaction و `ExecuteUpdateAsync` وجود دارد، ولی پیاده‌سازی Wallet یا نگاشت `RowVersion` دیده نشد. سورس `Yashil.Infrastructure` در نسخهٔ موجود نیست.
- **Recommended decision for approval:** فرمول مستند مبنای Rule باشد؛ Reservation، افزایش `ReservedAmount` در Assignment و Program در یک transaction انجام شود. انتخاب روش کنترل هم‌زمانی پس از بررسی نوع/نگاشت واقعی `AvailableAmount` و `RowVersion`، قیود یکتا و رفتار update در Schema انجام شود. تا آن زمان یک روش خاص قطعی نیست.

### ث) زمان انقضا و شکست عملیات

- **Question:** `ExpiresAt` چگونه تعیین می‌شود و پاسخ/وضعیت درخواست ناموفق چیست؟
- **Why it matters:** بدون زمان معتبر، عمر رزرو مشخص نیست؛ رکورد نیمه‌کاره نیز گزارش مانده را مخدوش می‌کند.
- **Available options:** زمان انقضای تعیین‌شده توسط فراخوانندهٔ معتبر؛ یا سیاست زمان‌بندی سمت سرویس. شکست بدون رکورد؛ یا ثبت تلاش ناموفق در سازوکاری جداگانه.
- **Evidence:** سند وجود `ExpiresAt` و `CreatedAt` را الزام می‌کند، اما مدت رزرو یا سیاست شکست را نمی‌گوید. Entity فعلی `CreationDate` دارد و `CreatedAt` جداگانه ندارد.
- **Recommended decision for approval:** `CreatedAt` هنگام Reserve تعیین شود؛ سیاست `ExpiresAt` را مالک جریان پرداخت مشخص کند. در خطای اعتبارسنجی یا کمبود اعتبار، Reservation و تغییر مانده ثبت نشود.

## 3. Final Pilot Specification

**Goal:** رزرو اتمیک بخشی از اعتبار Assignment برای یک سفارش، پیش از پرداخت.

- **Inputs:** `AssignmentId`، `OrderId`، `Amount`، `IdempotencyKey`. `[DECISION REQUIRED]` منبع `UserId`، `ExpiresAt` و نوع فراخواننده.
- **Expected Output:** نتیجهٔ رزرو شامل شناسه، مبلغ، وضعیت و انقضا. `[DECISION REQUIRED]` شکل دقیق پاسخ و اینکه API عمومی لازم است یا خیر.
- **Business Rules:** مانده طبق فرمول مستند محاسبه شود؛ منفی نشود؛ `Reserved + Consumed + Expired ≤ Assigned` باقی بماند؛ `Program.ReservedAmount` با رزروهای باز سازگار بماند. `[DECISION REQUIRED]` شرط‌های وضعیت/تاریخ Program و Assignment.
- **Idempotency Rules:** تکرار نباید Reservation یا افزایش مبلغ دوم بسازد. `[DECISION REQUIRED]` Scope کلید، پاسخ replay و برخورد ورودی متفاوت.
- **Concurrency Rules:** درخواست‌های هم‌زمان نباید خرج دوباره ایجاد کنند. `[DECISION REQUIRED]` روش فنی پس از مشاهدهٔ Schema.
- **Reservation State:** رزرو موفق با `Reserved`، زمان ایجاد و زمان انقضا ثبت شود. `[DECISION REQUIRED]` سیاست تعیین انقضا و رفتار دقیق شکست.
- **Database Behavior:** درج Reservation و تغییر مبالغ مرتبط باید اتمیک باشند؛ برای Reserve رکورد Ledger قطعی ایجاد نشود. `[DECISION REQUIRED]` قیود یکتا و نگاشت computed/`RowVersion` پس از بررسی Schema.
- **Authorization:** فقط فراخوانندهٔ مجاز بتواند برای Assignment و Order مجاز رزرو کند. `[DECISION REQUIRED]` نقطهٔ ورود و قاعدهٔ دقیق تطبیق مالکیت.
- **Error Cases:** ماندهٔ ناکافی، Assignment/Order نامعتبر یا غیرمجاز، تعارض کلید و رقابت هم‌زمان باید بدون تغییر نیمه‌کاره خاتمه یابند. `[DECISION REQUIRED]` کدها و شکل پاسخ.
- **Acceptance Criteria:** یک Reserve موفق یک رکورد `Reserved` و یک افزایش سازگار در مبالغ رزروشده ایجاد کند؛ retry همان عملیات اثر مالی دوم نداشته باشد؛ رقابت هم‌زمان مانده را منفی نکند؛ شکست تغییر نیمه‌کاره نگذارد؛ Ledger قطعی برای Reserve ساخته نشود.
- **Out of Scope:** Eligibility و Grant، OrderItem-level Reserve، Payment، Capture، Release، Refund، Coupon، Card و UI.

## 4. Updated Implementation Plan

1. تصمیم‌های بخش ۲ و Specification بالا را تأیید و قرارداد فراخوانی را نهایی کنیم.
2. فقط Schema و نگاشت‌های لازم برای Program، Assignment و Reservation را بررسی کنیم؛ computed بودن مانده، `RowVersion` و قیود یکتا را تأیید کنیم.
3. قرارداد Core، پیاده‌سازی Repository/Service و ثبت DI را طبق الگوی Commerce اضافه کنیم؛ endpoint فقط در صورت تأیید نیاز به API.
4. idempotency و به‌روزرسانی اتمیک مبالغ را با سازوکار تأییدشده پیاده‌سازی کنیم.
5. موارد ماندهٔ ناکافی، replay، تعارض کلید، شکست میانی و race را بررسی کنیم؛ Build/Test قابل اجرای بخش درگیر و Review تغییرات را انجام دهیم.

## 5. Readiness

**NOT READY — DECISIONS REQUIRED**

پیشنهادهای بخش ۲ Business Rule قطعی نیستند. پس از تأیید تصمیم‌ها و Specification، Plan قابل اجرا خواهد بود؛ تا آن زمان Implementation شروع نمی‌شود.

---

## user
برای Pilot Feature مربوط به **Wallet Reserve**، اکنون فقط یک بررسی فنی محدود انجام بده.

هدف این مرحله تعیین وضعیت واقعی Persistence و EF Mapping برای:

- `WalletAssignment`
- `WalletProgram`
- `WalletReservation`

است تا بتوانیم تصمیم نهایی درباره Atomicity، Concurrency و Idempotency بگیریم.

## Scope

فقط موارد زیر را بررسی کن:

### 1. Entity Mapping

برای هر سه Entity بررسی کن:

- Primary Key
- Foreign Key
- Required / Nullable بودن فیلدهای مرتبط
- Decimal precision
- Navigationها
- `RowVersion`
- Computed properties
- Default values
- Delete behavior

### 2. Database Schema

اگر Schema یا Migration واقعی در Repository موجود است، فقط بخش‌های مربوط به این سه Entity را بررسی کن.

مشخص کن:

- Tableهای واقعی
- Columnهای مرتبط
- Indexها
- Unique Constraintها
- Foreign Key Constraintها
- RowVersion / Timestamp configuration
- Computed Columnها

اگر Schema واقعی در Repository موجود نیست، صریحاً بگو چه چیزی قابل تأیید نیست.

### 3. Idempotency

فقط بررسی کن آیا برای:

`WalletReservation.IdempotencyKey`

قید Unique یا Index موجود است یا خیر.

اگر وجود دارد:

- Scope آن را مشخص کن.

اگر وجود ندارد:

- فقط گزارش کن.
- هنوز Migration یا Index ایجاد نکن.

### 4. Concurrency

بررسی کن `RowVersion` در Entity و EF Configuration چگونه Mapping شده است.

مشخص کن:

- آیا Optimistic Concurrency واقعاً فعال است؟
- آیا Database Column متناظر وجود دارد؟
- آیا Update روی Assignment/Program می‌تواند concurrency conflict ایجاد کند؟
- آیا در Repository الگوی دیگری برای Atomic Conditional Update وجود دارد؟

فقط الگوهای موجود را گزارش کن؛ هنوز روش نهایی را انتخاب یا پیاده‌سازی نکن.

### 5. Transaction

فقط بررسی کن در Repository/Infrastructure فعلی چه الگوی Transaction برای عملیات چند Entity وجود دارد.

به‌خصوص نمونه‌های واقعی مرتبط با:

- Begin/Commit/Rollback
- `TransactionScope`
- EF Core transaction
- چند تغییر Entity در یک transaction

را پیدا کن.

از الگوی موجود نمونه بیاور و محل آن را مشخص کن.

### 6. Available Amount

بررسی کن `AvailableAmount` در واقعیت:

- Column است؟
- Computed Column است؟
- `[NotMapped]` است؟
- Property معمولی است؟
- از Query محاسبه می‌شود؟

همچنین مشخص کن مقدار:

`Assigned - Consumed - Reserved - Expired`

در Code یا Database واقعاً چگونه محاسبه می‌شود.

## محدودیت بسیار مهم

در این مرحله:

- هیچ Source Code را تغییر نده.
- هیچ Migration ایجاد نکن.
- هیچ Index ایجاد نکن.
- هیچ Database را تغییر نده.
- هیچ Entity را اصلاح نکن.
- هیچ Specification را تغییر نده.
- هیچ Test ایجاد نکن.
- هیچ Implementation انجام نده.
- هیچ تصمیم Business جدیدی نگیر.

فقط **واقعیت موجود Repository را استخراج کن.**

اگر چیزی در Repository قابل مشاهده نیست، آن را حدس نزن و بنویس:

`Not verifiable from current repository.`

## خروجی

گزارش را دقیقاً با این ساختار بده:

### 1. Entity Mapping

| Entity            | مورد | وضعیت واقعی |
| ----------------- | ---- | ----------- |
| WalletAssignment  | ...  | ...         |
| WalletProgram     | ...  | ...         |
| WalletReservation | ...  | ...         |

### 2. Database Schema

- Tables
- Columns
- FK
- Indexes
- Unique Constraints
- RowVersion
- Computed Columns

### 3. Idempotency

- Current state
- Unique constraint/index
- Scope
- Gap

### 4. Concurrency

- RowVersion mapping
- Existing concurrency mechanism
- Relevant repository pattern
- Gaps

### 5. Transaction

- Existing transaction pattern
- Relevant example
- Suitability for Reserve

### 6. AvailableAmount

- Actual implementation
- Formula
- Source of truth

### 7. Technical Conclusions

فقط نتیجه‌های فنی که مستقیماً از بررسی Repository به دست آمده‌اند.

### 8. Decisions Still Required

فقط مواردی که بعد از این بررسی هنوز نیاز به تصمیم دارند.

### 9. Implementation Readiness

یکی از این دو:

`READY FOR FINAL SPECIFICATION`

یا

`NOT READY — TECHNICAL INFORMATION MISSING`

در این مرحله هیچ Implementation انجام نده.


---

## assistant
فقط سه Entity، ارجاع‌های EF/Schema به آن‌ها و نمونه‌های محدود Transaction و به‌روزرسانی شرطی را بررسی می‌کنم. نتیجه را به «آنچه در سورس دیده می‌شود» و «آنچه بدون Schema واقعی قابل تأیید نیست» تفکیک خواهم کرد.

---

## assistant
در سورس موجود، سه کلاس Entity و propertyهایشان دیده می‌شوند، اما DbContext/Mapping و Migration مربوط به آن‌ها پیدا نشده است. یک جست‌وجوی نهاییِ محدود برای فرمول `AvailableAmount`، تنظیم concurrency و نوع Transaction انجام می‌دهم تا نبود شواهد را دقیق گزارش کنم.

---

## assistant
### 1. Entity Mapping

جدول زیر **تعریف کلاس‌های C#** را نشان می‌دهد؛ وجود همین تنظیمات در EF یا Database بدون Mapping قابل تأیید نیست.

| Entity | مورد | وضعیت واقعی |
| --- | --- | --- |
| [WalletAssignment](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletAssignment.cs) | Primary Key | `Id: int` و `IBaseEntity<int>`؛ تنظیم واقعی PK در EF قابل تأیید نیست. |
| WalletAssignment | FK و Nullable | `WalletProgramId` و `UserId` از نوع `int`؛ `ParentId` از نوع `int?`. قید FK واقعی قابل تأیید نیست. |
| WalletAssignment | مبالغ | `AssignedAmount`، `ConsumedAmount`، `ReservedAmount` و `ExpiredAmount` از نوع `decimal`؛ `AvailableAmount` از نوع `decimal?`. Precision تنظیم‌شده دیده نشد. |
| WalletAssignment | Navigation | `WalletProgram`، `User`، `Parent` و collectionهای Assignment، Reservation و Transaction. |
| WalletAssignment | RowVersion / Computed | `RowVersion: byte[]`؛ `AvailableAmount` یک property معمولی C# با getter/setter است. تنظیم concurrency یا computed column دیده نشد. |
| WalletAssignment | Default / Delete | مقدار پیش‌فرض صریح برای scalarها یا تنظیم Delete Behavior دیده نشد؛ constructor فقط collectionها را مقداردهی می‌کند. |
| [WalletProgram](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletProgram.cs) | Primary Key | `Id: int` و `IBaseEntity<int>`؛ تنظیم واقعی PK در EF قابل تأیید نیست. |
| WalletProgram | FK و Nullable | `ParentId: int?`؛ فیلدهای ارجاعی مشترک و Navigationهای متناظر وجود دارند. قید FK واقعی قابل تأیید نیست. |
| WalletProgram | مبالغ | `TotalAmount`، `AllocatedAmount`، `ConsumedAmount`، `ReservedAmount` و `ExpiredAmount` از نوع `decimal`؛ Precision دیده نشد. |
| WalletProgram | Navigation | `Parent`، collectionهای Assignment، Scope، CardGroup و CouponBatch و Navigationهای مشترک/نوع وضعیت. |
| WalletProgram | RowVersion / Computed | `RowVersion: byte[]`؛ property محاسباتی برای مانده دیده نشد. تنظیم concurrency دیده نشد. |
| WalletProgram | Default / Delete | مقدار پیش‌فرض صریح scalar یا Delete Behavior دیده نشد. |
| [WalletReservation](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs) | Primary Key | `Id: int` و `IBaseEntity<int>`؛ تنظیم واقعی PK در EF قابل تأیید نیست. |
| WalletReservation | FK و Nullable | `WalletAssignmentId`، `UserId` و `OrderId` از نوع `int`؛ `OrderItemId` و `ParentId` از نوع `int?`. قید FK واقعی قابل تأیید نیست. |
| WalletReservation | مبلغ و تاریخ | `Amount: decimal` بدون Precision قابل مشاهده؛ `ExpiresAt` غیر nullable و `CapturedAt`/`ReleasedAt` nullable در C#. |
| WalletReservation | Navigation | Assignment، User، Order، OrderItem، Parent و collectionهای Reservation و Transaction، به‌علاوه Navigationهای مشترک. |
| WalletReservation | RowVersion / Computed | `RowVersion: byte[]`؛ computed property دیده نشد. `IdempotencyKey` یک `string` معمولی است. |
| WalletReservation | Default / Delete | مقدار پیش‌فرض صریح scalar یا Delete Behavior دیده نشد. |

`string` و `byte[]` در این کلاس‌ها annotation مربوط به Required/Nullable ندارند؛ **nullable بودن ستون‌های Database از نوع C# به‌تنهایی قابل تأیید نیست**.

### 2. Database Schema

در نسخهٔ موجود فایل DbContext، Migration یا SQL Schema برای این سه Entity پیدا نشد. پروژهٔ `Yashil.Infrastructure` که Commerce به آن ارجاع می‌دهد نیز در نسخهٔ موجود نیست. [WebHost Startup](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:465) فقط ثبت `YashilAppDbContext` با SQL Server را نشان می‌دهد.

- **Tables:** `Not verifiable from current repository.`
- **Columns و نوع/precision/nullable واقعی:** `Not verifiable from current repository.`
- **FK و Delete Behavior:** `Not verifiable from current repository.`
- **Indexes:** `Not verifiable from current repository.`
- **Unique Constraints:** `Not verifiable from current repository.`
- **RowVersion/Timestamp ستون‌های واقعی:** `Not verifiable from current repository.`
- **Computed Columns و Defaultهای Database:** `Not verifiable from current repository.`

### 3. Idempotency

- **Current state:** `WalletReservation` دارای property به نام `IdempotencyKey` است.
- **Unique constraint/index:** در سورس موجود تنظیمی برای آن دیده نشد؛ وجود یا نبود قید در Database، `Not verifiable from current repository.`
- **Scope:** `Not verifiable from current repository.`
- **Gap:** هیچ Mapping یا Schema موجودی تضمین یکتایی این کلید را نشان نمی‌دهد.

### 4. Concurrency

- **RowVersion mapping:** هر سه Entity دارای `byte[] RowVersion` هستند، اما `[Timestamp]`، `IsRowVersion()` یا تنظیم concurrency مرتبط در سورس موجود دیده نشد. فعال بودن optimistic concurrency و وجود ستون متناظر، `Not verifiable from current repository.`
- **Existing concurrency mechanism:** در بخش مرتبط، سازوکار Wallet برای مدیریت conflict یا `DbUpdateConcurrencyException` پیدا نشد. بنابراین اینکه Update روی Assignment یا Program conflict ایجاد می‌کند، قابل تأیید نیست.
- **Relevant repository pattern:** در [FactorPaymentInstallmentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/FactorPaymentInstallmentService.cs:163) نمونهٔ `ExecuteUpdateAsync` روی Query فیلترشده وجود دارد. این فقط توانایی استفاده از به‌روزرسانی شرطی را در Commerce نشان می‌دهد؛ تضمین concurrency برای Wallet نیست.
- **Gaps:** نگاشت EF، تعریف ستون‌های واقعی و نحوهٔ رسیدگی به conflict مشخص نیست.

### 5. Transaction

- **Existing transaction pattern:** سرویس‌ها از `IUnitOfWork.BeginTransactionAsync()` و سپس `CommitAsync()` یا `RollbackAsync()` استفاده می‌کنند. نمونهٔ `TransactionScope` در سورس بررسی‌شده پیدا نشد.
- **Relevant example:** [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:174) پاک‌کردن سبد، ساخت آدرس و درج/به‌روزرسانی سفارش را در یک بلوک transaction انجام می‌دهد. [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:222) نیز چند عملیات Repository/Service را با همین الگو اجرا می‌کند.
- **Suitability for Reserve:** این یک الگوی موجود برای طراحی عملیات چند Entity است. پیاده‌سازی `IUnitOfWork` و اینکه تمام عملیات موردنیاز Reserve واقعاً در یک EF transaction شرکت می‌کنند، `Not verifiable from current repository.`

### 6. AvailableAmount

- **Actual implementation:** در Entity، `AvailableAmount` یک `decimal?` با getter/setter معمولی است؛ `[NotMapped]` یا تنظیم computed در سورس موجود دیده نشد. Column بودن یا computed column بودن آن، `Not verifiable from current repository.`
- **Formula:** محاسبهٔ `Assigned − Consumed − Reserved − Expired` در کد موجود برای این سه Entity پیدا نشد.
- **Source of truth:** سورس موجود فقط propertyها را نشان می‌دهد؛ منبع واقعی مقدار در زمان اجرا، `Not verifiable from current repository.`

### 7. Technical Conclusions

وجود `Id`، Navigation، `RowVersion` و `IdempotencyKey` در Entity به‌تنهایی وجود PK/FK، کنترل optimistic concurrency یا قید یکتا در Database را اثبات نمی‌کند. Repository الگوی transaction و `ExecuteUpdateAsync` دارد، اما هیچ پیاده‌سازی Persistence برای Reserve در سورس موجود دیده نشد.

### 8. Decisions Still Required

برای انتخاب روش فنی Reserve، ابتدا Mapping یا Schema واقعی این سه Entity لازم است؛ به‌ویژه PK/FKها، precision، computed بودن `AvailableAmount`، پیکربندی `RowVersion` و Index/Unique Constraint کلید idempotency. پس از آن باید Scope یکتایی کلید و ترکیب مناسب transaction و کنترل هم‌زمانی تعیین شود.

### 9. Implementation Readiness

`NOT READY — TECHNICAL INFORMATION MISSING`

---

## user

# Files pasted by the user:

## "هدف این مرحله فقط این است که قبل از پیاده‌سازی Wallet، نحوه استفاده از زیرساخت‌…": C:\Users\pc\.codex/attachments/4ea1e747-778a-4566-a2a2-3561dd58bd14/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن پیوست را می‌خوانم و محدودهٔ بررسی این مرحله را از آن مشخص می‌کنم.

---

## assistant
طبق متن پیوست، فقط نحوهٔ استفادهٔ پروژه از زیرساخت‌های Package را از چند Feature واقعی استخراج می‌کنم. برای هر Convention، نمونه‌های هم‌خوان و تفاوت الگوها را جدا ثبت می‌کنم و دربارهٔ رفتار داخلی Genericها نتیجه‌گیری نمی‌کنم.

---

## assistant
چند الگوی روشن پیدا شد: سرویس‌های Commerce معمولاً GenericService را توسعه می‌دهند، Repository اختصاصی دارند و DI آن‌ها دستی در ModuleInitializer ثبت شده است. اکنون فقط نمونه‌های CRUD، Validation و Commit را در همان فایل‌های نماینده تکمیل می‌کنم تا گزارش دربارهٔ رفتار Packageها حدس نزند.

---

## assistant
### 1. Service Convention

- **Base/Inheritance و DI:** [ProductService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ProductService.cs:30)، [CategoryService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/CategoryService.cs:20) و `HekmatTransactionService` از `GenericService<TEntity, int>` ارث می‌برند و interface متناظر را پیاده می‌کنند. Constructor، `IUnitOfWork`، Repository، `IUserPrincipal` و `IEventPublisher` را به `base` می‌دهد؛ وابستگی‌های Business را جداگانه inject می‌کند.
- **Generic Service و CRUD:** نمونه‌ها از `GetAll(true)`، `Get`، `GetAsync` و `FindById` استفاده می‌کنند. `OrderService`، `AddAsync`/`UpdateAsync` و `ProductService`، `UpdateAsync`/`DeleteAsync` را override کرده و برای رفتار اضافی به `base` فراخوانی می‌کنند. متد واحدی به نام `GetById` در نمونه‌های بررسی‌شده دیده نشد.
- **Query:** سرویس گاهی Query را به Repository واگذار می‌کند و گاهی روی `GetAll` فیلتر می‌گذارد؛ `Include` عمدتاً در Repository نمونه دیده شد. `ProjectTo` در Controllerهای بررسی‌شده انجام می‌شود.
- **Save/Commit، Validation و Transaction:** `SaveChangeAsync()` در سرویس‌ها استفاده می‌شود؛ `OrderService` نشان می‌دهد پرچم `saveAfterAdd/Update` زمان ذخیره را کنترل می‌کند. اعتبارسنجی Business هم در متد سرویس دیده می‌شود و هم در hookهای Controller. برای چند تغییر، `IUnitOfWork.BeginTransactionAsync()` با Commit/Rollback استفاده شده است ([OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:174)).

### 2. Repository Convention

- **Generic Repository usage:** [ProductRepository](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/ProductRepository.cs:16) و `CategoryRepository` از `GenericApplicationBasedRepository`، و `HekmatTransactionRepository` از `GenericRepository` ارث می‌برند. هرکدام `YashilAppDbContext` را inject می‌کنند.
- **Query:** نمونه‌ها از `DbSet` یا `_context.Set<Product>()`، `Where`، `AsNoTracking` و `Include` استفاده می‌کنند. انتخاب بین Repository عمومی و ApplicationBased در نمونه‌ها یکسان نیست.
- **AddAsync / Update / Delete:** [CategoryRepository.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/CategoryRepository.cs:222) پیش از `base.AddAsync` مقدار مرتبط را تنظیم می‌کند. `ProductRepository` نمونهٔ Update مستقیم روی Query دارد. حذف در نمونه‌های بررسی‌شده عمدتاً از مسیر Service و `base.DeleteAsync` انجام می‌شود.
- **SaveChanges / Transaction:** فراخوانی مستقیم `SaveChanges` در Repositoryهای Commerce بررسی‌شده پیدا نشد؛ ذخیره و transaction در نمونه‌های Service انجام می‌شود. رفتار داخلی Generic Repository یا UnitOfWork از این Usage قابل استنتاج قطعی نیست.

### 3. Controller Convention

- **BaseController و CRUD:** [ProductController](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/ProductController.cs:35)، `CategoryController` و `FactorPaymentController` از `BaseController<Entity, int, ListViewModel, EditModel, SimpleViewModel>` ارث می‌برند و سرویس، Mapper، Logger و PermissionService را به `base` می‌دهند. جزئیات endpointهای CRUD پایه چون در Package هستند، **Not found** در Repository.
- **Validation:** [FactorPaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/FactorPaymentController.cs:78) هر دو hook درج و ویرایش را override می‌کند و پیام خطا یا نتیجهٔ `base` را برمی‌گرداند. [CategoryController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/CategoryController.cs:134) hook ویرایش/حذف و hookهای پس از درج و پیش از Mapping را به کار می‌برد.
- **Authorization:** اکشن‌های سفارشی نمونه از `[YashilAuthorization]` و گاهی `ActionName` استفاده می‌کنند. هویت کاربر از `IUserPrincipal` خوانده می‌شود؛ کنترل دسترسی به رکورد نیز در بعضی Controllerها صریح است.
- **Response/Error:** برای فهرست‌ها `LoadResult` همراه `ProjectTo` و `DataSourceLoader` رایج است. [HekmatTransactionController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/HekmatTransactionController.cs:85) برای عملیات سفارشی `ResultStatus<int>` برمی‌گرداند. یک قالب پاسخ/خطای واحد برای همهٔ عملیات سفارشی در نمونه‌ها دیده نشد.

### 4. Dependency Injection Convention

- **Service و Repository:** جفت‌های interface/implementation با `AddScoped` به‌صورت دستی در [ModuleInitializer.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs:54) ثبت می‌شوند؛ مانند `IProductRepository/ProductRepository` و `IProductService/ProductService`.
- **Controller:** WebHost سرویس‌های MVC را ثبت می‌کند؛ ثبت دستی Controllerهای نمونه دیده نشد. WebHost، ModuleInitializerها را کشف و اجرا می‌کند ([Startup.cs](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:480)). Assembly scanning مشاهده‌شده مربوط به کشف ModuleInitializer و در بخش دیگری Consumerهاست، نه ثبت خودکار Service/Repositoryهای Commerce.
- **نام‌گذاری:** `I{Name}Service`/`{Name}Service` و `I{Name}Repository`/`{Name}Repository` در چند نمونه تکرار می‌شود.

### 5. Wallet Persistence

- **WalletAssignment، WalletProgram، WalletReservation:** هر سه [Entity](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletAssignment.cs) در `Yashil.Core` موجودند.
- **Existing DbSet / Mapping / Repository / Configuration:** Repository اختصاصی، سرویس و EF Configuration مربوط به این سه Entity در سورس موجود **Not found**. سورس `YashilAppDbContext` در این نسخه **Not found**؛ بنابراین وجود DbSet یا Mapping آن‌ها در زیرساخت واقعی قابل تأیید نیست. SQL Schema و Migration مربوط نیز **Not found**.
- **امکان استفادهٔ Generic:** `_context.Set<Product>()` در ProductRepository نشان می‌دهد کد پروژه به typed DbSet محدود نیست. اما نمونه‌ای که نشان دهد Wallet بدون Repository اختصاصی و بدون تأیید Mapping با Generic infrastructure قابل استفاده است، **Not found**.

### 6. Missing Infrastructure

در Repository فعلی، پیاده‌سازی Wallet برای این سه Entity، Repository/Service مرتبط، EF Mapping/DbContext source و Schema/Migration مربوط **Not found**. [WalletController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs) وجود دارد، اما خالی است. سورس داخلی Generic Packageها نیز در Repository نیست و در این بررسی مبنای فرض قرار نگرفت.

### 7. Recommended Wallet Implementation Pattern

در صورت تأیید Specification، مسیر مشاهده‌شدهٔ Commerce این است: قراردادهای لازم در Core؛ Service و در صورت نیاز Repository اختصاصی در Infrastructure با استفاده از Generic base و الگوی `IUnitOfWork`؛ ثبت دستی interface/implementation در `ModuleInitializer`؛ و فقط اگر API در Scope باشد، Controller با ViewModel، Authorization و Validation متناسب با endpoint. انتخاب Repository اختصاصی، نوع Controller و شکل پاسخ باید از نیاز همان Feature و رفتار Persistence قابل تأیید تعیین شود.

### 8. Readiness

`CONVENTIONS FOUND — READY FOR SPECIFICATION`

**تصمیم‌های انسانی باقی‌مانده برای Wallet Reserve:** فراخواننده و نیاز به API؛ Scope و پاسخ idempotency؛ سطح Order/OrderItem و تعدد رزرو؛ سیاست انقضا؛ و قرارداد خطا و دسترسی. انتخاب فنی کنترل هم‌زمانی نیز به اطلاعات واقعی Mapping/Schema وابسته است. هیچ فایل یا کدی تغییر نکرد.

---

## user

# Files pasted by the user:

## "در مرحله قبل Conventionهای Service / Repository / Controller و DI پروژه را یاد …": C:\Users\pc\.codex/attachments/871d7650-e540-4602-b03d-e42691390e98/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن پیوست را می‌خوانم و ادامهٔ کار را در همان محدودهٔ Discovery نگه می‌دارم.

---

## assistant
Checkout، Order و Payment را فقط در مسیرهای مرتبط Commerce دنبال می‌کنم. تصمیم‌های قطعی شما دربارهٔ idempotency، سطح Order و انقضای ۳۰ دقیقه را ثابت می‌گیرم و فقط محل اتصال و Conventionهای فعلی را از کد استخراج می‌کنم.

---

## assistant
دو نقطهٔ متفاوت پیدا شد: صفحهٔ Checkout در بخش ECommerce آماده می‌شود، اما ثبت سفارش در `ECommerceAdminApi/OrderController` انجام می‌شود. مسیر پرداخت نیز بعد از ساخته‌شدن Order، Payment جداگانه ایجاد می‌کند. اکنون این سه مرز را با کدهای کوتاهِ همان مسیر تطبیق می‌دهم.

---

## assistant
نقطهٔ مهم برای Reserve روشن‌تر شد: Order می‌تواند هم هنگام ثبت سبد ساخته شود و هم بعداً از فهرست سفارش‌ها برای پرداخت دوباره وارد شود؛ مبلغ پرداخت در `CreateNewPaymentAndReturnAsync` محاسبه می‌شود. اکنون Timeout و الگوهای جلوگیری از درخواست تکراری را بررسی می‌کنم، چون قفل کوتاه‌مدت Checkout لزوماً معادل idempotency مالی نیست.

---

## assistant
## Handoff Summary

### Active task

Continue the **read-only discovery** of the existing `Checkout → Order → Payment` flow for the Wallet Reserve pilot. The latest request is in `C:\Users\pc\.codex\attachments\871d7650-e540-4602-b03d-e42691390e98\Pasted text.txt`. Deliver the report in its exact nine-section structure. Do not edit code, create files, write a Specification, or implement anything.

### Fixed business decisions from the user

- Reserve is idempotent: identical key and request replay the previous reservation/result; same key with different parameters returns conflict.
- Pilot Reserve is **Order-level**; do not use `OrderItemId`.
- Reservation lifetime is **30 minutes** from creation. Future implementation should contain a TODO to replace this temporary policy; configuration is not required unless existing conventions demand it.

### Findings so far

- Public Checkout page: `backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/OrderController.cs:250` calls `OrderDataFactory.PrepareCheckoutViewModelByRecipientIdAsync`. `OrderSummery` calls `FillOrderSummaryByRecipientId`.
- Order submission: `.../Areas/ECommerceAdminApi/Controllers/OrderController.cs:143`, `SaveShoppingCartToOrder`. It validates cart/recipient/payment method, calculates item prices including discounts and subsidies, creates an `Order` with `UserId = _userPrincipal.Id` and `InitialRegistration` or `RequiredSupplierApprovalBeforePayment`, builds parent/child `OrderItem`s, then calls `_orderService.AddOrderAndClearShoppingCartAsync` at line 538. Existing orders can enter the same action for later payment (around line 562). It returns a payment redirect URL; supplier approval can defer payment.
- `OrderService.cs:172` inserts Order and clears cart within `IUnitOfWork.BeginTransactionAsync`/Commit/Rollback. `OrderService.cs:552` `CreateNewPaymentAndReturnAsync` first calls `CalcAmountForPayAsync`, rejects nonpositive total, creates a Payment with `SendToPaymentGateway`, saves it and its reservation number in a transaction.
- `OrderService.cs:412` calculates amount from order totals and subtracts already paid payments; subsidy and payment method change the selected total/sale type. `OrderDataFactory.cs:651` builds Checkout view. Need inspect `FillOrderSummaryByRecipientId` or `OrderItemService.CreateOrderItemEntityAsync` only if more detail on discounts is needed.
- Gateway example `SamanPaymentController.cs:54`: receives Order ID, calls `CreateNewPaymentAndReturnAsync`, obtains token and redirects. Anonymous callback at line 141 checks prior reference/payment state, updates Payment to `WaitingResponseFromPaymentGateway`, then `Paid` or `Unsuccessful`/`Cancel`; success updates Order to `Complete` (`~328`). `PaymentService.cs:84` updates status, timestamps, and related item shares, and prevents some repeated updates of already paid non-Wallet payments.
- Legacy `PayWithWallet` in AdminApi `OrderController.cs:803` uses `CustomerClubCard` and existing `PaymentMethodTypesEnum.Wallet`; it is **not evidence of the MVP WalletReservation flow**.
- Authorization varies: `IUserPrincipal.Id` creates Order. `OrderService.CheckUserHasAccessForOrder` at `:942` accepts owner, some users without company, or company/representation access to an item. AdminApi `PaymentController` calls it and has `[YashilAuthorization]`; `SaveShoppingCartToOrder` and gateway Index have no explicit attribute visible. Do not infer package/global authorization. Exact Reserve ownership layer remains unresolved.
- Expiration: `SaveShoppingCartToOrder` has an in-memory cart lock expiring after **30 seconds** (`OrderController.cs:645–682`), unrelated to the user’s 30-minute Reservation policy. `UpdateExpiredOrderStatusJob` is conditionally scheduled via `ModuleInitializerPartial.cs:89` with a one-minute cron; it calls `OrderService.CancelExpiredOrdersAsync`, which calls an unseen SQL procedure via `OrderRepository.cs:350`. No relevant payment/checkout expiration setting found in `StoreConfiguration` search. `DateTime.Now` is common; `HekmatPaymentController` also uses `UtcNow`, so convention is mixed.
- Idempotency-like examples: cart `ConcurrentDictionary` lock; `PaymentTransactionUtil` dictionary used by Saman callback, duplicate gateway ref lookup, and `PaidDateUtc` replay check (`SamanPaymentController.cs:165–228`); duplicate order/item checks in AdminApi OrderController `:972`. None establishes Wallet Reserve idempotency or a persistent unique key. `WalletReservation.IdempotencyKey` is only a property in the supplied source.
- Candidate request identity fields based on model/flow: `OrderId`, `WalletAssignmentId`, `Amount`, `UserId`; consider `ApplicationId` because entities are application-based. `ExpiresAt` is server-derived and should not vary matching on retry. Present this as a **recommendation**, not a finalized specification.

### Remaining work

1. Optionally inspect narrowly:
   - `OrderDataFactory.FillOrderSummaryByRecipientId` and/or `OrderItemService.CreateOrderItemEntityAsync` to establish where discounts enter final totals.
   - `PaymentTransactionUtil.cs`, `PaymentRepository.CheckExitsByRefNumberAsync`, `StoreConfiguration.cs`, `YashilEnums.cs:303` and `:575` only to pin down concise idempotency/status/timeout evidence.
2. Synthesize the requested nine sections exactly: Checkout Flow, Order Flow, Payment Flow, Authorization Pattern, Expiration Pattern, Existing Idempotency Patterns, Recommended Reserve Integration Point, Remaining Decisions, Readiness.
3. Recommend the **boundary after persisted Order and final payable amount calculation, before Payment creation/gateway dispatch**, while noting the exact call site and payment method for the new Wallet MVP remain a decision. Do not conflate the existing CustomerClub Wallet payment path with the new Reserve flow.
4. Likely readiness: `READY FOR SPECIFICATION` if the remaining narrow checks confirm enough flow detail. Human decisions may remain about caller/authorization and how the new Wallet payment method enters the existing payment flow. No code or files have been changed.

---

## assistant
در ادامه، متن کامل درخواست پیوست‌شده را می‌خوانم و فقط نقاط لازم از مسیر Checkout، Order و Payment را با گزارش قبلی تطبیق می‌دهم. خروجی صرفاً گزارش فنی خواهد بود و فایلی تغییر نمی‌کند.

---

## assistant
برای این بررسی از راهنمای `wallet-feature` استفاده می‌کنم و فقط بخش‌های مرتبط آن را مبنا می‌گذارم. اکنون چند نقطه محدود از کد را برای مبلغ نهایی، مجوز دسترسی، وضعیت پرداخت و زمان انقضا تأیید می‌کنم.

---

## assistant
کد نشان می‌دهد سفارش ابتدا همراه آیتم‌ها ثبت می‌شود و مسیر پرداخت، مبلغ قابل‌پرداخت را جداگانه از سفارش محاسبه می‌کند. همچنین `Wallet` موجود در مسیر پرداخت به کارت باشگاه مشتریان مربوط است؛ آن را معادل Wallet Reserve جدید در نظر نمی‌گیرم.

---

## assistant
### 1. Checkout Flow

صفحه Checkout از `OrderDataFactory` برای نمایش سبد، قیمت، تخفیف و یارانه استفاده می‌کند. در [OrderDataFactory.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/DataFactory/OrderDataFactory.cs:501>) جمع مبلغ و تخفیف برای نمایش محاسبه می‌شود. ثبت نهایی در [SaveShoppingCartToOrder](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:142>) انجام می‌شود؛ این مسیر قیمت‌ها را دوباره محاسبه می‌کند، Order و آیتم‌هایش را می‌سازد و سپس URL مسیر پرداخت را برمی‌گرداند.

### 2. Order Flow

Order با `UserId` کاربر جاری، `ApplicationId` و جمع مبالغ با/بدون یارانه ساخته می‌شود. وضعیت اولیه آن `InitialRegistration` یا، در صورت نیاز به تأیید تأمین‌کننده، `RequiredSupplierApprovalBeforePayment` است. آیتم‌های والد و فرزند پیش از ذخیره به Order افزوده می‌شوند. [OrderService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:172>) ثبت Order و پاک‌کردن سبد را در یک transaction انجام می‌دهد. سفارشِ نیازمند تأیید، همان لحظه به پرداخت هدایت نمی‌شود؛ سفارش ذخیره‌شده نیز بعداً می‌تواند دوباره وارد مسیر پرداخت شود.

### 3. Payment Flow

[CalcAmountForPayAsync](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412>) مبلغ قابل‌پرداخت را از جمع Order، وضعیت یارانه، روش پرداخت و پرداخت‌های قبلاً موفق محاسبه می‌کند. `CreateNewPaymentAndReturnAsync` فقط برای مبلغ مثبت، رکورد Payment را با وضعیت `SendToPaymentGateway` در transaction می‌سازد. در نمونه درگاه سامان، پس از آن توکن گرفته و کاربر هدایت می‌شود؛ callback وضعیت را به `WaitingResponseFromPaymentGateway` و سپس `Paid`، `Unsuccessful` یا `Cancel` می‌برد. در موفقیت، Order به `Complete` می‌رسد. [SamanPaymentController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:54>) نمونه این مسیر است.

### 4. Authorization Pattern

کاربر جاری از `IUserPrincipal.Id` شناخته می‌شود و هنگام ساخت Order در `Order.UserId` ثبت می‌شود. در API جزئیات پرداخت، `[YashilAuthorization]` همراه با `OrderService.CheckUserHasAccessForOrder` استفاده شده است؛ آن متد علاوه بر مالک، برخی دسترسی‌های سازمانی/نمایندگی را نیز می‌پذیرد. در مسیرهای بررسی‌شده، کنترل دسترسی یکسانی در همه Controllerها دیده نمی‌شود. بنابراین Specification باید تعیین کند Reserve فقط برای **مالک Order** مجاز است یا همان دسترسی گسترده‌تر Order نیز کافی است، و این کنترل در کدام مرز Service/Controller الزام‌آور می‌شود. [PaymentController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/PaymentController.cs:35>)؛ [OrderService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:942>)

### 5. Expiration Pattern

قفل درون‌حافظه‌ای ثبت سبد **۳۰ ثانیه** عمر دارد و به Reservation مربوط نیست. Job لغو Orderهای منقضی، در صورت فعال‌بودن تنظیم `UpdateExpiredOrderStatus`، با cron یک‌دقیقه‌ای اجرا می‌شود؛ منطق SQL فراخوانی‌شده برای آن در این Discovery بررسی نشده است. در کد، هم `DateTime.Now` و هم `UtcNow` دیده می‌شود؛ قرارداد زمانی یکدستی برای Reservation از این مسیر قابل استخراج نیست. تصمیم قطعی این Pilot همان `ExpiresAt = CreationTime + 30 minutes` است و باید با TODO سیاست موقت ثبت شود. [OrderController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:650>)؛ [ModuleInitializerPartial.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializerPartial.cs:89>)

### 6. Existing Idempotency Patterns

ثبت سبد از `ConcurrentDictionary` برای جلوگیری موقت از درخواست هم‌زمان استفاده می‌کند. callback سامان نیز درخواست در حال پردازش، شماره مرجع تکراری و پرداخت قبلاً تأییدشده را بررسی می‌کند. این‌ها الگوی پایدارِ replay نتیجه Reserve نیستند. در بخش‌های بررسی‌شده، `IdempotencyKey` فقط به‌صورت property در [WalletReservation.cs](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs:62>) دیده شد؛ convention اجرایی برای آن **Not found**.

برای تشخیص «همان درخواست»، تطبیق `ApplicationId`، `UserId`، `OrderId`، `WalletAssignmentId` و `Amount` پیشنهاد می‌شود، چون این فیلدها در مدل Reservation و مسیر Order وجود دارند. `ExpiresAt` مقدار مشتق‌شده در سرور است و نباید retry یک درخواست یکسان را متفاوت کند. محدوده یکتایی خود کلید هنوز باید در Specification تعیین شود.

### 7. Recommended Reserve Integration Point

مرز مناسب، **پس از ذخیره Order و مشخص‌شدن مبلغ نهایی قابل‌پرداخت، و پیش از ایجاد Payment یا ارسال به درگاه** است. در این نقطه `OrderId` واقعی وجود دارد، تأیید لازم می‌تواند لحاظ شود و مبلغ از منطق موجود [OrderService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552>) قابل اخذ است. اتصال دقیق مسیر جدید به انتخاب روش پرداخت هنوز تصمیم Specification است. مسیر `PaymentMethodTypesEnum.Wallet` فعلی به پرداخت کارت باشگاه مشتریان وصل است و شاهدی برای Wallet Reserve جدید محسوب نمی‌شود.

### 8. Remaining Decisions

- مجوز Reserve: صرفاً مالک Order یا دسترسی گسترده‌تر موجود؛ و محل الزام کنترل.
- روش ورود Wallet Reserve به مسیر پرداخت و نسبت آن با `Wallet` قدیمی.
- محدوده یکتایی `IdempotencyKey` و شکل نتیجه‌ای که در retry بازپخش می‌شود.
- قرارداد زمانی ذخیره و مقایسه `CreationTime` و `ExpiresAt`.

### 9. Readiness

`READY FOR SPECIFICATION` — محل ادغام و قراردادهای موجود مشخص‌اند؛ موارد بخش ۸ باید در Specification نهایی تعیین شوند. هیچ فایل یا کدی تغییر نکرد.

---

## user

# Files pasted by the user:

## "اکنون Discovery مربوط به Wallet Reserve کامل شده است. بر اساس تمام Discoveryهای…": C:\Users\pc\.codex/attachments/30c52126-5b36-46b9-9fd5-2cffe1735577/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن درخواست پیوست‌شده را می‌خوانم و خروجی را بر پایه Discoveryهای همین گفتگو و تصمیم‌های قطعی شما آماده می‌کنم.

---

## assistant
درخواست، تولید Specification در همین پاسخ را می‌خواهد و Plan فنی را به مرحله بعد موکول کرده است. چند نام فیلد و وضعیت موجود را به‌صورت محدود تأیید می‌کنم تا متن Specification با مدل فعلی ناسازگار نباشد.

---

## assistant
## 1. Feature Overview

**Wallet Reserve** برای یک Order ثبت‌شده، مبلغی از اعتبار `WalletAssignment` را موقتاً رزرو می‌کند. این Specification رفتار مطلوب Feature را تعریف می‌کند؛ کد موجود منبع رفتار فعلی سیستم است.

## 2. Business Goal

پیش از ایجاد Payment، اعتبار موردنیاز Order نگه داشته شود تا درخواست‌های هم‌زمان نتوانند همان اعتبار را دوباره مصرف یا رزرو کنند.

## 3. Scope

هر Reserve موفق در این MVP در سطح **Order** است: `OrderId` الزامی و `OrderItemId` استفاده‌نشده است. فیلد موجود `OrderItemId` حفظ می‌شود. Reserve موفق، یک `WalletReservation` با وضعیت `Reserved` ایجاد می‌کند و مبالغ رزروشده Assignment و Program را افزایش می‌دهد.

## 4. Preconditions

- Order با موفقیت ثبت شده و مبلغ نهایی قابل‌پرداخت آن مشخص است.
- کاربر جاری مالک Order است.
- Assignment متعلق به همان کاربر و Application است و ارتباط معتبر آن با Program قابل تأیید است.
- مبلغ درخواستی مثبت و در محدوده اعتبار قابل استفاده است.

## 5. Input Contract

ورودی‌های مؤثر عبارت‌اند از `OrderId`، `WalletAssignmentId`، `Amount` و `IdempotencyKey`. هویت `UserId` و `ApplicationId` باید از زمینه معتبر درخواست به دست آید، نه از ادعای تأییدنشده درخواست‌کننده. `ExpiresAt` ورودی نیست.

مبلغ باید با مبلغ نهایی قابل استفاده برای Wallet در Order سنجیده شود. Reserve منطق قیمت محصول، تخفیف یا یارانه را دوباره محاسبه نمی‌کند؛ منطق موجود [OrderService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412>) مبنای مبلغ قابل‌پرداخت است.

## 6. Output Contract

نتیجه موفق و نتیجه بازپخش‌شده حداقل شامل `ReservationId`، `OrderId`، `Amount`، `Status` و `ExpiresAt` است. خطا باید نوع قابل تشخیصِ اعتبارسنجی، عدم دسترسی، موجودی ناکافی یا تعارض Idempotency را بیان کند. این Specification قالب HTTP یا کلاس پاسخ را تحمیل نمی‌کند.

## 7. Business Rules

- `RequestedAmount > 0`.
- `Available = Assigned - Consumed - Reserved - Expired`.
- `RequestedAmount <= AvailableAmount`.
- پس از Reserve موفق، `Reserved + Consumed + Expired <= Assigned` برقرار می‌ماند.
- هیچ Reserve نباید موجودی قابل استفاده را منفی کند.
- Reserve فقط وضعیت اولیه `Reserved` را ایجاد می‌کند؛ `Capture`، `Release` و `Refund` انجام نمی‌دهد.
- Reserve در این مرحله `WalletTransaction` یا Ledger نهایی ایجاد نمی‌کند.

## 8. Idempotency Rules

`ApplicationId + IdempotencyKey` باید حداقل در MVP یکتا باشد. برای تشخیص یکسان‌بودن درخواست، این مقادیر تطبیق داده می‌شوند: `ApplicationId`، `UserId`، `OrderId`، `WalletAssignmentId` و `Amount`.

- **همان کلید و همان درخواست:** Reservation و نتیجه Business قبلی بازپخش می‌شود؛ رکورد تازه و تغییر مجدد Balance مجاز نیست.
- **همان کلید و درخواست متفاوت:** عملیات با Conflict/Business Error رد می‌شود؛ رکورد و Balance تغییر نمی‌کنند.
- `ExpiresAt` در تطبیق ورودی شرکت ندارد، چون سرور آن را تعیین می‌کند.

## 9. Authorization Rules

تنها مالک Order مجاز است: `Order.UserId == CurrentUser.Id`. شکست این شرط باید پیش از هر تغییر مالی باعث رد عملیات شود. محل دقیق اجرای کنترل در لایه‌ها به مرحله Implementation Plan واگذار می‌شود؛ مسیر داخلی Checkout نیز باید همین قاعده را به‌صورت سازگار رعایت کند.

## 10. Expiration Rules

برای Reservation جدید، `ExpiresAt = CreationTime + 30 minutes` است. این مقدار یک سیاست موقت است. Retry یک درخواست موفق نباید زمان انقضا را از نو شروع کند و باید همان `ExpiresAt` قبلی را برگرداند.

## 11. Transaction & Atomicity Requirements

ایجاد `WalletReservation`، افزایش `WalletAssignment.ReservedAmount` و افزایش `WalletProgram.ReservedAmount` یک واحد اتمیک‌اند: یا هر سه پایدار می‌شوند یا هیچ‌کدام. هر خطای Persistence باید تمام تغییرات همین Reserve را rollback کند. الگوی transaction موجود پروژه `IUnitOfWork` است.

## 12. Concurrency Requirements

در درخواست‌های هم‌زمان، فقط Reserveهایی می‌توانند موفق شوند که مجموع آن‌ها از اعتبار قابل استفاده عبور نکند. نتیجه نباید Negative Balance، Double Spending یا به‌روزرسانی ناقص داشته باشد. روش فنی کنترل رقابت در Implementation Plan و بر اساس Infrastructure واقعی تعیین می‌شود.

## 13. Success Flow

1. Order، مالکیت آن، Assignment و مبلغ نهایی قابل‌پرداخت بررسی می‌شوند.
2. وضعیت Idempotency و اعتبار قابل استفاده بررسی می‌شود.
3. Reservation در وضعیت `Reserved` با انقضای ۳۰ دقیقه‌ای ایجاد می‌شود.
4. مبالغ رزروشده Assignment و Program در همان واحد اتمیک افزایش می‌یابند.
5. نتیجه شامل شناسه، Order، مبلغ، وضعیت و زمان انقضا برگردانده می‌شود.

## 14. Failure Scenarios

| وضعیت | رفتار مورد انتظار |
| --- | --- |
| مبلغ صفر یا منفی | رد درخواست؛ بدون تغییر مالی |
| اعتبار ناکافی | Business Error؛ بدون Reservation یا تغییر Balance |
| Order ناموجود یا غیرمجاز | رد درخواست؛ بدون تغییر مالی |
| عدم تطابق User/Application یا ارتباط نامعتبر Assignment | رد درخواست؛ بدون تغییر مالی |
| کلید تکراری با همان درخواست | بازپخش نتیجه قبلی؛ بدون تغییر مجدد |
| کلید تکراری با درخواست متفاوت | Conflict؛ بدون تغییر |
| رقابت هم‌زمان | فقط درخواست‌های در سقف اعتبار موفق شوند |
| خطای Persistence | rollback کامل Reserve |

## 15. Out of Scope

ایجاد Eligibility یا Assignment، Allocation، Capture، Release، Refund، Ledger نهایی، Coupon/Card، UI، تغییر Gateway، تغییر مسیر قدیمی `PaymentMethodTypesEnum.Wallet`، تغییر Generic Packageها، و طراحی Migration یا Schema در این Specification خارج از Scope هستند. ضرورت احتمالی تغییر Schema در Implementation Plan بررسی می‌شود.

## 16. Integration with Existing Order/Payment Flow

مرز منطقی Reserve پس از ثبت موفق Order و محاسبه مبلغ نهایی، و پیش از ایجاد Payment و هدایت به Gateway است. کد فعلی ثبت Order را در [OrderController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:538>) و ساخت Payment را در [OrderService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552>) انجام می‌دهد. محل اتصال دقیق در Implementation Plan مشخص می‌شود. مسیر `Wallet` فعلیِ متکی به کارت باشگاه مشتریان، پیاده‌سازی این Reserve نیست.

## 17. Acceptance Criteria

1. **Successful Reserve:** Given یک Order مجاز، Assignment معتبر و اعتبار کافی، When Reserve با مبلغ مثبت انجام شود، Then یک Reservation با وضعیت `Reserved` ایجاد شود، هر دو `ReservedAmount` به همان مبلغ افزایش یابند و نتیجه مقرر بازگردد.
2. **Insufficient Balance:** Given اعتبار کمتر از مبلغ درخواست، When Reserve انجام شود، Then خطای موجودی ناکافی برگردد و هیچ داده مالی تغییر نکند.
3. **Invalid Amount:** Given مبلغ صفر یا منفی، When درخواست شود، Then رد شود و هیچ Reservation ایجاد نشود.
4. **Unauthorized Order:** Given مالک نبودن کاربر جاری، When Reserve درخواست شود، Then رد شود و Balance ثابت بماند.
5. **Idempotent Retry:** Given Reserve موفق، When همان کلید و همان پنج پارامتر مؤثر تکرار شود، Then همان Reservation، نتیجه و `ExpiresAt` بازگردند و Balance دوباره تغییر نکند.
6. **Different Request:** Given کلید استفاده‌شده، When یکی از پارامترهای مؤثر تغییر کند، Then Conflict برگردد و داده مالی ثابت بماند.
7. **Concurrent Reserve:** Given اعتبار ۱٬۰۰۰٬۰۰۰ و دو درخواست هم‌زمان ۷۰۰٬۰۰۰ و ۵۰۰٬۰۰۰، When هر دو اجرا شوند، Then هر دو با هم موفق نشوند و invariant موجودی برقرار بماند.
8. **Persistence Failure:** Given خطا در هر یک از سه تغییر اتمیک، When عملیات شکست بخورد، Then هیچ‌یک از سه تغییر باقی نماند.
9. **30-minute Expiration:** Given Reserve موفق، Then `ExpiresAt` دقیقاً ۳۰ دقیقه پس از `CreationTime` ثبت شود؛ retry آن را تغییر ندهد.
10. **Assignment/User mismatch:** Given Assignment متعلق به کاربر یا Application مورد انتظار نیست، When Reserve درخواست شود، Then عملیات بدون تغییر مالی رد شود.

## 18. Open Questions / Conflicts

- تصمیم «یک Reservation برای Order» رفتار درخواست **جدید با کلید متفاوت برای همان Order** را مشخص نمی‌کند؛ باید معلوم شود رد می‌شود یا شرایط دیگری دارد.
- مدل موجود Assignment، `UserId` و `ApplicationId` دارد، اما ارتباط مستقیم Assignment با Order در فیلدهای بررسی‌شده دیده نشد. اگر «ارتباط معتبر با Order» شرطی فراتر از مالک و Application مشترک است، آن شرط باید تعریف شود.
- Discovery نشان داد زمان‌گذاری کد فعلی بین `DateTime.Now` و `UtcNow` یکدست نیست؛ مبنای زمانی ذخیره و مقایسه انقضا باید پیش از پیاده‌سازی مشخص شود.

## 19. Implementation TODOs

`TODO: Replace fixed 30-minute reservation expiration with the final Checkout/Payment expiration policy.`

**SPECIFICATION READY FOR REVIEW**

---

## user

# Files pasted by the user:

## "حالا بر اساس Specification تأییدشده‌ی Feature زیر، فقط یک **Implementation Plan…": C:\Users\pc\.codex/attachments/df9bbf36-945d-4bc1-9a38-9d7f1bce46fc/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن درخواست پیوست‌شده را می‌خوانم و Plan را بر اساس Specification تأییدشده و شواهد فنی موجود تنظیم می‌کنم. در این مرحله چیزی را پیاده‌سازی یا تغییر نمی‌دهم.

---

## assistant
برای بخش‌های Persistence و Concurrency، فقط فایل‌های Wallet، ثبت DI و نمونه‌های transaction مرتبط را بررسی می‌کنم. اگر Mapping یا قیدهای پایگاه‌داده در Repository قابل مشاهده نباشند، آن را به‌عنوان وابستگی صریح در Plan ثبت می‌کنم.

---

## assistant
دو محدودیت مهم در Plan اثر می‌گذارند: پروژه `Data/Yashil.Infrastructure` که در ارجاع پروژه آمده، در checkout حاضر نیست، و `WalletController` فعلاً خالی است. بنابراین قابلیت‌های واقعی Mapping/Transaction و راه ورود ورودی Reserve از Checkout را به‌صورت فرض قطعی بیان نمی‌کنم.

---

## assistant
## 1. Plan Summary

Reserve به‌صورت یک عملیات Business مستقل در لایه Service طراحی شود. یک API اختصاصی آن را برای Order ثبت‌شده فراخوانی کند؛ Service مبلغ قابل‌پرداخت را از `OrderService` بگیرد و ایجاد Reservation و افزایش دو `ReservedAmount` را در یک transaction انجام دهد. پیش از Implementation، چند وابستگی Persistence باید روشن شود.

## 2. Files to Add / Change

| مسیر | Symbol پیشنهادی | دلیل |
| --- | --- | --- |
| [YashilCommerce.Core/Services/IWalletReservationService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletReservationService.cs>) — جدید | `ReserveAsync` | قرارداد Business |
| [YashilCommerce.Core/DTO/WalletReserveRequestDto.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveRequestDto.cs>) و [WalletReserveResultDto.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveResultDto.cs>) — جدید | DTOها | ورودی و نتیجه صریح، بدون پذیرش User/Application از کاربر |
| [YashilCommerce.Core/Repositories/IWalletReservationRepository.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Repositories/IWalletReservationRepository.cs>) — جدید | Query و عملیات اتمیک موردنیاز | جداسازی دسترسی به داده |
| [YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs>) — جدید | `ReserveAsync` | اعتبارسنجی و هماهنگی transaction |
| [YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs>) — جدید | Lookup، قفل/به‌روزرسانی مشروط | اجرای Persistence متناسب با EF موجود |
| [WalletController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs>) — موجود و فعلاً خالی | `Reserve` | API اختصاصی Business؛ نیازی به تغییر `BaseController` نیست |
| [ModuleInitializer.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs:58>) — موجود | ثبت Interface/Implementation | پیروی از DI فعلی با lifetime `Scoped` |

فایل Schema/Mapping یا Migration را نمی‌توان اکنون با مسیر دقیق نام برد: پروژه ارجاع‌شده `Data/Yashil.Infrastructure` در checkout حاضر نیست.

## 3. Contract / API Design

امضای پیشنهادی: `Task<WalletReserveResultDto> ReserveAsync(WalletReserveRequestDto request)`؛ DTO درخواست فقط `OrderId`, `WalletAssignmentId`, `Amount`, `IdempotencyKey` داشته باشد. DTO نتیجه حداقل `ReservationId`, `OrderId`, `Amount`, `Status`, `ExpiresAt` را برگرداند.

`WalletController.Reserve` یک POST اختصاصی باشد و Context معتبر کاربر/Application را در اختیار Service بگذارد. Service خود نیز مالکیت Order را enforce کند. CRUD عمومی و `BaseController` برای این عملیات مالی مناسب نیستند.

## 4. Service / Business Logic

ترتیب پیشنهادی:

1. کاربر و Application معتبر، کلید غیرخالی و مبلغ مثبت را بررسی کند.
2. Order را در همان Application بخواند و `Order.UserId == CurrentUser.Id` را enforce کند.
3. وارد transaction شود و حفاظت هم‌زمانی مربوط به کلید و Order را برقرار کند.
4. `ApplicationId + IdempotencyKey` را جستجو کند: درخواست یکسان → replay؛ متفاوت → conflict.
5. Reservation فعال همان Order را جستجو کند: همان درخواست → replay؛ متفاوت → conflict.
6. Assignment، مالک/Application آن و ارتباطش با Program را بررسی کند.
7. مبلغ نهایی قابل‌پرداخت را با منطق موجود `CalcAmountForPayAsync` بسنجد؛ محاسبه Price/Discount/Subsidy تکرار نشود.
8. Available را بررسی و تغییرات مالی را اتمیک ثبت کند.

Replay باید پیش از اعتبارسنجی موجودی فعلی انجام شود؛ کاهش موجودی پس از Reserve موفق نباید retry همان نتیجه را خراب کند.

## 5. Repository / Persistence

Queryهای لازم: Order با `Id + ApplicationId`؛ Reservation با `(ApplicationId, IdempotencyKey)`؛ Reservation فعال با `(ApplicationId, OrderId)`؛ Assignment با `Id + UserId + ApplicationId` و Program متناظر. برای خواندن صرف، `Include` لازم نیست اگر فیلدهای لازم با projection یا Query جداگانه گرفته شوند. تغییرات Assignment/Program باید روی داده معتبر و در همان transaction انجام شوند.

Repository اختصاصی برای lookupهای مالی، قفل و به‌روزرسانی مشروط توجیه دارد؛ صرف Generic Repository برای تضمین race کافی نیست. قید یکتایی `(ApplicationId, IdempotencyKey)` در DB ضروری است. قید Order فعال نیز مطلوب است، اما نوع آن به تعریف دقیق «فعال» وابسته است؛ شرط مبتنی بر زمان جاری را نمی‌توان به‌سادگی در filtered unique index قرار داد.

`YashilAppDbContext` در کد مصرف می‌شود، اما پروژه و Mapping آن در این checkout موجود نیستند؛ وجود جدول‌ها، ستون‌ها و indexهای واقعی قابل تأیید نیست.

## 6. Idempotency Strategy

درون transaction و پس از حفاظت هم‌زمانی، lookup با `ApplicationId + IdempotencyKey` انجام شود. پنج مقدار `ApplicationId, UserId, OrderId, WalletAssignmentId, Amount` دقیقاً مقایسه شوند. در replay، همان Reservation و نتیجه قبلی، از جمله `ExpiresAt`، برگردد و update انجام نشود. اختلاف هر مقدار conflict است.

برای دو درخواست هم‌زمان با یک کلید، بررسی اولیه برنامه کافی نیست: قفل مشترک بین instanceها و قید یکتایی DB لازم است. در برخورد با قید یکتا، transaction بازگردد و رکورد برنده برای تشخیص replay یا conflict دوباره خوانده شود.

## 7. Concurrency Strategy

رویکرد قابل بررسی در Infrastructure موجود: در transaction SQL Server، قفل transaction-scoped برای `(ApplicationId, IdempotencyKey)` و `(ApplicationId, OrderId)`، سپس **به‌روزرسانی مشروط Assignment** با شرط `Assigned - Consumed - Reserved - Expired >= Amount` و کنترل تعداد ردیف‌های تغییرکرده. EF Core `ExecuteUpdateAsync` در همین Repository استفاده شده است؛ نمونه در [FactorPaymentInstallmentService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/FactorPaymentInstallmentService.cs:163>) دیده می‌شود. افزایش Program و درج Reservation فقط پس از موفقیت شرط Assignment و در همان transaction انجام شوند.

در مثال ۱٬۰۰۰٬۰۰۰ اعتبار و درخواست‌های ۷۰۰٬۰۰۰ و ۵۰۰٬۰۰۰، پس از update موفق اول، شرط update دوم نباید برقرار باشد. `RowVersion` در Entityهای Assignment و Program وجود دارد، اما Mapping آن به concurrency token و ستون واقعی قابل تأیید نیست؛ Plan نباید به آن تکیه کند. امکان استفاده از قفل transaction-scoped، دسترسی SQL Server و اشتراک transaction میان `IUnitOfWork` و DbContext نیز باید با Infrastructure واقعی تأیید شود.

## 8. Transaction Boundary

`IUnitOfWork.BeginTransactionAsync` پیش از lookupهای Idempotency و Order فعال، بررسی Balance و تمام writeها فراخوانی شود. قفل‌ها، بررسی‌ها، ایجاد Reservation و افزایش دو مقدار `ReservedAmount` داخل همان transaction باشند. پس از ذخیره موفق هر سه تغییر، commit؛ برای conflict، رقابت یا خطای Persistence، rollback. الگوی Begin/Commit/Rollback در [OrderService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:172>) موجود است.

## 9. Checkout / Payment Integration

نقطه فراخوانی Reserve، پس از ثبت موفق Order در `SaveShoppingCartToOrder` و پس از `CalcAmountForPayAsync`، اما پیش از `CreateNewPaymentAndReturnAsync` و redirect است. در Plan فعلی، `WalletController.Reserve` این گام مستقل را برای caller انجام می‌دهد؛ **اتصال خودکار به Checkout فعلی هنوز ممکن نیست**، چون ورودی Assignment/Amount/IdempotencyKey در `ShoppingCartParam` فعلی ارائه نمی‌شود و UI نیز خارج از Scope است.

شکست Reserve نباید Payment بسازد یا redirect پرداخت صادر کند؛ Order ثبت‌شده باقی می‌ماند. مسیر قدیمی `PaymentMethodTypesEnum.Wallet` و محاسبه قیمت Cart دوباره استفاده یا تغییر داده نمی‌شوند.

## 10. Expiration

`CreationDate` و `ExpiresAt` از یک زمان واحد محاسبه شوند: `ExpiresAt = CreationDate.AddMinutes(30)`. کد فعلی هم `Now` و هم `UtcNow` دارد؛ برای داده مالی جدید UTC پیشنهاد می‌شود، مشروط به تأیید قرارداد زمانی Mapping/DB. TODO دقیق Specification کنار محاسبه قرار گیرد. Jobِ Expire/Release در این Feature ساخته نمی‌شود.

## 11. DI / Module Registration

Interface و Implementationهای Service و Repository در `ModuleInitializer.cs` با `Scoped` ثبت شوند، مطابق ثبت `IOrderService` و `IOrderRepository`. `ModuleInitializer` تغییر می‌کند؛ `ModuleInitializerPartial` برای این Feature لازم نیست.

## 12. Tests

| سناریو | نوع و دلیل |
| --- | --- |
| مبلغ نامعتبر، مالکیت، User/Application/Assignment mismatch، درخواست متفاوت | Unit: تصمیم‌های Service |
| Reserve موفق و `ExpiresAt` صحیح | Unit برای Rule؛ Integration برای سه write واقعی |
| موجودی ناکافی | Unit برای پاسخ؛ Integration برای update مشروط |
| retry یکسان و کلید یکسان با درخواست متفاوت | Unit برای مقایسه؛ Integration برای یکتایی واقعی |
| Reservation فعال با درخواست متفاوت | Integration: قفل و constraint واقعی |
| دو Reserve هم‌زمان | Integration با SQL Server: اثبات عدم Double Spending |
| خطا بین سه write و rollback | Integration: اثبات transaction مشترک |
| عدم ایجاد Ledger/WalletTransaction | Unit برای عدم فراخوانی؛ Integration برای نبود رکورد |

در checkout حاضر پروژه Test مرتبط مشاهده نشد؛ محل و ابزار اجرای تست Integration باید هنگام دسترسی به Infrastructure کامل تعیین شود.

## 13. Risks / Assumptions / Blockers

**Assumptions:** SQL Server و EF Core مطابق مصرف فعلی Repository در دسترس‌اند؛ یک DbContext scoped مشترک با `IUnitOfWork` کار می‌کند. این فرض‌ها هنوز تأیید نشده‌اند.

**Risks:** `RowVersion` ممکن است فقط property باشد؛ وضعیت `Reserved` در Entity از نوع `int` است و مقدار معتبر آن در فایل‌های بررسی‌شده مشخص نشد. نبود تعریف روشن «Reservation فعال» بر قید Order و رفتار پس از گذشت ۳۰ دقیقه اثر دارد.

**Blockers:** پروژه ارجاع‌شده `Data/Yashil.Infrastructure`، Mapping و Schema واقعی در checkout حاضر نیست؛ قید یکتایی، توان transaction و مسیر اعمال تغییر ضروری Schema قابل اثبات نیستند. همچنین برای اتصال خودکار به Checkout باید قرارداد ورود داده‌های Reserve مشخص شود؛ API مستقل به‌تنهایی ترتیب اجرای آن توسط Checkout فعلی را تضمین نمی‌کند.

## 14. Final Readiness

`BLOCKED`

پیش از Implementation باید Mapping/Schema واقعی و روش اعمال قید `(ApplicationId, IdempotencyKey)` در دسترس باشد؛ اشتراک transaction و امکان قفل/به‌روزرسانی مشروط تأیید شود؛ مقدار وضعیت `Reserved` و تعریف «فعال» روشن شود؛ و قرارداد فراخوانی Reserve از Checkout مشخص گردد. هیچ فایل یا کدی تغییر نکرد.

---

## user

# Files pasted by the user:

## "Plan قبلی Feature `Wallet Reserve` را با توجه به تغییرات جدید Repository، Revis…": C:\Users\pc\.codex/attachments/4ba71f97-0f18-455d-b4c4-af8e907f8f9b/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
درخواست Revision را می‌خوانم و فقط وابستگی‌هایی را که طبق متن جدید تغییر کرده‌اند دوباره بررسی می‌کنم. سپس Plan را به‌روزرسانی می‌کنم؛ هیچ فایلی تغییر نمی‌دهم.

---

## assistant
زیرساخت EF اکنون در Repository دیده می‌شود. Mapping سه Entity، Indexهای Reservation و تنظیم `RowVersion` را مستقیم می‌خوانم تا مکانیزم نهایی Concurrency بر اساس همین کد انتخاب شود.

---

## assistant
یک تعارض مهم پیدا شد: در فایل Enum فعلی، `WalletReservationStatusType` شامل `Registered`، `Released` و `Expired` است و عضو `Reserved` در آن دیده نمی‌شود. همچنین Mapping فعلی برای `ExpiresAt` پیش‌فرض ۱۵ دقیقه‌ای دارد. این دو مورد را در Plan به‌عنوان اختلاف با Specification قطعی ثبت می‌کنم و از حدس‌زدن مقدار وضعیت خودداری می‌کنم.

---

## assistant
برای Concurrency، ترکیب «update مشروط Assignment + دو قید یکتایی در DB + transaction واحد» با EF و SQL Server موجود قابل طرح است؛ تکیه بر `RowVersion` به‌تنهایی کافی نیست. یک ابهام دیگر نیز باقی است: `CalcAmountForPayAsync` به روش پرداخت نیاز دارد، اما قرارداد Reserve چنین ورودی یا Context مشخصی ندارد.

---

## assistant
## 1. Final Plan Summary

Reserve یک Business Operation مستقل با Service و API اختصاصی باشد. برای تضمین مالی، افزایش `WalletAssignment.ReservedAmount` با **update مشروط و کنترل تعداد ردیف** انجام شود؛ دو قید یکتایی DB از تکرار کلید و Reservation فعالِ Order محافظت کنند؛ درج Reservation و افزایش مبالغ Assignment و Program در یک transaction باشند.

دو اختلاف واقعی با Specification در بخش ۱۶ مانع شروع امن Implementation هستند.

## 2. Files to Add / Change

| مسیر | Symbol / تغییر | دلیل |
| --- | --- | --- |
| [IWalletReservationService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletReservationService.cs>) — جدید | `ReserveAsync` | قرارداد عملیات |
| [WalletReserveRequestDto.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveRequestDto.cs>) و [WalletReserveResultDto.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveResultDto.cs>) — جدید | ورودی/نتیجه | Contract صریح |
| [IWalletReservationRepository.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Repositories/IWalletReservationRepository.cs>) — جدید | lookup و update مشروط | عملیات Persistence ویژه Reserve |
| [WalletReservationService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs>) — جدید | `ReserveAsync` | Ruleها و transaction |
| [WalletReservationRepository.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs>) — جدید | queryها و `ExecuteUpdateAsync` | دسترسی به `YashilAppDbContext` |
| [WalletController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs>) — موجود و خالی | `Reserve` | API اختصاصی، بدون CRUD عمومی |
| [ModuleInitializer.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs:58>) | ثبت Service/Repository | DI مطابق پروژه |
| [YashilAppDbContext.cs](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28085>) و Schema متناظر | Indexهای Reservation | enforce کردن یکتایی |
| [Enums.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:218>) | وضعیت `Reserved` پس از تعیین مقدار معتبر | رفع اختلاف Enum با Specification |

تغییر [OrderController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:142>) یا `ShoppingCartParam` برای Pilot لازم نیست.

## 3. Contract / DTO / API

`ReserveAsync(WalletReserveRequestDto request)` فقط `OrderId`، `WalletAssignmentId`، `Amount` و `IdempotencyKey` را می‌پذیرد. `UserId` از `IUserPrincipal` و `ApplicationId` از `ApplicationConfiguration` گرفته می‌شود. پاسخ شامل `ReservationId`, `OrderId`, `Amount`, `Status`, `ExpiresAt` است.

`WalletController` فعلاً فقط یک کلاس خالی است. با Area و route صریح، مسیر پیشنهادی مطابق route ناحیه در [Startup.cs](<P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:724>) چنین است: `POST /ECommerceAdminApi/Wallet/Reserve`. Controller باید احراز هویت را الزام کند؛ Service مستقل از آن، مالکیت Order را بررسی کند. `BaseController` تغییر نمی‌کند.

## 4. Service Logic

پس از اعتبارسنجی اولیه کلید و مبلغ:

1. کاربر جاری و Application معتبر را دریافت کند؛ Order را فقط در همان Application بخواند و مالکیت را بررسی کند.
2. transaction را شروع کند.
3. Reservation با `(ApplicationId, IdempotencyKey)` را جستجو کند؛ درخواست یکسان → replay، متفاوت → conflict.
4. Reservation با `OrderId` و **Status برابر Reserved** را جستجو کند؛ درخواست یکسان حتی با کلید دیگر → replay، متفاوت → conflict. `ExpiresAt` در تعریف فعال‌بودن دخالت ندارد.
5. Assignment با User/Application و Program مرتبط با همان Application را تأیید کند.
6. مبلغ قابل‌پرداخت Order را از `OrderService` بگیرد؛ Price/Discount/Subsidy دوباره محاسبه نشود.
7. update مشروط Assignment، افزایش Program و درج Reservation را انجام دهد؛ سپس commit.

برای replay، موجودی فعلی دوباره شرط موفقیت نیست و `ExpiresAt` قبلی حفظ می‌شود.

## 5. Repository / Persistence

[DbContext](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:299>) اکنون DbSet هر سه Entity را دارد. Mapping آن‌ها Fluent API و جدول‌ها `shp.WalletAssignment`، `shp.WalletProgram` و `shp.WalletReservation` هستند. `AvailableAmount` در Assignment یک computed column با همان فرمول Specification است؛ `ReservedAmount`ها `decimal(18,2)` هستند. رابطه Assignment→Program و رابطه Reservation→Order/Assignment در Mapping وجود دارد. [Mapping Assignment](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27391>)؛ [Mapping Reservation](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28085>)

Repository اختصاصی باید lookupهای بدون tracking برای replay/validation، update مشروط Assignment، update Program با کنترل affected rows و درج Reservation را فراهم کند. `OrderItemId` در رکورد جدید `null` بماند. Index فعلی `IdempotencyKey` به‌تنهایی **global unique** است؛ Index یکتای Order فعال وجود ندارد.

## 6. Idempotency

Lookup با `(ApplicationId, IdempotencyKey)` و مقایسه دقیق پنج مقدار مؤثر انجام شود. برای race، lookup صرف کافی نیست: unique index متناظر در DB محافظ نهایی است. در شکست درج به‌علت همان constraint، transaction rollback شود؛ رکورد برنده دوباره خوانده و بر اساس مقایسه، replay یا conflict برگردد. سایر خطاهای DB نباید به‌اشتباه replay تلقی شوند.

## 7. Active Reservation

تعریف قطعی: `WalletReservationStatusType == Reserved`، بدون شرط زمانی. پیشنهاد Schema: filtered unique index روی `(ApplicationId, OrderId)` با filter وضعیت `Reserved`. EF Infrastructure از `HasFilter` استفاده می‌کند؛ نمونه موجود در [Mapping Assignment](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27399>) است. مقدار عددی filter باید از وضعیت معتبر `Reserved` بیاید؛ در وضعیت فعلی قابل تعیین نیست.

## 8. Concurrency

[Mapping Assignment](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27457>) و [Mapping Program](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28008>)، `RowVersion` را با `IsRowVersion().IsConcurrencyToken()` تنظیم کرده‌اند. EF در update معمولِ tracked entity هنگام عدم تطابق نسخه، `DbUpdateConcurrencyException` می‌دهد. این Mapping وجود ستون متناظر را در مدل نشان می‌دهد؛ وضعیت DB در حال اجرا جداگانه باید تأیید شود.

برای Reserve، **conditional `ExecuteUpdateAsync` روی Assignment** انتخاب مناسب‌تری است: شرط SQL همان `AssignedAmount - ConsumedAmount - ReservedAmount - ExpiredAmount >= Amount` باشد و update دقیقاً یک ردیف را تغییر دهد. این عملیات در یک statement مبلغ Reserved را افزایش می‌دهد. `ExecuteUpdateAsync` کنترل خودکار `RowVersion` در `SaveChanges` را اجرا نمی‌کند؛ معیار موفقیت آن affected rows است. نمونه استفاده از این API در [FactorPaymentInstallmentService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/FactorPaymentInstallmentService.cs:163>) وجود دارد. سپس Program در همان transaction افزایش یابد. دو unique index، raceهای «همان Order» و «همان کلید» را پوشش می‌دهند؛ قفل جداگانه برنامه‌ای لازم نیست.

## 9. Transaction

[Startup.cs](<P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:465>)، `YashilAppDbContext` و `IUnitOfWork` مبتنی بر همان نوع Context را `Scoped` ثبت می‌کند؛ [OrderService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:172>) از `BeginTransactionAsync` استفاده می‌کند. Source داخلی `UnitOfWork` در پروژه بررسی‌شده نیست؛ اشتراک قطعی transaction باید با تست Integration اثبات شود. برای حذف این وابستگی در خود Reserve، Plan اجرایی می‌تواند تمام query، `ExecuteUpdateAsync` و insert را روی **یک `YashilAppDbContext` تزریق‌شده و transaction خود EF** انجام دهد. Transaction پیش از lookupها شروع، پس از هر سه write commit و در هر شکست rollback می‌شود.

## 10. Checkout Integration

API مستقل برای Pilot قابل استفاده است. محل اتصال آینده پس از ذخیره Order، سپس `CalcAmountForPayAsync`، Reserve، `CreateNewPaymentAndReturnAsync` و redirect است. کد فعلی ثبت Order و ساخت Payment را در دو گام جدا انجام می‌دهد، پس این ترتیب از نظر ساختار ممکن است. اتصال خودکار هنوز در Scope Pilot نیست؛ شکست Reserve باید پیش از ایجاد Payment متوقف شود و Order ثبت‌شده باقی بماند. مسیر قدیمی `PaymentMethodTypesEnum.Wallet` تغییر نمی‌کند.

## 11. Expiration

یک زمان UTC با دقت ثانیه برای `CreationDate` و `ExpiresAt = CreationDate + 30 minutes` استفاده شود تا اختلاف precision ستون‌ها نتیجه را جابه‌جا نکند. TODO سیاست موقت کنار همین محاسبه قرار گیرد. Mapping فعلی `ExpiresAt` پیش‌فرض **۱۵ دقیقه** دارد؛ Service باید مقدار ۳۰ دقیقه را صریحاً بنویسد و هم‌راستا کردن default DB در تغییر Schema بررسی شود. Replay مقدار قبلی را برمی‌گرداند. Expire/Release اجرا نمی‌شود.

## 12. DI

Service و Repository با lifetime `Scoped` در `ModuleInitializer.cs` ثبت شوند؛ همین lifetime برای DbContext در Host استفاده می‌شود.

## 13. Tests

Unit برای مبلغ نامعتبر، مالکیت، mismatchهای User/Application/Assignment، مقایسه Effective Request، replay/conflict و زمان انقضا. Integration با SQL Server برای Reserve موفق، insufficient balance، سه race مستقلِ Order/کلید/Assignment، دو unique index، rollback در هر یک از سه write، حفظ `ExpiresAt` در replay و نبود `WalletTransaction`. تست Concurrency باید درخواست‌های ۷۰۰٬۰۰۰ و ۵۰۰٬۰۰۰ را بر اعتبار ۱٬۰۰۰٬۰۰۰ اجرا کند. پروژه Test آماده‌ای در checkout مشاهده نشد؛ محل آن هنگام Implementation تعیین می‌شود.

## 14. Schema / Migration Requirements

دو تغییر Schema برای تضمین مستقل از process لازم است:

1. جایگزینی unique index فعلیِ `IdempotencyKey` با unique index `(ApplicationId, IdempotencyKey)`.
2. filtered unique index روی `(ApplicationId, OrderId)` برای `Status = Reserved`.

پیش از اعمال، داده‌های موجود از نظر duplicate بررسی شوند. Mapping Fluent نیز هم‌زمان اصلاح شود. Migration یا SQL deployment موجود در این بخش از Repository دیده نشد؛ روش انتشار Schema باید با روش واقعی پروژه مشخص شود. Default پانزده‌دقیقه‌ای `ExpiresAt` نیز با سیاست ۳۰ دقیقه‌ای ناسازگار است و باید در همین تغییر هم‌راستا شود یا حذف گردد.

## 15. Risks / Assumptions

- **تعارض وضعیت:** برخلاف شرح درخواست، Enum فعلی [WalletReservationStatusType](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:218>) فقط `Registered = 3184`، `Released = 3185` و `Expired = 3186` دارد؛ `Reserved` ندارد. نباید فرض کرد `Registered` همان `Reserved` است.
- **مبلغ Order:** `CalcAmountForPayAsync` به `paymentMethodType` نیاز دارد و نوع پرداخت بر محاسبه اثر می‌گذارد. چهار ورودی Reserve این Context را مشخص نمی‌کنند. انتخاب خودسرانه مسیر `Wallet` قدیمی مجاز نیست.
- Mapping و Indexها از کد Scaffold مشخص‌اند، اما Schema پایگاه‌داده در حال اجرا و داده‌های موجود با این بررسی تأیید نشده‌اند.

## 16. Final Readiness

`BLOCKED`

برای شروع امن Implementation، باید **مقدار معتبر و نام `Reserved`** و **روش پرداخت/Context معتبری که مبلغ نهایی Order برای Reserve با آن محاسبه می‌شود** مشخص شوند. سپس تغییرات Index و default در Schema واقعی قابل اجرا و آزمون‌اند. هیچ فایل یا کدی تغییر نکرد.

---

## user
دو اصلاح مهم نسبت به Plan قبلی انجام شده است. بر اساس این اصلاحات Plan را فقط نهایی کن و سپس آماده Implementation شو.

### اصلاح 1 — Reservation Status

Enum مربوط به `WalletReservationStatusType` اکنون مقدار زیر را دارد:
```text
Reserved

```

بنابراین دیگر `Reserved` را Unknown یا Blocker در نظر نگیر.

Active Reservation دقیقاً:
```text
Status == Reserved

```

است.

`ExpiresAt` در تعیین Active بودن دخالت ندارد، چون Expire/Release mechanism خارج Scope این Feature است.

---

### اصلاح 2 — CalcAmountForPayAsync

یک نکته مهم:

`CalcAmountForPayAsync` منطق محاسبه قیمت را خودش پیاده‌سازی نمی‌کند؛ این متد از **Stored Procedure موجود پروژه** برای محاسبه مبلغ استفاده می‌کند و نتیجه را برمی‌گرداند.

بنابراین:

**ممنوع:**

- تغییر Stored Procedure
- تغییر منطق داخلی `CalcAmountForPayAsync`
- بازنویسی Price/Discount/Subsidy
- اضافه کردن منطق Wallet به Stored Procedure
- تغییر رفتار موجود Payment Calculation صرفاً برای این Feature

در این Feature فقط در صورت نیاز، از **نتیجه موجود** `CalcAmountForPayAsync` استفاده شود.

ترکیب مبلغ محاسبه‌شده توسط سیستم و مبلغ Wallet/Reserve در Flow نهایی پرداخت هنوز توسط Developer بررسی می‌شود و فعلاً نباید توسط Codex حدس زده یا طراحی شود.

بنابراین:

- `WalletReserveRequestDto.Amount` همچنان وجود داشته باشد.
- Reserve Service مسئول محاسبه قیمت Order نباشد.
- Reserve Service فقط Amount دریافت‌شده را طبق قوانین Wallet اعتبارسنجی کند:
  - `Amount > 0`
  - `Amount <= AvailableAmount`
- در Integration آینده Checkout، محل استفاده از مقدار خروجی `CalcAmountForPayAsync` را فقط مشخص کن.
- هیچ تغییر در `CalcAmountForPayAsync` یا Stored Procedure پیشنهاد یا اجرا نکن.

---

## وضعیت فعلی که باید مبنای Plan باشد

### Reservation
```text
WalletReservation.Status = Reserved

```

### Active Reservation
```text
Status == Reserved

```

### Idempotency
```text
ApplicationId + IdempotencyKey

```

با Effective Request:
```text
ApplicationId
UserId
OrderId
WalletAssignmentId
Amount

```

### Active Order Reservation

در MVP:
```text
At most one Active Reservation per Order

```

### Balance
```text
Available =
AssignedAmount
- ConsumedAmount
- ReservedAmount
- ExpiredAmount

```

### Concurrency

برای Assignment از Conditional Update استفاده شود:
```text
AvailableAmount >= Amount

```

و affected rows کنترل شود.

### Atomicity

این سه عملیات در یک Transaction:
```text
WalletReservation INSERT
WalletAssignment.ReservedAmount += Amount
WalletProgram.ReservedAmount += Amount

```

### Schema

بررسی/ایجاد:
```text
UNIQUE(ApplicationId, IdempotencyKey)

```

و:
```text
UNIQUE(ApplicationId, OrderId)
WHERE Status = Reserved

```

### Expiration
```text
ExpiresAt = CreationTime + 30 minutes

```

و مکانیزم واقعی Expire/Release خارج Scope است.

---

## کار این مرحله

فقط Plan قبلی را با دو اصلاح بالا نهایی کن.

هیچ Code Change انجام نده.

در Final Readiness اگر تنها ابهام باقی‌مانده، نحوه ترکیب مبلغ Wallet با مبلغ نهایی Payment است، آن را **Blocker برای Wallet Reserve Service ندان**؛ چون Reserve Service Amount را به‌عنوان ورودی دریافت می‌کند و مسئول محاسبه مبلغ Order نیست.

در پایان:
```text
READY FOR IMPLEMENTATION

```

را اعلام کن، مگر اینکه یک Blocker واقعی و مستقل از این موارد پیدا کرده باشی.

اگر Blocker دیگری وجود دارد، دقیقاً File/Line/Symbol آن را گزارش کن و حدس نزن.


---

## assistant
Plan را با مرز جدید Amount نهایی می‌کنم: Service فقط Amount ورودی و اعتبار Wallet را می‌سنجد؛ محاسبه Order و Payment در Scope آن نیست. مقدار `Reserved` را یک‌بار در Enum فعلی تأیید می‌کنم و سپس نسخه نهایی Plan را می‌دهم.

---

## assistant
## 1. Final Plan Summary

Wallet Reserve به‌صورت Service و API مستقل پیاده‌سازی شود. Service فقط `Amount` ورودی، مالکیت Order، اعتبار Assignment/Program، Idempotency و موجودی Wallet را بررسی می‌کند. ایجاد Reservation و افزایش دو `ReservedAmount` در یک transaction انجام می‌شود. اتصال پرداخت به مرحله آینده تعلق دارد.

## 2. Files to Add / Change

| مسیر | Symbol / دلیل |
| --- | --- |
| `P:\Projects\walletProject\Wallet\backEnd\Modules\YashilCommerce\YashilCommerce.Core\Services\IWalletReservationService.cs` — جدید | قرارداد `ReserveAsync` |
| `P:\Projects\walletProject\Wallet\backEnd\Modules\YashilCommerce\YashilCommerce.Core\DTO\WalletReserveRequestDto.cs` و `WalletReserveResultDto.cs` — جدید | چهار ورودی مجاز و نتیجه قابل replay |
| `P:\Projects\walletProject\Wallet\backEnd\Modules\YashilCommerce\YashilCommerce.Core\Repositories\IWalletReservationRepository.cs` — جدید | lookupها و update مشروط |
| `P:\Projects\walletProject\Wallet\backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\ServiceImpl\WalletReservationService.cs` — جدید | Business Rules |
| `P:\Projects\walletProject\Wallet\backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\RepositoryImpl\WalletReservationRepository.cs` — جدید | EF query، update و transaction |
| [WalletController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs>) | افزودن action اختصاصی `Reserve` |
| [ModuleInitializer.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs:58>) | ثبت Service و Repository با `Scoped` |
| [YashilAppDbContext.cs](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28085>) و Schema متناظر | اصلاح indexها و default انقضا |

Enum فعلی [مقدار `Reserved = 3184`](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:218>) را دارد و نیاز به تغییر ندارد.

## 3. Contract / DTO / API

ورودی: `OrderId`, `WalletAssignmentId`, `Amount`, `IdempotencyKey`. `UserId` و `ApplicationId` فقط از Context معتبر گرفته شوند. خروجی حداقل `ReservationId`, `OrderId`, `Amount`, `Status`, `ExpiresAt` باشد.

در `WalletController` خالیِ فعلی، action اختصاصی با route صریح `POST /ECommerceAdminApi/Wallet/Reserve` قرار گیرد؛ `BaseController` CRUD استفاده نشود. Controller احراز هویت را الزام کند و Service مالکیت Order را مستقل بررسی کند.

## 4. Service Logic

ترتیب اجرا: اعتبارسنجی کلید و `Amount > 0`؛ دریافت User/Application؛ بررسی وجود Order در Application و `Order.UserId == CurrentUser.Id`؛ شروع transaction؛ lookup کلید Idempotency؛ lookup Reservation فعال Order؛ بررسی Assignment و Program با User/Application؛ بررسی موجودی؛ ثبت سه تغییر؛ commit.

Service قیمت Order، تخفیف، یارانه یا مبلغ Payment را محاسبه و مقایسه نمی‌کند. تنها شرط مبلغ Wallet در این Feature، مثبت‌بودن و `Amount <= AvailableAmount` است.

## 5. Repository / Persistence

`YashilAppDbContext` برای هر سه Entity، DbSet و Fluent Mapping دارد. `WalletAssignment.AvailableAmount` نیز با فرمول مصوب به‌صورت computed column تعریف شده است. Repository اختصاصی، Reservation را با `(ApplicationId, IdempotencyKey)` و `(ApplicationId, OrderId, Status=Reserved)` پیدا می‌کند و Assignment را با شرط موجودی در یک statement افزایش می‌دهد. `OrderItemId` در Reserve جدید `null` است. [Mapping Assignment](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27391>)؛ [Mapping Reservation](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28085>)

## 6. Idempotency

پنج مقدار `ApplicationId, UserId, OrderId, WalletAssignmentId, Amount` با Reservation پیدا‌شده مقایسه شوند. همان درخواست، همان Reservation و `ExpiresAt` را بدون write بازگرداند؛ درخواست متفاوت conflict است. در race مربوط به کلید، unique index محافظ نهایی است: پس از شکست constraint، transaction rollback و رکورد برنده برای replay یا conflict دوباره خوانده شود.

## 7. Active Reservation

Active دقیقاً `Status == Reserved` است؛ زمان انقضا در این query دخالت ندارد. Reservation فعال همان Order با همان Effective Request replay می‌شود، حتی اگر کلید جدید باشد؛ درخواست متفاوت conflict است. filtered unique index، این قاعده را در DB نیز enforce می‌کند.

## 8. Concurrency

`WalletAssignment.ReservedAmount` با `ExecuteUpdateAsync` و شرط اتمیک `AvailableAmount >= Amount` افزایش یابد؛ موفقیت فقط با **یک affected row** پذیرفته شود. افزایش Program و درج Reservation در همان transaction انجام شوند. دو unique index، رقابت روی Order و IdempotencyKey را پوشش می‌دهند.

`RowVersion` در Mapping Assignment و Program واقعاً concurrency token است؛ بااین‌حال `ExecuteUpdateAsync` از کنترل خودکار RowVersion در `SaveChanges` استفاده نمی‌کند. برای Reserve، شرط اتمیک و affected rows معیار اصلی است؛ RowVersion جایگزین آن نیست. [RowVersion Mapping](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27457>)

## 9. Transaction

برای اطمینان از اشتراک قطعی transaction، تمام lookupها و سه write مالی Reserve از یک `YashilAppDbContext` scoped و transaction همان Context استفاده کنند. Transaction پیش از lookupهای Idempotency/Active شروع و فقط پس از موفقیت درج Reservation و هر دو update commit شود. هر شکست یا conflict باعث rollback است. این انتخاب به internals پکیج Generic یا `IUnitOfWork` وابسته نیست.

## 10. Checkout Integration

Pilot، Service و API قابل استفاده فراهم می‌کند؛ Checkout فعلی تغییر نمی‌کند. محل اتصال آینده: ثبت Order → دریافت نتیجه `CalcAmountForPayAsync` → Reserve با Amount تعیین‌شده توسط Flow آینده → ایجاد Payment → redirect. نحوه ترکیب مبلغ Wallet و Payment هنوز در دست بررسی Developer است و بخشی از Reserve Service یا این Plan نیست. `CalcAmountForPayAsync`، Stored Procedure و `PaymentMethodTypesEnum.Wallet` قدیمی تغییر نمی‌کنند.

## 11. Expiration

از یک `CreationTime`، مقدار `ExpiresAt = CreationTime + 30 minutes` صریحاً ثبت شود؛ retry آن را تغییر ندهد. TODO انتقال سیاست ۳۰ دقیقه‌ای به Policy/Configuration آینده افزوده شود. Default فعلی Mapping برای `ExpiresAt` پانزده دقیقه است و در تغییر Schema هم‌راستا یا حذف شود. Expire/Release mechanism ساخته نمی‌شود.

## 12. DI

Interface و Implementationهای Service/Repository در `ModuleInitializer.cs` با lifetime `Scoped` ثبت شوند؛ این lifetime با DbContext ثبت‌شده در Host سازگار است.

## 13. Tests

Unit: مبلغ نامعتبر، مالکیت، mismatchهای User/Application/Assignment، مقایسه درخواست، replay/conflict و محاسبه انقضا.

Integration با SQL Server: Reserve موفق، موجودی ناکافی، replay با کلید یکسان، conflict کلید متفاوت، Reservation فعال Order، درخواست‌های هم‌زمان روی Order/کلید/Assignment، rollback هنگام شکست هر یک از سه write، حفظ `ExpiresAt` و ایجادنشدن `WalletTransaction`. سناریوی ۷۰۰٬۰۰۰ و ۵۰۰٬۰۰۰ روی اعتبار ۱٬۰۰۰٬۰۰۰ باید نشان دهد هر دو موفق نمی‌شوند.

## 14. Schema / Migration Requirements

Mapping فعلی فقط `IdempotencyKey` را به‌تنهایی unique می‌کند و قید Order فعال ندارد. تغییر لازم:

- `UNIQUE(ApplicationId, IdempotencyKey)`
- `UNIQUE(ApplicationId, OrderId) WHERE WalletReservationStatusType = 3184`

SQL Server و Fluent Mapping فعلی از filtered index پشتیبانی می‌کنند. پیش از اعمال constraintها، داده‌های موجود از نظر duplicate بررسی شوند. روش انتشار Schema مطابق فرایند واقعی پایگاه‌داده پروژه تعیین شود؛ در این مرحله Migration یا DDL ایجاد نمی‌شود.

## 15. Risks / Assumptions

Schema و داده‌های محیط اجرایی باید پیش از اعمال indexها بررسی شوند. `Amount` و ستون‌های مالی `decimal(18,2)` هستند؛ قرارداد API باید از گردشدن ناخواسته مبلغ جلوگیری کند. Route جدید، اتصال خودکار Checkout نیست و نباید به‌عنوان تکمیل جریان Payment معرفی شود.

## 16. Final Readiness

**READY FOR IMPLEMENTATION**

---

## user

# Files pasted by the user:

## "الان بر اساس Plan نهایی تأییدشده، Feature زیر را پیاده‌سازی کن: **Wallet Reserv…": C:\Users\pc\.codex/attachments/81b33dee-85be-4665-867e-ea2e45d01d56/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن دقیق درخواست پیوست‌شده را می‌خوانم و سپس پیاده‌سازی Wallet Reserve را بر اساس Plan تأییدشده پیش می‌برم. ابتدا محدودیت‌های مربوط به Schema، API و تست را از همان متن تثبیت می‌کنم.

---

## assistant
Conventionهای لازم مشخص شد. تغییرات را به Service، Repository، DTO، API، ثبت DI و Fluent Mapping محدود می‌کنم؛ فایل‌های تغییرکرده‌ی قبلی در workspace را دست‌نخورده نگه می‌دارم.

---

## user
<environment_context>
  <current_date>2026-10-03</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## assistant
کد اصلی نوشته شد. اکنون پروژه‌های Core، Infrastructure و Web را Build می‌کنم، خطاهای Compile را برطرف می‌کنم و سپس رفتار race، rollback و Scope تغییرات را بازبینی می‌کنم.

---

## assistant
Build به کد Feature نرسید: Restore به منبع محلی DevExpress و نسخه `9.8.45` پکیج‌های Yashil دسترسی ندارد. در همین حال بازبینی ایستا را ادامه می‌دهم؛ یک نکته در مسیر بازیابی race را اصلاح می‌کنم تا Query پس از rollback روی transaction پایان‌یافته اجرا نشود.

---

## assistant
بازبینی ایستا انجام شد: فایل‌های جدید از نظر Syntax سالم‌اند و خروجی‌های موقت Restore پاک شدند. تست رفتاری و Integration به‌دلیل نبود وابستگی‌های Build و پایگاه‌داده قابل اجرا نبوده‌اند؛ اکنون آخرین بررسی Scope و فایل‌های تغییرکرده را انجام می‌دهم.

---

## assistant
## Implemented

- [WalletReserveRequestDto.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveRequestDto.cs>) و [WalletReserveResultDto.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveResultDto.cs>): قرارداد چهار ورودی، نتیجه قابل replay و خطاهای مشخص.
- [IWalletReservationService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletReservationService.cs>)، [WalletReservationService.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs>): اعتبارسنجی، مالکیت Order، Idempotency و عملیات Reserve.
- [IWalletReservationRepository.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Repositories/IWalletReservationRepository.cs>)، [WalletReservationRepository.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs>): Queryها و update مشروط اتمیک.
- [WalletController.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs>): API احراز‌هویت‌شده `POST /ECommerceAdminApi/Wallet/Reserve`.
- [ModuleInitializer.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs>): ثبت Scoped سرویس و Repository.

## Business Rules

`Amount` باید مثبت، قابل ذخیره با دقت دو رقم اعشار و حداکثر برابر موجودی قابل استفاده باشد. همان Effective Request با کلید قبلی یا Reservation فعال همان Order، بدون تغییر موجودی replay می‌شود؛ درخواست متفاوت conflict است. افزایش Assignment با یک conditional `ExecuteUpdateAsync` و کنترل `affectedRows == 1` انجام می‌شود.

## Database

در [YashilAppDbContext.cs](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28085>)، Mapping به unique `(ApplicationId, IdempotencyKey)` و filtered unique `(ApplicationId, OrderId)` برای `Reserved = 3184` تغییر کرد. پیش‌فرض ۱۵ دقیقه‌ای `ExpiresAt` حذف و پیش‌فرض Status با `Reserved` هماهنگ شد. **Schema پایگاه‌داده اجراشده تغییر نکرده است.**

## Transaction

جستجوی Idempotency/Reservation فعال و هر سه write روی یک `YashilAppDbContext` و در یک transaction انجام می‌شوند. خطا rollback می‌کند؛ برخورد با unique constraint پس از rollback برای replay یا conflict دوباره خوانده می‌شود.

## Tests

| بررسی | نتیجه | دلیل |
| --- | --- | --- |
| Syntax هر ۹ فایل C# تحت تأثیر | **PASS** | Roslyn بدون خطای Syntax |
| مبلغ نامعتبر، موجودی ناکافی و کلید نامعتبر | **NOT RUN** | Build و محیط اجرای تست آماده نیست |
| مالکیت Order و mismatchهای User/Application/Assignment/Program | **NOT RUN** | Build و محیط اجرای تست آماده نیست |
| replay و conflict برای کلید و Order فعال؛ حفظ `ExpiresAt` | **NOT RUN** | نیازمند تست رفتاری و DB |
| race روی کلید، Order و Assignment؛ سناریوی ۷۰۰٬۰۰۰/۵۰۰٬۰۰۰ | **NOT RUN** | نیازمند SQL Server و constraintهای اعمال‌شده |
| rollback در شکست هر یک از سه write | **NOT RUN** | نیازمند تست Integration |
| نبود Payment و WalletTransaction | **NOT RUN** | بازرسی کد، فراخوانی ایجاد آن‌ها را نشان نمی‌دهد؛ تست اجرایی انجام نشد |

## Build

**FAIL — به Compile نرسید.** Restore به مسیر محلی DevExpress دسترسی ندارد و پکیج‌های `Yashil.Common.*` نسخه `9.8.45` در منابع موجود پیدا نشدند (`NU1301`/`NU1102`). اجرای مجدد خارج از sandbox نیز همین مانع را نشان داد.

## Out of Scope

`CalcAmountForPayAsync`، Stored Procedure، Checkout، Payment، `PaymentMethodTypesEnum.Wallet`، UI، Ledger و فرایند Expire/Release تغییر نکردند. Migration یا DDL ایجاد نشد.

## Remaining Risks

تا زمان انتشار Schema، قیدهای یکتایی جدید در DB اجراشده تضمین نشده‌اند؛ داده‌های موجود نیز باید پیش از اعمال آن‌ها از نظر duplicate بررسی شوند. Compile و تست Integration پس از فراهم‌شدن وابستگی‌های Build و SQL Server لازم‌اند.

**READY FOR CODE REVIEW**

---

## user
# AGENTS.md instructions for P:\Projects\walletProject\Wallet\backEnd

<INSTRUCTIONS>
These AGENTS.md instructions replace all previously provided AGENTS.md instructions.

# Codex Repository Guide

## Purpose
This repository contains the Wallet work built on an existing Yashil commerce application. Treat the existing code and repository documentation as the source of truth; do not invent missing architecture or database relationships.

## Start here
- `docs/codex/README.md` — Codex map and when to read each document.
- `docs/codex/architecture.md` — observed backend structure and boundaries.
- `docs/codex/frontend.md` — observed Angular structure and conventions.
- `docs/codex/wallet.md` — Wallet domain facts and invariants.
- `سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md` — detailed Wallet business reference.
- `docs/specifications/TEMPLATE.md` — template for new feature specifications.
- `.agents/skills/` — reusable task workflows; load only the relevant skill.

## Working rules
1. Inspect the existing implementation and a similar feature before changing code.
2. Prefer existing project patterns over introducing new abstractions or libraries.
3. Read only documentation relevant to the current task; do not load the whole knowledge base by default.
4. Keep changes scoped to the requested feature. Avoid unrelated refactoring.
5. Do not guess external/missing modules, schema relationships, configuration values, or business rules. State uncertainty and inspect references first.
6. For financial/wallet changes, preserve idempotency, concurrency protection, ledger integrity, and the documented Wallet invariants.
7. Never store or expose secrets, credentials, raw card PANs, or sensitive payment data in code or documentation.
8. Before finishing, build/test the affected area when the repository provides a valid command for it, then inspect the diff for unrelated changes.

## Planning
A feature request should state the desired behavior, acceptance criteria, and constraints/scope. Clarify only missing details needed to proceed.
Treat a feature as non-trivial if it changes multiple layers or a contract, changes or adds a business/financial rule, changes architecture or design, or requires choosing between implementation approaches. For non-trivial work, first produce a concise implementation plan identifying affected files, existing patterns, risks, and verification steps; obtain the user's approval before implementation. Avoid unnecessary plans for simple, local changes.

## Scope note
The supplied archive references additional Yashil modules from `Yashil.WebHost`, but those modules are not present in the supplied source archive. Do not assume their implementation details; treat them as external dependencies unless their source is later provided.

## Current technology signals
- Backend: .NET 8 / ASP.NET Core / EF Core / SQL Server / gRPC in the supplied projects.
- Backend Wallet code is currently integrated mainly through `Yashil.Core` entities and `YashilCommerce` Core/Infrastructure/Web layers.
- Frontend: Angular/TypeScript with existing shared base components and feature modules.

## What not to do by default
Do not introduce MCP, multi-agent orchestration, SDK automation, headless workflows, or other advanced Codex infrastructure unless explicitly requested.

</INSTRUCTIONS>

---

## user

# Files pasted by the user:

## "نقش تو در این Task: * Senior Software Architect * Senior Software Engineer * Pr…": C:\Users\pc\.codex/attachments/f4d1f7dd-ce70-416c-af47-d52a56a73477/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن درخواست پیوست‌شده را می‌خوانم و محدوده بررسی را بر اساس آن مشخص می‌کنم.

---

## assistant
نقشه راه را از وضعیت واقعی کد و مستندات Wallet استخراج می‌کنم. ابتدا فهرست محدود فایل‌های Wallet و بخش‌های MVP را پیدا می‌کنم، سپس وابستگی‌ها را با پیاده‌سازی فعلی Reserve تطبیق می‌دهم؛ هیچ عملیات Git یا تغییر فایل انجام نمی‌دهم.

---

## assistant
تا اینجا، Wallet در کد عمدتاً Entity و EF Mapping دارد و تنها عملیات Business جدیدِ قابل مشاهده Reserve است. در سند MVP چند قرارداد Ledger و Card با Entityهای Scaffold شده اختلاف دارند؛ این‌ها را به‌عنوان تصمیم لازم ثبت می‌کنم، نه اینکه مدل داده را از روی سند بازطراحی کنم.

---

## assistant
## Handoff Summary

### Active task

Produce a **read-only Wallet implementation roadmap** from the request in `C:\Users\pc\.codex\attachments\f4d1f7dd-ce70-416c-af47-d52a56a73477\Pasted text.txt`. The required output has **10 exact sections**, from `Wallet Current State` through `Confidence / Evidence`, followed by either `ROADMAP READY / NEXT FEATURE` or `ROADMAP BLOCKED / DECISION REQUIRED`.

**Do not edit files, implement features, or perform Git operations.** The user says Reserve has been implemented and initially reviewed; unavailable Build/Integration tests must **not** be treated as a blocker for the next feature.

### Findings established

- The detailed reference, [Wallet MVP reference](<P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md:1>), explicitly includes **organizational Program, Card Group/Match, and eight-digit Coupon** in MVP. Its core model is `Program → Scope → Assignment → Reservation → Transaction`; Card and Coupon are branches of Program. Financial flow: `Eligibility → Reserve → Payment → Capture on success / Release on failure`; Capture creates ledger debit and Refund creates ledger credit.
- [Wallet context](<P:/Projects/walletProject/Wallet/docs/codex/wallet.md:5>) lists invariants: no negative available balance, Program allocation within total, Assignment reserved+consumed+expired within assigned, idempotent Reserve/Capture/Release/Claim, concurrency protection, append-only ledger, and one active Assignment per user/program.
- Current code has Wallet Entity and EF Mapping for Program, Scope, Assignment, Reservation, Transaction, Card Group/Member, and Coupon Batch/Coupon. [DbContext](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:299>) defines all DbSets.
- Searching Wallet Entity references across `backEnd` found **only new Reserve service/repository/API/DI as Wallet business logic**; other Wallet types occur in entities, mapping, and navigation properties. No Program/Assignment/Card/Coupon/Ledger business service was found. Legacy [WalletPaymentController](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/WalletPaymentController.cs:20>) and `OrderController.PayWithWallet` are the **older CustomerClub/payment-method path**, not MVP Wallet.
- [Reserve service](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:30>) validates amount, Order owner, Assignment/Program; checks idempotency and one `Reserved` Reservation per Order; conditionally increments Assignment, increments Program, inserts Reservation in an EF transaction. It sets 30-minute `ExpiresAt` and creates no ledger/payment. [Repository](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs:37>) performs conditional `ExecuteUpdateAsync`. [Controller](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs:9>) exposes authenticated Reserve API. This is **implemented at source level but needs Checkout/Payment integration**. [Mapping](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28085>) declares application+key unique and filtered unique active Order; live DB deployment was not verified.
- [Assignment mapping](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27391>) has computed `AvailableAmount` and a filtered unique index for one active Assignment per Program/User; this is **infrastructure**, not allocation logic.
- Documentation/code conflicts relevant to future specifications: reference states `WalletTransaction` has **no Direction**, but [entity](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:61>) contains `WalletTransactionDirectionType`; mapping sets its default at [DbContext:28418](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28418>). Reference describes Assignment `SourceId`/`SourceRef`, but [entity](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletAssignment.cs:63>) has SourceType and no SourceId/Ref. Reference says CardMember `ValidUntil` and `LastMatchedAt` omitted in MVP unless needed, but [entity](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletCardMember.cs:61>) has both. **Do not suggest redesigning scaffolded entities solely for these differences.**
- Coupon mapping has `CodeHash` unique and Batch `CodeLength` default 8. Card mapping has fingerprint index. There is no implementation of claim/matching logic.
- `docs/specifications/` contains only `TEMPLATE.md`; no approved per-feature specification file was found.
- The detailed MVP reference does **not clearly designate a Post-MVP feature set**. State this ambiguity rather than invent a firm boundary. Reconciliation/reporting are not specified as end-to-end features; ledger balance fields and indexes provide only infrastructure.

### Likely roadmap decision

Although Reserve is already coded, its **upstream prerequisites are missing**. The strongest candidate for the **single next feature** is **WalletProgram lifecycle/management**, then Scope and Assignment/Grant, then Eligibility (organizational, Card, Coupon), followed by checkout/payment integration, Capture/Release, Refund, expiration, and audit/reconciliation. Explain why Program must precede Assignment and why Reserve’s existing code cannot alone provision usable credit. Consider whether the next feature should instead be Assignment/Grant if Program creation is assumed externally; there is no evidence of existing Program service/API.

### Remaining work

1. Verify any narrowly needed evidence about Program allocation, WalletTransaction ledger constraints, and absence of business APIs/background jobs. Avoid rescanning broadly.
2. Build the requested Feature Inventory table with statuses from the user’s allowed vocabulary. Use `Infrastructure Only` for scaffold-only areas; `Needs Integration` for Reserve; `Not Started` for business logic with no implementation.
3. Provide **feature definitions** with the user’s requested fields (purpose, status, scope, dependencies, blocks, entities, services, APIs, external/DB/concurrency/idempotency/jobs/integration/MVP/complexity/open questions), preferably compact grouped entries under the inventory or roadmap while preserving the exact 10 section headings.
4. Distinguish actual dependency edges from optional ordering. Report decisions requiring Product/Developer input in the requested `BLOCKER / DECISION REQUIRED` format, notably the ledger Direction conflict, Assignment source attribution, Wallet/payment amount composition, and uncertain MVP boundary for refund/operational reporting.
5. Decide final marker. Because the user says request decisions must be highlighted, `ROADMAP BLOCKED` may be appropriate **only if an unresolved domain decision prevents extracting the roadmap itself**. Otherwise use `ROADMAP READY` and list decisions that block individual future feature specifications, not the sequence.

No files have been changed during this roadmap task; no Git operation was performed.

---

## assistant
درخواست پیوست‌شده را می‌خوانم و گزارش را بر پایه شواهد مرحله قبل و بررسی‌های محدود تکمیل می‌کنم. هیچ فایل یا کدی را تغییر نمی‌دهم.

---

## assistant
تا اینجا Reserve در سطح سرویس و API پیاده‌سازی شده، اما اجزای بالادستی مانند مدیریت Program و تخصیص اعتبار هنوز در مسیر اجرایی Wallet دیده نمی‌شوند. اکنون مرز MVP و وابستگی‌ها را با مستندات و چند نقطه مشخص از کد تطبیق می‌دهم.

---

## assistant
مرز وابستگی‌ها روشن شد: مستند MVP مسیر `Program → Scope → Assignment → Reservation → Transaction` و شاخه‌های Card و Coupon را صریح می‌گوید. برای گزارش، وضعیت «فقط Entity/Mapping» را از سرویس عملیاتی جدا می‌کنم و ابهام‌های کسب‌وکار را به‌عنوان تصمیم لازم برای Specification هر Feature ثبت می‌کنم.

---

## assistant
برای کنترل قواعد مالی، دستورالعمل `wallet-feature` پروژه را هم به کار می‌گیرم. بررسی محدود نشان می‌دهد اختلاف Ledger در `Direction` و اختلاف مدل Assignment در منبع تخصیص، تصمیم‌های لازم برای Featureهای مربوط‌اند؛ فعلاً مانع استخراج ترتیب Roadmap نیستند.

---

## assistant
## 1. Wallet Current State

مدل داده و EF Mapping برای Program، Scope، Assignment، Reservation، Transaction، Card و Coupon وجود دارد. در منطق اجرایی Wallet MVP، **Reserve تنها Feature پیاده‌سازی‌شده در سطح سرویس و API** است: مالکیت Order و Assignment، مبلغ و موجودی را بررسی می‌کند؛ افزایش مبالغ رزروشده و ثبت Reservation را در یک Transaction انجام می‌دهد. اتصال آن به Checkout و نتیجه پرداخت هنوز وجود ندارد. Build و Integration Test آن نیز، طبق وضعیت اعلام‌شده، به‌دلیل وابستگی‌های محیط اصلی اجرا نشده‌اند؛ این موضوع مانع انتخاب Feature بعدی نیست.

مسیر قدیمی `PayWithWallet` به CustomerClub و روش پرداخت موجود تعلق دارد؛ شواهدی از اتصال آن به `WalletReservation` و Ledger جدید دیده نشد. وجود Entity یا Mapping در جدول زیر به معنی وجود منطق کسب‌وکار نیست.

## 2. Feature Inventory

| # | Feature | Status | MVP | Dependencies | Complexity |
| - | ------- | ------ | --- | ------------ | ---------- |
| 1 | Program lifecycle و مدیریت بودجه | Infrastructure Only | بله | — | Medium |
| 2 | Scope و محدودیت مصرف | Infrastructure Only | بله | 1 | Medium |
| 3 | Ledger و ثبت رخداد قطعی | Infrastructure Only | بله | 1 | High |
| 4 | Assignment، Allocation و Grant | Infrastructure Only | بله | 1، 3 | High |
| 5 | Eligibility سازمانی | Not Started | بله | 1، 2، 4 | Medium |
| 6 | Card Group و Card Match | Infrastructure Only | بله | 1، 4 | High |
| 7 | Coupon Batch و Claim | Infrastructure Only | بله | 1، 4 | High |
| 8 | نمایش Balance قابل استفاده | Infrastructure Only | بله | 4؛ برای مصرف، 5 تا 7 | Medium |
| 9 | Reserve | Needs Integration | بله | 4؛ در جریان کامل، Eligibility | Medium |
| 10 | Order و Payment Integration | Not Started | بله | 5 تا 9؛ قرارداد تسویه | High |
| 11 | Capture و Consumption | Not Started | بله | 3، 9، نتیجه موفق پرداخت | High |
| 12 | Release | Not Started | بله | 9، نتیجه ناموفق/لغو پرداخت | Medium |
| 13 | Refund | Not Started | در جریان مرجع آمده است | 3، 11 | High |
| 14 | Expiration و آزادسازی رزرو منقضی | Not Started | مرز اجرایی نامشخص | 12، سیاست انقضا | Medium |
| 15 | Audit و Reconciliation | Not Started | مرز اجرایی نامشخص | 3، 10 تا 14 | High |
| 16 | Reporting عملیاتی | Not Started | مرز اجرایی نامشخص | داده‌های 1 تا 15، بسته به گزارش | Medium |

**تعریف اجرایی Featureها:** نام سرویس‌ها و APIهای آینده در موارد زیر نقش پیشنهادی را نشان می‌دهد، نه پیاده‌سازی موجود. برای هیچ‌یک اثر Database قطعی بدون Specification و بررسی Schema استنتاج نمی‌شود.

| Feature | Purpose و Business Scope | وابستگی و خروجی برای دیگران | اجزای اصلی، اثر فنی و سؤال باز |
| - | - | - | - |
| **1. Program** | ایجاد و مدیریت Program، بودجه، بازه اعتبار و وضعیت. | مبنای Scope، Assignment و شاخه‌های Card/Coupon؛ مستقل از Reserve قابل توسعه است. | `WalletProgram`؛ سرویس/API مدیریتی هنوز دیده نشد. Mapping موجود است؛ اثر DB جدید نامشخص. هم‌زمانی تغییر بودجه مهم است؛ نیاز به idempotency و job مشخص نشده. سؤال: قواعد تغییر وضعیت و ویرایش بودجه چیست؟ |
| **2. Scope** | تعریف محدوده‌های مجاز Program. | به Program وابسته است و Eligibility را تغذیه می‌کند. | `WalletScope`؛ سرویس/API مدیریتی موجود دیده نشد. وابستگی به شناسه‌های بیرونی Scope باید در Feature بررسی شود. اثر DB، هم‌زمانی، idempotency و job فعلاً نامشخص‌اند. سؤال: ترکیب چند Scope چگونه تفسیر می‌شود؟ |
| **3. Ledger** | ثبت append-only رخدادهای قطعی؛ قرارداد مشترک Grant، Capture و Refund. | به مدل Assignment/Program متکی است و ثبت مالی Featureهای بعدی را ممکن می‌کند. | `WalletTransaction`؛ سرویس/API Wallet Ledger دیده نشد. اثر DB و کلیدهای تکرار باید مشخص شوند؛ هم‌زمانی و idempotency مالی الزامی‌اند؛ job مشخص نشده. سؤال: تعارض `Direction` و مبلغ علامت‌دار چگونه حل می‌شود؟ |
| **4. Assignment / Allocation / Grant** | تخصیص اعتبار به کاربر در سقف بودجه Program و ثبت Grant. | به Program و قرارداد Ledger وابسته است؛ Reserve و Eligibility به اعتبار تخصیص‌یافته نیاز دارند. | `WalletAssignment`، `WalletProgram`، `WalletTransaction`؛ سرویس/API تخصیص دیده نشد. به User متصل است؛ DB فعلاً فقط Mapping دارد. کنترل اتمیک بودجه، تکرار Grant و یک Assignment فعال مهم‌اند؛ job لازم مشخص نشده. سؤال: روش تخصیص و ثبت منبع آن چیست؟ |
| **5. Eligibility سازمانی** | تشخیص اعتبار قابل استفاده بر پایه Program، Scope، User و Order. | پس از Program/Scope/Assignment؛ ورودی جریان خرید و Reserve است. | سرویس Eligibility و API آن دیده نشد؛ Entity تازه‌ای از سند نتیجه نمی‌شود. اتصال به داده‌های Order/Company/Product لازم است؛ اثر DB و job نامشخص. هم‌زمانی هنگام Grant احتمالی و جلوگیری از تخصیص تکراری لازم است. سؤال: قواعد دقیق تطبیق Scope چیست؟ |
| **6. Card** | مدیریت گروه/عضو و Match امن کارت برای Eligibility. | به Program و مسیر Assignment وابسته؛ می‌تواند در کنار Coupon پیش برود. | `WalletCardGroup` و `WalletCardMember`؛ سرویس/API عملیاتی دیده نشد. وابستگی بیرونی به ورودی کارت و مدیریت امن کلید HMAC دارد؛ PAN خام نباید ذخیره شود. Mapping موجود، اثر DB دیگر نامشخص؛ تکرار Match/Grant و هم‌زمانی باید کنترل شوند؛ job مشخص نشده. سؤال: Match چه زمانی Grant ایجاد می‌کند؟ |
| **7. Coupon** | تولید Batch هشت‌رقمی و Claim اتمیک Coupon. | به Program و Assignment وابسته؛ مستقل از Card قابل توسعه است. | `WalletCouponBatch` و `WalletCoupon`؛ سرویس/API عملیاتی دیده نشد. تولید امن کد و یکپارچگی Claim/Grant لازم است؛ Mapping شامل یکتایی `CodeHash` است. هم‌زمانی و idempotency Claim الزامی‌اند؛ job انقضای Coupon مشخص نشده. سؤال: Claim چگونه به Assignment موجود متصل می‌شود؟ |
| **8. Balance** | ارائه مانده قابل استفاده برای تجربه خرید و کنترل مصرف. | به Assignment وابسته؛ Eligibility تعیین می‌کند کدام مانده قابل نمایش/مصرف است. | `WalletAssignment.AvailableAmount` در EF به ستون محاسباتی `Assigned−Consumed−Reserved−Expired` نگاشت شده؛ API Balance دیده نشد. اثر DB تازه و job مشخص نیست؛ خواندن هم‌زمان با رزرو و مرز دسترسی User مهم است. سؤال: تجمیع چند Assignment چگونه نمایش داده شود؟ |
| **9. Reserve** | نگه‌داشت موقت مبلغ برای Order. | به Assignment موجود متکی است؛ Capture، Release و Integration به Reservation آن وابسته‌اند. | `WalletReservationService`، Repository و `POST .../Wallet/Reserve` موجودند. تراکنش EF، افزایش شرطی Assignment، کلید `(ApplicationId, IdempotencyKey)` و یک رزرو `Reserved` برای Order در Mapping دیده می‌شود؛ استقرار واقعی قیود DB تأیید نشده. `ExpiresAt` سی دقیقه است؛ job آزادسازی ندارد. سؤال Integration: مبلغ ورودی چگونه با مبلغ نهایی پرداخت ترکیب می‌شود؟ |
| **10. Order / Payment Integration** | فراخوانی Eligibility/Reserve و اتصال نتیجه پرداخت به Capture یا Release. | به مسیرهای اعتبار و تسویه وابسته؛ Capture/Release عملیاتی را فعال می‌کند. | Order و Payment موجودند، ولی API/سرویس اتصال به Wallet MVP دیده نشد. اثر DB و نیاز به job/callback پس از تعیین قرارداد پرداخت روشن می‌شود؛ idempotency callback و رقابت نتایج پرداخت حیاتی است. سؤال: مبلغ Wallet و مبلغ قابل پرداخت چگونه ترکیب می‌شوند؟ |
| **11. Capture** | تبدیل Reservation موفق به مصرف قطعی و Ledger debit. | به Reserve، Ledger و موفقیت معتبر پرداخت وابسته؛ Refund را ممکن می‌کند. | `WalletReservation`، `WalletAssignment`، `WalletProgram`، `WalletTransaction`؛ سرویس/API Capture دیده نشد. تغییر وضعیت و مانده‌ها باید اتمیک و idempotent باشند؛ اثر DB و job نامشخص. سؤال: مرجع قطعی موفقیت پرداخت و کلید تکرار آن چیست؟ |
| **12. Release** | بازگرداندن مبلغ رزروشده پس از شکست یا لغو معتبر. | به Reserve وابسته؛ Expiration می‌تواند از همان منطق آزادسازی استفاده کند. | Reservation، Assignment و Program؛ سرویس/API Release دیده نشد. تغییر وضعیت و مانده‌ها باید اتمیک، هم‌زمانی‌پذیر و idempotent باشند؛ job مستقل فقط برای انقضا محتمل است. سؤال: کدام رخدادهای Order/Payment مجوز Release هستند؟ |
| **13. Refund** | ثبت اعتبار برگشتی به‌صورت Ledger credit تازه و اصلاح مصرف خالص. | پس از Capture و قرارداد Refund سفارش/پرداخت. | Transaction، Assignment، Program و مراجع Order/Payment؛ سرویس/API Wallet Refund دیده نشد. اتمی بودن، جلوگیری از Refund تکراری و حد مبلغ برگشتی لازم است؛ اثر DB و job نامشخص. سؤال: Refund جزئی و سطح آیتم چگونه اعمال می‌شود؟ |
| **14. Expiration** | تعیین تکلیف Reservationهایی که مهلتشان گذشته است. | به Release و سیاست انقضا وابسته است. | Reservation/Assignment/Program؛ `ExpiresAt` موجود است، اما job و مکانیزم Expire/Release دیده نشد. رقابت با Capture و اجرای تکراری job باید ایمن باشد؛ اثر DB نامشخص. سؤال: زمان و تقدم انقضا نسبت به callback پرداخت چیست؟ |
| **15. Audit / Reconciliation** | کشف اختلاف میان Ledger، Reservation و مانده‌های ذخیره‌شده. | پس از مشخص شدن قواعد ثبت مالی و رخدادهای پرداخت؛ بخش خواندنی آن مستقل از Reporting قابل توسعه است. | Transaction و مانده‌های Program/Assignment؛ سرویس/API/job مشخصی دیده نشد. ممکن است به داده پرداخت بیرونی نیاز داشته باشد؛ اثر DB و سیاست اصلاح اختلاف نامشخص. سؤال: مرجع تطبیق و مالک رسیدگی به اختلاف کیست؟ |
| **16. Reporting** | نمایش مصرف، تخصیص، مانده و وضعیت عملیات به مدیران. | بر داده Featureهای پیشین تکیه می‌کند و می‌تواند به‌صورت تدریجی ساخته شود. | Entityها و فیلدهای تجمیعی موجودند؛ API/گزارش Wallet MVP دیده نشد. اثر DB، job و نیاز به تجمیع دوره‌ای نامشخص؛ کنترل دسترسی و سازگاری ارقام مهم است. سؤال: گزارش‌های لازم و سطح دسترسی هرکدام چیست؟ |

Authorization و Configuration/Policy موضوعات مشترک هر API و جریان مالی‌اند. سند، Feature مستقل با قرارداد کامل برای آن‌ها تعیین نکرده است؛ در Specification هر Feature باید بررسی شوند.

## 3. Dependency Graph

```text
Program ──→ Scope ──→ Eligibility سازمانی ──┐
   │                         │                 │
   ├──→ Card Group / Match ───┼──→ Assignment / Grant ──→ Balance
   ├──→ Coupon Batch / Claim ─┘           │
   └──→ Ledger contract ──────────────────┤
                                          ↓
                                    Reserve موجود
                                          ↓
                               Order / Payment Integration
                                  ↙                 ↘
                          Capture + Ledger       Release
                                ↓                    ↓
                              Refund             Expiration
```

Card و Coupon نسبت به یکدیگر مستقل‌اند، اما هر دو به Program و مسیر تخصیص اعتبار نیاز دارند. Capture و Release دو نتیجه متفاوت پرداخت‌اند؛ Refund به Capture وابسته است. Audit/Reconciliation و Reporting مصرف‌کننده داده این جریان‌اند و پیش‌نیاز Reserve نیستند.

## 4. Recommended Roadmap

```text
Phase 1 — تأمین اعتبار قابل استفاده
  Program → Scope
  Ledger contract → Assignment / Allocation / Grant
  Eligibility سازمانی و Balance

Phase 2 — مسیرهای Eligibility دیگر MVP
  Card Group / Match
  Coupon Batch / Claim

Phase 3 — تکمیل جریان خرید
  اتصال Reserve موجود به Order / Payment
  Capture و Release

Phase 4 — چرخه پس از پرداخت
  Refund
  Expiration، پس از تعیین سیاست آن

Phase 5 — کنترل عملیاتی
  Audit / Reconciliation
  Reporting بر اساس نیازهای تأییدشده
```

Program ابتدا می‌آید چون Assignment، Scope و دو شاخه Card/Coupon بدون Program قابل راه‌اندازی نیستند. Ledger contract پیش از Grant و Capture، شکل ثبت قطعی مالی را یکسان می‌کند. Assignment اعتبار واقعی فراهم می‌کند؛ سپس Eligibility تعیین می‌کند چه اعتباری در خرید قابل مصرف است. Card و Coupon می‌توانند پس از این پایه به‌صورت مستقل توسعه یابند. Integration پس از روشن شدن قرارداد مبلغ پرداخت، Reserve موجود را به نتیجه پرداخت وصل می‌کند. Capture و Release مانده رزروشده را تعیین تکلیف می‌کنند؛ Refund به مصرف قطعی نیاز دارد. Expiration باید با همان گذارهای مالی و وضعیت پرداخت سازگار شود.

## 5. MVP Roadmap

سند مرجع صریحاً **Program سازمانی، Card Group/Match و Coupon هشت‌رقمی** را در MVP قرار داده و مدل اصلی تا Reservation/Transaction و جریان `Eligibility → Reserve → Payment → Capture/Release` را شرح می‌دهد. بنابراین Phaseهای 1 تا 3 هسته MVP هستند؛ Refund نیز در جریان مالی همان سند آمده است. Reserve موجود، بخشی از Phase 3 را در سطح سرویس/API پوشش می‌دهد.

سند برای **مکانیزم اجرایی Expiration، Audit/Reconciliation و Reporting** مرز دقیق MVP تعیین نمی‌کند. وضعیت این موارد را قطعیِ Post-MVP اعلام نمی‌کنم.

## 6. Post-MVP Roadmap

فهرست قطعی Post-MVP از مستندات فعلی قابل استخراج نیست. Audit/Reconciliation پیشرفته، Reporting گسترده و سیاست‌های اجرایی انقضا، نامزد برنامه‌ریزی پس از جریان اصلی‌اند؛ زمان‌بندی آن‌ها به تصمیم Product/Developer و نیاز عملیاتی بستگی دارد. این دسته‌بندی، تعهد دامنه‌ای جدید ایجاد نمی‌کند.

## 7. Risks / Blockers

- **شکاف اجرایی تأییدشده:** Program، Allocation/Grant و Eligibility سرویس عملیاتی دیده‌شده ندارند؛ در نتیجه Reserve موجود به‌تنهایی اعتباری برای مصرف ایجاد نمی‌کند.
- **شکاف Integration تأییدشده:** Reserve به نتیجه پرداخت متصل نیست و Capture/Release برای Wallet MVP دیده نشد. مسیر قدیمی CustomerClub جایگزین این اتصال نیست.
- **تعارض مستند و کد:** سند Ledger را با مبلغ علامت‌دار و بدون `Direction` توصیف می‌کند، اما Entity/Mapping فعلی `Direction` دارد. این موضوع پیش از Specification Ledger/Capture باید حل شود.
- **تعارض منبع Assignment:** سند `SourceType + SourceId` و `SourceRef` را توضیح می‌دهد، اما Entity فعلی `SourceId/SourceRef` ندارد. این موضوع برای Grant مبتنی بر Card/Coupon تصمیم می‌خواهد.
- **ریسک تأیید محیط:** یکتایی‌های Reserve و ستون محاسباتی Balance در EF Mapping دیده می‌شوند؛ وضعیت اعمال‌شدن آن‌ها در Database عملیاتی از این بررسی تأیید نشده است. این ریسک، مانع انتخاب Feature بعدی نیست.

## 8. Open Business Decisions

**BLOCKER / DECISION REQUIRED — قرارداد Ledger**

- **Question:** در Featureهای Ledger/Capture/Refund، `Direction` موجود چگونه با قاعده مبلغ علامت‌دار سند هماهنگ شود؟
- **Why it matters:** معنای debit/credit و ثبت قطعی مالی را تعیین می‌کند.
- **Affected Features:** Ledger، Grant، Capture، Refund، Reconciliation.
- **Possible interpretations:** قرارداد سند مبنا باشد و تفاوت Entity در Specification حل شود؛ یا قرارداد دامنه‌ای تازه‌ای برای `Direction` تأیید شود. این گزارش هیچ‌کدام را انتخاب نمی‌کند.

**BLOCKER / DECISION REQUIRED — منبع Assignment**

- **Question:** انتساب Grant به Card/Coupon/Organization با فیلدهای موجود چگونه ثبت و پیگیری شود؟
- **Why it matters:** جلوگیری از Grant تکراری و ردیابی منشأ اعتبار به آن وابسته است.
- **Affected Features:** Assignment، Card، Coupon، Eligibility.
- **Possible interpretations:** استفاده از قرارداد موجود با قواعد جدیدِ تأییدشده؛ یا تغییر قرارداد داده پس از بررسی Schema. هنوز انتخابی انجام نشده است.

**BLOCKER / DECISION REQUIRED — ترکیب مبلغ Wallet و پرداخت**

- **Question:** مبلغ ورودی Reserve چه نسبتی با مبلغ خروجی محاسبه موجود Order و مبلغ قابل پرداخت از درگاه دارد؟
- **Why it matters:** حدود Reserve، تسویه و Refund را تعیین می‌کند.
- **Affected Features:** Order/Payment Integration، Capture، Release، Refund.
- **Possible interpretations:** مصرف کامل یا جزئی Wallet با قواعد مشخص برای باقی‌مانده؛ تعیین این قواعد با Product/Developer است. این تصمیم، مانع سرویس Reserve که Amount را ورودی می‌گیرد نیست.

**BLOCKER / DECISION REQUIRED — سیاست انقضا و مرز عملیاتی MVP**

- **Question:** پس از `ExpiresAt`، در رقابت با نتیجه پرداخت چه زمانی و با چه رخدادی Reservation آزاد/منقضی شود، و کدام کنترل‌ها برای MVP لازم‌اند؟
- **Why it matters:** مانده‌های رزروشده نباید بی‌تکلیف بمانند و Capture/Expiration نباید هم‌زمان نتیجه ناسازگار بسازند.
- **Affected Features:** Release، Expiration، Payment Integration، Audit.
- **Possible interpretations:** مکانیزم زمان‌بندی‌شده یا فعال‌شونده با رخداد، طبق سیاست تأییدشده. سند فعلی انتخاب قطعی نمی‌دهد.

این تصمیم‌ها **Specification Featureهای مربوط** را مسدود می‌کنند؛ ترتیب کلی Roadmap را مسدود نمی‌کنند.

## 9. Recommended Next Feature

**WalletProgram lifecycle و مدیریت بودجه**

این Feature اولین گام بعد از Reserve پیاده‌سازی‌شده است، چون Program پیش‌نیاز Scope، Assignment و شاخه‌های Card/Coupon است و در Repository فعلی فقط Entity/Mapping آن دیده شد. توسعه آن باید با Specification کوتاه برای عملیات مدیریتی، گذار وضعیت، قواعد بودجه و سطح دسترسی شروع شود. سپس می‌توان Assignment/Grant را بر پایه Program واقعی ساخت.

## 10. Confidence / Evidence

- **مرز تصریح‌شده MVP، مدل، جریان مالی و تصمیم‌های داده:** [سند مرجع Wallet MVP](<P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md:1>)، به‌ویژه سطرهای 3 تا 6 و 134 تا 166.
- **قواعد مانده، idempotency، concurrency و Ledger:** [Wallet domain context](<P:/Projects/walletProject/Wallet/docs/codex/wallet.md:5>).
- **وجود Entityها و Mapping:** [YashilAppDbContext](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:299>)؛ محاسبه `AvailableAmount` و یکتایی Assignment در [همان فایل](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27391>)؛ یکتایی‌های Reserve در [همان فایل](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28085>).
- **رفتار Reserve و TODO سیاست انقضا:** [WalletReservationService](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:30>) و [WalletReservationRepository](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs:19>)؛ API در [WalletController](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs:9>).
- **تعارض‌های مدل:** [WalletTransaction](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:61>)، [WalletAssignment](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletAssignment.cs:63>) و سند مرجع بالا. تفاوت Scaffold به‌تنهایی پیشنهاد بازطراحی Entity نیست.
- **حد شواهد:** در Services/Repositories/DI مرتبط، سرویس Wallet عملیاتی دیگری جز Reserve دیده نشد؛ [ModuleInitializer](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs:60>). در `docs/specifications/` فقط Template دیده شد. Database عملیاتی و اجرای Build/Integration Test در این Task بررسی نشدند.

**ROADMAP READY**  
**NEXT FEATURE: WalletProgram lifecycle و مدیریت بودجه**

---

## user

# Files pasted by the user:

## "# Master Feature Implementation Prompt ## Feature **Feature Name:** `<FEATURE_N…": C:\Users\pc\.codex/attachments/aef0da74-2ad3-4fb7-b072-18303a2398ae/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
درخواست Feature را از پیوست می‌خوانم، سپس فقط Context و کد مرتبط با همان Feature را بررسی می‌کنم و طبق معیار Planning پروژه پیش می‌روم.

---

## assistant
پیوست یک قالب عمومی است و نام و Scope آن هنوز با placeholder نوشته شده‌اند. برای اینکه درخواست قابل اجرا شود، درباره Feature هدف سؤال فرستادم؛ هم‌زمان اطلاعات محدود WalletProgram و الگوی CRUD مشابه را آماده می‌کنم تا در صورت تأیید، از همین‌جا ادامه دهیم.

---

## assistant
**Self-Gate: Feature هنوز مشخص نشده است.** در پیوست، `<FEATURE_NAME>` و `<FEATURE_SCOPE>` مقدار ندارند. بنابراین بدون انتخاب Feature نمی‌توان Implementation را شروع کرد.

آیا منظورتان **WalletProgram lifecycle و مدیریت بودجه**، یعنی Feature بعدی Roadmap، است؟ اگر بله، محدوده مطلوب آن را مشخص کنید یا بگویید از سند Wallet MVP استخراج کنم. اگر Feature دیگری مدنظر است، نام و Scope آن را بفرستید.

هیچ فایلی تغییر نکرده است.

---

## user

# Files pasted by the user:

## "# Feature: WalletProgram Lifecycle و مدیریت بودجه Feature فعلی که باید پیاده‌سا…": C:\Users\pc\.codex/attachments/d35f6f40-45a5-4661-ac2c-c20d46c3ee0f/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
Feature مشخص شد. متن پیوست را می‌خوانم و سپس Specification و نقاط مرتبط Repository را برای Self-Gate بررسی می‌کنم.

---

## assistant
Self-Gate به یک ابهام دامنه‌ای واقعی رسیده است: سند وضعیت‌های `Draft/Active/Suspended/Closed` و فیلدهای بودجه و اعتبار را نام می‌برد، اما قواعد تغییر وضعیت، ویرایش بودجه پس از تخصیص و تغییر بازه اعتبار را تعیین نمی‌کند. الگوهای CRUD پروژه هم نمی‌توانند این قواعد مالی را مشخص کنند. پیش از کدنویسی، تصمیم‌های لازم را یک‌جا می‌پرسم.

---

## assistant
## Self-Gate — تصمیم دامنه‌ای لازم است

بررسی مستند Wallet و کد مرتبط نشان داد `WalletProgram` فیلدهای بودجه و بازه اعتبار و وضعیت‌های `Draft`، `Active`، `Suspended` و `Closed` را دارد، اما **قواعد چرخه حیات آن مشخص نشده است**. الگوی CRUD موجود، این تصمیم‌های مالی را تعیین نمی‌کند؛ بنابراین طبق دستور شما Implementation را شروع نکردم.

برای ادامه، لطفاً این سه مورد را مشخص کنید:

1. وضعیت اولیه و گذارهای مجاز چیست؟ هر وضعیت چه اثری بر Grant و Reserve دارد؟
2. تغییر `TotalAmount` پس از ایجاد، به‌ویژه کاهش آن پس از تخصیص یا رزرو، تحت چه شرایطی مجاز است؟
3. پس از فعال‌سازی کدام فیلدها و تاریخ‌ها قابل ویرایش‌اند، و چه نقش/مجوزی می‌تواند Program ایجاد کند، بودجه را تغییر دهد یا وضعیت را عوض کند؟

پاسخ‌ها رفتار Service، Validation، API و فرم مدیریتی را تعیین می‌کنند. **هیچ فایل، Entity، DbContext یا Database تغییر نکرده است.**

---

## user
**1. وضعیت اولیه و Lifecycle**

- `Draft` وضعیت اولیه `WalletProgram` است.&#x20;
- &#x20;در `Draft`، Program هنوز عملیاتی نیست و **Grant و Reserve مجاز نیستند**.&#x20;
- &#x20;از `Draft` می‌توان به `Active` رفت، مشروط به اینکه اطلاعات ضروری Program تکمیل و معتبر باشند و `TotalAmount > 0` باشد.&#x20;
- `Active` وضعیت عملیاتی Program است و ایجاد Grant و Reserve در آن مجاز است.&#x20;
- &#x20;از `Active` می‌توان به `Suspended` رفت.&#x20;
- &#x20;در `Suspended`، ایجاد Grant و Reserve جدید مجاز نیست؛ اما داده‌ها و تراکنش‌های قبلی حفظ می‌شوند و عملیات لازم برای مدیریت/تسویه آن‌ها باید طبق Ruleهای مربوط به خودشان ادامه پیدا کند.&#x20;
- &#x20;از `Suspended` می‌توان مجدداً به `Active` برگشت.&#x20;
- `Closed` وضعیت نهایی است و برگشت از آن به وضعیت دیگری مجاز نیست.&#x20;
- &#x20;در `Closed` هیچ Grant یا Reserve جدیدی مجاز نیست.&#x20;
- &#x20;انتقال مستقیم `Draft -> Suspended` یا `Draft -> Closed` مجاز نیست، مگر اینکه در کد فعلی پروژه Business Rule مستندی برای آن وجود داشته باشد.&#x20;
- &#x20;وضعیت `Closed` فقط زمانی مجاز است که تعهد مالی باز یا Reserve قابل‌استفاده‌ای باقی نمانده باشد؛ در صورت وجود چنین محدودیتی در مدل فعلی، آن را در Service enforce کن.&#x20;

**2. تغییر `TotalAmount`**

`TotalAmount` در `Draft` قابل ویرایش است.

پس از `Active` شدن:

- &#x20;افزایش `TotalAmount` مجاز است، مشروط به اینکه با مدل مالی فعلی سیستم سازگار باشد.&#x20;
- &#x20;کاهش `TotalAmount` فقط در صورتی مجاز است که مقدار جدید از مجموع مبالغ متعهدشده/رزروشده/مصرف‌شده کمتر نباشد.&#x20;
- &#x20;به عبارت دیگر، کاهش بودجه نباید باعث شود Program از نظر مالی در وضعیت نامعتبر قرار بگیرد.&#x20;
- &#x20;اگر بخشی از بودجه قبلاً Grant شده یا Reserve شده، آن بخش نباید با کاهش `TotalAmount` نقض شود.&#x20;
- &#x20;اگر `TotalAmount` قابل کاهش نیست، Service باید Validation مناسب برگرداند و عملیات را رد کند.&#x20;
- **موجودیت‌های Grant/Reserve نباید برای حل این مسئله تغییر کنند.** Rule باید در لایه Service/Domain موجود پیاده‌سازی شود.&#x20;

قاعده اصلی:

`NewTotalAmount >= ExistingFinancialCommitment`

که منظور از `ExistingFinancialCommitment` باید بر اساس مدل فعلی پروژه و معنای واقعی Grant/Reserve استخراج شود؛ Codex نباید این مفهوم را حدس بزند و اگر ambiguity وجود دارد، قبل از Implementation سؤال کند.

**3. ویرایش بعد از Activation و دسترسی‌ها**

بعد از `Active` شدن:

- &#x20;اطلاعات هویتی/توضیحی Program در صورت پشتیبانی مدل فعلی قابل ویرایش هستند.&#x20;
- &#x20;فیلدهای مؤثر بر ساختار مالی و Lifecycle باید محدود شوند.&#x20;
- `TotalAmount` فقط طبق Rule بند 2 قابل تغییر است.&#x20;
- &#x20;تاریخ شروع/پایان فقط در صورتی قابل تغییر است که تغییر آن باعث نقض Grant/Reserve موجود نشود.&#x20;
- &#x20;تغییر تاریخ به گذشته یا ایجاد بازه نامعتبر باید Validation شود.&#x20;
- &#x20;تغییر وضعیت فقط از طریق Service و Business Rule مجاز است و نباید صرفاً با CRUD Update انجام شود.&#x20;

از نظر Permission:

- &#x20;ایجاد `WalletProgram` فقط برای **نقش مدیریتی/مالی دارای Permission مربوط به مدیریت Wallet Program** مجاز است.&#x20;
- &#x20;تغییر بودجه نیز نیازمند همان Permission مدیریتی مالی است.&#x20;
- &#x20;تغییر Lifecycle (`Activate`, `Suspend`, `Resume`, `Close`) نیز باید Permission مستقل یا Permission مدیریتی مناسب موجود در پروژه را داشته باشد.&#x20;
- **نام Role یا Permission جدید ایجاد نکن.** ابتدا Permission/Authorization موجود در پروژه را پیدا و از همان Convention استفاده کن.&#x20;
- &#x20;اگر Permission مناسب در پروژه وجود ندارد، قبل از ایجاد Permission جدید متوقف شو و سؤال کن.
- قبل از Implementation، ابتدا Ruleهای فوق را با Entity، Service، Authorization، Grant و Reserve موجود تطبیق بده. اگر در تعریف دقیق `ExistingFinancialCommitment`، مجوزها، یا رفتار Grant/Reserve ابهامی وجود دارد، آن مورد را به‌عنوان Business/Architecture blocker گزارش کن و حدس نزن. در غیر این صورت، Feature را با معماری و Convention فعلی پروژه پیاده‌سازی کن و هیچ Entity/DbContext/EF Mapping/Migration/DDL/Index/Constraint را تغییر نده.  


---

## assistant
قواعد Lifecycle را با مدل مالی، Reserve موجود و مجوزهای پروژه تطبیق می‌دهم. اگر `ExistingFinancialCommitment` یا Permission مناسب از منابع فعلی قابل تعیین نباشد، مطابق Self-Gate پیش از تغییر کد متوقف می‌شوم.

---

## assistant
از مدل مالی می‌توان نتیجه گرفت `ReservedAmount` و `ConsumedAmount` زیرمجموعه اعتبار Grant شده‌اند، نه بودجه‌ای افزوده بر آن؛ بنابراین کف کاهش `TotalAmount` به `AllocatedAmount` وابسته است. دو مانع مستقل باقی مانده: شناسه‌های وضعیت Program در کد مشخص نیستند و مجوز مدیریت WalletProgram در منابع Repository پیدا نشده است. یک بررسی محدودِ فایل‌های داده/تنظیمات انجام می‌دهم تا نبود آن‌ها را قطعی‌تر گزارش کنم.

---

## assistant
## Self-Gate — Implementation متوقف است

قواعد ارائه‌شده با مدل فعلی تطبیق داده شد. طبق سند، `AllocatedAmount` جمع Grantهاست و مبالغ رزرو و مصرف‌شده از همان اعتبار تخصیص‌یافته‌اند؛ بنابراین کف کاهش بودجه **`NewTotalAmount >= AllocatedAmount`** است. همچنین [Reserve موجود](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:72>) هنوز وضعیت Program را کنترل نمی‌کند؛ اجرای قاعده «Reserve فقط در Active» به تغییر مرتبط در همان مسیر نیاز دارد.

سه مورد برای پیاده‌سازی صحیح هنوز پاسخ مشخصی در Repository ندارند:

1. شناسه‌های معتبر `Draft`، `Active`، `Suspended` و `Closed` در `CommonBaseData` چیست یا چگونه باید از داده موجود خوانده شوند؟ [Mapping فعلی](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28026>) فقط مقدار پیش‌فرض عددی دارد.
2. کدام **Permission/Action موجود** برای ایجاد Program، تغییر بودجه و تغییر Lifecycle استفاده شود؟ الگوی `YashilAuthorization` وجود دارد، ولی Permission مخصوص WalletProgram در منابع بررسی‌شده پیدا نشد. طبق دستور شما Permission جدید ایجاد نمی‌کنم.
3. شرط دقیق **Close و کوتاه‌کردن بازه اعتبار** چیست؟ به‌ویژه آیا مانده تخصیص‌یافته مصرف‌نشده، رزرو `Reserved`، تاریخ پایان Assignmentها و `ExpiresAt` رزروها مانع این تغییرها هستند؟ آیا تغییر بودجه و تاریخ در `Suspended` مجاز است؟

این تصمیم‌ها بر Validation، Service، API و فرم مدیریتی اثر مستقیم دارند. سؤال‌ها برای پاسخ شما ارسال شده‌اند. تا روشن‌شدن آن‌ها **هیچ فایل، Entity، DbContext یا Database تغییر نکرده است**.

---

## user
Self-Gate را ادامه بده. Enum مربوط به WalletProgramStatus در پروژه اضافه شده و می‌توانی از آن استفاده کنی.

Statusهای معتبر و IDهای موجود در CommonBaseData:

- `WalletProgramStatusEnum.Draft = 3168` — هنوز منتشر نشده
- `WalletProgramStatusEnum.Active = 3169` — فعال
- `WalletProgramStatusEnum.Suspended = 3170` — موقتاً متوقف شده
- `WalletProgramStatusEnum.Expired = 3171` — تاریخ اعتبار تمام شده
- `WalletProgramStatusEnum.Closed = 3172` — به‌صورت مدیریتی بسته شده

این IDها مربوط به رکوردهای موجود `CommonBaseData` هستند. رکورد جدیدی برای Status ایجاد نکن و IDها را تغییر نده.

`Expired` و `Closed` دو مفهوم متفاوت هستند:

- `Expired`: Program به دلیل پایان بازه اعتبار به وضعیت منقضی‌شده رسیده است.
- `Closed`: Program به‌صورت صریح و مدیریتی بسته شده است.

Lifecycle را با این Statusها تطبیق بده:

- `Draft -> Active`
- `Active -> Suspended`
- `Suspended -> Active`
- `Active -> Closed`
- `Suspended -> Closed`
- `Active -> Expired`
- `Suspended -> Expired`

`Expired` و `Closed` وضعیت نهایی هستند و از آن‌ها Transition دیگری مجاز نیست.

Rules قبلی همچنان برقرار هستند:

1. `Draft`
   - Grant جدید مجاز نیست.
   - Reserve جدید مجاز نیست.
   - اطلاعات Program قابل ویرایش است.
2. `Active`
   - Grant جدید مجاز است.
   - Reserve جدید مجاز است.
3. `Suspended`
   - Grant جدید مجاز نیست.
   - Reserve جدید مجاز نیست.
   - امکان Resume به `Active` وجود دارد.
   - تغییر `TotalAmount` طبق Rule قبلی مجاز است.
   - تغییر تاریخ طبق Validationهای تعیین‌شده مجاز است.
4. `Expired`
   - Grant جدید مجاز نیست.
   - Reserve جدید مجاز نیست.
   - Transition دیگری مجاز نیست.
5. `Closed`
   - Grant جدید مجاز نیست.
   - Reserve جدید مجاز نیست.
   - تغییر `TotalAmount` مجاز نیست.
   - تغییر بازه اعتبار مجاز نیست.
   - Transition دیگری مجاز نیست.

برای `TotalAmount`:

- در `Draft` قابل ویرایش است.
- در `Active` و `Suspended` افزایش مجاز است.
- کاهش فقط زمانی مجاز است که:\
  `NewTotalAmount >= AllocatedAmount`
- `AllocatedAmount` طبق مدل فعلی مجموع Grantهاست.
- Reserve و مصرف از همان اعتبار تخصیص‌یافته انجام می‌شوند.
- اگر شرط کاهش برقرار نباشد، Validation/Business Error برگردانده شود.

برای Close:

- وجود `AllocatedAmount` یا Grant مصرف‌نشده به‌تنهایی مانع Close نیست.
- `Reserved` Reservation فعال باید تعیین تکلیف شده باشد و وجود آن مانع Close است.
- Reservation منقضی‌شده یا تکمیل‌شده مانع Close نیست.
- Assignment یا Grant تاریخی/تمام‌شده به‌تنهایی مانع Close نیست.
- اگر مدل فعلی یک Assignment یا Grant را دارای تعهد مالی فعال تعریف می‌کند، همان تعهد باید بررسی شود.
- در صورت وجود ambiguity در تعریف «تعهد فعال»، از روی کد و Statusهای موجود بررسی کن و حدس نزن؛ در صورت نبودن Rule قطعی، فقط همان مورد را به‌عنوان Business blocker گزارش کن.

برای Expiration:

- پایان بازه اعتبار باید باعث قرار گرفتن Program در `Expired` شود، مطابق مکانیزم و Convention موجود پروژه.
- بعد از `Expired`، Grant و Reserve جدید ممنوع است.
- هیچ Transition دیگری از `Expired` مجاز نیست.

برای Reservation:

در `WalletReservationService` ایجاد Reserve جدید فقط زمانی مجاز است که Program در وضعیت `Active` باشد. این Rule باید در Service enforce شود، نه فقط در UI.

برای Authorization:

`YashilAuthorization` در این پروژه برای کنترل دسترسی Role کاربر به یک Action خاص استفاده می‌شود.

Permission/Action جدید ایجاد نکن.

ابتدا Action/Permission موجود و مناسب برای عملیات مدیریتی مشابه را در Repository پیدا کن و از همان Convention استفاده کن.

این Authorizationها باید برای موارد زیر در لایه مناسب موجود اعمال شوند:

- ایجاد WalletProgram
- تغییر بودجه
- تغییر Lifecycle

Authorization صرفاً با مخفی‌کردن UI کافی نیست و باید در مسیر Service/API نیز enforce شود.

اگر Action/Permission مناسب و موجود در Repository پیدا نشد، قبل از ایجاد مورد جدید متوقف شو و فقط همان مورد را به‌عنوان Architecture/Authorization blocker گزارش کن.

در نهایت:

- قبل از Implementation، Serviceهای Grant، Assignment و Reservation و الگوی Authorization موجود را با این Rules تطبیق بده.
- از `WalletProgramStatusEnum` جدید استفاده کن.
- Entity، DbContext، EF Mapping، Migration، DDL، Index یا Constraint را تغییر نده.
- CommonBaseData جدید ایجاد نکن.
- اگر برای اجرای این Rules نیاز به تغییر Schema وجود داشت، متوقف شو و گزارش بده.
- در غیر این صورت Implementation را ادامه بده.


---

## assistant
شناسه‌ها و گذارهای وضعیت روشن شد. اکنون Enum اضافه‌شده، مسیرهای Grant/Assignment/Reserve و Actionهای مجوز موجود را بررسی می‌کنم. معیار توقف باقی‌مانده این است که برای عملیات مدیریتی WalletProgram هیچ Permission قابل‌استفاده‌ای در Repository پیدا نشود.

---

## assistant
## Self-Gate — Architecture/Authorization blocker

`WalletProgramStatusEnum` با هر پنج شناسه اعلام‌شده در [کد موجود](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:225>) تأیید شد. قاعده کاهش بودجه نیز با مدل فعلی روشن است: `NewTotalAmount >= AllocatedAmount`.

**مانع باقی‌مانده، مجوز است.** Repository الگوی `YashilAuthorization` و Actionهای عمومی مانند `PostEntity` و `UpdateEntity` را نشان می‌دهد، اما Action/Permission **موجود و قابل انتساب به مدیریت مالی WalletProgram** در منابع بررسی‌شده پیدا نشد. استفاده از مجوز یک Feature دیگر، دسترسی مالی موردنظر شما را تضمین نمی‌کند. طبق دستور صریح شما، Permission یا Action جدید ایجاد نمی‌کنم و Implementation را اینجا متوقف می‌کنم.

لطفاً مشخص کنید کدام **Permission/Action موجود** برای ایجاد Program، تغییر بودجه و تغییر Lifecycle استفاده شود. هیچ فایلی تغییر نکرده است.

---

## user
در مورد Authorization، با توجه به بررسی Repository، تصمیم زیر را اعمال کن:

1. برای CRUD پایه WalletProgram از Actionهای عمومی موجود در `YashilAuthorization` استفاده کن، مشروط بر اینکه دقیقاً مطابق الگوی موجود پروژه باشند:

- ایجاد Program → `PostEntity`
- ویرایش Program → `UpdateEntity`

قبل از استفاده، الگوی واقعی مصرف این Actionها را در چند Feature مشابه بررسی کن و همان Convention را عیناً دنبال کن.

2. تغییرات حساس Business را صرفاً به دلیل داشتن `UpdateEntity` مجاز تلقی نکن.

تغییرات زیر باید همچنان از طریق Service و Business Rule کنترل شوند:

- تغییر `TotalAmount`
- تغییر Lifecycle
- `Activate`
- `Suspend`
- `Resume`
- `Close`

یعنی داشتن مجوز `UpdateEntity` نباید باعث bypass شدن Business Ruleهای بالا شود.

3. فعلاً Action/Permission جدید ایجاد نکن.

اگر در معماری فعلی پروژه `PostEntity` و `UpdateEntity` واقعاً مجوز دسترسی CRUD به Entity را کنترل می‌کنند، از همان‌ها برای دسترسی پایه WalletProgram استفاده کن.

4. برای Lifecycle:

اگر در Repository الگوی مشخصی وجود دارد که تغییر Status موجودیت‌ها را نیز با `UpdateEntity` کنترل می‌کند، همان الگو را استفاده کن؛ اما Transitionهای مجاز باید در Service enforce شوند.

اگر برای عملیات Lifecycle در Featureهای مشابه Action اختصاصی وجود دارد، ابتدا همان الگو را بررسی کن، ولی Action جدید برای WalletProgram ایجاد نکن.

5. برای تغییر Budget:

`UpdateEntity` به‌عنوان مجوز دسترسی پایه کافی است، اما Validation زیر الزامی است:

`NewTotalAmount >= AllocatedAmount`

و تغییر Budget نباید از طریق مسیر عمومی Update باعث bypass شدن این Rule شود.

6. بنابراین فعلاً Architecture/Authorization blocker را رفع‌شده در نظر بگیر، مشروط به اینکه بررسی کد تأیید کند `PostEntity` و `UpdateEntity` واقعاً برای CRUD مشابه مورد استفاده قرار می‌گیرند.

اگر الگوی موجود Repository نشان داد که `PostEntity`/`UpdateEntity` برای این نوع Entity مناسب نیستند، یا مسیر جداگانه‌ای برای عملیات مدیریتی وجود دارد، آن مورد را گزارش کن و Implementation را متوقف کن.

هیچ Permission یا Action جدید ایجاد نکن.

سایر Constraints قبلی همچنان پابرجاست:

- Entity تغییر نکند.
- DbContext تغییر نکند.
- EF Mapping تغییر نکند.
- Migration/DDL/Index/Constraint ایجاد یا تغییر نکند.
- CommonBaseData جدید ایجاد نشود.
- از `WalletProgramStatusEnum` موجود استفاده شود.


---

## assistant
مجوز پایه را طبق تصمیم شما با `PostEntity` و `UpdateEntity` تطبیق می‌دهم. دستورالعمل‌های `backend-feature` و `angular-feature` پروژه را برای رعایت الگوی موجود می‌خوانم، سپس چند CRUD مشابه و مسیر تغییر وضعیت را بررسی می‌کنم تا راهی برای دور زدن Ruleهای Service باقی نماند.

---

## assistant
الگوی مجوز تأیید شد: یک عملیات مدیریتی مشابه با `PostEntity` محافظت می‌شود و عملیات تغییر وضعیت مشابه با `UpdateEntity` به Service واگذار می‌شود. پیش از Implementation، یک نکته فنی باقی است: باید ببینم پروژه برای گذار خودکار Program به `Expired` مکانیزم زمان‌بندی موجود دارد یا خیر؛ انتخاب مکانیزم تازه بدون شواهد، تغییر معماری خواهد بود.

---

## assistant
الگوی Quartz برای وضعیت‌های تاریخ‌دار و الگوی CRUD با `PostEntity`/`UpdateEntity` موجود است. Plan اجرایی این است: Service و API اختصاصی WalletProgram با همان Actionها، کنترل بودجه و Transition در Service، محدودکردن Reserve به Program فعال، و Job انقضا طبق الگوی موجود؛ سپس UI مدیریتی مطابق کامپوننت‌های مشترک Angular. فایل Enum که شما تغییر داده‌اید دست‌نخورده می‌ماند.

---

## assistant
Backend مسیرهای Create/Update/ChangeStatus را با کنترل Rule در Service دریافت کرد؛ Reserve نیز حالا وضعیت و بازه اعتبار Program را در خود Service و در Update شرطی Program می‌سنجد. پیش از تکمیل Client، Build محدوده Backend را اجرا می‌کنم تا خطاهای قراردادی و وابستگی‌ها زود مشخص شوند.

---

## assistant
در Client یک وابستگی مجوزی مهم پیدا شد: `BaseList` اجازه Create/Edit را از Resource با کلید `WalletProgram` و Actionهای موجود می‌خواند. وجود Action عمومی به‌تنهایی وجود این Resource و انتساب آن به نقش‌های مجاز را ثابت نمی‌کند؛ در Repository هم تعریف آن دیده نشد. طبق شرط توقف شما، Implementation را متوقف می‌کنم و تغییرات ناتمام این نوبت را برمی‌گردانم؛ Enum افزوده‌شده توسط شما باقی می‌ماند.

---

## assistant
با بازبینی شرط شما، نبود تعریف Resource در سورس به‌تنهایی نشان نمی‌دهد `PostEntity` و `UpdateEntity` نامناسب‌اند؛ الگوی کد استفاده از همین Actionها را تأیید می‌کند و داده مجوز در زمان اجرا مدیریت می‌شود. بنابراین این مورد را به‌عنوان محدودیت Verification نگه می‌دارم و Implementation را ادامه می‌دهم. تغییرات ناتمام قبلی پاک شده‌اند و اکنون نسخه نهایی را با کنترل مسیرهای Update می‌سازم.

---

## assistant
Backend و صفحه مدیریتی WalletProgram به مسیرهای موجود پروژه متصل شدند. اکنون Self-check را روی قرارداد API و فرم، کنترل هم‌زمانی و بودجه، مجوزهای `PostEntity`/`UpdateEntity` و تغییرات نهایی انجام می‌دهم. Build اولیه به‌دلیل نبود `project.assets.json` متوقف شد؛ آن را با خطای کد یکی حساب نمی‌کنم و بررسی ایستا را ادامه می‌دهم.

---

## assistant
## Implemented

پیاده‌سازی سورس **WalletProgram Lifecycle و مدیریت بودجه** انجام شد: ایجاد در `Draft`، مشاهده و ویرایش، گذارهای مجاز وضعیت، کنترل بودجه و بازه اعتبار، انقضای زمان‌بندی‌شده و صفحه مدیریتی Angular.

## Backend Changes

- [WalletProgramService](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletProgramService.cs>) قواعد مالی و Lifecycle را اعمال می‌کند؛ Update عمومی نمی‌تواند وضعیت یا بودجه را بدون Validation تغییر دهد.
- [WalletProgramController](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletProgramController.cs>) از Actionهای موجود `PostEntity` و `UpdateEntity` استفاده می‌کند. DTOها و قرارداد سرویس نیز اضافه شدند.
- [Reserve service](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs>) و [Repository آن](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs>) ایجاد رزرو جدید را به Program فعال و دارای بازه معتبر محدود می‌کنند.
- [Job انقضا](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/JobImpl/ExpireWalletProgramsJob.cs>) طبق الگوی Quartz موجود ثبت شد.

## Angular Changes

[List](<P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-list.component.ts>)، [فرم Create/Edit و Lifecycle](<P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.ts>) و مسیر `walletPrograms` اضافه شدند. حذف Program در UI فعال نشده است.

## Business Rules

کاهش بودجه زیر `AllocatedAmount` رد می‌شود؛ `Close` با Reservation دارای وضعیت `Reserved` رد می‌شود؛ `Expired` و `Closed` وضعیت نهایی‌اند. فعال‌سازی به اطلاعات معتبر، بودجه مثبت و بازه معتبر نیاز دارد. سرویس Grant در Repository وجود ندارد؛ کنترل وضعیت Program هنگام Grant باید در Feature آینده آن اعمال شود.

## Database Boundary

**Entity، DbContext، EF Mapping، Migration، DDL، Index، Constraint و CommonBaseData تغییر نکردند.** Enum وضعیت که پیش از این نوبت توسط شما اضافه شده بود دست‌نخورده ماند. Permission یا Action جدیدی ایجاد نشد.

## Verification

بررسی ایستای syntax فایل‌های جدید C# و `git diff --check` خطایی نشان ندادند. **Build تأیید نشد:** `dotnet build --no-restore` با `NETSDK1004` به‌دلیل نبود `project.assets.json` متوقف شد. در Client نیز فایل `package.json` یا فرمان Build قابل اتکا در مسیر بررسی‌شده موجود نبود؛ Angular compile اجرا نشد.

## Known Limitations

وجود Resource و انتساب Actionهای `PostEntity`/`UpdateEntity` به نقش‌های مجاز **در داده زمان اجرا** از Repository قابل تأیید نیست. Lookupهای نوع Program و روش تخصیص و اجرای Job نیز نیاز به بررسی در محیط اصلی دارند.

## Remaining Work

Build و آزمون یکپارچه در محیط دارای وابستگی‌ها، سپس تأیید مجوزهای زمان اجرا و Lookupهای WalletProgram.

---

## user
پیاده‌سازی این Feature را فعلاً تأیید می‌کنم و نیازی به Code Review مستقل در این مرحله نیست.

طبق فرآیند پروژه، Review و Verification جامع را بعد از تکمیل تمام Featureهای Roadmap انجام خواهیم داد.

Known Limitationهای فعلی را به‌عنوان موارد باز نگه دار، از جمله:

- عدم تأیید Build به دلیل نبود `project.assets.json`
- عدم اجرای Angular compile به دلیل نبود Build configuration قابل اتکا
- عدم تأیید Runtime Authorization
- عدم تأیید Lookupهای WalletProgram در محیط واقعی
- اعمال Rule مربوط به Grant در Feature مربوط به Grant، نه Feature فعلی

فعلاً این موارد نباید مانع ادامه Implementation Featureهای بعدی شوند، مگر اینکه یکی از آن‌ها مستقیماً مانع پیاده‌سازی Feature بعدی شود.

هیچ Review یا Refactoring گسترده‌ای روی Feature فعلی انجام نده.

به Feature بعدی Roadmap برو و همان فرآیند Self-Gate را اجرا کن:

1. ابتدا Scope و Business Ruleهای Feature را بررسی کن.
2. Repository و معماری موجود را بررسی کن.
3. اگر تصمیم Business/Architecture ضروری وجود ندارد، Self-Gate کن و سؤال بپرس.
4. اگر Ruleها مشخص هستند، Implementation را انجام بده.
5. Database Boundary فعلی همچنان پابرجاست.
6. بعد از پایان تمام Featureهای Roadmap، یک Full Review و Integration Verification جامع روی کل مجموعه انجام خواهیم داد.


---

## assistant
Feature بعدی در ترتیب Roadmap، **WalletScope و محدودیت‌های مصرف Program** است. ابتدا قواعد همین Feature و مدل موجود آن را بررسی می‌کنم. Known Limitationهای WalletProgram را باز نگه می‌دارم و در این مرحله Review یا Refactor آن انجام نمی‌دهم.

---

## assistant
## Handoff Summary

### Active task

The user accepted the **WalletProgram Lifecycle** implementation and directed us to move to the next Roadmap feature, **WalletScope and Program consumption restrictions**. Apply the same Specification → Repository Discovery → Self-Gate → Implementation process. Do **not** review or refactor WalletProgram now. Complete integration review is deferred until all Roadmap features are implemented.

### Progress this turn

Read only WalletScope documentation, Entity, EF Mapping, and targeted code references. **No files changed in this turn.**

- The MVP reference describes `WalletProgram → WalletScope → WalletAssignment` and Scope types **Company, Representation, City, Product**. It lists nullable `CompanyId`, `RepresentationId`, `CityId`, `ProductId`, `IsActive`, and creator metadata, but does **not** define matching semantics or lifecycle rules.
- [WalletScope entity](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletScope.cs>) has `long Id`, `int WalletProgramId`, `int WalletScopeType`, optional target IDs, `bool IsActive`, `byte[] RowVersion`, and navigation properties.
- [EF Mapping](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28226>) maps `shp.WalletScope`, indexes by Program/type/active and target IDs, default `IsActive=true`, and RowVersion as concurrency token.
- No WalletScope service, controller, or business implementation was found in targeted `YashilCommerce` code.
- Source: [MVP reference](<P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md:30>) and [Wallet context](<P:/Projects/walletProject/Wallet/docs/codex/wallet.md:5>).

### Likely Self-Gate decisions

Before implementation, determine from any remaining **narrowly relevant** source whether these rules exist. If absent, ask the user rather than inventing them:

1. How multiple active Scopes combine: OR within a type, AND across types, or another rule; what zero Scopes means.
2. Required target IDs for each Scope type, including whether City may also require `CompanyId`; whether multiple target IDs may coexist.
3. Who may create/edit/deactivate Scope and whether an Active Program with existing Assignments/Reservations permits Scope changes.
4. Whether WalletScope is only administrative CRUD now or must enforce consumption eligibility in Reserve. There is no WalletEligibilityService yet, so integration boundaries matter.
5. Verify existing enum/`CommonBaseData` IDs for WalletScopeType before using them.

The user’s wording “اگر تصمیم ... وجود ندارد، Self-Gate کن و سؤال بپرس” appears inverted in context; follow their established rule: **ask only when a necessary Business/Architecture decision remains unresolved**, otherwise implement.

### Persistent constraints and limitations

- Database is a fixed contract: **do not change Wallet Entities, DbContext, EF Mapping, Migration, DDL, Index, Constraint, or CommonBaseData**.
- New methods require meaningful **Persian documentation/comments**.
- Use existing backend/Angular patterns and generic `YashilAuthorization` actions where appropriate; do not create new Permission or Action.
- `clientApp` is outside the writable root. A previous turn successfully used `functions.exec` with `sandbox_permissions:"require_escalated"` to edit it when authorized.
- WalletProgram Known Limitations are **open but do not block subsequent features**: backend Build unavailable because `project.assets.json` is missing; Angular compile unavailable because no reliable build configuration was found; runtime Authorization and WalletProgram lookups unverified; Grant status check belongs in the Grant feature.
- Existing WalletProgram changes, including the user’s preexisting `WalletProgramStatusEnum`, must be preserved. No independent review or broad refactor now.

---

## assistant
Feature بعدی Roadmap را از مستندات و کدهای مرتبط شناسایی می‌کنم، سپس فقط Ruleهای لازم برای همان Feature را با قراردادهای موجود تطبیق می‌دهم. اگر تصمیم ضروری باز مانده باشد، همان مورد را برای Self-Gate مطرح می‌کنم؛ در غیر این صورت پیاده‌سازی را ادامه می‌دهم.

---

## assistant
برای این بررسی از دستورالعمل‌های `wallet-feature` و `backend-feature` استفاده می‌کنم. مسیر مستندات و Skillها در ریشه `Wallet` است؛ کد Backend در `backEnd` قرار دارد.

---

## assistant
تا اینجا `WalletScope` در مدل و Mapping موجود است، اما در مستنداتِ مرتبط فقط نوع Scopeها و فیلدهای آن آمده است. اکنون مسیرهای مرتبط با Eligibility/Reserve و کدهای مشابه محدودسازی مصرف را بررسی می‌کنم تا روشن شود Ruleهای تطبیق Scope از قبل در پروژه تعریف شده‌اند یا نیاز به تصمیم شما دارند.

---

## assistant
## Self-Gate: WalletScope

Feature بعدی را **WalletScope و محدودیت مصرف Program** در نظر گرفتم. [مرجع Wallet MVP](<P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md:30>) نوع‌ها و فیلدهای Scope را معرفی می‌کند و [Entity موجود](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletScope.cs:62>) آن‌ها را نگه می‌دارد؛ اما Rule قطعی برای تطبیق Scopeها و اثر تغییر آن‌ها بر مصرف تعریف نشده است. در کد مرتبط نیز Service یا منطق Eligibility برای WalletScope پیدا نشد.

برای ادامه Implementation، پاسخ سه تصمیم Business در درخواست Self-Gate لازم است: نحوه ترکیب Scopeها و منبع داده تطبیق؛ شناسه‌های مجاز هر نوع و IDهای موجود آن‌ها؛ و اینکه کنترل Eligibility در Reserve جزو همین Feature است یا خیر، همراه با محدودیت تغییر Scope در Program فعال.

**فعلاً Implementation متوقف است.** هیچ فایلی تغییر نکرده و محدودیت‌های باز Feature قبلی همچنان مانع Featureهای بعدی تلقی نشده‌اند.

---

## user
Self-Gate مربوط به WalletScope را ادامه بده. تصمیم‌های Business زیر قطعی هستند و مبنای Implementation قرار بگیرند.

### 1. Scope Types

طبق Wallet MVP، چهار نوع Scope داریم:

- Company / مرکز
- Representation / نمایندگی
- City / شهر
- Product / محصول

از Entity، Enum یا CommonBaseData موجود در Repository استفاده کن و مقدارهای واقعی آن‌ها را از Repository/DB پیدا کن. IDها را حدس نزن و CommonBaseData جدید ایجاد نکن.

اگر مقدار قطعی یکی از Typeها در Repository/DB قابل استخراج نیست، فقط همان مورد را گزارش کن.

### 2. Scope Matching

Scopeها محدودکننده مصرف WalletProgram هستند.

Rules موجود:

- Company Scope → Company و Representation/Productهای زیرمجموعه آن
- Representation Scope → همان Representation و Productهای آن
- City Scope → Representationهای واقع در آن City
- Product Scope → فقط همان Product

### 3. ترکیب چند Scope

منطق ترکیب Scopeها:

**بین Scopeهای متفاوت: AND**

**بین چند Scope از یک Type: OR**

مثال:

Company = A\
City = Tehran\
Product = P1, P2

مصرف فقط زمانی مجاز است که:

- متعلق به Company A باشد،
- در محدوده Tehran باشد،
- و Product آن P1 یا P2 باشد.

اگر Program فقط یک Type از Scope داشته باشد، همان Type معیار Eligibility است.

اگر Program هیچ Scopeای نداشته باشد، آن را به معنی «بدون محدودیت Scope» در نظر بگیر؛ مگر اینکه مدل موجود صراحتاً رفتار دیگری تعریف کرده باشد.

### 4. Eligibility

Eligibility بخشی از همین Feature است.

جریان موردنظر:

WalletProgram\
→ WalletScope\
→ WalletAssignment\
→ Eligibility\
→ Reservation\
→ Capture/Release

در این Feature منطق تعیین اینکه یک مصرف با Scopeهای Program سازگار است یا خیر را پیاده‌سازی کن.

Reservation باید از همان Eligibility استفاده کند و ایجاد Reservation برای مصرفی که Scopeهای Program را satisfy نمی‌کند مجاز نباشد.

Rule مربوط به وضعیت Program که در Feature قبلی تعیین شد نیز همچنان برقرار است:\
Reservation جدید فقط برای Program در وضعیت Active و در بازه معتبر مجاز است.

### 5. تغییر Scope

در Draft:

- افزودن Scope مجاز است.
- حذف Scope مجاز است.
- ویرایش Scope مجاز است.

بعد از Active شدن Program:

- تغییر Scope مجاز نیست.
- افزودن، حذف یا تغییر Scope باید رد شود.

در Suspended نیز Scope قابل تغییر نیست.

در Expired و Closed نیز Scope قابل تغییر نیست.

دلیل این Rule این است که تغییر دامنه مصرف پس از عملیاتی شدن Program می‌تواند Eligibility اعتبار تخصیص‌یافته و مصرف‌های آینده را تغییر دهد.

### 6. Database Boundary

هیچ Entity، DbContext، EF Mapping، Migration، DDL، Index یا Constraint تغییر نکند.

Entityهای موجود WalletScope و سایر Entityهای مرتبط را همان‌طور که هستند استفاده کن.

اگر برای پیاده‌سازی Eligibility نیاز به تغییر Schema وجود داشت، متوقف شو و گزارش بده.

### 7. Authorization

از الگوی Authorization موجود پروژه استفاده کن و Permission/Action جدید ایجاد نکن.

مدیریت Scope باید تحت همان دسترسی مدیریتی موجود برای مدیریت WalletProgram قرار داشته باشد.

### 8. قبل از Implementation

ابتدا Entityهای زیر و روابط واقعی آن‌ها را بررسی کن:

- WalletProgram
- WalletScope
- WalletAssignment
- WalletReservation
- Order / OrderItem
- Product
- Company
- Representation
- City

همچنین Service/Repositoryهای موجود مربوط به این روابط را پیدا کن و از همان منبع داده برای Eligibility استفاده کن.

مخصوصاً برای Company/Representation/City/Product hierarchy هیچ Join یا رابطه جدیدی اختراع نکن؛ از روابط واقعی موجود در Repository استفاده کن.

اگر Ruleهای بالا با مدل واقعی Repository ناسازگار بود، همان مورد را به‌عنوان Business/Architecture blocker گزارش کن؛ در غیر این صورت Implementation را ادامه بده.

پس از Implementation، مانند Feature قبلی فعلاً Code Review مستقل انجام نده. Review جامع بعد از تکمیل کل Featureهای Roadmap انجام خواهد شد.


---

## assistant
تصمیم‌های Scope و Eligibility را مبنا قرار می‌دهم. ابتدا IDهای واقعی Typeها، روابط Order/Product/Representation/City و مسیر فعلی Reserve را بررسی می‌کنم. اگر این قراردادها با Ruleها سازگار باشند، پیاده‌سازی را در همان لایه‌های موجود انجام می‌دهم.

---

## assistant
رابطه‌های مستقیم OrderItem → Representation و OrderItem → ProductSalesPlanning → Product پیدا شد. یک نکته تعیین‌کننده باقی مانده است: `Representation` در Entity ظاهراً `CityId` مستقیم ندارد. مسیر واقعی شهر را در مدل و سرویس‌های مرتبط بررسی می‌کنم؛ همچنین مقدارهای `WalletScopeType` هنوز در Enum یا فایل‌های Repository دیده نشده‌اند.

---

## assistant
**Self-Gate متوقف شد.** مقدار قطعی `WalletScopeType` برای هر چهار نوعِ **Company، Representation، City و Product** از Repository قابل استخراج نیست. [WalletScope](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletScope.cs:62>) این مقدار را به `CommonBaseData` ارجاع می‌دهد، اما Enum یا داده‌ای شامل IDهای چهار نوع در Repository پیدا نشد. اتصال مشخصی به DB پروژه نیز در محیط فعلی در دسترس نیست.

لطفاً IDهای موجود این چهار رکورد در `CommonBaseData` یا مسیر دسترسی به DB مربوط را ارائه کنید. تا آن زمان از حدس‌زدن IDها خودداری می‌کنم. **هیچ فایلی تغییر نکرده است.**

---

## user
enum اضافه شد
پس Self-Gate قبلی برطرف شده و می‌توانی با همین مقادیر ادامه Implementation مربوط به WalletScope و Eligibility را انجام بدهی.


---

## assistant
Enum جدید را بررسی می‌کنم و با Ruleهای تأییدشده تطبیق می‌دهم. سپس مدیریت Scope و کنترل Eligibility در Reserve را با استفاده از روابط موجود پیاده‌سازی می‌کنم؛ مرز Database و تغییرات Feature قبلی حفظ می‌شوند.

---

## assistant
Enum چهار نوع را با IDهای 3173 تا 3176 تأیید کردم. مسیر شهر هم در پروژه وجود دارد: `OrderItem.Representation → Representation.Address.CityId`. برای Reserve که در سطح Order ثبت می‌شود، Eligibility را بر همه آیتم‌های همان Order اعمال می‌کنم؛ چون درخواست Reserve شناسه آیتم یا سهم مبلغ هر آیتم را ندارد.

---

## assistant
مدیریت Scope در Draft و کنترل Eligibility در مسیر Reserve به Backend اضافه شد. اکنون API مدیریتی را با الگوی دسترسی WalletProgram و UI موجود مرتبط می‌کنم، سپس فقط بررسی‌های محدودِ قابل اجرا را انجام می‌دهم.

---

## assistant
یک نکته هم‌زمانی را در مسیر UI پوشش دادم: تغییر Scope نسخه `WalletProgram` را عوض می‌کند، بنابراین فرم Program پس از ثبت یا حذف Scope دوباره بارگذاری می‌شود. حالا بررسی Build و صحت اتصال فایل‌های جدید به DI/API را انجام می‌دهم.

---

## assistant
Backend Build به همان محدودیت شناخته‌شدهٔ نبود `project.assets.json` برخورد کرد. برای Angular نیز در این Checkout، `tsconfig` و `node_modules` قابل اتکا موجود نیست. در پایان، فقط اتصال Serviceها، فایل‌های تغییرکرده و نبود تغییر در Entity/DbContext را بررسی می‌کنم؛ Review مستقل را طبق درخواست شما به مرحله نهایی Roadmap واگذار می‌کنم.

---

## assistant
پیاده‌سازی **WalletScope و Eligibility** انجام شد:

- مدیریت Scope در وضعیت `Draft` به [Service و API](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletScopeService.cs>) اضافه شد. افزودن، ویرایش و حذف پس از خروج Program از Draft در Service رد می‌شود و API از Actionهای موجود WalletProgram استفاده می‌کند.
- [Eligibility](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletEligibilityService.cs>) با Enum جدید و روابط موجود پیاده شد: بین نوع‌ها `AND` و درون هر نوع `OR`. چون Reserve در سطح Order است، همه اقلام Order باید با Scopeهای فعال سازگار باشند. [Reserve](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs>) نتیجه را پیش از رزرو موجودی کنترل می‌کند.
- مدیریت Scope به [فرم WalletProgram](<P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.ts>) اضافه شد. Entity، DbContext، Mapping و Schema تغییر نکردند.

بررسی محدود اتصال Serviceها و فایل‌ها انجام شد. **Build تأیید نشد**: `project.assets.json` موجود نیست. Angular compile و رفتار Runtimeِ Authorization و Lookupها نیز هنوز تأیید نشده‌اند؛ این موارد برای Integration Verification نهایی Roadmap باز می‌مانند.

---

## user

# Files pasted by the user:

## "# Master Feature Prompt — WalletAssignment Feature بعدی Roadmap: **WalletAssign…": C:\Users\pc\.codex/attachments/d5a2e93f-4df3-4c27-9246-4a8cf5517cb8/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
درخواست WalletAssignment را از فایل پیوست می‌خوانم و طبق همان Scope، ابتدا Specification و قراردادهای موجود را بررسی می‌کنم. سپس Self-Gate را انجام می‌دهم و فقط اگر تصمیم ضروری باز نمانده باشد وارد Implementation می‌شوم.

---

## assistant
مدل Assignment برای User و Program رابطهٔ مستقیم دارد، اما فعلاً دو بخش لازم برای Create روشن نیست: IDهای Status/SourceType در Enumهای موجود دیده نشدند، و مستند MVP فیلدهای `SourceId/SourceRef` را ذکر می‌کند که در Entity فعلی نیستند. Mapping و مصرف فعلی Assignment را بررسی می‌کنم تا مشخص شود آیا می‌توان بدون حدس و بدون ورود به Grant، رکورد معتبری ایجاد کرد.

---

## assistant
**Self-Gateِ WalletAssignment هنوز عبور نکرده است.** رابطهٔ Assignment با `WalletProgram` و `User` در [Entity](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletAssignment.cs:63>) مشخص است، اما برای Create و کنترل Reserve سه تصمیم ضروری باز مانده است:

- IDهای Status و SourceType در Enum یا داده‌های Repository پیدا نشدند. [Mapping](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27399>) فقط نشان می‌دهد فیلتر «یک Assignment فعال برای هر User/Program» از مقدار `3177` استفاده می‌کند.
- [مستند MVP](<P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md:44>) Assignment را دارای `AssignedAmount` می‌داند، ولی Grant به Feature بعدی موکول شده است. باید مشخص شود ایجاد Assignment در این مرحله با مبلغ صفر است یا مبلغ مثبت و، در حالت دوم، اثر آن بر `AllocatedAmount` چیست.
- `SourceId` و `SourceRef` در مستند آمده‌اند، اما در Entity فعلی وجود ندارند. محدودهٔ منبع و قواعد ویرایش یا حذف Assignment نیز باید تعیین شوند.

این موارد را در درخواست Self-Gate پرسیده‌ام. تا دریافت پاسخ، **هیچ فایلی تغییر نکرده است**.

---

## user
Self-Gate WalletAssignment را با تصمیم‌های زیر ادامه بده:

### 1. WalletAssignmentStatusType

مقادیر قطعی از `CommonBaseData`:
```csharp
public enum WalletAssignmentStatusTypeEnum
{
    Active = 3177,
    Suspended = 3178,
    Expired = 3179
}

```

Mapping:

- `3177` → فعال → Active
- `3178` → متوقف شده → Suspended
- `3179` → منقضی شده → Expired

در DB رکوردی برای `Cancelled` وجود ندارد؛ بنابراین **Cancelled را ایجاد یا حدس نزن**.

---

### 2. WalletAssignmentSourceType

مقادیر قطعی:
```csharp
public enum WalletAssignmentSourceTypeEnum
{
    Manual = 3180,
    Organization = 3181,
    Card = 3182,
    Coupon = 3183
}

```

Mapping:

- `3180` → دستی → Manual
- `3181` → سازمانی → Organization
- `3182` → شماره کارت → Card
- `3183` → کوپن / کد تخفیف → Coupon

---

### 3. مرز Assignment و Grant

در Feature فعلی **WalletAssignment با مبلغ صفر ایجاد می‌شود**:
```text
AssignedAmount = 0

```

Assignment در این مرحله مفهوم «تخصیص/عضویت کاربر در WalletProgram» را دارد و هنوز ایجاد اعتبار مالی نمی‌کند.

بنابراین:

- Create Assignment → `AssignedAmount = 0`
- Create Assignment نباید `WalletProgram.AllocatedAmount` را افزایش دهد.
- Assignment نباید Ledger/Grant ایجاد کند.
- افزایش `AllocatedAmount` و ایجاد اعتبار واقعی به Feature بعدی یعنی **Grant** موکول می‌شود.
- در Feature Grant، مبلغ اعتبار به Assignment/کاربر تخصیص داده خواهد شد و منطق مالی آنجا پیاده می‌شود.

تا قبل از Grant، Assignment با موجودی صفر نباید بتواند باعث Reserve موفق شود.

---

### 4. SourceId / SourceRef

در Entity فعلی `SourceId` و `SourceRef` وجود ندارند.

بنابراین:

- Entity را برای اضافه‌کردن این فیلدها تغییر نده.
- Migration/Schema ایجاد نکن.
- فقط از فیلدها و روابط واقعاً موجود استفاده کن.
- اگر مستند MVP به این فیلدها اشاره کرده ولی مدل فعلی آن‌ها را ندارد، این اختلاف را در گزارش نهایی به‌عنوان Known Limitation ثبت کن.

---

### 5. Status Rules

با توجه به Statusهای واقعی موجود:

- `Active` → Assignment قابل استفاده است.
- `Suspended` → Assignment موقتاً غیرفعال است و نباید برای Reserve استفاده شود.
- `Expired` → Assignment منقضی است و نباید برای Reserve استفاده شود.

Reserve باید علاوه بر `WalletProgram` و `Eligibility`، وضعیت Assignment را نیز کنترل کند.

Assignment با `AssignedAmount = 0` نیز نباید Reserve موفق ایجاد کند.

---

### 6. Database Boundary

همان Database Boundary قبلی پابرجاست:

- Entity تغییر نکند.
- DbContext تغییر نکند.
- EF Mapping تغییر نکند.
- Migration / DDL / Index / Constraint ایجاد نشود.
- CommonBaseData تغییر نکند.

اضافه‌کردن Enum با مقادیر بالا مجاز است.

---

با این تصمیم‌ها Self-Gate باید بسته شود و Implementation را ادامه بده.

اگر در حین Implementation به Business/Architecture blocker دیگری برخورد کردی که با Repository، Entity و مستندات قابل حل نیست، فقط همان مورد را گزارش کن و متوقف شو.

بعد از Implementation Code Review جداگانه انجام نده؛ Review جامع در پایان کل Roadmap انجام خواهد شد.


---

## assistant
تصمیم‌های جدید Self-Gate را می‌بندند: Assignment در این Feature با موجودی صفر ساخته می‌شود و Grant به مرحلهٔ بعد می‌ماند. Create/List را بر پایهٔ User و WalletProgram موجود پیاده می‌کنم و در Reserve وضعیت، بازهٔ اعتبار و موجودی Assignment را کنترل می‌کنم. برای تغییر وضعیت یا مبلغ، چون Transition و Rule مالی تعیین نشده است، مسیر Update/Delete عمومی باز نمی‌کنم.

---

## assistant
Enumهای Status و SourceType از قبل در Repository اضافه شده‌اند. برای مسیر مدیریتی فعلی، Create را به Assignment دستیِ با مبلغ صفر محدود می‌کنم؛ Card/Coupon/Organization بدون مرجع قابل راستی‌آزمایی در این مرحله ساخته نمی‌شوند. Target همان `UserId` موجود است و کنترل Scope در زمان Reserve از `WalletEligibilityService` فعلی انجام می‌شود.

---

## assistant
مسیر Create/List برای Assignment اضافه شد و Reserve اکنون علاوه بر Program و Scope، Status و بازهٔ Assignment را هم در Service و در Update شرطی موجودی کنترل می‌کند. مرحلهٔ پایانی را به Verification محدود می‌کنم: `git diff --check`، ارجاع‌های DI/API/DTO و تلاش Build بدون Restore.

---

## assistant
**WalletAssignment پیاده‌سازی شد.** مسیر مدیریتی فعلی فقط Assignment دستی با اعتبار صفر می‌سازد؛ Grant، افزایش `AllocatedAmount` و Ledger به Feature بعدی موکول ماندند.

- **فایل‌ها:** [DTO](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletAssignmentDto.cs>)، [قرارداد Service](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletAssignmentService.cs>)، [Service](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletAssignmentService.cs>)، [Controller](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletAssignmentController.cs>) و [ثبت DI](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs>) اضافه یا ویرایش شدند. [Service رزرو](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs>)، [Repository رزرو](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs>) و فایل‌های [TypeScript](<P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.ts>) و [HTML](<P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.html>) فرم WalletProgram نیز تغییر کردند.
- **Ruleها و روابط:** Create فقط برای User فعال در Application جاری و Program فعال در بازهٔ معتبر انجام می‌شود. رکورد با `AssignedAmount = 0`، Status برابر `Active` و SourceType برابر `Manual` ساخته می‌شود؛ قید موجودِ یک Assignment فعال برای هر User/Program رعایت می‌شود. Reserve اکنون Status، بازه و موجودی Assignment را در Service و Update شرطی کنترل می‌کند و همچنان از Eligibility موجود برای Scope استفاده می‌کند. رابطه‌های `WalletAssignment → User/WalletProgram` و روابط فعلی Reserve مبنا بودند.
- **Database Boundary:** کامل رعایت شد؛ Entity، DbContext، Mapping، Migration، Index و CommonBaseData تغییر نکردند. اختلاف مستند MVP با Entity دربارهٔ نبود `SourceId` و `SourceRef` یک **Known Limitation** باقی است. مسیر عمومی Update/Delete یا تغییر Status بدون Rule مصوب باز نشده است.
- **Verification:** بررسی Syntax فایل‌های C# با Roslyn خطایی نشان نداد. Build به علت نبود `project.assets.json` اجرا نشد؛ Angular compile و رفتار Runtimeِ Authorization و Lookupها نیز تأیید نشده‌اند. `git diff --check` فقط فاصله‌های انتهاییِ موجود در بخش WalletScope فایل `Enums.cs` را گزارش کرد؛ در فایل‌های این Feature فاصلهٔ انتهایی پیدا نشد.

---

## user

# Files pasted by the user:

## "# Master Feature Prompt — WalletGrant Feature بعدی Roadmap: **WalletGrant** ## …": C:\Users\pc\.codex/attachments/af47e1a7-f1f0-4a0d-910a-1ef1eee59aab/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
درخواست WalletGrant را از پیوست می‌خوانم. سپس فقط قراردادهای مالی و مسیرهای مرتبط با Grant را با Assignment و Program موجود تطبیق می‌دهم تا پیش از هر تغییر، Self-Gate این Feature روشن شود.

---

## assistant
در Repository، Entity مستقلی به نام `WalletGrant` وجود ندارد. تنها ثبت رخداد مالیِ مرتبط، `WalletTransaction` است؛ سند MVP هم `Grant` را یکی از نوع‌های آن می‌داند. اکنون Mapping و شناسه‌های واقعی Transaction را بررسی می‌کنم، چون ثبت Grant بدون نوع/جهت معتبر و مرجع قابل تکرار، یک تصمیم مالیِ حدسی خواهد بود.

---

## assistant
**Self-Gateِ WalletGrant متوقف شد؛ هیچ فایلی تغییر نکرد.**

در Repository، Entity مستقلی برای `WalletGrant` وجود ندارد. [WalletTransaction](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:62>) می‌تواند رکورد مالی مرتبط باشد، اما ID قطعیِ نوع `Grant` و جهت `Credit` پیدا نشد. Entity فیلد Direction دارد، در حالی که [سند MVP](<P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md:135>) آن را حذف‌شده توصیف می‌کند. بدون تعیین مرز Ledger، مبنای ثبت «مجموع مبلغ Grantها» نیز روشن نیست.

همچنین [Mapping تراکنش](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28355>) قید یکتایی برای مرجع Grant ندارد و قرارداد Idempotency مشخص نشده است. کنترل شرطی بودجه و Transaction را می‌توان با الگوهای موجود انجام داد، اما آن‌ها به‌تنهایی تکلیف **درخواست تکراری Grant** را تعیین نمی‌کنند.

دو تصمیم لازم دربارهٔ ثبت Ledger و قرارداد Idempotency را در درخواست Self-Gate مطرح کرده‌ام. پس از دریافت آن‌ها Implementation را ادامه می‌دهم.

---

## user

# Files pasted by the user:

## "Self-Gate مربوط به WalletGrant را با تصمیم‌های زیر ادامه بده: ## 1. مرز Grant و…": C:\Users\pc\.codex/attachments/96007e17-4fb3-4e4f-8838-4ab86c556783/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
تصمیم‌های جدید WalletGrant را از پیوست می‌خوانم و با Entity و Mapping موجود تطبیق می‌دهم. اگر مرز مالی و Idempotency اکنون قطعی باشند، پیاده‌سازی را ادامه می‌دهم.

---

## assistant
در کد، IDهای `Grant` و `Credit` پیدا نشدند. SQL Server محلی در حال اجراست، اما اتصال Read-only از حساب Sandbox با خطای احراز هویت/SSL شکست خورد. یک بار از مسیر دسترسی محلیِ مجاز برای خواندن داده‌های واقعی تلاش می‌کنم؛ اگر دسترسی فراهم نباشد، مطابق Self-Gate فقط همین شناسه‌ها را Blocker گزارش می‌کنم.

---

## assistant
**Self-Gateِ WalletGrant متوقف شد؛ هیچ فایلی تغییر نکرد.**

ID قطعی `WalletTransactionType = Grant` و `WalletTransactionDirectionType = Credit` در Enumها یا داده‌های در دسترس پیدا نشد. مقدار `Credit = 1` در یک Enum دیگر، متعلق به WalletTransaction نیست و نمی‌توان از آن استفاده کرد.

اتصال Read-only به SQL Server محلی برقرار شد، اما هیچ‌یک از DBهای محلی جدول WalletTransaction یا WalletAssignment ندارند و در CommonBaseDataهای موجود نیز رکورد مرتبط پیدا نشد. لطفاً این دو ID قطعی یا دسترسی به DB واقعی Wallet را ارائه کنید تا Implementation ادامه یابد.

---

## user
مقادیر قطعی WalletTransactionType و WalletTransactionDirectionType از DB مشخص شد.

### WalletTransactionType
```csharp
public enum WalletTransactionTypeEnum
{
    Grant = 3187,             // اعطای اعتبار
    Capture = 3188,           // ثبت نهایی / مصرف واقعی
    Release = 3189,           // برگشت اعتبار کیف پول
    Expired = 3190,           // منقضی
    ManualAdjustment = 3191   // اصلاح دستی
}

```

### WalletTransactionDirectionType
```csharp
public enum WalletTransactionDirectionTypeEnum
{
    Debit = 3192,   // بدهکار
    Credit = 3193   // بستانکار
}

```

برای WalletGrant:
```text
WalletTransactionType = Grant = 3187
WalletTransactionDirectionType = Credit = 3193

```

این مقادیر قطعی و از `CommonBaseData` استخراج شده‌اند؛ ID دیگری ایجاد یا حدس زده نشود.

### Self-Gate

Self-Gate مربوط به Type و Direction اکنون برطرف شده است.

با توجه به تصمیمات قبلی:

- Grant به‌صورت `WalletTransaction` ثبت می‌شود.
- Entity مستقل `WalletGrant` ایجاد نشود.
- Grant با Type = `3187` و Direction = `3193` ثبت شود.
- `WalletProgram.AllocatedAmount` باید با Grant هماهنگ و Atomic باشد.
- Assignment باید `Active` باشد.
- Program باید `Active` باشد.
- Budget نباید Over-Allocate شود.
- `WalletTransaction.Code` به‌عنوان Idempotency Key استفاده شود.
- تکرار همان Key با Payload یکسان → Replay نتیجه قبلی.
- همان Key با Payload متفاوت → Conflict.
- Grant نباید Reservation/Capture/Release ایجاد کند.
- Database Boundary قبلی کاملاً پابرجاست.
- هیچ Index/Constraint جدیدی برای Idempotency ایجاد نکن.

اکنون Self-Gate را عبور بده و Implementation WalletGrant را شروع کن.

اگر بعد از این تصمیم‌ها به یک Business/Architecture blocker واقعی برخورد کردی، فقط همان مورد را گزارش کن؛ در غیر این صورت Implementation را کامل کن.

بعد از Implementation Code Review جداگانه انجام نده؛ Review جامع در پایان کل Roadmap انجام خواهد شد.


---

## assistant
IDهای Grant و Credit اکنون قطعی‌اند. برای جلوگیری از ثبت دوباره بدون Index جدید، مسیر موجودِ EF Transaction و Update شرطی را بررسی می‌کنم و Grant را به‌صورت یک عملیات اتمیک روی Program، Assignment و WalletTransaction می‌سازم.

---

## assistant
## Handoff: WalletGrant

**Active task:** Implement WalletGrant. The user resolved the Self-Gate and explicitly authorized implementation. **No files have been changed in this turn.**

### Confirmed contract

- No `WalletGrant` entity exists. Each Grant is a `WalletTransaction` with `WalletTransactionTypeEnum.Grant = 3187` and `WalletTransactionDirectionTypeEnum.Credit = 3193`. These enums are already present in [Enums.cs](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs>); do not duplicate them.
- `WalletTransaction.Code` is the request’s idempotency key. Same key and payload must replay the previous transaction; same key with different payload must return Conflict.
- One transaction must atomically insert the ledger row, increment `WalletProgram.AllocatedAmount`, and increment `WalletAssignment.AssignedAmount`. Grant amount is positive with two decimal places. Program and Assignment must be Active and within their validity windows. Program budget must remain sufficient.
- Multiple Grants may increase one Assignment’s credit. Grant creates no Reservation or consumption.
- **Database boundary:** No Entity, DbContext, EF mapping, migration, DDL, index, constraint, or CommonBaseData changes. No new permission/action. No separate code review; comprehensive review follows the Roadmap.

### Repository findings

- [WalletTransaction entity](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:62>) has `WalletAssignmentId`, `UserId`, `Code`, `WalletTransactionType`, `WalletTransactionDirectionType`, `Amount`, optional `PreviousBalance`/`NewBalance`, `OccurredAt`, and metadata. It has no dedicated idempotency field.
- [WalletTransaction mapping](<P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28355>) has no unique index on `Code`; amount is `decimal(18,2)`, Title is required. The user explicitly forbids adding an index.
- Assignment `AvailableAmount` is computed as `AssignedAmount - ConsumedAmount - ReservedAmount - ExpiredAmount` in DbContext around line 27413.
- Existing [WalletReservationService](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:53>) uses EF `BeginTransactionAsync`; its repository uses conditional `ExecuteUpdateAsync` and affected-row checks. [WalletAssignmentService](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletAssignmentService.cs:55>) also locks the Program row via conditional update in a transaction.
- [WalletProgramService](<P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletProgramService.cs:91>) rejects budget reductions below `AllocatedAmount`.
- Current Angular management UI is [wallet-program-detail.component.ts](<P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.ts>) and its HTML sibling. Assignment list and Create already appear there.
- WalletProgram/Assignment controllers use `YashilAuthorization` aliases for existing Program actions. Grant changes budget, so `UpdateEntity` on `WalletProgram` is the likely existing permission.

### Next steps

1. Resolve the **concurrency detail in implementation**: `Code` has no unique constraint. Use an EF transaction with appropriate isolation and the existing conditional Program update pattern so idempotency lookup, budget update, Assignment update, and ledger insert are atomic, including simultaneous same-key requests across Programs. Handle serialization conflicts safely; never claim duplicate protection from a missing constraint.
2. Add Grant DTO, interface, service, controller, DI, and a small Grant form/list in the existing Angular Program/Assignment UI. Use direct DTO projections as nearby Wallet services do; AutoMapper is unnecessary unless a real convention requires it. Add short Persian comments for each new method.
3. Validate amount, Application ownership, statuses, dates, budget, key length (`WalletTransaction.Code` max 150), and replay payload (`ApplicationId`, AssignmentId, Amount, Type, Direction). Keep `PreviousBalance`/`NewBalance` nullable if a consistent balance snapshot cannot be obtained.
4. Run C# syntax check and `git diff --check`; attempt `dotnet build --no-restore`. The known missing `project.assets.json` blocks Build, and Angular has no reliable compile setup. Existing trailing whitespace in the previously supplied WalletScope enum makes full `git diff --check` fail; do not refactor earlier features just for that.
5. Report the actual verification limits. Preserve all existing WalletProgram, Scope, Assignment, and Reservation work.

The user wants work carried through to implementation unless a **new, real Business/Architecture blocker** is found.

---

## assistant
پیاده‌سازی WalletGrant را بر اساس تصمیم‌های قطعی Type/Direction و مرز Database ادامه می‌دهم. ابتدا قرارداد دقیق Feature و الگوهای مرتبط فعلی را تطبیق می‌دهم، سپس تغییرات محدود و بررسی‌های قابل‌اجرای همین پروژه را انجام می‌دهم.

---

## assistant
برای این تغییر مالی، دستورالعمل‌های `wallet-feature` و `backend-feature` را به کار می‌گیرم؛ اگر UI لازم باشد، الگوی `angular-feature` را هم بررسی می‌کنم. مقادیر Enum در کد موجود هستند؛ تمرکز باقی‌مانده روی Atomicity، Idempotency و اتصال به مسیرهای فعلی است.

---

## assistant
قرارداد Grant با مدل فعلی سازگار است: Ledger همان `WalletTransaction` است و مانده Assignment از فیلدهای مالی موجود محاسبه می‌شود. برای درخواست‌های هم‌زمان، درج Ledger و افزایش دو مانده را در Transaction انجام می‌دهم و بررسی Key را در همان مرز نگه می‌دارم؛ قید یا Schema تازه‌ای اضافه نمی‌شود.

---

## assistant
مسیر Backend اکنون شامل DTO، Service، API و DI است. Build طبق محدودیت شناخته‌شده به‌دلیل نبود `project.assets.json` متوقف شد؛ در ادامه رابط مدیریتی موجود Program/Assignment را برای ثبت و مشاهده Grant تکمیل می‌کنم و سپس فقط Self-check محدود Feature را انجام می‌دهم.

---

## user
<environment_context>
  <current_date>2026-10-04</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## assistant
Self-check اولیه نشان می‌دهد منطق بودجه با `ExecuteUpdateAsync` شرطی اجرا می‌شود و اگر Assignment معتبر نباشد، کل Transaction بازگردانده می‌شود. یک نکتهٔ فنی را هم اصلاح می‌کنم: در خطای deadlock، SQL Server خود Transaction را باطل می‌کند؛ مسیر retry نباید با Rollback اضافی همان خطا را پنهان کند.

---

## assistant
پیاده‌سازی و بررسی‌های محدود Feature کامل شده‌اند. چهار فایل C# تازه از نظر Syntax بدون خطا هستند؛ Build همچنان فقط به‌خاطر نبود assets متوقف است. اکنون وضعیت فایل‌های همین Feature و مرز Database را یک بار نهایی تطبیق می‌دهم و نتیجه را گزارش می‌کنم.

---

## assistant
WalletGrant پیاده‌سازی شد. Grant به‌صورت `WalletTransaction` با Type=`3187` و Direction=`3193` ثبت می‌شود؛ Entity یا تغییر Database جدیدی ایجاد نشد.

فایل‌های این Feature: [DTO](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletGrantDto.cs>)، [Service interface](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletGrantService.cs>)، [Service](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletGrantService.cs>) و [Controller](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletGrantController.cs>) ایجاد شدند. [DI](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs>) و فرم ثبت و نمایش Grant در [رابط Program](</P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.ts>) نیز به‌روز شدند.

Service فقط برای Program و Assignment فعال در بازه اعتبار Grant می‌سازد. مبلغ باید مثبت و دارای حداکثر دو رقم اعشار باشد. کنترل شرطی بودجه، افزایش `AllocatedAmount` و `AssignedAmount`، و درج Ledger در یک Transaction انجام می‌شوند. چند Grant برای یک Assignment مجاز است. تکرار `ApplicationId + Code` با همان درخواست نتیجه قبلی را برمی‌گرداند و با مبلغ یا Assignment متفاوت، Conflict می‌دهد. Grant هیچ Reservation، Capture یا Release ایجاد نمی‌کند.

**Self-check:** چهار فایل C# جدید خطای Syntax ندارند و فایل‌های جدید بررسی‌شده خطای whitespace ندارند. Build به‌دلیل نبود `project.assets.json` انجام نشد؛ Angular compile و رفتار هم‌زمان در Runtime نیز تأیید نشده‌اند. `git diff --check` روی کل Repository فقط به whitespace موجود در بخش WalletScope از `Enums.cs` گیر می‌دهد. چون برای `WalletTransaction.Code` قید یکتا وجود ندارد و طبق دستور اضافه نشد، رفتار Idempotency و Concurrency پیاده‌شده با Transaction سطح `Serializable` باید در بررسی یکپارچه Roadmap در محیط واقعی تأیید شود.

---

## user
<environment_context>
  <current_date>2026-10-05</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user

# Files pasted by the user:

## "# Master Prompt — WalletReservationSettlement ## Feature **WalletReservationSet…": C:\Users\pc\.codex/attachments/b37cbf02-a4bb-403c-9d2d-a77a0e62468c/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن Feature جدید را می‌خوانم و Scope و تصمیم‌های لازم برای Self-Gate را با مسیرهای فعلی Wallet تطبیق می‌دهم. سپس، اگر Rule یا تصمیم ضروری باز نمانده باشد، پیاده‌سازی را در همان مرز Database پیش می‌برم.

---

## user

# Files pasted by the user:

## "State transition و Balance update و WalletTransaction insert باید در یک Atomic …": C:\Users\pc\.codex/attachments/53ee6ea7-32c0-42c4-8f84-7b628cdb1c5c/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
Self-Gate به یک مانع واقعی رسید: مدل Reservation فیلد `CapturedAt` دارد، اما در Enum فعلی فقط `Reserved`، `Released` و `Expired` ثبت شده‌اند. پیش از هر تغییر، مرز این اختلاف را با Ruleهای Reserve و قید Reservation فعال تطبیق می‌دهم تا دقیقاً مشخص شود چه تصمیمی لازم است.

---

## assistant
**Self-Gate متوقف شد؛ هیچ فایلی تغییر نکرد.**

برای Capture، Prompt انتقال Reservation به وضعیت نهایی `Captured` را لازم می‌داند، اما [Enum فعلی](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:218>) فقط `Reserved=3184`، `Released=3185` و `Expired=3186` دارد. وجود [CapturedAt](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs:69>) این خلأ را رفع نمی‌کند: [قید Reservation فعال](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28096>) و کنترل‌های فعلی همچنان `Status=Reserved` را فعال می‌دانند. نگه‌داشتن این Status پس از Capture، سفارش را در وضعیت رزروشده باقی می‌گذارد.

لطفاً **ID قطعی رکورد `Captured` یا `Completed` در `CommonBaseData`** را اعلام کنید؛ اگر چنین رکوردی وجود ندارد، برای ادامه به تصمیم صریح درباره وضعیت نهایی Capture نیاز است. بدون آن، پیاده‌سازی State Transition ایمن ممکن نیست.

---

## user

# Files pasted by the user:

## "# Master Prompt — Wallet MVP Specification Alignment ## Goal Implementationهای …": C:\Users\pc\.codex/attachments/b6214af7-ced7-4f1b-b00e-d8d78d89d467/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن Specification Alignment را می‌خوانم و محدودهٔ تغییرات مجاز آن را مشخص می‌کنم. سپس فقط همان بخش‌های مرتبط را با وضعیت فعلی Repository تطبیق می‌دهم.

---

## assistant
برای خواندن سند جامع `.docx` از مهارت documents استفاده می‌کنم و فقط بخش‌های مالی، وضعیت‌ها و تصمیم‌های صریح MVP را استخراج می‌کنم. الگوی Wallet را نیز با همان دستورالعمل مالی پروژه بررسی می‌کنم؛ این Task به Capture/Release جدید گسترش پیدا نمی‌کند.

---

## assistant
تطبیق نشان داد Grant از قبل مبلغ مثبت ثبت می‌کند و Reserve هیچ Ledger قطعی نمی‌سازد. تنها اصلاح کدی که بدون تغییر Entity/DB لازم است، حذف وابستگی خواندن و Replayِ Grant به `Direction` است؛ مقدار `Credit` هنگام درج همچنان برای سازگاری با ستون اجباری مدل فعلی نوشته می‌شود. اختلاف‌های Status و Transaction Type را به‌صورت Gap گزارش می‌کنم.

---

## assistant
## Wallet MVP Specification Alignment

**تنها فایل اصلاح‌شده:** [WalletGrantService.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletGrantService.cs>). فهرست Grant و بررسی Replay دیگر از `Direction` برای تعیین معنای مالی استفاده نمی‌کنند و Grant را با `Type=Grant` و `Amount>0` می‌شناسند. Grant همچنان مبلغ مثبت ثبت می‌کند و Reserve هیچ `WalletTransaction` قطعی نمی‌سازد.

**اختلاف‌های اصلاح‌نشده در مرز Database:** ستون واقعی `WalletTransactionDirectionType` اجباری است؛ بنابراین درج Grant همچنان مقدار موجود `Credit=3193` را برای سازگاری با مدل می‌نویسد. Entity، Mapping و DB تغییر نکردند. وضعیت‌های Assignment در کد `Active/Suspended/Expired` هستند، در حالی که سند `Active/Suspended/Exhausted/Cancelled` را تعریف می‌کند. Program نیز در کد وضعیت `Expired` دارد که در فهرست وضعیت‌های سند نیست. این موارد بدون تغییر Status یا داده باقی ماندند.

**تطبیق CommonBaseData و Enum:** مقادیر قطعی اعلام‌شده برای `Grant=3187`، `Capture=3188`، `Release=3189`، `Expired=3190`، `ManualAdjustment=3191` و `Credit=3193` در Enum موجودند. سند به‌جای سه نام اخیر، `Refund/Expire/Adjustment` را تعریف می‌کند. هیچ ID حدس زده یا Rename نشد؛ `Refund` و وضعیت Reservation از نوع `Captured` هنوز مقدار قطعیِ قابل‌تأیید از Repository ندارند.

**آمادگی Settlement:** Grant اکنون از نظر *Signed Amount* با سند منطبق است. Reservation برای Capture آماده نیست: [Enum وضعیت Reservation](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:218>) فقط `Reserved/Released/Expired` دارد. فیلد `CapturedAt` جای وضعیت نهایی را نمی‌گیرد، چون [قید Reservation فعال](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28096>) بر `Status=Reserved` استوار است. **ID قطعی `Captured` یا تصمیم صریح درباره وضعیت نهایی Capture، Blocker شروع Capture است.**

Capture/Release در این Task پیاده‌سازی نشدند. بررسی Syntax فایل اصلاح‌شده و `git diff --check` محدود به آن بدون خطا بود؛ Build و رفتار Runtime در این مرحله تأیید نشدند.

---

## user

# Files pasted by the user:

## "## Feature Wallet Reservation Settlement — Capture / Release ## Specification س…": C:\Users\pc\.codex/attachments/a6f7879b-9989-4bd0-98f5-0efc12c23cbf/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
Specification جدید Capture/Release را می‌خوانم و با Blocker وضعیت `Captured` و مرز Database فعلی تطبیق می‌دهم. اگر تصمیم‌های لازم در آن مشخص شده باشند، پیاده‌سازی Settlement را شروع می‌کنم.

---

## assistant
`Captured=3210` در Enum موجود است و Self-Gate وضعیت Reservation برطرف شده است. برای Settlement از همان فیلدهای `ReservedAmount` و `ConsumedAmount` استفاده می‌کنم: Capture کامل، با Payment ثبت‌شده و موفقِ همان Order/Application؛ Release فقط آزادسازی Reserve، بدون Ledger. هر گذار وضعیت و تغییر مانده در یک Transaction انجام می‌شود.

---

## assistant
منطق اصلی Service نوشته شده است. پیش از Build، عبارت‌های شرطی داخل `ExecuteUpdateAsync` را به دو مسیر صریح Capture و Release تبدیل می‌کنم تا ترجمهٔ EF برای ستون‌های زمانی و شمارنده‌های مالی روشن و قابل اتکاتر باشد؛ رفتار Feature تغییر نمی‌کند.

---

## assistant
مسیرهای Capture و Release اکنون کامل‌اند: Capture مبلغ کامل Reservation را از Reserve به Consumed منتقل و یک Ledger با مبلغ منفی ثبت می‌کند؛ Release فقط Reserve را آزاد می‌کند. برای Replayِ Capture، شناسه Payment نیز در توضیح Ledger ثبت و با درخواست تکراری تطبیق داده می‌شود. بررسی نهایی محدود به Syntax، ارجاع‌های Service/API و فایل‌های همین Feature است.

---

## assistant
## Wallet Reservation Settlement

**Capture و Release در Service و API پیاده‌سازی شدند.** فایل‌های جدید شامل [DTO](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletSettlementDto.cs>)، [Interface](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletSettlementService.cs>)، [Service](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs>) و [Controller](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletSettlementController.cs>) هستند؛ ثبت DI در [ModuleInitializer.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs>) اضافه شد.

- **Capture:** مبلغ کامل Reservation را فقط در وضعیت `Reserved`، پیش از `ExpiresAt` و با Payment موفقِ همان Order/Application می‌پذیرد. وضعیت را به `Captured=3210` می‌برد، `ReservedAmount` را کاهش و `ConsumedAmount` را در Assignment و Program افزایش می‌دهد و یک Ledger از نوع `Capture=3188` با مبلغ **منفی** ثبت می‌کند.
- **Release:** وضعیت `Reserved` را به `Released=3185` می‌برد و `ReservedAmount` را در Assignment و Program کاهش می‌دهد. `ConsumedAmount` و `AllocatedAmount` را تغییر نمی‌دهد و Ledger قطعی ایجاد نمی‌کند.
- **Idempotency و Concurrency:** Capture با `ApplicationId + Code` نتیجه قبلی را Replay می‌کند؛ تغییر Reservation، مبلغ، عملیات یا Payment با همان کلید Conflict است. Release با وضعیت نهایی Reservation تکرارپذیر است. گذار وضعیت، تغییر مانده و درج احتمالی Ledger در یک Transaction با isolation سطح `Serializable` و به‌روزرسانی شرطی انجام می‌شوند. API از مجوز مدیریتی موجود `UpdateEntity` برای WalletProgram استفاده می‌کند و کنترل‌های مالی در Service هستند.

**Angular تغییر نکرد:** Specification برای این عملیات مالی، فرم مدیریتی مشخصی الزام نکرده است. اتصال خودکار Service به callback پرداخت نیز در این تغییر انجام نشده؛ API برای فراخوانی Settlement آماده است. Payment و Reservation در مدل فعلی از طریق `OrderId` تطبیق داده می‌شوند و FK مستقیمی میان آن‌ها وجود ندارد. Job انقضای Reservation هم در Repository فعلی یافت نشد، بنابراین Race با آن در Runtime قابل تأیید نبود.

**Verification:** چهار فایل C# جدید خطای Syntax یا whitespace ندارند. Build با `NETSDK1004` به‌دلیل نبود `project.assets.json` متوقف شد؛ تست Runtime و هم‌زمانی اجرا نشد. `git diff --check` سراسری فقط whitespace موجود در `Enums.cs` را گزارش کرد. هیچ Entity، DbContext، Mapping، Migration، Index، Constraint یا CommonBaseData تغییر نکرد.

---

## user

# Files pasted by the user:

## "## Feature Wallet Order & Payment Integration ## Specification سند جامع `Wallet…": C:\Users\pc\.codex/attachments/c141fc75-bc27-472f-aa03-58ac9b7ad897/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
Specification اتصال Wallet به Order و Payment را می‌خوانم و محل دقیق Reserve، Capture و Release را در Flow فعلی پرداخت پیدا می‌کنم. سپس Self-Gate را روی Ruleهای مبلغ، مالکیت Order و مرز Database انجام می‌دهم.

---

## assistant
در Flow فعلی، `SaveShoppingCartToOrder` سفارش را ذخیره و به Controller روش پرداخت هدایت می‌کند. `CreateNewPaymentAndReturnAsync` خودش دوباره مبلغ را محاسبه می‌کند. همچنین مسیر `Wallet` فعلی به اعتبار کارت باشگاه مشتریان متصل است؛ بررسی می‌کنم آیا قرارداد Checkout برای انتخاب Assignment و مبلغ Wallet، و Source مربوط به Vibe در این Repository وجود دارد یا نه.

---

## assistant
**Self-Gate متوقف شد؛ هیچ فایلی تغییر نکرد.** وضعیت‌های Payment قابل تشخیص‌اند: `Paid=1095` موفق، `Unsuccessful=1125` ناموفق، `Cancel=1098` انصراف؛ `SendToPaymentGateway=1094` و `WaitingResponseFromPaymentGateway=1123` هنوز نتیجه قطعی نیستند.

دو قرارداد لازم برای اتصال امن در Source فعلی وجود ندارند:

1. [ShoppingCartParam](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs:3>)، Assignment، مبلغ Wallet و کلید Reserve را دریافت نمی‌کند. [CalcAmountForPayAsync](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412>) فقط `Total` را برمی‌گرداند و [ایجاد Payment](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552>) همان مبلغ را دوباره محاسبه می‌کند. بنابراین معلوم نیست Wallet باید **تمام `Total`** را رزرو کند یا بخشی از آن را، و Assignment و Idempotency Key از کجا می‌آیند.
2. [OrderController](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:790>) به `VibePayment` هدایت می‌کند، اما `VibePaymentController` و `VerifyVibeOrderAsync` در Source موجود نیستند. مسیر `Wallet` همین Controller نیز به پرداخت قدیمی کارت باشگاه مشتریان متصل است و نمی‌توان آن را بدون تصمیم صریح، مسیر WalletProgram فرض کرد.

لطفاً مشخص کنید **WalletProgram تمام `CalcAmountForPayAsync.Total` را می‌پردازد یا بخشی از آن را**؛ در حالت جزئی، مبلغ Wallet و مبلغ Payment چگونه تعیین می‌شوند؟ همچنین منبع انتخاب `WalletAssignmentId` و کلید Reserve را مشخص کنید و Source مسیر Vibe را، اگر باید در این Feature متصل شود، ارائه دهید. تا روشن‌شدن این موارد، اتصال Reserve پیش از Payment یا اتصال callbackهای Vibe بر پایه حدس خواهد بود.

---

## user
Self-Gate decisions:

### 1. Wallet Payment Amount

برای MVP، WalletProgram باید **کل مبلغ نهایی قابل پرداخت** را پوشش دهد.

بنابراین:
```text
Wallet Reserve Amount = CalcAmountForPayAsync.Total
Payment Amount = CalcAmountForPayAsync.Total
Wallet Capture Amount = Reservation Amount

```

در MVP فعلی **Partial Wallet Payment / ترکیب Wallet و Payment Gateway** نداریم.

مبلغ Wallet را دوباره با منطق مستقل محاسبه نکن؛ مبلغ نهایی همان `CalcAmountForPayAsync.Total` است.

اگر در Source فعلی `Total` شامل مبلغی است که طبق Business Rule نباید توسط Wallet پرداخت شود، آن را به‌عنوان Blocker گزارش کن و حدس نزن.

---

### 2. WalletAssignmentId

`WalletAssignmentId` نباید از Client دریافت شود.

Service باید Assignment فعال کاربر فعلی را برای همان `WalletProgram` پیدا کند.

منبع انتخاب:
```text
Current User/Application
        +
WalletProgram
        ↓
Active WalletAssignment

```

طبق Business Rule فعلی فقط یک Assignment فعال برای `(UserId, WalletProgramId)` مجاز است.

اگر بیش از یک Assignment فعال در Source/DB قابل وجود باشد، آن را به‌عنوان مسئله Concurrency/Business گزارش کن؛ Assignment دلخواه انتخاب نکن.

---

### 3. Reserve Idempotency Key

Reserve باید در سطح Order idempotent باشد.

برای یک Order نباید Retry باعث ایجاد Reservation دوم شود.

از Order به‌عنوان منبع Idempotency استفاده کن.

اگر Reserve Service فعلی `Code` یا Idempotency Key مشخصی دارد، از همان Convention استفاده کن و Key را deterministic بر اساس Order بساز؛ برای مثال مفهومی:
```text
WalletReserve:{OrderId}

```

اما اگر Convention دقیق Repository نام متفاوتی دارد، همان Convention را استفاده کن.

Client نباید بتواند با ارسال Key دلخواه، برای یک Order چند Reservation ایجاد کند.

---

### 4. Payment Status

Statusهای قطعی Payment در Source:
```text
Paid = 1095
Unsuccessful = 1125
Cancel = 1098

```

Statusهای زیر نتیجه قطعی نیستند:
```text
SendToPaymentGateway = 1094
WaitingResponseFromPaymentGateway = 1123

```

بنابراین:
```text
Paid
    → Capture

Unsuccessful / Cancel
    → Release

SendToPaymentGateway / WaitingResponseFromPaymentGateway
    → No Settlement

```

برای Statusهای نامشخص Capture یا Release انجام نده.

---

### 5. Vibe Payment

در این Feature، بر اساس Source فعلی، Vibe را حدس نزن و callback جدید فرضی ایجاد نکن.

`VibePaymentController` و `VerifyVibeOrderAsync` در مسیر مورد بررسی پیدا نشده‌اند.

بنابراین:

- فعلاً Vibe callback integration را پیاده نکن.
- مسیر پرداخت واقعی موجود Repository را بررسی کن.
- اگر Payment abstraction عمومی و قابل استفاده برای تمام Providerها وجود دارد، Integration را روی همان abstraction انجام بده.
- اگر Vibe نیازمند مسیر جداگانه‌ای است که Source آن در Repository فعلی موجود نیست، فقط Gap را گزارش کن.

---

### 6. Implementation Scope

با تصمیم‌های بالا، Implementation را برای **Order/Payment Integration عمومی موجود در Repository** ادامه بده.

ترتیب:
```text
SaveShoppingCartToOrder
        ↓
CalcAmountForPayAsync
        ↓
Wallet Reserve(Total)
        ↓
Create Payment(Total)
        ↓
Payment Result
   ├── Paid → Capture
   ├── Unsuccessful → Release
   └── Cancel → Release

```

از Serviceهای Wallet موجود استفاده کن:

- WalletReservationService
- WalletSettlementService
- WalletAssignmentService

منطق مالی را در Payment Controller تکرار نکن.

---

### 7. Important

اگر پس از این تصمیم‌ها هنوز یک قرارداد ضروری در Source وجود ندارد که بدون حدس قابل پیاده‌سازی نیست، فقط همان مورد را به‌عنوان Blocker گزارش کن.

در غیر این صورت Implementation را ادامه بده.

همچنان:

- Entity تغییر نکند.
- DbContext تغییر نکند.
- Mapping تغییر نکند.
- Migration/DDL/Index/Constraint ایجاد نشود.
- CommonBaseData تغییر نکند.
- Code Review کلی انجام نشود.


---

## assistant
تصمیم‌های مبلغ کامل، انتخاب Assignment در Service و Key قطعیِ Order، سه ابهام اصلی را برطرف کردند. اکنون مسیر عمومی ایجاد و تغییر وضعیت Payment را بررسی می‌کنم تا اتصال Wallet در همان نقطهٔ مشترک انجام شود و مسیر قدیمی کارت باشگاه مشتریان تغییر رفتار ناخواسته نداشته باشد.

---

## assistant
## Handoff: Wallet Order & Payment Integration

**Current state:** Discovery is in progress. **No files have been changed in this task.** The user authorized implementation if the remaining Self-Gate checks pass. Keep the strict Database boundary and avoid a broad Wallet review.

### Decisions confirmed by the user

- Wallet covers the full `CalcAmountForPayAsync.Total`; no partial Wallet or gateway split.
- The server selects the current user’s single active Assignment for a specified WalletProgram. The client must not supply `WalletAssignmentId`.
- Reserve uses a deterministic Order-based idempotency key.
- `Paid=1095` triggers Capture; `Unsuccessful=1125` and `Cancel=1098` trigger Release. `SendToPaymentGateway=1094`, `WaitingResponseFromPaymentGateway=1123`, and unknown statuses trigger neither.
- Vibe callback integration is excluded if its Source is unavailable. No hypothetical callback.
- Reuse `WalletReservationService` and `WalletSettlementService`; no Entity, DbContext, Mapping, Migration, DDL, Index, Constraint, or CommonBaseData changes.

### Repository findings

- [OrderController.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:142>) saves the cart and returns a redirect. Its `Wallet` route leads to a **legacy customer club card** payment flow at lines 755 and 803; that flow creates Payment before validating the card at line 824. Do not silently replace it with WalletProgram behavior.
- [ShoppingCartParam.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs:3>) has neither WalletProgramId nor Assignment/Wallet amount inputs.
- [OrderService.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412>) calculates `AmountForPayment.Total`. `CreateNewPaymentAndReturnAsync` at line 552 **calls the calculator again** and creates a Payment for its `Total` at line 570. Preserve its pricing logic.
- [WalletReserveRequestDto.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveRequestDto.cs>) currently requires AssignmentId, Amount, OrderId, and IdempotencyKey. Existing Reserve Service validates ownership, Program/Assignment status, Eligibility, balance, and concurrency.
- [WalletSettlementService.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs>) already implements Capture/Release. Capture requires a Paid PaymentId for the same Order/Application and a key; Release uses ReservationId. Both operate within their own transaction.
- `PaymentService.UpdatePaymentStatusAsync` in [PaymentService.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:54>) appears to be a shared status update abstraction; **its implementation and call sites still need inspection** before choosing a hook.
- `Vibe=3160` is redirected to `VibePayment` at [OrderController.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:790>), but a targeted search found neither `VibePaymentController` nor `VerifyVibeOrderAsync` in this Repository. Report this gap.
- [WalletAssignmentService.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletAssignmentService.cs>) creates active, zero-balance Assignments. Investigate the existing filtered unique active Assignment index and ensure lookup rejects unexpected duplicates instead of selecting one arbitrarily.

### Next steps

1. Inspect `PaymentService.UpdatePaymentStatusAsync` and its actual success/failure callers to determine whether one shared hook can settle Wallet reservations without affecting non-Wallet payments or breaking Payment transaction boundaries.
2. Determine how a WalletProgram is selected in checkout. The user forbids client-supplied AssignmentId but has not explicitly named a source for WalletProgramId. A request field may be reasonable; check current checkout conventions first.
3. Check whether `CalcAmountForPayAsync.Total` is safe for full Wallet funding for the intended payment method. The existing `Wallet` method selects installment sale type and belongs to the legacy customer club flow. Do not assume it can represent WalletProgram.
4. If the shared payment hook, WalletProgram selection, or full-amount contract cannot be established without guessing, stop under the user’s Self-Gate and report the **specific remaining blocker**. Otherwise implement the narrow integration, then run syntax/build and `git diff --check`. Missing `project.assets.json` is a known build limitation; preexisting Enum whitespace must not block the feature.

---

## assistant
ادغام Order/Payment را از نقطه‌ای که Discovery قبلی متوقف شده ادامه می‌دهم: مسیر واقعی ثبت وضعیت Payment، انتخاب WalletProgram در Checkout، و قرارداد مبلغ را بررسی می‌کنم. اگر این مسیرها با تصمیم‌های شما سازگار باشند، تغییرات را فقط در همان محدوده پیاده‌سازی می‌کنم.

---

## assistant
دو نکته از مسیر موجود روشن است: Checkout فعلی یک شاخه قدیمی برای کارت باشگاه مشتریان دارد، و متد ساخت Payment مبلغ را دوباره از `CalcAmountForPayAsync` می‌گیرد. اکنون Hook مشترک وضعیت Payment و قرارداد انتخاب Program را دقیق‌تر بررسی می‌کنم تا جریان جدید با آن شاخه اشتباه گرفته نشود.

---

## assistant
در کد فعلی، `PaymentMethodTypesEnum.Wallet` به جریان قدیمی کارت باشگاه مشتریان وصل است و در محاسبه مبلغ، نوع فروش اقساطی می‌گیرد. این برای ادغام WalletProgram یک تفاوت مهم است. پیش از هر تغییر، بررسی می‌کنم آیا قرارداد جدا و قابل اتکایی برای تشخیص این دو جریان و ثبت نتیجه Payment وجود دارد یا نه.

---

## assistant
برای این بررسی از دستورالعمل‌های `wallet-feature` و `backend-feature` استفاده کردم. Self-Gate به یک قرارداد لازم رسیده است: مسیر Wallet موجود فقط پرداخت کارت باشگاه مشتریان را تأیید می‌کند، و برای پرداخت کامل با WalletProgram منبع مستقلی در Source دیده نمی‌شود که Payment را به نتیجه قطعی `Paid`، `Unsuccessful` یا `Cancel` برساند. طبق شرط شما، اینجا بدون تغییر کد متوقف می‌شوم.

---

## assistant
**Self-Gate متوقف شد؛ هیچ فایلی تغییر نکرد.**

Blocker، **قرارداد نتیجهٔ پرداخت WalletProgram** است. مسیر Wallet موجود به کارت باشگاه مشتریان وصل است: [OrderController.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:755>) کاربر را به آن مسیر می‌فرستد و [PayWithWallet](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:803>) موفقیت را بر اساس اعتبار همان کارت ثبت می‌کند. [PaymentService.cs](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:77>) وضعیت Payment را ثبت می‌کند، اما خودش نتیجهٔ پرداخت WalletProgram را تعیین نمی‌کند.

برای جریان **WalletProgram که کل مبلغ را می‌پردازد**، در Source فعلی مسیر مشخصی پیدا نشد که پس از ایجاد Payment، نتیجهٔ قطعی آن را تعیین کند. انتخاب خودسرانهٔ `Paid` پس از Reserve یا عبور دادن این پرداخت از مسیر کارت باشگاه مشتریان، یک تصمیم Business جدید است. بنابراین هنوز نمی‌توان Capture/Release را به نتیجه‌ای معتبر وصل کرد.

**تصمیم لازم:** در پرداخت کامل با WalletProgram، چه رویداد یا عملیات معتبری Payment را `Paid` می‌کند و چه مسیری آن را `Unsuccessful` یا `Cancel` می‌کند؟ پس از مشخص شدن این قرارداد، Implementation را ادامه می‌دهم.

---

## user

# Files pasted by the user:

## "# Feature: Wallet Order Integration — Pre-Payment ## هدف در این Feature، Wallet…": C:\Users\pc\.codex/attachments/72e519fd-319a-4111-9a23-326a16ef6206/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن Feature جدید را می‌خوانم و آن را با مسیر فعلی ثبت Order، محاسبه مبلغ و Reserve تطبیق می‌دهم. سپس تغییرات مجاز همین مرحله را انجام می‌دهم.

---

## assistant
درخواست جدید پرداخت ترکیبی را برای این مرحله تعریف کرده و Payment Module را خارج از Scope گذاشته است؛ بنابراین قرارداد «کل مبلغ فقط با Wallet» از مرحله قبل، مبنای این Feature نیست. ابتدا شش شرط Self-Gate را روی Serviceهای موجود بررسی می‌کنم و اگر برقرار باشند، نتیجهٔ `OrderTotal / WalletAmount / RemainingPayable / Reservation` را بدون ایجاد Payment پیاده‌سازی می‌کنم.

---

## assistant
Self-Gate از نظر داده‌های لازم عبور می‌کند: مبلغ Order از `CalcAmountForPayAsync.Total`، موجودی از مانده‌های Assignment، و Eligibility از Service موجود قابل محاسبه است؛ `WalletReservationService` هم مبلغ جزئی را می‌پذیرد. یک Service محدود برای آماده‌سازی قبل از Payment اضافه می‌کنم و ورودی عمومی Reserve را به `OrderId` و `WalletProgramId` محدود می‌کنم تا Assignment و کلید Idempotency در سرور تعیین شوند.

---

## assistant
پیاده‌سازی مسیر پیش از Payment انجام شد. پاسخ API اکنون مبلغ Order، سهم Reserve شدهٔ Wallet، مانده قابل پرداخت و Reservation را برمی‌گرداند؛ هیچ Payment یا Capture ایجاد نمی‌کند. Build با `--no-restore` به دلیل نبود `project.assets.json` متوقف شد؛ بررسی نهایی را به فایل‌های همین Feature محدود می‌کنم.

---

## assistant
**Wallet Order Integration — Pre-Payment پیاده‌سازی شد.** مسیر جدید پس از ثبت Order از `POST ECommerceAdminApi/Wallet/Reserve` با ورودی `OrderId` و `WalletProgramId` قابل فراخوانی است. این مرحله Payment ایجاد نمی‌کند.

فایل‌های تغییرکرده: [DTO نتیجه و درخواست](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletOrderPreparationDto.cs>)، [قرارداد Service](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletOrderPreparationService.cs>)، [Service آماده‌سازی](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs>)، [WalletController](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs>) و [ثبت DI](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs>).

- `OrderTotal` از `CalcAmountForPayAsync.Total` گرفته می‌شود. `WalletAmount = min(OrderTotal, max(0, Assigned − Consumed − Reserved − Expired))` و `RemainingPayable = OrderTotal − WalletAmount` است.
- Service، Assignment فعال و یکتای کاربر و Program معتبر را پیدا می‌کند و Eligibility همهٔ اقلام Order را با Service موجود می‌سنجد. اگر سهم Wallet صفر باشد، Reservation نمی‌سازد.
- فقط `WalletAmount` به `WalletReservationService` داده می‌شود. کلید `WalletReserve:{OrderId}` در سرور ساخته می‌شود؛ Client دیگر AssignmentId یا کلید دلخواه به endpoint نمی‌دهد. Retry، Reservation معتبر همان Order را برمی‌گرداند و موارد ناسازگار را رد می‌کند.
- شکست Reserve نتیجهٔ خطا برمی‌گرداند و این Flow هیچ Payment یا Capture ایجاد نمی‌کند. Gateway، Vibe، callback، وضعیت Payment و تصمیم Capture برای ماندهٔ صفر عمداً به Feature بعدی موکول شدند.

**اعتبارسنجی:** بررسی محدود تغییرات انجام شد. Build با خطای `NETSDK1004` به دلیل نبود `project.assets.json` قابل تأیید نبود. اتصال خودکار این endpoint به UI فعلی `SaveShoppingCartToOrder` در این مرحله انجام نشده است؛ فراخوانی آن باید پس از ثبت Order و پیش از ورود به Payment انجام شود.

---

## user

# Files pasted by the user:

## "# Feature: Wallet Refund ## هدف پیاده‌سازی Refund برای اعتبار WalletProgram بر …": C:\Users\pc\.codex/attachments/a374aad2-578c-47d3-a234-73d54146c2c6/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن Feature Refund را می‌خوانم و سپس فقط مسیرهای موجود Capture، Ledger و Refund را با آن تطبیق می‌دهم. اگر قراردادهای لازم در Repository روشن باشند، پیاده‌سازی را در همان محدوده انجام می‌دهم.

---

## assistant
Self-Gate به یک قرارداد مالی مشخص رسیده است: `WalletTransaction` ارتباط با Transaction اصلی را از طریق `ParentId` دارد، اما Enum فعلی Wallet نوع `Refund` ندارد. پیش از توقف، فقط بررسی می‌کنم آیا در داده‌ها یا قراردادهای موجود، نوع رسمی دیگری برای Wallet Refund ثبت شده است یا خیر؛ `Release` را به‌جای Refund فرض نمی‌کنم.

---

## assistant
**Self-Gate Wallet Refund متوقف شد؛ در این Task هیچ فایلی تغییر نکرد.**

قرارداد مفقود، **نوع تراکنش Ledger برای Wallet Refund و مقدار قطعی آن در `CommonBaseData`** است. [WalletTransactionTypeEnum](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:257>) فقط `Grant`، `Capture`، `Release`، `Expired` و `ManualAdjustment` دارد. سند Wallet، `Refund` را یک Ledger Credit مستقل می‌داند و `Release` را عملیات Reservation می‌شمارد؛ بنابراین استفاده از `Release=3189` برای Refund قابل استنتاج نیست.

[WalletTransaction](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:15>) فیلد `ParentId` برای ارتباط با Capture اصلی و `WalletAssignmentId` برای بازگرداندن اعتبار به همان Assignment دارد. مانع فعلی، **شناسهٔ تأییدشدهٔ Transaction Type مخصوص Refund** است. تا تعیین آن، Ledger یا تغییر مانده‌ای پیاده‌سازی نکردم و Build/Test این Feature موضوعیت نداشت.

---

## user
<environment_context>
  <current_date>2026-10-07</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user

# Files pasted by the user:

## "# Master Prompt — Implement Wallet Refund ## Feature **Wallet Refund** ## Scope…": C:\Users\pc\.codex/attachments/549b8c94-dfb9-4933-8b16-42ddfbceac41/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن تصمیم‌های جدید Wallet Refund را می‌خوانم و Self-Gate قبلی را با آن‌ها تطبیق می‌دهم. بعد، در صورت رفع مانع Transaction Type، پیاده‌سازی Wallet-side Refund را در محدودهٔ خواسته‌شده ادامه می‌دهم.

---

## assistant
مانع قبلی رفع شده است: Enum فعلی `Refund = 3189` دارد و `Release` دیگر Transaction Type نیست. Capture، Assignment و Order از Ledger موجود قابل resolve هستند؛ `ParentId` هم ارتباط Refund با Capture را فراهم می‌کند. منطق را با الگوی `Serializable` و به‌روزرسانی شرطی مانده‌ها پیاده‌سازی می‌کنم تا Refund هم‌زمان از مبلغ Capture عبور نکند.

---

## assistant
Service و API Refund اضافه شدند. مسیر جدید، Capture و Reservation ثبت‌شدهٔ آن را بررسی می‌کند، مجموع Refundهای فرزند Capture را می‌خواند و کاهش `ConsumedAmount` در Assignment و Program را همراه Ledger مثبت در یک Transaction انجام می‌دهد. اکنون سناریوهای تکرار، Refund جزئی/کامل و تعارض هم‌زمان را روی همین تغییرات بررسی و Build را امتحان می‌کنم.

---

## assistant
**Wallet Refund در سمت Wallet پیاده‌سازی شد.** API جدید `POST /api/WalletRefund` فقط `CaptureTransactionId`، مبلغ و `Code` را می‌گیرد؛ Assignment از Capture اصلی تعیین می‌شود.

فایل‌ها: [DTO](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletRefundDto.cs>)، [قرارداد Service](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletRefundService.cs>)، [WalletRefundService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs>)، [Controller](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletRefundController.cs>) و ثبت DI در [ModuleInitializer](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs>).

Service، Capture و ارتباط آن با Order، Reservation و Assignment را اعتبارسنجی می‌کند. Refund جزئی و کامل را تا سقف `CapturedAmount − TotalRefunded` می‌پذیرد و Ledger جدید با `Refund=3189`، مبلغ مثبت و `ParentId` برابر شناسهٔ Capture می‌سازد. در همان Transaction، `ConsumedAmount` همان Assignment و Program کاهش می‌یابد؛ `AllocatedAmount` و Reservation تغییر نمی‌کنند.

تکرار `Code` با اطلاعات یکسان نتیجهٔ قبلی را برمی‌گرداند و اطلاعات متفاوت Conflict می‌دهد. برای درخواست‌های هم‌زمان از Transaction با isolation سطح `Serializable`، به‌روزرسانی شرطی مانده‌ها و retry تعارض استفاده شده است.

**اعتبارسنجی:** مسیرهای Full، Partial، Over Refund، Replay، Conflict و Capture نامعتبر با بررسی منطق تغییرات پوشش داده شدند؛ اجرای runtime آن‌ها تأیید نشده است. Build با `NETSDK1004` به دلیل نبود `project.assets.json` متوقف شد. اتصال به Payment، Vibe و Gateway طبق Scope انجام نشد.

---

## user

# Files pasted by the user:

## "# Master Prompt — Implement / Complete Wallet Expiration ## Feature **Wallet Ex…": C:\Users\pc\.codex/attachments/09a36860-1272-42c1-9df5-482fccedef7c/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن Feature Expiration را می‌خوانم و آن را با Job و Serviceهای موجود Wallet تطبیق می‌دهم. سپس فقط بخش‌های ناقص همین Feature را، در صورت روشن بودن قراردادها، تکمیل می‌کنم.

---

## assistant
**Self-Gate Wallet Expiration متوقف شد؛ هیچ فایلی تغییر نکرد.**

قرارداد مفقود، رفتار Reservation با وضعیت `Reserved` هنگام انقضای Assignment یا Program است. سند Wallet وضعیت‌ها و `ExpiresAt` را مشخص می‌کند، اما نمی‌گوید این Reservation باید `Released` شود یا `Expired`، و مبلغ رزروشده پس از آن چگونه از اعتبار خارج شود. [WalletSettlementService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:166>) در حال حاضر Release را انجام می‌دهد؛ [ExpireWalletProgramsJob](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/JobImpl/ExpireWalletProgramsJob.cs:16>) فقط انقضای وضعیت Program را اجرا می‌کند.

این تصمیم برای ثبت صحیح `ExpiredAmount` و Ledger ضروری است: مبلغ Reserved تا تعیین تکلیف، جزو Available نیست. طبق شرط صریح Self-Gate، منطق مالی Expiration را حدس نزدم و پیاده‌سازی یا Build جدیدی انجام ندادم.

---

## user
<environment_context>
  <current_date>2026-10-08</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user

# Files pasted by the user:

## "# Feature: Wallet Expiration — انقضای اعتبار کیف پول ## هدف قابلیت Expiration ر…": C:\Users\pc\.codex/attachments/29582c84-35a2-41b5-b6b3-cd647141a0ff/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
تصمیم‌های جدید Expiration را می‌خوانم و با مانع قبلی مربوط به Reservationهای `Reserved` تطبیق می‌دهم. اگر این قرارداد اکنون مشخص شده باشد، بخش مالی Expiration را با Job موجود تکمیل می‌کنم.

---

## assistant
قاعدهٔ Reservation اکنون روشن است: هنگام انقضای اعتبار، Reservation باز به `Expired` می‌رود و مبلغ آن نباید دوباره قابل استفاده شود. مدل فعلی `ExpiredAmount` را در Assignment و Program دارد؛ بنابراین می‌توان مبلغ رزروشده و باقی‌ماندهٔ آزاد را یک‌بار در انقضای مالی همان Assignment حساب کرد. پیش از تغییر کد، مرز اجرای Job سیستمی و ثبت اطلاعات سازندهٔ Ledger را بررسی می‌کنم.

---

## assistant
Discovery نشان داد Job دقیقه‌ای از قبل ثبت شده و فعلاً فقط وضعیت Program را تغییر می‌دهد؛ Ledger یا مانده‌ها را منقضی نمی‌کند. آن را تکرار نمی‌کنم. منطق مالی را به همان Service پشت Job اضافه می‌کنم: برای هر Assignment سررسیدشده، Reservationهای باز، ماندهٔ آن Assignment و سهم Program در یک Transaction تعیین تکلیف می‌شوند.

---

## assistant
تکمیل Job انجام شد. در انقضای هر Assignment، `ReservedAmount` همراه اعتبار آزاد در یک مبلغ Expiration حساب می‌شود؛ Reservationهای `Reserved` به `Expired` می‌روند و یک Ledger منفی برای همان Assignment ثبت می‌شود. `ConsumedAmount` و `AllocatedAmount` تغییر نمی‌کنند. اکنون صحت نحوی تغییرات، سناریوهای مانده و امکان Build را بررسی می‌کنم.

---

## assistant
بررسی نحوی فایل‌های تغییرکرده بدون خطا بود و سناریوهای عددی Available، اجرای مجدد، مصرف کامل و Refund پس از انقضا با فرمول فعلی صفر شدن Available را نشان دادند. Build همچنان پیش از کامپایل به `NETSDK1004` و نبود `project.assets.json` می‌رسد؛ بنابراین رفتار Runtime و Transaction روی SQL Server هنوز تأیید نشده است.

---

## assistant
**Wallet Expiration تکمیل شد.** Job دقیقه‌ای [ExpireWalletProgramsJob](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/JobImpl/ExpireWalletProgramsJob.cs>) از قبل وجود داشت و فقط وضعیت Program را منقضی می‌کرد. اکنون [WalletProgramService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletProgramService.cs:173>) پیش از آن، Assignmentهای سررسیدشده را مستقل پردازش می‌کند. توضیح قرارداد در [IWalletProgramService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletProgramService.cs>) و توضیح زمان‌بندی در [ModuleInitializerPartial](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializerPartial.cs>) نیز به‌روز شد؛ Job یا زمان‌بندی جدیدی ایجاد نشد.

برای هر Assignment، مبلغ انقضا برابر `Assigned − Consumed − Expired` است؛ این مبلغ **اعتبار آزاد و Reserved مصرف‌نشده** را یک‌بار شامل می‌شود. در یک Transaction، Reservationهای باز به `Expired=3186` می‌روند، `ReservedAmount` کاهش می‌یابد، `ExpiredAmount` در Assignment و Program افزایش می‌یابد و فقط اگر مبلغ مثبت باشد یک Ledger منفی با نوع `Expired=3190` ثبت می‌شود. `ConsumedAmount` و `AllocatedAmount` تغییر نمی‌کنند. اجرای مجدد از ماندهٔ به‌روزشده عبور می‌کند؛ Transaction با سطح `Serializable` و کنترل مانده‌ها از ثبت مضاعف هنگام هم‌زمانی محافظت می‌کند.

**اعتبارسنجی:** بررسی نحوی فایل‌های تغییرکرده بدون خطا بود؛ سناریوهای عددی اعتبار آزاد و رزروشده، اجرای مجدد، مصرف کامل و Refund پس از انقضا بررسی شدند. Build با `NETSDK1004` به دلیل نبود `project.assets.json` متوقف شد، پس اجرای Runtime و رفتار SQL Server هنوز تأیید نشده است. انقضای صرفِ مهلت ۳۰ دقیقه‌ای `ExpiresAt`، بدون سررسید اعتبار Assignment یا Program، در این تغییر تعیین تکلیف نشده است.

---

## user

# Files pasted by the user:

## "# Feature: Wallet Audit / Reconciliation — ممیزی و تطبیق مالی کیف پول ## هدف قا…": C:\Users\pc\.codex/attachments/2a5d6634-ac91-4d03-a074-4bbd6741bdd5/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن Feature ممیزی و تطبیق مالی را می‌خوانم و فقط قراردادهای موجود Ledger، مانده‌ها و Reservation را با آن تطبیق می‌دهم. سپس در صورت مشخص بودن قواعد گزارش‌گیری و دسترسی، پیاده‌سازی را در همان محدوده انجام می‌دهم.

---

## assistant
قرارداد مالی برای گزارش‌گیری روشن است: Grant مقدار Assigned و Allocated را بالا می‌برد؛ Capture و Refund مصرف خالص را تغییر می‌دهند؛ Reservation با وضعیت `Reserved` ماندهٔ رزروشده را می‌سازد؛ و Expiration در Ledger منفی و `ExpiredAmount` ثبت می‌شود. گزارش را به‌صورت یک API مدیریتیِ خواندنی برای یک Program در Application جاری می‌سازم؛ Job یا اصلاح خودکار اضافه نمی‌کنم.

---

## assistant
گزارش خواندنی Program و endpoint مدیریتی اضافه شدند. مغایرت‌های ماندهٔ Assignment و Program، اتصال Capture/Refund، Reservation تعیین‌تکلیف‌نشده، کلید Ledger تکراری و نوع تراکنش با اثر مالی نامشخص بررسی می‌شوند. یک کنترل نسخهٔ Program در انتهای خواندن گذاشتم تا گزارشِ هم‌زمان با تغییر مالی به‌اشتباه قطعی اعلام نشود.

---

## assistant
## Handoff — Wallet Audit / Reconciliation

**Current state:** Implementation is written, but final validation and report remain. No comprehensive Wallet review was performed.

- Added `WalletAuditDto`, `IWalletAuditService`, `WalletAuditService` and `WalletAuditController`; registered the service in `ModuleInitializer`.
- API: `GET /api/WalletAudit/{walletProgramId}`, protected by the existing `GetEntity` permission for `WalletProgram`.
- Audit is read-only and scoped to the current Application and requested Program. It compares Assignment and Program balances with Grant, Capture, Refund, Expired Ledger entries and `Reserved` Reservations. It also reports invalid links, duplicate Ledger codes, impossible balances and overdue open Reservations.
- No Job, automatic correction, database change or new permission was added.

**Important constraints:** Existing user changes are extensive and uncommitted; preserve them. The user requested no comprehensive code review. Wallet financial flows and the expiration implementation from previous turns are the source for reconciliation formulas. `ManualAdjustment` has no established balance contract, so the service reports that type and skips Ledger-derived comparisons where it appears.

**Validation so far:** Roslyn syntax parsing reports zero errors in all four new files. Targeted whitespace checks passed. `dotnet build --no-restore` stops with `NETSDK1004` because `project.assets.json` is absent; semantic compilation and runtime behavior remain unverified.

**Next steps:** Perform a focused self-check of `WalletAuditService`, especially financial formulas, false positives, EF query translation, Application isolation and the Program `RowVersion` consistency check. Run feasible scenario checks for healthy data, mismatched balances, partial/full Refund, duplicate codes and unresolved Reservations. Fix concrete issues, then give the user the final feature report with files, detected issue types, validation results and limitations.

---

## assistant
پیاده‌سازی Audit از مرحله قبل موجود است. ابتدا متن درخواست و قراردادهای مرتبط را با تغییرات فعلی تطبیق می‌دهم، سپس فقط Self-check و اعتبارسنجی محدود همین Feature را انجام می‌دهم.

---

## assistant
متن Feature تأیید می‌کند Audit باید فقط گزارش بدهد و هیچ مانده یا Ledger را اصلاح نکند. Skillهای Wallet و Backend را از مسیر اصلی Repository می‌خوانم و بعد محاسبات و محدوده داده‌های گزارش را در فایل‌های جدید بررسی می‌کنم.

---

## assistant
محاسبات Grant، Capture، Refund و Expiration با سرویس‌های فعلی تطبیق دارند. دو نقطه را دقیق‌تر بررسی می‌کنم: پیوند Refund با Capture و رفتار گزارش هنگام تغییر هم‌زمان داده‌ها؛ سپس اعتبارسنجی محدود را تمام می‌کنم.

---

## assistant
قابلیت **Wallet Audit / Reconciliation** پیاده‌سازی شد. API مدیریتی `GET /api/WalletAudit/{walletProgramId}` مغایرت‌ها را برای Program در Application جاری گزارش می‌کند و هیچ مانده، Reservation یا Ledger را تغییر نمی‌دهد.

فایل‌های Feature: [WalletAuditService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletAuditService.cs)، [WalletAuditDto.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletAuditDto.cs)، [IWalletAuditService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletAuditService.cs)، [WalletAuditController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletAuditController.cs) و ثبت سرویس در [ModuleInitializer.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs).

گزارش، مغایرت مانده‌های Assignment و Program با Grant، Capture، Refund، Expiration و Reservationهای باز را تشخیص می‌دهد؛ همچنین پیوند نامعتبر تراکنش‌ها، Code تکراری، مانده ناممکن و Reservation بازِ سررسیدگذشته را نشان می‌دهد. در Self-check، تطبیق پیوند Refund با Capture کامل‌تر شد. اثر مالی `ManualAdjustment` در قرارداد موجود روشن نیست؛ در گزارش مشخص می‌شود و از مقایسه‌های مشتق از Ledger کنار گذاشته می‌شود.

بررسی Syntax فایل‌های جدید و بررسی محدود تغییرات انجام شد. **Build و اجرای سناریوهای Runtime تأیید نشدند**: دستور Build به‌دلیل نبود `project.assets.json` با `NETSDK1004` متوقف شد. Review جامع Wallet طبق تصمیم قبلی به پایان Roadmap موکول است.

---

## user
<environment_context>
  <current_date>2026-10-09</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user
## Feature: Wallet Entity Architecture Alignment

هدف: هماهنگ‌سازی کد موجود کیف پول با ساختار جدید Entityهای `WalletReservation` و `WalletTransaction` که در پروژه به‌روزرسانی شده‌اند.

### محدوده کار

1. ابتدا Entityها، Mappingها، سرویس‌ها، DTOها، Controllerها و تمام محل‌های استفاده از `WalletReservation` و `WalletTransaction` را بررسی کن.
2. ساختار فعلی `Payment` و ارتباط آن با `Order` را فقط برای شناخت معماری موجود بررسی کن.
3. تمام وابستگی‌های قدیمی به `OrderId` و `OrderItemId` در `WalletReservation` و `WalletTransaction` را شناسایی کن و با ساختار واقعی Entityهای جدید هماهنگ کن.
4. ارتباط‌های موردنیاز از طریق `PaymentId`، `WalletAssignmentId` و `ReservationId` را مطابق Entityها و Mappingهای موجود حفظ کن.
5. سرویس‌های Reservation، Capture، Release، Refund، Expiration و Audit را با ساختار جدید سازگار کن. در صورت نیاز به اطلاعات سفارش، از مسیر ارتباطی موجود `Payment → Order` استفاده کن.
6. منطق فعلی Idempotency، کنترل موجودی، تراکنش‌های دیتابیس، هم‌زمانی و ثبت Ledger را حفظ کن. تغییرات نباید باعث ثبت تکراری تراکنش مالی یا مصرف دوباره اعتبار شوند.
7. در `WalletTransaction` فرض نکن که همه تراکنش‌ها الزاماً `PaymentId` دارند؛ به‌خصوص Grant را مطابق قرارداد واقعی فعلی پیاده‌سازی کن.
8. هیچ تغییر جدیدی در Schema دیتابیس، Entityها، Migrationها، وضعیت‌های پرداخت، APIهای درگاه یا فرآیند Checkout ایجاد نکن. تغییرات ساختاری Entityها از قبل انجام شده‌اند؛ آن‌ها را بررسی کن و کد را با ساختار واقعی موجود تطبیق بده.

### محدودیت مهم

در این مرحله، **جریان ایجاد Payment، انتخاب درگاه، Redirect، Callback، Verify و تغییر وضعیت پرداخت را تغییر نده**؛ زیرا مستند نهایی یکپارچه‌سازی درگاه‌ها هنوز تکمیل نشده است.

اگر سازگاری کامل با Entityهای جدید بدون تغییر درگاه‌ها ممکن نیست، تغییرات مستقل و امن را انجام بده و موارد وابسته به یکپارچه‌سازی درگاه را گزارش کن؛ برای تصمیم‌های مسدودکننده حدس نزن.

### گزارش نهایی

- فایل‌های تغییرکرده و دلیل تغییر هرکدام
- وابستگی‌های قدیمی حذف یا جایگزین‌شده
- مواردی که به دلیل ناقص‌بودن مستند درگاه‌ها انجام نشدند
- خطاهای Build و علت آن‌ها
- وضعیت تست‌ها و محدودیت‌های اجرای آن‌ها

فعلاً Code Review جامع کل ماژول را انجام نده؛ این بررسی پس از تکمیل قابلیت‌های باقی‌مانده انجام خواهد شد.


---

## assistant
ساختار جدید Entityها و محل‌های استفاده از فیلدهای حذف‌شده را بررسی می‌کنم، سپس فقط سرویس‌های Wallet را با قرارداد موجود هماهنگ می‌کنم. مسیر ایجاد و پردازش Payment و درگاه‌ها را دست نمی‌زنم؛ هر وابستگی حل‌نشده به آن مسیر را در گزارش می‌آورم.

---

## assistant
Entity جدید برای Reservation و Transaction به‌جای فیلدهای Order، `PaymentId` دارد و ارتباط سفارش از `Payment → Order` برقرار است. `PaymentId` در Reservation اجباری است؛ بنابراین مسیر قبلیِ Reserve پیش از ایجاد Payment با مدل جدید سازگار نیست. بخش‌های مستقل مانند Settlement، Refund و Audit را تطبیق می‌دهم و مرز این جریان را صریح نگه می‌دارم.

---

## assistant
یک مانع مستقل هم روشن شد: `WalletTransaction.PaymentId` در Entity از نوع `int` اجباری است و Mapping برای آن FK به Payment دارد، در حالی که Grant و Expiration طبق قرارداد فعلی بدون Payment ثبت می‌شوند. برای این دو جریان Payment ساختگی نمی‌سازم. کدهای وابسته به Order را در مسیرهای قابل‌تطبیق از Payment می‌خوانم و این ناسازگاری مالی را جداگانه گزارش می‌کنم.

---

## assistant
ارجاع‌های مستقیم به `OrderId` و `OrderItemId` از کدهای Wallet که روی Reservation یا Transaction تکیه داشتند برداشته شده‌اند. Capture و Refund اکنون `PaymentId` را با Reservation و Payment واقعی تطبیق می‌دهند. Build همچنان پیش از کامپایل کد، به‌دلیل نبود `project.assets.json` متوقف می‌شود؛ در حال انجام Self-check نحوی و بررسی آخرین وابستگی‌ها هستم.

---

## assistant
کدهای Wallet که به `OrderId` و `OrderItemId` حذف‌شده از `WalletReservation` و `WalletTransaction` وابسته بودند، با ارتباط موجود `Payment → Order` هماهنگ شدند. **هماهنگی کامل هنوز ممکن نیست**؛ دو ناسازگاری قرارداد داده باقی مانده است.

| فایل‌های تغییرکرده | دلیل |
| --- | --- |
| [WalletReserveRequestDto.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveRequestDto.cs)، [WalletReservationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs)، [WalletReservationRepository.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs) | دریافت و اعتبارسنجی Payment موجود، ثبت `PaymentId`، یافتن رزرو فعال از مسیر Payment و حفظ کلید تکرار ثابت مبتنی بر Order. |
| [WalletOrderPreparationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs) | جایگزینی خواندن `Reservation.OrderId` با `Reservation.Payment.OrderId`. |
| [WalletSettlementService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs)، [WalletRefundService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs) | تطبیق Capture و Refund با `PaymentId`، Reservation و Order مرتبط، بدون تغییر منطق مانده یا تراکنش. |
| [WalletAuditService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletAuditService.cs) | تطبیق پیوندهای Ledger و Reservation بر اساس `PaymentId` به‌جای فیلدهای حذف‌شده. |

**موارد باز:** مسیر آماده‌سازی فعلی پیش از ساخت Payment درخواست Reserve می‌دهد، اما Entity جدید برای Reservation یک `PaymentId` اجباری دارد؛ این درخواست اکنون با خطای روشن رد می‌شود. همچنین `WalletTransaction.PaymentId` در Entity اجباری و در Mapping دارای FK است، در حالی که Grant و Expiration طبق قرارداد فعلی Payment ندارند. برای آن‌ها Payment ساختگی ثبت نشد. رفع این دو ناسازگاری به تصمیم نهایی درباره ترتیب Payment/Reserve و قرارداد PaymentId تراکنش‌های مستقل از Payment نیاز دارد. جریان درگاه، Callback، Verify، وضعیت Payment، Entity و Schema تغییر نکرد.

**اعتبارسنجی:** بررسی نحوی هفت فایل تغییرکرده صفر خطا داشت و بررسی محدود whitespace گذشت. Build با `NETSDK1004` به‌دلیل نبود `project.assets.json` پیش از کامپایل متوقف شد؛ بنابراین تست اجرایی و صحت Runtime تأیید نشده‌اند.

---

## user
<environment_context>
  <current_date>2026-10-10</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user
## Task: Resolve Wallet Payment Contract Decisions

هدف: بررسی دو ناسازگاری گزارش‌شده بین Entityهای جدید کیف پول و قرارداد مالی سیستم، بدون تغییر کد یا دیتابیس.

### بررسی اول: ترتیب Payment و Reservation

- قرارداد فعلی ایجاد Payment، وضعیت‌های آن و امکان نامعتبر کردن یا خاتمه‌دادن به تلاش پرداخت ناموفق را بررسی کن.
- مشخص کن اگر Payment ساخته شود ولی WalletReservation شکست بخورد، سیستم با استفاده از قابلیت‌های موجود چگونه باید از ادامه آن تلاش جلوگیری کند.
- تفاوت خطای قطعی Reserve، خطای موقت و Retry را بررسی کن.
- هیچ وضعیت یا رفتار جدیدی برای Payment اختراع نکن.

### بررسی دوم: PaymentId در WalletTransaction

- Entity، Mapping، Schema واقعی و تمام محل‌های ایجاد WalletTransaction را بررسی کن.
- برای Grant، Capture، Refund، Expiration و ManualAdjustment مشخص کن آیا واقعاً Payment مرتبط وجود دارد یا خیر.
- گزینه‌های حفظ PaymentId اجباری در برابر اختیاری‌بودن آن را از نظر یکپارچگی داده، FK، گزارش‌گیری و قابلیت Audit مقایسه کن.
- مشخص کن آیا مدل فعلی می‌تواند تراکنش‌های مستقل از Payment را بدون Payment ساختگی ثبت کند.

### محدودیت‌ها

- هیچ فایل، Entity، Mapping، Schema، Migration، وضعیت پرداخت یا API را تغییر نده.
- نتیجه را با ارجاع به فایل‌ها و قراردادهای واقعی پروژه گزارش کن.
- اگر برای تصمیم نهایی به مستند درگاه‌ها نیاز است، وابستگی را صریحاً مشخص کن.

### خروجی

1. وضعیت فعلی هر دو قرارداد.
2. گزینه‌های معماری و مزایا/معایب آن‌ها.
3. پیشنهاد نهایی با دلیل.
4. تصمیم‌هایی که باید در مستند نهایی Wallet–Payment ثبت شوند.


---

## assistant
قراردادهای فعلی Payment و WalletTransaction را از کد و Mapping بررسی می‌کنم و مسیرهای ایجاد و تغییر وضعیت Payment را فقط در حد این دو تصمیم دنبال می‌کنم. هیچ فایل یا داده‌ای تغییر نخواهد کرد.

---

## assistant
در کد موجود، `CreateNewPaymentAndReturnAsync` ابتدا Payment را با وضعیت `SendToPaymentGateway` ثبت می‌کند و بعد شناسه آن در اختیار مسیر پرداخت قرار می‌گیرد. سرویس تغییر وضعیت، `Unsuccessful` و `Cancel` را پشتیبانی می‌کند، اما هنوز بررسی می‌کنم آیا صرف تغییر وضعیت واقعاً مانع ادامه تمام مسیرهای درگاه می‌شود یا فقط رکورد Payment را خاتمه می‌دهد.

---

## assistant
تعارض مستند و مدل تأیید شد: راهنمای Wallet ترتیب `Reserve → Payment` را ثبت کرده، اما Entity جدید Reservation به `PaymentId` اجباری و FK به Payment نیاز دارد. برای نتیجه نهایی، تفاوت «تغییر وضعیت Payment» و «جلوگیری واقعی از ارسال به درگاه» را جدا می‌کنم؛ کد فعلی تضمین سراسری برای دومی نشان نمی‌دهد.

---

## assistant
## 1. وضعیت فعلی قراردادها

**Payment و Reservation:** [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552) ابتدا Payment را با وضعیت `SendToPaymentGateway` ثبت و commit می‌کند. [WalletReservationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33) برای Reserve به Payment موجود نیاز دارد؛ [WalletReservation.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs:71) نیز `PaymentId` اجباری دارد. این ترتیب با جریان `Reserve → Payment` در [wallet.md](P:/Projects/walletProject/Wallet/docs/codex/wallet.md:21) و مسیر فعلی آماده‌سازی پیش از Payment تعارض دارد.

برای **شکست قطعی Reserve**، کد موجود امکان ثبت `Unsuccessful` از طریق [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:77) را نشان می‌دهد؛ نمونه‌های آن در [OrderController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:838) وجود دارند. با این حال، *تغییر وضعیت به‌تنهایی تضمین جلوگیری از ارسال یا ادامه درگاه نیست*. ارسال/Redirect باید تا موفقیت Reserve انجام نشود. برای خطای موقت یا نتیجه نامعلوم، نباید شکست قطعی فرض شود: Retry باید با همان Payment و کلید ثابت Order انجام شود و ابتدا نتیجه احتمالی Reserve قبلی را بازیابی کند. کد فعلی کلید `WalletReserve:{OrderId}` و Replay را پشتیبانی می‌کند، ولی PaymentId را نیز در تطبیق Replay دخیل می‌داند. `Cancel` در Enum معنای انصراف کاربر دارد؛ برای خطای Reserve انتخاب مستندی نیست. `Void` در Enum هست، اما رفتار قابل استفاده‌ای برای این حالت در مسیر بررسی‌شده پیدا نشد.

**WalletTransaction:** [WalletTransaction.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:70) `PaymentId` غیر nullable دارد و [EF Mapping](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28449) برای آن FK به Payment تعریف می‌کند.

| نوع تراکنش | Payment در مسیر ایجاد فعلی |
| --- | --- |
| Grant | ندارد؛ [WalletGrantService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletGrantService.cs:158) بدون PaymentId درج می‌کند. |
| Capture | دارد؛ [WalletSettlementService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:227) PaymentId رزرو را ثبت می‌کند. |
| Refund | دارد؛ [WalletRefundService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs:187) PaymentId مربوط به Capture را ثبت می‌کند. |
| Expiration | ندارد؛ [WalletProgramService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletProgramService.cs:333) بدون PaymentId درج می‌کند. |
| ManualAdjustment | فقط در [Enum](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:257) تعریف شده؛ مسیر ایجاد یا قرارداد ارتباط با Payment پیدا نشد. |

**Schema زنده قابل تأیید نبود.** فایل DDL یا Migration مرتبط در Repository پیدا نشد؛ نتیجه درباره اجباری‌بودن ستون و FK بر پایه Entity و EF Mapping است. در مدل فعلی، ثبت امن Grant و Expiration بدون Payment واقعی ممکن نیست؛ مقدار پیش‌فرض `0` جایگزین معتبر یک Payment مرتبط نیست.

## 2. گزینه‌های معماری

| گزینه | مزیت | هزینه و محدودیت |
| --- | --- | --- |
| PaymentId اجباری برای تمام Ledger | هر سطر از راه FK به Payment وصل می‌شود؛ گزارش‌های Payment محور ساده‌ترند. | Grant و Expiration، Payment واقعی ندارند؛ به ارتباط ساختگی یا تغییر مدل مالی نیاز پیدا می‌کنند. |
| PaymentId اختیاری در WalletTransaction، همراه با FK برای مقدار غیر null | Grant و Expiration مستقل ثبت می‌شوند؛ Capture و Refund همچنان به Payment واقعی قابل ردیابی‌اند. | گزارش و Audit باید بر اساس نوع تراکنش، وجود Payment را اعتبارسنجی کنند؛ تغییر Entity/Schema لازم است. |

برای ترتیب عملیات نیز **Payment سپس Reserve و سپس ارسال به درگاه** با Reservation فعلی سازگارتر است. حفظ ترتیب مستندِ **Reserve سپس Payment** مستلزم قرارداد دیگری برای PaymentId رزرو و نحوه اتصال بعدی آن است.

## 3. پیشنهاد نهایی

`PaymentId` در **WalletTransaction اختیاری** باشد، در حالی که سرویس‌های Capture و Refund وجود و تطابق آن را الزامی کنترل کنند. برای Grant و Expiration، Payment ساختگی ایجاد نشود. درباره ManualAdjustment تا تعریف قرارداد آن تصمیمی گرفته نشود.

برای Reservation، مسیر **ایجاد Payment → Reserve → ارسال/Redirect فقط پس از موفقیت Reserve** را به‌عنوان گزینه پیشنهادی در مستند نهایی بررسی کنید؛ این گزینه با FK فعلی سازگار است. اجرای آن هنوز به قرارداد نهایی درگاه‌ها وابسته است: کد بررسی‌شده یک gate سراسری که صرفاً با تغییر Status مانع ادامه همه مسیرهای پرداخت شود، نشان نمی‌دهد.

## 4. تصمیم‌های لازم برای مستند Wallet–Payment

1. ترتیب رسمی Payment و Reserve و جایگزینی جریان متعارض در `wallet.md`.
2. رفتار Payment ساخته‌شده پس از شکست قطعی Reserve، و محل تضمین «عدم ارسال به درگاه»؛ همچنین رفتار Retry با **همان PaymentId** در نتیجه نامعلوم.
3. وضعیت مجاز برای شکست محلی Reserve و قواعد انتقال وضعیت، با توجه به Callbackهای احتمالی هر درگاه.
4. nullable بودن PaymentId در Ledger و الزام آن برای Capture/Refund؛ قرارداد مستقل Grant/Expiration و وضعیت هنوز نامشخص ManualAdjustment.
5. نحوه تطبیق مبلغ Payment و Reserve پیش از ارسال.

هیچ فایل یا دیتابیسی تغییر نکرد.

---

## user
<environment_context>
  <current_date>2026-10-12</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user

# Files pasted by the user:

## "# Feature: Wallet–Commerce Payment Integration ## هدف یکپارچه‌سازی کامل کیف پول…": C:\Users\pc\.codex/attachments/7a25cfea-b3f8-4f04-a33f-c8c66b149c4d/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن کامل Feature و وضعیت فعلی Repository را می‌خوانم، سپس قراردادهای Payment، Reserve و Settlement را با همان محدوده تطبیق می‌دهم. بعد از روشن‌شدن ابهام‌های واقعی، تغییرات لازم را در مسیر موجود پروژه اعمال می‌کنم.

---

## assistant
Self-Gate یک مانع قطعی برای پرداخت کامل پیدا کرد: `WalletTransaction.PaymentId` اکنون nullable است، اما `WalletReservation.PaymentId` هنوز `int` اجباری است و Reservation رابطهٔ مستقیمی با Order ندارد. بنابراین Full Wallet بدون Payment صفر، با Entityهای فعلی قابل ثبت و بازیابی مطمئن نیست. مسیر پرداخت ترکیبی و نقاط اتصال درگاه‌ها را بررسی می‌کنم تا بخش مستقل و ایمن آن مشخص شود.

---

## assistant
## نتیجه Self-Gate

**یکپارچه‌سازی کامل فعلاً مسدود است و در این مرحله کدی تغییر نکرد.** طبق درخواست Feature، برای مسیرهایی که قرارداد قابل اتکا ندارند رفتار تازه‌ای فرض نکردم.

- **Full Wallet:** برخلاف متن Feature، [WalletReservation.PaymentId](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs:71) هنوز `int` اجباری است و [Mapping](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28208) آن را به Payment وصل می‌کند. Reservation رابطهٔ مستقیمی با Order ندارد. بنابراین Reserve بدون Payment صفر و بازیابی آن از مسیر معتبر Order با مدل فعلی قابل پیاده‌سازی نیست. [WalletSettlementService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:34) نیز برای Capture، PaymentId و Payment با وضعیت `Paid` می‌خواهد؛ مسیر تأییدشده‌ای برای Capture و نهایی‌سازی Order بدون Payment پیدا نشد.
- **Wallet + Gateway:** [CreateNewPaymentAndReturnAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552) مبلغ کامل را دوباره محاسبه و همان را در Payment ثبت می‌کند؛ قرارداد موجودی برای ثبت فقط `RemainingPayable` ندارد. [Checkout](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:742) مستقیماً به مسیرهای جداگانهٔ درگاه Redirect می‌کند و [ShoppingCartParam](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs) شناسهٔ WalletProgram ندارد. در Source فعلی فقط ارجاع URL به Vibe دیده شد و مسیر Verify آن در این Repository موجود نیست. در نتیجه نقطهٔ مشترک و قابل اتکایی برای تضمین Reserve پیش از ارسال، سپس Capture/Release پس از Verify همهٔ درگاه‌ها مشخص نیست.
- **Retry تلاش‌های متعدد:** کلید فعلی Reserve در [WalletReservationService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:61) بر اساس Order است؛ تلاش دوم با Payment متفاوت را به‌عنوان تعارض می‌بیند. تغییر این قرارداد باید همراه با مسیر ایجاد و بازیابی هر تلاش انجام شود تا رزرو تکراری یا مصرف دوباره رخ ندهد.

[WalletTransaction.PaymentId](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:70) اکنون nullable است؛ مانع قبلی Grant و Expiration از این جهت در **Entity** برطرف شده است. Schema زنده در این بررسی قابل تأیید نبود.

برای ادامه، حداقل قرارداد لازم این است: مدل معتبر اتصال **Reservation بدون Payment به Order**، مسیر مجاز **Capture و تکمیل Order بدون Payment**، و نقطهٔ اتصال هر درگاه برای **ثبت Payment با RemainingPayable، جلوگیری از ارسال پیش از Reserve، و Settlement پس از Verify** مشخص شود. سؤال مربوط به مدل Full Wallet نیز برای شما ارسال شده است.

Build یا تست اجرا نشد، چون تغییری اعمال نشد.

---

## user
یک بار دیگه وضعیت entity و کار های مربوط به walletReservation رو بررسی کن اصلاح کردم و کارشو انجام بده


---

## assistant
Entity و Mapping به‌روز‌شدهٔ `WalletReservation` را دوباره بررسی می‌کنم، سپس مسیرهای Reserve، Retry، Capture و ارتباط آن با Order را با ساختار جدید تطبیق می‌دهم. تغییرات را تا بخش‌هایی که قراردادشان در کد روشن است پیاده‌سازی و اعتبارسنجی می‌کنم.

---

## assistant
Nullable شدن `PaymentId` برای Reservation تأیید شد. تا روشن‌شدن پیوند Full Wallet با Order، بخش مستقل را پیش می‌برم: کلید رزرو پرداخت ترکیبی را به همان Payment وابسته می‌کنم و Replay را با `PaymentId` و Payload بررسی می‌کنم تا تلاش جدیدِ همان Order به رزرو تلاش قبلی تبدیل نشود.

---

## assistant
Replay رزرو پرداخت ترکیبی اکنون بر اساس `PaymentId` همان تلاش بررسی می‌شود و کلید ذخیره‌شده برای هر Payment ثابت است. این تغییر از استفادهٔ دوباره از رزرو یک تلاش برای Payment بعدی جلوگیری می‌کند. مسیر Full Wallet هنوز به پاسخ سؤال پیوند پایدار Reservation با Order وابسته است.

---

## assistant
بخش مستقل Reserve انجام شد و بررسی نحوی آن بدون خطا بود. Build همچنان با `NETSDK1004` و نبود `project.assets.json` متوقف می‌شود. دو تصمیمِ در انتظار پاسخ، فقط مسیرهای وابسته به Full Wallet و Verify درگاه‌ها را متوقف کرده‌اند؛ تغییرات فعلی به Entity، Mapping، Checkout یا Callback دست نزده‌اند.

---

## assistant
`WalletReservation.PaymentId` را دوباره بررسی کردم: اکنون nullable است و [EF Mapping](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28206) نیز آن را به Payment وصل می‌کند.

**بخش قابل‌اجرای Reserve انجام شد.** در [WalletReservationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:61) کلید رزرو پرداخت ترکیبی برای هر `PaymentId` ثابت شد. با تغییر [قرارداد Repository](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Repositories/IWalletReservationRepository.cs:11) و [پیاده‌سازی آن](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs:30)، Retry همان Payment رزرو قبلی را با تطبیق کاربر، Assignment و مبلغ بازیابی می‌کند؛ دادهٔ متفاوت Conflict می‌گیرد. تلاش جدید با Payment متفاوت، رزرو تلاش قبلی را Replay نمی‌کند.

**Full Wallet هنوز کامل نشده است:** [WalletReservation.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs:71) با وجود nullable شدن `PaymentId`، ارتباط مستقیم یا قرارداد تأییدشده‌ای برای بازیابی Reservation بدون Payment از روی Order ندارد. [WalletSettlementService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:34) نیز Capture بدون Payment را نمی‌پذیرد. تصمیم دربارهٔ پیوند پایدار Order و Source/قرارداد Verify درگاه‌ها برای شما ارسال شده است؛ تا پاسخ آن‌ها، مسیر پرداخت کامل و اتصال Checkout/Callback را با حدس پیاده‌سازی نمی‌کنم.

بررسی نحوی سه فایل تغییرکرده **صفر خطا** داشت. Build با `NETSDK1004` به‌دلیل نبود `project.assets.json` متوقف شد؛ تست Runtime تأیید نشده است. هیچ Entity، Mapping، Schema یا مسیر درگاه تغییر نکرد.

---

## user

# Files pasted by the user:

## "دقیقاً؛ اول کدهای ماژول Payment را به پروژه اضافه کن و بعد این پرامپت را به Cod…": C:\Users\pc\.codex/attachments/9d7133aa-030f-47d5-8674-5cb37af13c52/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن پیوست را می‌خوانم و سپس بررسی می‌کنم Source ماژول Payment در Workspace حاضر شده است یا نه. بر اساس قراردادهایی که واقعاً در پروژه موجودند ادامه می‌دهم.

---

## assistant
## Handoff: Payment Module Architecture Review

### Current task

The latest pasted request is **read-only analysis** of the newly added `Modules/YashilPayment/` module. The user wants a Persian report covering architecture, end-to-end payment flows, gateway Verify behavior, Wallet integration points, unresolved decisions, and one limited next feature. **Do not edit any file or implement anything.**

I read the attachment at:

`C:\Users\pc\.codex\attachments\9d7133aa-030f-47d5-8674-5cb37af13c52\Pasted text.txt`

The full request explicitly says the Payment module should already be added before analysis. It is now present as untracked `Modules/YashilPayment/`, including `YashilPayment.Web/Areas/Payment/Controllers/VibePaymentController.cs`, `YashilPayment.Infrastructure/ServiceImpl/VibeService.cs`, and other gateway controllers. I told the user I would read the request and check whether the module was present. **No edits were made in this turn.**

### Prior relevant facts

- Repository root: `P:\Projects\walletProject\Wallet`; current working directory: `P:\Projects\walletProject\Wallet\backEnd`.
- Existing Wallet files are largely uncommitted/untracked user work. Preserve them.
- `WalletReservation.PaymentId` and `WalletTransaction.PaymentId` were most recently observed as nullable in the Entity files; verify current state before reporting.
- Earlier analysis found Commerce payment creation in `Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs::CreateNewPaymentAndReturnAsync`, and Checkout routing in `.../YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs`. Previously Vibe source was missing; **it is now present**, so refresh conclusions from actual code.
- Current Wallet services include `WalletOrderPreparationService`, `WalletReservationService`, `WalletSettlementService`, `WalletRefundService`, `WalletProgramService`, `WalletAuditService`, and `WalletGrantService`.
- `git status` reported `?? Modules/YashilPayment/` and many earlier Wallet changes. Do not mistake those for current-turn modifications.
- A memory lookup in `C:\Users\pc\.codex\memories\MEMORY.md` returned no relevant hits; no memory citation is needed.

### Next steps

1. Read the relevant repository guide and Wallet skill only as needed; they were read in prior turns, but the latest pasted request asks to consider them. Read `docs/codex/README.md` and relevant Wallet documentation selectively.
2. Map the newly added Payment module: file structure, projects, DI, entities, services, repositories, controllers, configuration and shared abstractions. Avoid dumping generated references or secrets.
3. Trace actual calls for Checkout → Payment creation → redirect → callback → Verify → Payment/Order status, especially `VibePaymentController` and `VibeService`; separately note Ansar/Saderat and Commerce Saman/Samin/Hekmat where relevant.
4. Examine retry, duplicate callback, timeout, transaction boundaries and status transitions from concrete methods.
5. Compare actual Payment flow with Wallet services and current Entity/EF mapping. Clearly distinguish verified integration points from gaps.
6. Produce **only** the seven requested report sections in Persian, including a table of integration points, clickable absolute file links with line numbers, and a limited suggested next feature. State inaccessible or missing dependencies explicitly.

### Hard constraints

No code, Entity, Mapping, Schema, Service, Repository, Controller, or documentation changes. No new Feature implementation. Do not assume gateway behavior from names or prior reports; the new Payment module is the key source of truth.

---

## assistant
ماژول تازه‌اضافه‌شدهٔ Payment را در کنار مسیرهای فعلی Commerce و Wallet بررسی می‌کنم و جریان واقعی ایجاد، بازگشت و Verify پرداخت را تا نقاط اتصال Wallet دنبال می‌کنم. این مرحله فقط تحلیل است و فایلی تغییر نمی‌دهم.

---

## assistant
از Skill مرتبط با Wallet برای بررسی محدود invariants مالی استفاده می‌کنم. راهنماها در ریشهٔ Repository قرار دارند و آن‌ها را از همان مسیر می‌خوانم.

---

## assistant
مسیر Vibe اکنون در Source موجود است. در آن، Callback ابتدا با Token به Payment می‌رسد و سپس Verify سرویس درگاه اجرا می‌شود؛ وضعیت سفارش و پرداخت در همان سرویس تغییر می‌کند. حالا مرز این تغییر وضعیت‌ها با سرویس‌های Wallet و سناریوی پرداخت کامل با Wallet را تطبیق می‌دهم.

---

## assistant
دو ناسازگاری برای گزارش نهایی روشن شد: `PaymentId` در Entityهای Wallet nullable است، اما `Reserve` و `Capture` فعلی Payment معتبر را الزامی می‌دانند؛ همچنین مسیر آماده‌سازی Order، Reserve را بدون ساخت Payment فراخوانی می‌کند. این‌ها را به‌عنوان وضعیت فعلی کد گزارش می‌کنم و برای پرداخت کامل با Wallet قرارداد تازه‌ای فرض نمی‌کنم.

---

## assistant
## ۱. معماری ماژول Payment

ماژول اضافه‌شدهٔ `YashilPayment` در سه پروژهٔ Core، Infrastructure و Web قرار دارد. قراردادهای درگاه در Core، پیاده‌سازی‌هایی مانند `VibeService` در Infrastructure، و Controllerهای بازگشت درگاه در Web هستند. ثبت سرویس‌ها در [ModuleInitializer.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/ModuleInitializer.cs:15) انجام می‌شود.

`Payment` و ارتباط آن با `Order` در Entityهای مشترک قرار دارند؛ ایجاد Payment و تغییر وضعیت آن در Commerce انجام می‌شود، نه در ماژول درگاه: [Payment.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/Payment.cs:27)، [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552)، [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:77).

## ۲. زنجیرهٔ فرایند پرداخت

`SaveShoppingCartToOrder` سفارش را ثبت می‌کند و بر اساس روش انتخابی، URL درگاه را برمی‌گرداند: [OrderController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:142)، [GetRedirectUrl](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:745).

در مسیر Vibe زنجیرهٔ واقعی چنین است:

`VibePaymentController.Index` → `CreateNewPaymentAndReturnAsync` → `VibeService.CreateOrderAsync` → ذخیرهٔ Token روی Payment → Redirect → Callback → `VerifyVibeOrderAsync` → تغییر وضعیت Order و Payment. مبدأ و Callback در [VibePaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/VibePaymentController.cs:34) و اجرای درگاه در [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:48) هستند.

`CalcAmountForPayAsync` در کد فعلی مبلغ سفارش را با کسر Paymentهای `Paid` محاسبه می‌کند؛ `CreateNewPaymentAndReturnAsync` برای مبلغ مثبت Payment با وضعیت `SendToPaymentGateway` می‌سازد و برای مبلغ صفر یا منفی `null` برمی‌گرداند: [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412)، [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552).

## ۳. قرارداد درگاه‌ها و Verify

- **Vibe:** درخواست اولیه شامل `ReservationNumber` به‌عنوان `order_id` و مبلغ Payment به‌عنوان `cart_amount` است. Token از URL برگشتی استخراج و روی Payment ذخیره می‌شود. Callback با `ref_id`، Payment را از روی Token پیدا می‌کند؛ `result` موفق به‌تنهایی اثبات پرداخت نیست و سرویس درخواست جداگانه‌ای به `VerifyUrl` می‌فرستد. پاسخ Verify در DTO فعلی فیلد مبلغ ندارد. `Status=true` به ثبت `Order=Complete` و سپس `Payment=Paid` می‌رسد: [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:67)، [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:142)، [VerifyVibeOrderResponseDto](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Core/DTO/VibeCreditPaymentGatewayDto.cs:157).
- **Ansar و Saderat:** هر دو Payment را پیش از درخواست Token می‌سازند و در Callback با شناسهٔ تلاش، Payment را بازیابی می‌کنند. سپس Verify اختصاصی درگاه را اجرا می‌کنند و در مسیر موفقیت، Order و Payment را جداگانه به `Complete` و `Paid` می‌برند: [AnsarPaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:38)، [SaderatPaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/SaderatPaymentController.cs:89).
- **HekmatOnline:** ساخت Token و Verify در سرویس اختصاصی است؛ علاوه بر Callback، مسیر بررسی با Token دارد. تأیید درگاه در این مسیر نیز پیش از ثبت `Paid` انجام می‌شود: [HekmatOnlinePaymentGatewayController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/HekmatOnlinePaymentGatewayController.cs:81)، [HekmatOnlinePaymentGatewayService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:633).

حفاظت Callbackها یک قرارداد مشترک پایدار نیست. برای نمونه، Vibe از `TryAdd` در یک `ConcurrentDictionary` درون‌حافظه‌ای استفاده می‌کند و کلید را در این متد پاک نمی‌کند؛ پس Retry پس از خطای موقت نیز می‌تواند رد شود. Vibe پس از گذشت ۳۰ دقیقه یا خطای Verify، Payment را `Unsuccessful` می‌کند. قطعی بودن شکست در همهٔ حالت‌های Timeout از کد قابل اثبات نیست: [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:156)، [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:181)، [PaymentTransactionUtil.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/Implementations/PaymentTransactionUtil.cs:5).

## ۴. وضعیت فعلی اتصال Wallet

`WalletReservation.PaymentId` و `WalletTransaction.PaymentId` در Entityها nullable هستند و FK آن‌ها به Payment در EF Mapping ثبت شده است: [WalletReservation.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs:71)، [WalletTransaction.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:70)، [YashilAppDbContext.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28207)، [YashilAppDbContext.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28455). ارتباط صریح و پایدار `OrderId` در این دو Entity وجود ندارد؛ مسیر فعلی تشخیص سفارش از `Payment → Order` می‌گذرد.

در مقابل، قرارداد اجرایی فعلی Wallet برای Full Wallet آماده نیست: `WalletReserveRequestDto.PaymentId` اجباری است، `ReserveAsync` وجود Payment را بررسی می‌کند، و `CaptureAsync` نیز Payment موفق می‌خواهد. [WalletReserveRequestDto.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletReserveRequestDto.cs:3)، [WalletReservationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33)، [WalletSettlementService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:34).

مسیر `WalletController.Reserve` از `WalletOrderPreparationService` استفاده می‌کند، اما این سرویس بدون ساخت Payment، درخواست Reserve می‌فرستد و جست‌وجوی Reservation قبلی را نیز از `x.Payment.OrderId` انجام می‌دهد. بنابراین این مسیر با قرارداد فعلی `ReserveAsync` سازگار نیست: [WalletController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs:22)، [WalletOrderPreparationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:65)، [WalletOrderPreparationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:130). مسیر تأییدشده‌ای برای نهایی‌سازی سفارش بدون Payment در جریان بررسی‌شده پیدا نشد.

## ۵. جدول نقاط اتصال

| مرحله | متد و فایل موجود | رفتار فعلی | تغییر احتمالی موردنیاز |
|---|---|---|---|
| آماده‌سازی سفارش | [SaveShoppingCartToOrder](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:142) | ثبت Order و انتخاب مسیر پرداخت | تعیین سهم Wallet پیش از شروع درگاه، بدون تغییر مسیر عادی |
| محاسبه و ساخت تلاش پرداخت | [CreateNewPaymentAndReturnAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552) | Payment با مبلغ قابل پرداخت فعلی می‌سازد | قرارداد مبلغ باقی‌ماندهٔ درگاه در Mixed Payment باید مشخص شود |
| پیش از Redirect | [VibePaymentController.Index](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/VibePaymentController.cs:54) | بلافاصله پس از ساخت Payment، درخواست Vibe را آغاز می‌کند | محل مناسب اتصال Reserve به **همان Payment attempt**؛ شکست Reserve باید مانع درخواست درگاه شود |
| نتیجهٔ درگاه | [VerifyVibeOrderAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:142) | Verify و ثبت وضعیت Payment/Order | Capture فقط پس از Verify معتبر و `Paid`؛ Release فقط برای شکست قطعی |
| Settlement | [WalletSettlementService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:54) | تغییر Reservation، مانده‌ها و Ledger در تراکنش `Serializable` | هماهنگی با نتیجهٔ Payment و Order، همراه با Retry قابل بازیابی |
| Full Wallet | [WalletOrderPreparationService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:37) | سهم Wallet را حساب می‌کند، ولی Reserve فعلی Payment می‌خواهد | قرارداد پایدار اتصال به Order و نهایی‌سازی بدون Payment نیاز به تصمیم دارد |

## ۶. محدودیت‌ها و تصمیم‌های معماری

1. **Mixed Payment:** در مسیرهای فعلی، مبلغ Payment و درخواست درگاه از مبلغ کامل قابل پرداخت ساخته می‌شوند. باید مشخص شود سهم Wallet چگونه از مبلغ درگاه و اطلاعات اقلام ارسالی به هر Provider کسر می‌شود؛ مخصوصاً Vibe هم `cart_amount` و هم جزئیات کامل سبد را می‌فرستد: [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:67)، [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:366).
2. **Full Wallet:** nullable بودن `PaymentId` در Entity کافی نیست. قرارداد فعلی Reserve، Capture، Refund و تشخیص Order به Payment تکیه دارد. بدون Payment صوری، شناسهٔ پایدار سفارش و مسیر مجاز نهایی‌سازی آن باید تعیین شود.
3. **نتیجهٔ نامشخص Verify:** درگاه‌ها در برخی خطاهای ارتباطی وضعیت `Unsuccessful` ثبت می‌کنند. برای Release اعتبار باید تفاوت شکست قطعی و نتیجهٔ نامشخص در قرارداد یکپارچه‌سازی تعیین شود؛ Callback به‌تنهایی مبنای Capture نیست.
4. **اتمیک‌بودن بین ماژول‌ها:** `Order=Complete` و `Payment=Paid` در مسیرهای بررسی‌شده دو فراخوانی جدا هستند و Capture Wallet هنوز به آن‌ها متصل نیست. بازیابی وضعیت در صورت خطا بین این مراحل تعریف نشده است. تراکنش داخلی Settlement فقط تغییرات مالی خود Wallet را پوشش می‌دهد.
5. **چند تلاش پرداخت:** هر فراخوانی `CreateNewPaymentAndReturnAsync` می‌تواند Payment تازه بسازد. Reserve فعلی کلید را از `PaymentId` می‌سازد و برای همان تلاش Replay دارد؛ اتصال Reservation تلاش قبلی به Payment جدید نباید انجام شود: [WalletReservationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:61).

رفتار Runtime درگاه‌ها، تنظیمات محیط و Schema واقعی DB در این بررسی اجرا یا تأیید نشده‌اند؛ نتایج بالا بر اساس Source موجود هستند.

## ۷. پیشنهاد مرحلهٔ بعد

یک Feature محدود برای **هماهنگ‌کردن قرارداد آماده‌سازی Wallet با Payment attempt در مسیر پیش از Redirect** تعریف شود: ابتدا ناسازگاری `WalletOrderPreparationService` و `ReserveAsync` رفع شود، سپس فقط برای یک مسیر مشخص مانند Vibe، محل Reserve پیش از درخواست درگاه و رفتار شکست آن نهایی شود. قرارداد مبلغ Mixed Payment و نتیجهٔ نامشخص Verify باید پیش از اتصال Capture/Release به Callback تصویب شوند.

هیچ فایلی تغییر نکرد و Implementation یا Build/Test در این مرحله انجام نشد.

---

## user
# Task: بررسی قرارداد مبلغ درگاه برای Mixed Wallet Payment

**نقش:** Senior .NET Architect

این وظیفه فقط برای تحلیل کد است. هیچ فایلی را تغییر نده و هیچ پیاده‌سازی انجام نده.

## هدف

مشخص کن آیا درگاه‌های فعلی پروژه می‌توانند در سناریوی Mixed Payment، مبلغ باقی‌مانده پس از سهم Wallet را دریافت کنند، بدون اینکه مبلغ سفارش یا اطلاعات سبد با درخواست درگاه مغایرت پیدا کند.

## موارد بررسی

1. مسیر محاسبه مبلغ را از `CalcAmountForPayAsync` تا `CreateNewPaymentAndReturnAsync` و سپس ساخت درخواست هر درگاه دنبال کن.
2. برای هر درگاه بررسی کن:
   - مبلغ درخواست از کجا خوانده می‌شود؟
   - آیا مبلغ از `Payment.Amount` گرفته می‌شود یا دوباره از Order محاسبه می‌شود؟
   - آیا جزئیات کامل Order یا اقلام آن به درگاه ارسال می‌شوند؟
   - آیا مجموع مبالغ جزئیات سبد باید با مبلغ درخواست برابر باشد؟
   - آیا امکان پرداخت بخشی از مبلغ سفارش در قرارداد فعلی درگاه وجود دارد؟
3. برای Vibe به‌طور ویژه، ارتباط `cart_amount`، `ReservationNumber`، جزئیات سبد و مبلغ `Payment` را بررسی کن. مشخص کن آیا کاهش مبلغ درگاه به اندازه سهم Wallet با اطلاعات ارسالی فعلی سازگار است یا به تغییر قرارداد درخواست نیاز دارد.
4. بررسی کن آیا `CalcAmountForPayAsync` با کسر Paymentهای Paid، در صورت Capture شدن Wallet، مبلغ قابل پرداخت را به‌درستی محاسبه می‌کند یا ممکن است مبلغ Wallet دوباره کسر یا دوباره مطالبه شود.
5. مشخص کن Wallet باید صرفاً یک روش تأمین مالی باشد یا کد فعلی فرض می‌کند کل مبلغ Order از یک Payment پرداخت می‌شود. فقط شواهد کد را گزارش کن.
6. مشخص کن حداقل تغییر لازم برای Mixed Payment چیست و چه قسمت‌هایی باید بدون تغییر باقی بمانند تا پرداخت عادی فعلی مختل نشود.

## خروجی

گزارش فارسی و مختصر با این بخش‌ها:

- زنجیره واقعی محاسبه مبلغ و ساخت درخواست درگاه
- جدول مقایسه درگاه‌ها و قرارداد مبلغ آن‌ها
- تحلیل دقیق Vibe
- ریسک‌های دوباره‌کسرشدن یا مغایرت مبلغ
- قرارداد پیشنهادی برای سهم Wallet و مبلغ باقی‌مانده
- یک Feature اجرایی محدود برای مرحله بعد، همراه با فایل‌ها و متدهای مرتبط

برای ادعاهای مهم مسیر فایل، نام متد و شماره خط را ذکر کن. واقعیت‌های موجود را از فرضیات جدا کن. تا زمان روشن‌شدن قرارداد، هیچ کدی تغییر نکند.


---

## assistant
مسیر مبلغ را از محاسبهٔ Order تا درخواست هر درگاه دنبال می‌کنم. برای هر درگاه، مبلغ ارسالی و داده‌های سبد را جدا بررسی می‌کنم و هر شرطی را که فقط به مستند بیرونی درگاه وابسته است، تأییدنشده گزارش می‌دهم. هیچ فایلی تغییر نمی‌دهم.

---

## assistant
درگاه‌های Saman، Samin، Ansar، Saderat و Hekmat مبلغ را به‌صورت مستقل در درخواست Token/پرداخت دریافت می‌کنند. Vibe علاوه بر مبلغ، جزئیات کامل Order را هم می‌فرستد. اکنون منطق جمع Paymentهای موفق و زمان `Complete` شدن Order را بررسی می‌کنم تا ریسک دوباره‌کسرشدن سهم Wallet روشن شود.

---

## assistant
## زنجیرهٔ واقعی محاسبه مبلغ و ساخت درخواست درگاه

`CalcAmountForPayAsync` مبلغ Order را می‌خواند و مجموع Paymentهای `Paid` را از آن کم می‌کند. `CreateNewPaymentAndReturnAsync` حاصل مثبت را در `Payment.Total` ثبت می‌کند و همان مبلغ را برمی‌گرداند؛ برای حاصل صفر یا منفی Payment نمی‌سازد. در Entity، فیلدی به نام `Payment.Amount` وجود ندارد و فیلد مبلغ این مسیر `Total` است. [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412)، [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552)، [Payment.cs](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/Payment.cs:122)

`GetOrderTotalPayment` مجموع `Payment.Total`های موفق را محاسبه می‌کند و در صورت وجود RefundDetail، مبلغ اصلاح‌شدهٔ آن Payment را به کار می‌برد. **Capture کیف پول را مستقلاً از Ledger در این محاسبه نمی‌خواند.** [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261)

## جدول مقایسه درگاه‌ها و قرارداد مبلغ آن‌ها

| مسیر | منبع مبلغ درخواست | دادهٔ Order ارسالی | نتیجه برای Mixed Wallet |
|---|---|---|---|
| Saman | `PaymentInformation.Total` → `Amount` توکن | شماره تلاش و شماره موبایل؛ اقلام ارسال نمی‌شوند | ارسال عددِ مانده از نظر شکل درخواست ممکن است. [SamanPaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:88)، [GenerateTokenAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:576) |
| Samin | `PaymentInformation.Total` از `GetGenerateTokenParams` → `Amount` توکن | شماره تلاش، نوع خرید و تعداد اقساط؛ اقلام ارسال نمی‌شوند | شکل درخواست مبلغ مستقل دارد؛ اثر مبلغ کمتر بر قرارداد اقساطی از Source تأیید نمی‌شود. [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:1014)، [SaminPaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SaminPaymentController.cs:379) |
| Ansar / Saderat | `PaymentInformation.Total` → مبلغ درخواست Token | شناسهٔ تلاش و اطلاعات محدود پرداخت؛ اقلام ارسال نمی‌شوند | شکل درخواست مبلغ مستقل دارد؛ پذیرش تجاری پرداخت بخشی تأیید نشده است. [AnsarPaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:51)، [SaderatPaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/SaderatPaymentController.cs:55) |
| Hekmat / HekmatOnline | `PaymentInformation.Total` → مبلغ درخواست | عنوان/شماره تلاش یا اطلاعات اقساط؛ فهرست اقلام ارسال نمی‌شود | مبلغ مستقل ارسال می‌شود، اما قواعد اقساط با مبلغ مانده نیاز به تأیید قرارداد درگاه دارد. [HekmatPaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/HekmatPaymentController.cs:89)، [HekmatOnlinePaymentGatewayService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:78) |
| Vibe | `PaymentInformation.Total` → `cart_amount` | **مبلغ و جزئیات کامل Order** | با صرفِ کاهش `cart_amount`، درخواست فعلی از نظر ارقام داخلی ممکن است ناسازگار شود. [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:67)، [PrepareVibeOrderData](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:370) |

برای هیچ‌یک از این درگاه‌ها، «پذیرش قطعی پرداخت بخشی از Order» صرفاً از امکان ارسال یک فیلد مبلغ اثبات نمی‌شود. همچنین الزام بیرونی به برابری مجموع اقلام و مبلغ درخواست از Source موجود **قابل تأیید نیست**.

## تحلیل دقیق Vibe

در `CreateOrderAsync`، مقدار `order_id` ارسالی به Vibe در واقع `PaymentInformation.ReservationNumber` است و `cart_amount` از `PaymentInformation.Total` گرفته می‌شود. در همان درخواست، `PrepareVibeOrderData` مقدار `goods_amount` و مالیات را از مجموع‌های **کامل Order** و `items` را از اقلام آن می‌سازد؛ این داده‌ها بر اساس سهم Wallet تعدیل نمی‌شوند. [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:67)، [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:370)

بنابراین کاهش `cart_amount` به مبلغ مانده، بدون تعیین تکلیف `goods_amount`، `tax` و `items`، قرارداد منسجمی در **کد فعلی** ندارد. کد برابری این ارقام را کنترل نمی‌کند؛ اینکه Vibe چنین اختلافی را می‌پذیرد یا رد می‌کند به قرارداد بیرونی آن نیاز دارد. تغییر صرفِ `Payment.Total` برای فعال‌کردن Mixed Payment در Vibe قابل اتکا نیست.

## ریسک‌های دوباره‌کسرشدن یا مغایرت مبلغ

- Capture فقط ماندهٔ Wallet و Ledger را تغییر می‌دهد؛ Payment موفقِ جداگانه‌ای برای سهم Wallet در محاسبهٔ `GetOrderTotalPayment` ایجاد نمی‌کند. پس اگر Payment درگاه به مبلغ **کل Order** ساخته شود، هم درگاه کل مبلغ را می‌گیرد و هم Wallet مصرف می‌شود. اگر Payment درگاه به مبلغ مانده ساخته شود، محاسبهٔ بعدی `CalcAmountForPayAsync` فقط سهم درگاهِ `Paid` را کم می‌کند و سهم Wallet را همچنان بدهی باقی‌مانده می‌بیند، مگر Order قبلاً `Complete` شده باشد. [WalletSettlementService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:202)، [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:434)
- کد از چند Payment موفق در محاسبهٔ مانده پشتیبانی می‌کند، اما Callback درگاه‌های بررسی‌شده پس از موفقیت **همان یک Payment**، Order را `Complete` می‌کند. نمونه: [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:330). بنابراین نمی‌توان نتیجه گرفت نهایی‌سازی Mixed Payment از قبل آماده است.
- `CaptureAsync` به Payment متصل و `Paid` نیاز دارد. ثبت Payment موفق، Capture و نهایی‌سازی Order در قرارداد فعلی یک عملیات اتمیک واحد نیستند. [WalletSettlementService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:162)

## قرارداد پیشنهادی برای سهم Wallet و مبلغ باقی‌مانده

**پیشنهاد، نه رفتار پیاده‌سازی‌شده:** برای هر تلاش پرداخت، یک مبلغ نهایی Order (`T`)، سهم رزروشدهٔ Wallet (`W`) و مبلغ درگاه (`G`) ثبت و اعتبارسنجی شود، به‌طوری‌که `T = W + G` و `Payment.Total = G`. مبلغ درگاه باید از **همان Payment attempt** خوانده شود؛ Capture فقط برای Reservation متصل به آن تلاش و پس از Verify معتبر انجام شود. Order زمانی کامل شود که هر دو سهم با نتیجهٔ قطعی سازگار باشند.

این قرارداد نباید محاسبهٔ عمومی `CalcAmountForPayAsync` یا مسیر پرداخت عادی را بدون شرط Wallet تغییر دهد. برای Vibe، پیش از استفاده از `G` باید قرارداد `cart_amount` در برابر `goods_amount`، مالیات و اقلام با مستند یا پاسخ رسمی درگاه روشن شود. همچنین باید مشخص شود سهم Captureشدهٔ Wallet چگونه در محاسبهٔ ماندهٔ Order لحاظ می‌شود تا نه دوباره مطالبه شود و نه دوباره کسر گردد.

## یک Feature اجرایی محدود برای مرحله بعد

**«قرارداد مبلغ Payment attempt مختلط»**: در لایهٔ Commerce، محاسبه و اعتبارسنجی `T/W/G` برای یک Order و یک Payment attempt مشخص شود و نتیجهٔ آن بدون فراخوانی درگاه در اختیار جریان Checkout قرار گیرد. نقاط اصلی بررسی و تغییر احتمالی: `CalcAmountForPayAsync` و `CreateNewPaymentAndReturnAsync` در [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412)، `GetOrderTotalPayment` در [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261)، و `PrepareAsync` در [WalletOrderPreparationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:37). اتصال Vibe و تغییر Callbackها تا روشن‌شدن قرارداد مبلغ درگاه در این Feature قرار نگیرند.

این بررسی فقط Source را خواند؛ هیچ کدی تغییر نکرد.

---

## user
# Feature: Wallet Mixed Payment Amount Contract

## نقش

به‌عنوان Senior .NET Architect و Developer، قرارداد محاسبه مبلغ پرداخت ترکیبی Wallet و Commerce را طراحی و پیاده‌سازی کن.

## هدف

برای هر Order و Payment attempt، مبلغ کل سفارش، سهم Wallet و مبلغ باقی‌مانده درگاه باید به‌صورت دقیق و قابل اعتبارسنجی مشخص باشند:

`T = W + G`

- `T`: مبلغ نهایی قابل پرداخت Order
- `W`: مبلغ رزروشده از Wallet
- `G`: مبلغ پرداختی درگاه
- `Payment.Total = G`

این Feature باید مبنای قابل اتکایی برای اتصال بعدی Reserve، Redirect، Verify، Capture و Release فراهم کند.

## مراحل

### ۱. بررسی قبل از تغییر

- مستندات `AGENTS.md` و Skill مربوط به Wallet را بخوان.
- پیاده‌سازی فعلی `CalcAmountForPayAsync`، `CreateNewPaymentAndReturnAsync`، `GetOrderTotalPayment` و `WalletOrderPreparationService` را بررسی کن.
- بررسی کن مبلغ Order چگونه محاسبه می‌شود، Paymentهای موفق چگونه از آن کسر می‌شوند و آیا `T` بدون دوباره‌کسرشدن یا دوباره‌مطالبه‌شدن سهم Wallet قابل محاسبه است.
- قرارداد موجود `WalletReservation`، `WalletAssignment` و `WalletTransaction` را بررسی کن.
- اگر برای حفظ مبلغ نهایی Order یا جلوگیری از محاسبه مجدد، به تصمیم معماری یا داده‌ای نیاز است که در مدل فعلی وجود ندارد، پیش از پیاده‌سازی متوقف شو و دقیقاً گزارش بده. Schema و Entity را خودسرانه تغییر نده.

### ۲. قرارداد محاسبه

در صورت کافی‌بودن مدل فعلی، منطق محاسبه را در لایه مناسب Commerce پیاده‌سازی کن، به‌طوری‌که:

- مبلغ Order و Paymentهای قبلی طبق قواعد فعلی محاسبه شوند.
- سهم Wallet از اعتبار قابل استفاده و Eligibility معتبر به دست آید.
- سهم Wallet بیش از مبلغ بدهی Order نباشد.
- مبلغ درگاه برابر با باقی‌مانده باشد.
- `T = W + G` برقرار باشد.
- سهم Wallet دوباره در محاسبات بعدی به‌عنوان بدهی مطالبه نشود.
- Retry همان Payment attempt باعث رزرو مجدد یا تغییر ناخواسته مبالغ نشود.
- تلاش پرداخت جدید از تلاش قبلی مستقل باشد.
- مسیر پرداخت عادی بدون Wallet رفتار فعلی خود را حفظ کند.

از تغییر عمومی `CalcAmountForPayAsync` خودداری کن، مگر اینکه بررسی کد ثابت کند تغییر محدود و سازگار ضروری است. هر تغییر باید با آزمون مشخص پوشش داده شود.

### ۳. محدوده خارج از این Feature

در این مرحله:

- هیچ درگاه یا Callback را تغییر نده.
- Capture، Release و Verify را به Payment متصل نکن.
- Full Wallet و نهایی‌سازی Order بدون Payment را پیاده‌سازی نکن.
- برای Vibe قرارداد `cart_amount`، `goods_amount`، مالیات و `items` را تغییر نده.
- Entity، Mapping، Schema، Migration، Status ID و Permission جدید نساز.
- هیچ Payment صوری یا Payment با مبلغ صفر ایجاد نکن.

### ۴. آزمون‌ها

در صورت وجود زیرساخت تست، آزمون‌های متناسب اضافه کن:

1. پرداخت عادی بدون Wallet
2. پرداخت ترکیبی با سهم Wallet مثبت و مبلغ درگاه مثبت
3. Wallet برابر کل بدهی Order
4. اعتبار Wallet بیشتر از بدهی Order
5. عدم کفایت اعتبار Wallet
6. وجود Payment موفق قبلی
7. Retry همان تلاش پرداخت
8. تلاش پرداخت جدید برای همان Order
9. بررسی عدم دوباره‌کسرشدن یا دوباره‌مطالبه‌شدن سهم Wallet

اگر زیرساخت تست یا اطلاعات لازم موجود نیست، محدودیت را گزارش کن؛ تست اجرا‌نشده را موفق اعلام نکن.

## گزارش نهایی

به فارسی گزارش بده:

- فایل‌ها و متدهای تغییرکرده
- قرارداد نهایی محاسبه `T/W/G`
- نحوه جلوگیری از دوباره‌کسرشدن سهم Wallet
- نتیجه تست‌ها و Build
- محدودیت‌ها و تصمیم‌های حل‌نشده
- مرحله بعد پیشنهادی

برای متدهای جدید XML documentation فارسی کوتاه مطابق قواعد مخزن بنویس. تغییرات را محدود به همین Feature نگه دار.


---

## assistant
ابتدا قرارداد فعلی Order، Payment و Wallet را با مدل داده تطبیق می‌دهم. اگر بتوان `T/W/G` را برای هر تلاش پرداخت بدون افزودن داده یا تصمیم تازه پایدار نگه داشت، Plan کوتاه پیاده‌سازی را بر همان مبنا آماده می‌کنم؛ اگر مدل فعلی برای این تضمین کافی نباشد، طبق Self-Gate درخواست، همان نقطه را با ارجاع دقیق گزارش می‌دهم.

---

## assistant
## Self-Gate: تصمیم معماری لازم است

مدل فعلی هنوز برای پیاده‌سازی قابل اتکای `T = W + G` در **هر Payment attempt** قرارداد کافی ندارد؛ طبق شرط توقف شما، هیچ کدی را تغییر ندادم.

- `CalcAmountForPayAsync` فقط Paymentهای `Paid` را از مبلغ Order کم می‌کند؛ Capture کیف پول در این محاسبه نیست. `CreateNewPaymentAndReturnAsync` نیز کل حاصل را در `Payment.Total` می‌گذارد، نه `G`. [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412)، [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552)، [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261)
- `WalletOrderPreparationService.PrepareAsync` سهم Wallet را حساب می‌کند و **بدون `PaymentId`** درخواست Reserve می‌فرستد؛ `WalletReservationService.ReserveAsync` همان درخواست را رد می‌کند و Payment موجود را الزامی می‌داند. [WalletOrderPreparationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:124)، [WalletReservationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33)
- مبلغ Order پس از ثبت هم می‌تواند تغییر کند. برای تلاش پرداختی که Payment آن ساخته شده ولی Reserve آن هنوز ثبت نشده، `Payment.Total = G` به‌تنهایی `W` و در نتیجه `T` اولیه را برای Retry حفظ نمی‌کند. [ModifiedOrderItemService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:447)

**تصمیم لازم:** آیا قرارداد مورد تأیید شما این است که برای Mixed Payment، ساخت Payment با `Total = G` و ثبت Reservation به مبلغ `W` در **یک تراکنش واحد** انجام شود، به‌طوری‌که در صورت شکست Reserve هیچ Payment attempt نیمه‌کاره‌ای باقی نماند و پس از ثبت، `T` همان `Payment.Total + Reservation.Amount` آن تلاش باشد؟ این تصمیم ترتیب فعلی `PrepareAsync` و مرز تراکنش‌های موجود را تغییر می‌دهد؛ آن را خودسرانه انتخاب نمی‌کنم.

تا روشن‌شدن این قرارداد، Implementation، تست و Build انجام نشد. پرداخت عادی و کد درگاه‌ها تغییری نکردند.

---

## user
تصمیم معماری برای Mixed Payment:

اصل قرارداد پیشنهادی را تأیید می‌کنم، با این توضیح که نحوه اجرای اتمیک باید با معماری واقعی Repository و Transaction موجود سازگار باشد.

برای هر Mixed Payment attempt:

- `T` مبلغ بدهی Order در زمان ایجاد تلاش است.
- `W` مبلغ Reservation کیف پول است.
- `G = T - W` مبلغ باقی‌مانده درگاه است.
- `Payment.Total = G`.
- `WalletReservation.Amount = W`.
- رابطه `T = G + W` باید برای همان تلاش برقرار و قابل بازیابی باشد.
- Reservation باید به همان `PaymentId` متصل باشد؛ تلاش‌های پرداخت متفاوت نباید Reservation یکدیگر را Replay کنند.
- Retry همان تلاش باید همان نتیجه را بازیابی کند و مبلغ یا سهم Wallet را دوباره محاسبه و تغییر ندهد.
- اگر Reserve ناموفق باشد، نباید Payment قابل Redirect و قابل استفاده‌ای باقی بماند. ابتدا امکان Rollback اتمیک را با معماری موجود بررسی کن.
- اگر تراکنش دیتابیسی مشترک میان ایجاد Payment و Reservation با معماری فعلی ممکن نیست، راهکار امن و محدود را پیشنهاد بده و پیش از پیاده‌سازی گزارش کن؛ وضعیت نیمه‌کاره و Recovery نباید مبهم باشد.
- تغییر بعدی مبلغ Order نباید بدون سیاست مشخص، مبالغ تلاش قبلی را بازنویسی کند.
- پرداخت عادی بدون Wallet باید رفتار فعلی خود را حفظ کند.
- Full Wallet، Callback، Verify و Capture/Release در Scope این Feature نیستند.

اکنون فقط بررسی کن که آیا این قرارداد با Entityها، Repositoryها، DbContext و مرز تراکنش‌های فعلی قابل اجراست یا خیر. اگر بله، طرح اجرای دقیق و محدود را ارائه کن؛ اگر خیر، تصمیم‌های باقی‌مانده را مشخص کن. پس از این بررسی، در صورت نبود Blocker، همین Feature را پیاده‌سازی کن.

هیچ Schema، Entity، Mapping، Status ID یا Permission جدیدی بدون تأیید من تغییر نده. تغییرات را به قرارداد Mixed Payment محدود نگه دار و در پایان فایل‌های تغییرکرده، رفتار Retry و Rollback، تست‌ها و نتیجه Build را گزارش کن.


---

## assistant
قرارداد `T/W/G` و اتمیک‌بودن ایجاد Payment و Reservation را مبنا می‌گیرم. اول بررسی می‌کنم آیا `OrderService`، `WalletReservationService` و EF از یک `DbContext` و تراکنش مشترک استفاده می‌کنند؛ سپس مسیر محدود پیاده‌سازی را انتخاب می‌کنم، بدون تغییر درگاه یا جریان پرداخت عادی.

---

## assistant
تراکنش مشترک در همان `YashilAppDbContext` از نظر EF قابل طراحی است، اما دو متد فعلی هرکدام تراکنش خود را باز و Commit می‌کنند؛ بنابراین نمی‌توان آن‌ها را بدون تغییرِ مرز تراکنش به‌صورت اتمیک پشت‌سرهم فراخواند. در حال تطبیق ورودی API با هویت Payment attempt هستم تا Retry و انتخاب روش درگاه بر اساس دادهٔ واقعی انجام شوند.

---

## assistant
## نتیجه بررسی

**تراکنش مشترک با Entityهای فعلی قابل اجراست، اما قرارداد ورودی هنوز برای پیاده‌سازی کامل نیست.** `PaymentRepository` و `WalletReservationRepository` هر دو از `YashilAppDbContext` استفاده می‌کنند. با این حال، `CreateNewPaymentAndReturnAsync` و `ReserveAsync` هرکدام تراکنش مستقل باز و Commit می‌کنند؛ فراخوانی پشت‌سرهم آن‌ها اتمیک نیست. برای Mixed Payment باید ایجاد Payment و Reserve زیر یک تراکنش EF مشترک قرار گیرند. [PaymentRepository.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/PaymentRepository.cs:20)، [WalletReservationRepository.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs:17)، [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552)، [WalletReservationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:63)

طرح محدود اجرا، پس از روشن‌شدن دو ورودی زیر، این است: در یک تراکنش مبلغ `T` محاسبه و اعتبارسنجی شود؛ Payment با `Total = G > 0` ایجاد شود؛ Reservation با `Amount = W` و همان `PaymentId` ثبت شود؛ سپس برابری `T = G + W` بررسی و تراکنش Commit شود. شکست Reserve کل تراکنش، از جمله Payment، را Rollback می‌کند. پس از Commit، مبلغ تلاش از `Payment.Total + WalletReservation.Amount` بازیابی می‌شود و Retry نباید آن را از Order دوباره محاسبه کند. این مسیر فقط برای Mixed Payment خواهد بود و متد عمومی پرداخت عادی تغییر رفتار نمی‌دهد.

## دو تصمیم ورودی که باقی مانده‌اند

1. **روش درگاه از کجا می‌آید؟** درخواست فعلی `Wallet/Reserve` فقط `OrderId` و `WalletProgramId` دارد؛ `Order` نیز `PaymentMethodType` ذخیره نمی‌کند. در مقابل، ساخت Payment و محاسبهٔ آن به `PaymentMethodType` نیاز دارند. پیشنهاد من افزودن `PaymentMethodType` به درخواست Mixed و اعتبارسنجی آن با روش‌های موجود است. [WalletOrderPreparationDto.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletOrderPreparationDto.cs:3)، [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552)

2. **Retry چگونه همان attempt را مشخص می‌کند؟** درخواست فعلی شناسهٔ Payment attempt ندارد. با `(OrderId, WalletProgramId)` یکسان، پس از پایان تلاش قبلی نمی‌توان Retry آن را از درخواستِ تلاش جدید تشخیص داد. پیشنهاد من یک `PaymentAttemptKey` مستقل در درخواست Mixed است؛ کلید Reserve همچنان فقط از `PaymentId` ساخته شود و کلید ارسالی Client مستقیماً به‌عنوان کلید Reservation استفاده نشود. [WalletOrderPreparationDto.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletOrderPreparationDto.cs:3)، [WalletReservationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:60)

این دو مورد قرارداد API و معنای «همان تلاش» را تعیین می‌کنند. طبق دستور شما، پیش از انتخاب خودسرانهٔ آن‌ها متوقف شدم. **هیچ فایلی تغییر نکرد؛ تست و Build اجرا نشد.**

---

## user
یک نکته در orderController از adminApi میتوانی متد SaveShoppingCartToOrder را بررسی کنی که نوع درخواست پرداخت از این متد مشخص می شود 


---

## assistant
درست است؛ مسیر `SaveShoppingCartToOrder` را از ورودی `PaymentMethodType` تا خروجی و داده‌ای که روی Order ذخیره می‌شود بررسی می‌کنم. بعد نتیجهٔ Self-Gate را اصلاح می‌کنم و اگر هویت روش پرداخت برای Mixed از همین مسیر قابل انتقال باشد، کار را بر همان مبنا ادامه می‌دهم.

---

## assistant
درست می‌گویید. در [SaveShoppingCartToOrder](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:142)، روش انتخابی از `ShoppingCartParam.PaymentMethodType` گرفته می‌شود؛ هم برای Order جدید و هم برای پرداخت دوبارهٔ Order موجود. سپس [GetRedirectUrl](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:745) مسیر درگاه را بر اساس آن تعیین می‌کند. بنابراین گفتهٔ قبلی من که «روش درگاه در Checkout مشخص نیست» نادرست بود.

نکتهٔ باقی‌مانده این است که این مقدار در [Order ساخته‌شده](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:354) ذخیره نمی‌شود و به درخواست فعلی [Wallet/Reserve](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletOrderPreparationDto.cs:3) هم منتقل نمی‌شود. می‌توان آن را از همان مسیر Checkout به آماده‌سازی Mixed Payment رساند؛ نیازی به فیلد جدید در Entity نیست.

برای ادامهٔ پیاده‌سازی، تصمیم لازم اکنون **محل آغاز مسیر Mixed** است: آیا `SaveShoppingCartToOrder` باید با دریافت `WalletProgramId` آماده‌سازی Mixed را آغاز کند، یا پس از ثبت Order، درخواست جداگانهٔ `Wallet/Reserve` باید `PaymentMethodType` انتخاب‌شده را نیز دریافت کند؟ همچنین برای تمایز Retry همان تلاش از تلاش جدید، شناسهٔ پایدار attempt باید در قرارداد درخواست/پاسخ مشخص شود. این دو داده در قرارداد فعلی [ShoppingCartParam](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs:3) و Wallet به‌صورت مشترک وجود ندارند.

این بررسی اصلاحی فقط خواندنی بود؛ فایلی تغییر نکرد.

---

## user
این مسیر رو هم کامل بررسی کن \backEnd\Yashil.WebHost\Views\Order تا فرایند مربوط به انتخاب درگاه رو کامل مسلط بشی و بتونی سمت کاربری رو که قرار است کاربر کسر از کیف پول را انتخاب کند و سناریو و پیاده سازی آن را با هم بررسی کنیم و کامل به من توضیح بده که چه برنامه ای برای این موضوع داری
همچنین به اسکریپت های این صفحه که هر کدام مربوط به موضوع پرداخت و انتخاب درگاه و موارد سمت کاربری است که باید باید اضافه کنیم یا تغییر دهیم نیز مسلط بشو و نتیجه رو اعلام کن به همراه نتیجه بررسی پرامپت قبلی


---

## assistant
نمای `Yashil.WebHost/Views/Order` و اسکریپت‌های متصل به انتخاب روش پرداخت را از ورودی صفحه تا `SaveShoppingCartToOrder` دنبال می‌کنم. سپس نتیجهٔ بررسی قبلی را با این مسیر سمت کاربر یکجا جمع‌بندی می‌کنم و برنامهٔ مشخصی برای انتخاب سهم Wallet ارائه می‌دهم؛ در این مرحله کدی تغییر نمی‌دهم.

---

## assistant
انتخاب درگاه در یک Razor partial انجام می‌شود: کامپوننت، روش‌های مجاز را به radio button تبدیل می‌کند و اسکریپت همان partial مقدار انتخاب‌شده را به `SaveShoppingCartToOrder` می‌فرستد. حالا مسیر پرداخت مجدد Order، فیلترهای نمایش درگاه و اسکریپت‌های مشترک صفحه را بررسی می‌کنم تا محل افزودن انتخاب Wallet برای هر دو حالت Order جدید و موجود روشن شود.

---

## assistant
## Handoff summary

### Active task

The user asked for a **read-only, detailed review** of `backEnd/Yashil.WebHost/Views/Order` and its payment-related scripts. They want to understand where a customer would choose a Wallet deduction, how that choice would fit the existing gateway selection flow, and a concrete implementation plan. Include an updated conclusion from the preceding **Wallet Mixed Payment Amount Contract** discussion. **Do not edit files yet.**

Repository root is `P:/Projects/walletProject/Wallet`; working directory is `P:/Projects/walletProject/Wallet/backEnd`. Use absolute clickable file links in the Persian final report. The user corrected a previous claim: the selected gateway **is** available in `SaveShoppingCartToOrder`; it simply is not persisted on `Order` or passed to the separate Wallet preparation API.

### Findings verified this turn

- [CheckOut.cshtml](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/CheckOut.cshtml:51) has recipient and order-summary tabs. Its `loadOrderSummary` function loads `Order/OrderSummery` into `#orderSummary` via AJAX, so the payment controls live in a dynamically loaded partial.
- MVC [OrderController.OrderSummery](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/OrderController.cs:260) returns `_OrderSummary`, which contains `_OrderSummaryForPay`. `Detail(orderId)` also returns `_OrderSummary`, so the same controls can appear when paying an existing Order.
- [_OrderSummaryForPay.cshtml](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:159) displays gateway options only when `AllowSelectPaymentGateway` and no supplier-approval requirement. It invokes `ApplicationPaymentMethodComponent` with Hekmat, bank-gateway, and sale-type filters. The button is shown under `AllItemsHasValue`, `AllowContinueShopping`, and `SaleType.HasValue` (line 174).
- [ApplicationPaymentMethodComponent.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewComponents/ApplicationPaymentMethod/ApplicationPaymentMethodComponent.cs:38) queries payment methods configured for the application, then filters by sale type. Its Razor [view](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Shared/Components/ApplicationPaymentMethodComponent/_ApplicationPaymentMethod.cshtml:9) renders radio buttons named `paymentMethodType`. It has icon cases for Ansar, Hekmat, Saman, Samin, Saderat, Hybrid, HekmatOnline; no explicit Vibe icon case, despite routing support elsewhere.
- The main click flow in [_OrderSummaryForPay.cshtml](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:221): `checkAndPayDuplicateOrder` (global `commerce.js`) → `doPay` checks the approval checkbox, reads selected `paymentMethodType`; Samin opens a purchase-type modal; other gateways call `doFinalPay`. `doFinalPay` (line 273) posts `{recipientId, paymentMethodType, orderId, purchaseType, RequiredSupplierApprovalBeforePayment}` to `/api/order/saveShoppingCartToOrder` (line 308), then navigates to `data.returnUrl` (line 321).
- The Samin modal is loaded through `showPaymnetMethodInfo` (line 349) from `SaminPayment/ShowPurchaseType`; its buttons call `doFinalPay` with a specific purchase type: [ShowPurchaseType.cshtml](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/SaminPayment/ShowPurchaseType.cshtml:43).
- [ShoppingCartParam.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs:3) carries `PaymentMethodType`, `OrderId`, `PurchaseType`, and recipient. It has no WalletProgram or Wallet-use selection.
- API [OrderController.SaveShoppingCartToOrder](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:142) gets selected payment method for new Orders (line 321) and existing Orders (line 593); for `IncompleteCashAndInstallmentPay`, it overrides to Hybrid (line 590). After saving, it returns `GetRedirectUrl` (line 637). [GetRedirectUrl](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:745) directly selects gateway URL, including legacy `WalletPayment` for `PaymentMethodTypesEnum.Wallet`, Vibe, others. Payment method is not stored on the new Order initialization (around lines 354–391) or in the `Order` Entity.
- Global, readable [commerce.js](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/wwwroot/js/commerce.js:767) implements `checkAndPayDuplicateOrder`; avoid dumping `commerce.min.js`, which is one huge line. Check actual layout inclusion of this script.
- Prior backend conclusions: Existing [WalletController.Reserve](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs:22) accepts only `OrderId` and `WalletProgramId`; [WalletOrderPreparationService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:130) calls Reserve without `PaymentId`; [WalletReservationService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33) requires existing Payment. `OrderService.CreateNewPaymentAndReturnAsync` and `ReserveAsync` have separate transactions; technically a shared EF transaction appears possible because Payment and Wallet repositories both use `YashilAppDbContext`, but neither can be composed as-is. `Payment.Total` currently gets the whole outstanding amount. Retry identity must be defined, ideally tied to Payment attempt, with lost-response handling. No code changes were made.

### Remaining work

1. Read relevant sections, with line numbers, of:
   - `Yashil.WebHost/Views/Order/_OrderSummary.cshtml`, `OrderList.cshtml`, `_OrderListItem.cshtml` to trace existing-Order repayment.
   - `Yashil.WebHost/wwwroot/js/commerce.js` around lines 735–815 and the layout(s) that load it.
   - `Yashil.WebHost/Views/Shared/Components/Content/_OrderSummeryDescriptions.cshtml` for the approval checkbox.
   - The `OrderDataFactory` methods `FillOrderSummaryByRecipientId`, `FillOrderSummaryByOrderId`, and summary-model population to identify how `SumTotalForPay`, `AllowSelectPaymentGateway`, `SaleType` and filters are calculated. Keep inspection scoped.
   - Legacy `WalletPayment` view/controller **only enough** to distinguish its old card-based payment method from new WalletProgram deduction.
2. Produce a Persian report explaining the complete current customer flow, exact scripts/functions and UI conditions, re-payment flow, and a concrete phased plan for adding WalletProgram selection + amount preview/confirmation without breaking normal payment or prematurely redirecting. Highlight that gateway selection is already known at `SaveShoppingCartToOrder`, but `WalletProgramId` and attempt identity are not carried there. Avoid inventing UI or business decisions: identify choices (Wallet toggle vs method, multiple Programs, partial amount control, retry token) as proposals/questions.
3. Do **not** implement in this turn. The user explicitly requested review and explanation of the plan.

### Communication

A commentary update was already sent describing the read-only review, followed by an update explaining that gateway radio selection occurs in a dynamically loaded Razor partial. Send another concise commentary update if work continues beyond ~60 seconds. No memory files were used; no memory citation is needed.

---

## assistant
مسیر نمای Order و اسکریپت‌های پرداخت را تا انتخاب درگاه دنبال کرده‌ام. اکنون شاخه‌های پرداخت سفارش موجود، شرط‌های نمایش کنترل‌ها و محاسبه خلاصه سفارش را بررسی می‌کنم تا طرح سمت کاربر با قرارداد Mixed Payment دقیق باشد. از راهنمای `wallet-feature` هم برای کنترل مرزهای مالی استفاده می‌کنم؛ در این مرحله فقط گزارش می‌دهم و کدی تغییر نمی‌دهم.

---

## assistant
نتیجه مهم تا اینجا: انتخاب درگاه در صفحه مشتری داخل partial خلاصه سفارش انجام می‌شود و همراه درخواست `SaveShoppingCartToOrder` به API می‌رسد. برای Wallet جدید هنوز انتخاب Program و سهم Wallet در همان درخواست وجود ندارد. شاخه پرداخت مجدد و محاسبه مبلغ نمایشی را نیز با سرویس مالی تطبیق می‌دهم تا نقطه اتصال درست مشخص شود.

---

## assistant
## نتیجه بررسی مسیر کاربر

در Checkout، پس از انتخاب گیرنده، تابع `loadOrderSummary` خلاصه سفارش را از سرور داخل `#orderSummary` بارگذاری می‌کند. بنابراین محل واقعی افزودن انتخاب کسر از کیف پول، صفحه اصلی Checkout نیست؛ بخش پرداخت در [_OrderSummaryForPay.cshtml](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:120>) است که از [_OrderSummary.cshtml](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummary.cshtml:52>) نمایش داده می‌شود. همین بخش در [Detail.cshtml](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/Detail.cshtml:69>) برای پرداخت سفارش موجود نیز استفاده می‌شود.

جریان فعلی مشتری چنین است:

1. گزینه‌های درگاه، بر اساس تنظیمات Application و نوع فروش، با [ApplicationPaymentMethodComponent](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewComponents/ApplicationPaymentMethod/ApplicationPaymentMethodComponent.cs:38>) ساخته می‌شوند. هر گزینه یک radio با نام `paymentMethodType` است. نمایش آن‌ها به `AllowSelectPaymentGateway` و نبود الزام تأیید تأمین‌کننده وابسته است؛ دکمه پرداخت نیز شرط‌های جداگانه‌ای دارد.
2. دکمه پرداخت ابتدا `checkAndPayDuplicateOrder` را در [commerce.js](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/wwwroot/js/commerce.js:767>) اجرا می‌کند. صفحه در عمل `commerce.min.js` را از [_Layout.cshtml](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Shared/_Layout.cshtml:855>) بارگذاری می‌کند؛ تغییر آینده این منطق باید در فایلِ اجراشونده هم منعکس شود.
3. `doPay` تأیید توافقنامه و درگاه انتخابی را می‌خواند. ثمین ابتدا پنجره انتخاب نوع خرید را باز می‌کند؛ بقیه به `doFinalPay` می‌روند. این تابع `paymentMethodType`، `orderId` و سایر ورودی‌ها را به `SaveShoppingCartToOrder` می‌فرستد و سپس به `returnUrl` می‌رود. [محل ارسال](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:221>) و [پنجره ثمین](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/SaminPayment/ShowPurchaseType.cshtml:43>) مشخص‌اند.

**تصحیح مهم نسبت به برداشت قبلی:** درگاه انتخابی در [SaveShoppingCartToOrder](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:321>) در دسترس است؛ برای سفارش موجود نیز خوانده می‌شود. سپس [GetRedirectUrl](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:745>) مسیر درگاه را تعیین می‌کند. آنچه در این قرارداد حمل نمی‌شود، **انتخاب WalletProgram، سهم Wallet و شناسه تلاش Payment** است؛ [ShoppingCartParam](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs:3>) این فیلدها را ندارد.

گزینه فعلی `Wallet` در `GetRedirectUrl` به [WalletPaymentController](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/WalletPaymentController.cs:15>) می‌رود. این مسیر پرداخت قدیمی مبتنی بر کارت باشگاه مشتریان است و نباید بدون بررسی، معادل کسر از `WalletProgram` تلقی شود.

## نتیجه پرامپت قبلی: قرارداد مبلغ Mixed Payment

مدل موجود، ارتباط Reservation با Payment را از طریق `PaymentId` دارد؛ پس بازیابی `T = W + G` برای **یک تلاش مشخص** از نظر داده‌ای قابل طرح است. اما اجرای فعلی هنوز این قرارداد را برقرار نمی‌کند:

- [CreateNewPaymentAndReturnAsync](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552>) مبلغ کامل محاسبه‌شده را در `Payment.Total` می‌گذارد و Payment را در تراکنش خودش ثبت می‌کند. مسیر Vibe نیز تازه **پس از Redirect** این متد را صدا می‌زند؛ [VibePaymentController](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/VibePaymentController.cs:54>).
- [WalletReservationService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33>) وجود `PaymentId` معتبر را الزامی می‌داند و تراکنش جداگانه‌ای آغاز می‌کند؛ در مقابل، [WalletOrderPreparationService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:130>) هنگام فراخوانی Reserve، `PaymentId` نمی‌فرستد. این مسیر در وضعیت فعلی نمی‌تواند Reservation موفق برای قرارداد جدید بسازد.
- `CalcAmountForPayAsync` مجموع Paymentهای `Paid` را از بدهی کم می‌کند و [GetOrderTotalPayment](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261>) `Payment.Total` آن‌ها را جمع می‌زند. اگر `Payment.Total = G` شود، سهم `W` بدون قرارداد مشخص برای شناسایی Capture در بدهی سفارش، در محاسبه بعدی باقی می‌ماند. همچنین Preparation فعلی مبلغ را با نوع پرداخت `Wallet` می‌خواند، حال آنکه درگاه انتخابی می‌تواند نوع فروش دیگری داشته باشد؛ [OrderService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412>).
- برای Vibe، `cart_amount` از مبلغ Payment گرفته می‌شود ولی `goods_amount`، مالیات و اقلام از **کل Order** ساخته می‌شوند؛ [VibeService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:68>) و [جزئیات ارسالی](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:370>). سازگاری درخواست Mixed با قرارداد Vibe هنوز تأیید نشده است.

Payment و مخزن Reservation هر دو از `YashilAppDbContext` استفاده می‌کنند و ثبت DbContext و UnitOfWork به‌صورت scoped است؛ [Startup.cs](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:465>). بنابراین تراکنش مشترک **از نظر معماری محتمل است، اما با متدهای فعلی که هرکدام تراکنش خود را باز و Commit می‌کنند، آماده استفاده نیست**. تا تعیین مرز مشترک، نباید Payment قابل Redirect پیش از Reserve موفق باقی بماند.

## برنامه پیشنهادی برای سمت کاربر و اتصال آن

پیشنهاد من این است که «استفاده از اعتبار WalletProgram» کنار انتخاب درگاه و به‌عنوان **سهم تأمین مبلغ** نمایش داده شود، نه به‌جای radio درگاه؛ گزینه قدیمی `Wallet` معنای دیگری دارد. در خلاصه پرداخت، کاربر در صورت داشتن Program واجد شرایط، استفاده از آن را فعال کند و پیش‌نمایش «بدهی سفارش `T`، سهم کیف پول `W`، مبلغ درگاه `G`» را ببیند. این پیش‌نمایش باید پس از تغییر Program یا درگاه تازه شود و **مبلغ نهایی را سرور هنگام ثبت تلاش دوباره اعتبارسنجی کند**؛ خلاصه سبد پیش از ساخت Order مبنای قطعی Reserve نیست.

برای پیاده‌سازی، ابتدا قرارداد backend همان تلاش را تکمیل می‌کنم: انتخاب Program و درگاه به درخواست برسد، پس از قطعی‌شدن Order مبلغ با روش پرداخت انتخابی محاسبه شود، Payment و Reservation با یک شناسه تلاش و مرز تراکنش روشن ساخته شوند، و فقط پس از موفقیت هر دو اجازه Redirect داده شود. Retry همان تلاش باید همان `PaymentId`، `W` و `G` را بازیابی کند؛ تلاش جدید شناسه مستقلی بگیرد. مسیر بدون Wallet همان ارسال و Redirect فعلی را حفظ می‌کند. سپس کنترل‌های [_OrderSummaryForPay.cshtml](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:159>)، درخواست `doFinalPay` و مدل خلاصه سفارش تغییر می‌کنند؛ مسیر ثمین و پرداخت از صفحه Detail نیز با همان قرارداد بررسی می‌شوند.

پیش از اجرا، دو تصمیم باید در طرح نهایی روشن شوند: **کاربر از میان چند Program واجد شرایط کدام را انتخاب می‌کند و آیا سهم Wallet خودکار تا سقف موجودی است یا مبلغ دلخواه دارد؛** و **درگاه‌هایی مانند Vibe که جزئیات کل سبد را می‌فرستند، پرداختِ فقط `G` را طبق قراردادشان می‌پذیرند یا خیر.** برای Vibe از روی کد فعلی نمی‌توان پذیرش این مغایرت را تأیید کرد. نمایش پیش‌نمایش و اتصال UI را نباید به‌عنوان حل‌شدن این دو قرارداد مالی و درگاهی تلقی کرد.

این نوبت فقط بررسی بود؛ هیچ فایل، تست یا Build تغییر یا اجرا نشد.

---

## user

# Files pasted by the user:

## "# Task: Finalize Mixed Wallet Payment Backend Design ## Role Act as a senior .N…": C:\Users\pc\.codex/attachments/6193d2d5-bfd2-4da8-82bb-c7a22a5fa32e/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن پیوست را می‌خوانم و تصمیم‌های آن را با مسیر Payment و Wallet که بررسی کردیم تطبیق می‌دهم. سپس خروجی همین مرحله را طبق محدوده‌ای که در پیوست مشخص شده آماده می‌کنم.

---

## assistant
در کد فعلی یک شکاف مالی قطعی دیده می‌شود: `Payment.Total` فقط مبلغ درگاه را در محاسبه بدهی پوشش می‌دهد، در حالی که Capture کیف پول در Ledger جدا ثبت می‌شود. اکنون مسیرهای Verify، Refund و ساخت درخواست درگاه‌ها را بررسی می‌کنم تا فرمول بدهی و محدودیت هر درگاه را با شواهد کد مشخص کنم.

---

## assistant
بررسی تراکنش‌ها نشان می‌دهد هر دو بخش از `YashilAppDbContext` استفاده می‌کنند، اما متدهای فعلی ساخت Payment و Reserve هرکدام تراکنش را مستقل شروع و نهایی می‌کنند. طرح اتمیک باید این دو عملیات را در یک orchestrator و یک تراکنش اجرا کند. همچنین مسیرهای Verify فعلی در چند درگاه سفارش را پیش از Capture کیف پول `Complete` می‌کنند؛ این ترتیب برای Mixed Payment قابل استفاده مستقیم نیست.

---

## assistant
# طرح نهایی Backend برای Mixed Wallet Payment

**نتیجه:** Entityهای فعلی برای پیوند دادن یک `Payment` به `WalletReservation` کافی‌اند، اما متدهای فعلی هنوز قرارداد Mixed Payment را اجرا نمی‌کنند. ساخت Payment و Reserve باید در یک مرز تراکنش بازطراحی شود و محاسبه بدهی پس از Capture باید سهم Wallet را جداگانه بشناسد. این گزارش صرفاً طراحی است؛ هیچ فایلی تغییر نکرد.

## ۱. جریان و محاسبه فعلی

`SaveShoppingCartToOrder` روش پرداخت انتخابی را دریافت می‌کند و پس از ثبت یا بررسی Order، فقط URL درگاه را برمی‌گرداند؛ در این مرحله Payment ساخته نمی‌شود. این رفتار برای سفارش جدید و سفارش بازشده از `Detail.cshtml` برقرار است. [OrderController.SaveShoppingCartToOrder](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:142>)، [انتخاب روش برای سفارش موجود](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:590>) و [GetRedirectUrl](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:745>).

در مسیر درگاه، `CreateNewPaymentAndReturnAsync` دوباره `CalcAmountForPayAsync` را اجرا می‌کند، **کل مبلغ بدهی** را در `Payment.Total` می‌گذارد و Payment را Commit می‌کند. محاسبه بدهی، مبلغ Paymentهای `Paid` را کم می‌کند؛ `GetOrderTotalPayment` بازپرداخت‌های ثبت‌شده در `RefundDetail` را نیز در مبلغ خالص این Paymentها لحاظ می‌کند. این محاسبه برای Order با وضعیت `Complete` اصلاً وارد شاخه محاسبه بدهی نمی‌شود. [OrderService.CalcAmountForPayAsync](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412>)، [CreateNewPaymentAndReturnAsync](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552>) و [PaymentService.GetOrderTotalPayment](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261>).

`Capture` اعتبار Wallet را به Ledger بدهکار تبدیل می‌کند؛ `Refund` یک Ledger بستانکار وابسته به همان Capture ثبت می‌کند. هیچ‌کدام در محاسبه فعلی بدهی Order وارد نمی‌شوند. بنابراین اگر `Payment.Total = G` باشد، پس از `Paid` و `Capture`، محاسبه فعلی فقط `G` را پرداخت‌شده می‌شناسد و `W` را بدهی باقیمانده می‌بیند. [WalletSettlementService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:230>)، [WalletRefundService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs:187>).

| وضعیت تلاش | اثر درست بر بدهی |
| --- | --- |
| Payment و Reservation تازه ساخته‌شده | هنوز پرداخت قطعی رخ نداده؛ `G` و `W` از بدهی کسر نمی‌شوند. |
| شکست یا لغو قطعی | Payment پرداخت‌شده نیست و Reservation آزاد می‌شود؛ تلاش بعدی بدهی را دوباره محاسبه می‌کند. |
| تأیید درگاه و Capture موفق | `G` از Payment موفق و `W` از Capture خالص همان Order کسر می‌شوند؛ `G + W = T`. |
| Refund | سهم Wallet برگشت‌داده‌شده دیگر Capture خالص نیست. اثر آن بر بدهی قابل پرداخت باید با تغییر مبلغ/وضعیت Order و قواعد Refund هماهنگ شود؛ Refund به‌تنهایی نباید خودکار Order تکمیل‌شده را دوباره قابل پرداخت کند. |
| Retry همان تلاش | `T/W/G` ثبت‌شده بازیابی می‌شود؛ قیمت و اعتبار دوباره برای آن تلاش محاسبه نمی‌شوند. |

**فرمول پیشنهادی برای Order قابل پرداخت:** مبلغ معتبر Order طبق روش پرداخت انتخابی، منهای Paymentهای موفقِ خالص از بازپرداخت، منهای `Capture − Refund` معتبر Wallet برای همان Order. شمارش Wallet باید از Ledger و پیوند `Reservation → Payment → Order` انجام شود؛ `Reserved` هنوز پرداخت نیست و `Released` اثری بر بدهی ندارد. این فرمول باید در یک مسیر مشخص Commerce اعمال شود و با رفتار `Complete` و تغییر مبلغ Order آزموده شود.

## ۲. هویت تلاش و Idempotency

`Payment.Id` و `Payment.ReservationNumber` تلاش ثبت‌شده را مشخص می‌کنند؛ `WalletReservation.PaymentId` آن را به Reservation وصل می‌کند. EF برای `IdempotencyKey` Reservation قید Unique دارد و Reserve فعلی کلید `WalletReserve:Payment:{PaymentId}` می‌سازد. [Mapping](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28097>) و [WalletReservationService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:60>).

این‌ها **برای تشخیص Retry درخواست اولیه پس از گم‌شدن پاسخ کافی نیستند**: پیش از دریافت `PaymentId`، کلاینت شناسه تلاش ندارد. `ShoppingCartParam` نیز شناسه‌ای حمل نمی‌کند و `Payment.Code` در ساخت فعلی مقدار ثابت `"1"` دارد؛ برای آن قید Unique دیده نمی‌شود. [ShoppingCartParam](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs:3>) و [ساخت Payment](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:560>).

قرارداد لازم این است: یک شناسه پایدار **برای درخواست ایجاد تلاش** به سرور برسد و با همان Order، کاربر، Application، روش پرداخت و Program تطبیق داده شود؛ Retry همان شناسه نتیجه همان Payment را برگرداند و درخواست صریحِ تلاش جدید شناسه دیگری داشته باشد. استفاده از `Payment.Code` موجود برای این منظور یک گزینه بدون تغییر Schema است، اما تغییر معنای آن و تضمین یکتایی در ارسال هم‌زمان باید پیش از انتخاب نهایی بررسی و تصویب شود. صرف `OrderId` یا جست‌وجوی آخرین Payment نمی‌تواند Retry را با تلاش جدید به‌طور قطعی تفکیک کند.

## ۳. تراکنش و بازیابی خطا

Payment و Wallet در همین `YashilAppDbContext` ثبت شده‌اند و DbContext و UnitOfWork به‌صورت scoped ثبت می‌شوند. پس **تراکنش دیتابیسی مشترک برای ثبت محلی قابل طراحی است**. با متدهای فعلی قابل ترکیب مستقیم نیست: سازنده Payment تراکنش خود را Commit می‌کند و Reserve نیز تراکنش مستقلی آغاز و Commit می‌کند. [Startup](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:465>)، [OrderService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:560>) و [WalletReservationService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:63>).

طرح محدود: یک orchestrator در Commerce، Order و روش پرداخت را اعتبارسنجی کند، `T/W/G` را یک‌بار تعیین کند، سپس در **یک تراکنش** Payment با `Total = G` و Reservation با `Amount = W` و همان `PaymentId` را ثبت کند. منطق داخلی ثبت Payment و Reserve باید نسخه قابل فراخوانی در تراکنش موجود داشته باشد؛ آغاز تراکنش تودرتو و Commit داخلی حذف شود. پیش از Commit، یکتایی تلاش، نبود Reservation فعال ناسازگار برای Order و کنترل شرطی موجودی بررسی شوند. Redirect فقط پس از Commit موفق مجاز است. Rollback، هر دو رکورد و تغییر مانده را برمی‌گرداند.

تراکنش دیتابیس نمی‌تواند درخواست شبکه‌ای درگاه را اتمیک کند. اگر پاسخ درخواست درگاه نامعلوم ماند، تلاش باید تا استعلام/نتیجه قطعی **در انتظار تعیین تکلیف** بماند؛ آن را خودسرانه ناموفق اعلام کردن یا تلاش تازه‌ای با Reserve جدید آغاز کردن امن نیست. پس از تأیید درگاه نیز ثبت `Paid`، `Capture` و نهایی‌شدن Order باید به‌صورت تکرارپذیر هماهنگ شود. مسیرهای Verify فعلی در چند درگاه، Order را پیش از Capture، `Complete` می‌کنند؛ برای Mixed قابل استفاده مستقیم نیستند، از جمله [Saman](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:325>) و [Ansar](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:199>). Capture فعلی نیز Payment با وضعیت `Paid` و Reservation منقضی‌نشده می‌خواهد؛ نتیجه تأیید دیرهنگام درگاه نیازمند سیاست بازیابی مشخص است. [WalletSettlementService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:156>).

## ۴. سازگاری درگاه‌ها

| مسیر | شاهد مبلغ درخواست | نتیجه برای Mixed |
| --- | --- | --- |
| Ansar، Saman، Saderat | مبلغ از `PaymentInformation.Total` به درخواست می‌رود؛ [Ansar](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:51>)، [Saman](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:89>)، [Saderat](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/SaderatPaymentController.cs:55>). | از نظر ساخت درخواست در کد، ارسال `G` ممکن به نظر می‌رسد؛ پذیرش پرداخت بخشی از Order توسط قرارداد بیرونی تأیید نشده است. |
| Hekmat و HekmatOnline | درخواست از مبلغ Payment و اطلاعات اقساط استفاده می‌کند؛ [Hekmat](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/HekmatPaymentController.cs:89>)، [HekmatOnline](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:421>). | مبلغ قابل تغییر است، اما قواعد اقساط و تأیید بیرونی باید جداگانه بررسی شوند. |
| Samin | `GetGenerateTokenParams` Payment تازه می‌سازد و مبلغ آن را به توکن می‌دهد؛ [OrderService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:1019>). | به مسیر تلاش آماده‌شده نیاز دارد؛ ساخت Payment دوم در این متد نباید رخ دهد. نوع خرید نیز باید حفظ شود. |
| Hybrid | مسیر اختصاصی دارد و حتی می‌تواند مبلغ را مستقل از Payment معمولی محاسبه کند؛ [HekmatPaymentController](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/HekmatPaymentController.cs:83>). | خارج از فعال‌سازی اولیه Mixed Wallet تا بررسی قرارداد اختصاصی. |
| Vibe | `cart_amount` از Payment، اما مبلغ کالا، مالیات و اقلام از کل Order می‌آیند؛ [VibeService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:68>) و [جزئیات](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:370>). | **غیرفعال بماند** تا قرارداد پرداخت جزئی و سازگاری همه مبالغ تأیید شود. |

این جدول تأییدیه درگاه‌ها نیست؛ فقط رفتار ساخت درخواست در Source را نشان می‌دهد.

## ۵. Full Wallet و برنامه اجرا

با `G = 0`، سازنده فعلی Payment مقدار غیرمثبت را رد می‌کند؛ Capture فعلی نیز `PaymentId` مربوط به Payment موفق می‌خواهد. مسیر قدیمی `PayWithWallet` یک Payment واقعی برای کارت باشگاه مشتریان می‌سازد و جایگزین Full Wallet مبتنی بر `WalletProgram` نیست. [OrderService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:555>)، [WalletSettlementService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:33>) و [مسیر قدیمی](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:802>). بنابراین Full Wallet به قرارداد نهایی‌سازی مستقل نیاز دارد و در این طرح اجرا نمی‌شود.

ترتیب حداقلی کار:

1. **Backend:** قرارداد شناسه تلاش و بازیابی Retry را قطعی؛ محاسبه بدهی شامل Capture خالص را با آزمون‌های Payment موفق قبلی، Refund و تغییر Order تعریف؛ سپس سازنده Payment و Reserve را زیر یک تراکنش مشترک قرار دهد.
2. **Backend درگاه:** هر مسیر مجاز، Payment آماده‌شده همان تلاش را مصرف کند و Payment دوم نسازد. مسیرهای Verify، وضعیت نهایی Payment، Capture/Release و نهایی‌شدن Order را با بازیابی خطای تکرارپذیر هماهنگ کنند. Vibe و Hybrid تا تأیید قرارداد خود فعال نشوند.
3. **UI:** در خلاصه سفارش، درگاه منتخب و Program منتخب/خودکار همراه `T/W/G` نمایش داده شوند؛ همان شناسه تلاش در Retry حفظ شود. Checkout جدید و پرداخت از `Detail.cshtml` هر دو پوشش داده شوند؛ مسیر بدون Wallet همان رفتار فعلی را نگه دارد.

**موارد واقعاً باز پیش از فعال‌سازی:** تضمین پایدار هویت درخواست اولیه با فیلدهای موجود؛ سیاست اثر Refund Wallet بر بدهی و Order تکمیل‌شده؛ بازیابی تأیید درگاه وقتی Capture به‌علت انقضای Reservation ممکن نیست؛ و تأیید قرارداد بیرونی هر درگاهی که برای Mixed فعال خواهد شد. همچنین `Payment.Total` دقت `decimal(18,8)` و Reservation دقت `decimal(18,2)` دارد، در حالی که چند درگاه مبلغ را به عدد صحیح تبدیل می‌کنند؛ واحد و قاعده گردکردن باید در قرارداد مبلغ آزموده و مشخص شود. [Payment mapping](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:16411>) و [Reservation mapping](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28112>).

**Self-check:** هیچ فایل یا دیتابیسی تغییر نکرد؛ هیچ پذیرش پرداخت جزئی از سوی درگاه‌ها فرض نشد؛ طرح، سهم Wallet را جدا از `Payment.Total` حساب می‌کند و Redirect را به موفقیت ثبت اتمیک وابسته می‌سازد.

---

## user
# Task: Analyze Mixed Wallet Payment Idempotency Without Schema Changes

## Role

Act as a senior .NET architect and programmer. Analyze the existing implementation and propose a safe, concrete strategy. **Analysis only: do not modify files, database schema, migrations, or tests.**

## Hard Constraints

- No Schema or Entity changes.
- No new database columns, indexes, or constraints.
- Do not invent status IDs or modify gateway contracts.
- Inspect actual source code and database mappings before drawing conclusions.
- If reliable idempotency cannot be guaranteed under these constraints, state that explicitly. Do not present best-effort behavior as a guarantee.

## Context

Mixed Wallet Payment requires:

- `T` = outstanding Order amount.
- `W` = automatically calculated Wallet contribution, capped at `T` and eligible available credit.
- `G = T - W`.
- `Payment.Total = G`; Reservation amount = `W`.
- Payment and Reservation must belong to the same attempt.
- Retrying the same attempt must recover the same Payment, Reservation, `W`, and `G`.
- A new attempt must be distinguishable from a retry.
- Concurrent duplicate requests must not create duplicate financial records or reserve credit twice.
- Payment and Reservation creation must be atomic where possible.
- No gateway redirect is allowed before the required records are safely committed.
- Schema changes are currently prohibited.

## Required Investigation

### 1. Existing identity and uniqueness

Inspect:

- `Payment` fields, especially `Id`, `Code`, `ReservationNumber`, Order/Application/User relationships, and existing unique constraints.
- `WalletReservation.PaymentId`, `IdempotencyKey`, its unique index, and all relevant EF mappings.
- Request DTOs and `ShoppingCartParam`.
- Existing request IDs, session/cart identifiers, transaction utilities, and concurrency protections.

For each candidate identity, explain whether it distinguishes:

1. a retry of the same request;
2. a deliberately new payment attempt for the same Order;
3. two concurrent requests for the same attempt;
4. requests from different users or Applications.

Do not assume `Payment.Code = "1"` is suitable or unique without checking all usages and database constraints.

### 2. Can the current schema provide a safe guarantee?

Evaluate possible approaches using existing fields only, such as:

- a stable attempt key supplied by the caller and stored in an existing field, if its semantics and uniqueness permit it;
- a server-side identifier already persisted before Payment creation;
- an existing unique Reservation key tied to a durable attempt identifier.

For each viable candidate, describe its collision risks, race conditions, compatibility with existing code, and whether uniqueness is enforced by the database.

Distinguish clearly between:

- idempotency within one already-created Payment;
- idempotency of the initial request before a PaymentId exists.

### 3. Transaction and concurrency

Inspect actual UnitOfWork/DbContext transaction boundaries and the Reserve/Payment creation methods.

Explain how the chosen approach would prevent:

- duplicate Payments;
- duplicate Reservations;
- double reservation of credit;
- a committed Payment without its Reservation;
- a committed Reservation without its Payment.

Account for concurrent requests, process crashes, and a lost response after database commit.

### 4. Final recommendation

Return:

1. The strongest safe approach possible without Schema changes.
2. Exact existing fields and methods it would use.
3. Whether the database itself can enforce uniqueness for the initial attempt.
4. Any residual race condition or limitation that cannot be eliminated.
5. If a reliable guarantee is impossible, the minimum future Schema change that would be needed—but only as a recommendation, not an implementation.
6. The safest next implementation step under the current restriction.

## Self-check

- No files, tests, migrations, or database objects were changed.
- No uniqueness guarantee was claimed without a supporting constraint or equivalent proven mechanism.
- Initial-request idempotency and post-Payment retry idempotency were evaluated separately.
- Existing checkout behavior and legacy payment flows were considered.


---

## assistant
این بررسی را فقط روی هویت تلاش پرداخت، قیدهای موجود و مرز تراکنش‌ها متمرکز می‌کنم. به‌خصوص جدا می‌کنم که قید یکتای Reservation چه چیزی را بعد از ساخته‌شدن Payment تضمین می‌کند و آیا همان قید می‌تواند درخواست اولیه را هم در برابر Retry و هم‌زمانی محافظت کند. هیچ فایلی را تغییر نمی‌دهم.

---

## assistant
قید یکتای موجود روی `WalletReservation.IdempotencyKey` می‌تواند برای Mixed Payment با `W > 0` نقش قید یکتای درخواست اولیه را هم داشته باشد، اما فقط اگر کلیدِ پایدارِ همان تلاش در آن ذخیره شود و Payment و Reservation در یک تراکنش Commit شوند. کد فعلی کلید را از `PaymentId` می‌سازد؛ بنابراین این تضمین در مسیر فعلی وجود ندارد. بررسی رقابت دو تلاش متفاوت برای یک Order را هم جداگانه جمع‌بندی می‌کنم.

---

## assistant
# نتیجه بررسی Idempotency پرداخت ترکیبی، بدون تغییر Schema

**نتیجه اصلی:** کد فعلی برای درخواست اولیه Mixed Payment تضمین Idempotency ندارد. با مدل **تعریف‌شده در EF** می‌توان برای تلاش‌های واقعاً ترکیبی (`W > 0` و `G > 0`) راهکاری قابل اتکا طراحی کرد: یک کلید پایدارِ تلاش در `WalletReservation.IdempotencyKey` ذخیره شود و ساخت Payment، Reservation و تغییر مانده در **یک تراکنش** انجام شود. این تضمین مشروط به وجود قید Unique متناظر در دیتابیسِ مستقر و عبور همه مسیرهای ساخت تلاش Mixed از همان جریان است؛ Schema واقعی محیط در این بررسی تأیید نشده است.

## هویت‌ها و قیدهای موجود

| گزینه | وضعیت و محدودیت |
| --- | --- |
| `Payment.Id` | شناسه قطعیِ Payment **پس از ایجاد آن** است. برای Replay همان Payment مناسب است، ولی درخواست اولیه‌ای را که هنوز `PaymentId` ندارد شناسایی نمی‌کند. |
| `Payment.ReservationNumber` | از پیشوند و `Payment.Id` ساخته می‌شود؛ آن هم فقط پس از Insert. در [Mapping Payment](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:16264>) قید Unique برای آن یا `Payment.Code` دیده نمی‌شود. [نحوه ساخت](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:547>). |
| `Payment.Code` | سازنده معمول Payment مقدار ثابت `"1"` می‌گذارد؛ پس در وضع فعلی کلید تلاش نیست و یکتایی دیتابیسی ندارد. [OrderService](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:560>). |
| `Order.Id` و `Order.Code` | سفارش را مشخص می‌کنند، نه چند تلاش متفاوت همان سفارش را. Payment از راه `OrderId` به Order وصل است و `UserId` مستقیم ندارد؛ مالکیت باید از `Payment → Order.UserId` کنترل شود. [Entity Order](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/Order.cs:22>) و [FK Payment](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:16455>). |
| `WalletReservation.PaymentId` | رابطه تلاش را فراهم می‌کند، ولی nullable است و در Mapping مرورشده قید Unique روی `PaymentId` ندارد. یک FK به‌تنهایی «حداکثر یک Reservation برای Payment» را تضمین نمی‌کند. [Entity](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs:62>) و [Mapping](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28207>). |
| `WalletReservation.IdempotencyKey` | فیلد اجباری با طول حداکثر ۱۰۰ و **Unique index سراسری** در مدل EF است. این تنها قید یکتای مناسبِ مشاهده‌شده برای درخواست اولیه Mixed با `W > 0` است. [Mapping](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28103>). |

کلیدهای فعلی این امکان را به کار نمی‌گیرند: `WalletReserveRequestDto` کلید دارد، اما `ReserveAsync` آن را با `WalletReserve:Payment:{PaymentId}` جایگزین می‌کند. `WalletOrderPreparationService` کلید را از `OrderId` می‌سازد و هنگام درخواست Reserve اصلاً `PaymentId` نمی‌فرستد، در حالی که Reserve آن را الزامی می‌داند. [ReserveAsync](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33>) و [PrepareAsync](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:64>).

شناسه سبد در Cookie نیز شناسه تلاش نیست. قفل `ProcessingShoppingCarts` یک دیکشنری static در حافظه پردازش است؛ بین چند instance یا پس از crash پایدار نمی‌ماند. ابزار `PaymentTransactionUtil` هم دیکشنری singleton در حافظه است. بررسی سفارش تکراری، جای قید یکتای تلاش را نمی‌گیرد. [قفل سبد](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:650>)، [ابزار تراکنش](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/Implementations/PaymentTransactionUtil.cs:5>) و [بررسی سفارش تکراری](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:972>).

## راهکار قوی‌تر با فیلدهای فعلی

برای **هر درخواست ایجاد تلاش Mixed**، یک کلید تصادفی پایدار و مجزا پیش از ارسال درخواست تولید و تا تعیین نتیجه همان تلاش نگهداری شود. Retry همان کلید را بفرستد؛ اقدام صریح برای تلاش جدید، کلید تازه داشته باشد. سرور کلید را با پیشوند مخصوص Mixed، در محدوده ۱۰۰ کاراکتر، در `WalletReservation.IdempotencyKey` ثبت کند؛ کلید باید به `ApplicationId`، `Order.UserId`، روش پرداخت، Program و همان تلاش مقید و هنگام Replay با آن‌ها تطبیق داده شود. کلید متعلق به کاربر یا Application دیگر باید **Conflict** بدهد، نه نتیجه آن کاربر را افشا کند.

ترتیب پیشنهادی در یک orchestrator Commerce:

1. با کلید، Reservation ثبت‌شده را جست‌وجو کند. اگر وجود داشت، رابطه `Reservation → Payment → Order` و ورودی‌های ثابت تلاش را بررسی و **همان `Payment.Id`، `Payment.Total = G` و `Reservation.Amount = W`** را بازیابی کند. برای این Replay نباید مبلغ Order یا موجودی Wallet دوباره مبنای تغییر `W/G` شود.
2. برای کلید جدید، دسترسی به همان Order را در یک تراکنش SQL Server **به‌صورت مشترک و قابل اتکا میان پردازش‌ها سریالی** کند؛ سپس کلید و وجود Reservation فعال ناسازگار را دوباره بررسی کند. قید یکتای کلید، درخواست‌های هم‌زمانِ *همان تلاش* را مهار می‌کند؛ سریالی‌کردن در سطح Order برای *دو کلید متفاوتِ هم‌زمان* لازم است. خواندن فعلیِ «Reservation فعال» به‌تنهایی قید یکتای دیتابیسی نیست. [بررسی فعال در Reserve](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:85>).
3. `T/W/G` را محاسبه کند، Payment با `Total = G` بسازد، `Payment.Id` را بگیرد، Reservation با همان `PaymentId` و کلید پایدار را ثبت کند و افزایش شرطی مانده‌های رزروشده را انجام دهد؛ سپس **یک‌بار Commit** کند. قید Unique روی `IdempotencyKey` سبب می‌شود از دو Insert هم‌زمان با یک کلید، حداکثر یکی Commit شود؛ تراکنشِ بازنده باید Payment و تغییر مانده خودش را Rollback کند و سپس رکورد برنده را برای Replay بخواند.
4. تنها بعد از Commit سازگار، اجازه مسیر درگاه بدهد. اگر فرآیند پیش از Commit متوقف شود، تراکنش باز می‌گردد؛ اگر پاسخ پس از Commit گم شود، همان کلید رکورد Commit‌شده را بازیابی می‌کند. شکست یا پاسخ نامعلومِ **خود درگاه** مسئله جداگانه‌ای است و نباید باعث ساخت Payment/Reservation تازه با همان کلید شود.

این طرح نمی‌تواند متدهای فعلی را پشت سر هم صدا بزند: [ساخت Payment](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552>) و [Reserve](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:63>) هرکدام تراکنش خود را باز و Commit می‌کنند. Payment و Repository رزرو از `YashilAppDbContext` استفاده می‌کنند و ثبت DbContext scoped است؛ بنابراین استخراج عملیات داخلیِ بدون Commit برای یک تراکنش مشترک مسیر فنی مناسبی است. اجرای داخلی `UnitOfWork` در Source بررسی‌شده موجود نبود؛ پیاده‌سازی نباید بر فرضِ پشتیبانی آن از تراکنش تودرتو تکیه کند. [ثبت DI](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:465>) و [WalletReservationRepository](</P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs:13>).

## مرز تضمین و گام بعد

- **برای درخواست اولیه Mixed با `W > 0`:** مدل EF یک قید Unique قابل استفاده روی Reservation دارد؛ بنابراین پس از تأیید وجود آن در دیتابیس مستقر، حفظ کلید توسط درخواست‌کننده، تراکنش مشترک و سریالی‌کردن تلاش‌های همان Order، می‌توان تضمین را در مسیر Mixed پیاده‌سازی کرد. **کد فعلی این تضمین را ندارد.**
- **برای `W = 0`:** Reservation ایجاد نمی‌شود؛ قید یکتای آن هیچ حفاظتی برای Payment عادی فراهم نمی‌کند. رفتار عادی فعلی حفظ می‌شود، اما نباید Idempotency سراسری Payment عادی را تضمین‌شده اعلام کرد.
- **برای Checkout از سبدِ هنوز بدون `OrderId`:** ثبت Order در `SaveShoppingCartToOrder` مرحله‌ای جداست و قفل سبدِ حافظه‌ای تضمین چندپردازشی برای یکتایی خود Order نیست. تضمین شرح‌داده‌شده از **Order ثبت‌شده به بعد** است. صفحه `Detail.cshtml` از ابتدا `OrderId` دارد.
- **برای مسیرهای قدیمی درگاه:** آن‌ها امروز در ورود به صفحه درگاه Payment تازه می‌سازند. اگر مسیر Mixed پس از آماده‌سازی دوباره به همان سازنده هدایت شود، طرح شکست می‌خورد. همچنین مسیر مستقلی که این قفل و کنترل‌ها را دور بزند می‌تواند تلاش دیگری برای همان Order بسازد. همه ورودی‌های ساخت Payment برای Order دارای تلاش Mixed باید از قرارداد تلاش آماده‌شده تبعیت کنند؛ جریان بدون Wallet می‌تواند رفتار فعلی را نگه دارد.

**تغییر Schema در آینده، فقط اگر تضمین برای Payment بدون Reservation یا همه مسیرها لازم شود:** یک کلید پایدار تلاش در Payment با Unique constraint در محدوده Application، و قید Unique روی `WalletReservation.PaymentId` برای Reservationهای دارای Payment، راه مستقیم‌تر و مستقل از مسیر برنامه است. این صرفاً پیشنهاد است؛ اکنون هیچ‌کدام ایجاد نشوند.

**امن‌ترین گام اجرایی بعدی تحت محدودیت فعلی:** نخست، وجود فیزیکی `UX_WalletReservation_IdempotencyKey` را در محیط هدف به‌صورت read-only تأیید و قرارداد کلید پایدار و محدوده تضمین آن را ثبت کنید؛ سپس orchestrator اتمیکِ محدود به `W > 0, G > 0` را طراحی و مسیرهای ساخت Payment قدیمی را از ایجاد Payment دوم برای همان تلاش محافظت کنید. تا آن زمان **Idempotency درخواست اولیه و جلوگیری از دوباره‌رزرو شدن در کد فعلی تضمین‌شده نیست**.

هیچ فایل، تست، Migration یا شیء دیتابیس تغییر نکرد.

---

## user

# Files pasted by the user:

## "# Feature: Atomic Mixed Wallet Payment Preparation ## Role Act as a senior .NET…": C:\Users\pc\.codex/attachments/393f46cf-1750-43c3-983c-1d52f164642d/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن Feature پیوست را می‌خوانم و آن را با قرارداد Idempotency و تراکنش‌های فعلی تطبیق می‌دهم. اگر دستور اجرا باشد، ابتدا فقط فایل‌ها و مسیرهای مرتبط را بررسی می‌کنم و سپس در همان محدوده پیش می‌روم.

---

## assistant
طبق شرط صریح پیوست، قبل از هر ویرایش باید وجود واقعی Unique index مربوط به `WalletReservation.IdempotencyKey` را در دیتابیس هدف با یک Query فقط‌خواندنی تأیید کنم. فعلاً فقط مسیر Configuration و ابزار اتصال موجود را بررسی می‌کنم؛ اگر اتصال قابل اتکا فراهم نباشد، همین‌جا متوقف می‌شوم و هیچ فایلی را تغییر نمی‌دهم.

---

## assistant
## Phase 1 متوقف شد — اتصال به دیتابیس هدف در دسترس نیست

در [EF Mapping](</P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28103>)، ایندکس `UX_WalletReservation_IdempotencyKey` به‌صورت Unique تعریف شده است. **این تعریف، وجود و فعال‌بودن ایندکس در دیتابیس هدف را تأیید نمی‌کند.**

برنامه اتصال `YashilAppDB` را از Configuration می‌خواند و رمزگشایی می‌کند؛ [Startup.cs](</P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:463>). در محیط فعلی، فایل `appsettings.json`، نسخه Development، کپی آن‌ها در خروجی برنامه، یا متغیر محیطیِ اتصال `YashilAppDB` پیدا نشد. بنابراین دیتابیس هدف مشخص نیست و Query فقط‌خواندنیِ metadata قابل اجرا نبود.

طبق شرط صریح Feature، **پیش از تأیید ایندکس واقعی هیچ Implementation انجام ندادم**. هیچ فایل، تست، Migration یا شیء دیتابیس تغییر نکرد؛ Build و تست نیز اجرا نشدند.

برای ادامه، دسترسی فقط‌خواندنی به دیتابیس هدف را در محیط فراهم کنید، یا خروجی metadata همان دیتابیس را برای ایندکس `UX_WalletReservation_IdempotencyKey` بفرستید؛ نیازی به ارسال رمز یا Connection String در گفتگو نیست.

---

## user
**Continue Mixed Wallet Payment Backend implementation.**

### Verified database prerequisite

The physical SQL Server database `IsiClubCommerceLocal` was checked in SSMS. The index `shp.WalletReservation.UX_WalletReservation_IdempotencyKey` exists with:

- `IsUnique = 1`
- `IsDisabled = 0`
- `HasFilter = 0`
- Key column: `IdempotencyKey`

Treat this prerequisite as verified for this database only. Do not claim that another environment was checked.

### Mandatory constraints

- Do not modify database schema, entities, migrations, or database objects.
- Do not add or modify indexes.
- Do not enable Mixed Payment for Vibe or Hybrid until their partial-payment contracts are verified.
- Full Wallet payment (`G = 0`) remains out of scope.
- Preserve the existing normal checkout/payment path.
- Do not introduce fake or zero-amount Payments.
- Do not claim successful build, tests, or runtime verification unless actually performed.

### Implementation requirements

Continue from the existing Mixed Wallet Payment specification and inspect the current source before editing.

1. Implement the mixed-payment attempt using a stable, caller-generated attempt key. Retries of the same attempt must reuse the same key and amounts; a new attempt must use a new key.
2. Bind the attempt to the relevant Application, User, Order, payment method, WalletProgram, and financial amounts. Reject conflicting reuse of an existing key.
3. Atomically create the gateway-remainder Payment (`Payment.Total = G`), create/reuse the WalletReservation (`Amount = W`), and update the required wallet balances. Reuse a shared database transaction; existing service methods that commit independently must not break atomicity.
4. Handle concurrent requests safely. If a unique-key race occurs, roll back the losing transaction and load/validate the winning reservation rather than creating another attempt.
5. Ensure the legacy gateway path does not create a second Payment after the mixed-payment attempt has already created one.
6. Reconcile the outstanding Order debt calculation with successful Payment amounts and net Wallet settlement (`Capture - Refund`). Do not introduce an inconsistent financial calculation merely to complete the feature.
7. If any requirement cannot be implemented safely without schema/entity changes, or if existing Order completion/refund behavior prevents a correct solution, stop before making a partial or unsafe financial change. Report the exact blocker and the smallest proposed follow-up change; do not silently weaken the requirement.

### Scope and validation

- Keep changes limited to Mixed Wallet Payment backend integration and the minimum necessary gateway-flow adjustments.
- Do not implement Full Wallet payment or frontend/UI changes in this task.
- Run the most relevant available checks. If build assets are missing (for example, `project.assets.json`), report that limitation clearly.
- Finish with a concise report: changed files, transaction/idempotency behavior, supported payment methods, checks actually run, and any unresolved blocker.
- Do not perform a broad final review of every previously implemented Wallet feature yet; that remains a single later task.


---

## assistant
تأیید ایندکس را فقط برای `IsiClubCommerceLocal` مبنا می‌گیرم. پیش از ویرایش، مسیرهای فعلی ساخت Payment، Reserve، Verify و محاسبه بدهی پس از Capture/Refund را دوباره با Source تطبیق می‌دهم. اگر این‌ها بدون تغییر Schema و بدون دست‌زدن به قراردادهای خارج از Scope به یک جریان مالی سازگار نرسند، طبق شرط شما پیش از هر تغییر متوقف می‌شوم.

---

## assistant
## Handoff: Atomic Mixed Wallet Payment

### Active request

Implement the approved Mixed Wallet Payment backend for `W > 0`, `G > 0`, subject to a strict self gate: **if correct debt accounting or Order completion/refund behavior cannot be handled safely within scope, stop before editing and report the exact blocker**. No UI, Full Wallet, Vibe, Hybrid, Schema, Entity, index, or migration changes. Preserve normal checkout. The user verified the physical unique index `shp.WalletReservation.UX_WalletReservation_IdempotencyKey` in `IsiClubCommerceLocal` (`IsUnique=1`, enabled, unfiltered, key `IdempotencyKey`); treat this as verified **only for that database**.

### Progress

No files have been edited. No build or tests have run. I informed the user that I am checking the financial self gate before editing.

Previous analysis established that a caller generated stable attempt key could be stored in the existing unique `WalletReservation.IdempotencyKey`, provided Payment, Reservation, and balance updates share one transaction. The current implementation does not yet do this:

- [OrderService.CreateNewPaymentAndReturnAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552) independently starts and commits a UnitOfWork transaction, stores the full outstanding amount in `Payment.Total`, and uses `Payment.Code = "1"`.
- [WalletReservationService.ReserveAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33) requires `PaymentId`, derives its own key `WalletReserve:Payment:{id}`, and independently begins/commits a DbContext transaction.
- [WalletOrderPreparationService.PrepareAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:58) calculates with `PaymentMethodTypesEnum.Wallet`, uses an Order based key, and calls Reserve **without** `PaymentId`; its present path cannot succeed.
- [Startup](/P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Startup.cs:465) registers `YashilAppDbContext` and `UnitOfWork<YashilAppDbContext>` as scoped; PaymentRepository and WalletReservationRepository use the same DbContext. Internal UnitOfWork source was not found.

### Critical financial evidence found this turn

- [OrderService.CalcAmountForPayAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412) only calculates for non-`Complete` Orders and subtracts `GetOrderTotalPayment`, which counts **Paid Payment.Total**, from Order totals (lines 434–447). It does **not** count net Wallet Capture. With `Payment.Total = G`, `W` remains outstanding unless a carefully scoped accounting change is made.
- [WalletRefundService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs:78) validates Capture via `Payment → Order`, then reduces Assignment and Program `ConsumedAmount` and inserts a positive Refund ledger linked to the same Payment (lines 140–203). It does not adjust Order total/status or Commerce `RefundDetail`.
- **Potential decisive blocker:** [ModifiedOrderItemService.ApplyModificationAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:430) changes Order totals and derives refund or `IncompletePayment` from **only Paid Payment.Total** (lines 447–486). Its `AddRefundAsync` allocates refunds by Payment method (lines 509–587); `AddWalletRefundDetail` (lines 597–630) concerns the **legacy** `PaymentMethodTypesEnum.Wallet`/Naja payment, not WalletProgram ledger Capture. A Mixed Payment using a normal gateway method has `Payment.Total = G`, while its `W` lives in a separate ledger. Thus existing Order modification/refund path would compute incorrect paid and refundable amounts for Mixed. Inspect its remaining method and callers enough to determine if a correct fix is possible within the approved “minimum necessary gateway flow and backend” scope, or stop **before editing** with exact evidence and smallest follow up.
- [WalletSettlementService.CaptureAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:33) requires `PaymentId`, a `Paid` Payment and a non-expired Reservation; it creates Capture ledger in its own Serializable transaction. Existing gateway Verify paths (e.g. [Saman](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:329), [Ansar](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:201), [Samin](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SaminPaymentController.cs:257)) set Order `Complete` and Payment `Paid` independently before any Wallet Capture. This is a second likely blocker for safely enabling a gateway without wider Verify changes.

### Immediate next steps

1. Finish the scoped check of `ModifiedOrderItemService` refund behavior and whether the user’s “stop if Order completion/refund prevents correct solution” applies. Do not edit until resolved.
2. If it is a genuine blocker, stop and report the exact symbols/lines, why `Payment.Total = G` plus net Capture cannot be safely accounted for by merely changing `CalcAmountForPayAsync`, and the smallest proposed follow up (a coordinated Commerce Order modification/refund and settlement contract). State that no code changed and no tests/build ran. Do not present a partial preparation endpoint as completed.
3. If a safe bounded fix actually exists, proceed with scoped implementation, then verify meaningful tests/build and report limits. Applicable repository instructions require a plan and approval for nontrivial work; the user expressly approved implementation in this turn.

Use absolute clickable file links in the Persian final response. Previous tool commands occasionally failed due PowerShell brace expansion or escaped quotes; use straightforward individual paths and `rg -F` where possible.

---

## assistant
ابتدا مسیر محاسبه بدهی، تکمیل سفارش و Refund را در کد فعلی بررسی می‌کنم تا مطمئن شوم قرارداد `T = W + G` بدون مغایرت مالی قابل پیاده‌سازی است. اگر مانع مستقلی وجود داشته باشد، طبق شرط شما پیش از هر ویرایش متوقف می‌شوم.

---

## assistant
برای این بررسی از راهنمای Wallet و Backend مخزن استفاده می‌کنم؛ مسیر آن‌ها نسبت به پوشهٔ فعلی یک سطح بالاتر است. اکنون رابطهٔ تغییر سفارش با پرداخت‌های موفق و Ledger کیف پول را دقیق‌تر تطبیق می‌دهم.

---

## assistant
یک مانع مالی در مسیر موجود پیدا شد: محاسبهٔ تغییر سفارش و Refund فقط `Payment.Total` را پرداخت‌شده می‌داند، در حالی که در پرداخت ترکیبی این مقدار فقط سهم درگاه (`G`) است. پیش از نتیجه‌گیری، مسیر فراخوانی این منطق و ترتیب ثبت `Paid` و `Capture` را هم بررسی می‌کنم.

---

## assistant
**پیاده‌سازی را پیش از هر ویرایش متوقف کردم.** قرارداد `Payment.Total = G` در مسیر فعلی تغییر سفارش و Refund نتیجهٔ مالی نادرست می‌دهد:

- [CalcAmountForPayAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412) فقط Paymentهای موفق را از بدهی سفارش کم می‌کند. [GetOrderTotalPayment](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) نیز سهم Capture شدهٔ Wallet را حساب نمی‌کند؛ بنابراین پس از پرداخت ترکیبی، `W` همچنان بدهی دیده می‌شود.
- [ApplyModificationAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:450) وضعیت سفارش و مبلغ Refund را از مجموع `Payment.Total`های موفق می‌سازد. [AddRefundAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:509) سهم WalletProgram را در تخصیص Refund ندارد؛ مسیر Wallet آن در [AddWalletRefundDetail](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:597) مربوط به روش پرداخت Wallet قدیمی است.
- در نمونهٔ [Saman Verify](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:327)، سفارش پیش از ثبت Payment موفق `Complete` می‌شود، در حالی که [CaptureAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:160) به Payment با وضعیت `Paid` نیاز دارد. ترتیب اتمیک تکمیل سفارش و Capture هنوز برقرار نیست.

**کوچک‌ترین کار بعدی:** قرارداد مشترک محاسبهٔ پرداخت خالص سفارش، شامل Payment موفق و `Capture − Refund` کیف پول، باید در محاسبهٔ بدهی و مسیر تغییر سفارش/Refund اعمال شود؛ سپس ترتیب تکمیل سفارش و Settlement برای درگاه‌های مجاز تعیین و پیاده‌سازی شود. بدون این کار، ایجاد اتمیک Payment و Reservation به‌تنهایی پرداخت ترکیبیِ صحیحی نمی‌سازد.

فایل تغییرکرده: **هیچ‌کدام**. رفتار Retry و Rollback پیاده‌سازی نشده و هیچ روش پرداختی برای Mixed فعال نشده است. بررسی ایستای مسیرهای بالا انجام شد؛ Build، تست و تأیید Runtime اجرا نشد. وجود ایندکس یکتا را فقط برای دیتابیس `IsiClubCommerceLocal` مطابق بررسی اعلام‌شدهٔ شما مبنا قرار دادم.

---

## user
### Task: Define a consistent order payment, wallet settlement, and refund contract

**Mode: Analysis only. Do not edit files.**

Before implementing Mixed Wallet Payment, analyze the existing financial calculations and propose the smallest safe change that makes Order debt, payment verification, wallet Capture/Release, Order completion, and Refund consistent.

#### Inspect

- `OrderService.CalcAmountForPayAsync`
- `PaymentService.GetOrderTotalPayment`
- `ModifiedOrderItemService.ApplyModificationAsync`
- `ModifiedOrderItemService.AddRefundAsync`
- `ModifiedOrderItemService.AddWalletRefundDetail`
- `WalletSettlementService.CaptureAsync` and `ReleaseAsync`
- Relevant gateway Verify/callback flows, including Saman and the other supported gateways
- Existing Payment, RefundDetail, Order status, and wallet transaction relationships

#### Required analysis

1. Identify the authoritative calculation for:
   - Current outstanding Order debt
   - Successful gateway payments net of refunds
   - Net Wallet settlement (`Capture - Refund`)
   - Refund allocation between gateway and WalletProgram
2. Trace the lifecycle for both ordinary and mixed payments:
   - Payment creation
   - Gateway redirect and verification
   - Wallet Capture/Release
   - Order completion
   - Order modification and Refund
3. Determine whether a shared financial calculation can be introduced without changing database schema or entities.
4. Determine how refunds should be allocated when an Order has both gateway and WalletProgram funding. Do not assume the allocation policy; derive it from existing business rules or explicitly identify the decision required.
5. Identify gateway-specific sequencing differences and whether Capture can be made consistent with existing transaction boundaries.
6. Check for double subtraction, double refund, stale Order status, and retries after uncertain gateway verification.

#### Constraints

- Do not edit any files.
- Do not change schema, entities, migrations, or database objects.
- Do not enable Mixed Payment for Vibe or Hybrid.
- Full Wallet payment is out of scope.
- Preserve existing payment behavior unless a specific inconsistency is demonstrated.

#### Deliverable

Return:

1. A concise current-flow map with file/method references.
2. The proposed shared financial contract and formulas.
3. The minimum files/methods that would need changes.
4. Decisions that require business approval, especially refund allocation and Order completion.
5. Risks, edge cases, and recommended implementation sequence.
6. Whether the design is achievable under the no-schema/no-entity-change constraint.

Do not begin implementation until the analysis identifies a coherent financial contract and all blocking business decisions are resolved.


---

## assistant
مسیرهای محاسبهٔ بدهی، Verify درگاه‌ها، Settlement و Refund را در کد فعلی کنار هم می‌گذارم. خروجی فقط قرارداد پیشنهادی و تصمیم‌های باز خواهد بود؛ هیچ فایلی را تغییر نمی‌دهم.

---

## assistant
تا اینجا مدل اجازه می‌دهد سهم درگاه از `Payment` و سهم کیف پول از Ledger وابسته به همان `PaymentId` خوانده شود، اما مسیر Refund موجود فقط `RefundDetail`های وابسته به Payment را می‌شناسد. اکنون اختلاف ترتیب Verify در درگاه‌ها و مرز تراکنش‌ها را مشخص می‌کنم تا پیشنهاد، موفقیت درگاه و Capture را اشتباهاً یک عملیات اتمیک فرض نکند.

---

## assistant
## 1. نقشهٔ جریان فعلی

| مرحله | رفتار مشاهده‌شده |
|---|---|
| ایجاد Payment | [CreateNewPaymentAndReturnAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552) مبلغ را از `CalcAmountForPayAsync` می‌گیرد و Payment را در تراکنش خودش می‌سازد. شروع پرداخت Saman و Ansar آن را فراخوانی می‌کند؛ مسیر Samin از [GetGenerateTokenParams](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:1019) به همان متد می‌رسد. |
| بدهی سفارش | [CalcAmountForPayAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412) برای سفارش غیر `Complete`، Paymentهای موفق را کم می‌کند. [GetOrderTotalPayment](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) سهم WalletProgram را نمی‌خواند و `NewPaymentAmount` را بدون فیلتر وضعیت نهایی RefundDetail اعمال می‌کند. برای سفارش `Complete` نیز محاسبهٔ بدهی انجام نمی‌شود. |
| Verify | Saman، Ansar، Saderat، Samin و HekmatOnline در مسیر موفق، ابتدا Order را `Complete` و سپس Payment را `Paid` می‌کنند؛ نمونه‌ها: [Saman](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:327)، [Ansar](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:199)، [Saderat](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/SaderatPaymentController.cs:209)، [Samin](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SaminPaymentController.cs:255) و [HekmatOnline](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:639). این مسیرها Wallet Capture را اجرا نمی‌کنند. |
| Settlement | [CaptureAsync و ReleaseAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:33) در تراکنش داخلی خودشان مانده‌ها را تغییر می‌دهند. Capture به Payment با وضعیت `Paid` نیاز دارد و پس از انقضای Reservation رد می‌شود؛ Release وضعیت `Reserved` را آزاد می‌کند، اما موفق یا ناموفق بودن Payment را شرط نمی‌کند. |
| تغییر سفارش و Refund | [ApplyModificationAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:430) مبلغ پرداخت‌شده را فقط از `Payment.Total`های موفق می‌سازد. [AddRefundAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:509) Refund را بین Paymentها توزیع می‌کند. [AddWalletRefundDetail](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:597) مربوط به روش پرداخت Wallet قدیمی است، نه Capture کیف پول جدید. [WalletRefundService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs:77) Ledger جبرانی را ثبت می‌کند، ولی Commerce `RefundDetail` یا وضعیت Order را تغییر نمی‌دهد. |

## 2. قرارداد مالی پیشنهادی

این فرمول‌ها **طرح پیشنهادی‌اند، نه رفتار فعلی کد**:

- `O`: مبلغ فعلی سفارش، با همان انتخاب قیمت مبتنی بر Subsidy که Commerce امروز انجام می‌دهد.
- `P`: مجموع `Payment.Total`های `Paid`، پس از کسر **Refundهای واقعاً نهایی‌شدهٔ همان Payment**. Refund در وضعیت ثبت یا انتظار نباید زودتر از تحقق مالی از `P` کم شود.
- `W`: مجموع Captureهای معتبر WalletProgram وابسته به Paymentهای این Order، منهای Refundهای Ledger که به همان Captureها ارجاع دارند.
- `Debt = max(0, O − P − W)`؛ مقدار `P + W − O` نیز جداگانه به‌عنوان اضافه‌پرداخت/تعهد Refund گزارش شود، نه اینکه با `max` پنهان بماند.

برای یک تلاش Mixed، قبل از پرداخت `T = Debt` و `T = G + W_reservation` است. Reservation هنوز پرداخت قطعی نیست و وارد `W` نمی‌شود. پس از Verify موفق و Capture، `G` در `P` و سهم کیف پول در `W` قرار می‌گیرد. `Payment.Total` فقط `G` است؛ `RefundDetail` درگاه نیز نباید سهم Wallet را دوباره کم کند.

روابط فعلی `Payment → Order`، `WalletReservation → Payment` و `WalletTransaction → Payment/Reservation` برای **محاسبهٔ مشتق‌شده** کافی به نظر می‌رسند؛ [Entityها](/P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletTransaction.cs:61) و [Mapping](/P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28455) این مسیر را دارند. این مشاهده، صحت تراکنش اجرایی یا وضعیت دیتابیس‌های دیگر را اثبات نمی‌کند.

## 3. حداقل تغییرات لازم در مرحلهٔ پیاده‌سازی

1. یک محاسبهٔ مالی مشترک برای `O/P/W/Debt` در لایهٔ Commerce؛ مصرف آن در [CalcAmountForPayAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412)، [GetOrderTotalPayment](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) و [ApplyModificationAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:430). محاسبهٔ Refund قبلی در `ApplyModificationAsync` باید هم‌زمان بازنویسی شود تا Refund نهایی دوبار کسر نشود.
2. مسیر تخصیص Refund در [AddRefundAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:509) و هماهنگی آن با [WalletRefundService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs:32)، پس از تعیین سیاست Business.
3. یک مسیر نهایی‌سازی ویژهٔ Payment دارای Reservation در Verify درگاه‌های مجاز: ثبت `Paid`، Capture و تعیین وضعیت Order بر مبنای بدهی، با یک مرز تراکنش محلی و بدون آغاز/Commit مستقل در سرویس‌های داخلی. مسیر پرداخت عادی دست‌نخورده بماند. فراخوانی درگاه بیرون از تراکنش دیتابیس است و به منطق Retry/بازیابی نتیجه نیاز دارد.

## 4. تصمیم‌های Business باز

- **تخصیص Refund ترکیبی:** وقتی هر دو منبع پرداخت کرده‌اند، Refund ابتدا به Wallet برگردد، ابتدا به درگاه، یا به نسبت سهم‌ها تقسیم شود؟ ترتیب فعلی `Naja → درگاه‌ها → Hekmat` در [AddRefundAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:557) حکمی برای WalletProgram نمی‌دهد.
- **زمان `Complete` شدن Order:** پیشنهاد فنی این است که در تلاش Mixed فقط پس از `Paid` شدن درگاه، Capture موفق و صفر شدن بدهی، `Complete` ثبت شود. این تغییر رفتار نیازمند تأیید است.
- **Verify موفق همراه با Capture ناموفق یا Reservation منقضی:** پول درگاه ممکن است دریافت شده باشد؛ تبدیل Payment به `Unsuccessful` یا Release خودکار، واقعیت مالی را مخدوش می‌کند. سیاست بازیابی، Refund درگاه یا رسیدگی عملیاتی باید تعیین شود.
- **Refund Wallet مستقل:** آیا Wallet Refund بدون تغییر/Refund سفارش مجاز است؟ سرویس فعلی چنین امکانی دارد؛ در صورت مجاز بودن، وضعیت و بدهی Order باید با آن سازگار شوند.

## 5. ریسک‌ها و ترتیب پیشنهادی

ریسک‌های مشخص عبارت‌اند از دوباره‌کسرشدن Refund، باقی‌ماندن `W` به‌عنوان بدهی، `Complete` شدن زودهنگام Order، Release پس از Verify نامطمئن، و Retry پس از موفقیت بانک ولی خطای ذخیره‌سازی. در [Saman](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:344) خطای پس از Verify موفق حتی می‌تواند به تلاش برای ثبت `Unsuccessful` برسد؛ بنابراین پاسخ نامطمئن باید از شکست قطعی درگاه جدا شود.

ترتیب اجرا: **تصویب تصمیم‌های بالا → محاسبهٔ مشترک و آزمون حالت عادی/ترکیبی/Refund → هماهنگی تغییر سفارش و Refund → نهایی‌سازی و بازیابی Verify در هر درگاه مجاز → فعال‌سازی Mixed**. Vibe، Hybrid و Full Wallet در این طرح فعال نمی‌شوند.

**امکان‌پذیری:** محاسبه و اتصال رکوردهای مالی با Entityهای موجود، بدون تغییر Schema، قابل طراحی است. اجرای *منسجم* Mixed هنوز به تصمیم‌های Business بالا و اثبات مرز تراکنش و بازیابی Verify وابسته است؛ در وضعیت فعلی آمادهٔ پیاده‌سازی نیست. هیچ فایلی تغییر نکرد و چون کار صرفاً تحلیل بود، Build و تست اجرا نشد.

---

## user
<environment_context>
  <current_date>2026-10-13</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user
### Task: Finalize the Mixed Wallet Payment implementation plan

**Mode: Analysis only. Do not edit files.**

Use the following approved business decisions:

1. **Refund allocation:** Refund the gateway-funded portion first, then return the remaining refundable amount to WalletProgram.
2. **Order completion:** A Mixed Payment Order can become `Complete` only after the gateway Payment is `Paid`, Wallet Capture is confirmed successful, and outstanding Order debt is zero.
3. **Capture failure after successful gateway Verify:** Preserve the successful gateway Payment and retry Wallet Capture safely. If Capture definitively cannot succeed after the defined retry/recovery process, release the reservation only after confirming that Capture was not committed, and move the Order to the existing partial-payment state if such a state and transition are supported. Recalculate the remaining debt so the user can pay only the actual outstanding amount.
4. **Independent WalletProgram refund:** Ledger-based Wallet credit restoration is allowed. For full Order cancellation or Order-item modification/replacement, use the existing `ModifiedOrderItemService` business flow to calculate the refund. Record the actual customer-money refund through the existing Payment/RefundDetail mechanism where applicable, and restore the Wallet-funded portion through WalletRefund/Ledger. Never refund the same amount twice.

### Required analysis

Inspect the actual code and determine:

- Whether an existing Order status represents partial payment and whether the current Order lifecycle supports transitioning into and out of it.
- How to distinguish definitive Capture failure from an unknown outcome after timeout, exception, or lost response.
- How to safely retry Capture and detect a previously committed Capture before releasing a Reservation.
- How the gateway-first refund policy maps to existing `RefundDetail` statuses, refund amounts, and Payment allocation logic.
- How Wallet refunds should be linked to the correct Capture and Order modification, avoiding duplicate refunds and double subtraction from outstanding debt.
- How the shared Order financial calculation should account for:
  - Successful gateway Payments minus finalized gateway Refunds
  - Wallet Capture minus Wallet Refund
  - Open Reservations, which are not completed payments
  - Current Order amount and overpayment/refund obligations
- How ordinary checkout, Order modification, cancellation, and each proposed Mixed Payment gateway flow interact.
- Whether all requirements can be implemented without changing schema, entities, migrations, or database objects.
- Which gateway flows can safely support Mixed Payment. Keep Vibe and Hybrid disabled unless their partial-payment contracts are verified; Full Wallet remains out of scope.

### Constraints

- No file edits, schema changes, entity changes, migrations, or database object changes.
- Do not invent a new Order status. If no suitable existing status exists, report that as a blocker and propose the smallest follow-up change separately.
- Do not mark a gateway Payment unsuccessful merely because Wallet Capture failed.
- Do not release a Reservation while Capture outcome remains uncertain.
- Preserve the existing normal payment path.
- Do not claim that build, tests, or runtime checks were performed unless they actually were.

### Deliverable

Return the final implementation sequence, exact files/methods to change, transaction boundaries, idempotency and recovery strategy, refund allocation rules, supported gateway list, required tests, and any blockers that prevent a safe implementation under the current constraints.

Do not begin implementation until this plan is reviewed and explicitly approved.


---

## assistant
وضعیت‌های موجود Order، جزئیات Refund و مسیرهای Verify را با تصمیم‌های جدید تطبیق می‌دهم. سپس توالی اجرا و مرزهای تراکنش را مشخص می‌کنم و هر مانع باقی‌مانده را با ارجاع به کد گزارش می‌دهم. هیچ فایلی تغییر نمی‌کند.

---

## assistant
`IncompletePayment` در کد وجود دارد و گذار از `Complete` به آن و بازگشت به `Complete` در Service پشتیبانی می‌شود. با این حال، محاسبهٔ فعلی Refund قبل از ثبت سهم Ledger کیف پول تصمیم می‌گیرد؛ برای Plan باید روشن شود کدام عملیات Refund صرفاً «درخواست» است و کدام واقعاً نهایی شده است.

---

## assistant
در کد فعلی، `RefundDetail.Done` هنگام گروه‌بندی Refund ثبت می‌شود و مسیر جداگانه‌ای برای گروه‌بندی پرداخت وجه به مشتری وجود دارد. بنابراین در Plan، «ثبت درخواست Refund»، «نهایی‌شدن رکورد» و «بازگشت واقعی وجه» را جدا می‌کنم؛ یکسان گرفتن آن‌ها می‌تواند سهم Wallet را زودتر از موعد برگرداند.

---

## assistant
# برنامهٔ نهایی پیشنهادی برای Mixed Wallet Payment

**وضعیت:** قرارداد مالی با Entityهای موجود قابل طراحی است، اما فعال‌سازی هیچ درگاهی هنوز ایمن نیست. معنای «Refund نهایی» در مسیر پرداخت وجه و راه بازیابی نتیجهٔ نامطمئن Verify باید برای هر درگاه تثبیت شود. این برنامه برای بررسی و تأیید شماست؛ پیاده‌سازی آغاز نشد.

## قرارداد مشترک مالی

برای هر Order، محاسبه از رکوردهای همان Application و همان Order انجام شود:

| مقدار | تعریف |
|---|---|
| `O` | مبلغ فعلی Order با همان قاعدهٔ Subsidy موجود |
| `P` | مجموع `Payment.Total`های `Paid`، منهای سهم **واقعاً نهایی‌شدهٔ** Refund هر Payment |
| `W` | مجموع Captureهای معتبر WalletProgram، منهای Wallet Refundهای وابسته به همان Capture |
| `Debt` | `max(0, O − P − W)` |
| `Overpayment` | `max(0, P + W − O)`؛ تعهد قابل بررسی برای Refund |

Reservation باز در `W` شمرده نمی‌شود. هنگام ایجاد تلاش Mixed، `T` بدهی همان لحظه، `W_attempt` مبلغ Reservation و `G = T − W_attempt > 0` است؛ `Payment.Total = G`. مقادیر همان تلاش در Retry از Payment و Reservation بازیابی می‌شوند و دوباره قیمت‌گذاری نمی‌شوند.

امروز [CalcAmountForPayAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412) فقط پرداخت‌های موفق را کم می‌کند و برای Order با وضعیت `Complete` بدهی را محاسبه نمی‌کند. [GetOrderTotalPayment](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) همچنین `NewPaymentAmount` همهٔ RefundDetailها را، بدون محدودکردن به `Done`، لحاظ می‌کند. این دو مسیر باید به محاسبهٔ مشترک متصل شوند؛ محاسبهٔ فعلی نباید صرفاً با کم‌کردن دوبارهٔ مبلغ Wallet وصله شود.

## وضعیت Order و نهایی‌سازی

`OrderStatusTypesEnum.IncompletePayment = 1131` وجود دارد. [UpdateOrderStatusAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:223) گذار `Complete → IncompletePayment` و `IncompletePayment → Complete` را می‌پذیرد؛ [ApplyModificationAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:450) نیز از هر دو استفاده می‌کند.

برای Verify موفقِ یک تلاش Mixed، توالی محلی پیشنهادی این است:

1. نتیجهٔ قطعی درگاه را برای **همان Payment** احراز کنید. تا وقتی نتیجه نامعلوم است، Payment را `Unsuccessful` نکنید و Reservation را آزاد نکنید.
2. در یک تراکنش محلی، Payment را `Paid` و Order را، تا پایان Capture، `IncompletePayment` ثبت کنید. موفقیت بانکی حتی اگر Capture شکست بخورد حفظ می‌شود.
3. Capture را با کلید ثابت وابسته به `PaymentId/ReservationId` اجرا کنید. تغییر Reservation، مانده‌ها و Ledger باید اتمیک باشد. در همان مرز تراکنش، بدهی را دوباره بخوانید و فقط اگر صفر است Order را `Complete` کنید.
4. اگر نتیجهٔ Capture نامطمئن شد، ابتدا وضعیت Reservation و Ledger همان Payment را دوباره بخوانید و سپس با همان کلید Retry کنید. وجود Capture معتبر به معنی موفقیت قبلی است؛ نبود پاسخ به معنی شکست قطعی نیست.
5. فقط پس از احراز عدم ثبت Capture و شکست قطعیِ قابل بازیابی، Reservation را Release کنید، Payment را `Paid` نگه دارید و Order را `IncompletePayment` بگذارید. بدهی جدید بر اساس `O − P` محاسبه می‌شود. تا تعیین تکلیف این وضعیت، تلاش جدید برای همان Order نباید از اعتبار رزروشدهٔ قبلی عبور کند.

[WalletSettlementService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:53) اکنون تراکنش مستقل باز می‌کند؛ Capture به `Paid` و Reservation منقضی‌نشده نیاز دارد و Replay را از Ledger و وضعیت Reservation تشخیص می‌دهد. Release فعلی شرطی دربارهٔ وضعیت Payment ندارد. هر دو مسیر برای این توالی به هماهنگی تراکنش و گاردهای مخصوص Payment موفق نیاز دارند.

## Refund با اولویت درگاه

در تغییر یا لغو Order، ابتدا مبلغ قابل Refund از **منابع پرداخت‌شدهٔ خالص** و مبلغ جدید Order محاسبه شود. سپس:

`GatewayRefundAllocation = min(RefundDue, gateway-funded refundable balance)`  
`WalletRefundAllocation = RefundDue − GatewayRefundAllocation`

سهم اول فقط از طریق Payment و RefundDetail همان درگاه ثبت شود؛ سهم دوم با `WalletRefundService` به Capture همان Payment و Order وصل شود. `WalletRefundService` اکنون `ParentId = CaptureTransactionId`، `PaymentId` و `WalletReservationId` را ثبت و سقف Refundهای قبلی را کنترل می‌کند ([کد](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs:119)). برای عملیات ناشی از تغییر Order، کلید Refund باید از شناسهٔ پایدار همان تغییر و Capture ساخته شود تا Retry سهم Wallet را دوباره برنگرداند.

مسیر فعلی [AddRefundAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:509) ابتدا روش Wallet قدیمی را بررسی می‌کند و سپس RefundDetail درگاه‌ها را می‌سازد. [AddWalletRefundDetail](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:597) به WalletProgram مربوط نیست. محاسبهٔ `totalPayment − previousRefundAmount − newOrderTotal` در [ApplyModificationAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:451) نیز باید با `P + W` و Refundهای هر منبع بازطراحی شود تا کسر مضاعف رخ ندهد.

**نکتهٔ مسدودکننده برای اجرای Refund:** `RefundDetail` با وضعیت `Register` ساخته می‌شود؛ [گروه‌بندی Refund](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:156) آن را `Done` می‌کند، در حالی که [گروه‌بندی پرداخت وجه به مشتری](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:89) مسیر جداگانه‌ای دارد. از کد بررسی‌شده نمی‌توان نتیجه گرفت که `Done` به‌تنهایی به معنی واریز واقعی وجه است. پیش از تعیین لحظهٔ کسر Refund از `P`، این قرارداد عملیاتی باید تأیید شود. Wallet Refund مستقل مجاز می‌ماند، ولی برای تغییر/لغو Order باید در همان محاسبه و هماهنگی Refund لحاظ شود.

## فایل‌ها و ترتیب اجرا پس از تأیید

1. محاسبهٔ مشترک و آزمون‌های آن: [OrderService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412)، [PaymentService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) و قراردادهای Service متناظر. پرداخت عادی باید همان مبلغ قبلی را برگرداند.
2. تغییر سفارش و تخصیص Refund: [ModifiedOrderItemService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:430)، [RefundDetailRepository](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:77) و [WalletRefundService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs:32). ثبت تغییر Order و اثر Ledger محلی باید مرز تراکنش مشترک داشته باشند؛ پرداخت واقعی وجه درگاه، عملیات بیرونی و نیازمند بازیابی جداگانه است.
3. آماده‌سازی تلاش Mixed: [CreateNewPaymentAndReturnAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552) و [WalletReservationService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33). مسیر Mixed باید Payment با مبلغ `G` و Reservation با مبلغ `W_attempt` را در **یک تراکنش دیتابیس** بسازد؛ Commit مستقل فعلی این متدها برای آن مسیر قابل استفاده نیست. کلید پایدار تلاش در ایندکس یکتای موجود `WalletReservation.IdempotencyKey` نگه داشته و هنگام Replay، Application/User/Order/روش پرداخت/Program/مبالغ تطبیق داده شود. تأیید فیزیکی این ایندکس فقط برای `IsiClubCommerceLocal` اعلام شده است.
4. Verify و بازیابی: مسیرهای درگاه به نهایی‌سازی مشترک مخصوص Payment دارای Reservation متصل شوند. مسیر بدون Reservation همان رفتار عادی را حفظ کند.

## درگاه‌ها و آزمون‌های لازم

**هیچ درگاهی اکنون برای Mixed فعال یا تأیید Runtime نشده است.** Saman، Ansar و Saderat نامزد مرحلهٔ نخست‌اند: درخواست توکن آن‌ها مبلغ ورودی را می‌گیرد؛ نمونه‌ها [Saman](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:576)، [Ansar](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/BaseAnsarPaymentController.cs:159) و [Saderat](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/BaseSaderatPaymentController.cs:32). این مشاهده قرارداد کامل پرداخت جزئی یا بازیابی Verify را اثبات نمی‌کند. Samin و HekmatOnline به دلیل پارامترهای اقساط و مسیر Verify متفاوت، نیازمند بررسی جداگانه‌اند. Vibe، Hybrid و Full Wallet خارج از فعال‌سازی این Plan هستند.

آزمون‌ها باید حداقل پرداخت عادی، `T = G + W`، Retry و درخواست هم‌زمان، Rollback ایجاد Payment/Reservation، Verify تکراری یا با پاسخ گم‌شده، انقضای Reservation، شکست و Replay Capture، گذارهای `IncompletePayment/Complete`، Refund درگاه‌اول، Refund جزئی و تکراری، تغییر/لغو Order، و جلوگیری از کسر یا Refund مضاعف را پوشش دهند. آزمون اتصال برای هر درگاه، به‌خصوص نتیجهٔ Verify پس از قطعی ارتباط، لازم است.

**جمع‌بندی امکان‌پذیری:** مدل فعلی برای محاسبه و ارتباط Payment با Reservation و Ledger، تغییر Schema را الزام نمی‌کند. اجرای ایمن همچنان به تأیید معنای Refund نهایی و روش احراز نتیجهٔ بانکی پس از Verify نامطمئن برای هر درگاه وابسته است؛ هیچ کدام را از روی کد حدس نمی‌زنم. هیچ فایلی تغییر نکرد؛ Build، تست و بررسی Runtime انجام نشد.

---

## user
### Task: Trace and establish the actual Refund completion contract

**Mode: Read-only analysis. Do not edit files.**

For the Mixed Wallet Payment project, determine exactly when a gateway Refund becomes financially final and whether `RefundDetail.Status = Done` proves that money was actually returned to the customer.

#### Inspect

- `RefundDetailRepository`, especially refund registration and grouping paths.
- `ModifiedOrderItemService.AddRefundAsync`, `ApplyModificationAsync`, and related refund methods.
- All callers that transition `RefundDetail` between `Register`, `Done`, and any other statuses.
- Gateway-specific refund request, verification, callback, and reconciliation paths.
- Existing logs, transaction identifiers, idempotency keys, and persistence of gateway refund outcomes.

#### Questions to answer

1. What does each RefundDetail status mean in actual business and financial terms?
2. Which code path sends the actual refund request to the payment provider?
3. Is `Done` set before or after provider confirmation?
4. If the provider request times out, how can the system determine whether the refund succeeded?
5. Can a refund be retried without sending the same refund twice?
6. Which amount should be subtracted from a Payment when calculating net successful payments, and at which exact state transition?
7. How does the existing process distinguish a registered refund, an accepted refund, a completed refund, and a failed or uncertain refund?
8. What is the smallest safe implementation compatible with the current schema and entities?

#### Constraints

- Do not edit files or database objects.
- Do not infer provider behavior from status names alone.
- Cite concrete file/method references for every important conclusion.
- If the code does not prove actual customer-money transfer, explicitly report the missing evidence and the required business/provider confirmation.

#### Deliverable

Return a concise state-transition table, the exact financial-finality rule supported by the code, unresolved uncertainties, and a recommendation for integrating RefundDetail with WalletProgram refunds without double-refunding or double-subtracting amounts.


---

## assistant
مسیر ایجاد RefundDetail تا گروه‌بندی، ارسال برای پرداخت و ثبت نتیجه را در کد دنبال می‌کنم. نتیجه را بر اساس شواهد همین Repository می‌دهم و اگر تأیید واریز به مشتری در کد ثبت نمی‌شود، آن را صریحاً جدا می‌کنم. هیچ فایلی تغییر نمی‌کند.

---

## assistant
بررسی مسیرهای درگاه نشان می‌دهد RefundDetailهای آنلاین در این Source مستقیماً درخواست Refund به درگاه نمی‌فرستند؛ مسیر موجود بیشتر ثبت و گروه‌بندی برای پرداخت وجه است. یک مسیر جدا برای تراکنش‌های حکمت پیدا شده که باید از RefundDetail سفارش تفکیک شود، چون صرف وجود متد Refund در آن به معنی نهایی‌شدن بازپرداخت سفارش نیست.

---

## assistant
## نتیجهٔ اصلی

**`RefundDetail.Status = Done` اثبات نمی‌کند که پول به مشتری برگشته است.** در Source بررسی‌شده، `Done` هنگام گروه‌بندی RefundDetail ثبت می‌شود؛ این مسیر پاسخ موفق درگاه یا رسید واریز را بررسی نمی‌کند ([RefundDetailRepository.AssignItemGroupForRefundDetailAndConfirmAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:156)). بنابراین لحظهٔ نهایی‌شدن مالی Refund در Repository فعلی **قابل احراز نیست**.

## گذارهای قابل مشاهده

| وضعیت یا رویداد | آنچه کد ثابت می‌کند | آنچه ثابت نمی‌کند |
|---|---|---|
| `Register` | درخواست RefundDetail در [ModifiedOrderItemService.CreateNewRefundDetailEntity](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:818) ثبت شده است. | ارسال درخواست یا بازگشت وجه |
| `SendForModifyPrice` | مقدار در [Enum](/P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Enums/YashilEnums.cs:35) تعریف شده است؛ در مسیرهای بررسی‌شده گذار اجرایی آن پیدا نشد. | معنای عملیاتی و نتیجهٔ مالی |
| `Done` همراه `RefundDetailGroupId` | [عملیات گروه‌بندی](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:156) هر دو مقدار را ثبت می‌کند. [ModifiedOrderItemService](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:759) آن را برای اجازهٔ تغییر بعدی Order «کامل» می‌شمارد. | تأیید واریز به مشتری |
| `RefundDetailForPaymentGroupId` و آرشیو | [مسیر جداگانهٔ گروه‌بندی جهت پرداخت](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:89) این فیلد را ثبت می‌کند. | اجرای انتقال پول یا نتیجهٔ آن |
| شکست یا نتیجهٔ نامطمئن | وضعیت اختصاصی قابل مشاهده‌ای در [RefundStatusTypeEnum](/P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Enums/YashilEnums.cs:35) ندارد. | امکان تشخیص قطعی موفقیت پس از timeout |

## مسیر واقعی پرداخت Refund

[AddRefundAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:509) Refund و RefundDetail می‌سازد و [AddRefundDetail](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:649) مبلغ هر Payment را تخصیص می‌دهد. این متدها درخواست بازپرداخت به درگاه نمی‌فرستند.

در Controllerهای بررسی‌شدهٔ Saman، Ansar، Saderat و Samin نیز اتصال RefundDetail به درخواست Refund، callback یا استعلام نتیجهٔ بازپرداخت پیدا نشد. [SamanPaymentController](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:269) عملیات `Reverse` در **Verify پرداخت** دارد؛ این مسیر بازپرداخت RefundDetail ناشی از تغییر Order نیست.

یک مسیر [RefundTerminalPaymentTransaction](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/RefundPaymentTransactionItemRequestService.cs:87) برای تراکنش‌های ترمینال حکمت وجود دارد. این مسیر از درخواست و موجودیت‌های جداگانه استفاده می‌کند و در کد بررسی‌شده نتیجه‌اش به `RefundDetail.Status = Done` در Commerce متصل نیست. همچنین بارگذاری فایل عودتی حکمت به [Stored Procedure](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:324) متکی است؛ متن آن Procedure در بررسی فعلی موجود نبود.

## قاعدهٔ مالی قابل اتکا

از کد فعلی **هیچ گذار دقیق RefundDetail نمی‌توان به‌عنوان اثبات واریز واقعی انتخاب کرد**. به‌ویژه [PaymentService.GetOrderTotalPayment](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) اکنون کمترین `NewPaymentAmount` همهٔ RefundDetailها را بدون فیلتر وضعیت به‌کار می‌برد؛ ممکن است سهم Refund صرفاً ثبت‌شده را زودتر از وقوع مالی از Payment کم کند.

برای محاسبهٔ «Payment موفق خالص از وجه واقعاً بازگشته»، مبلغ قابل کسر باید **مبلغ بازپرداخت تأییدشدهٔ همان Payment** باشد. Repository فعلی نقطهٔ قابل‌اثباتی برای این تأیید در مسیرهای درگاهِ بررسی‌شده ارائه نمی‌کند. مقدار `RefundAmount`، `Done`، گروه‌بندی و آرشیو به‌تنهایی جای رسید یا نتیجهٔ تأییدشدهٔ پرداخت وجه را نمی‌گیرند.

## ابهام و کوچک‌ترین گام ایمن

پس از timeout، از رکوردهای فعلی RefundDetail نمی‌توان فهمید درخواست بازپرداخت اصلاً ارسال شده، در ارائه‌دهنده پذیرفته شده یا وجه منتقل شده است. در نتیجه Retry خودکار بدون خطر ارسال دوباره نیز از روی این کد قابل تضمین نیست. برای تعیین قرارداد نهایی، **تأیید Business و ارائه‌دهندهٔ پرداخت** لازم است: بازپرداخت آنلاین دقیقاً از چه کانالی انجام می‌شود، شناسهٔ پایدار آن کجاست، چه استعلامی نتیجهٔ نامطمئن را تعیین می‌کند، و کدام رویداد به معنی بازگشت قطعی وجه است.

تا روشن‌شدن آن، گام ایمنِ سازگار با Entityهای موجود این است که تخصیص درگاه‌اول را در `ModifiedOrderItemService` به‌عنوان **تعهد Refund** ثبت کنند، اما آن را با «وجه برگشته» یکی نگیرند. Wallet Refund فقط برای سهم مشخصِ همان تغییر Order، با اتصال به Capture درست و کلید تکرار پایدار، انجام شود؛ هنگام محاسبهٔ بدهی، سهم درگاه و سهم Ledger جداگانه شمرده شوند تا هیچ مبلغی دوبار Refund یا دوبار کسر نشود. اجرای این هماهنگی مالی تا تعیین رویداد تأیید بازپرداخت درگاه مسدود است.

هیچ فایل یا شیء دیتابیس تغییر نکرد؛ Build، تست و بررسی Runtime انجام نشد.

---

## user
### Task: Discover the existing operational gateway-refund workflow

**Mode: Read-only investigation. Do not edit files or database objects.**

We have confirmed that `RefundDetail.Status = Done` does not prove that money was returned to the customer. Do not implement Mixed Wallet Payment or change refund calculations yet.

Investigate the repository and available database scripts to determine whether an existing operational process completes gateway refunds outside the currently inspected Commerce methods.

#### Investigate

1. Search the entire repository for:
   - Refund-related services, controllers, jobs, APIs, stored procedure names, and SQL scripts.
   - `RefundDetailForPaymentGroupId`, `RefundDetailGroupId`, `RefundAmount`, and related refund status transitions.
   - Hekmat terminal refund request and response processing.
   - Import or processing of refund/return files.
2. Locate the definition and callers of the stored procedure used by the Hekmat refund-file process. If the procedure definition is not available in the repository, explicitly report that.
3. Determine whether any process records:
   - A provider refund transaction identifier.
   - Provider acceptance or final success.
   - Failure or an unknown outcome.
   - A timestamp or audit trail proving refund completion.
4. Identify any existing manual or external operational process documented in code, configuration, comments, or project documentation.
5. Distinguish gateway Verify/Reverse used to validate or reverse an initial payment from an actual refund of a previously successful payment.

#### Constraints

- No code edits, migrations, schema changes, or database changes.
- Do not infer actual money transfer from `Done`, grouping, file archival, or a request being sent.
- Do not invent provider APIs or refund capabilities.
- Cite exact file/method/SQL references for findings.

#### Deliverable

Provide:

- The complete discovered refund workflow, if one exists.
- A table mapping each payment provider to the available refund request, confirmation, and reconciliation mechanism.
- Any missing stored procedure definitions or external operational dependencies.
- The smallest next step needed to establish a reliable refund-finality contract.
- A clear statement of whether the current repository is sufficient to implement safe automatic gateway-first refunds without schema changes.

Do not implement anything.


---

## assistant
کل Repository و اسکریپت‌های موجود را برای مسیرهای Refund، فایل‌های عودتی و Procedureهای مرتبط جست‌وجو می‌کنم. نتیجهٔ هر درگاه را جداگانه از مسیر Reverse هنگام Verify گزارش می‌دهم و هیچ تغییری اعمال نمی‌کنم.

---

## assistant
## Handoff: کشف مسیر عملیاتی Refund درگاه

**درخواست فعال:** بررسی فقط‌خواندنی کل Repository و اسکریپت‌های موجود برای یافتن مسیر واقعی بازپرداخت وجه، نتیجهٔ ارائه‌دهنده و معنای مالی `RefundDetail.Done`. هیچ فایل یا شیء دیتابیس نباید تغییر کند؛ Mixed Payment نباید پیاده‌سازی شود.

### پیشرفت و شواهد

- هیچ فایلی تغییر نکرد. `git status` از ابتدا شامل تغییرات قبلی کاربر بود؛ به آن‌ها دست زده نشد.
- `ModifiedOrderItemService.AddRefundAsync` درخواست‌های `Refund`/`RefundDetail` را می‌سازد. `CreateNewRefundDetailEntity` وضعیت `Register` ثبت می‌کند: `Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:509,818`.
- `RefundDetailRepository.AssignItemGroupForRefundDetailAndConfirmAsync` هنگام گروه‌بندی، وضعیت را `Done` و `RefundDetailGroupId` را تنظیم می‌کند، بدون تماس با ارائه‌دهنده: `.../RefundDetailRepository.cs:156–164`. فراخوانی از `RefundDetailService.cs:96–110` و `RefundDetailGroupController.cs:66–70` است.
- مسیر **جداگانهٔ گروه‌بندی برای پرداخت وجه** `RefundDetailForPaymentGroupId` و `IsArchived` را تنظیم می‌کند: `RefundDetailRepository.cs:89–99,113–129`، `RefundDetailService.cs:80–94`، `RefundDetailForPaymentGroupController.cs:69–73`. این نیز تأیید واریز نیست.
- `RefundDetail` دارای `RefundAmount`, `NewPaymentAmount`, `RefundTime`, `Confirmed`, `PersonBankAccountId` و دو شناسهٔ گروه است: `Data/Yashil.Core/Entities/RefundDetail.cs:19–61,95–105`. جست‌وجوی تمام فایل‌های C# مرتبط برای `RefundTime =` هیچ محل مقداردهی پیدا نکرد؛ فقط Mapping و ViewModel از آن نام می‌برند.
- `PaymentService.GetOrderTotalPayment` بدون فیلتر وضعیت، کمینهٔ `NewPaymentAmount` تمام RefundDetailهای Payment را لحاظ می‌کند: `PaymentService.cs:261–284`. این رویداد بازگشت واقعی وجه را اثبات نمی‌کند.
- برای بازپرداخت سفارش از Saman/Ansar/Saderat/Samin در Controllerهای Verify مرتبط، اتصال RefundDetail به API/Callback/استعلام بازپرداخت پیدا نشد. `SamanPaymentController.cs:269–313` عملیات `Reverse` **هنگام Verify پرداخت اولیه و مغایرت مبلغ** دارد؛ آن را Refund پرداخت موفق قبلی تلقی نکن.
- یک جریان **مجزای ترمینال حکمت** وجود دارد: `RefundPaymentTransactionItemRequestService.ApplyRefundPaymentTransactionItemRequestActionAsync` در `Modules/YashilPayment/.../ServiceImpl/RefundPaymentTransactionItemRequestService.cs:87–181` پس از تأیید CRM، وضعیت رکورد `HekmatPaymentGatewayTransaction` را `Refunded` می‌کند، سپس `[pay].[RefundTerminalPaymentTransaction]` را فراخوانی می‌کند و `PaymentTransactionItemRefund` ثبت می‌کند. فراخوانی SP در `.../RepositoryImpl/RefundPaymentTransactionItemRequestRepository.cs:73–103` است. این جریان به RefundDetail سفارش وصل نشده است؛ باید جزئیات DTO، SP و نتیجه‌اش برای حکم نهایی خوانده شود.
- فایل بازپرداخت حکمت از طریق `HekmatService.BulkCopyHekmatRefundedTransactionsDataAsync` در `Modules/YashilCommerce/.../ServiceImpl/HekmatService.cs:259–309` بارگذاری می‌شود و سپس `RefundDetailRepository.UpdateHekmatRefundDetailRefundAmountAsync`، Procedure `[shp].[SetRefundDetailIdInHekmatRefund]` را اجرا می‌کند (`.../RefundDetailRepository.cs:324–327`). متن Procedure هنوز پیدا نشده؛ جست‌وجوی نام آن در Source فقط همین محل را نشان داد. وجود اسکریپت‌های SQL باید با `rg --files` محدود به `*.sql` دوباره و دقیق بررسی شود.
- موجودیت‌های `HekmatCanceledTransaction`, `HekmatModifiedTransaction` و فایل آپلود Refund حکمت وجود دارند. Source خود Stored Procedure، نحوهٔ تطبیق نتیجه با `RefundDetail` و اثبات واریز مشتری هنوز معلوم نیست.

### گام‌های باقی‌مانده

1. جست‌وجوی محدود و قابل گزارش برای تمام `*.sql` و تعریف هر دو Procedure؛ نتیجهٔ نبود تعریف را صریح ثبت کن.
2. `RefundPaymentTransactionItemRequestRepository.RefundTerminalPaymentTransaction`، Entityهای `PaymentTransactionItemRefund` و `RefundPaymentTransactionItemRequest`، مسیر Controller آن و داده‌های پاسخ/شناسهٔ تراکنش را بخوان؛ مشخص کن آیا به Order/Payment/RefundDetail متصل‌اند.
3. بارگذاری فایل‌های حکمت، فرایند `Confirmed` و `RefundAmount`، و نبود/وجود نشانهٔ نتیجهٔ بیرونی برای هر RefundDetail را دقیق جمع‌بندی کن.
4. جدول درگاه‌ها بساز: Saman، Ansar، Saderat، Samin، Hekmat/HekmatOnline، Vibe و Hybrid؛ درخواست Refund، تأیید، تطبیق و محدودیت هرکدام را جدا بنویس. درگاه‌های خارج از Mixed را به‌عنوان پشتیبانی‌شده معرفی نکن.
5. پاسخ نهایی فارسی، کوتاه و با لینک فایل و شمارهٔ خط: مسیر کامل کشف‌شده، وابستگی‌های بیرونی/Procedureهای ناموجود، کمترین گام برای قرارداد finality و اینکه آیا Refund خودکار ایمن بدون تغییر Schema از Source فعلی قابل اثبات است. Build و تست اجرا نشده‌اند.

---

## assistant
مسیرهای `RefundDetail` و فرایند جداگانهٔ حکمت در بررسی اولیه پیدا شده‌اند. حالا تعریف Procedureها، خروجی فرایند حکمت و هر نشانهٔ ثبت نتیجهٔ درگاه را در کل مخزن بررسی می‌کنم تا روشن شود بازپرداخت وجه واقعاً در کجا نهایی می‌شود.

---

## assistant
برای این بررسی از راهنمای `wallet-feature` استفاده می‌کنم؛ معیار اصلی آن حفظ شواهد مالی و تمایز وضعیت داخلی از تأیید واقعی درگاه است. فایل‌های راهنما در ریشهٔ والد مخزن قرار دارند و آن‌ها را از همان مسیر می‌خوانم.

---

## assistant
در کد یک مسیر دیگر برای «لغو تراکنش ترمینال حکمت» پیدا شد، اما این مسیر به سفارش Commerce و `RefundDetail` وصل نیست و اجرای اصلی‌اش داخل Stored Procedure است. همچنین بارگذاری فایل حکمت دادهٔ برگشتی را برای تطبیق وارد می‌کند؛ هنوز باید روشن کنم آیا این فایل نتیجهٔ نهایی واریز را اثبات می‌کند یا فقط مبنای تأیید دستی است.

---

## assistant
## نتیجه

در مخزن، **فرایند عملیاتیِ ثبت، گروه‌بندی، خروجی گزارش و تطبیق دستی عودت وجه** پیدا شد؛ اما برای Refund سفارش‌های Commerce، کد موجود **تأیید نهاییِ برگشت پول از ارائه‌دهنده به مشتری را ثابت نمی‌کند**. بنابراین این مخزن به‌تنهایی برای اجرای امنِ Refund خودکار «اول درگاه، سپس Wallet» کافی نیست.

### مسیر کشف‌شده

1. `ModifiedOrderItemService.AddRefundAsync` پرداخت‌های موفق سفارش را بررسی می‌کند و `RefundDetail` را با وضعیت `Register` می‌سازد ([ModifiedOrderItemService.cs:509](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:509)، [خط 818](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:818)).
2. گروه‌بندی اولیه، `RefundDetailGroupId` را تنظیم و همان‌جا وضعیت را `Done` می‌کند؛ در این مسیر درخواست یا پاسخ درگاه وجود ندارد ([RefundDetailRepository.cs:156](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:156)، [RefundDetailService.cs:96](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/RefundDetailService.cs:96)). بنابراین `Done` در کد، **تکمیل گروه‌بندی** را نشان می‌دهد، نه اثبات واریز.
3. گروه‌بندی «جهت پرداخت»، `RefundDetailForPaymentGroupId` و `IsArchived` را ثبت می‌کند و برای گروه، گزارش Excel قابل دریافت است ([RefundDetailRepository.cs:89](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:89)، [ItemGroupController.cs:211](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/ItemGroupController.cs:211)). کدِ ارسال وجه یا تطبیق نتیجهٔ بانکی در ادامهٔ این مسیر پیدا نشد.
4. برای **حکمت**، مدیر فایل تراکنش‌های لغوشده/اصلاح‌شده را بارگذاری می‌کند؛ داده‌ها وارد چهار جدول می‌شوند و سپس `[shp].[SetRefundDetailIdInHekmatRefund]` اجرا می‌شود ([RefundController.cs:97](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/RefundController.cs:97)، [HekmatService.cs:259](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/HekmatService.cs:259)، [RefundDetailRepository.cs:324](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:324)). سپس کارشناس می‌تواند مبلغ تطبیق‌یافته با فایل، یا مبلغ اعلامیِ دستی، را تأیید کند ([RefundDetailService.cs:228](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/RefundDetailService.cs:228)، [خط 287](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/RefundDetailService.cs:287)). این شواهد فرایند تطبیق را نشان می‌دهند؛ اعتبار فایل به‌عنوان **تأیید نهایی انتقال وجه** باید از صاحب فرایند حکمت روشن شود.
5. مسیر جداگانه‌ای برای **لغو تراکنش ترمینال حکمت/سفارش IsiBook** وجود دارد: تأیید CRM، تغییر وضعیت داخلی تراکنش، اجرای `[pay].[RefundTerminalPaymentTransaction]` و ایجاد `PaymentTransactionItemRefund` ([RefundPaymentTransactionItemRequestService.cs:87](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/RefundPaymentTransactionItemRequestService.cs:87)، [خط 115](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/RefundPaymentTransactionItemRequestService.cs:115)، [RefundPaymentTransactionItemRequestRepository.cs:73](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/RepositoryImpl/RefundPaymentTransactionItemRequestRepository.cs:73)). این مسیر به `RefundDetail` پرداخت سفارش Commerce متصل نیست. `PaymentTransactionItemRefund` نیز در زمان ایجاد با وضعیت `Register` ثبت می‌شود ([PaymentTransactionItemRefundService.cs:31](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/PaymentTransactionItemRefundService.cs:31)).

| مسیر پرداخت | درخواست عودتِ کشف‌شده | تأیید نهایی و تطبیق |
|---|---|---|
| Saman، Samin | Refund پرداخت موفق قبلی در مسیر Commerce پیدا نشد. Saman در Verify اولیه، هنگام مغایرت مبلغ `Reverse` می‌فرستد ([SamanPaymentController.cs:259](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:259)، [خط 274](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:274))؛ Samin نیز مسیر Verify اولیه دارد ([SaminPaymentController.cs:113](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SaminPaymentController.cs:113)). | اتصال نتیجهٔ Refund به `RefundDetail` پیدا نشد. |
| Ansar، Saderat | در کنترلرهای بررسی‌شده مسیر Verify پرداخت اولیه دیده شد ([AnsarPaymentController.cs:77](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:77)، [SaderatPaymentController.cs:89](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/SaderatPaymentController.cs:89)). | مکانیزم تأیید/تطبیق Refund سفارش پیدا نشد. |
| Hekmat | گروه‌بندی، فایل برگشتی و تأیید کارشناس؛ همچنین مسیر **مجزای** لغو ترمینال وجود دارد. | معنای مالی قطعی فایل و رفتار دو Procedure از Source معلوم نیست. |
| HekmatOnline، Vibe، Hybrid | مسیرهای Verify پرداخت اولیه وجود دارند ([HekmatOnlinePaymentGatewayController.cs:110](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/HekmatOnlinePaymentGatewayController.cs:110)، [VibePaymentController.cs:82](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/VibePaymentController.cs:82)، [HybridIpgController.cs:347](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/HybridIpgController.cs:347)). | مسیر Refund نهایی متصل به `RefundDetail` پیدا نشد. |
| `YashilIpg` داخلی | Endpoint به نام `RefundRequest` اعتبار کارت/نقد داخلی را برمی‌گرداند ([YashilIpgController.cs:421](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/YashilIpgController.cs:421)). | این کد `CreditOrCashPayment` و `CardInfo` را تغییر می‌دهد ([خط 449](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/YashilIpgController.cs:449)، [خط 502](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/YashilIpgController.cs:502))؛ مدرکی برای Refund درگاه خارجیِ سفارش Commerce نیست. |

**وابستگی‌های ناموجود:** در فایل‌های SQL مخزن، تعریف هیچ‌یک از `[shp].[SetRefundDetailIdInHekmatRefund]` و `[pay].[RefundTerminalPaymentTransaction]` پیدا نشد. فراخوانی Procedure دوم فقط رشتهٔ خروجی `@Result` را می‌خواند و خطا را به پیام عمومی تبدیل می‌کند؛ بدون تعریف Procedure نمی‌توان دانست آیا پول منتقل می‌شود، چه شناسه‌ای ثبت می‌شود یا وضعیت نامعلوم چگونه بازیابی می‌شود ([RefundPaymentTransactionItemRequestRepository.cs:73](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/RepositoryImpl/RefundPaymentTransactionItemRequestRepository.cs:73)). بررسی دیتابیس زنده در این تحقیق انجام نشد.

`RefundDetail` فیلدهای `RefundTime` و `Confirmed` دارد ([RefundDetail.cs:57](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/RefundDetail.cs:57)، [خط 95](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/RefundDetail.cs:95))؛ در کد C# محل ثبت `RefundTime` یا شناسهٔ نتیجهٔ Refund ارائه‌دهنده برای آن پیدا نشد. `Confirmed` در مسیر حکمت با تأیید کارشناس، و حتی با مبلغ اعلامی دستی، تنظیم می‌شود. ازاین‌رو نه `Done`، نه `Confirmed` و نه بایگانی گروه به‌تنهایی قاعدهٔ قابل اتکای «وجه به مشتری برگشت» نیستند. در حال حاضر `GetOrderTotalPayment` نیز `NewPaymentAmount` را از همهٔ `RefundDetail`های مرتبط، بدون شرط تأیید مالی نهایی، در محاسبه می‌گیرد ([PaymentService.cs:261](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261)).

**کوچک‌ترین گام بعدی:** مالک فرایند مالی و ارائه‌دهندگان باید برای هر مسیر، سند نتیجهٔ قابل استعلام یا فایل تسویهٔ معتبر، شناسهٔ یکتای Refund، معنای موفق/ناموفق/نامعلوم و قاعدهٔ Retry را ارائه کنند؛ هم‌زمان متن دو Procedure فوق باید بررسی شود. پس از مشخص‌شدن این قرارداد می‌توان تعیین کرد آیا ثبت و تطبیق آن با فیلدهای موجود ممکن است. تا آن زمان، اجرای خودکار gateway-first refund و کسر آن از خالص پرداخت موفق، تضمین مالی قابل دفاعی ندارد.

هیچ فایلی یا شیء دیتابیس تغییر نکرد؛ Build و تست اجرا نشدند.

---

## user
## Task: Assess Mixed Wallet Payment Without Refund Changes

Perform a **read-only feasibility analysis** for implementing Mixed Wallet Payment in the existing repository.

### Scope

Trace the complete payment path:

- Checkout and payment-method selection
- Order debt calculation
- Payment creation and idempotent retry
- WalletReservation creation
- Gateway redirect and Verify
- Wallet Capture/Release
- Order status transitions and final debt calculation

### Constraints

- Do not modify files, entities, schemas, migrations, SQL objects, or database data.
- Do not implement anything.
- Do not change existing Refund behavior.
- Do not treat `RefundDetail.Status = Done`, `Confirmed`, or archive status as proof of actual money returned.
- Preserve normal checkout behavior.
- Vibe and Hybrid remain excluded until their partial-payment contracts are verified.
- Full Wallet Payment (`G = 0`) remains out of scope.
- Do not mark an Order Complete until gateway success, Wallet Capture, and zero outstanding debt are established.
- Do not release a Reservation while gateway or Capture outcome is uncertain.

### Required analysis

1. Determine whether Mixed Payment can be implemented without relying on incorrect Refund accounting.
2. Identify every method that calculates Order debt or changes Order status, including paths used by checkout, payment callbacks, retries, and order modifications.
3. Determine whether existing Refund records can make the payable amount unreliable even without changing Refund code. Explain concrete scenarios.
4. Verify whether Payment creation, Reservation, balance updates, and retry handling can be coordinated safely using existing database fields and constraints only.
5. Identify how gateway Verify success followed by Capture failure can be recovered idempotently.
6. Check whether an Order can be completed through an existing supported path without creating a fake or zero-amount Payment.
7. Separate findings into:
   - Safe to implement now
   - Blocked by Refund accounting
   - Blocked by missing gateway or database contracts
8. Recommend the smallest next implementation scope, or conclude that implementation must wait.

### Output

Provide exact file paths, method names, and relevant line numbers. For each blocker, explain the concrete financial or consistency risk and the minimum information or change needed to resolve it.

Do not claim build or runtime correctness without running the relevant checks. Make no changes.


---

## assistant
مسیر Checkout تا تسویه و وضعیت سفارش را از کد فعلی دنبال می‌کنم و هر نقطه‌ای را که `RefundDetail` می‌تواند مبلغ بدهی را تغییر دهد مشخص می‌کنم. نتیجه را به بخش‌های قابل اجرا و مسدودشده تقسیم می‌کنم؛ هیچ فایلی تغییر نمی‌کند.

---

## assistant
یک ناسازگاری مهم مشخص شد: `CalcAmountForPayAsync` برای روش `Wallet` نوع فروش اقساطی را انتخاب می‌کند، در حالی که بعضی درگاه‌ها مسیر نقدی دارند. همچنین مسیر فعلی آماده‌سازی Wallet قبل از ساخت Payment رزرو می‌کند و کلید آن به Order وابسته است. حالا اثر این دو مورد را بر مبلغ `T/W/G`، تلاش‌های مجدد و مرز تراکنش بررسی می‌کنم.

---

## assistant
مسیر Verify فعلی چند درگاه پس از موفقیت، Order را مستقیم `Complete` می‌کند؛ این برای Mixed کافی نیست، چون Capture و ماندهٔ واقعی هنوز کنترل نشده‌اند. از طرف دیگر `ModifiedOrderItemService` هنگام اصلاح سفارش فقط Paymentهای `Paid` را مبنای تأمین مالی می‌گیرد و Capture کیف پول را در این محاسبه وارد نمی‌کند. دارم مرز دقیق این دو مانع و امکان آماده‌سازی اتمیکِ تلاش پرداخت را جمع‌بندی می‌کنم.

---

## assistant
## نتیجهٔ امکان‌سنجی

**آماده‌سازی اتمیک یک تلاش Mixed Payment با فیلدهای موجود از نظر فنی قابل طراحی است، اما فعال‌کردن پرداخت Mixed از ابتدا تا تکمیل Order فعلاً ایمن نیست.** دو مانع مالی باقی است: محاسبهٔ بدهی، Capture کیف پول را به‌عنوان پرداخت حساب نمی‌کند و `RefundDetail` ثبت‌شده را پیش از اثبات بازگشت پول در محاسبهٔ Payment اثر می‌دهد. مسیر اصلاح سفارش نیز تأمین مالی WalletProgram را نمی‌بیند. هیچ فایل یا داده‌ای تغییر نکرد.

### مسیر فعلی پرداخت

| مرحله | رفتار مشاهده‌شده |
|---|---|
| انتخاب روش | اسکریپت [`_OrderSummaryForPay.cshtml:248`](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:248) روش انتخابی را با `OrderId` به `SaveShoppingCartToOrder` می‌فرستد؛ **attempt key ندارد** ([خط 308](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:308)، [`ShoppingCartParam.cs:3`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs:3)). کنترلر Order را ذخیره یا بازیابی و URL درگاه را برمی‌گرداند ([`OrderController.cs:538`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:538)، [خط 631](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:631)، [خط 745](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:745)). |
| بدهی و Payment | `CalcAmountForPayAsync` از مبلغ ذخیره‌شدهٔ Order، خروجی `GetOrderTotalPayment` را کم می‌کند؛ برای Order در وضعیت `Complete` اصلاً وارد این محاسبه نمی‌شود ([`OrderService.cs:412`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412)، [خط 434](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:434)). `CreateNewPaymentAndReturnAsync` هر بار Payment تازه با `Code = "1"` و `Total` محاسبه‌شده می‌سازد و تراکنش خود را Commit می‌کند ([خط 552](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552)). |
| Reserve | مسیر فعلی `WalletOrderPreparationService` کلید ثابت `WalletReserve:{OrderId}` می‌سازد و قبل از Payment، `ReserveAsync` را فرا می‌خواند ([`WalletOrderPreparationService.cs:58`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:58)، [خط 130](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:130)). قرارداد فعلی `ReserveAsync` در مقابل **PaymentId موجود** می‌خواهد، کلید ورودی را به `WalletReserve:Payment:{PaymentId}` تبدیل می‌کند و تراکنش مستقل خود را Commit می‌کند ([`WalletReservationService.cs:33`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33)، [خط 54](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:54)، [خط 60](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:60)، [خط 157](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:157)). این دو مسیر در وضع فعلی قرارداد واحدی ندارند. |
| Redirect و Verify | برای نمونه، Ansar و Saderat در `Index` Payment تازه می‌سازند و مبلغ همان Payment را برای توکن درگاه می‌فرستند ([`AnsarPaymentController.cs:51`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:51)، [`SaderatPaymentController.cs:55`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/SaderatPaymentController.cs:55)). Saman نیز همین الگو را دارد ([`SamanPaymentController.cs:88`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:88)). Verify موفق، Order را **پیش از Capture** مستقیم `Complete` و Payment را `Paid` می‌کند؛ نمونه‌ها: [Ansar:201](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/AnsarPaymentController.cs:201)، [Saderat:209](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/SaderatPaymentController.cs:209)، [Saman:327](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SamanPaymentController.cs:327)، [Samin:255](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/SaminPaymentController.cs:255). |
| Settlement | `CaptureAsync` و `ReleaseAsync` تراکنش‌های مالی Serializable و گذار شرطی Reservation دارند ([`WalletSettlementService.cs:33`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:33)، [خط 53](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:53)، [خط 176](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:176)). Capture، `Payment.Paid` را کنترل می‌کند ([خط 160](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:160)) و Ledger را می‌نویسد ([خط 224](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:224)). در کنترلرهای Verify فراخوانی Settlement پیدا نشد؛ API جداگانهٔ آن وجود دارد ([`WalletSettlementController.cs:22`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletSettlementController.cs:22)). |

### بدهی و Refund: مانع مالی

`GetOrderTotalPayment` برای هر Payment موفق، **کمترین `NewPaymentAmount` همهٔ `RefundDetail`های آن را بدون بررسی وضعیت یا تأیید بازگشت وجه** جایگزین `Payment.Total` می‌کند ([`PaymentService.cs:261`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261)). `ModifiedOrderItemService` مقدار `NewPaymentAmount` را هنگام ساخت `RefundDetail` با وضعیت `Register` تعیین می‌کند ([`ModifiedOrderItemService.cs:685`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:685)، [خط 818](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:818)). طبق بررسی قبلی، `Done` نیز در زمان گروه‌بندی تنظیم می‌شود و اثبات پرداخت به مشتری نیست ([`RefundDetailRepository.cs:156`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:156)).

سناریوی مشخص: Payment موفق ۱۰۰ است؛ برای اصلاح سفارش `RefundDetail` با `NewPaymentAmount = ۷۰` ثبت می‌شود، اما وجه ۳۰ هنوز برنگشته است. `GetOrderTotalPayment` همان لحظه ۷۰ را به‌جای ۱۰۰ حساب می‌کند. پس بدهی قابل پرداخت یا تصمیم تکمیل Order می‌تواند بر مبنای **Refund ثبت‌شده، نه Refund نهایی** تغییر کند. حتی اگر هنگام ساخت تلاش Mixed هیچ Refundی وجود نداشته باشد، اصلاح بعدی Order این مشکل را دوباره وارد چرخه می‌کند.

مشکل دوم مستقل از آن است: پس از `G` موفق و `W` Capture شده، `CalcAmountForPayAsync` فقط Paymentهای موفق را کم می‌کند و Capture/WalletRefund را نمی‌شمارد ([`OrderService.cs:437`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:437)، [`PaymentService.cs:261`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261)). برای `T=100، W=30، G=70` ماندهٔ محاسباتی ۳۰ می‌ماند؛ اگر callback قبلاً Order را `Complete` کرده باشد، متد به‌جای اثبات صفر بودن بدهی، محاسبه را کنار می‌گذارد. در اصلاح سفارش نیز `ApplyModificationAsync` فقط جمع `Payment.Total`های `Paid` را مبنا می‌گیرد ([`ModifiedOrderItemService.cs:430`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:430)، [خط 450](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:450)) و ترتیب Refund آن مربوط به Wallet قدیمی/ناجا است، نه Capture کیف پول جدید ([خط 557](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:557)، [خط 597](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:597)). **بدون تغییر رفتار Refund، این مسیر برای Order مختلط نتیجهٔ مالی قابل اتکا ندارد.**

### Retry، تراکنش و بازیابی

- **آنچه مدل اجازه می‌دهد:** `WalletReservation.PaymentId` به Payment متصل است و Mapping برای `IdempotencyKey` ایندکس یکتا دارد ([`WalletReservation.cs:62`](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletReservation.cs:62)، [`YashilAppDbContext.cs:28097`](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28097)). طبق تأیید قبلی شما، وجود فیزیکی این ایندکس فقط برای `IsiClubCommerceLocal` بررسی شده است؛ محیط دیگری در این نوبت تأیید نشد. در یک پیاده‌سازی بعدی، **Payment، Reservation و افزایش دو مانده باید در یک تراکنش مشترک** ثبت شوند؛ برخورد کلید یکتا باید کل Payment بازنده را نیز Rollback کند، سپس نتیجهٔ برنده با Application/User/Order/روش/Program و `T/W/G` اعتبارسنجی شود.
- **آنچه فعلاً وجود ندارد:** درخواست Checkout، attempt key ندارد؛ `Payment.Code = "1"` است و در Mapping بررسی‌شده برای آن قید یکتا دیده نشد ([`ShoppingCartParam.cs:3`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/ViewModels/ShoppingCartParam.cs:3)، [`OrderService.cs:560`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:560)، [`YashilAppDbContext.cs:16264`](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:16264)). کلید فعلیِ Order محور تلاش جدید را از Retry جدا نمی‌کند؛ کلید Payment محور نیز قبل از ایجاد Payment هویت درخواست اولیه را مشخص نمی‌کند. متدهای موجود Payment و Reserve هرکدام تراکنش مستقل Commit می‌کنند. قفل سبد در Controller فقط حافظهٔ همان پردازش و ۳۰ ثانیه‌ای است ([`OrderController.cs:650`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:650))؛ تضمین هم‌زمانی بین پردازش‌ها نیست. در Mapping نیز قید یکتا برای «حداکثر یک Reservation فعال در هر Order» دیده نشد؛ چک فعلی یک Query برنامه‌ای است ([`WalletReservationRepository.cs:36`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/WalletReservationRepository.cs:36)).
- **Capture پس از Verify:** سرویس، وضعیت Captured و Ledger را در یک تراکنش ثبت می‌کند و Replayِ Capture ثبت‌شده را تشخیص می‌دهد ([`WalletSettlementService.cs:117`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:117)، [خط 176](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:176)). بنابراین پس از timeout باید ابتدا نتیجهٔ درگاه و سپس وضعیت Reservation و Ledger از دیتابیس دوباره خوانده شوند؛ Release در وضعیت نامعلوم مجاز نیست. **هماهنگ‌کنندهٔ خودکار این بازیابی در مسیر callback پیدا نشد.** Capture برای Reservation منقضی‌شده رد می‌شود ([خط 165](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:165))، در حالی که Reservation فعلی ۳۰ دقیقه اعتبار دارد ([`WalletReservationService.cs:136`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:136)). این موضوع برای Verify دیرهنگام نیازمند سیاست قطعی است. `ReleaseAsync` فعلی نیز وضعیت Payment را بررسی نمی‌کند ([`WalletSettlementService.cs:44`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:44)، [خط 158](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:158))؛ برای Mixed نمی‌توان صرفاً به تصمیم Controller تکیه کرد.
- **تکمیل بدون Payment صوری:** وضعیت موجود `IncompletePayment` و گذار آن به `Complete` در `UpdateOrderStatusAsync` پشتیبانی می‌شود ([`OrderService.cs:223`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:223)). خود این متد Payment تازه نمی‌سازد؛ پس از لحاظ وضعیت، تکمیل بدون Payment صفر ممکن است، **اما فقط پس از** تأیید `Paid + Capture + بدهی صفر` توسط محاسبهٔ مالی اصلاح‌شده. هنگام Paid، `PaymentService` همچنین Procedure مربوط به سهم اقلام سفارش را صدا می‌زند ([`PaymentService.cs:120`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:120)، [`OrderItemRepository.cs:417`](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/OrderItemRepository.cs:417))؛ تعریف `[shp].[SetOrderItemPriceRightShare]` در اسکریپت‌های مخزن پیدا نشد، پس سازگاری آن با `Payment.Total = G` هنوز اثبات نشده است.

### تصمیم اجرایی

| دسته | نتیجه و کوچک‌ترین اقدام لازم |
|---|---|
| **قابل طراحی اکنون** | Quote بدون اثر مالی، بررسی Eligibility، و طراحی تلاش اتمیک با `0 < W < T` و کلید پایدارِ مجزا برای هر تلاش. مسیر عادی Checkout باید شاخهٔ فعلی خود را حفظ کند. این‌ها به‌تنهایی مجوز Redirect نیستند. |
| **مسدود به‌علت Refund و بدهی** | یک محاسبهٔ مشترک باید Payment موفق، **Refund درگاهِ واقعاً نهایی**، خالص `Capture − WalletRefund` و مبلغ جاری Order را تفکیک کند. چون قرارداد finality درگاه هنوز ثابت نشده و مسیر اصلاح سفارش WalletProgram را حساب نمی‌کند، فعال‌سازی Mixed برای سفارش‌های قابل اصلاح/Refund ایمن نیست. حداقل نیاز: قرارداد مالی Refund و بازنگری محدود محاسبهٔ بدهی و اصلاح سفارش؛ صرفاً فیلترکردن `Done` کافی نیست. |
| **مسدود به‌علت قرارداد درگاه/DB** | باید مشخص شود کدام درگاه `G` را به‌عنوان پرداخت بخشی از Order می‌پذیرد؛ Vibe و Hybrid همچنان خارج‌اند. رفتار Procedure سهم اقلام با Payment جزئی، سیاست Verify پس از انقضای Reservation، و اجرای اتمیک با ایندکس یکتای موجود در هر محیط هدف باید تأیید شوند. |
| **مسدود به‌علت اتصال مسیرها** | Index درگاه نباید Payment دوم بسازد؛ callback نباید پیش از Capture، Order را Complete کند؛ Recovery باید نتیجهٔ نامعلوم Verify/Capture را بدون Release زودهنگام پیگیری کند. این‌ها تغییرات کدِ لازم در مرحلهٔ Implementation هستند، نه قابلیت‌های موجود فعلی. |

**پیشنهاد:** فعلاً Mixed Payment را برای کاربر فعال نکنید. کوچک‌ترین مرحلهٔ بعد، تثبیت قرارداد finality و بدهی/اصلاح سفارش و بررسی Procedure سهم اقلام و درگاه منتخب است؛ سپس می‌توان آماده‌سازی اتمیکِ یک درگاه را با مسیر Retry و Recovery پیاده‌سازی و آزمایش کرد. این بررسی فقط‌خواندنی بود؛ Build، تست و بررسی Runtime انجام نشد و هیچ ادعای صحت اجرایی مطرح نمی‌شود.

---

## user
<environment_context>
  <current_date>2026-10-16</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user
## Task: Finalize Mixed Wallet Payment Decision & Blockers

نقش: Senior Software Architect و مسئول تحلیل مالی/فنی پروژه Wallet.

### هدف

با استفاده از مستندات Codex، گزارش‌های امکان‌سنجی قبلی و کد فعلی مخزن، یک سند تصمیم‌گیری کوتاه و دقیق برای تعیین شرایط شروع Implementation قابلیت Mixed Wallet Payment تهیه کن.

### ابتدا بررسی کن

- `AGENTS.md` و مستندات مرتبط با Wallet و Mixed Payment
- گزارش آخر امکان‌سنجی Mixed Payment و گزارش بررسی Refund
- پیاده‌سازی فعلی Payment، WalletReservation، WalletSettlement، Order debt calculation و Order modification

از جست‌وجوی مجدد کل مخزن خودداری کن؛ فقط برای رفع ابهام مشخص یا تأیید یک ادعای مهم، کد مرتبط را بررسی کن.

### تصمیم‌های قطعی که باید حفظ شوند

- هیچ تغییر Schema، Entity، Migration یا Database Object در محدوده فعلی مجاز نیست.
- Vibe، Hybrid و Full Wallet Payment خارج از محدوده فعلی‌اند.
- `T = W + G`، با `0 < W < T` و `G > 0`.
- Payment فقط به مبلغ `G` ساخته می‌شود و Reservation مبلغ `W` را نگه می‌دارد.
- Retry همان تلاش باید همان Payment و Reservation را بازیابی کند؛ تلاش جدید باید کلید مستقل داشته باشد.
- Order فقط پس از تأیید موفق درگاه، Capture موفق Wallet و اثبات بدهی صفر می‌تواند Complete شود.
- در وضعیت نامعلوم Verify یا Capture، Reservation نباید زودهنگام Release شود.
- رفتار Refund نباید بر اساس `Done`، `Confirmed` یا بایگانی به‌عنوان اثبات بازگشت وجه فرض شود.
- Refund و اصلاح سفارش تا تعیین تکلیف قرارداد مالی آن‌ها نباید به‌صورت ناایمن تغییر کنند.

### خروجی موردنیاز

یک سند کوتاه با عنوان `Mixed Wallet Payment - Decision & Blockers` تهیه کن که شامل این بخش‌ها باشد:

1. **تصمیم معماری:** چه چیزهایی تأیید شده و چه چیزهایی هنوز تأیید نشده‌اند.
2. **Blockerها:** برای هر مورد، ریسک مشخص، مدرک موجود و حداقل اقدام لازم را بنویس.
3. **وابستگی‌های خارج از کد:** دقیقاً مشخص کن چه اطلاعاتی باید از مسئول مالی، مسئول فرایند Refund یا ارائه‌دهنده درگاه دریافت شود.
4. **ترتیب رفع موانع:** اقدام‌ها را به ترتیب وابستگی مرتب کن؛ از تحقیقاتی که قبلاً نتیجه قطعی دارند دوباره شروع نکن.
5. **Implementation Plan:** پس از رفع موانع، فازهای پیاده‌سازی و معیار پذیرش هر فاز را مشخص کن.
6. **Go/No-Go Criteria:** شرایط روشن و قابل‌آزمایشی تعیین کن که بر اساس آن‌ها بتوان تصمیم گرفت پیاده‌سازی واقعی و فعال‌سازی قابلیت آغاز شود.

### محدودیت‌ها

- هیچ کدی را تغییر نده و هیچ Migration یا SQL اجرا نکن.
- هیچ فرض تأییدنشده‌ای را به‌عنوان تصمیم قطعی ثبت نکن.
- موارد ناشناخته را صریحاً با عنوان `Unresolved` مشخص کن.
- برای ادعاهای فنی، مسیر فایل، نام متد و در صورت امکان شماره خط بیاور.
- میان شواهد کد، تصمیم‌های تأییدشده و پیشنهاد معماری تفاوت بگذار.
- گزارش را کوتاه، اجرایی و بدون تکرار جزئیات گزارش‌های قبلی نگه دار.
- Build و Test اجرا نکن؛ این مرحله صرفاً مستندسازی و تصمیم‌گیری است.

در پایان، فقط خلاصه تصمیم، مسیر فایل ایجادشده و Blockerهایی را که برای ادامه به پاسخ انسانی نیاز دارند گزارش کن.


---

## assistant
سند تصمیم را بر پایهٔ دو گزارش قبلی همین گفتگو و چند نقطهٔ کلیدی کد فعلی تنظیم می‌کنم. ابتدا مسیر مناسب سند در مخزن و تغییرات احتمالی همان فایل‌ها را بررسی می‌کنم، سپس فقط سند را می‌نویسم؛ Build و Test اجرا نمی‌کنم.

---

## assistant
مدارک برای سند کافی است. نتیجهٔ فعلی «No-Go برای فعال‌سازی» است: قرارداد تلاش اتمیک را می‌توان با مدل موجود طراحی کرد، اما finality بازپرداخت، محاسبهٔ بدهی و رفتار برخی مسیرهای درگاه هنوز تصمیم یا تأیید لازم دارند. سند را با دو Gate جدا برای «شروع کدنویسی» و «فعال‌سازی عملیاتی» ثبت می‌کنم.

---

## assistant
سند [Mixed Wallet Payment - Decision & Blockers](P:/Projects/walletProject/Wallet/backEnd/docs/decisions/Mixed-Wallet-Payment-Decision-and-Blockers.md) ایجاد شد. تصمیم فعلی **No-Go برای فعال‌سازی** است؛ در سند، شرایط جداگانهٔ شروع Implementation و فعال‌سازی ثبت شده‌اند.

مواردی که برای ادامه به پاسخ انسانی نیاز دارند:

- مسئول مالی و Refund: معیار قطعی بازگشت وجه و اثر آن بر بدهی و اصلاح سفارش.
- مالک درگاه منتخب: پذیرش مبلغ جزئی `G` و روش تعیین نتیجهٔ Verify نامعلوم یا دیرهنگام.
- مسئول DB: رفتار Procedure سهم اقلام سفارش برای Payment جزئی و تأیید ایندکس Reservation در محیط هدف.
- تیم Wallet–Commerce: سیاست انقضای Reservation و بازیابی Capture پس از Verify موفق.

فقط سند تغییر کرد؛ Build و Test اجرا نشدند.

---

## user
## Task: Select the First Gateway for Mixed Wallet Payment

نقش: Senior Software Architect.

### هدف

از روی کد موجود مشخص کن کدام درگاه، گزینهٔ مناسب‌تری برای اولین پیاده‌سازی محدود Mixed Wallet Payment است؛ بدون شروع Implementation.

### منابع

- `docs/decisions/Mixed-Wallet-Payment-Decision-and-Blockers.md`
- کد مسیرهای Checkout، Payment creation، Redirect و Verify در درگاه‌های موجود
- قراردادها و مستندات درگاه، فقط در صورت وجود در مخزن

از تکرار تحلیل عمومی Wallet و Refund خودداری کن. فقط وابستگی‌هایی را بررسی کن که انتخاب درگاه را تغییر می‌دهند.

### محدودیت‌ها

- هیچ فایلی، Entity، Schema، Migration یا Database Object را تغییر نده.
- هیچ پرداخت واقعی یا درخواست خارجی اجرا نکن.
- Vibe و Hybrid فعلاً خارج از گزینه‌های انتخاب هستند.
- پذیرش پرداخت جزئی را صرفاً از روی کد داخلی فرض نکن.
- نام درگاه را بدون شواهد کافی «تأییدشده برای Mixed» اعلام نکن.

### بررسی

برای هر گزینهٔ باقی‌مانده:

1. مسیر ساخت Payment و Redirect را مشخص کن؛ آیا می‌توان از Payment موجود با مبلغ `G` استفاده کرد، بدون اینکه Payment دومی ساخته شود؟
2. مسیر Verify و وضعیت نامعلوم/دیرهنگام را بررسی کن.
3. مشخص کن کدام اطلاعات از کد قابل اثبات و کدام‌یک نیازمند تأیید مالک درگاه است.
4. وابستگی به `[shp].[SetOrderItemPriceRightShare]` و سازگاری آن با Payment جزئی را جداگانه ثبت کن.
5. پیچیدگی تغییرات، ریسک‌های مالی و تست‌های لازم را مقایسه کن.

### خروجی

یک جدول مقایسه‌ای کوتاه، سپس:

- گزینهٔ پیشنهادی برای اولین پیاده‌سازی، مشروط به تأییدهای لازم
- دلایل رد گزینه‌های دیگر
- سؤال‌های دقیق و حداقلی برای مالک درگاه
- معیار Go/No-Go برای شروع پیاده‌سازی همان درگاه

در پایان، فقط مسیر فایل گزارش ایجادشده و نتیجهٔ پیشنهادی را اعلام کن. هیچ تغییری در کد ایجاد نکن و Build/Test اجرا نکن.


---

## assistant
برای انتخاب درگاه اول، فقط مسیر ساخت Payment، Redirect، Verify و بازیابی نتیجهٔ هر گزینه را مقایسه می‌کنم. گزارش تصمیم قبلی را مبنا می‌گیرم و برای قرارداد پرداخت جزئی، شواهد کد را از تأیید لازمِ مالک درگاه جدا نگه می‌دارم.

---

## assistant
## Handoff: انتخاب اولین درگاه برای Mixed Wallet Payment

**درخواست فعال:** تحلیل فقط‌خواندنی درگاه‌های موجود، تهیهٔ جدول مقایسه و ایجاد یک فایل گزارش کوتاه. هیچ Implementation، Build، Test، درخواست خارجی یا تغییر فایل‌های موجود مجاز نیست. Vibe و Hybrid از گزینه‌ها خارج‌اند. خروجی نهایی باید فقط مسیر فایل گزارش و گزینهٔ پیشنهادی را اعلام کند.

### انجام‌شده

- سند مبنا خوانده شد: [Mixed-Wallet-Payment-Decision-and-Blockers.md](P:/Projects/walletProject/Wallet/backEnd/docs/decisions/Mixed-Wallet-Payment-Decision-and-Blockers.md). موانع عمومی Refund و بدهی همچنان برقرارند؛ این تحلیل فقط انتخاب درگاه را بررسی می‌کند.
- `OrderController.GetRedirectUrl` مسیرهای Hekmat، Ansar، Saman، Samin، Saderat و HekmatOnline را مشخص می‌کند: `Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:745–799`.
- `OrderService.CreateNewPaymentAndReturnAsync` در هر فراخوانی Payment تازه می‌سازد و Commit می‌کند؛ برای Mixed، Index درگاه منتخب باید بتواند Payment موجود را مصرف کند: `.../OrderService.cs:552–591`.
- سند قرارداد رسمی درگاه در فایل‌های مرتبط مخزن پیدا نشد. **پذیرش پرداخت جزئی برای هیچ گزینه‌ای تأیید نشده است.**

### شواهد درگاه‌ها

- **Saman:** `Index` Payment تازه می‌سازد و مبلغ `PaymentInformation.Total` را برای توکن می‌فرستد (`SamanPaymentController.cs:88–108`). Verify، `OrginalAmount` و `AffectiveAmount` را با `Payment.Total` مقایسه می‌کند (`:259–271`) و در صورت مغایرت `Reverse` می‌فرستد (`:274–316`). نکتهٔ پرریسک: حتی اگر Reverse ناموفق شود، Payment را `Unsuccessful` می‌کند (`:299–308`)؛ Verify ناموفق نیز `Unsuccessful` می‌شود (`:371–377`). موفقیت Verify، Order را پیش از ثبت Paid مستقیم Complete می‌کند (`:327–338`). با وجود تطبیق مبلغ داخلی، **سیاست وضعیت نامعلوم/Reverse نیازمند اصلاح و تأیید مالک درگاه است**.
- **Ansar:** `Index` Payment تازه می‌سازد؛ مبلغ ارسالی از `PaymentInformation.Total` است (`AnsarPaymentController.cs:51–65`). `VerifyMerchantTransAsync` نتیجهٔ bool می‌دهد؛ شکست آن `Unsuccessful` ثبت می‌شود (`:177–185`). موفقیت Order را پیش از Paid مستقیم Complete می‌کند (`:199–208`). تطبیق صریح مبلغ پاسخ با `Payment.Total` در بخش بررسی‌شده دیده نشد.
- **Saderat:** `Index` Payment تازه می‌سازد؛ مبلغ از `PaymentInformation.Total` است (`SaderatPaymentController.cs:55–68`). Verify bool و شکست آن `Unsuccessful` (`:189–197`)؛ موفقیت Complete پیش از Paid (`:209–218`). پاسخ دیرهنگام و تطبیق مبلغ نیازمند بررسی/تأیید است.
- **Samin:** `Index` به `purchaseType` نیاز دارد و `GetGenerateTokenParams` را فرا می‌خواند که Payment تازه می‌سازد (`SaminPaymentController.cs:57–93` و `OrderService.cs:1020–1063`). علاوه بر مبلغ، تعداد اقساط و نوع خرید به توکن می‌روند (`:87–92`). Verify پس از نتیجهٔ `isSuccess`، Complete پیش از Paid انجام می‌دهد (`:242–265`). به‌دلیل قرارداد اضافی خرید/اقساط، انتخاب اول کم‌هزینه نیست.
- **Hekmat/HekmatOnline:** هنوز باید متمرکز بررسی شوند؛ Hekmat وضعیت `Processing` و استعلام دیرهنگام دارد (`HekmatService.cs:108–153`). HekmatOnline `ConfirmPay` و `InquiryPay` دارد، اما Complete قبل از Paid در callback و استعلام دیده شد (`HekmatOnlinePaymentGatewayService.cs:633–651,759–772`). مسیر ایجاد Payment/مبلغ و پیچیدگی بازیابی باید بررسی شود.
- **وابستگی مشترک:** `PaymentService.UpdatePaymentStatusAsync` هنگام `Paid`، `[shp].[SetOrderItemPriceRightShare]` را اجرا می‌کند (`PaymentService.cs:119–125`؛ `OrderItemRepository.cs:417–420`). تعریف Procedure در Source موجود نیست؛ سازگاری آن با `Payment.Total = G` برای همهٔ گزینه‌ها **Unresolved** است.

### قدم‌های بعد

1. فقط مسیرهای Hekmat و HekmatOnline را برای Payment creation، درخواست مبلغ، Verify/Inquiry و وضعیت نامعلوم بخوان؛ در صورت لزوم خطوط دقیق Ansar/Saderat را تکمیل کن.
2. از میان گزینه‌ها یک **پیشنهاد مشروط** بده. Saman تطبیق صریح مبلغ دارد ولی Reverse ناموفق را نادرست نهایی می‌کند؛ HekmatOnline استعلام دارد ولی پیچیدگی بیشتری دارد. این موازنه را قبل از انتخاب نهایی بررسی کن.
3. گزارشی کوتاه در `P:\Projects\walletProject\Wallet\backEnd\docs\decisions\` با جدول گزینه‌ها، دلایل کنار گذاشتن سایر گزینه‌ها، سؤال‌های دقیق مالک درگاه و Gateهای Go/No-Go ایجاد کن. هیچ درگاهی را «تأییدشده برای Mixed» ننام.
4. فقط فایل گزارش جدید را بررسی کن؛ Build/Test اجرا نکن. در Final تنها لینک فایل و نتیجهٔ پیشنهاد را بیاور.

**وضعیت فایل‌ها:** برای این درخواست هنوز هیچ فایلی ایجاد یا تغییر نکرده است.

---

## assistant
مسیرهای Hekmat و HekmatOnline را برای ساخت Payment، مبلغ ارسالی و بازیابی نتیجه بررسی می‌کنم. سپس مقایسهٔ کوتاهی در یک فایل گزارش جدید ثبت می‌کنم؛ کد پرداخت و فایل‌های موجود را تغییر نمی‌دهم.

---

## assistant
تا اینجا HekmatOnline مسیر استعلام با توکن دارد، اما در شروع Redirect مثل سایر مسیرها Payment تازه می‌سازد. Hekmat هم نتیجهٔ در حال پردازش را نگه می‌دارد. تفاوت تعیین‌کننده، نحوهٔ بازیابی نتیجهٔ نامعلوم و اثبات مبلغ `G` است؛ همین دو بخش را دقیق‌تر بررسی می‌کنم.

---

## assistant
HekmatOnline را گزینهٔ پیشنهادیِ مشروط می‌دانم: درخواست توکن مبلغ Payment را می‌گیرد و مسیر Inquiry می‌تواند نتیجهٔ دیرهنگام را بررسی کند. با این حال کد فعلی در چند شاخهٔ نامعلوم نتیجه را ناموفق ثبت می‌کند و Order را زود Complete می‌کند؛ این‌ها باید جزو شرط‌های صریح Go/No-Go باشند.

---

## assistant
گزارش: [انتخاب اولین درگاه Mixed Wallet Payment](P:/Projects/walletProject/Wallet/backEnd/docs/decisions/Mixed-Wallet-First-Gateway-Selection.md)

**پیشنهاد: HekmatOnline، مشروط به تأیید قرارداد پرداخت جزئی و رفع شرط‌های Go/No-Go گزارش.** هنوز هیچ درگاهی برای Mixed تأیید نشده است.

---

## user
## Task: Resolve HekmatOnline Mixed Payment Go/No-Go Gates

**هدف:** فقط با بررسی Read-only، شواهد لازم برای تصمیم شروع پیاده‌سازی محدود Mixed Wallet Payment را تکمیل کن. هیچ کدی را تغییر نده.

### محدوده بررسی

1. گزارش `docs/decisions/Mixed-Wallet-Payment-Decision-and-Blockers.md` و گزارش انتخاب درگاه قبلی را بخوان.
2. مسیر HekmatOnline را در کد دنبال کن:
   - ساخت Payment و ارسال مبلغ
   - `PayedConfirm`، `PayedOnly` و `PayedReverse`
   - Inquiry و بازیابی نتیجهٔ نامعلوم یا دیرهنگام
   - `PaymentService.UpdatePaymentStatusAsync`
   - فراخوانی `[shp].[SetOrderItemPriceRightShare]`
3. بررسی کن آیا Source یا تست موجود، رفتار این مسیرها را اثبات می‌کند. صرف وجود متد یا پارامتر را اثبات قرارداد درگاه تلقی نکن.
4. بررسی کن آیا می‌توان Payment موجود را با مبلغ `G` استفاده کرد، بدون ایجاد Payment دوم و بدون تغییر Schema، Entity یا Database Object.

### محدودیت‌ها

- هیچ فایل، کد، تنظیم یا دیتابیسی را تغییر نده.
- هیچ پرداخت یا درخواست واقعی به درگاه ارسال نکن.
- Build و Test اجرا نکن.
- پرداخت کامل با Wallet، Vibe و Hybrid خارج از محدوده‌اند.
- نتیجهٔ تأییدنشده را به‌عنوان واقعیت اعلام نکن.
- یافته‌ها را به سه گروه تقسیم کن: **اثبات‌شده از Source، نیازمند تأیید مالک درگاه، نیازمند تأیید مالک DB/مالی**.

### خروجی

یک گزارش کوتاه شامل:

1. جدول Gateها با وضعیت `Resolved / Unresolved / Blocked` و شواهد دقیق فایل و خط.
2. پرسش‌های دقیق و حداقلی برای مالک HekmatOnline و مالک DB/مالی.
3. بررسی امکان حفظ این ترتیب:\
   `Gateway Paid confirmed → Wallet Capture succeeded → outstanding debt = 0 → Order Complete`
4. مشخص کن برای جلوگیری از ساخت Payment دوم و آزادسازی اشتباه Reservation چه پیش‌شرط‌هایی لازم است.
5. در پایان یک تصمیم صریح بده: `GO` برای شروع پیاده‌سازی محدود یا `NO-GO`، همراه با دلایل.

گزارش را در `docs/decisions/` ذخیره کن. هیچ تغییری خارج از گزارش انجام نده.


---

## assistant
گزارش‌های تصمیم قبلی و مسیر HekmatOnline را با تمرکز بر Gateهای حل‌نشده بررسی می‌کنم. نتیجه را در یک فایل جدید در `docs/decisions/` ثبت می‌کنم؛ Build، Test و درخواست درگاه اجرا نمی‌شود.

---

## assistant
کد نشان می‌دهد Inquiry وجود دارد، اما در مسیر فعلی `PayedConfirm` پیش از Capture سفارش را Complete می‌کند و پاسخ نامعلوم یا مغایرت مبلغ در نهایت Unsuccessful ثبت می‌شود. وجود Inquiry به‌تنهایی Gate بازیابی امن را حل نمی‌کند. حالا امکان مصرف Payment موجود و اثر `Paid` بر Procedure سهم اقلام را جمع‌بندی می‌کنم.

---

## user
بریم ادامه کار


---

## assistant
[گزارش Gateهای HekmatOnline برای Mixed Payment](P:/Projects/walletProject/Wallet/backEnd/docs/decisions/HekmatOnline-Mixed-Payment-Go-No-Go-Gates.md) ثبت شد.

**تصمیم: NO-GO برای شروع پیاده‌سازی محدود مالی.** کد فعلی Payment دوم می‌سازد، Order را پیش از Wallet Capture کامل می‌کند و بعضی نتایج نامعلوم را ناموفق ثبت می‌کند. قرارداد پرداخت جزئی و نتیجهٔ قطعی درگاه، و رفتار Procedure سهم اقلام با مبلغ `G` نیز هنوز تأیید نشده‌اند.

---

## user

# Files pasted by the user:

## "# Task: Complete Wallet Payment Settlement, Refund, and Customer UI ## هدف قابل…": C:\Users\pc\.codex/attachments/281e50ea-ebe9-4f9f-82ed-2e560bce472a/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن پیوست را می‌خوانم و محدودهٔ دقیق کار را با وضعیت فعلی مخزن تطبیق می‌دهم. بعد، پیش‌نیازهای تصمیم‌گیری و بخش‌هایی را که می‌توان با اطمینان پیش برد مشخص می‌کنم.

---

## assistant
در متن پیوست، انجام یکپارچه و بدون توقف مرحله‌ای خواسته شده است. از workflow کیف پول پروژه استفاده می‌کنم و مسیرهای موجود را بررسی می‌کنم تا تغییرات مستقل و امن را انجام بدهم؛ هر وابستگی به قرارداد بیرونی یا رفتار نامعلوم دیتابیس را جداگانه ثبت می‌کنم.

---

## assistant
یک مانع مستقیم پیدا شد: محاسبهٔ بدهی فعلی فقط Paymentهای Paid را کم می‌کند و Wallet Capture را در بدهی Order منظور نمی‌کند. با `Payment.Total = G`، پس از Capture هم بدهی محاسبه‌شده به اندازهٔ `W` باقی می‌ماند؛ در عین حال پیوست تغییر همین محاسبه را ممنوع کرده و Complete را به بدهی صفر مشروط کرده است. مسیر Refund و UI را هم بررسی می‌کنم تا روشن شود کدام بخش‌ها مستقل از این مانع قابل انجام‌اند.

---

## assistant
Refund هم به قرارداد تازه نیاز دارد: `ModifiedOrderItemService` شاخهٔ `PaymentMethodTypesEnum.Wallet` را به فرایند قدیمی ناجا/اقساط وصل می‌کند، در حالی که WalletProgram اعتبار را با Capture و Ledger ثبت می‌کند. اتصال خودکار این دو مسیر می‌تواند مبلغی را دوبار برگرداند. تا تعیین تکلیف قرارداد بدهی، سهم اقلام و finality درگاه، UI انتخاب کیف پول را فعال نمی‌کنم؛ این انتخاب فعلاً مبلغ قابل اتکایی برای پرداخت ندارد.

---

## assistant
پیاده‌سازی انتهابه‌انتها در محدودهٔ فعلی **مسدود است**. با `Payment.Total = G`، محاسبهٔ موجود بدهی فقط Paymentهای موفق را کم می‌کند؛ حتی پس از Wallet Capture، بدهی Order به اندازهٔ `W` باقی می‌ماند. بنابراین شرط «بدهی صفر پیش از Complete» بدون تغییر قرارداد محاسبه برقرار نمی‌شود.

[گزارش موانع و شواهد کد](P:/Projects/walletProject/Wallet/backEnd/docs/decisions/Wallet-End-to-End-Implementation-Blockers.md) ثبت شد. برای ادامه، قرارداد محاسبهٔ بدهی، پذیرش پرداخت جزئی و بازیابی نتیجه در HekmatOnline، رفتار Procedure سهم اقلام برای `G`، و finality بازپرداخت درگاه نیاز به تعیین تکلیف دارند.

هیچ کد یا UI تغییر نکرد و Build/Test اجرا نشد.

---

## user

# Files pasted by the user:

## "# Phase 4 — Mixed Payment Final Decisions ## Role Act as the senior software ar…": C:\Users\pc\.codex/attachments/53b4da21-70e3-403d-b7ce-4b152a9eb65e/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
تصمیم‌های نهایی Phase 4 را از پیوست می‌خوانم و با مانع‌های ثبت‌شده تطبیق می‌دهم. سپس کار مجاز این مرحله را بر همان مبنا ادامه می‌دهم.

---

## assistant
# Phase 4 — تصمیم‌های Mixed Payment

## 1. Confirmed Decisions

- برای هر تلاش: `T = W + G`، `0 < W < T`، `Payment.Total = G` و `WalletReservation.Amount = W`. Reservation به همان Payment متصل است؛ Retry همان تلاش باید همان رکوردها و مبالغ را بازیابی کند.
- Order فقط پس از **موفقیت قطعی درگاه، Capture موفق و اثبات بدهی صفر** می‌تواند `Complete` شود. نتیجهٔ نامعلوم مجوز `Release` یا ثبت شکست قطعی نیست.
- اگر درگاه Paid شد و Capture انجام نشد، موفقیت درگاه حفظ می‌شود؛ Capture باید ایمن بازیابی شود. پس از شکست قطعی Capture، مسیر موجود `IncompletePayment` مطرح است.
- تخصیص Refund تأییدشده **ابتدا سهم درگاه، سپس سهم Wallet** است. ثبت یا گروه‌بندی `RefundDetail` اثبات بازگشت پول نیست.
- HekmatOnline فقط **گزینهٔ نخستِ مشروط** است. پشتیبانی آن از پرداخت جزئی هنوز تأیید نشده است. Hybrid، Vibe و Full Wallet خارج از محدوده‌اند.

## 2. Questions Requiring My Decision

**Q1 — مجوز محاسبهٔ بدهی خالص Mixed**

- **Current situation:** محاسبهٔ فعلی Capture کیف پول را از بدهی Order کم نمی‌کند؛ بنابراین پس از پرداخت `G` و Capture مقدار `W`، بدهی محاسبه‌شده هنوز `W` است.
- **Exact decision required:** آیا ایجاد محاسبهٔ جداگانهٔ بدهی خالص برای Mixed، با حفظ قیمت‌گذاری و رفتار checkout عادی، مجاز است؟
- **Options:** (الف) محاسبهٔ جداگانه بر اساس Payment قطعی و Ledger کیف پول؛ (ب) حفظ محاسبهٔ فعلی بدون تغییر.
- **Recommended option and reason:** (الف)؛ بدون آن شرط بدهی صفر و `Complete` قابل تحقق نیست.
- **Consequences of each material option:** (الف) به آزمون اصلاح Order و Refund نیاز دارد؛ (ب) یعنی Mixed Payment فعلاً قابل پیاده‌سازی نیست.
- **Who must confirm it, if not me:** تصمیم محدودهٔ تغییر با شما؛ قواعد شناسایی عودت قطعی با مسئول مالی.
- **What implementation decision depends on the answer:** محاسبهٔ بدهی در Retry، تکمیل Order و اصلاح سفارش.

**Q2 — قرارداد پرداخت جزئی و بازیابی HekmatOnline**

- **Current situation:** Source ارسال `Amount` و Inquiry با Token را نشان می‌دهد؛ پذیرش `G<T`، معنای قطعی وضعیت‌ها و رفتار Retry را اثبات نمی‌کند.
- **Exact decision required:** مالک درگاه باید مجازبودن پرداخت جزئی و قرارداد Verify/Inquiry/Void را تأیید کند.
- **Options:** تأیید مستند HekmatOnline؛ یا کنارگذاشتن آن و ارزیابی درگاه بعدی.
- **Recommended option and reason:** فقط پس از تأیید مستند، HekmatOnline درگاه نخست باشد؛ مسیر Inquiry آن برای بازیابی مناسب‌تر به نظر می‌رسد.
- **Consequences of each material option:** تأیید، طراحی مسیر محدود را ممکن می‌کند؛ نبود تأیید، فعال‌سازی این درگاه را متوقف می‌کند.
- **Who must confirm it, if not me:** مالک/ارائه‌دهندهٔ HekmatOnline.
- **What implementation decision depends on the answer:** مبلغ Token، تشخیص نتیجهٔ قطعی، Retry و زمان مجاز Release.
- **پرسش آمادهٔ ارسال:** «آیا پذیرنده می‌تواند برای سفارشی با بدهی `T` فقط مبلغ `G<T` را با DTO فعلی و `InstallmentsCount` موجود دریافت کند؟ `PayedConfirm`، `PayedOnly`، `PayedReverse` و پاسخ Void دقیقاً چه قطعیت مالی دارند؟ پس از timeout یا پاسخ گم‌شده، با همان Token/ReservationNumber تا چه زمانی و با چه شناسه‌ای می‌توان نتیجه و مبلغ قطعی را استعلام کرد؟ تکرار درخواست با همان ReservationNumber آیا برداشت دیگری ایجاد می‌کند؟ Endpoint صحیح Void کدام است؟»

**Q3 — Procedure سهم اقلام با Payment جزئی**

- **Current situation:** هر بار Paid شدن Payment، `[shp].[SetOrderItemPriceRightShare]` اجرا می‌شود؛ تعریف و اثر آن با `Payment.Total=G<T` در Source در دسترس نیست.
- **Exact decision required:** آیا Procedure سهم اقلام را با Payment جزئی و Capture مستقل Wallet درست محاسبه می‌کند؟
- **Options:** سازگاری تأییدشده؛ یا نیاز به تغییر Procedure.
- **Recommended option and reason:** تعریف Procedure و یک نمونهٔ عددی از مالک DB گرفته و رفتار آن تأیید شود.
- **Consequences of each material option:** در حالت اول می‌توان ادامه داد؛ در حالت دوم، با ممنوعیت فعلی تغییر Database Object، Mixed مسدود می‌ماند.
- **Who must confirm it, if not me:** مالک DB و مسئول مالی Commerce.
- **What implementation decision depends on the answer:** امکان ثبت Paid و تخصیص درست سهم اقلام.
- **پرسش آمادهٔ ارسال:** «لطفاً تعریف `[shp].[SetOrderItemPriceRightShare]` و خروجی مورد انتظار آن را برای Order با `T=100`، `Payment.Total=60` و Wallet Capture برابر `40` ارائه کنید. آیا اجرای آن هنگام Paid شدن مبلغ ۶۰، پیش از Capture کیف پول، سهم اقلام را درست نگه می‌دارد؟»

**Q4 — قطعیت Refund و تغییر Order در وضعیت معلق**

- **Current situation:** `Done`/`Confirmed` مدرک واریز نیستند. همچنین سیاست تغییر یا لغو Order هنگام Verify یا Capture نامعلوم مشخص نشده است.
- **Exact decision required:** منبع قطعی نتیجهٔ Refund درگاه چیست، و آیا تغییر/لغو Order تا تعیین تکلیف تلاش پرداخت متوقف شود؟
- **Options:** (الف) Refund درگاه فقط پس از تأیید قابل تطبیق در محاسبات اثر کند و تغییر Order در وضعیت نامعلوم معلق بماند؛ (ب) ثبت درخواست Refund یا وضعیت میانی مبنای محاسبه و ادامهٔ تغییر Order باشد.
- **Recommended option and reason:** (الف)؛ مانع دوباره‌کسرشدن، دوباره‌پرداخت و تغییر بدهی بر پایهٔ نتیجهٔ نامعلوم می‌شود.
- **Consequences of each material option:** (الف) ممکن است اصلاح سفارش را تا تعیین تکلیف به تأخیر بیندازد؛ (ب) خطر تعهد مالی نادرست دارد.
- **Who must confirm it, if not me:** سیاست توقف تغییر Order با شما و مسئول مالی؛ رویداد قطعی Refund با مسئول فرایند Refund و ارائه‌دهندهٔ درگاه.
- **What implementation decision depends on the answer:** زمان کاهش خالص پرداخت درگاه، تخصیص gateway-first و زمان Wallet Refund.
- **پرسش آمادهٔ ارسال:** «برای Refund یک Payment موفق HekmatOnline، کدام پاسخ یا سندِ دارای شناسه، مبلغ و زمان، بازگشت قطعی پول به مشتری را ثابت می‌کند؟ در timeout یا نتیجهٔ نامعلوم چگونه بدون ارسال دوبارهٔ Refund تطبیق انجام می‌شود؟»

## 3. Technical Facts Established from Source

- [OrderService.CalcAmountForPayAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412) فقط خروجی [GetOrderTotalPayment](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) را کم می‌کند؛ Capture را نمی‌خواند.
- [GenerateTokenAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:417) Payment تازه می‌سازد. مسیرهای Callback و Inquiry، Order را پیش از Capture به `Complete` می‌برند (همان فایل، خطوط 633–651 و 759–773).
- [WalletOrderPreparationService.PrepareAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:136) Reserve را بدون `PaymentId` می‌خواند، در حالی که [ReserveAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:33) Payment موجود می‌خواهد. اصلاح این مسیر یک کار فنی مرحلهٔ پیاده‌سازی است.
- [PaymentService.UpdatePaymentStatusAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:119) هنگام Paid شدن، Procedure سهم اقلام را اجرا می‌کند. [AddWalletRefundDetail](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:597) به مسیر قدیمی Payment/اقساط ناجا مربوط است و قرارداد Refund کیف پول برنامه را ثابت نمی‌کند.

## 4. Implementation Readiness

- **تأیید مالی/درگاه:** پاسخ Q1، قرارداد مکتوب Q2، و رویداد قطعی Refund و سیاست وضعیت معلق در Q4.
- **تأیید DB/Procedure:** پاسخ Q3 و تأیید Index یکتای Reservation در محیط هدف؛ تأیید قبلی فقط برای `IsiClubCommerceLocal` گزارش شده است.
- **بررسی فنی هنگام برنامه‌ریزی Implementation:** تراکنش مشترک Payment/Reservation، کلید پایدار تلاش، کنترل هم‌زمانی Order، بازیابی Callback/Inquiry، انقضای Reservation و آزمون حفظ checkout عادی. این‌ها تصمیم‌های اجرایی‌اند و سؤال کسب‌وکار تازه نمی‌خواهند.

## 5. Stop Condition

**وضعیت فعلی: NO-GO برای Implementation.** این مرحله در همین گزارش متوقف می‌شود. هیچ فایل، کد یا دیتابیسی تغییر نکرد و Build/Test یا عملیات پرداخت اجرا نشد.

---

## user
<environment_context>
  <current_date>2026-10-17</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user

# Files pasted by the user:

## "# Task: Complete Wallet Mixed Payment End-to-End ## نقش و هدف به‌عنوان Senior .…": C:\Users\pc\.codex/attachments/532c2dad-bdf6-469f-a229-91e80e87b4da/Pasted text.txt

Pasted text contains the user's request.

## My request:



---

## assistant
متن پیوست جدید را می‌خوانم و تصمیم‌های آن را با Gateهای باز Phase 4 تطبیق می‌دهم. سپس طبق محدوده‌ای که تعیین کرده، کار را ادامه می‌دهم.

---

## assistant
پیوست جدید چند قاعدهٔ مالی را مشخص کرده و اجازه داده بخش‌های مستقل را پیش ببرم. ابتدا وضعیت Git و قراردادهای فعلی Refund، Settlement و Checkout را هدفمند بررسی می‌کنم تا تغییرات موجود کاربر حفظ شوند و فقط شکاف‌های قابل پیاده‌سازی را تغییر بدهم.

---

## assistant
مسیر مستقل و امن برای نمایش مانده پیدا شد: می‌توان فقط Assignmentهای کاربر جاری را در Application جاری خواند و مانده‌ها را بدون ایجاد Payment یا تغییر Ledger نمایش داد. این بخش را در صفحهٔ کاربری موجود اضافه می‌کنم. انتخاب کیف پول در Checkout و اتصال درگاه همچنان به قراردادهای مالی باز وابسته‌اند.

---

## assistant
## Handoff — Mixed Wallet Payment End-to-End

### وضعیت فعلی

درخواست فعال، تکمیل Mixed Payment است. کاربر خواسته بخش‌های مستقل ادامه یابند، اما برای قرارداد مالی یا درگاهِ تأییدنشده حدس زده نشود. **هنوز هیچ درگاه Mixed فعال نشده است.**

بخش مستقلِ نمایش مانده کیف پول پیاده‌سازی شد:

- سرویس خواندنی، Assignmentهای کاربر و Application جاری را می‌خواند و تخصیص‌یافته، مصرف‌شده، رزروشده، منقضی‌شده و قابل استفاده را محاسبه می‌کند.
- صفحهٔ MVC «کیف پول من» و لینک آن در دو منوی کاربری اضافه شد.
- دسترسی به `OrderController.Detail` به Order متعلق به کاربر و Application جاری محدود شد.

فایل‌های این تغییر: [DTO](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/CustomerWalletBalanceDto.cs)، [interface](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/ICustomerWalletBalanceService.cs)، [service](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/CustomerWalletBalanceService.cs)، [controller](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/MyWalletController.cs)، [view](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/MyWallet/Index.cshtml)، `ModuleInitializer.cs`، `_Layout.cshtml` و `OrderController.cs`.

### موانع مالی که باید حفظ شوند

- HekmatOnline هنوز تأیید بیرونی برای دریافت `G<T`، قطعیت Inquiry/Void و Retry ندارد؛ [گزارش Gateها](P:/Projects/walletProject/Wallet/backEnd/docs/decisions/HekmatOnline-Mixed-Payment-Go-No-Go-Gates.md).
- `CalcAmountForPayAsync` فقط Paymentهای Paid را کم می‌کند و Capture کیف پول را در بدهی نمی‌بیند؛ [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412).
- `WalletOrderPreparationService.PrepareAsync`، Reserve را بدون `PaymentId` فرا می‌خواند، ولی Reserve به Payment موجود نیاز دارد؛ [WalletOrderPreparationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:136).
- هنگام Paid، `[shp].[SetOrderItemPriceRightShare]` اجرا می‌شود؛ تعریف و رفتار آن برای `Payment.Total=G` تأیید نشده است.
- تصمیم جدید Q7، `SendForModifyPrice=1213` و `Done=1214` را برای Refund قطعی می‌داند؛ اما Source، `Done` را هنگام گروه‌بندی ثبت می‌کند. این تعارض باید پیش از محاسبه یا اجرای Refund مالی مشخص شود. `ModifiedOrderItemService.AddWalletRefundDetail` نیز مسیر قدیمی ناجا/اقساط است، نه WalletProgram.
- Vibe، Hybrid و Full Wallet خارج از محدوده‌اند. Entity، Mapping، Schema و Database Object نباید تغییر کنند.

### Verification

`dotnet build Modules/YashilCommerce/YashilCommerce.Web/YashilCommerce.Web.csproj --no-restore` اجرا شد و با **NETSDK1004** به‌علت نبود `obj/project.assets.json` متوقف شد. تست مرتبط پیدا و اجرا نشد. `git diff --check` برای فایل‌های tracked تغییرکرده موفق بود. هیچ پرداخت یا درخواست درگاه اجرا نشد.

### گام بعد

ابتدا تغییرات تازه را از نظر کامپایل و رفتار صفحهٔ مانده، با محدودیت نبود assets، بررسی کن. سپس فقط مسیرهای مالی‌ای را پیش ببر که قراردادشان از Source یا تصمیم‌های جدید ثابت است؛ Checkout انتخاب Wallet، Redirect، Capture/Release و Refund را بر پایهٔ فرضِ تأیید درگاه، Procedure یا finality فعال نکن. تغییرات موجود کاربر در Git بسیار گسترده و عمدتاً uncommitted هستند؛ آن‌ها را بازنویسی نکن.

---

## assistant
متن کامل درخواست جدید و وضعیت فعلی مخزن را می‌خوانم، سپس بخش‌های قابل اجرای Mixed Payment را از مواردی که به قرارداد مالی یا درگاه وابسته‌اند جدا می‌کنم.

---

## assistant
برای بخش مالی، دستورالعمل `wallet-feature` را اعمال می‌کنم. ابتدا قراردادهای فعلی را با کد جاری تطبیق می‌دهم و فقط مسیرهایی را تغییر می‌دهم که بدون فرض تازه دربارهٔ درگاه یا بازپرداخت قابل اثبات‌اند.

---

## assistant
بخش مستقلِ نمایش اطلاعات مالی سفارش را تکمیل کردم، اما **Mixed Payment هنوز قابل فعال‌سازی نیست** و هیچ درگاهی برای آن پشتیبانی‌شده اعلام نمی‌شود.

در [سرویس خلاصه تأمین مالی سفارش](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/CustomerOrderFundingService.cs)، Paymentهای `Paid` و مصرف خالص Wallet از `Capture − WalletRefund` فقط برای سفارش‌های کاربر و Application جاری خوانده می‌شوند. این داده‌ها به [تاریخچه سفارش](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderListItem.cshtml) و [جزئیات سفارش](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummary.cshtml) وصل شدند. مبلغ Payment به‌صراحت «ثبت‌شده پیش از بازپرداخت احتمالی» نمایش داده می‌شود؛ به‌عنوان وجه خالص درگاه یا بدهی باقی‌مانده محاسبه نشده است. صفحهٔ «کیف پول من» از کار قبلی موجود بود.

بخش‌های مالی را فعال نکردم، چون [آماده‌سازی فعلی](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:130) Reserve را بدون `PaymentId` لازم فرا می‌خواند؛ [محاسبه بدهی](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412)، Capture کیف پول را لحاظ نمی‌کند؛ و [محاسبه Payment](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) قرارداد قابل اتکای Refund قطعی برای Mixed ندارد. همچنین پذیرش `G<T` و بازیابی نتیجهٔ نامعلوم HekmatOnline و اثر Procedure سهم اقلام تأیید نشده‌اند. بنابراین Checkout انتخاب Wallet، Redirect، Settlement خودکار و Refund ترکیبی تغییری نکردند؛ Vibe، Hybrid و Full Wallet نیز خارج از محدوده ماندند.

`git diff --check` موفق بود. Build پروژهٔ Web با `NETSDK1004` به‌دلیل نبود `project.assets.json` متوقف شد؛ تستی اجرا نشد. **در این اجرا هیچ Entity، Mapping، Schema، Migration یا Database Object تغییر نکرد.**

---

## user
الان ازت میخوام کارو ادامه بدی تا پایان و پیاده سازی vibe و hekmat online رو هم انجام بدی اگر مانعی هست بهم بگو


---

## assistant
مسیرهای فعلی HekmatOnline و Vibe را تا ساخت Payment، درخواست درگاه و تأیید نتیجه بررسی می‌کنم. سپس بخش‌هایی را که با قرارداد موجود قابل اثبات‌اند پیاده‌سازی می‌کنم؛ اگر پرداخت جزئی یا بازیابی نتیجهٔ نامعلوم به تأیید بیرونی وابسته باشد، همان مانع را دقیق گزارش می‌دهم.

---

## assistant
مسیر Vibe و HekmatOnline را در کد فعلی بررسی کردم. **پیاده‌سازی و فعال‌سازی Mixed Payment هنوز مانع واقعی دارد**؛ در این نوبت تغییر مالی ناقص یا قابل Redirect ایجاد نکردم.

- **Vibe:** [درخواست فعلی](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:68)، `cart_amount` را از مبلغ Payment می‌گیرد، اما `goods_amount`، مالیات و اقلام را از کل Order می‌سازد ([همان فایل](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:370)). با `Payment.Total = G`، درخواست هم‌زمان مبلغ `G` و جزئیات سبد `T` را ارسال می‌کند. قرارداد مجاز این اختلاف در مخزن موجود نیست.
- **HekmatOnline:** [ساخت Token](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:417) هر بار Payment تازه می‌سازد. پذیرش `G<T`، قطعیت نتیجه پس از timeout و رفتار Retry با `ReservationNumber` هنوز تأیید نشده‌اند. مسیر موفق فعلی نیز [Order را پیش از Capture کامل می‌کند](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:633).
- **مانع مشترک مالی:** [بدهی Order](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412) سهم Capture را کم نمی‌کند. همچنین `PaymentService` هنگام `Paid`، Procedure سهم اقلام را اجرا می‌کند؛ رفتار آن برای `G<T` در Source اثبات نشده است. تصمیم «Refund قطعی در `Done`» نیز با کد فعلی تعارض دارد: [Repository هنگام گروه‌بندی، `Done` ثبت می‌کند](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:153)، بدون شاهد بازگشت وجه.

برای ادامهٔ امن، تأیید قرارداد پرداخت جزئی و بازیابی نتیجه از مالکان **Vibe و HekmatOnline**، تعریف و رفتار `[shp].[SetOrderItemPriceRightShare]` از مالک **DB/مالی**، و تعیین تکلیف معنای مالی `RefundDetail.Done` لازم است. تا آن زمان هیچ‌کدام را برای Mixed پشتیبانی‌شده اعلام نمی‌کنم. در این نوبت فایلی تغییر نکرد و Build/Test اجرا نشد.

---

## user

# Files pasted by the user:

## "سند تصمیم‌های کسب‌وکار و الزامات بررسی فنی Mixed Payment نسخه آماده ارائه به Co…": C:\Users\pc\.codex/attachments/d5849e47-e13d-4f0c-b126-e83fcd8636ad/Pasted text.txt

## My request:
ببین جواب چند تا از سوال هات اینجا هست


---

## assistant
سند جدید را با موانع Vibe، HekmatOnline، بدهی سفارش و Refund تطبیق می‌دهم. هر تصمیمی که در آن قطعی شده باشد مبنای ادامهٔ کار قرار می‌گیرد.

---

## assistant
سند چند تصمیم را روشن می‌کند، از جمله فرمول بدهی، ترتیب Refund و محدودهٔ پرداخت جزئی. خود سند این مرحله را «بررسی بدون تغییر کد» تعیین کرده است؛ بنابراین فعلاً فقط تطبیق Q1 تا Q11 با Source را کامل می‌کنم و مواردی را که هنوز مانع اجرا هستند مشخص می‌کنم.

---

## assistant
بله؛ سند چند **تصمیم کسب‌وکار** را روشن می‌کند، اما خودش این مرحله را «بررسی بدون تغییر کد» تعیین کرده و **Vibe را خارج از محدوده** گذاشته است. بنابراین این پاسخ نتیجهٔ تطبیق با Source است؛ فایلی تغییر نکرد.

| مورد | وضعیت نسبت به کد فعلی | نتیجه |
|---|---|---|
| Q1 ـ Payment و Reserve اتمیک | ناسازگار | [ساخت Payment](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:552) و [Reserve](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletReservationService.cs:63) تراکنش‌های مستقل دارند؛ [آماده‌سازی فعلی](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:130) هم `PaymentId` لازم را نمی‌فرستد. |
| Q2 ـ Timeout و نبود Callback | مسدود | سیاست عملیاتی مشخص است، ولی نتیجهٔ نامعلوم می‌تواند با موفقیت واقعی درگاه هم‌زمان باشد. [Release](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:44) قطعیت شکست Payment را کنترل نمی‌کند؛ اتصال امن آن به Callback/لغو سیستمی وجود ندارد. |
| Q3 ـ شکست Capture | مسدود | `IncompletePayment` در [چرخهٔ Order](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:220) وجود دارد و Capture در Service تکرارپذیر طراحی شده است، اما به مسیر تأیید درگاه و بازیابی پس از خطا متصل نیست. Capture پس از [انقضای Reservation](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:165) رد می‌شود. |
| Q4 ـ Procedure سهم اقلام | قابل اثبات نیست | تصمیم مالک «فقط گزارش‌گیری» است. کد هنگام `Paid` آن را [فرا می‌خواند](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:119)، اما تعریف و اثر دیتابیسی Procedure در Source موجود نیست. |
| Q5 ـ Lock و idempotency | ناسازگار | Lock سبد یک [دیکشنری static در همان پردازه](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:41) است. یکتایی `IdempotencyKey` در [Mapping](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:28105) هست، ولی کلید تلاش اولیه و تراکنش مشترک Payment/Reserve هنوز پیاده نشده‌اند. تأیید فیزیکی Index قبلی فقط مربوط به `IsiClubCommerceLocal` است. |
| Q6 ـ بدهی و Overpayment | ناسازگار | فرمول کسب‌وکار روشن شد. [محاسبهٔ بدهی](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:412) Capture/WalletRefund را نمی‌بیند و [جمع Payment](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) Refund را بدون فیلتر وضعیت قطعی اعمال می‌کند. |
| Q7 ـ وضعیت Refund قطعی | ناسازگار | IDها قطعی‌اند: `SendForModifyPrice=1213` و `Done=1214` در [Enum](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Enums/YashilEnums.cs:35). محاسبهٔ فعلی هر RefundDetail را لحاظ می‌کند؛ افزون بر آن، [گروه‌بندی، `Done` ثبت می‌کند](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:153). این تفاوت معنای کد با تصمیم کسب‌وکار، ریسک مالی مشخصی است؛ تصمیم Q7 را تغییر ندادم. |
| Q8 ـ Gateway-first | ناسازگار | [اصلاح سفارش](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:509) ابتدا شاخهٔ Wallet قدیمی ناجا را اجرا می‌کند. آن شاخه، WalletProgram و Ledger آن نیست. اتصال Gateway-first به [WalletRefundService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletRefundService.cs:78) وجود ندارد. |
| Q9 ـ معنای پرداخت جزئی | تصمیم روشن؛ قرارداد درگاه قابل اثبات نیست | منظور، پرداخت یک‌مرحله‌ایِ کل `G` است. [HekmatOnline فقط مبلغ Payment را به Token می‌فرستد](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:63)؛ پذیرش `G<T` از Source اثبات نمی‌شود. |
| Q10 ـ Confirm و Inquiry | ناسازگار | [Callback HekmatOnline](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:633) Confirm را فرا می‌خواند، اما در موفقیت Order را پیش از ثبت Paid و Capture کامل می‌کند؛ خطا/Timeout نیز ممکن است محلی ناموفق ثبت شود. [مسیر Inquiry در کد وجود دارد](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:739)، هرچند سند می‌گوید در فرایند عملیاتی استفاده نشود. ادعای بازگشت وجه در ۷۲ ساعت از کد قابل اثبات نیست. |
| Q11 ـ Retry و Void | مسدود | `ReservationNumber` در [Callback برای یافتن Payment](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:481) استفاده می‌شود؛ اما [GenerateTokenAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:417) در Retry، Payment تازه می‌سازد. تضمین «همان شناسه، بدون برداشت دوباره» و قطعیت [Void](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:946) از قرارداد موجود قابل اثبات نیست. |

**مانع‌های باقی‌مانده برای اجرا:** تأیید پذیرش `G<T` و رفتار Retry/Void از مالک HekmatOnline؛ تعریف و اثر Procedure از مالک DB؛ تعیین کنترل عملیاتیِ نتیجهٔ نامعلوم پیش از Release؛ و رفع تعارض اجرای `Done` با تصمیم Q7. برای Vibe نیز این سند پاسخی نمی‌دهد و صریحاً آن را از این مرحله خارج می‌کند؛ در کد فعلی، `cart_amount` از Payment و جزئیات سبد از کل Order ساخته می‌شوند.

ریسک‌های اصلی همان پرداخت موفق درگاه با سفارش محلی ناموفق، Release زودهنگام، بدهی و Overpayment نادرست، و Refund دوباره هستند. مرحلهٔ بعد پس از رفع این موارد، پیاده‌سازی محاسبهٔ مالی قابل آزمون و تلاش اتمیک Payment/Reservation است؛ سپس اتصال یک درگاه و Recovery. طبق دستور صریح همین سند، **کد، دیتابیس، Build و Test در این مرحله تغییر یا اجرا نشدند**.

---

## user
<environment_context>
  <current_date>2026-10-18</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user
## Execution Policy — Maximize Completion, Minimize Token Usage

هدف، تکمیل حداکثری تمام قابلیت‌های درخواستی در یک اجرای پیوسته است؛ با حداقل مصرف توکن، حداقل رفت‌وبرگشت و بدون گزارش‌های میانی غیرضروری.

1. **Implementation first:** مستقیماً پیاده‌سازی را ادامه بده. ممیزی مجدد، ارائه طرح تکراری، گزارش پیشرفت و درخواست تأیید برای تصمیم‌هایی که قبلاً مشخص شده‌اند ممنوع است.
2. **Do not stop at the first blocker:** اگر در بخشی با خطا، ابهام یا محدودیت مواجه شدی، ابتدا خودت کد مرتبط، الگوهای موجود و راهکارهای سازگار را بررسی کن. راهکار متعارف و کم‌ریسک را انتخاب کن و ادامه بده؛ منتظر پاسخ کاربر نمان.
3. **Continue independent work:** اگر یک بخش واقعاً مسدود شد، فقط همان بخش را موقتاً کنار بگذار و تمام بخش‌های مستقل و قابل اجرا را تکمیل کن. وابستگی‌های آن بخش را مشخص کن و پس از تکمیل سایر بخش‌ها، در صورت امکان دوباره به آن برگرد.
4. **Resolve routine decisions autonomously:** برای انتخاب بین راهکارهای متعارف معماری، نام‌گذاری، ساختار سرویس‌ها، مدیریت خطا، تست و یکپارچه‌سازی سؤال نپرس. مطابق الگوهای موجود پروژه تصمیم بگیر. تصمیم‌های قطعی کسب‌وکار این گفتگو را تغییر نده.
5. **No speculative blockers:** نبود تست، دشواری Build، نامشخص بودن یک جزئیات غیرضروری یا محدودیت احتمالی یک درگاه، به‌تنهایی مجوز توقف کل کار نیست. ابتدا پیاده‌سازی و اعتبارسنجی‌های ممکن را انجام بده و محدودیت واقعی را جداگانه ثبت کن.
6. **Respect hard constraints:** محدودیت‌های صریح پروژه، به‌ویژه ممنوعیت تغییر Schema، Entity، Mapping، Migration، DDL و Database Objects، همچنان لازم‌الاجرا هستند. برای عبور از مانع، این محدودیت‌ها را نقض نکن.
7. **No false completion:** چیزی را که پیاده‌سازی یا آزمایش نشده، تکمیل‌شده اعلام نکن. اگر بخشی واقعاً قابل تکمیل نیست، دقیقاً علت، محل کد، وابستگی مسدودکننده و اقدامات انجام‌شده را ثبت کن؛ سپس بقیه کار را کامل کن.
8. **Single final report:** فقط پس از اتمام تمام کارهای ممکن، یک گزارش نهایی کوتاه ارائه بده که شامل پیاده‌سازی‌های انجام‌شده، فایل‌های مهم تغییرکرده، تست‌ها و Build واقعی، و موارد مسدودشده باقی‌مانده باشد. برای موارد مسدودشده، راهکار پیشنهادی را نیز مشخص کن.

**Completion criterion:** کار زمانی تمام است که همه قابلیت‌های مستقل قابل اجرا پیاده‌سازی شده باشند، یکپارچه‌سازی نهایی بررسی شده باشد و فقط موانع واقعی و حل‌نشده باقی مانده باشند؛ نه زمانی که اولین مانع مشاهده می‌شود.


---

## assistant
کار را از آماده‌سازی اتمیک تلاش Mixed و محاسبهٔ مالی قابل استفادهٔ مجدد ادامه می‌دهم. مسیر درگاه‌ها را فقط جایی فعال می‌کنم که مبلغ، Retry و نتیجهٔ پرداخت با قرارداد موجود قابل اتکا باشند؛ تغییرات Schema و Entity انجام نمی‌دهم.

---

## assistant
## Handoff: Mixed Wallet Payment

### وضعیت کار

کاربر خواسته پیاده‌سازی تا حد ممکن در یک اجرای پیوسته ادامه یابد، بدون توقف در اولین مانع. ممنوعیت تغییر Entity، Mapping، Schema، Migration و Database Object برقرار است. تغییرات گستردهٔ قبلی در working tree وجود دارند و نباید بازنویسی شوند. Vibe و HekmatOnline همچنان قرارداد تأییدشده‌ای برای فعال‌سازی Mixed ندارند.

در این نوبت:

- [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261): برای پرداخت‌های شتابی و HekmatOnline، فقط Refund با وضعیت `SendForModifyPrice=1213` یا `Done=1214` در مبلغ پرداخت خالص اثر می‌کند؛ مبلغ نهایی نامعتبر رد می‌شود.
- [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:439): خالص Ledger کیف پول، یعنی `Capture − WalletRefund`، از بدهی قابل پرداخت کسر می‌شود؛ Reservation کسر نمی‌شود. `YashilAppDbContext` به constructor اضافه شده است.
- [WalletSettlementService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:174): Release برای Payment با وضعیت `Paid` رد می‌شود.
- [WalletOrderPreparationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs): پیاده‌سازی قبلی که Reserve را بدون `PaymentId` فرا می‌خواند، با آماده‌سازی Payment و Reservation در یک تراکنش `Serializable` جایگزین شد. کلید GUID درخواست در `WalletReservation.IdempotencyKey` ذخیره می‌شود؛ `sp_getapplock` برای کلید تلاش و Order استفاده شده است. Assignment/Eligibility، بودجهٔ قابل استفاده، `0<W<T`، Replay و تعارض بررسی می‌شوند. فعلاً فقط `HekmatOnline` در این سرویس پذیرفته می‌شود.
- [WalletOrderPreparationDto.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletOrderPreparationDto.cs): `AttemptKey`، `PaymentMethodType` و شناسه/وضعیت Payment و `CanRedirect` به قرارداد افزوده شده‌اند.

### بررسی فوری لازم

**پیش از ادامهٔ اتصال درگاه، پیاده‌سازی جدید Preparation را هدفمند بررسی و اصلاح کن.** Build به‌علت نبود `project.assets.json` با `NETSDK1004` متوقف شد؛ کامپایل آن تأیید نشده است. سرویس از طریق [WalletController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs) در دسترس است؛ ورودی و Authorization این endpoint و اینکه مصرف‌کننده `CanRedirect` را enforce می‌کند، باید بررسی شوند. در مسیرهای بازگشت زودهنگام پس از `SaveChanges` یا `ExecuteUpdate`، تراکنش با dispose بدون Commit باید Rollback شود؛ این رفتار و مدیریت برخورد کلید یکتا را تأیید کن. هماهنگی `sp_getapplock` با مسیر عادی ساخت Payment نیز هنوز اثبات نشده است.

`git diff --check` برای فایل‌های tracked تغییرکرده موفق شد؛ فایل Preparation و Settlement از قبل untracked بوده‌اند و در diff معمول Git دیده نمی‌شوند. تست مرتبطی پیدا یا اجرا نشد.

### موانع و گام‌های بعد

1. بررسی دقیق کد تازه، خطاهای احتمالی کامپایل و تراکنش، سپس آزمون‌های معنادار برای Replay، رقابت هم‌زمان، Rollback و `T=W+G` در صورت فراهم‌شدن زیرساخت.
2. اتصال Checkout و HekmatOnline به **همان Payment آماده‌شده**؛ مسیر فعلی `GenerateTokenAsync` Payment دوم می‌سازد. هیچ Redirect پیش از Commit مجاز نیست.
3. Settlement مشترک پس از Verify قطعی، با ترتیب `Paid → Capture → بدهی صفر → Complete` و Recovery شکست Capture. Callback فعلی HekmatOnline Order را زود Complete می‌کند.
4. Vibe: کد فعلی `cart_amount` را از Payment و `goods_amount`/اقلام را از کل Order می‌سازد؛ قرارداد ارائه‌دهنده برای `G<T` و نمایش اقلام هنوز لازم است. HekmatOnline نیز نیازمند تأیید پذیرش `G<T` و معنای Retry/Timeout/Confirm/Void است.
5. Refund ترکیبی و اصلاح سفارش هنوز به Gateway-first و Wallet Ledger متصل نیستند. `Done` طبق تصمیم کسب‌وکار قطعی محسوب می‌شود، اما Source آن را هنگام گروه‌بندی ثبت می‌کند؛ ریسک مالی را پنهان نکن.
6. رفتار `[shp].[SetOrderItemPriceRightShare]` برای Payment جزئی از Source قابل اثبات نیست. هیچ درگاه Mixed یا UI انتخاب Wallet فعال‌شده اعلام نشود.

---

## assistant
ادامهٔ پیاده‌سازی را از وضعیت فعلی مخزن و تصمیم‌های ثبت‌شده پیش می‌برم. ابتدا اتصال Payment آماده‌شده به مسیر Checkout و Verify را بررسی می‌کنم، سپس بخش‌های مستقل قابل اجرا را تکمیل و اعتبارسنجی می‌کنم.

---

## assistant
برای این کار از راهنمای `wallet-feature` و الگوی Backend مخزن استفاده می‌کنم. سند پیوست تصمیم‌های مالی را روشن کرده؛ بخش‌های نیازمند تأیید قرارداد درگاه را فعال نمی‌کنم و کار مستقل Backend را پیش می‌برم.

---

## assistant
یک ناسازگاری مستقیم در مسیر بازیابی پیدا شد: `Capture` پس از پایان مهلت Reservation رد می‌شود، حتی اگر Payment قطعی `Paid` شده باشد؛ Job انقضا هم می‌تواند همان Reservation باز را منقضی کند. این دو مسیر را برای حفظ امکان Retry امن پس از Verify اصلاح می‌کنم.

---

## assistant
مسیر Checkout فعلی سرویس آماده‌سازی Mixed را مصرف نمی‌کند؛ ورود مستقیم به درگاه HekmatOnline همچنان Payment دوم می‌سازد. بنابراین پاسخ `CanRedirect` و endpoint فعلی نباید به کاربر اجازه تصورِ Redirect امن بدهند. دسترسی اجرایی این endpoint را تا اتصال اتمیک مسیر درگاه می‌بندم؛ سرویس آماده‌سازی برای تکمیل Backend باقی می‌ماند.

---

## assistant
پیاده‌سازی Mixed Payment هنوز کامل و فعال نشده است. بخش‌های مستقلی که بدون تغییر Schema قابل انجام بود، برای جلوگیری از تسویه یا Redirect ناامن اصلاح شد:

- در [WalletSettlementService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletSettlementService.cs:157)، `Capture` برای Payment قطعی `Paid` پس از پایان مهلت Reservation نیز قابل تکرار است. `Release` فقط برای وضعیت‌های `Unsuccessful`، `Cancel` و `Void` مجاز است.
- در [WalletProgramService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletProgramService.cs:274)، انقضای Assignment تا تعیین تکلیف Reservation متصل به Payment باز یا موفق به تعویق می‌افتد.
- در [WalletOrderPreparationService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:244) مقدار `CanRedirect` غیرفعال شد و [endpoint رزرو](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletController.cs:13) پیش از ایجاد رکورد پاسخ تعارض می‌دهد.

**مانع فعال‌سازی:** مسیر فعلی HekmatOnline در `GenerateTokenAsync` [Payment تازه می‌سازد](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:417) و پس از Confirm، [Order را پیش از Wallet Capture کامل می‌کند](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:641). اتصال مستقیم آن به Preparation فعلی می‌تواند Payment دوم یا Order کامل با سهم Wallet مصرف‌نشده ایجاد کند. پذیرش مبلغ `G<T` نیز به تأیید قرارداد HekmatOnline نیاز دارد. در Vibe، `cart_amount` از مبلغ Payment ولی جزئیات کالا از کل Order ساخته می‌شود؛ سازگاری این درخواست با `G<T` هنوز تأیید نشده است. بنابراین هیچ‌یک برای Mixed فعال نشدند.

`git diff --check` برای فایل tracked تغییرکرده موفق بود. Build با `NETSDK1004` متوقف شد؛ تلاش Restore نیز به‌دلیل در دسترس نبودن بسته‌های `Yashil.Common.*` نسخهٔ `9.8.45` با `NU1102` شکست خورد. تست اجرایی انجام نشد. مرحلهٔ بعد، تأیید قرارداد مبلغ دو درگاه و سپس اتصال Redirect و Verify به **همان Payment** همراه با مسیر بازیابی Capture است.

---

## user
مشکل hekmatOnline را کامل با جزئیات بهم توضیح بده برای vibe هم مشکلی نیست و این که از order جزئیات کالا رو میخواند ارتباطی با cart_amount ندارد و مستقل هستند و مانع کار تو نیست پس فکر میکنم اگر مشکل دیگه ای نباشد میتوانی آن را تکمیل کنی


---

## assistant
در HekmatOnline دو مشکلِ قابل‌مشاهده در کد وجود دارد: شروع پرداخت Payment تازه می‌سازد، و Callback پیش از Capture کیف پول Order را `Complete` می‌کند. مسیرهای دقیق خطا و Retry آن را هم بررسی می‌کنم. دربارهٔ Vibe، استقلال جزئیات کالا از `cart_amount` را طبق توضیح شما مبنا می‌گیرم و اتصال Mixed را پیش می‌برم.

---

## assistant
بررسی Vibe نشان داد مانع اصلی آن مبلغ اقلام نیست؛ مشکل اتصال جریان است: `Index` همیشه Payment جدید می‌سازد و `VerifyVibeOrderAsync` پیش از ثبت `Paid`، Order را `Complete` می‌کند. در خطاهای Verify نیز وضعیت ناموفق ثبت می‌شود، حتی وقتی نتیجه درخواست قطعی نیست. برای Mixed باید این مسیرها فقط برای Payment دارای Reservation تغییر کنند و پرداخت عادی دست‌نخورده بماند.

---

## assistant
اتصال Vibe اکنون در کد، Payment و Reservation یک تلاش را به هم وصل می‌کند: Checkout کلید تلاش را می‌فرستد، آماده‌سازی مبلغ `G` را در همان Payment ثبت می‌کند، و مسیر Vibe با `paymentId` همان Payment را مصرف می‌کند. Callback مختص Mixed نیز ابتدا `Paid` را ثبت می‌کند، سپس Capture را تکرارپذیر اجرا می‌کند و فقط با بدهی صفر Order را کامل می‌کند. اکنون خطاهای کامپایل و مسیرهای Retry را بررسی می‌کنم.

---

## assistant
حق با شماست: در کد Vibe، `cart_amount` از مبلغ Payment گرفته می‌شود و جزئیات کالا جداگانه از Order. فرض قبلی من که این دو الزاماً باید برابر باشند، مانع معتبری نبود.

## مشکل HekmatOnline دقیقاً چیست؟

برای مثال، اگر بدهی سفارش `T=100`، سهم کیف پول `W=30` و سهم درگاه `G=70` باشد، آماده‌سازی Mixed یک Payment با مبلغ ۷۰ و یک Reservation با مبلغ ۳۰ می‌سازد. اما مسیر فعلی [HekmatOnlinePaymentGatewayService.GenerateTokenAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:417) فقط `orderId` می‌گیرد و در خط ۴۲۱ با `CreateNewPaymentAndReturnAsync` **Payment تازه‌ای** می‌سازد. چون Reservation هنوز پرداخت قطعی نیست، محاسبه بدهی می‌تواند همچنان ۱۰۰ باشد. در نتیجه این مسیر Payment آماده‌شدهٔ ۷۰ تومانی را مصرف نمی‌کند و خطر مطالبهٔ دوباره وجود دارد.

حتی اگر ساخت Payment دوم اصلاح شود، مسیر نتیجه نیز باید تغییر کند:

1. پس از `ConfirmPay` موفق، کد فعلی [Order را `Complete` می‌کند](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:633) و سپس Payment را `Paid` می‌کند؛ Wallet Capture و اثبات بدهی صفر میان این دو وجود ندارد.
2. اگر ثبت محلی پس از Confirm خطا بدهد، [مسیر خطا Payment را ناموفق ثبت می‌کند](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:661)، در حالی که نتیجهٔ سمت درگاه ممکن است قطعی شده باشد.
3. مسیر Inquiry برای `PayedConfirm` نیز [مستقیماً Order را Complete می‌کند](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:759). این مسیر هم به Capture نیاز دارد.
4. در [اعتبارسنجی Callback](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:286)، بررسی امضا کامنت شده و مبلغ فقط از نظر مثبت‌بودن بررسی می‌شود؛ برابری آن با `Payment.Total = G` ثابت نمی‌شود. کنترل Callback تکراری نیز [به وضعیت حافظهٔ برنامه متکی است](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:548).

اصلاح کدی Hekmat روشن است: مسیر Mixed باید `paymentId` آماده‌شده را بگیرد، همان `ReservationNumber` و مبلغ `G` را برای Token استفاده کند، Callback را با همان Payment تطبیق دهد و ترتیب `Paid → Capture → بدهی صفر → Complete` را در هر دو مسیر Callback و Inquiry اعمال کند. **تأیید پذیرش `G<T` و رفتار درخواست Token تکراری با همان `ReservationNumber` از قرارداد HekmatOnline هنوز لازم است**؛ به همین دلیل HekmatOnline را فعال نکردم.

## کاری که برای Vibe انجام شد

مسیر Mixed Vibe را در کد به Checkout، Payment آماده‌شده، Redirect و Verify متصل کردم. انتخاب Program در [صفحه پرداخت](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:171) نمایش داده می‌شود؛ [ثبت سفارش](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:648) همان کلید تلاش را برای ساخت اتمیک Payment و Reservation می‌فرستد. [VibePaymentController](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/VibePaymentController.cs:47) در شاخهٔ Mixed همان Payment را مصرف می‌کند و Payment دوم نمی‌سازد.

در [VibeService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:401)، نتیجهٔ نامعلوم Reservation را آزاد نمی‌کند. Verify موفق ابتدا Payment را `Paid` می‌کند، سپس Capture تکرارپذیر انجام می‌شود و Order فقط با بدهی صفر `Complete` می‌شود. برای Payment موفقی که Capture آن باقی مانده، [مسیر بازیابی](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/VibePaymentController.cs:143) و دکمهٔ آن در خلاصه سفارش اضافه شد. تا وقتی Reservation باز است، [ساخت Payment عادی دوم](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:578) مسدود می‌شود.

**مرز تکمیل:** اگر پاسخ درخواست اولیهٔ `CreateOrder` در Vibe گم شود، Source فعلی راهی برای بازیابی URL با `ReservationNumber` نشان نمی‌دهد. کد درخواست را کورکورانه تکرار نمی‌کند؛ تلاش در وضعیت نیازمند بررسی می‌ماند. نتیجهٔ نامعلوم Verify نیز همین رفتار را دارد. همچنین Refund ترکیبی Vibe هنوز به جریان gateway-first و Ledger کیف پول متصل نشده است. بنابراین مسیر پرداخت موفق در کد پیاده‌سازی شده، اما تکمیل عملیاتی و Refund را تأییدشده اعلام نمی‌کنم.

`git diff --check` برای فایل‌های tracked مرتبط موفق بود. Build با `NETSDK1004` به‌دلیل نبود `project.assets.json` متوقف شد؛ Restore قبلی نیز بسته‌های خصوصی `Yashil.Common.*` نسخهٔ `9.8.45` را پیدا نکرد. تست و تأیید Runtime انجام نشده است.

---

## user
در رابطه با حکمت اصلاح کدی که گفتی درست است همان کار را انجام بده و در رابطه با Refund هم کارو تکمیل کن برای vibe و hekmatOnline و درگاه های شتابی دیگر و اگر مشکلی بود بگو با جزئیات کامل و مثال و در غیر این صورت تا اتمام کار ادامه بده نقش یک معمار و برنامه نویس ارشد رو داری


---

## assistant
اصلاح HekmatOnline را روی همان Payment آماده‌شده پیش می‌برم و سپس مسیر Refund را برای سهم درگاه و Wallet در Vibe، HekmatOnline و درگاه‌های شتابی دنبال می‌کنم. ابتدا قراردادهای واقعی Refund و وضعیت‌های نهایی را در کد مرتبط می‌خوانم تا تغییر مالی تکراری یا بازپرداخت مضاعف ایجاد نشود.

---

## assistant
## Handoff: Mixed Wallet Payment

### درخواست فعال

کاربر خواسته اصلاح HekmatOnline مطابق طرح قبلی انجام شود و Refund ترکیبی برای Vibe، HekmatOnline و درگاه‌های شتابی تکمیل شود. اگر مانع واقعی وجود دارد، با مثال و جزئیات گزارش شود؛ کار مستقل متوقف نشود. توضیح قبلی کاربر قطعی است: در Vibe، جزئیات کالا مستقل از `cart_amount` هستند و این اختلاف به‌تنهایی مانع Mixed نیست.

### محدودیت‌ها و ترجیحات

- هیچ Entity، DbContext، Mapping، Schema، Migration، Index یا Database Object تغییر نکند.
- رفتار پرداخت عادی حفظ شود؛ پرداخت کامل Wallet و Hybrid در محدوده Mixed نیستند.
- `T=W+G`، Payment برابر `G`، Reservation برابر `W`؛ Order فقط پس از `Paid`، Capture موفق و بدهی صفر `Complete` شود.
- نتیجه نامعلوم درگاه یا Capture نباید موجب Release شود. Refund باید gateway-first باشد و مبلغ دوبار بازپرداخت یا از بدهی کسر نشود.
- کاربر اجرای پیوسته و گزارش نهایی واحد می‌خواهد؛ برای تصمیم‌های متعارف سؤال نپرس.
- Skillهای `wallet-feature` و `backend-feature` و مستند `docs/codex/wallet.md` در نوبت قبلی خوانده شده‌اند. Build به‌دلیل نبود assets و Restore به‌دلیل نبود بسته خصوصی `Yashil.Common.* 9.8.45` قابل تأیید نبوده است.
- Working tree از قبل تغییرات و فایل‌های untracked فراوان دارد؛ آن‌ها را بازنویسی نکن. برای Git از `git -c safe.directory=P:/Projects/walletProject/Wallet` استفاده کن.

### وضعیت کد از نوبت قبل

Vibe Mixed در کد به Checkout، انتخاب Program در `Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml`، `WalletOrderPreparationService`، Payment موجود در `VibePaymentController.Index(oId,paymentId)` و شاخه Mixed در `VibeService.VerifyMixedAsync` وصل شده است. `WalletSettlementService` Capture را پس از مهلت Reservation برای Payment `Paid` می‌پذیرد؛ Release فقط وضعیت‌های نهایی را می‌پذیرد. `WalletProgramService` انقضای Assignment با Reservation و Payment باز/موفق را به تعویق می‌اندازد. `OrderService.CreateNewPaymentAndReturnAsync` هنگام Reservation باز Payment دوم نمی‌سازد. Vibe برای Callback چند سرور از SQL `sp_getapplock` با تراکنش مستقل استفاده می‌کند؛ نتیجه نامعلوم Verify را باز نگه می‌دارد؛ Capture موفق و بدهی صفر را پیش از Complete بررسی می‌کند. مسیر `RecoverMixed` برای Payment `Paid` و Capture ناقص اضافه شده است.

**این مسیر Build یا Runtime تست نشده است.** پاسخ گم‌شدهٔ `CreateOrder` در Vibe یا Verify نامعلوم، بدون قرارداد بازیابی ارائه‌دهنده، عمداً درخواست خارجی دوم نمی‌فرستد و به بررسی عملیاتی نیاز دارد. Refund مختلط هنوز به جریان اصلاح سفارش وصل نشده است. ادعای تکمیل عملیاتی یا Refund نکن.

### یافته‌های HekmatOnline

- `HekmatOnlinePaymentGatewayController.Index` فقط `orderId` می‌گیرد و `GenerateTokenAsync(orderId)` را صدا می‌زند.
- `HekmatOnlinePaymentGatewayService.GenerateTokenAsync` در حدود خط ۴۲۱ با `CreateNewPaymentAndReturnAsync` Payment تازه می‌سازد؛ باید برای Mixed از `paymentId` آماده‌شده استفاده کند و `G` و همان `ReservationNumber` را به Token بدهد.
- بعد از `ConfirmPayAsync` موفق، در حدود خط ۶۴۱ Order پیش از Payment `Paid` و Wallet Capture به `Complete` می‌رود. catch بعد از Confirm ممکن است Payment را `Unsuccessful` کند، با وجود موفقیت خارجی.
- مسیر `VerifyPaymentTransactionByTokenAsync` برای `PayedConfirm` نیز در حدود خط ۷۵۹ Order را مستقیم Complete می‌کند.
- `ValidateCallbackParameters` در حدود خط ۲۸۶ بررسی امضا را کامنت کرده و مبلغ را فقط مثبت چک می‌کند؛ باید برای Mixed مبلغ و Payment تطبیق یابند.
- قفل Callback فعلی `PaymentTransactionUtil` حافظه‌ای است.
- سؤال خارج از کد همچنان باقی است: پذیرش `G<T` و رفتار ساخت Token تکراری با همان ReservationNumber نزد HekmatOnline اثبات نشده؛ بااین‌حال اصلاحات مستقل کد را انجام بده.

### یافته‌های Refund از اولین بررسی نوبت جاری

در `ModifiedOrderItemService.AddRefundAsync` حدود خط ۵۰۹:

- لیست Paymentهای `Paid` گرفته می‌شود؛ Refundهای قبلی فقط با `RefundStatusTypeEnum.Done` لحاظ می‌شوند.
- حدود خطوط ۵۵۷ تا ۵۷۳، **ابتدا** `AddWalletRefundDetail` اجرا می‌شود. این تابع حدود خط ۵۹۷ مربوط به `PaymentMethodTypesEnum.Wallet` قدیمی ناجا و اقساط است؛ هیچ ربطی به `WalletTransaction`/WalletProgram Capture ندارد.
- سپس فقط برای Ansar، Saman، Samin و Hekmat `AddRefundDetail` فراخوانی می‌شود. HekmatOnline، Vibe و بعضی روش‌های شتابی پوشش ندارند. این ترتیب gateway-first مورد تأیید را برای WalletProgram اجرا نمی‌کند.
- `WalletRefundService.RefundAsync` موجود، Ledger credit مستقل با سقف Capture و کلید تکرار دارد؛ `WalletRefundRequestDto` شامل `CaptureTransactionId`، `Amount`، `Code` است.
- `PaymentService.GetOrderTotalPayment` در حدود خط ۲۶۱ Refund را برای روش‌های شتابی و HekmatOnline فقط در `SendForModifyPrice` یا `Done` نهایی حساب می‌کند؛ Vibe در فهرست آن نیست. تصمیم کسب‌وکار قبلی این دو وضعیت را برای «شتابی و حکمت آنلاین» قطعی اعلام کرده، برای Vibe هنوز قرارداد نهایی Refund روشن نیست. `Done` طبق گزارش‌های قدیمی لزوماً اثبات انتقال واقعی پول نیست؛ تصمیم کسب‌وکار فقط برای موارد نام‌برده ملاک است.

### گام‌های بعد

1. **ابتدا کار HekmatOnline را پیاده کن:** مسیر `Index` با Payment آماده‌شده و اعتبارسنجی مالک/Program/Reservation، Token برای `G` بدون Payment دوم، Callback و Inquiry مخصوص Mixed با Paid → Capture تکرارپذیر → Debt=0 → Complete و Release فقط شکست قطعی. مسیر عادی را حفظ کن. قرارداد خارجی نامعلوم را به‌عنوان محدودیت دقیق نگه دار.
2. **Refund را هدفمند بررسی و سپس پیاده کن:** `ModifiedOrderItemService.ApplyModificationAsync`، `AddRefundAsync`، `AddRefundDetail`، وضعیت‌های `RefundDetail` و callerهای گروه‌بندی/تأیید، `WalletRefundService`، لغو سفارش و روش‌های پرداخت واقعی را بخوان. از Refund ثبت‌شده/نامعلوم وجه واقعی فرض نکن. تخصیص gateway-first و کلید idempotency برای Wallet ledger را به Capture درست و اصلاح Order پیوند بده؛ اگر قرارداد نهایی Vibe یا ثبت مبلغ Provider واقعاً فاقد منبع قابل‌اعتماد است، آن قسمت را ایمن مسدود و بقیه را تکمیل کن.
3. تغییرات اخیر Vibe را هدفمند برای ایراد کامپایل، Race، Authorization و مسیر Retry بررسی کن؛ `git diff --check` فقط tracked را پوشش می‌دهد چون ماژول YashilPayment و بسیاری سرویس‌های Wallet untracked هستند.
4. Build/Test واقعی را تلاش کن و فقط نتیجه واقعی را گزارش بده؛ Build پیشین `NETSDK1004` و Restore پیشین `NU1102` بود.

### ابزار

`functions.exec` با `tools.exec_command`، `tools.apply_patch` و در صورت نیاز `tools.write_stdin` در دسترس است. جست‌وجو را محدود به فایل‌های مرتبط نگه دار.

---

## assistant
قراردادهای فعلی HekmatOnline و Refund را در کد بررسی می‌کنم و اصلاحات قابل‌اجرا را با حفظ مسیر پرداخت عادی پیش می‌برم. هر وابستگی واقعی به تأیید درگاه یا فرایند بازپرداخت را در گزارش پایانی جدا می‌کنم.

---

## assistant
در این کار از راهنمای Wallet و Backend مخزن استفاده می‌کنم. ابتدا نقاط ساخت Payment، تأیید HekmatOnline و ثبت Refund را روی کد فعلی تطبیق می‌دهم.

---

## assistant
در مسیر HekmatOnline یک خطر مشخص تأیید شد: متد شروع درگاه Payment تازه می‌سازد و مسیر Callback می‌تواند Order را پیش از Capture کامل کند. ابتدا همین دو مسیر و بازیابی نتیجه نامعلوم را اصلاح می‌کنم؛ برای Refund نیز ثبت درخواست را از تأیید واقعی بازگشت وجه جدا نگه می‌دارم.

---

## user
الان در چه مرحله ای هستیم کاری که گفتم تمام شد؟


---

## assistant
نه، هنوز تمام نشده است. اتصال HekmatOnline به Payment آماده‌شده، ترتیب Paid → Capture → بررسی بدهی، و مسیر بازیابی Capture را در کد اضافه کرده‌ام. Refund ترکیبی هنوز کامل نیست: جریان اصلاح سفارش سهم Wallet را در مبلغ بازپرداخت حساب نمی‌کند و در کد بررسی‌شده، تأیید بازگشت وجه برای Vibe و HekmatOnline پیدا نشده است. فعلاً اصلاح یا لغو این سفارش‌ها را مسدود کرده‌ام تا بازپرداخت اشتباه رخ ندهد؛ بررسی نهایی تغییرات را ادامه می‌دهم.

---

## assistant
خیر، **کار کامل نشده است**. اصلاح مسیر Mixed در HekmatOnline را در کد پیش برده‌ام، اما Refund ترکیبی برای Vibe، HekmatOnline و سایر درگاه‌های شتابی هنوز به‌صورت ایمن قابل تکمیل و تأیید نیست.

### آنچه انجام شد

- مسیر HekmatOnline اکنون از [Payment آماده‌شده](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:479) با مبلغ `G` استفاده می‌کند و Payment دوم نمی‌سازد. ساخت Token برای هر تلاش قفل می‌شود؛ اگر نتیجهٔ ساخت Token نامعلوم بماند، درخواست خارجی خودکار تکرار نمی‌شود.
- در مسیر Mixed، [Callback و بازیابی](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:798) فقط پس از تأیید پرداخت، Capture را اجرا می‌کنند. Order پس از Capture موفق و بدهی صفر Complete می‌شود. برای Capture ناموفق، [پیگیری از صفحه سفارش](/P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummary.cshtml:65) اضافه شد.
- انتخاب HekmatOnline با Wallet به [Checkout](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:649) و [صفحه انتخاب پرداخت](/P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:209) وصل شد. توضیح شما درباره استقلال اقلام Vibe از `cart_amount` مبنای کار است؛ آن را مانع تلقی نکردم.
- تا روشن‌شدن Refund، [اصلاح سفارش دارای Reservation باز یا Capture](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:124) و لغو آن از مسیر Checkout مسدود شد تا محاسبه فعلی بازپرداخت، نتیجه مالی اشتباه ثبت نکند.

### مانع دقیق Refund

[محاسبه فعلی اصلاح سفارش](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:468) فقط Paymentهای موفق را در مبلغ پرداخت‌شده می‌بیند؛ سهم Capture کیف پول را وارد تخصیص Refund نمی‌کند. مثلاً اگر سفارش `۱۰۰۰`، سهم Wallet برابر `۳۰۰` و درگاه برابر `۷۰۰` باشد و مبلغ سفارش به `۶۰۰` کاهش یابد، تعهد بازپرداخت `۴۰۰` است؛ مسیر فعلی فقط `۱۰۰` محاسبه می‌کند.

افزون بر این، [AddRefundAsync](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:523) مسیر WalletProgram و ثبت بازپرداخت Vibe، HekmatOnline، صادرات و ایران‌کیش را پوشش نمی‌دهد. متد [گروه‌بندی RefundDetail](/P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/RepositoryImpl/RefundDetailRepository.cs:156) وضعیت `Done` را ثبت می‌کند، ولی در کد بررسی‌شده نتیجه قطعی انتقال وجه توسط این درگاه‌ها را ثبت نمی‌کند. `VoidPay` حکمت آنلاین نیز ابطال تلاش پرداخت است، نه Refund یک Payment موفق. بنابراین وصل‌کردن خودکار WalletRefund به `Done` می‌تواند سهم کیف پول را پیش از بازگشت سهم درگاه برگرداند؛ **Refund خودکار را تکمیل‌شده اعلام نمی‌کنم**.

برای تکمیل این بخش، قرارداد ثبت نتیجه قطعی بازپرداخت هر ارائه‌دهنده و مسیر عملیاتی آن لازم است؛ سپس می‌توان تخصیص gateway-first را به RefundDetail و Wallet Ledger با کلید تکرار پایدار وصل کرد.

### اعتبارسنجی و وضعیت اجرا

`git diff --check` برای فایل‌های tracked تغییرکرده گذشت. Build و تست اجراشده **موفق نبودند**: Restore بسته‌های خصوصی `Yashil.Common.*` نسخه `9.8.45` را پیدا نکرد (`NU1102`؛ نزدیک‌ترین نسخه محلی `9.8.42` است)، بنابراین کامپایل و رفتار Runtime این تغییرات تأیید نشده‌اند. پذیرش مبلغ `G<T` و بازیابی Token پس از پاسخ گم‌شده نیز باید در محیط واقعی HekmatOnline تأیید شود. تا رفع این موارد و تکمیل Refund، این تغییرات را آماده فعال‌سازی عملیاتی نمی‌دانم.

---

## user
موضوع refund رو خودم پیاده سازی می کنم باقی موارد پروژه تکمیل شده؟ ادمین پنل و کلاینت و ... در رابطه با درگاه ایران کیش هم لازم نیست هیچ کاری انجام بدهی همچنین صادرات نیز کاری نکن چون دیگر استفاده نمی شوند حالا گزارش کامل از وضعیت جاری بهم بده


---

## assistant
وضعیت فعلی را از روی کد موجود جمع‌بندی می‌کنم: قابلیت‌های Wallet، پنل ادمین، سمت مشتری و اتصال Vibe/HekmatOnline را جداگانه بررسی می‌کنم. Refund، ایران‌کیش و صادرات را از کار باقی‌مانده کنار می‌گذارم و چیزی را که Build یا Runtime آن تأیید نشده، کامل‌شده اعلام نمی‌کنم.

---

## assistant
## وضعیت و درخواست فعلی

کاربر می‌خواهد **گزارش کامل وضعیت جاری پروژه Wallet** بداند و بپرسد آیا جز Refund بقیه کار، از جمله پنل ادمین و کلاینت، تکمیل شده است. تصمیم جدید: **Refund را خودش پیاده‌سازی می‌کند؛ برای ایران‌کیش و صادرات هیچ کاری لازم نیست.** این نوبت صرفاً بررسی خواندنی انجام شده و هیچ فایلی تغییر نکرده است. هنوز پاسخ نهایی وضعیت ارائه نشده است.

## یافته‌های تأییدشده از کد

- Backend سرویس و API برای `WalletProgram`، `WalletScope`، `WalletAssignment` دستی، `WalletGrant`، Eligibility، Reservation، Capture/Release، Expiration و Audit موجود است. ثبت DI در `Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs:60–71` و Job انقضا در `ModuleInitializerPartial.cs:98–103` دیده شد.
- پنل Angular فقط کامپوننت‌های `wallet-program-list` و `wallet-program-detail` دارد (`../clientApp/ecommerce/wallet-program/`). فرم Program، Scope، Assignment دستی و Grant دارد؛ route `walletPrograms` در `../clientApp/ecommerce/ecommerce-routing.module.ts:223`. **UI اختصاصی برای Audit، مدیریت وضعیت Assignment، Settlement و گزارش عملیاتی پیدا نشد**؛ API Audit وجود دارد. `WalletAssignmentService` فقط `GetAll/GetAsync/GetUsers/CreateAsync` عمومی دارد، نه Suspend/Resume.
- سمت مشتری `MyWalletController` و `Yashil.WebHost/Views/MyWallet/Index.cshtml` برای مانده، `Programs` برای انتخاب اعتبار، لینک‌های Layout و انتخاب Wallet در `Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:172,209–363` وجود دارند. UI Mixed فقط Vibe و HekmatOnline را انتخاب می‌کند.
- Mixed backend در `WalletOrderPreparationService` Payment و Reservation همان تلاش را می‌سازد و `OrderController.SaveShoppingCartToOrder` آن را به Redirect وصل می‌کند. HekmatOnline اکنون متد `GenerateMixedTokenAsync` و `VerifyMixedAsync` دارد؛ Paid → Capture → بدهی صفر → Complete؛ در Paid/Capture ناقص recovery دارد. Vibe نیز مسیر Mixed و recovery دارد. این‌ها **از روی کد مشاهده شده‌اند، نه Build/Runtime تأییدشده**.
- در نوبت قبلی برای Refund ناقص، اصلاح سفارش دارای Reservation باز/Capture در `ModifiedOrderItemService.cs:124–130` و لغو Order از API کاربر در `OrderController.cs:1224–1231` مسدود شده است. با واگذاری Refund به کاربر، این محدودیت‌ها همچنان فعال‌اند؛ باید در گزارش واضح گفته شود.
- مستند مرجع `../سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md:3–7,93–149` MVP را شامل **Card Group/Match و Coupon هشت‌رقمی** نیز می‌داند. در جست‌وجوی نام فایل‌های ماژول Commerce، هیچ `WalletCardService`/`WalletCouponService` یا UI آن پیدا نشد. باید مشخص کرد این‌ها جز Roadmap مورد انتظار هستند یا فاز آینده؛ بدون حدس «کل پروژه کامل است» اعلام نشود.
- مستندات `docs/decisions/HekmatOnline-Mixed-Payment-Go-No-Go-Gates.md` **قدیمی و مربوط به قبل از اصلاحات اخیر** هستند؛ بعضی ادعاهای آن دیگر با کد فعلی مطابقت ندارند. هنوز تأیید عملی پذیرش `G<T`، بازیابی Token گم‌شده، رفتار واقعی Inquiry و اجرای Runtime موجود نیست.
- آخرین Build واقعی در نوبت قبلی: `dotnet build Modules/YashilPayment/YashilPayment.Infrastructure/YashilPayment.Infrastructure.csproj --no-restore` شکست با `NETSDK1004` ابتدا؛ سپس `dotnet restore --ignore-failed-sources` شکست `NU1102` چون `Yashil.Common.* 9.8.45` در مخزن بسته موجود نیست (نزدیک‌ترین `9.8.42`). Build مجدد هم `NU1102`. تست/Runtime/Angular compile انجام نشده‌اند. `git diff --check` برای فایل‌های tracked مربوط به تغییرات اخیر گذشت، با هشدار CRLF.
- Working tree از قبل بسیار کثیف است، از جمله **تمام `Modules/YashilPayment/` و بسیاری فایل‌های Wallet و docs به‌صورت untracked**. تغییرات Entity/DbContext در status مربوط به کار پیشین‌اند؛ این نوبت چیزی تغییر نداده. برای Git از `git -c safe.directory=P:/Projects/walletProject/Wallet` استفاده شود.

## ادامه لازم

1. برای گزارش کامل، مسیر Vibe و HekmatOnline و وضعیت Retry/Redirect را هدفمند بررسی کن؛ به‌خصوص محدودیت Token بعد از پاسخ گم‌شده و این‌که کد فعلی HekmatOnline بدون تأیید قرارداد ارائه‌دهنده فعال شده است.
2. وضعیت تکمیل ادمین و کلاینت را به سه دسته «در کد موجود»، «UI/API موجود نیست»، «تأیید نشده» تفکیک کن؛ به‌خصوص Audit UI، Assignment status، Card/Coupon و Full Wallet را روشن کن.
3. نیازی به تغییر فایل، اجرای Build/Test مجدد یا بررسی Refund/ایران‌کیش/صادرات نیست. اگر نتیجه قبلی Build را گزارش می‌کنی، تصریح کن مربوط به نوبت قبلی است.
4. پاسخ نهایی فارسی، نسبتاً کامل و دقیق، با لینک absolute file path و موارد باز برای انتشار عملیاتی باشد. به سؤال اصلی مستقیم جواب بده: **خیر، صرف کنار گذاشتن Refund به معنی تکمیل و تأیید تمام پروژه نیست.**

---

## assistant
وضعیت فعلی Backend، پنل ادمین و مسیر کاربر را از روی کد جمع‌بندی می‌کنم. Refund را از کارهای من خارج می‌گذارم و ایران‌کیش و صادرات را هم بررسی یا تغییر نمی‌دهم.

---

## assistant
تا اینجا، APIهای اصلی Wallet و رابط مدیریت Program/Scope/Assignment/Grant در کد هستند و گزینه پرداخت ترکیبی در صفحه سفارش هم دیده می‌شود. اما «وجود کد» را از «تأیید Build، اجرای واقعی درگاه و آماده‌بودن کل MVP» جدا می‌کنم؛ چند مورد باز هنوز باید در گزارش مشخص شوند.

---

## assistant
**خیر؛ به‌جز Refund، همه بخش‌های Wallet هنوز «تکمیل و تأییدشده» محسوب نمی‌شوند.** پیاده‌سازی اصلی پرداخت ترکیبی برای Vibe و HekmatOnline و رابط‌های پایه ادمین و مشتری در کد وجود دارد، اما چند قابلیت MVP و اعتبارسنجی نهایی باز است. طبق تصمیم شما، Refund را خودتان انجام می‌دهید و ایران‌کیش و صادرات از کارهای باقی‌مانده من خارج‌اند.

| بخش | وضعیت فعلی |
|---|---|
| هسته Wallet | سرویس‌ها و APIهای Program، Scope، Assignment دستی، Grant، Eligibility، Reservation، Capture/Release، Expiration و Audit موجود و در DI ثبت شده‌اند؛ نمونه در [ModuleInitializer.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs:60). اجرای عملی آن‌ها تأیید نشده است. |
| پنل ادمین | صفحه فهرست و جزئیات Program، مدیریت Scope، ایجاد Assignment دستی و Grant در [wallet-program-detail.component.html](P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.html:49) و مسیر [ecommerce-routing.module.ts](P:/Projects/walletProject/Wallet/clientApp/ecommerce/ecommerce-routing.module.ts:223) وجود دارد. UI اختصاصی Audit و تغییر وضعیت Assignment پیدا نشد؛ API Audit موجود است. بنابراین پنل ادمین کاملِ عملیاتی نیست. |
| سمت مشتری | صفحه مانده [MyWallet/Index.cshtml](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/MyWallet/Index.cshtml:1)، فهرست Programهای قابل انتخاب و گزینه کسر Wallet در [صفحه پرداخت سفارش](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml:172) وجود دارد. پیش‌نمایش قطعی سهم‌های `W/G` پیش از ارسال در این UI دیده نشد. |
| پرداخت ترکیبی | [PrepareAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:38) با کلید تلاش، Payment به مبلغ باقی‌مانده و Reservation متصل به همان Payment را در تراکنش مشترک می‌سازد. [SaveShoppingCartToOrder](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:638) فقط برای Vibe و HekmatOnline به این مسیر می‌رود و مسیر عادی را جدا نگه می‌دارد. |
| Verify و تسویه | شاخه‌های Mixed برای [Vibe](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:416) و [HekmatOnline](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:798) نوشته شده‌اند؛ Capture قابل تکرار و تکمیل Order مشروط به بدهی صفر در کد دیده می‌شود. صحت رفتار درگاه واقعی و سناریوهای قطع ارتباط هنوز تأیید نشده است. |
| Refund و اصلاح سفارش | به شما واگذار شد. تا تکمیل آن، اصلاح سفارش دارای Reservation باز یا Capture در [ModifiedOrderItemService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/ModifiedOrderItemService.cs:124) و لغو چنین سفارشی در [OrderController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs:1224) عمداً مسدود است. |

**موارد باز مهم:**

1. سند مرجع MVP، Card Group/Match و Coupon هشت‌رقمی را هم شامل می‌شود، اما برای آن‌ها در ماژول Commerce سرویس و رابط اجرایی پیدا نشد؛ فعلاً فقط Entity و Mapping دیده می‌شود. بنابراین اگر این دو جزء همچنان در محدوده تحویل MVP باشند، پیاده‌سازی‌شان باقی است. [سند مرجع](P:/Projects/walletProject/Wallet/سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md:3)
2. محاسبه بدهی، Gateway Paid و خالص Wallet Capture/Refund را در [OrderService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/OrderService.cs:441) لحاظ می‌کند؛ ولی [PaymentService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/PaymentService.cs:261) هنوز برخی وضعیت‌های `RefundDetail` از جمله `Done` را در مبلغ خالص Payment دخیل می‌کند. از بررسی‌های قبلی، `Done` به‌تنهایی اثبات بازگشت واقعی وجه نیست. این بخش باید در کار Refund شما تعیین تکلیف شود تا بدهی و بازپرداخت دوباره محاسبه نشود.
3. در ساخت Token حکمت، تلاش پیش از تماس خارجی به وضعیت انتظار می‌رود؛ اگر پاسخ Token گم شود، همان تلاش به‌سادگی Token را دوباره درخواست نمی‌کند. در Vibe نیز درخواست CreateOrder برای هر تلاش یک‌بار Claim می‌شود. این رفتار جلوی ارسال تکراری را می‌گیرد، ولی **بازیابی عملیاتی نتیجه نامعلوم** هنوز باید با درگاه و سناریوی واقعی آزموده شود. [HekmatOnlinePaymentGatewayService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs:506)، [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs:76)
4. Full Wallet Payment در محدوده پرداخت ترکیبی فعلی پیاده‌سازی نشده است؛ [PrepareAsync](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletOrderPreparationService.cs:112) عمداً `W=T` را رد می‌کند.

**اعتبارسنجی و وضعیت تحویل:** در همین بررسی، Build پروژه `YashilPayment.Infrastructure` با `--no-restore` اجرا شد و با `NU1102` شکست خورد: بسته‌های `Yashil.Common.*` نسخه `9.8.45` در منابع در دسترس نبودند و مخزن محلی فقط تا `9.8.42` داشت. پس موفقیت Build، تست، Angular compile یا اجرای واقعی Vibe/HekmatOnline قابل ادعا نیست. تغییرات Wallet و کل `Modules/YashilPayment/` نیز در working tree فعلی هنوز تغییرکرده یا untracked هستند؛ این نوبت هیچ فایلی را تغییر ندادم.

**جمع‌بندی:** مسیر اصلی Wallet و Mixed Payment در Source پیش رفته است، ولی پروژه هنوز آماده اعلام «تمام‌شده» نیست. پس از کار Refund شما، تعیین محدوده Card/Coupon، تکمیل بخش‌های لازم ادمین، تأمین وابستگی‌های Build و آزمون یکپارچه و واقعی دو درگاه برای بستن کار لازم است.

---

## user
لیست تسک های باقی مانده را به ریز بهم بده چه کاری های مخوای انجام بدی تا پروژه تمام شود کار refund و build و آزمون رو نمیخوام انجام بدی و پرداخت ترکیبی اگر منظورت hybrid هست انجام نده و اگر منظورت ترکیب درگاه پرداخت و کیف پول است مشکلی ندراد


---

## assistant
منظورم از **پرداخت ترکیبی، پرداخت یک سفارش با سهم Wallet و باقی‌مانده درگاه** است (`T = W + G`). ماژول **Hybrid** را تغییر نمی‌دهم.

با محدوده‌ای که مشخص کردی، فهرست کارهای باقی‌مانده من این است:

| ترتیب | تسک | خروجی مورد انتظار |
|---|---|---|
| ۱ | **تکمیل مسیر تلاش پرداخت ترکیبی** | در Retry همان تلاش، همان Payment، Reservation و مبلغ‌های `W/G` بازیابی شوند؛ شروع تلاش جدید کلید مستقل داشته باشد. مسیر پرداخت عادی همچنان مستقل بماند. |
| ۲ | **تعیین تکلیف خطای ساخت درخواست درگاه** | برای Vibe و HekmatOnline، حالت‌هایی مثل دریافت‌نشدن پاسخ ساخت Token یا CreateOrder، ثبت‌نشدن Token و پاسخ نامعلوم را در کد مدیریت کنم؛ تلاش نامعلوم نباید پرداخت دوم بسازد یا Reservation را زود آزاد کند. هر موردی که بدون قابلیت Inquiry درگاه قابل بازیابی قطعی نباشد، صریحاً به‌عنوان نیاز عملیاتی ثبت شود. |
| ۳ | **تکمیل اتصال نتیجه پرداخت به Wallet** | در دو درگاه فوق، نتیجه قطعی موفق به Capture تکرارپذیر برسد؛ شکست قطعی به Release برسد؛ نتیجه نامعلوم محفوظ بماند. Order فقط با Payment موفق، Capture موفق و بدهی صفر Complete شود. بخش عمده این مسیر موجود است و کار باقی‌مانده، رفع فاصله‌های مشخص در بازیابی و یکپارچگی آن است. |
| ۴ | **تکمیل صفحه پرداخت مشتری** | انتخاب Program، نمایش سهم واقعی Wallet و مبلغ باقی‌مانده درگاه، پیام خطا و وضعیت تلاش نامعلوم را به هم وصل کنم. مبلغ نهایی را سرور تعیین کند. کلید تلاش در Retry حفظ و برای تلاش جدید عوض شود. |
| ۵ | **تکمیل بخش‌های عملیاتی ادمین** | برای Audit و مغایرت مانده، وضعیت Reservation و تسویه، و وضعیت Assignment نمای قابل استفاده اضافه کنم. عملیات تغییر وضعیت یا تسویه دستی را فقط در حد قرارداد Business و مجوزهای موجود پیاده کنم؛ اختیار مالی تازه‌ای از روی UI اختراع نمی‌کنم. |
| ۶ | **Card Group / Card Match** | چون در سند مرجع Wallet MVP آمده ولی سرویس و UI اجرایی آن دیده نشده، با Entityهای موجود جریان مدیریت گروه/عضو، تطبیق امن کارت و اتصال به Assignment را تکمیل کنم؛ PAN خام ذخیره نشود. |
| ۷ | **Coupon هشت‌رقمی** | ایجاد Batch، تولید و Claim امن کد، جلوگیری از Claim تکراری، اتصال به Assignment و رابط مدیریت/استفاده را با مدل موجود تکمیل کنم. اگر فیلد یا قرارداد ضروری در مدل فعلی نباشد، فقط همان وابستگی را گزارش می‌کنم. |
| ۸ | **جمع‌بندی اتصال‌ها و وضعیت تحویل کد** | مسیرهای API، DI، Route، Authorization و تغییرات نهایی را به‌صورت ایستا بررسی کنم و فهرست قابلیت‌های انجام‌شده و موارد باز را تحویل بدهم. این مرحله شامل Build یا آزمون نیست. |

**از محدوده کار من خارج است:** Refund و اصلاح/لغو سفارش وابسته به آن که خودت انجام می‌دهی؛ Build و هر نوع آزمون؛ ماژول Hybrid؛ ایران‌کیش و صادرات؛ Full Wallet Payment که قبلاً خارج از محدوده تعیین شده بود؛ و هر تغییر Entity، Mapping یا Schema.

یک وابستگی مهم باقی می‌ماند: تا وقتی Refund را تکمیل نکرده‌ای، محدودیت فعلی اصلاح و لغو سفارش‌های دارای Wallet Capture باید برقرار بماند. همچنین پس از انجام کارهای بالا، من می‌توانم **تکمیل کد در محدوده توافق‌شده** را گزارش کنم، اما بدون Build، آزمون و بررسی واقعی درگاه‌ها، عنوان «آماده انتشار و تأییدشده در اجرا» دقیق نخواهد بود.

---

## user
تسک اول مربوط به retry رو نمیخوام هیچ کاری انجام نده خودم انجام می دهم
تسک دوم مربوط به تلاش دوم رو هم نمیخوام کاری انجام بدهی و خودم انجام می دهم
با کم ترین توکن مصرفی کارهای باقی مانده رو انجام بده و به اتمام برسان اگر موافقی حالت medium مربوط به effort رو یک لول پایین تر بیاورم برای مصرف کمتر توکن تا اتمام کار و چرا انقدر مصرف توکن بالاست ؟



---

## assistant
موافقم؛ اگر در برنامه effort را از medium به low ببری، مصرف توکن کمتر می‌شود، هرچند برای تصمیم‌های مالی ممکن است دقت بررسی پایین‌تر بیاید. مصرف بالای این گفتگو بیشتر از تاریخچه بسیار طولانی Wallet و بررسی چندلایه کد و درگاه‌هاست. از اینجا روی کارهای باقی‌مانده متمرکز می‌شوم و به Retry و تلاش دوم، Refund، Build و آزمون دست نمی‌زنم. برای اجرا، دستورالعمل مهارت‌های Wallet، Backend و Angular مخزن را می‌خوانم.

---

## assistant
دو بخش Card/Coupon فعلاً قرارداد لازم برای پیاده‌سازی امن ندارند: مقدار وضعیت‌هایشان در کد مشخص نیست و برای تطبیق کارت، کلید HMAC معتبر هم پیدا نشد. روی این‌ها مقدار یا کلید حدسی نمی‌گذارم. بخش مستقلِ قابل انجام را ادامه می‌دهم: نمایش Audit موجود در پنل ادمین، بدون تغییر منطق مالی یا مسیر Retry.

---

## assistant
تسک‌های **Retry** و **تلاش دوم** را کنار گذاشتم و هیچ تغییری در آن‌ها ندادم. منظور از Mixed هم فقط ترکیب **Wallet + درگاه** است؛ به Hybrid، Refund، ایران‌کیش و صادرات دست نزدم.

کارهای مستقلِ قابل انجام در این نوبت:

- نمایش گزارش مغایرت API موجود را به [جزئیات Program در پنل ادمین](P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.ts:107) اضافه کردم.
- مبلغ واقعی Wallet رزروشده و سهم درگاهِ همان تلاش را در [خلاصه سفارش](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummary.cshtml:52) و فهرست سفارش‌ها نمایش دادم. داده از [CustomerOrderFundingService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/CustomerOrderFundingService.cs:53) خوانده می‌شود و رزرو را پرداخت قطعی حساب نمی‌کند.
- اتصال Paid → Capture → بدهی صفر → Complete برای Vibe و HekmatOnline از قبل در کد موجود است؛ در این نوبت آن را تغییر ندادم.

**هنوز نمی‌توانم پایان کل پروژه را اعلام کنم.** Card Match و Coupon که در سند MVP آمده‌اند، برای پیاده‌سازی امن به مقدار قطعی وضعیت‌های `CommonBaseData`، کلید HMAC با پیکربندی امن، و تعیین قرارداد ردیابی منبع Assignment در مدل بدون `SourceId/SourceRef` نیاز دارند. طبق دستورالعمل `wallet-feature` مخزن، Claim و تطبیق کارت باید در برابر تکرار و هم‌زمانی امن باشند؛ این مقادیر و قراردادها را حدس نزدم. نمایش `W/G` اکنون برای تلاش ساخته‌شده در خلاصه سفارش است؛ پیش‌نمایش قطعی **قبل از ایجاد تلاش** هنوز وجود ندارد.

طبق درخواستت **Build و آزمون اجرا نکردم**؛ فقط بررسی ایستای فایل‌های تغییرکرده انجام شد. پایین‌آوردن effort به `low` برای ادامه ممکن است. مصرف بالای توکن عمدتاً از تاریخچه طولانی این گفتگو و بررسی قراردادهای مالی چندلایه بوده است؛ با محدوده‌ای که اکنون تعیین کردی، بررسی‌های بعدی کوتاه‌تر می‌مانند.

---

## user
الان فقط پیاده سازی کوپن باقی مانده است؟


---

## assistant
خیر. جدا از کارهایی که خودت بر عهده گرفتی، **کوپن و Card Match** هنوز پیاده‌سازی نشده‌اند. همچنین پیش‌نمایش قطعی سهم Wallet و درگاه **پیش از ایجاد تلاش پرداخت** باقی است؛ نمایش مبلغ‌های تلاشِ ساخته‌شده در خلاصه سفارش انجام شده است.

برای کوپن و Card Match، مقدار قطعی وضعیت‌ها و قرارداد کلید HMAC باید مشخص شود. اگر Card Match و پیش‌نمایش را از محدوده تحویل خارج می‌دانی، آن‌وقت کوپن تنها قابلیت اجرایی باقی‌مانده در محدوده من خواهد بود. وضعیت Build و آزمون را نیز طبق درخواستت بررسی نکرده‌ام.

---

## user
هر سوال و کسری برای تکمیل 2 مورد باقی مانده که گفتی داری بپرس پاسخ بدم و بعد از پاسخ پیاده سازی رو تکمیل کنیم


---

## assistant
برای تکمیل **کوپن و Card Match**، این تصمیم‌ها لازم است. لطفاً مقدار یا قاعده را بده؛ **خودِ کلیدهای محرمانه را در گفتگو نفرست**.

1. **شناسه وضعیت‌ها:** IDهای قطعی `CommonBaseData` را برای CardGroup (`Draft/Active/Suspended/Closed`)، CardMember (`Active/Blocked/Closed`)، CouponBatch (`Draft/Active/Closed/Cancelled`) و Coupon (`Available/Claimed/Expired/Cancelled`) بفرست.
2. **Card Match:** شماره کارت از کدام فیلد و در چه رویدادی خوانده شود؟ آیا `Recipient.CardNumber` همان شماره کارت موردنظر است؟ پس از Match، فقط Assignment با مبلغ صفر ساخته شود یا اعتبار هم اعطا شود؟ اگر اعتبار اعطا می‌شود، مبلغ از کجا می‌آید؟
3. **Coupon Claim:** آیا Claim موفق باید همان لحظه `AmountPerCoupon` را از طریق Grant به Assignment فعال کاربر اضافه کند؟ اگر کاربر از قبل Assignment فعال همان Program را دارد، همان Assignment استفاده شود؟
4. **کلید HMAC و تحویل کد:** آیا مجازم دو کلید جداگانه برای کارت و کوپن از تنظیمات امن محیط بگیرم؟ کدهای خام هشت‌رقمی فقط یک‌بار هنگام ساخت Batch به مدیر تحویل داده شوند و بعداً صرفاً Hash و نسخه ماسک‌شده نگهداری شوند؟

بعد از پاسخ، پیاده‌سازی همین دو مورد را ادامه می‌دهم. Retry و تلاش دوم پرداخت، Refund، Build و آزمون همچنان با شماست.

---

## user
با کم ترین مصرف توکن کار هارو تا زمان تکمیل ادامه بده مگر اینکه مورد باشد که نیاز به تصمیم گیری داشته باشد
enum هارو نیز اضافه کردم
Recipient.CardNumber` همان شماره کارت موردنظر است  ?  بلی
 پس از Match، فقط Assignment با مبلغ صفر ساخته شود یا اعتبار هم اعطا شود؟ بلی
اگر اعتبار اعطا می‌شود، مبلغ از کجا می‌آید؟ اختصاص اعتبار از WalletProgram  باید بخونه با توجه به WalletProgramType اگر از نوع کد تخفیف  باشه و اگر WalletProgramAllocationMethodType از نوع ثابت برای کاربر باشه برای همه کاربران AllocatedAmount رو میده ولی از TotalAmount  بیشتر ر نباید باشه بررسی کن و اینکه اگه WalletProgramAllocationMethodType از نوع دستی باشه خود ادمین مقادیر رو میزنه و باز هم از TotalAmount  بیشتر نباید باشه بررسی کن
جواب سوال 3
**Q3 — Coupon Claim**

بله. پس از اعتبارسنجی موفق کوپن و ثبت موفق Claim، باید به میزان `AmountPerCoupon` یک Grant برای کاربر ایجاد شود و اعتبار همان لحظه قابل استفاده باشد.

اگر کاربر از قبل Assignment فعال برای همان Program دارد، از همان Assignment استفاده شود و Assignment تکراری ساخته نشود.

Claim و Grant باید در برابر درخواست تکراری، Retry و اجرای هم‌زمان idempotent باشند؛ هر Coupon فقط یک بار مجاز به ایجاد Grant است. اگر ثبت Claim موفق شود ولی Grant شکست بخورد، سیستم نباید وضعیت موفقیت ناقص ایجاد کند و باید امکان بازیابی امن وجود داشته باشد.

لطفاً این رفتار را با ساختار فعلی Entityها، Transactionها و محدودیت‌های دیتابیس تطبیق بده. اگر تضمین آن بدون تغییر Schema ممکن نیست، پیش از پیاده‌سازی گزارش بده.
جواب سوال  4 :**Q4 — HMAC و تحویل امن کدها**

بله. برای کارت و کوپن، دو کلید HMAC مستقل از تنظیمات امن محیط دریافت شود؛ کلیدها نباید در Source Code یا تنظیمات عمومی ذخیره شوند.

کدهای خام هشت‌رقمی هنگام ساخت Batch فقط یک بار به مدیر مجاز نمایش داده شوند. پس از آن، فقط HMAC کد و نسخه ماسک‌شده در دیتابیس نگهداری شود و کد خام از دیتابیس قابل بازیابی نباشد.

الزامات:
- تولید کدها باید از نظر تصادفی‌بودن، یکتایی و جلوگیری از برخورد بررسی شود.
- HMAC با الگوریتم و مقایسه امن و سازگار با قرارداد پروژه پیاده‌سازی شود.
- کلیدها از Secret Manager یا تنظیمات امن محیط دریافت شوند و در Logها یا پاسخ‌های غیرمجاز افشا نشوند.
- دسترسی به نمایش اولیه کدها فقط برای مدیر مجاز باشد.
- Retry درخواست ساخت Batch نباید Batch یا مجموعه کدهای تکراری ایجاد کند.
- اگر پاسخ اولیه حاوی کدها از دست رفت، کد خام نباید از دیتابیس بازیابی شود؛ سیاست لغو و ساخت مجدد Batch باید مشخص باشد.
- پیش از تغییر، سازگاری با ساختار فعلی کارت، کوپن و مدیریت کلیدها بررسی شود. اگر تغییر Schema یا تصمیم امنیتی اضافی لازم است، پیش از پیاده‌سازی گزارش بده.


---

## assistant
ابتدا Enumهای تازه و قیدهای موجود Batch/Coupon را بررسی می‌کنم. طبق شرطی که گذاشتی، اگر ساخت Batch با Retry یا Claim همراه Grant بدون تغییر Schema قابل تضمین نباشد، قبل از تغییر مالی همان مانع را گزارش می‌دهم.

---

## assistant
Enumهای Card و Coupon را در کد پیدا کردم؛ این بخش برطرف شده است. **قبل از تغییر مالی، دو تصمیم لازم مانده است:**

1. **مبلغ Card Match:** پاسخ «بله» روشن نمی‌کند Match فقط Assignment صفر می‌سازد یا Grant هم می‌دهد. همچنین [WalletProgram.AllocatedAmount](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletProgram.cs:68) مبلغ ثابت هر کاربر نیست؛ جمع اعتبار اعطاشده است و در [WalletGrantService](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletGrantService.cs:132) با هر Grant افزایش می‌یابد. استفاده از آن به‌عنوان مبلغ هر کاربر اعتبار نادرست ایجاد می‌کند. لطفاً مشخص کن: **Card Match فقط Assignment صفر بسازد، یا Grant کند؟** اگر Grant می‌کند، مبلغ ثابت هر کاربر از کدام فیلد/منبع می‌آید؟ IDهای قطعی روش تخصیص «ثابت» و «دستی» را هم بفرست.
2. **پاسخ گم‌شده ساخت Batch:** کد خام طبق تصمیم تو قابل بازیابی نیست. پیشنهاد امن من: Batch ابتدا `Draft` ساخته شود؛ مدیر کدها را یک‌بار دریافت کند و سپس آن را فعال کند. اگر پاسخ گم شد، همان Batch فعال نشود؛ مدیر آن را `Cancelled` کند و با کلید درخواست جدید Batch تازه بسازد. **این سیاست را تأیید می‌کنی؟**

از نظر کد، Claim و Grant می‌توانند در یک تراکنش مشترک انجام شوند، ولی `WalletGrantService.CreateAsync` اکنون تراکنش مستقل باز می‌کند و برای این کار باید به‌شکل محدود سازگار شود. همچنین [Code در WalletCouponBatch](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27861) قید یکتای دیتابیس ندارد؛ هماهنگی درخواست‌های هم‌زمان را می‌توان با قفل SQL Server در مسیر سرویس انجام داد، اما یکتایی Batch در برابر نوشتن از مسیرهای خارج از آن سرویس، تضمین دیتابیسی ندارد. مطابق شرطی که گذاشتی، تا روشن‌شدن دو تصمیم بالا پیاده‌سازی Card/Coupon را شروع نکردم و در این نوبت فایلی تغییر نکرد.

---

## user
مقدار enum جدید رو اضافه کردم
برای سوال دوم این جواب رو مد نظر بگیر و پاسخ بده با مصرف توکن کم
**Clarification — WalletProgram Amount Allocation**

منطق تجاری موردنظر این است:

- `WalletProgram.TotalAmount` بودجه کل برنامه کیف پول را مشخص می‌کند؛ مثلاً 1000.
- مبلغ تخصیص باید بر اساس تعداد کارت‌ها یا کاربران واجد شرایط، طبق قاعده تخصیص برنامه محاسبه شود.
- برای مثال، با بودجه 1000 و 10 واحد تخصیص، مبلغ هر واحد 100 خواهد بود.
- باید مشخص شود `WalletProgram.AllocatedAmount` در پیاده‌سازی فعلی مجموع Grantهای ثبت‌شده است یا مبلغ تخصیص هر کاربر. این دو مفهوم نباید با یکدیگر اشتباه شوند.

لطفاً پیش از هر تغییر کد، موارد زیر را بررسی و گزارش کن:

1. تعریف واقعی `TotalAmount` و `AllocatedAmount` در Entity و دیتابیس.
2. تمام محل‌هایی که `AllocatedAmount` خوانده یا تغییر داده می‌شود، به‌ویژه `WalletGrantService`.
3. منبع فعلی تعداد کارت‌ها و تعداد کاربران واجد شرایط و اینکه تخصیص بر اساس کدام‌یک انجام می‌شود.
4. منطق Card Match: آیا فقط Assignment ایجاد می‌کند یا Grant هم می‌دهد؟
5. روش تخصیص مبلغ صحیح و تعیین تکلیف باقیمانده تقسیم.
6. اینکه آیا می‌توان منطق موردنظر را با ساختار فعلی پیاده کرد یا به فیلد/تغییر Schema نیاز است.

از روی نام فیلدها فرض نساز و `AllocatedAmount` را بدون تأیید قرارداد واقعی به‌عنوان مبلغ هر کاربر استفاده نکن. فعلاً هیچ کدی یا Schema را تغییر نده؛ ابتدا شواهد کد و پیشنهاد دقیق را ارائه کن.



---

## assistant
قرارداد مبلغ را فقط از Entity، Mapping و مسیر Grant بررسی می‌کنم؛ این نوبت هیچ کد یا Schemaای تغییر نمی‌دهم.

---

## assistant
بررسی خواندنی انجام شد؛ **هیچ کد، Schema یا داده‌ای تغییر نکرد**.

1. در [Entity](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Core/Entities/WalletProgram.cs:66)، `TotalAmount` و `AllocatedAmount` دو مبلغ جدا هستند. [EF Mapping](P:/Projects/walletProject/Wallet/backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27965) هر دو را `decimal(18,2)` با مقدار پیش‌فرض صفر تعریف می‌کند. این شاهدِ Mapping است؛ دیتابیس فیزیکی را در این نوبت بررسی نکردم.
2. **قرارداد واقعی کد:** Program با `AllocatedAmount = 0` ساخته می‌شود؛ هر [Grant](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletGrantService.cs:132) آن را به‌اندازه مبلغ Grant افزایش می‌دهد، مشروط به `AllocatedAmount + Grant ≤ TotalAmount`. [Audit](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletAuditService.cs:267) آن را با مجموع Grantها و Assignmentها مقایسه می‌کند. بنابراین **`AllocatedAmount` مجموع تخصیص‌های ثبت‌شده است، نه سهم هر کاربر**. ProgramService آن را نمایش می‌دهد و اجازه کاهش `TotalAmount` به زیر آن را نمی‌دهد.
3. Enum جدید روش تخصیص را مشخص می‌کند: `FixedForUser = 3166` و `Manual = 3167`. اما Entity فیلدی برای **مبلغ ثابت هر کاربر** یا **تعداد واحدهای مبنای تقسیم** ندارد. CardMember کارت را نگه می‌دارد، ولی به User وصل نیست؛ `Recipient.CardNumber` نیز در Entity Recipient شناسه User ندارد. سرویس فعلی‌ای که تعداد «کاربران واجد شرایط یکتا» را برای Program تعیین کند پیدا نشد.
4. **Card Match در کد فعلی نه Assignment می‌سازد و نه Grant.** `WalletAssignmentService.CreateAsync` فقط Assignment دستی با مبلغ صفر می‌سازد؛ Grant عملیات جداگانه است.
5. پیشنهاد محاسبه، پس از تعیین واحدها: `سهم هر واحد = کفِ دو رقم اعشارِ TotalAmount ÷ تعداد واحدهای قطعی`؛ باقیمانده نزد Program تخصیص‌نیافته بماند. هر Grant همچنان باید کنترل اتمیک بودجه فعلی را طی کند. این **پیشنهاد** است، نه قاعده تأییدشده.

برای پیاده‌سازی بدون حدس، دو تصمیم لازم است: **واحد تقسیم، کارت فعالِ یکتا است یا کاربر واجد شرایطِ یکتا، و تعداد آن چه زمانی ثابت می‌شود؟** همچنین تأیید کن **Card Match در روش ثابت، Grant خودکارِ سهم محاسبه‌شده بدهد و در روش دستی فقط Assignment صفر بسازد**. با مدل فعلی، محاسبه از تعداد کارت‌های مشخص‌شده ممکن است؛ ذخیره یک مبلغ ثابت مستقل برای Program بدون فیلد موجود ممکن نیست، مگر اینکه سهم از یک مجموعه ثابت در زمان تخصیص محاسبه شود. سیاست Batch با پاسخ گم‌شده نیز همچنان تصمیم امنیتی بازِ کوپن است.

---

## user
<environment_context>
  <current_date>2026-10-19</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user
با کم ترین مصرف توکن انجام بده و سوال دیگه ای نداری؟ کار دیگه باید تموم بشه
مدل `WalletProgram` با دو ستون جدید `TargetUserCount` و `AmountPerUser` توسعه داده شده است. پیاده‌سازی باید بر اساس این مدل انجام شود:

1. `TargetUserCount` تعداد هدف کاربران برای تخصیص را نگه می‌دارد.
2. `AmountPerUser` مبلغ ثابت هر تخصیص را نگه می‌دارد.
3. در `EqualSplit`، هر دو مقدار `TargetUserCount` و `AmountPerUser` پیش از شروع تخصیص تعیین و اعتبارسنجی شوند. مقدار سهم از تقسیم `TotalAmount` بر `TargetUserCount` محاسبه و با `AmountPerUser` ذخیره‌شده تطبیق داده شود. در صورت وجود باقیمانده، آن مبلغ تخصیص‌نیافته باقی بماند.
4. در `FixedForUser`، `AmountPerUser` مبلغ هر Grant است. `TargetUserCount` تعداد هدف تخصیص‌ها را نشان می‌دهد، اما تعداد واقعی کاربران واجد شرایط باید از داده‌های معتبر استخراج شود.
5. در `Manual`، تخصیص طبق جریان دستی موجود انجام شود؛ ایجاد Assignment به‌تنهایی به معنی Grant مالی نیست.
6. `AllocatedAmount` مجموع Grantهای موفق است و هنگام ایجاد برنامه صفر می‌ماند. هر Grant باید افزایش اتمیک آن را با شرط `AllocatedAmount + GrantAmount <= TotalAmount` انجام دهد.
7. برای جلوگیری از Grant تکراری، عملیات تخصیص باید idempotent باشد و قید یکتایی مناسب برای یک کاربر در هر برنامه اعمال شود، مگر اینکه مدل کسب‌وکار صراحتاً چند تخصیص برای یک کاربر را مجاز کند.
8. در برنامه‌های مبتنی بر کارت، تعداد کارت مورد انتظار در صورت نیاز با `ExpectedMemberCount` در `WalletCardGroup` ذخیره شود؛ تعداد واقعی از `WalletCardMember` محاسبه شود. تعداد کارت‌ها نباید بدون بررسی یکتایی و احراز ارتباط کارت با کاربر، معادل تعداد کاربران تخصیص‌گیرنده فرض شود.
9. Card Match در `FixedForUser` پس از اعتبارسنجی برنامه، کارت و کاربر می‌تواند Grant خودکار ایجاد کند؛ در `Manual`، Card Match به‌تنهایی Grant ایجاد نمی‌کند.
10. کنترل بودجه، ثبت Grant و به‌روزرسانی `AllocatedAmount` باید در یک عملیات تراکنشی و در برابر درخواست‌های هم‌زمان ایمن باشند.


---

## assistant
مدل و Enumهای جدید را بررسی می‌کنم و سپس Card Match و Coupon را با همان قرارداد پیاده‌سازی می‌کنم. سیاست امن برای پاسخ گم‌شده Batch را Draft تا تأیید دریافت کدها در نظر می‌گیرم؛ اگر مدل فعلی مانع اجرای اتمیک شود، همان مانع را دقیق گزارش می‌دهم.

---

## assistant
`AllocatedAmount` در کد فعلی جمع Grantهای موفق است؛ مقدار هر کاربر نیست. فیلدهای جدید Program را برای روش ثابت به قرارداد API و اعتبارسنجی وصل می‌کنم، بدون تغییر مسیر مالی Grant. مقدار قطعی `EqualSplit` را هنوز در Enum موجود نمی‌بینم و برای آن ID حدس نمی‌زنم.

---

## assistant
بله، **سه تصمیم لازم مانده**؛ بدون آن‌ها Grant خودکار را نمی‌توان امن نهایی کرد:

1. `EqualSplit` در [Enum فعلی](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:303) وجود ندارد؛ فقط `FixedForUser=3166` و `Manual=3167` ثبت شده‌اند. **ID قطعی EqualSplit چیست؟**
2. در Card Match با `Recipient.CardNumber`، اعتبار به **UserId خریدارِ Order** اعطا شود، حتی اگر گیرنده شخص دیگری باشد؟
3. برای Coupon، آیا پیشنهاد زیر را تأیید می‌کنی: در روش ثابت، `AmountPerCoupon` باید با `AmountPerUser` برابر باشد؛ در روش دستی Claim خودکار Grant نکند؛ Batch تا تأیید دریافت کدها `Draft` بماند و در صورت گم‌شدن پاسخ اولیه، مدیر آن را لغو و با کلید جدید دوباره ایجاد کند؟

در این نوبت فیلدهای `TargetUserCount` و `AmountPerUser` را به [API و اعتبارسنجی Program](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletProgramService.cs:43) و فرم ادمین وصل کردم. `AllocatedAmount` همچنان فقط با Grant تغییر می‌کند. **Card/Coupon و EqualSplit هنوز تکمیل نشده‌اند.** طبق درخواست قبلی، Build و آزمون اجرا نکردم.

---

## user
fixedForUser در واقع برابر است با equalSplit
جواب سوال دوم هم بله درسته مشکلی ندارد
جواب سوال سوم هم هر دو سوالی که پرسیدی پاسخ بله است


---

## assistant
با این پاسخ‌ها، `FixedForUser` را همان تقسیم مساوی در نظر می‌گیرم؛ Grant کارت به خریدار سفارش تعلق می‌گیرد و سیاست Draft کوپن را اعمال می‌کنم. ابتدا مسیرهای موجود Assignment، Grant و Checkout را برای اتصال اتمیک بررسی می‌کنم، سپس بخش‌های قابل پیاده‌سازی را انجام می‌دهم.

---

## assistant
مسیرهای Backend برای گروه کارت، تولید Batch کوپن، Claim و Grant اتمیک اضافه شده‌اند. اکنون رابط مدیریت را به همین APIها وصل می‌کنم؛ کد خام کوپن فقط در پاسخ نخست ساخت Batch نمایش داده می‌شود.

---

## assistant
## Handoff — Wallet Card/Coupon implementation in progress

### User decisions and constraints

- `FixedForUser` **is** EqualSplit. `AmountPerUser` must equal `truncate(TotalAmount / TargetUserCount, 2)`; remainder stays unallocated.
- Card Match uses `Recipient.CardNumber`; Grant belongs to the authenticated Order buyer. For `FixedForUser`, Card Match may grant automatically; for `Manual`, it creates zero balance Assignment only.
- Coupon Claim grants `AmountPerCoupon` immediately; reuse active Assignment; Claim and Grant must be atomic and idempotent. For fixed programs, `AmountPerCoupon = AmountPerUser`; manual coupon auto Grant was answered “yes” to the proposal to disallow it.
- Coupon codes: separate HMAC keys from secure environment, raw eight digit codes shown once at Batch creation; Draft until administrator confirms receipt. Lost response: cancel Draft Batch and recreate with new key.
- No Entity, DbContext, Mapping, Schema, Migration, Index or database object changes. No Refund, build, tests, payment Retry/second attempt, Hybrid, IranKish, Saderat. User wants minimum tokens but completion.
- Skills `wallet-feature`, `backend-feature`, `angular-feature` were read and announced. No subagents requested.

### Changes already made this turn

- `WalletProgramDto.cs`, `WalletProgramService.cs`: exposed and saved `TargetUserCount`/`AmountPerUser`; fixed allocation validation, including exact equal split on Active; preserves `AllocatedAmount` as sum of successful Grants. Admin Angular detail form exposes fields.
- `WalletGrantService.cs`: fixed program Grant must equal AmountPerUser and rejects a second Grant for same user/program.
- New `WalletBenefitsDto.cs`, `IWalletBenefitsService.cs`, `WalletBenefitsService.cs`: card group/member management, HMAC card match, coupon Batch issuance/activation/cancel, coupon Claim and atomic Grant, read-only list projections. Uses SQL app locks, serializable transactions, existing unique active Assignment index, existing mapped unique Coupon.CodeHash index. Config names `WalletSecurity:CardHmacKey` and `WalletSecurity:CouponHmacKey`, expected Base64 of at least 32 bytes; no secrets committed.
- New admin `WalletCardController.cs`, `WalletCouponBatchController.cs`; `ModuleInitializer.cs` DI.
- `MyWalletController.cs` and `Yashil.WebHost/Views/MyWallet/Index.cshtml`: customer forms for coupon claim and explicit recipient card match.
- Angular `wallet-program-detail.component.ts/.html`: admin group, member and Batch UI with one time code display and cancellation.
- Prior turn added Audit UI and Order summary open W/G display. Preserve those.
- No build or tests run, per user preference.

### Immediate next steps — essential

1. **Inspect and correct new code carefully** without running build/tests. Especially C# compile correctness, financial concurrency, error handling and Angular template/method compatibility. Run `git diff --check` and focused static inspections.
2. `WalletBenefitsService.cs` is large and newly written. Check navigation/nullable expressions, transaction rollback when methods return before commit, EF translation, and correct use of `IUserPrincipal`, `ApplicationConfiguration`, `WalletAssignment`/`WalletTransaction` required fields.
3. Coupon Batch generation: unique `CodeHash` mapping exists, but physical index in current DB has not been verified; global SQL app lock now serializes Batch generation through service. Batch `Code` lacks unique index; attempt key app lock prevents duplicate through service, not external writes.
4. Coupon Claim should not return success for a Claimed coupon if Ledger does not match same Batch/program and amount; strengthen replay validation. Potential race/exception paths should return safe failure without partial commit.
5. Card Match currently triggered by explicit button in MyWallet, not automatically at Checkout. Assess if user’s requirement calls for checkout integration, while honoring no changes to payment Retry/second attempt.
6. `WalletGrantService` second Grant rejection for fixed programs means a user can claim only one coupon per fixed Program; verify this interpretation against “each Coupon only once” and “one allocation per user.” No new user question unless truly unavoidable.
7. Angular Admin needs static review: batch replay/no raw codes UI, CreateGroup constraints, masking, form field names, and Admin API POST primitive body convention.
8. Mark full completion only when all independent paths are connected. Report remaining limitations honestly; do not run build/tests.

### Relevant evidence

- Enum statuses and `WalletProgramAllocationMethodType.FixedForUser=3166`, `Manual=3167`: `Modules/YashilCommerce/YashilCommerce.Core/Enums/Enums.cs:272-307`.
- Unique active assignment filtered index: `Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs:27411`.
- Coupon hash mapped unique index: same file `:27735`.
- Existing Grant budget atomic update: `WalletGrantService.cs:138` before current edits.
- Recipient ownership convention: `RecipientRepository.cs:100-104` checks `Recipient.CreateBy == IUserPrincipal.Id`.
- Program new fields: `Data/Yashil.Core/Entities/WalletProgram.cs:78-79`.
- Backend root: `P:\Projects\walletProject\Wallet\backEnd`; Angular: `P:\Projects\walletProject\Wallet\clientApp`.

---

## assistant
پاسخ‌ها روشن شد. پیاده‌سازی کارت و کوپن را با این قواعد نهایی می‌کنم و اتصال‌ها و تغییرات همین بخش را بررسی می‌کنم.

---

## assistant
پاسخ‌های جدید اعمال شد: در روش `FixedForUser`، مبلغ هر کاربر همان سهم تقسیم مساوی است و با `AmountPerUser` کنترل می‌شود؛ باقیمانده بودجه تخصیص نمی‌یابد. جریان کارت و کوپن نیز طبق پاسخ‌های شما پیاده‌سازی شده است. در این مرحله، اعتبارسنجی Replay کوپن را سخت‌گیرانه‌تر کردم تا فقط Grant با همان برنامه و مبلغ پذیرفته شود، و جست‌وجوی Assignment را به رکوردهای فعال محدود کردم.

**وضعیت:** پیاده‌سازی کد این بخش انجام شده، اما صحت اجرایی آن را تأییدشده اعلام نمی‌کنم. طبق درخواست قبلی شما Build و تست اجرا نکردم. `git diff --check` اجرا شد و چند ایراد فاصله‌گذاری در فایل Enum نشان داد؛ آن‌ها را تغییر ندادم چون خارج از اصلاح فعلی بودند. برای استفاده عملیاتی نیز دو کلید مستقل HMAC باید در تنظیمات امن محیط فراهم شوند و وجود قید یکتای کوپن در دیتابیس مقصد تأیید شود.

فایل اصلی این بخش: [WalletBenefitsService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletBenefitsService.cs).

---

## user
الان وضعیت چطوره به غیر از کلید و refund و تست تمام کار های دیگه تمام شده است ؟


---

## assistant
خیر، هنوز نمی‌توانم بگویم همهٔ کارهای دیگر تمام شده‌اند. کد مسیرهای کارت، کوپن، پنل مدیریت و صفحهٔ «کیف پول من» نوشته شده، اما **اتصال Card Match به Checkout** انجام نشده است؛ فعلاً کاربر باید آن را از صفحهٔ کیف پول اجرا کند. همچنین یکپارچگی نهایی این مسیرها بررسی نشده است.

پس حتی با کنار گذاشتن کلیدها، Refund و تست، وضعیت فعلی «پیاده‌سازی بخش‌های اصلی» است، نه پایان قطعی پروژه.

---

## user
برای قسمت اتصال Card match به checkout چه تسک های باقی مانده بگو بهم؟
با مصرف حداقل توکن


---

## assistant
برای اتصال Card Match به Checkout، این کارها مانده است:

1. در Checkout، `Recipient` انتخاب‌شده را از درخواست موجود بخوانیم و تعلق آن به خریدار را بررسی کنیم.
2. پیش از محاسبه سهم Wallet، کارت همان Recipient را Match کنیم تا Assignment و، برای `FixedForUser`، Grant ثبت شود. در `Manual` فقط Assignment ساخته شود.
3. پس از Match موفق، مانده Wallet و مبلغ‌های `T/W/G` را از دادهٔ به‌روز محاسبه کنیم؛ نتیجهٔ مالی قبلی را استفاده نکنیم.
4. مسیر خطا و Retry را طوری وصل کنیم که Match ناموفق پرداخت عادی را خراب نکند و Grant تکراری ایجاد نشود.
5. به کاربر در Checkout نشان دهیم کارت تطبیق داده شد و چه اعتباری برای پرداخت قابل استفاده است.

اتصال خودکار هنوز انجام نشده؛ فعلاً Match فقط از صفحهٔ «کیف پول من» قابل اجراست.

---

## user
پنج کار باقی مانده را نیز به ترتیب انجام بده تام تمام بشود با مصرف حداقل توکن


---

## assistant
مسیر Checkout را بررسی می‌کنم و پنج مورد را در همان جریان موجود وصل می‌کنم.

---

## assistant
در Checkout، فهرست اعتبارها پیش از تطبیق کارت بارگذاری می‌شود. تطبیق را هنگام انتخاب کیف پول اجرا می‌کنم و سپس فهرست و مانده را تازه می‌خوانم؛ درست پیش از آماده‌سازی پرداخت هم همان عملیات را تکرار می‌کنم تا مسیر سرور به اجرای مرورگر وابسته نباشد.

---

## assistant
اتصال Card Match به Checkout انجام شد. با انتخاب کیف پول، کارت گیرنده بررسی می‌شود و فهرست اعتبارهای قابل استفاده تازه می‌شود. پیش از آماده‌سازی پرداخت نیز سرور تطبیق را دوباره اجرا می‌کند؛ تطبیق تکراری Grant دوم نمی‌سازد. پیام نتیجه در Checkout نمایش داده می‌شود و شکست تطبیق، مسیر پرداخت عادی را متوقف نمی‌کند.

فایل‌های اصلی: [MyWalletController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/MyWalletController.cs)، [OrderController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs)، [_OrderSummaryForPay.cshtml](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml) و [WalletBenefitsService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletBenefitsService.cs).

بررسی فاصله‌گذاری تغییرات Checkout خطایی نشان نداد. طبق خواستهٔ شما Build و تست اجرا نکردم؛ بنابراین عملکرد اجرایی را تأییدشده اعلام نمی‌کنم.

---

## user
لان دیگه کاری باقی نمانده است و همه چی تمامه؟


---

## assistant
نه، هنوز نمی‌شود گفت «همه‌چیز تمام است». **کدنویسی بخش‌های توافق‌شده، از جمله اتصال Card Match به Checkout، انجام شده**؛ اما Build و تست اجرا نشده و عملکرد نهایی تأیید نشده است.

مواردی که خودتان بر عهده گرفتید نیز باقی‌اند: Refund، Retry و تلاش پرداخت دوم. علاوه بر آن، کلیدهای HMAC باید در محیط تنظیم شوند. بنابراین وضعیت فعلی «پیاده‌سازی انجام‌شده، با اعتبارسنجی و موارد واگذارشدهٔ باقی‌مانده» است.

---

## user
یک پرامپت در cmd بهم بده که این کلید هارو ست کنه مقدارش رو خودم می زارم


---

## assistant
در **Command Prompt (cmd)** اجرا کن و مقدارهای داخل گیومه را با دو کلید مستقلِ Base64 جایگزین کن:

```cmd
setx WalletSecurity__CardHmacKey "CARD_BASE64_KEY"
setx WalletSecurity__CouponHmacKey "COUPON_BASE64_KEY"
```

هر کلید پس از Decode باید دست‌کم **۳۲ بایت** باشد. `setx` روی پنجرهٔ فعلی اثر ندارد؛ برنامه را از یک پنجرهٔ جدید اجرا یا سرویس را دوباره راه‌اندازی کن.

---

## user
لیست کلیه فایل های که تغییر دادی رو بهم بده


---

## assistant
فایل‌هایی که در **مرحله اخیر کارت، کوپن و اتصال Card Match به Checkout** تغییر دادم:

**Backend**

- [WalletProgramDto.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletProgramDto.cs)
- [WalletBenefitsDto.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/DTO/WalletBenefitsDto.cs)
- [IWalletBenefitsService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Core/Services/IWalletBenefitsService.cs)
- [WalletProgramService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletProgramService.cs)
- [WalletGrantService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletGrantService.cs)
- [WalletBenefitsService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl/WalletBenefitsService.cs)
- [WalletCardController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletCardController.cs)
- [WalletCouponBatchController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/WalletCouponBatchController.cs)
- [MyWalletController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerce/Controllers/MyWalletController.cs)
- [OrderController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/Areas/ECommerceAdminApi/Controllers/OrderController.cs)
- [ModuleInitializer.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilCommerce/YashilCommerce.Web/ModuleInitializer.cs)
- [Index.cshtml](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/MyWallet/Index.cshtml)
- [_OrderSummaryForPay.cshtml](P:/Projects/walletProject/Wallet/backEnd/Yashil.WebHost/Views/Order/_OrderSummaryForPay.cshtml)

**پنل مدیریت Angular**

- [wallet-program-detail.component.ts](P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.ts)
- [wallet-program-detail.component.html](P:/Projects/walletProject/Wallet/clientApp/ecommerce/wallet-program/wallet-program-detail.component.html)

این فهرست مربوط به مرحله اخیر است. فایل‌های تغییرکردهٔ مراحل قدیمی‌تر را نمی‌توانم صرفاً از وضعیت فعلی Git با اطمینان به تغییرات خودم نسبت بدهم، چون تغییرات شما هم در همان Working Tree وجود دارد.

---

## user
اگر ممکنه از اول تا الان هر فایل جدیدی ایجاد کردی یا تغییر دادی رو با آدرسش در namesplace یا فیزیکال بررسی کن و بهم بده که در پروژه اصلی جایگذاری کنم


---

## assistant
فهرست فایل‌های فعلی مرتبط با Wallet را از Git و مسیرهای پروژه جمع می‌کنم. فایل‌هایی را که سهم تغییرات من و شما در آن‌ها قابل تفکیک نیست، جدا مشخص می‌کنم تا برای انتقال به پروژه اصلی اشتباهی رخ ندهد.

---

## assistant
بررسی کردم. **Git فقط وضعیت فعلی فایل‌ها را نشان می‌دهد و نویسندهٔ هر تغییر را مشخص نمی‌کند.** بنابراین فهرست زیر، فایل‌های مرتبط با Wallet است که اکنون در این Workspace جدید یا تغییریافته‌اند؛ برای انتقال، مسیر را نسبت به ریشهٔ `P:\Projects\walletProject\Wallet\` حفظ کن. فایل‌های Entity، Mapping و Enum را که خودت نیز تغییر داده‌ای، جدا آورده‌ام.

### فایل‌های جدید Backend

**`P:\Projects\walletProject\Wallet\backEnd\Modules\YashilCommerce\YashilCommerce.Core\DTO\`**

```text
CustomerOrderFundingDto.cs
CustomerWalletBalanceDto.cs
WalletAssignmentDto.cs
WalletAuditDto.cs
WalletBenefitsDto.cs
WalletGrantDto.cs
WalletOrderPreparationDto.cs
WalletProgramDto.cs
WalletRefundDto.cs
WalletScopeDto.cs
WalletSettlementDto.cs
```

**`P:\Projects\walletProject\Wallet\backEnd\Modules\YashilCommerce\YashilCommerce.Core\Services\`**

```text
ICustomerOrderFundingService.cs
ICustomerWalletBalanceService.cs
IWalletAssignmentService.cs
IWalletAuditService.cs
IWalletBenefitsService.cs
IWalletEligibilityService.cs
IWalletGrantService.cs
IWalletOrderPreparationService.cs
IWalletProgramService.cs
IWalletRefundService.cs
IWalletScopeService.cs
IWalletSettlementService.cs
```

**`P:\Projects\walletProject\Wallet\backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\ServiceImpl\`**

```text
CustomerOrderFundingService.cs
CustomerWalletBalanceService.cs
WalletAssignmentService.cs
WalletAuditService.cs
WalletBenefitsService.cs
WalletEligibilityService.cs
WalletGrantService.cs
WalletOrderPreparationService.cs
WalletProgramService.cs
WalletRefundService.cs
WalletScopeService.cs
WalletSettlementService.cs
```

**سایر فایل‌های جدید Backend**

```text
backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\JobImpl\ExpireWalletProgramsJob.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerce\Controllers\MyWalletController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletAssignmentController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletAuditController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletCardController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletCouponBatchController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletGrantController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletProgramController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletRefundController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletScopeController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletSettlementController.cs
backEnd\Yashil.WebHost\Views\MyWallet\Index.cshtml
```

### فایل‌های موجود که تغییر کرده‌اند

```text
backEnd\Modules\YashilCommerce\YashilCommerce.Core\DTO\WalletReserveRequestDto.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Core\DTO\WalletReserveResultDto.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Core\Repositories\IWalletReservationRepository.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\RepositoryImpl\WalletReservationRepository.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\ServiceImpl\WalletReservationService.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\ServiceImpl\OrderService.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\ServiceImpl\PaymentService.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Infrastructure\ServiceImpl\ModifiedOrderItemService.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerce\Controllers\OrderController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerce\ViewModels\OrderStatisticsPagePresentationViewModel.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerce\ViewModels\OrderSummaryViewModel.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerce\ViewModels\ShoppingCartParam.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\OrderController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\Areas\ECommerceAdminApi\Controllers\WalletController.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\ModuleInitializer.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Web\ModuleInitializerPartial.cs
backEnd\Yashil.WebHost\Views\Order\_OrderListItem.cshtml
backEnd\Yashil.WebHost\Views\Order\_OrderSummary.cshtml
backEnd\Yashil.WebHost\Views\Order\_OrderSummaryForPay.cshtml
backEnd\Yashil.WebHost\Views\Shared\_Layout.cshtml
```

### پنل مدیریت Angular

```text
clientApp\ecommerce\ecommerce-routing.module.ts
clientApp\ecommerce\index.ts
clientApp\ecommerce\wallet-program\wallet-program-detail.component.html
clientApp\ecommerce\wallet-program\wallet-program-detail.component.ts
clientApp\ecommerce\wallet-program\wallet-program-list.component.html
clientApp\ecommerce\wallet-program\wallet-program-list.component.ts
```

### فایل‌های مشترک با تغییرات شما؛ قبل از جایگزینی، Merge شوند

```text
backEnd\Data\Yashil.Core\Entities\Payment.cs
backEnd\Data\Yashil.Core\Entities\WalletProgram.cs
backEnd\Data\Yashil.Core\Entities\WalletReservation.cs
backEnd\Data\Yashil.Core\Entities\WalletScope.cs
backEnd\Data\Yashil.Core\Entities\WalletTransaction.cs
backEnd\Data\Yashil.Infrastructure\Data\YashilAppDbContext.cs
backEnd\Modules\YashilCommerce\YashilCommerce.Core\Enums\Enums.cs
```

پوشهٔ `backEnd\Modules\YashilPayment\` در این Workspace به‌صورت کامل **Untracked** است و طبق سابقه، کد آن به پروژه اضافه شده بود؛ نمی‌توانم آن را به‌عنوان فایل‌های ساخته‌شدهٔ خودم معرفی کنم. پوشه‌های `obj`، `bin` و `node_modules` را هم منتقل نکن. فایل‌های بالا Build و تست نهایی نشده‌اند؛ برای فایل‌های موجود، به‌ویژه Entity و Mapping، کپیِ جایگزینِ مستقیم می‌تواند تغییرات پروژهٔ اصلی را از بین ببرد.

---

## user
از حافظت بررسی کن در YashilPayment چه فایل های رو اضافه کردی یا تغییر دادی و بهم بگو 


---

## assistant
حافظهٔ ثبت‌شده و خلاصهٔ همین گفتگو را بررسی می‌کنم تا فایل‌های YashilPayment را فقط در حدی که سابقه پشتیبانی می‌کند، به خودم نسبت بدهم.

---

## assistant
حافظهٔ ثبت‌شده فهرستی از تغییرات `YashilPayment` ندارد؛ پس نمی‌توانم با اطمینان بگویم کدام فایل را **من** ایجاد یا ویرایش کرده‌ام. از بررسی کد فعلی، اتصال‌های Wallet/Mixed در این چهار فایل دیده می‌شود و این‌ها فایل‌هایی‌اند که باید برای انتقال بررسی کنی:

- [HekmatOnlinePaymentGatewayController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/HekmatOnlinePaymentGatewayController.cs)
- [HekmatOnlinePaymentGatewayService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/HekmatOnlinePaymentGatewayService.cs)
- [VibePaymentController.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Web/Areas/Payment/Controllers/VibePaymentController.cs)
- [VibeService.cs](P:/Projects/walletProject/Wallet/backEnd/Modules/YashilPayment/YashilPayment.Infrastructure/ServiceImpl/VibeService.cs)

کل ماژول `YashilPayment` در Git فعلی **Untracked** است؛ به همین دلیل Git هم نسخهٔ پایه‌ای برای تفکیک فایل‌های اضافه‌شده از فایل‌های ویرایش‌شده در اختیار نمی‌گذارد. این چهار فایل «دارای کد Wallet» هستند، نه فهرست اثبات‌شدهٔ تغییرات من.

<oai-mem-citation>
<citation_entries>
MEMORY.md:1-3|note=[confirmed no consolidated task memory]
</citation_entries>
<rollout_ids>
</rollout_ids>
</oai-mem-citation>

---

## user
<environment_context>
  <current_date>2026-10-23</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root><root>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry><entry access="read"><path>C:\Users\pc\.codex\visualizations\2026\10\02\01a0fd58-414c-79f2-bdd0-de34d4e92a4c\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user
الان می خوام این کد بیسی که تو تغییر داده و توسعه دادی را با کدی اصلی خودم merge کنم بنظرت بهترین راه حل انجام این کار چیه؟ الان یک برنچ جدید در کل اصلی پروژه ایجاد کردم در ادامه چطور مرج کنم که کم دردسر ترین روش باشه کد های اصلی پروژه بهم نریزه کانفلیکت ها مشخص بشه و کد های جدید هم درست قرار بگیرند؟


---

## assistant
کم‌دردسرترین راه، **انتقال مرحله‌ای با commitهای انتخابی** است؛ پوشهٔ فعلی را یک‌جا روی پروژهٔ اصلی کپی یا `git add .` نکن. در Workspace فعلی فایل‌های تولیدی و کل ماژول `YashilPayment` هم Untracked هستند و انتقال یک‌جای آن‌ها ریسک بالایی دارد.

پیشنهاد من:

1. **در همین نسخهٔ توسعه‌یافته، فایل‌های موردنظر را انتخابی Stage کن.** `obj`، `bin`، `node_modules` و فایل‌های تولیدی را کنار بگذار. پیش از commit، `git diff --cached --stat` و `git diff --cached` را ببین.
2. **تغییرات را به چند commit کوچک تقسیم کن:** هستهٔ Wallet، اتصال Commerce/Checkout، رابط کاربری، و اتصال‌های Vibe/HekmatOnline. این کار محل Conflict را روشن می‌کند.
3. **در برنچ جدید پروژهٔ اصلی، commitها را به همین ترتیب `cherry-pick` کن**، به شرطی که دو نسخه تاریخچهٔ Git مشترک داشته باشند. بعد از هر commit، Conflict را همان‌جا حل و تغییرات را بررسی کن.
4. **Entity، DbContext، Enum و فایل‌های مشترک Payment را دستی Merge کن.** نسخهٔ توسعه‌یافته را کامل روی نسخهٔ اصلی جایگزین نکن؛ تغییرات جدید پروژهٔ اصلی باید حفظ شوند.
5. **برای `YashilPayment` ابتدا فایل‌های چهارگانهٔ دارای اتصال Wallet را با نسخهٔ اصلی مقایسه کن.** چون کل این ماژول در Workspace توسعه Untracked است، آن را به‌صورت یک پوشهٔ کامل منتقل نکن.
6. پس از انتقال، `git diff` برنچ جدید نسبت به مبنا را مرور کن؛ سپس Build و تست را **در پروژهٔ اصلی** اجرا کن و تا رفع خطاها Merge نهایی انجام نده.

اگر تاریخچهٔ دو مخزن مشترک نیست، به‌جای `cherry-pick` از **patch انتخابیِ همان commitها** استفاده کن. مهم‌ترین قدم بعدی، تهیهٔ commit اول فقط از فایل‌های Wallet بدون فایل‌های تولیدی و بدون تغییرات مشترکی است که منشأشان مشخص نیست.

---

## user
من میگم ماژولی که تو تغییر دادی رو کپی پیست کنم جای ماژول اصلی بعدش در گیت نگاه کنم فایل های که مشکلی ندارند که اوکیه فایل های که کانفلیکت خوردن رو مرج میکنم از مازول کامرس شروع کنم


---

## assistant
از Commerce شروع‌کردن منطقی است، اما یک نکته مهم: **کپی‌پیست روی فایل موجود Conflict گیت ایجاد نمی‌کند؛ فایل را بی‌صدا جایگزین می‌کند.** `git diff` فقط تفاوت نتیجه را نشان می‌دهد و نمی‌فهمد کدام تغییرِ پروژهٔ اصلی از دست رفته است.

اگر می‌خواهی با کپی پیش بروی، این ترتیب کم‌ریسک‌تر است:

1. برنچ جدید پروژهٔ اصلی را در وضعیت **Clean** نگه دار و نقطهٔ شروعش را commit کن.
2. فایل‌های **جدید** `YashilCommerce` را در مسیر متناظر کپی کن.
3. فایل‌های **موجود** را جایگزین نکن؛ آن‌ها را با ابزار مقایسهٔ دوطرفه باز کن و تغییرات Wallet را به نسخهٔ اصلی منتقل کن. به‌ویژه `OrderService`، `PaymentService`، `OrderController`ها و `ModuleInitializer`.
4. بعد از هر دسته، `git diff` را بررسی و یک commit جدا ثبت کن.
5. در پایان سراغ `YashilPayment` و سپس Entity/Mapping/Enum برو؛ این‌ها بیشترین ریسکِ بازنویسی تغییرات اصلی را دارند.

پس بله، **Commerce اولین ماژول باشد**؛ ولی برای فایل‌های موجود «مقایسه و ادغام» انجام بده، نه جایگزینی. اگر Conflict واقعی و قابل‌نمایش در Git می‌خواهی، باید تغییرات را با `cherry-pick` یا `git merge` وارد کنی، نه با کپی‌پیست.

---

## user
مثلا الان کپی کردم میگه فایل های هم نام داری ویندوز خب اونارو بگم از پروژه کیف پول در پروژه اصلی Replace کنه بعدا در ویژوال استدیو گیت چینج نگاه کنم و مرج رو انجام بدم؟


---

## assistant
نه؛ **Replace نکن.** بعد از Replace، بخش‌های متفاوت فایل اصلی از بین می‌روند و Visual Studio در Git Changes فقط نسخهٔ جایگزین‌شده را نشان می‌دهد؛ Conflict قابل Merge تولید نمی‌شود.

برای فایل‌های هم‌نام:

1. فعلاً در پیام ویندوز **Skip** را بزن.
2. فایل‌های جدید و بدون هم‌نام را کپی کن.
3. فایل‌های هم‌نام را با **Compare** در Visual Studio یا WinMerge باز کن و تغییرات Wallet را داخل فایل اصلی ادغام کن.
4. بعد از هر چند فایل، Git Changes را بررسی و commit کن.

اگر قبلاً Replace کرده‌ای، هنوز می‌شود نسخهٔ قبلی را از Git بازیابی و با فایل Wallet مقایسه کرد؛ فقط قبل از بازیابی، فایل Replace‌شده را در مسیر جداگانه نگه دار.

---

## user
اوکی فعلا skip میکنم بعدا از کجا بفهمم کدوم ها مشابه هستن و این که باید دونه به دونه چطور این کارو انجام بدم؟


---

## assistant
بله. بعد از **Skip**، برای پیدا کردن فایل‌های هم‌نام، ساده‌ترین راه **مقایسهٔ دو پوشه با WinMerge** است:

1. پوشهٔ `YashilCommerce` پروژهٔ Wallet را سمت چپ و پوشهٔ `YashilCommerce` پروژهٔ اصلی را سمت راست باز کن.
2. گزینهٔ **Include Subfolders** را فعال کن و Compare بزن.
3. فایل‌های **Identical** کاری ندارند؛ فایل‌های **Different** همان‌هایی هستند که باید بررسی شوند؛ **Left only** فایل‌های جدید Wallet هستند.
4. هر فایل Different را باز کن و فقط تغییرات موردنیاز Wallet را از چپ به فایل اصلی در راست منتقل کن. کل فایل را Replace نکن.
5. بعد از هر چند فایل، در Visual Studio بخش **Git Changes** را ببین و یک commit با عنوان همان بخش ثبت کن.

برای شروع، `DTO` و `Services` معمولاً ساده‌ترند. `OrderService`، `PaymentService`، Controllerهای Order و `ModuleInitializer` را آخر Commerce انجام بده، چون تغییرات دو طرف در آن‌ها بیشتر به هم وابسته‌اند.

---

## user
یک ویدیو برای این کارو پیدا کن و لینکشو بفرست چون نمیدونم باید دقیقا چیکار کنم


---

## assistant
یک آموزش تصویری مناسب برای مقایسهٔ دو پوشه و ادغام فایل‌های متفاوت در WinMerge پیدا می‌کنم و لینک مستقیمش را می‌فرستم.

---

## assistant
در این محیط دسترسی به یوتیوب برقرار نشد، بنابراین نمی‌خواهم لینک یک ویدیوی مشخص را بدون بررسی حدس بزنم. این [جست‌وجوی آمادهٔ ویدیوهای آموزش WinMerge](https://www.youtube.com/results?search_query=WinMerge+compare+folders+merge+files+tutorial) دقیقاً موضوع **مقایسهٔ دو پوشه و ادغام تفاوت فایل‌ها** را نشان می‌دهد.

در ویدیو دنبال این سه مرحله باش: **Select folders → Compare → باز کردن فایل‌های Different و انتقال تغییرات موردنیاز**. قبل از انتقال، نسخهٔ فایل پروژهٔ اصلی را نگه دار؛ دکمهٔ کپی کل فایل را نزن.

---

## user
علت این خطا چیه در ویژوال استدیو؟
Severity	Code	Description	Project	File	Line	Suppression State
Error (active)	CS0006	Metadata file 'P:\Projects\Inst\yashilsoft\Modules\YashilTms\YashilTms.Web\obj\Debug\net8.0\ref\YashilTms.Web.dll' could not be found	YashilCommerce.Web	P:\Projects\Inst\yashilsoft\Modules\YashilCommerce\YashilCommerce.Web\CSC	1	



---

## assistant
این خطا یعنی `YashilCommerce.Web` به پروژهٔ `YashilTms.Web` وابسته است، اما فایل خروجیِ کامپایل آن پیدا نشده. **معمولاً CS0006 پیامد یک خطای قبلی در Build پروژهٔ `YashilTms.Web` است، نه علت اصلی.**

در Visual Studio:

1. در **Error List** نمایش را روی **Build Only** و **Entire Solution** بگذار.
2. اولین خطای مربوط به `YashilTms.Web` را پیدا کن؛ همان را باید اول رفع کرد.
3. روی `YashilTms.Web` راست‌کلیک و **Build** بزن. اگر موفق شد، دوباره Solution را Build کن.

اگر هیچ خطای قبلی ندارد، بررسی کن پروژهٔ `YashilTms.Web` در Solution موجود و برای کانفیگ `Debug` فعال باشد، سپس **Restore NuGet Packages** و **Rebuild** را انجام بده. مسیر `obj\...\ref\YashilTms.Web.dll` فایل تولیدی Build است و نباید دستی کپی شود.

---

## user
Severity	Code	Description	Project	File	Line	Suppression State
Error (active)	CS0006	Metadata file 'P:\Projects\Inst\yashilsoft\Data\Yashil.Infrastructure\obj\Debug\net8.0\ref\Yashil.Infrastructure.dll' could not be found	YashilCommerce.Web	P:\Projects\Inst\yashilsoft\Modules\YashilCommerce\YashilCommerce.Web\CSC	1	



---

## assistant
این هم همان نوع خطاست، اما وابستگیِ بالادستی‌تر را نشان می‌دهد: **`Yashil.Infrastructure` کامپایل نشده** و در نتیجه `YashilCommerce.Web` فایل مرجع آن را پیدا نمی‌کند.

روی پروژهٔ `Yashil.Infrastructure` جداگانه **Build** بزن و **اولین خطای آن پروژه که CS0006 نیست** را بفرست. تا آن خطا رفع نشود، خطاهای CS0006 در پروژه‌های وابسته ادامه پیدا می‌کنند. فایل DLL را دستی کپی نکن.

---

## user
All packages are already installed and there is nothing to restore.
Time Elapsed: 00:00:00.1480587
========== Finished ==========




---

## assistant
این پیام فقط می‌گوید **بسته‌های NuGet نصب‌اند**؛ موفقیت Build را تأیید نمی‌کند.

حالا در Solution Explorer روی **`Yashil.Infrastructure` → Build** بزن. سپس در پنجرهٔ **Output**، گزینهٔ **Build** را انتخاب کن و اولین خطای آن پروژه را بفرست. خطای اصلی همان‌جاست؛ `CS0006` در `YashilCommerce.Web` پیامدِ ساخته‌نشدن آن است.

---

## user
حل نشد چیکار کنم حلشه


---

## assistant
مسیر پروژهٔ اصلی و وابستگی‌های `Yashil.Infrastructure` را بررسی می‌کنم تا علت ساخته‌نشدن DLL مشخص شود.

---

## assistant
علت اصلی پیدا شد: هنگام انتقال، دو Entity غیرمرتبط با Wallet اینترفیس `IApplicationBasedEntity` گرفته‌اند، در حالی که `ApplicationId` آن‌ها nullable است و قرارداد اینترفیس `int` می‌خواهد. همان دو اعلان کلاس را اصلاح می‌کنم و دوباره Build می‌گیرم.

---

## assistant
علت اصلی را پیدا کردم: در پروژهٔ اصلی، به دو Entity اینترفیس `IApplicationBasedEntity` اضافه شده، ولی `ApplicationId` آن‌ها `int?` است و اینترفیس `int` می‌خواهد. این دو خط را اصلاح کن:

- [AppConfig.cs](P:/Projects/Inst/yashilsoft/Data/Yashil.Core/Entities/AppConfig.cs:7): `,IApplicationBasedEntity` را از اعلان کلاس حذف کن.
- [ResourceAttributeRole.cs](P:/Projects/Inst/yashilsoft/Data/Yashil.Core/Entities/ResourceAttributeRole.cs:7): همان بخش را حذف کن.

**نوع `ApplicationId` را تغییر نده**؛ این فیلدها از قبل nullable بوده‌اند. سپس به‌ترتیب `Yashil.Core`، `Yashil.Infrastructure` و `YashilCommerce.Web` را Build کن.

من Build تشخیصی را اجرا کردم و همین دو خطای `CS0738` را پیدا کردم. درخواست دسترسی برای ویرایش فایل‌های پروژهٔ اصلی رد شد، بنابراین فایلی را تغییر ندادم و رفع‌شدن Build را تأیید نمی‌کنم.

---

## user
این دوتارو اصلاح کردم 
appconfig
resouceAtrtributeRole
تعداد خطا ها از 300 رسید به 24 
الان اینو دارم
Severity	Code	Description	Project	File	Line	Suppression State
Error (active)	CS0006	Metadata file 'P:\Projects\Inst\yashilsoft\Modules\YashilCommerce\YashilCommerce.Web\obj\Debug\net8.0\ref\YashilCommerce.Web.dll' could not be found	YashilLight.Web	P:\Projects\Inst\yashilsoft\Modules\YashilLight\YashilLight.Web\CSC	1	



---

## user
این دوتارو اصلاح کردم
appconfig
resouceAtrtributeRole
تعداد خطا ها از 300 رسید به 24
الان اینو دارم
Severity	Code	Description	Project	File	Line	Suppression State
Error (active)	CS0006	Metadata file 'P:\Projects\Inst\yashilsoft\Modules\YashilCommerce\YashilCommerce.Web\obj\Debug\net8.0\ref\YashilCommerce.Web.dll' could not be found	YashilLight.Web	P:\Projects\Inst\yashilsoft\Modules\YashilLight\YashilLight.Web\CSC	1


---

## user
<environment_context>
  <current_date>2026-10-26</current_date>
  <timezone>Asia/Tehran</timezone>
  <filesystem><workspace_roots><root>P:\Projects\walletProject\Wallet\backEnd</root></workspace_roots><permission_profile type="managed"><file_system type="restricted"><entry access="read"><special>:root</special></entry><entry access="write"><path>P:\Projects\walletProject\Wallet\backEnd</path></entry><entry access="write"><special>:slash_tmp</special></entry><entry access="write"><special>:tmpdir</special></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.git</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.agents</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.codex</path></entry><entry access="read"><path>P:\Projects\walletProject\Wallet\backEnd\.aws</path></entry></file_system></permission_profile></filesystem>
</environment_context>

---

## user
can we continue work?

---

## user
hi whats up?

---

## user
hi whats up?

---

