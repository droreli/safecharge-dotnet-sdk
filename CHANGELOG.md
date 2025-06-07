# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial SDK-wide logging support using `Microsoft.Extensions.Logging.Abstractions`. Loggers can be injected via `ILoggerFactory` into `Safecharge.CreateAsync` methods and `SafechargeRequestExecutor` constructors.
- Enhanced input validation framework in `Utils/Validations/Guard.cs` with new methods:
  - `RequiresValidCurrencyCode` for ISO 4217 currency codes.
  - `RequiresValidCountryCode` for ISO 3166-1 alpha-2 country codes.
  - `RequiresValidEnum<TEnum>` for validating string and enum typed values against enum definitions.
- Applied new validation methods to key model properties (e.g., `UserAddress.Country`, `MerchantInfo.HashAlgorithm`) and request constructors (e.g., currency codes in `OpenOrderRequest`, `PaymentRequest`).
- Comprehensive unit tests for the new validation methods in `Guard.cs` (`Safecharge.Test.Core/GuardTests.cs`).
- Unit tests in `ModelValidationsTest.cs` to verify guards in `UserAddress.Country` and `MerchantInfo` constructor.
- Unit tests in `OpenOrderTest.cs` and `PaymentWorkflowTest.cs` for currency code validation in request constructors.
- Fluent builder pattern for `PaymentRequest` accessible via `PaymentRequest.CreateBuilder(...)` to simplify request object construction.
- Enabled XML documentation file generation in `Safecharge.csproj`.
- Added extensive XML documentation comments for public APIs, including interfaces (`ISafecharge`, `ISafechargeRequestExecutor`), core classes (`Safecharge`, `SafechargeRequestExecutor`), request class hierarchy, key model classes, and utility classes (`Guard`, `ApiConstants`, `Constants`, and representative enums).

### Changed
- Refactored `Safecharge.cs` constructors to be private and introduced public static asynchronous factory methods (`CreateAsync`). This addresses potential deadlocks from blocking on async calls during construction and improves initialization robustness.
- Upgraded `Newtonsoft.Json` package dependency from version 12.0.3 to 13.0.3 in `Safecharge.csproj`.
- Systematically applied `.ConfigureAwait(false)` to `await` calls in `SafechargeRequestExecutor.cs` and `Safecharge.cs` to prevent potential deadlocks in UI/ASP.NET Classic applications.

### Fixed
- Resolved potential deadlocks in `Safecharge.cs` by making its initialization fully asynchronous through the `CreateAsync` factory pattern.
- Addressed various build issues encountered during the session, including:
  - Case sensitivity problems in `.sln` and `.csproj` file paths.
  - .NET SDK installation and runtime discovery issues for test execution (by installing appropriate SDKs/runtimes and updating test package versions).
  - Corrected `SGen` task incompatibility with .NET Core MSBuild by disabling `GenerateSerializationAssemblies` in `Safecharge.csproj`.
  - Fixed C# compiler errors related to static members in non-static classes, incorrect method signatures, and missing `using` directives that arose during refactoring.
- Corrected the `GetDccDetails` method in `Safecharge.cs` to be properly asynchronous by adding `async` and `await`.
- Fixed several badly formed XML comment tags (e.g., `<<param` to `<param`), duplicate param tags, and unresolvable `cref` attributes in `ISafecharge.cs` and `ISafechargeRequestExecutor.cs`.

## [1.0.5] - 2020-01-01
### Added
- Initial public release of the Safecharge REST API SDK.
  *(This is a placeholder entry as specific changes for this historical version are not known.)*
