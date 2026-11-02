# Wallet Session Recovery

## Scope and Evidence

This document recovers `Wallet-Session-Extract.md` and cross-checks the current checkout. It is not a feature specification and does not approve behaviour that the history did not confirm.

Evidence labels:

- **Confirmed decision**: explicitly decided in the recovered conversation.
- **Implemented, source-confirmed**: code currently exists at the cited path; runtime correctness is not implied.
- **Implemented, unverified**: history reports implementation/static checking, but no successful build, integration test, database verification, or gateway test is recorded.
- **Proposal / unresolved**: design or question that must not be treated as approved behaviour.

## Project Overview and Architecture

- Backend targets .NET 8 and uses shared entities in `backEnd/Data/Yashil.Core`, Commerce Core/Infrastructure/Web projects, EF Core/SQL Server through `YashilAppDbContext`, and `Yashil.WebHost`. Payment is a separate `backEnd/Modules/YashilPayment` Core/Infrastructure/Web module.
- The established feature path is contracts/DTOs in `YashilCommerce.Core`, implementations in `YashilCommerce.Infrastructure`, HTTP controllers in `YashilCommerce.Web`, and DI in `YashilCommerce.Web/ModuleInitializer.cs`.
- Angular administration is under `clientApp/ecommerce`; customer MVC/checkout is under `backEnd/Yashil.WebHost/Views` and Commerce's ECommerce area.
- Wallet's intended MVP flow is `WalletProgram -> WalletScope -> WalletAssignment -> WalletReservation -> WalletTransaction`, plus Card and Coupon branches. The documented financial sequence is `Eligibility -> Reserve -> Payment -> Capture`, or `Release` after a definite failure; a refund is a new credit ledger record.

### Relevant Paths and Examples

- Domain guidance: `docs/codex/wallet.md`, `سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md`.
- Entities/mapping: `backEnd/Data/Yashil.Core/Entities/Wallet*.cs`, `backEnd/Data/Yashil.Infrastructure/Data/YashilAppDbContext.cs`.
- Financial services: `WalletReservationService.cs`, `WalletSettlementService.cs`, `WalletGrantService.cs`, `WalletRefundService.cs`, `WalletProgramService.cs`, `WalletBenefitsService.cs`, and `WalletAuditService.cs` in `backEnd/Modules/YashilCommerce/YashilCommerce.Infrastructure/ServiceImpl`.
- Commerce/payment integration: `WalletOrderPreparationService.cs`, `CustomerOrderFundingService.cs`, `OrderService.cs`, `PaymentService.cs`, `OrderController.cs`, `VibeService.cs`, and `HekmatOnlinePaymentGatewayService.cs`.
- Existing decision records: `backEnd/docs/decisions/Mixed-Wallet-Payment-Decision-and-Blockers.md` and `backEnd/docs/decisions/HekmatOnline-Mixed-Payment-Go-No-Go-Gates.md`.

## Confirmed Domain Rules and Decisions

### Core invariants

- **Confirmed**: `AvailableAmount = AssignedAmount - ConsumedAmount - ReservedAmount - ExpiredAmount`; it must not be negative.
- **Confirmed**: `AllocatedAmount` must not exceed `WalletProgram.TotalAmount`; the program budget cannot be reduced below `AllocatedAmount`.
- **Confirmed**: `ReservedAmount + ConsumedAmount + ExpiredAmount` cannot exceed `AssignedAmount`.
- **Confirmed**: financial transitions must be atomic, idempotent, concurrency-safe, and ledger rows are append-only (no deletion/rewrite).
- **Confirmed**: only one active Assignment is permitted for a `(UserId, WalletProgramId)` pair.
- **Confirmed security rule**: never persist raw PAN; card matching uses a normalized-card HMAC fingerprint plus a masked display value.

### Statuses and exact values

Current source declares:

- `WalletReservationStatusType`: `Reserved=3184`, `Released=3185`, `Expired=3186`, `Captured=3210`.
- `WalletProgramStatusEnum`: `Draft=3168`, `Active=3169`, `Suspended=3170`, `Expired=3171`, `Closed=3172`.
- `WalletScopeTypeEnum`: `Company=3173`, `Representation=3174`, `City=3175`, `Product=3176`.
- `WalletAssignmentStatusTypeEnum`: `Active=3177`, `Suspended=3178`, `Expired=3179`; source types: `Manual=3180`, `Organization=3181`, `Card=3182`, `Coupon=3183`.
- Ledger transaction types: `Grant=3187`, `Capture=3188`, `Refund=3189`, `Expired=3190`, `ManualAdjustment=3191`; directions: `Debit=3192`, `Credit=3193`.
- Card groups: `Draft=3194`, `Active=3195`, `Suspended=3196`, `Closed=3197`; card members: `Active=3198`, `Blocked=3199`, `Closed=3200`.
- Coupon batches: `Draft=3201`, `Active=3202`, `Closed=3203`, `Cancelled=3204`; coupons: `Available=3205`, `Claimed=3206`, `Expired=3207`, `Cancelled=3208`.
- Allocation methods: `FixedForUser=3166`, `Manual=3167`.

