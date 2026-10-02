---
name: backend-feature
description: Implement or modify a .NET backend feature using the repository's existing Core, Infrastructure, and Web patterns.
---

## Summary
Use for non-trivial backend feature work.

## Steps
1. Find the closest existing feature and inspect its Core, Infrastructure, and Web path.
2. Identify affected entities, services, repositories, DTO/view models, endpoints and tests.
3. Produce a concise plan before implementation when the task is non-trivial.
4. Implement by extending existing patterns; avoid unrelated refactoring.
5. Build/test the affected area using commands actually available in the repository.
6. Inspect the diff and report changed files, verification results, and remaining risks.

## Self-check
- Existing layer boundaries preserved?
- Similar project pattern reused?
- Validation/error handling consistent?
- No unrelated files changed?
