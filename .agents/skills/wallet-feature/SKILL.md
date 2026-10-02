---
name: wallet-feature
description: Implement Wallet or financial-flow changes while preserving balance, ledger, idempotency, concurrency, and security invariants.
---

## Summary
Use whenever a task changes Wallet eligibility, assignment, reservation, payment, capture, release, refund, coupon, card, or ledger behavior.

## Steps
1. Read `docs/codex/wallet.md` and the relevant section of the detailed Wallet reference.
2. Find the closest existing implementation and inspect real schema relationships before coding.
3. Identify the balance/ledger and concurrency effects of the change.
4. Implement the smallest change consistent with existing architecture.
5. Verify affected tests/builds and inspect the final diff.

## Self-check
- No negative available balance?
- No double-spend under concurrency?
- Required operations idempotent?
- Ledger append-only?
- No raw PAN or secret persisted?
- No invented FK/business rule?
