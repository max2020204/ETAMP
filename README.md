# ETAMP Protocol - Encrypted Token and Message Protocol

## [ETAMP Documentation](https://blackdreams-organization.gitbook.io/etamp/)
## [NuGet Package](https://www.nuget.org/packages/ETAMP/)

## Introduction
ETAMP (Encrypted Token and Message Protocol) is a sophisticated .NET library designed for secure message and token encryption and validation. Built upon advanced cryptographic methods like ECC, AES, and ECDH, ETAMP ensures high security and flexibility for digital communication. The library's modular design allows extensive customization for various security needs.

## Features
- **Advanced Cryptography**: Utilizes ECC, AES, and ECDH for top-tier security.
- **Token Generation and Validation**: Efficient creation and verification of secure tokens.
- **ECDH and ECDSA Integration**: Enhanced security with Elliptic Curve Cryptography.
- **Flexibility and Customization**: Adaptable to specific security requirements with customizable curves, keys, and algorithms.
- **SOLID Principles Compliance**: Developed following SOLID principles for high maintainability and scalability.

## Installation
Install ETAMP via NuGet Package Manager:
```shell
Install-Package ETAMP
```

## Installation
Install ETAMP via NuGet Package Manager:
```shell
Install-Package ETAMP
```
## Usage Examples

### Creating ETAMP Tokens
To create an ETAMP token with an `Order` payload, first define and instantiate the `Order` class, then use the `CreateETAMP` method from the `ETAMP` class. Here's an example:

```csharp
var etamp = new ETAMP();

public class Order : BasePayload {
    public string ItemName { get; set; }
    public decimal Price { get; set; }
}

// Create an order instance and fill it with data
var order = new Order {
    ItemName = "Laptop",
    Price = 999.99M
};

// Create an ETAMP token with the order payload
string token = etamp.CreateETAMP("order", order, true, 1.0);
```
This example demonstrates how to create a new Order object with specific details (in this case, "Laptop" as the item name and a price of 999.99) and then create a token using this payload.

### Validating ETAMP Tokens
To validate an ETAMP token, you can use the `ValidateToken` class with various methods provided for different validation needs. Here is a detailed example of validating an ETAMP token:

```csharp
var validator = new ValidateToken(new VerifyWrapper(new EcdsaWrapper()));

// Example ETAMP token
string token = "[Your ETAMP Token]";

// Basic validation of the ETAMP token
bool isValidBasic = validator.VerifyETAMP(token);

// Full validation with JWT signature and custom ECDSA parameters
string audience = "[Expected Audience]";
string issuer = "[Expected Issuer]";
ECCurve curve = ECCurve.NamedCurves.nistP256; // Example curve
string publicKeyBase64 = "[Base64 Encoded Public Key]";

bool isValidFull = await validator.FullVerifyWithTokenSignature(token, audience, issuer, curve, publicKeyBase64);
```

### Encrypting ETAMP Tokens
ETAMP provides a robust mechanism for token encryption using `EciesEncryptionService`. This service requires proper initialization with key exchange and encryption service parameters. Here is a detailed implementation:

#### Initializing the `EciesEncryptionService`
First, initialize the required components for the `EciesEncryptionService`:

```csharp
// Initialize the ECDH Key Wrapper for key exchange
var ecdhKeyWrapper = new EcdhKeyWrapper();

// Create an Encryption Service Factory and register the AES encryption service
var encryptionServiceFactory = new EncryptionServiceFactory();
encryptionServiceFactory.RegisterEncryptionService("AES", () => new AesEncryptionService());

// Now, initialize the ECIES Encryption Service
var eciesEncryptionService = new EciesEncryptionService(ecdhKeyWrapper, encryptionServiceFactory, "AES");
```
In this example, an instance of EcdhKeyWrapper is created for the elliptic curve key exchange. The EncryptionServiceFactory is used to register and create an AES encryption service, which is then passed to the EciesEncryptionService constructor along with the ecdhKeyWrapper and the encryption type ("AES").

#### Creating and Encrypting an ETAMP Token
Next, create and encrypt an ETAMP token:
```csharp
// Assuming you have a payload for the ETAMP token
public class Order : BasePayload {
    public string ItemName { get; set; }
    public decimal Price { get; set; }
}

var order = new Order {
    ItemName = "Laptop",
    Price = 999.99M
};

// Initialize the ETAMP class for token creation
var etamp = new ETAMP();

// Create an ETAMP token
string token = etamp.CreateETAMP("order", order, true, 1.0);

// Initialize the ETAMPEncryption service with the ECIES encryption service
var etampEncryption = new ETAMPEncryption(eciesEncryptionService);

// Encrypt the ETAMP token
string encryptedToken = etampEncryption.EncryptETAMPToken(JsonConvert.SerializeObject(token));

// 'encryptedToken' now contains the encrypted ETAMP token
```

#### Directly Creating an Encrypted ETAMP Token
Instead of creating a standard token and then encrypting it, you can directly create an encrypted token with the `CreateEncryptETAMP` method. This method streamlines the process, combining token creation and encryption into one step. 

First, initialize the required components for `EciesEncryptionService` and `ETAMPEncryption`:

```csharp
// Initialize the ECDH Key Wrapper for key exchange
var ecdhKeyWrapper = new EcdhKeyWrapper();

// Create an Encryption Service Factory and register the AES encryption service
var encryptionServiceFactory = new EncryptionServiceFactory();
encryptionServiceFactory.RegisterEncryptionService("AES", () => new AesEncryptionService());

// Now, initialize the ECIES Encryption Service
var eciesEncryptionService = new EciesEncryptionService(ecdhKeyWrapper, encryptionServiceFactory, "AES");

// Initialize ETAMPEncryption with the ECIES Encryption Service
var etampEncryption = new ETAMPEncryption(eciesEncryptionService);
```
Next, define your payload and use CreateEncryptETAMP to directly create an encrypted token:
```csharp
// Define a payload for the ETAMP token
public class Order : BasePayload {
    public string ItemName { get; set; }
    public decimal Price { get; set; }
}

var order = new Order {
    ItemName = "Laptop",
    Price = 999.99M
};

// Directly create and encrypt the ETAMP token
ETAMPEncrypted encryptedETAMP = etampEncryption.CreateEncryptETAMP("order", order, true, 1.0);

// 'encryptedETAMP' contains the encrypted ETAMP token along with its cryptographic details
```
This approach is particularly useful when you want to ensure the security of the token content right from its creation, without handling an unencrypted token at any stage.

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
