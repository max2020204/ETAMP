# ETAMP (Encrypted Token And Message Protocol)

## [NuGet Package](https://www.nuget.org/packages/ETAMP/)

ETAMP (Encrypted Token And Message Protocol) is a comprehensive .NET library designed to enhance the security of message and transaction handling across applications. It utilizes advanced cryptographic techniques such as JWT (JSON Web Tokens), ECDSA (Elliptic Curve Digital Signature Algorithm), ECDH (Elliptic Curve Diffie-Hellman) encryption, and ECIES (Elliptic Curve Integrated Encryption Scheme), ensuring secure and reliable data exchanges.

## Features

- **Secure JWT creation and verification with ECDSA**: Ensures that tokens are not only created securely but are also verifiable.
- **ECDH and ECIES encryption**: Offers advanced encryption for tokens, providing an additional layer of security.
- **Robust ECDSA digital signature**: Guarantees the integrity and authenticity of messages.
- **Flexible integration**: Supports dependency injection (DI) to suit various application architectures.
- **High performance**: Optimized for high throughput and scalability, suitable for enterprise-level solutions.
- **Comprehensive API**: Includes a builder pattern for creating customizable ETAMP models.

## Installation

Install ETAMP using the NuGet Package Manager:

```shell
Install-Package ETAMP
```

## Usage

ETAMP supports Dependency Injection (DI) for seamless integration into .NET projects. Below is an example of how to use the ETAMP controller to create, sign, encrypt, and compress an ETAMP model within a .NET Core application.

### Dependency Injection (DI) Setup

