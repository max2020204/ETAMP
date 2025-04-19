using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Encryption.Console;

internal class Program
{
    private static IServiceProvider _provider;
    private static IECIESEncryptionManager _encryptionManager;

    private static async Task Main(string[] args)
    {
        Setup();

        var data = "String to encrypt";

        var person1 = ECDiffieHellman.Create();
        var person2 = ECDiffieHellman.Create();

        var encrResult = await _encryptionManager.EncryptAsync(data, person1, person2.PublicKey);

        System.Console.WriteLine(encrResult);


        var decrResult = await _encryptionManager.DecryptAsync(encrResult, person2, person1.PublicKey);
        System.Console.WriteLine(decrResult);
    }

    private static void Setup()
    {
        ServiceCollection services = new();
        services.AddEncryptionServices();
        services.AddBaseServices();
        var provider = services.BuildServiceProvider();
        _encryptionManager = provider.GetRequiredService<IECIESEncryptionManager>();
    }
}