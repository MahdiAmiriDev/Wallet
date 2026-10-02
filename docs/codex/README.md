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

## Source-of-truth order
1. Existing code and executable behavior.
2. Approved repository documentation/specification.
3. The detailed Wallet business document for Wallet rules.
4. Task prompt.

If two sources conflict, do not silently choose. Report the conflict and use the most directly verifiable source.
