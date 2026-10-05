# Continuous integration

The repository uses GitHub Actions to validate the project on every push to `main` and every pull request targeting `main` through [.github/workflows/verify-build-test.yml](../../.github/workflows/verify-build-test.yml).

## Workflow overview

The workflow is intentionally minimal and directly reflects the current project structure:

1. checks out the repository,
2. installs the repo's .NET SDK from [global.json](../../global.json),
3. runs `dotnet restore`,
4. runs `dotnet build --no-restore`, and
5. runs `dotnet test --no-build --verbosity normal`.

This ensures the same success criteria are enforced locally and in CI.

## Current .NET SDK pin

The repository is configured to use .NET 10.0.200 with `rollForward: latestFeature` in [global.json](../../global.json):

```json
{
  "sdk": {
    "version": "10.0.200",
    "rollForward": "latestFeature"
  }
}
```

The workflow installs the same SDK family with `actions/setup-dotnet@v4` and `dotnet-version: 10.0.x`.

## Failure behavior

If either the build or test phase fails, the workflow terminates with a failing GitHub Actions job. That means a pull request cannot pass without a successful restore, build, and test run.

## Local parity check

The project can be verified locally with:

```bash
dotnet test --nologo
```

This mirrors the CI job and is the quickest way to confirm the current branch is in a valid state before creating or updating a pull request.

## Related references

- [Project README](../../README.md)
- [Development setup](../development/local-run.md)
- [Testing overview](../testing/test-overview.md)
