using ETAMPManagement.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPExample;

/// <summary>
///     The main entry point for the application.
/// </summary>
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

    /// <summary>
    ///     Configures the services required for ETAMP management.
    /// </summary>
    /// <returns>A configured <see cref="ServiceProvider" />.</returns>
    private static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddETAMPServices();
        return serviceCollection.BuildServiceProvider();
    }
}