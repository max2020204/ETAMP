# ETAMP (Encrypted Token And Message Protocol)

## Overview

ETAMP (Encrypted Token And Message Protocol) is designed for secure and efficient message transmission in a
semi-decentralized network. The protocol ensures message integrity and supports encryption and signing using ECC (
Elliptic Curve Cryptography).

## Architecture

The protocol includes the following fields:

- `GUID Id`
- `double Version`
- `string Token` (compressed JSON)
- `string UpdateType`
- `string SignatureMessage`
- `string CompressionType`

### Token Structure

The token structure includes the following fields:

- `Guid Id`
- `Guid MessageId`
- `bool IsEncrypted`
- `string Data`
- `DateTimeOffset TimeStamp`

## Installation

Install ETAMP using the NuGet Package Manager:

```shell
Install-Package ETAMP
```

## Usage

### Prerequisites

Ensure you have the necessary dependencies and services configured in your application. The examples below
use `Microsoft.Extensions.DependencyInjection` for dependency injection.

### Example Files

1. **Program.cs**

   This file serves as the entry point of the application, demonstrating the creation, signing, encryption, and
   validation of ETAMP messages.

   ```csharp
   // Program.cs
   using ETAMPManagement.Extensions;
   using Microsoft.Extensions.DependencyInjection;

   namespace ETAMPExample
   {
       internal static class Program
       {
           private static void Main()
           {
               var provider = ConfigureServices();
               var createETAMP = new CreateETAMP(provider);
               var createSignETAMP = new CreateSignETAMP(provider);
               var validateETAMP = new ValidateETAMP(provider);
               var encrypted = new CreateEncryptedETAMP(provider);

               Console.WriteLine("Create default ETAMP: \n" + createETAMP.CreateETAMPMessage() + "\n");
               Console.WriteLine("Create ETAMP with sign: \n" + createSignETAMP.CreateAndSignETAMPMessage() + "\n");
               Console.WriteLine("Create encrypted ETAMP: \n" + encrypted.CreateEncryptedETAMPMessage() + "\n");
               Console.WriteLine("Validate ETAMP: \n" + validateETAMP.Validate());
           }

           private static ServiceProvider ConfigureServices()
           {
               var serviceCollection = new ServiceCollection();
               serviceCollection.AddETAMPServices();
               return serviceCollection.BuildServiceProvider();
           }
       }
   }
   ```

2. **CreateETAMP.cs**

   This class is responsible for creating a basic ETAMP message.

   ```csharp
   // CreateETAMP.cs
   using ETAMPManagement.ETAMP.Interfaces;
   using ETAMPManagement.Extensions.Builder;
   using ETAMPManagement.Factory.Interfaces;
   using ETAMPManagement.Models;
   using Microsoft.Extensions.DependencyInjection;

   namespace ETAMPExample
   {
       public class CreateETAMP
       {
           private readonly ServiceProvider _provider;

           public CreateETAMP(ServiceProvider provider)
           {
               _provider = provider;
           }

           public string CreateETAMPMessage()
           {
               var etampBase = _provider.GetService<IETAMPBase>();
               var compression = _provider.GetService<ICompressionServiceFactory>();
               var token = CreateToken("SomeDataInJson");
               var etamp = etampBase.CreateETAMPModel("Message", token, CompressionNames.Deflate);

               return etamp.Build(compression);
           }

           private Token CreateToken(string data)
           {
               return new Token { Data = data };
           }
       }
   }
   ```

3. **CreateSignETAMP.cs**

   This class handles the creation and signing of ETAMP messages.

   ```csharp
   // CreateSignETAMP.cs
   using System.Security.Cryptography;
   using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
   using ETAMPManagement.ETAMP.Interfaces;
   using ETAMPManagement.Extensions.Builder;
   using ETAMPManagement.Factory.Interfaces;
   using ETAMPManagement.Models;
   using Microsoft.Extensions.DependencyInjection;

   namespace ETAMPExample
   {
       public class CreateSignETAMP
       {
           private readonly ServiceProvider _provider;

           public CreateSignETAMP(ServiceProvider provider)
           {
               _provider = provider;
           }

           public IECDsaProvider ECDsaProvider { get; private set; }
           public HashAlgorithmName HashAlgorithm { get; private set; }

           public string CreateAndSignETAMPMessage()
           {
               var etampBase = _provider.GetService<IETAMPBase>();
               var sign = _provider.GetService<SignWrapperBase>();
               var creator = _provider.GetService<IECDsaCreator>();
               var compression = _provider.GetService<ICompressionServiceFactory>();

               InitializeSignature(sign, creator);
               var token = CreateToken("SomeDataInJson");
               var etamp = etampBase.CreateETAMPModel("Message", token, CompressionNames.Deflate);

               return etamp.Sign(sign).Build(compression);
           }

           private void InitializeSignature(SignWrapperBase sign, IECDsaCreator creator)
           {
               ECDsaProvider = creator.CreateECDsa();
               HashAlgorithm = HashAlgorithmName.SHA512;
               sign.Initialize(ECDsaProvider, HashAlgorithm);
           }

           private Token CreateToken(string data)
           {
               return new Token { Data = data };
           }
       }
   }
   ```

