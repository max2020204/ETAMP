[![NuGet version (ETAMP)](https://img.shields.io/nuget/v/ETAMP.svg?style=flat-square)](https://www.nuget.org/packages/ETAMP/)
# ETAMP Protocol - Detailed Overview

## [ETAMP in ChatGPT](https://chat.openai.com/g/g-GUGWcZ5gR-encrypted-token-and-message-protocol)
## [ETAMP Documentation](https://blackdreams-organization.gitbook.io/etamp/)
## [Nuget](https://www.nuget.org/packages/ETAMP/)
## Introduction to ETAMP
ETAMP (Encrypted Token and Message Protocol) is a sophisticated .NET library designed to create and validate encrypted tokens and messages. This protocol leverages the robustness of elliptic curve cryptography (ECC) standards, ensuring high security in digital communications. ETAMP is particularly valuable for its ability to generate signed tokens with user-defined payloads, making it an ideal solution for secure data transmission over networks. The tokens are verifiable for both integrity and authenticity, providing a secure means of data exchange.

## Installation Process
1. **NuGet Package Installation**: ETAMP can be seamlessly integrated into your .NET projects via the NuGet package manager. To install, simply execute the command:
    ```shell
    Install-Package ETAMP
    ```
    This command fetches and installs the latest version of the ETAMP library into your project.

## Creating ETAMP Tokens

1. **Instantiation of the ETAMP Class**: Begin by creating an instance of the `Etamp` class. This class may be configured with optional parameters such as ECDsa instances, elliptic curves, signature algorithms, and hash algorithms for customized cryptographic settings.
    ```csharp
    var etamp = new Etamp();
    ```
2. **Defining a Payload**: Define a custom payload class by inheriting from `BasePayload`. Add properties to this class as per your data requirements. For example, to create an order token:
    ```csharp
    public class Order : BasePayload {
       public string ItemName { get; set; }
       public decimal Price { get; set; }
    }
    ```
3. **Token Generation**: Generate a signed ETAMP token by calling the `CreateETAMP` method. This method requires parameters such as update type, payload instance, signature flag, and version.
    ```csharp
    string token = etamp.CreateETAMP("order", new Order(), true, 1.0);
    ```
    To generate an unsigned token, use `CreateETAMPWithoutSignature` method.

## Validating ETAMP Tokens

1. **Token Validation Setup**: Instantiate a `ValidateToken` object by passing an instance of `VerifyWrapper`, which in turn encapsulates an `EcdsaWrapper`.
    ```csharp
    var validator = new ValidateToken(new VerifyWrapper(new EcdsaWrapper()));
    ```
2. **Token Verification**: Use the `VerifyETAMP` method to validate the token's authenticity.
    ```csharp
    bool valid = validator.VerifyETAMP(token);
    ```
    For comprehensive validation including JWT claims and lifetime, utilize the `FullVerify` methods.

## Cryptographic Components

- The `EcdsaWrapper` factory class is used to create ECDsa instances for cryptographic operations.
- Custom implementations of `IVerifyWrapper` handle the cryptographic verification process.
- The architecture supports custom curves, keys, algorithms, and extensions for a tailored cryptographic solution.

## Additional Features

- ETAMP includes lightweight validation methods focusing exclusively on cryptographic checks.
- It offers methods for verifying JWT properties like lifetime, issuer, and audience.
- The flexible architecture of ETAMP allows for the integration of custom extensions.
- The protocol supports integration with hardware security modules for enhanced security, making it suitable for high-security applications.
