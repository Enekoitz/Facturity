# Testing

Given the importance of maintaining a well-defined modular monolithic architecture—with the long-term goal of extracting high-demand modules into microservices—a dedicated .NET 10 testing project is created for each developed module.

This facilitates the migration of individual modules—along with their corresponding test suites—into microservices.

Furthermore, these tests are integrated into the CI/CD deployment pipeline, ensuring that any code modifications pass an automated testing gate, thereby minimizing the risk of failures in the production environment.

## Features

- Framework: **.NET 10**

- Libraries used:
- **xUnit**
- **FakeTimeProvider**
- **NetArchTest**
- **FluentAssertions**

## Structure

| Test Type    | Scope/Coupling Level                                            |
| ------------ | --------------------------------------------------------------- |
| Unit         | Module-level: `Facturity.<module>.UnitTests`                    |
| Integration  | Full system (API + modules): `Facturity.IntegrationTests`       |
| Architecture | Full system (API + modules): `Facturity.ArchitectureTests`      |

## Conventions

- Naming convention: _MethodName_StateUnderTest_ExpectedBehavior_

## Additional Architecture Testing Project

A dedicated project for architecture tests has also been developed. This is essential for verifying—during every build—that the rules and conventions established by the development team are strictly upheld:

- **Correct references:** Adherence to the defined architecture, ensuring the Domain layer remains entirely independent.
- **Module isolation:** Modules must remain isolated from one another, with no cross-references.
- **Communication via public contracts:** Modules must interact solely through public contracts (e.g., integration events, exposed interfaces) rather than internal types.

## Notes
None at this time.

---
_Last reviewed: 2026-10-10_