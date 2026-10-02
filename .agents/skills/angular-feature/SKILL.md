---
name: angular-feature
description: Implement or modify Angular features using the existing clientApp modules, shared base components, services, and routing patterns.
---

## Summary
Use for Angular UI, module, routing, component, model, or service changes.

## Steps
1. Find the closest existing feature with the same UI behavior.
2. Reuse shared base components/services before creating new abstractions.
3. Match the existing module, routing, component and naming conventions.
4. Implement only the required UI/client changes.
5. Use the repository's actual frontend verification command when available.
6. Inspect the diff for generated or unrelated changes.

## Self-check
- Existing shared components reused?
- Routing/module conventions preserved?
- API/service naming consistent?
- No unnecessary generated-file changes?