1. **Register ETAMP Services in Startup:**

   Configure your application's startup class to include ETAMP services:

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddETAMPServices();
       // Additional service registrations
   }
   ```

2. **Use ETAMP in Your Application:**

   Here's an example of how you can implement the ETAMP controller in your application to utilize the cryptographic functionalities:

   ```csharp
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.IdentityModel.Tokens;
   using System.Security.Cryptography;
   using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
   using ETAMPManagement.Encryption.Interfaces;
   using ETAMPManagement.ETAMP.Base.Interfaces;
   using ETAMPManagement.Extensions;
   using ETAMPManagement.Factory.Interfaces;
   using ETAMPManagement.Managment;
   using ETAMPManagement.Models;
   using ETAMPManagement.Services.Interfaces;
   using ETAMPManagement.Validators.Interfaces;
   using ETAMPManagement.Wrapper.Interfaces;

   namespace WebApplication2.Controllers
   {
       [Route("api/[controller]")]
       [ApiController]
       public class ETAMPController : ControllerBase
       {
           private readonly IETAMPBase _etampBase;
           private readonly IECDsaCreator _ecdsaCreator;
           private readonly ISigningCredentialsProvider _signingCredentialsProvider;
           private readonly ICompressionServiceFactory _serviceFactory;
           private readonly ISignWrapper _signWrapper;
           private readonly IEciesEncryptionService _eciesEncryption;
           private readonly IEncryptionService _encryptionService;
           private readonly IKeyExchanger _key;
           private readonly IKeyPairProvider _pairProvider;
           public readonly IETAMPValidator _etampValidator;

           public ETAMPController(IETAMPBase etamp, IECDsaCreator ecdsa, ISignWrapper wrapper,
                                   ISigningCredentialsProvider signing, ICompressionServiceFactory serviceFactory,
                                   IEciesEncryptionService eciesEncryption, IEncryptionService encryptionService,
                                   IKeyExchanger key, IKeyPairProvider pairProvider, IETAMPValidator etampValidator)
           {
               _ecdsaCreator = ecdsa;
               _signingCredentialsProvider = signing;
               _serviceFactory = serviceFactory;
               _eciesEncryption = eciesEncryption;
               _encryptionService = encryptionService;
               _key = key;
               _pairProvider = pairProvider;
               _etampValidator = etampValidator;
               _etampBase = etamp;
               _signWrapper = wrapper;
           }

           [HttpGet]
           public async Task<string> GetETAMP()
           {
               // Create an ECDSA provider for the specified curve (nistP521 in this case)
               IECDsaProvider provider = _ecdsaCreator.CreateECDsa(ECCurve.NamedCurves.nistP521);

               // Create an ECDsaSecurityKey based on the ECDSA provider
               ECDsaSecurityKey ecDsaSecurityKey = new ECDsaSecurityKey(provider.GetECDsa());

               // Initialize the sign wrapper with the ECDSA provider and SHA512 hash algorithm
               _signWrapper.Initialize(provider, HashAlgorithmName.SHA512);

               // Initialize the signing credentials provider with the ECDSA provider
               _signingCredentialsProvider.Initialize(provider);

               // Set the security algorithm for the signing credentials provider
               _signingCredentialsProvider.SecurityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature;

               // Initialize the ECIES encryption service with the key exchanger and encryption service
               _eciesEncryption.Initialize(_key, _encryptionService);

               // Initialize the key exchanger with the key pair provider
               _key.Initialize(_pairProvider);

               // Create an ECDiffieHellman object for key derivation
               ECDiffieHellman ecDiffieHellmanCng = ECDiffieHellmanCng.Create();

               // Derive the key using the ECDiffieHellman public key
               _key.DeriveKey(ecDiffieHellmanCng.PublicKey);

               // Create an ETAMP model with the specified message, payload, and version
               // Sign the model using the sign wrapper
               // Encrypt the token within the model using the ECIES encryption service
               ETAMPModel model = _etampBase
                   .CreateETAMPModel("Message", new BasePayload(), 1, _signingCredentialsProvider)
                   .Sign(_signWrapper)
                   .EncryptToken(_eciesEncryption);

               // Compress the model using the Deflate compression algorithm
               string deflate = model.Compress(_serviceFactory, CompressionNames.Deflate);

               // Decompress the compressed model using the Deflate compression algorithm
               ETAMPModel decompressedModel = deflate.Decompress(_serviceFactory, CompressionNames.Deflate);

               // Validate the decompressed ETAMP model using the ETAMP validator and the ECDsaSecurityKey
               bool result = await _etampValidator.ValidateETAMPLite(decompressedModel, ecDsaSecurityKey);

               // Return the validation result and the decompressed model as a string
               return $"Result: {result}{Environment.NewLine}" + decompressedModel.ToString();
           }
       }
   }
   ```
**Note:** Once the token within the ETAMP model is encrypted, it cannot be directly verified without first decrypting it. This means that if you choose to encrypt your tokens, you must ensure proper decryption mechanisms are in place for verification purposes.
This example demonstrates the following steps:

1. **Creating an ECDSA Provider**: An ECDSA provider is created using the `IECDsaCreator` service, which generates an Elliptic Curve Digital Signature Algorithm (ECDSA) provider for the specified curve (`nistP521` in this case).

2. **Creating an ECDsaSecurityKey**: An `ECDsaSecurityKey` is created based on the ECDSA provider, which is used for signing and verifying the ETAMP model.

3. **Initializing the Sign Wrapper**: The `ISignWrapper` service is initialized with the ECDSA provider and the SHA512 hash algorithm, which is used for signing the
 provides clear instructions on how to implement and utilize the ETAMP controller, making it accessible for developers to integrate ETAMP into their projects effectively.

4. **Initializing Signing Credentials**: The `ISigningCredentialsProvider` service is initialized with the ECDSA provider, and the security algorithm is set to `EcdsaSha512Signature`, which specifies the algorithm used for digital signatures.

5. **Encryption Setup**: The `IEciesEncryptionService` is initialized with key exchange and encryption services to provide secure token encryption within the ETAMP model.

6. **Key Derivation**: A key is derived using ECDiffieHellman, facilitating secure communications by enabling shared secret derivation.

7. **Model Creation and Processing**: An ETAMP model is created, signed, and encrypted in a fluent manner using builder pattern methods. This sequence simplifies the complex steps involved in ensuring the security of the token.

8. **Compression and Decompression**: The model is compressed using the Deflate algorithm to reduce its size for storage or transmission and then decompressed to retrieve the original data structure.

9. **Validation**: The decompressed ETAMP model is validated using an ETAMP validator service and the ECDSA security key to ensure that it has not been tampered with during the process.

10. **Result Generation**: The result of the validation and the string representation of the decompressed model are returned, providing clear feedback about the operation's success.

This comprehensive example serves as a practical demonstration of how to effectively use the ETAMP library in a real-world application, showcasing the integration of cryptographic techniques to secure and manage data transactions within .NET applications.

## Contributing

Contributions to ETAMP are welcome! Please fork the repository, make your changes, and submit a pull request. We appreciate your input in improving the project.

## License

ETAMP is licensed under the MIT License. For more information, see the [LICENSE](https://github.com/max2020204/ETAMP/blob/master/LICENSE.txt) file in the repository.
