# ETAMP Protocol Documentation

## Overview
ETAMP (Encrypted Token And Message Protocol) is a lightweight .NET library designed for enhancing message and transaction security within applications. It incorporates advanced cryptographic techniques, including JWT (JSON Web Tokens), ECDSA (Elliptic Curve Digital Signature Algorithm), and optional ECDH (Elliptic Curve Diffie-Hellman) encryption, to provide robust protection against unauthorized access and data tampering. Ideal for systems requiring secure data transmission, ETAMP is easy to integrate and ensures the highest levels of data integrity and confidentiality.

---

## Key Components

### `Etamp` Class
The `Etamp` class is the core of the ETAMP protocol, handling the creation and signing of secure tokens.

#### Properties
- **`Curve`**: The elliptic curve used in cryptographic operations, defaulting to the NIST P-521 curve.
- **`HashAlgorithm`**: The hash algorithm used for cryptographic signatures, defaulting to SHA-512.
- **`SecurityAlgorithm`**: The JWT security algorithm, defaulting to EcdsaSha512Signature.
- **`PrivateKey`**: The private key in PEM format, generated from the ECDSA object.
- **`PublicKey`**: The public key in PEM format, extracted from the ECDSA object.
- **`Ecdsa`**: The instance of the elliptic curve digital signature algorithm used for cryptographic operations.

#### Methods
- **`CreateETAMP<T>`**: Creates an ETAMP token with a unique message ID, payload, and optional signature.

### `ValidateToken` Class
The `ValidateToken` class is responsible for verifying the authenticity and integrity of ETAMP tokens.

#### Methods
- **`VerifyData`**: Verifies the given data against a specified signature.
- **`VerifyETAMP`**: Verifies the integrity and authenticity of an ETAMP token.

---

## Usage Guidelines

### Creating an ETAMP Token
1. Instantiate the `Etamp` Class**: Create an `Etamp` object, optionally specifying the ECDSA, elliptic curve, security algorithm, and hash algorithm.
   ```csharp
   var etamp = new Etamp();
   ```
2. Generate a Token: Call `CreateETAMP` method with the required payload and update type.
   ```csharp
   var token = etamp.CreateETAMP("UpdateType", payload, true, 1.0);
   ```
## Verifying an ETAMP Token
1. Instantiate the ValidateToken Class: Create a ValidateToken object with the ECDSA instance and hash algorithm.
   ```csharp
   var validator = new ValidateToken(ecdsa, HashAlgorithmName.SHA512);
   ```
2. Verify the Token: Use VerifyETAMP to validate the integrity and authenticity of the token.
   ```csharp
   bool isValid = validator.VerifyETAMP(token);
   ```
## Best Practices

- **Secure Key Management**: Ensure the secure storage and handling of private keys.
- **Regular Key Rotation**: Periodically rotate keys to maintain security.
- **Validation**: Always validate tokens before processing their content.
- **Error Handling**: Implement robust error handling to manage potential cryptographic errors.

## Advanced Features

- **Custom Curves**: While NIST P-521 is the default, ETAMP supports custom curves for specific requirements.
- **Flexible Hashing**: SHA-512 is the default hashing algorithm, but ETAMP can be configured to use other hash functions.
- **Extensibility**: ETAMP is designed to be extendable and can be adapted to include additional cryptographic mechanisms as needed.
   