### Program, eligibility, grants, cards, and coupons

- **Confirmed**: Program is created in `Draft`; only valid data, positive budget, and valid dates may activate it. `Expired` and `Closed` are final; close is rejected while a `Reserved` reservation exists.
- **Confirmed**: Scope semantics are AND across scope types and OR within a type; because Reserve is order-level, every order item must be eligible.
- **Confirmed**: initial manual Assignment has zero balance; it is not a Grant.
- **Confirmed**: a Grant is a `WalletTransaction` with `Type=3187`, `Direction=3193`, and positive amount. Same `ApplicationId + Code` and same request replays; different data conflicts.
- **Confirmed**: `FixedForUser` is equal split: `AmountPerUser = truncate(TotalAmount / TargetUserCount, 2)`; remainder stays unallocated. `AllocatedAmount` remains the sum of successful Grants, not a per-user amount.
- **Confirmed**: Card Match reads `Recipient.CardNumber`; any resulting Grant belongs to the authenticated order buyer. Fixed programs may grant automatically; manual programs create only a zero-balance Assignment.
- **Confirmed**: Coupon Claim immediately grants `AmountPerCoupon`, reuses an active Assignment, and Claim plus Grant must be atomic/idempotent. Fixed programs require `AmountPerCoupon == AmountPerUser`; automatic Grant for manual coupons is disallowed. Raw eight-digit coupon codes are shown only once at batch creation; a lost response leaves the batch Draft for cancellation/recreation.

## Reservations, Payment, Capture, Release, Refund, and Expiration

### Reservation

- **Implemented, source-confirmed**: `WalletReservationService.ReserveAsync` requires a current user/application, owned Order, existing Payment for that Order/Application, active Assignment/Program, eligibility, a positive two-decimal amount, and sufficient available balance. It uses a context transaction and conditional updates of Assignment/Program reserved balances.
- **Implemented, source-confirmed**: current reservation identity is payment-based: `IdempotencyKey` is rebuilt as `WalletReserve:Payment:{PaymentId}`; replay validates `PaymentId`, Assignment, user, and amount. `ExpiresAt` is set to UTC creation time plus 30 minutes.
- **Implemented, source-confirmed**: the reservation query treats an active reservation as `Status == Reserved`; it uses `Payment.OrderId` rather than a reservation `OrderId` field.

### Payment and mixed payment

- **Confirmed decision**: for Mixed only, `T = W + G`, `0 < W < T`, `Payment.Total = G`, and `WalletReservation.Amount = W`; Reservation belongs to that `PaymentId`. The same attempt must replay the same records/amounts; a new attempt requires a fresh key.
- **Confirmed decision**: complete an Order only after definite gateway success, successful Capture, and zero debt. An unknown Verify/Capture result must preserve the Reservation; it is not grounds for Release or final failure.
- **Implemented, source-confirmed but unverified**: `WalletOrderPreparationService.PrepareAsync`, `OrderController`, `VibeService`, and `HekmatOnlinePaymentGatewayService` contain dedicated Mixed branches, prepare a Payment/Reservation pair, and route verification to capture before a zero-debt completion check.
- **Out of scope/unsupported**: Full Wallet (`W = T`), Hybrid, IranKish, and Saderat were explicitly excluded from the final Mixed work.

### Capture, release, and refund

- **Implemented, source-confirmed**: `WalletSettlementService.CaptureAsync`/`ReleaseAsync` exist. Capture moves the full amount from reserved to consumed, changes Reservation to `Captured`, and appends a negative `Capture=3188` ledger row. Release changes Reservation to `Released` and removes only reserved balances.
- **Confirmed decision**: Release is only allowed after a definite gateway outcome (`Unsuccessful`, `Cancel`, or `Void`); late definitive Payment success must remain recoverable for capture.
- **Implemented, source-confirmed**: `WalletRefundService.RefundAsync` records a positive `Refund=3189` ledger row with `ParentId` equal to the capture transaction, permits partial/full refund only up to `CapturedAmount - TotalRefunded`, and reduces consumed balances without changing allocation or Reservation.
- **Historical scope conflict**: later conversation says Refund was handed to the user. Source contains a Refund service/controller, but gateway/refund-finality and order-modification integration were not completed or verified. Treat wallet-side Refund as implemented but unverified; do not claim end-to-end Refund is complete.

### Expiration and audit

- **Confirmed decision / implemented, source-confirmed**: on Assignment expiry, open Reservations become `Expired`; `ReservedAmount` is removed, `ExpiredAmount` increases, and a negative `Expired=3190` ledger row is added once for `Assigned - Consumed - Expired`. `ConsumedAmount` and `AllocatedAmount` do not change.
- **Unresolved**: expiry of the 30-minute Reservation alone, without Assignment/Program expiry, was not fully specified/closed.
- **Implemented, source-confirmed**: `WalletAuditService.AuditProgramAsync` and `GET /api/WalletAudit/{walletProgramId}` are read-only reconciliation facilities. They report balance/ledger/reservation/linkage/duplicate-code anomalies and exclude `ManualAdjustment` from derived comparisons because its financial meaning is unresolved.

