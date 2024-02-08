[![NuGet version (ETAMP)](https://img.shields.io/nuget/v/ETAMP.svg?style=flat-square)](https://www.nuget.org/packages/ETAMP/)
# ETAMP Protocol - Encrypted Token and Message Protocol

## [ETAMP Documentation](https://blackdreams-organization.gitbook.io/etamp/)
## [NuGet Package](https://www.nuget.org/packages/ETAMP/)

## Introduction
ETAMP (Encrypted Token and Message Protocol) is a comprehensive .NET library tailored for secure message and token encryption and validation. Utilizing the power of elliptic curve cryptography (ECC), ETAMP offers a robust solution for ensuring the security of digital communication. The library is designed with flexibility in mind, allowing for the generation of customizable signed tokens and secure message transmissions.

## Features
- **Advanced Cryptography**: Leverages ECC for digital signatures, ensuring high-security standards.
- **Token Generation**: Create signed or unsigned tokens with user-defined payloads, suitable for various secure data transmission needs.
- **Token Validation**: Robust methods to validate the authenticity and integrity of tokens.
- **JWT Integration**: Supports JSON Web Tokens (JWT) with claims validation for integrity and lifetime verification.
- **Customization**: Flexible architecture supports custom curves, keys, and algorithms.

## Installation
Install ETAMP via NuGet Package Manager:
```shell
Install-Package ETAMP
```
## Usage Examples

### Creating ETAMP Tokens
```csharp
var etamp = new Etamp();

public class Order : BasePayload {
    public string ItemName { get; set; }
    public decimal Price { get; set; }
}

string token = etamp.CreateETAMP("order", new Order(), true, 1.0);
```

### Validating ETAMP Tokens
```csharp
var validator = new ValidateToken(new VerifyWrapper(new EcdsaWrapper()));
bool valid = validator.VerifyETAMP(token);
```

## Cryptographic Components
- **EcdsaWrapper**: A factory class for creating ECDsa instances.
- **VerifyWrapper**: Handles cryptographic verification processes.
- **Flexible Security**: Supports integration with hardware security modules.

## Additional Features
- Lightweight validation methods focusing on cryptographic checks.
- Methods for verifying JWT properties like lifetime, issuer, and audience.
- Integrates seamlessly with existing .NET applications.

## Contributing
Contributions are welcome! If you're interested in contributing, please feel free to submit pull requests or open issues for bugs and feature requests.
