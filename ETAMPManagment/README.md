# ETAMP (Encrypted Token And Message Protocol)
## [NuGet Package](https://www.nuget.org/packages/ETAMP/)

ETAMP (Encrypted Token And Message Protocol) is a lightweight .NET library designed for enhancing message and transaction security within applications. It incorporates advanced cryptographic techniques, including JWT (JSON Web Tokens), ECDSA (Elliptic Curve Digital Signature Algorithm), and optional ECDH (Elliptic Curve Diffie-Hellman) encryption, to provide robust protection against unauthorized access and data tampering. Ideal for systems requiring secure data transmission, ETAMP is easy to integrate and ensures the highest levels of data integrity and confidentiality.

## Features

- Create and verify JWT tokens using ECDSA
- Optional encryption of JWT tokens using ECDH
- Message integrity verification using ECDSA digital signatures
- Easy integration with existing projects
- High scalability and performance

## Installation

Install ETAMP via NuGet Package Manager:
```shell
Install-Package ETAMP
```

## Usage

### Creating a Basic ETAMP

```csharp
using ETAMPManagment;
using ETAMPManagment.Factories;
using ETAMPManagment.Models;

// Create an ETAMP factory
var etampFactory = new ETAMPFactory();
etampFactory.RegisterGenerator(ETAMPType.Base, () => new ETAMPBase());

// Create an ETAMP builder
var etampBuilder = new ETAMPBuilder(etampFactory);

// Create a basic ETAMP
var payload = new BasePayload();
var etamp = etampBuilder.CreateETAMP("update_type", payload).Build();
```

### Creating a Signed ETAMP

```csharp
using System.Security.Cryptography;
using ETAMPManagment.Wrapper;

// Create a signature wrapper
var ecdsa = ECDsa.Create();
var signWrapper = new SignWrapper(ecdsa, HashAlgorithmName.SHA512);

// Create an ETAMP factory
var etampFactory = new ETAMPFactory();
etampFactory.RegisterGenerator(ETAMPType.Sign, () => new ETAMPSign(signWrapper));

// Create an ETAMP builder
var etampBuilder = new ETAMPBuilder(etampFactory);

// Create a signed ETAMP
var payload = new BasePayload();
var etamp = etampBuilder.CreateSignETAMP("update_type", payload).Build();
```

### Creating an Encrypted ETAMP

```csharp
using ETAMPManagment.Encryption;
using ETAMPManagment.Factories;

// Create an encryption service factory
var encryptionServiceFactory = new EncryptionServiceFactory();
encryptionServiceFactory.RegisterEncryptionService("AES", () => new AesEncryptionService());

// Create an ETAMP factory
var etampFactory = new ETAMPFactory();
etampFactory.RegisterGenerator(ETAMPType.Encrypted, () => new ETAMPEncrypted(new EciesEncryptionService(new KeyExchanger(new KeyPairProvider()), encryptionServiceFactory, "AES")));

// Create an ETAMP builder
var etampBuilder = new ETAMPBuilder(etampFactory);

// Create an encrypted ETAMP
var payload = new BasePayload();
var etamp = etampBuilder.CreateEncryptedETAMP("update_type", payload).Build();
```

### Creating an Encrypted and Signed ETAMP

```csharp
using System.Security.Cryptography;
using ETAMPManagment.Encryption;
using ETAMPManagment.Factories;
using ETAMPManagment.Wrapper;

// Create a signature wrapper
var ecdsa = ECDsa.Create();
var signWrapper = new SignWrapper(ecdsa, HashAlgorithmName.SHA512);

// Create an encryption service factory
var encryptionServiceFactory = new EncryptionServiceFactory();
encryptionServiceFactory.RegisterEncryptionService("AES", () => new AesEncryptionService());

// Create an ETAMP factory
var etampFactory = new ETAMPFactory();
etampFactory.RegisterGenerator(ETAMPType.EncryptedSign, () => new ETAMPEncryptedSigned(signWrapper, new EciesEncryptionService(new KeyExchanger(new KeyPairProvider()), encryptionServiceFactory, "AES")));

// Create an ETAMP builder
var etampBuilder = new ETAMPBuilder(etampFactory);

// Create an encrypted and signed ETAMP
var payload = new BasePayload();
var etamp = etampBuilder.CreateEncryptedSignETAMP("update_type", payload).Build();
```

### Validating an ETAMP

```csharp
using ETAMPManagment.Validators;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

// Create an ETAMP validator
var ecdsa = ECDsa.Create();
var tokenSecurityKey = new ECDsaSecurityKey(ecdsa);
var jwtValidator = new JwtValidator();
var structureValidator = new StructureValidator(jwtValidator);
var signatureValidator = new SignatureValidator(new VerifyWrapper(ecdsa, HashAlgorithmName.SHA512));
var etampValidator = new ETAMPValidator(jwtValidator, structureValidator, signatureValidator);

// Validate the ETAMP
var isValid = await etampValidator.ValidateETAMP(etamp.ToString(), "audience", "issuer", tokenSecurityKey);
```

## License

This project is licensed under the [MIT License](https://github.com/max2020204/ETAMP/blob/master/LICENSE.txt).

## Contributing

Contributions are welcome! If you're interested in contributing, please feel free to submit pull requests or open issues for bugs and feature requests.
