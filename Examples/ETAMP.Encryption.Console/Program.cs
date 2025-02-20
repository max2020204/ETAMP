using ETAMP.Core.Interfaces;
using ETAMP.Encryption.Interfaces;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        ServiceCollection services = new();
        services.AddEncryptionServices();
        services.AddBaseServices();
        var provider = services.BuildServiceProvider();

        var protocol = provider.GetRequiredService<IETAMPBase>();
        var aes = provider.GetRequiredService<IEncryptionService>();
        var ecies = provider.GetRequiredService<IECIESEncryptionService>();
    }
}