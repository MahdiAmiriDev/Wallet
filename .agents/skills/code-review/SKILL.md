---
name: code-review
description: Review a change for correctness, architecture, regressions, security, and unnecessary scope.
---

## Summary
Use when reviewing an implementation or final diff.

## Steps
1. Read the diff and identify the intended behavior.
2. Compare changed code with nearby established patterns.
3. Check correctness, error paths, security, concurrency, and data consistency.
4. Check tests/build evidence and missing coverage.
5. Report findings by severity with file/line references where available.

## Self-check
Do not report style preferences as defects unless they violate a repository rule or create a concrete risk.
