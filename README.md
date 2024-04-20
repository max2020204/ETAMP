
# ETAMP (Encrypted Token And Message Protocol)
## [NuGet Package](https://www.nuget.org/packages/ETAMP/)

ETAMP (Encrypted Token And Message Protocol) is a versatile .NET library that enhances the security of message and transaction handling in applications. It employs advanced cryptographic methods, including JWT (JSON Web Tokens), ECDSA (Elliptic Curve Digital Signature Algorithm), and ECDH (Elliptic Curve Diffie-Hellman) encryption, ensuring secure and reliable data exchange.

## Features

- Secure JWT creation and verification with ECDSA.
- Optional ECDH encryption for enhanced token security.
- Robust ECDSA digital signature for message integrity verification.
- Flexible integration options with both DI and manual configurations.
- Performance-optimized for high throughput and scalability.
- Comprehensive builder pattern support for creating different ETAMP models using `ETAMPBuilder`.

## Installation

To install ETAMP, use the NuGet Package Manager:

```shell
Install-Package ETAMP
```

## Usage

ETAMP supports both Dependency Injection (DI) for seamless integration in .NET projects and manual configuration for customized setup.

### Dependency Injection (DI) Setup

1. **Register ETAMP Services in Startup:**

   In your application's startup class, register ETAMP services to the services collection:

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddETAMPServices();
       // Additional service registrations
   }
   ```

2. **Use ETAMP in Your Application:**

   Inject `IETAMPBuilder<string>` into your classes to create ETAMP instances:

   ```csharp
   public class MyService
   {
       private readonly IETAMPBuilder<string> _etampBuilder;

       public MyService(IETAMPBuilder<string> etampBuilder)
       {
           _etampBuilder = etampBuilder;
       }

       public ETAMPModel CreateEtamp(string type, string updateType, BasePayload payload, double version = 1)
       {
           return _etampBuilder.CreateETAMP(type, updateType, payload, version).Build();
       }
   }
   ```

Here's a refined and clearer version of the "Manual Factory Setup" section for your README:

---

### Manual Factory Setup

This section describes how to manually set up and use the ETAMPFactory for generating ETAMP models, providing a flexible approach to handle dependencies and configurations explicitly.

1. **Instantiate ETAMPFactory:**

   Begin by manually creating an instance of `ETAMPFactory`. Register all necessary generators based on your application's requirements, ensuring each generator is tailored to handle specific types of ETAMP models.

   ```csharp
   var etampFactory = new ETAMPFactory();
   etampFactory.RegisterGenerator(ETAMPTypeNames.Base, () => new ETAMPBase(/* dependencies */));
   // Register additional generators as needed for other ETAMP types
   ```

2. **Build ETAMP Instances:**

   Utilize `ETAMPBuilder` in conjunction with the manually instantiated factory to create ETAMP objects. This allows for explicit control over the creation process and dependencies.

   ```csharp
   var etampBuilder = new ETAMPBuilder(etampFactory);
   var payload = new BasePayload();
   var etamp = etampBuilder.CreateETAMP(ETAMPTypeNames.Base, "update", payload).Build();
   ```

Alternatively, you can directly use `ETAMPBuilder` with a service provider to streamline the instantiation and configuration of ETAMP components:

1. **Instantiate ETAMPBuilder:**
   
   Configure `ETAMPBuilder` using Dependency Injection to seamlessly integrate it within your .NET project's service architecture.

   ```csharp
   var serviceProvider = new ServiceCollection()
       .AddETAMPServices()
       .BuildServiceProvider();
   
   var etampBuilder = serviceProvider.GetRequiredService<IETAMPBuilder<string>>();
   ```

2. **Build ETAMP Instances:**
   
   Directly create and configure ETAMP models using the `ETAMPBuilder`. This method simplifies the instantiation process, leveraging the configured services.

   ```csharp
   var payload = new BasePayload();
   var etamp = etampBuilder.CreateETAMP("Encrypted", "update", payload, 1).Build();
   ```

These steps ensure that both manual and automated configurations are covered, giving developers flexibility according to their setup preferences and requirements.

## Contributing

We welcome contributions to ETAMP! To contribute, please fork the repository, make your changes, and submit a pull request.

## License

ETAMP is licensed under the MIT License. See the [LICENSE](https://github.com/max2020204/ETAMP/blob/master/LICENSE.txt) file for more information.
