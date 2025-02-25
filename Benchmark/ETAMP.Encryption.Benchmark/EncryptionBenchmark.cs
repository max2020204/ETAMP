using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using ETAMP.Encryption.Interfaces;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Encryption.Benchmark;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[GcServer]
public class EncryptionBenchmark
{
    private readonly string _data = "String to encrypt";
    private string _encryptData;
    private IECIESEncryptionManager _encryptionManager;
    private ECDiffieHellman _person1;
    private ECDiffieHellman _person2;

    [GlobalSetup]
    public async Task Setup()
    {
        ServiceCollection services = new();
        services.AddEncryptionServices();
        var provider = services.BuildServiceProvider();

        _person1 = ECDiffieHellman.Create();
        _person2 = ECDiffieHellman.Create();

        _encryptionManager = provider.GetRequiredService<IECIESEncryptionManager>();

        _encryptData = await _encryptionManager.EncryptAsync(_data, _person1, _person2.PublicKey);
    }

    [Benchmark]
    public async Task Encrypt()
    {
        await _encryptionManager.EncryptAsync(_data, _person1, _person2.PublicKey);
    }

    [Benchmark]
    public async Task Decrypt()
    {
        await _encryptionManager.DecryptAsync(_encryptData, _person2, _person1.PublicKey);
    }
}