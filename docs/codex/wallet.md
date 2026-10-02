# Wallet Domain Context

The detailed business source is `سند_مرجع_پیاده_سازی_Wallet_MVP_Agent_Ready.md`. This short file is only a navigation layer.

## Core model
`WalletProgram -> WalletScope -> WalletAssignment -> WalletReservation -> WalletTransaction`

Additional branches:
- `WalletProgram -> WalletCardGroup -> WalletCardMember`
- `WalletProgram -> WalletCouponBatch -> WalletCoupon`

## Critical invariants
- Available balance must not become negative.
- Allocated amount must not exceed Program total.
- Reserved + consumed + expired must not exceed assigned amount.
- Reserve, Capture, Release and Claim must be idempotent.
- Concurrency must prevent double-spend.
- Ledger records must not be deleted/overwritten.
- Only one active Assignment is allowed for `(UserId, WalletProgramId)`.

## Financial flow
`Eligibility -> Reserve -> Payment -> Capture on success OR Release on failure`
`Capture` creates a final ledger debit; `Refund` creates a new ledger credit.

## Security
Raw PAN must not be stored in Wallet. Card matching uses a normalized-card HMAC fingerprint and a masked display value.

## Do not guess
The reference document explicitly says external FK relationships should not be guessed before the real schema is inspected.
