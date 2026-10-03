# Codex Knowledge Map

This directory is intentionally small. `AGENTS.md` is the entry point; these documents are loaded only when relevant.

| Need | Read |
|---|---|
| Understand repository boundaries | `architecture.md` |
| Backend feature | `architecture.md` + relevant `.agents/skills/backend-feature` |
| Angular feature | `frontend.md` + relevant `.agents/skills/angular-feature` |
| Wallet business behavior | `wallet.md` + the detailed Wallet reference |
| Wallet/financial feature | `wallet.md` + `.agents/skills/wallet-feature` |
| Review existing changes | `.agents/skills/code-review` |
| Define a new feature | `docs/specifications/TEMPLATE.md` |
| Agent role/background | `ROLE.md` |

## Context policy
Do not read every document before every task. Start from the task, locate the smallest relevant set of files, then expand context only when evidence requires it.

## Source of truth
- Repository code and executable behavior describe the system's existing behavior.
- The user request defines the expected behavior of the feature.
- An approved feature specification, when present, formally defines the desired behavior, business rules, and acceptance criteria.

If existing behavior differs from the requested or approved behavior, identify and report the difference. Clarify the desired behavior from the user request or specification before implementation; do not decide it independently. Read only the code and documentation relevant to the feature.
