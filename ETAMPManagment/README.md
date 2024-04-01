# ETAMP (Encrypted Token And Message Protocol)
## [NuGet Package](https://www.nuget.org/packages/ETAMP/)

ETAMP (Encrypted Token And Message Protocol) is a versatile .NET library that enhances the security of message and transaction handling in applications. It employs advanced cryptographic methods, including JWT (JSON Web Tokens), ECDSA (Elliptic Curve Digital Signature Algorithm), and ECDH (Elliptic Curve Diffie-Hellman) encryption, ensuring secure and reliable data exchange.

## Features

- Secure JWT creation and verification with ECDSA.
- Optional ECDH encryption for enhanced token security.
- Robust ECDSA digital signature for message integrity verification.
- Flexible integration options with both DI and manual configurations.
- Performance-optimized for high throughput and scalability.

## Installation

To install ETAMP, use the NuGet Package Manager:

```shell
Install-Package ETAMP
```

## Usage

ETAMP supports both Dependency Injection (DI) for seamless integration in .NET projects and manual configuration for customized setup.

### Dependency Injection (DI) Setup

1. **Register ETAMP Services in Startup:**

   In your application’s startup class, register ETAMP services to the services collection:

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddETAMPServices();
       // Additional service registrations
   }
   ```

2. **Use ETAMP in Your Application:**

   Inject `IETAMPBuilder<ETAMPType>` into your classes to create ETAMP instances:

   ```csharp
   public class MyService
   {
       private readonly IETAMPBuilder<ETAMPType> _etampBuilder;

       public MyService(IETAMPBuilder<ETAMPType> etampBuilder)
       {
           _etampBuilder = etampBuilder;
       }

       public ETAMPModel CreateEtamp()
       {
           var payload = new BasePayload();
           return _etampBuilder.CreateETAMP(ETAMPType.Base, "update", payload).Build();
       }
   }
   ```

### Manual Factory Setup

1. **Instantiate ETAMPFactory:**

   Manually create an instance of `ETAMPFactory` and register the necessary generators:

   ```csharp
   var etampFactory = new ETAMPFactory();
   etampFactory.RegisterGenerator(ETAMPType.Base, () => new ETAMPBase(/* dependencies */));
   // Register additional generators as needed
   ```

2. **Build ETAMP Instances:**

   Use `ETAMPBuilder` with the factory to create ETAMP objects:

   ```csharp
   var etampBuilder = new ETAMPBuilder(etampFactory);
   var payload = new BasePayload();
   var etamp = etampBuilder.CreateETAMP(ETAMPType.Base, "update", payload).Build();
   ```

### Service Registration in ETAMPFactory

To dynamically manage ETAMP creation strategies, register services and generators within the `ETAMPFactory`:

```csharp
var etampFactory = new ETAMPFactory();
etampFactory.RegisterGenerator(ETAMPType.Base, () => new ETAMPBase(/* dependencies */));
etampFactory.RegisterGenerator(ETAMPType.Sign, () => new ETAMPSign(/* dependencies */));
etampFactory.RegisterGenerator(ETAMPType.Encrypted, () => new ETAMPEncrypted(/* dependencies */));
etampFactory.RegisterGenerator(ETAMPType.EncryptedSign, () => new ETAMPEncryptedSigned(/* dependencies */));
```

This approach allows for flexibility and scalability in generating different types of ETAMP objects based on the application's needs.

## Contributing

We welcome contributions to ETAMP! To contribute, please fork the repository, make your changes, and submit a pull request.

## License

ETAMP is licensed under the MIT License. See the [LICENSE](https://github.com/max2020204/ETAMP/blob/master/LICENSE.txt) file for more information.