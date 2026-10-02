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
