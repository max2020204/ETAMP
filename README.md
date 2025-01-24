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

Install the required ETAMP libraries using the NuGet Package Manager:

```shell
Install-Package ETAMP.Compression
Install-Package ETAMP.Core
Install-Package ETAMP.Encryption
Install-Package ETAMP.Extension
Install-Package ETAMP.Extension.ServiceCollection
Install-Package ETAMP.Validation
Install-Package ETAMP.Wrapper
```

## Usage

### Prerequisites

Ensure you have the necessary dependencies and services configured in your application. The examples below use
`Microsoft.Extensions.DependencyInjection` for dependency injection.

### Example Files

1. **Program.cs**

   Demonstrates the initialization of services and ETAMP model creation.

   ```csharp
   using ETAMP.Compression.Interfaces.Factory;
   using ETAMP.Core.Models;
   using ETAMP.Extension.ServiceCollection;
   using Microsoft.Extensions.DependencyInjection;

   public static class Program
   {
       public static void Main(string[] args)
       {
           var provider = ConfigureServices();
           var compression = provider.GetService<ICompressionServiceFactory>();
           var etampModel = CreateETAMP.InitializeEtampModel(provider);
           Console.WriteLine(etampModel.Build(compression));
       }

       private static ServiceProvider ConfigureServices()
       {
           var services = new ServiceCollection();
           services.AddETAMPServices();
           return services.BuildServiceProvider();
       }
   }
   ```

2. **CreateETAMP.cs**

   Handles the creation of a basic ETAMP message.

   ```csharp
   using ETAMP.Core.Interfaces;
   using ETAMP.Core.Models;
   using ETAMP.Extension.Builder;

   public static class CreateETAMP
   {
       public static ETAMPModel<TokenModel> InitializeEtampModel(IServiceProvider provider)
       {
           var etampBase = provider.GetService<IETAMPBase>();
           var tokenModel = new TokenModel
           {
               Message = "Hello World!",
               Email = "<EMAIL>",
               Data = "Some data",
               IsEncrypted = false,
               LastName = "Last",
               Name = "Name",
               Phone = "+1234567890"
           };
           return etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip);
       }
   }
   ```

3. **CreateSignETAMP.cs**

   Handles signing of ETAMP messages.

   ```csharp
   using System.Security.Cryptography;
   using ETAMP.Console.CreateETAMP.Models;
   using ETAMP.Core.Models;
   using ETAMP.Encryption.Interfaces.ECDSAManager;
   using ETAMP.Extension.Builder;
   using ETAMP.Wrapper.Interfaces;
   using Microsoft.Extensions.DependencyInjection;
   
   public class CreateSignETAMP
   {
       private static ServiceProvider _provider;
       private static ECDsa _ecdsaInstance;
       public static string PublicKey { get; private set; }
   
       private static void Main(string[] args)
       {
           _ecdsaInstance = ECDsa.Create();
           _provider = CreateETAMP.ConfigureServices();
           var etamp = SignETAMP(_provider).ToJson();
           Console.WriteLine(etamp);
       }
   
       public static ETAMPModel<TokenModel> SignETAMP(IServiceProvider provider)
       {
           var (sign, pemCleaner) = GetServices(provider);
   
           InitializeSigning(sign, pemCleaner);
   
           var etampModel = CreateETAMP.InitializeEtampModel(provider);
           etampModel.Sign(sign);
           return etampModel;
       }
   
       private static (ISignWrapper?, IPemKeyCleaner?) GetServices(IServiceProvider provider)
       {
           return (
               provider.GetService<ISignWrapper>(),
               provider.GetService<IPemKeyCleaner>()
           );
       }
   
       private static void InitializeSigning(ISignWrapper sign, IPemKeyCleaner pemCleaner)
       {
           var publicKeyPem = pemCleaner.ClearPemPublicKey(_ecdsaInstance.ExportSubjectPublicKeyInfoPem());
           PublicKey = publicKeyPem.KeyModelProvider.PublicKey;
   
           sign.Initialize(_ecdsaInstance, HashAlgorithmName.SHA512);
       }
   }
   ```

4. **ValidateETAMP.cs**

   Handles validation of ETAMP messages.

   ```csharp
   using System.Security.Cryptography;
   using ETAMP.Validation.Interfaces;
   using Microsoft.Extensions.DependencyInjection;
   
   internal class ETAMPValidationRunner
   {
       private static readonly HashAlgorithmName DefaultHashAlgorithm = HashAlgorithmName.SHA512;
       private static async Task Main(string[] args)
       {
           var provider = CreateETAMP.ConfigureServices();
           var etampValidator = provider.GetService<IETAMPValidator>();
           
           CreateSignETAMP.Main();
           
           var publicKeyBytes = Convert.FromBase64String(CreateSignETAMP.PublicKey);
           var initializedEcdsa = CreateInitializedEcdsa(publicKeyBytes);
           
           etampValidator.Initialize(initializedEcdsa, DefaultHashAlgorithm);
           var validationResult = await etampValidator.ValidateETAMPAsync(CreateSignETAMP.ETAMP, false);
   
           Console.WriteLine(validationResult.IsValid);
       }
   
       private static ECDsa CreateInitializedEcdsa(byte[] publicKey)
       {
           var ecdsa = ECDsa.Create();
           ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
           return ecdsa;
       }
   }
   ```

## Conclusion

This README provides an updated overview of the ETAMP protocol, including its usage with examples. Follow the structure
to integrate ETAMP into your projects for secure and efficient message handling.

## Contributing

Contributions to ETAMP are welcome! Please fork the repository, make your changes, and submit a pull request. We
appreciate your input in improving the project.

## License

ETAMP is licensed under the MIT License. For more information, see
the [LICENSE](https://github.com/max2020204/ETAMP/blob/master/LICENSE.txt) file in the repository.