## Authorization and Context

- **Confirmed**: User and Application must come from authenticated application context, never request input. Service methods independently enforce ownership/Application boundaries; controller authorization alone is insufficient.
- **Implemented, source-confirmed**: administrative endpoints use existing `PostEntity`, `UpdateEntity`, or `GetEntity` WalletProgram permissions rather than a new permission model.
- **Unverified**: resource definitions and role grants live in runtime data and were not recoverable from source.

## Implementation Status

### Implemented and source-confirmed

- Program lifecycle/budget, Scope management and order eligibility, manual zero-balance Assignment, Grant, Reserve, Capture/Release, wallet-side Refund, expiration processing, Audit, Benefits/Card/Coupon, customer balance/funding summaries, administrative Angular Program UI, and customer `MyWallet` UI all have current source/DI registrations.
- Current source also contains customer checkout Card Match calls and Mixed Vibe/HekmatOnline routes. HMAC key names are `WalletSecurity:CardHmacKey` and `WalletSecurity:CouponHmacKey`; no values were recovered or recorded here.

### Implemented but unverified

- Historical validation was static/syntax/diff review. The recovered builds repeatedly failed before compilation because required `Yashil.Common.*`/DevExpress dependencies could not be restored (notably requested `9.8.45` while an examined local source had only `9.8.42`), or because `project.assets.json` was missing in earlier checkouts.
- No successful end-to-end build, SQL Server integration test, physical production-schema/index check, Angular compilation, live-gateway verification, or concurrency test was recovered.
- The current checkout has Wallet source files, but their runtime path is therefore evidence of implementation, not verified behaviour.

## Known Conflicts, Risks, and Open Questions

- **Current source conflict**: `WalletReservation.PaymentId` is nullable, but current `ReserveAsync` rejects `PaymentId <= 0`; Full Wallet still lacks a stable Reservation-to-Order completion contract.
- **Current mapping conflict**: `YashilAppDbContext` currently shows only global unique `UX_WalletReservation_IdempotencyKey`, not the earlier historical proposal/claim of `UNIQUE(ApplicationId, IdempotencyKey)` plus filtered active-order uniqueness. It also shows defaults of `1` for reservation/program statuses, not the wallet enum values. Database deployment state is unknown.
- **Model/documentation conflict**: reference material described signed ledger amounts without `Direction` and Assignment `SourceId`/`SourceRef`; current entity has `WalletTransactionDirectionType` and lacks those Assignment fields. Do not redesign scaffolded entities solely to reconcile this.
- **Mixed-payment Go/No-Go remains unresolved**: gateway partial-payment acceptance/finality, final gateway-refund proof, correct debt formula and order modification/cancellation allocation, `[shp].[SetOrderItemPriceRightShare]` behaviour for `G`, physical uniqueness/index deployment, and real timeout/lost-response recovery must be proven before operational enablement.
- **Current working tree**: `Enums.cs` is modified and `Enums.cs.bak` is untracked; they pre-existed this recovery and were not changed. `Wallet-Session-Extract.md` is also untracked.

## Recommended Next Task

**Task: perform a read-only build-and-integration readiness audit, then produce an approved test plan for the existing Wallet/Mixed paths.** Do not activate or expand Mixed Payment first.

Prerequisites: restore the exact internal NuGet/DevExpress feed versions; identify a safe disposable SQL Server database whose deployed schema/indexes can be queried; obtain non-secret runtime configuration for a test environment; confirm gateway sandbox/finality contracts for the one selected gateway.

Acceptance criteria:

1. The affected Commerce and Payment projects compile successfully without source changes unrelated to findings.
2. The deployed mapping/index/default discrepancies above are recorded as confirmed or fixed by an explicitly approved schema task.
3. Integration tests cover replay/conflict, concurrent reserve/grant/capture/refund, atomic rollback, late paid capture, definite failure release, and no early Order completion.
4. A gateway sandbox proves the selected Mixed flow sends `G`, captures `W` once, leaves unknown results pending, and completes only at zero debt.
5. Refund finality and order modification/cancellation allocation are approved before enabling user-facing Mixed checkout.

## History Gaps

- The extract contains many attachment placeholders (`Pasted text contains the user's request`) but not their full attachment contents. Their exact acceptance criteria, wording, and any omitted decisions cannot be recovered beyond later assistant summaries and user messages preserved inline.
- No explicit encrypted-record marker was found. Unsupported/omitted material is limited to those attachment payloads and any external database, gateway, runtime permission, or configuration state not captured in source/history.
- Several historical reports describe changing mappings or later code states that differ from current source. Those are recorded above as conflicts, not silently reconciled.