4. **CreateEncryptedETAMP.cs**

   This class demonstrates how to create an encrypted ETAMP message.

   ```csharp
   // CreateEncryptedETAMP.cs
   using System.Security.Cryptography;
   using ETAMPManagement.Encryption.Base;
   using ETAMPManagement.Encryption.Interfaces;
   using ETAMPManagement.ETAMP.Interfaces;
   using ETAMPManagement.Extensions.Builder;
   using ETAMPManagement.Factory.Interfaces;
   using ETAMPManagement.Models;
   using Microsoft.Extensions.DependencyInjection;

   namespace ETAMPExample
   {
       public class CreateEncryptedETAMP
       {
           private readonly ServiceProvider _provider;

           public CreateEncryptedETAMP(ServiceProvider provider)
           {
               _provider = provider;
           }

           public string CreateEncryptedETAMPMessage()
           {
               var etampBase = _provider.GetService<IETAMPBase>();
               var ecies = _provider.GetService<ECIESEncryptionServiceBase>();
               var compression = _provider.GetService<ICompressionServiceFactory>();
               var keyExchanger = _provider.GetService<KeyExchangerBase>();
               var keyPairProvider = _provider.GetService<KeyPairProviderBase>();
               var aes = _provider.GetService<IEncryptionService>();

               keyPairProvider.Initialize(ECDiffieHellman.Create());
               keyExchanger.Initialize(keyPairProvider);
               keyExchanger.DeriveKey(ECDiffieHellman.Create().PublicKey);
               ecies.Initialize(keyExchanger, aes);

               var token = CreateToken("SomeDataInJson");
               var etamp = etampBase.CreateETAMPModel("Message", token, CompressionNames.Deflate);

               return etamp.EncryptData(ecies).Build(compression);
           }

           private Token CreateToken(string data)
           {
               return new Token { Data = data };
           }
       }
   }
   ```

5. **ValidateETAMP.cs**

   This class is responsible for validating ETAMP messages.

   ```csharp
   // ValidateETAMP.cs
   using ETAMPManagement.Extensions.Builder;
   using ETAMPManagement.Factory.Interfaces;
   using ETAMPManagement.Models;
   using ETAMPManagement.Validators.Base;
   using Microsoft.Extensions.DependencyInjection;

   namespace ETAMPExample
   {
       public class ValidateETAMP
       {
           private readonly ServiceProvider _provider;

           public ValidateETAMP(ServiceProvider provider)
           {
               _provider = provider;
           }

           public bool Validate()
           {
               var validator = _provider.GetService<ETAMPValidatorBase>();
               var compression = _provider.GetService<ICompressionServiceFactory>();
               var createSignETAMP = new CreateSignETAMP(_provider);

               var etamp = createSignETAMP.CreateAndSignETAMPMessage();
               var model = etamp.DeconstructETAMP<Token>(compression);

               validator?.Initialize(createSignETAMP.ECDsaProvider, createSignETAMP.HashAlgorithm);
               var result = validator?.ValidateETAMP(model, false);

               return result.IsValid;
           }
       }
   }
   ```

## Conclusion

This documentation provides an overview of the ETAMP protocol and its usage with example implementations. Follow the
structure and examples to integrate ETAMP into your projects, ensuring secure and efficient message handling.

## Contributing

Contributions to ETAMP are welcome! Please fork the repository, make your changes, and submit a pull request. We
appreciate your input in improving the project.

## License

ETAMP is licensed under the MIT License. For more information, see
the [LICENSE](https://github.com/max2020204/ETAMP/blob/master/LICENSE.txt) file in the repository.
