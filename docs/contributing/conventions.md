# Contribution conventions

These conventions are explicitly documented in the root README and should be followed for consistent branch, issue, and pull request hygiene.

## Branch naming

```text
feat/v<Milestone>.<Issue#>-<short-title>
```

Example:

```text
feat/v0.1.7-config-ci
```

## Issue naming

```text
Issue <Issue#>: <message>
```

## Pull request naming

```text
PR <Issue#>: <Issue Title>
```

## Code review format

```text
Acceptance Criteria:
1. [ ] AC1 from Issue
2. [ ] AC2 from Issue
3. [ ] AC3 from Issue
etc...

Manual Verification:
1. [ ] Step 1 from Issue
2. [ ] Step 2 from Issue
3. [ ] Step 3 from Issue
etc...

Notes
<Any additional comments.>

Criteria met/failed. PR can/can NOT be merged.
```

## Practical workflow

The project expects contributors to keep changes scoped to the current milestone and architecture. For behavior changes, prefer a failing test before the fix and validate with the smallest relevant test command before broader verification.

## Related references

- [Project README](../../README.md)
- [Project documentation index](../README.md)
- [Testing overview](../testing/test-overview.md)
