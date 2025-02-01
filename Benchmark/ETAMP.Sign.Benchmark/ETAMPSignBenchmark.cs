#region

using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Extension.ServiceCollection;
using ETAMP.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

#endregion

namespace ETAMP.Sign.Benchmark;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[GcServer]
//[HardwareCounters(HardwareCounter.CacheMisses, HardwareCounter.BranchMispredictions)]
public class ETAMPSignBenchmark
{
    private ICompressionServiceFactory? _compressionServiceFactory;
    private IETAMPBase _etampBase;
    private IServiceProvider _provider;
    private ISignWrapper _signWrapper;

    [GlobalSetup]
    public void Setup()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.Async(a => a.Console(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
            .CreateLogger();

        serviceCollection.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
        });

        serviceCollection.AddBaseServices(false);
        serviceCollection.AddWrapperServices(false);
        serviceCollection.AddCompositionServices(false);
        _provider = serviceCollection.BuildServiceProvider();
        _etampBase = _provider.GetService<IETAMPBase>();
        _compressionServiceFactory = _provider.GetService<ICompressionServiceFactory>();
        _signWrapper = _provider.GetService<ISignWrapper>();
        var ecDsa = ECDsa.Create();
        _signWrapper.Initialize(ecDsa, HashAlgorithmName.SHA3_256);
    }

    [Benchmark]
    public async Task CreateETAMP()
    {
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

        var signModel = await _etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.Deflate)
            .Sign(_signWrapper);

        await signModel.BuildAsync(_compressionServiceFactory);
    }


    [Benchmark]
    public async Task CreateETAMP_LargeData()
    {
        var largeData = new string('A', 10_000); // Строка размером 10KB
        var tokenModel = new TokenModel
        {
            Email = "<EMAIL>",
            Data = largeData,
            IsEncrypted = false,
            LastName = "Last",
            Name = "Name",
            Phone = "+1234567890"
        };

        var singModel = await _etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.Deflate)
            .Sign(_signWrapper);
        await singModel.BuildAsync(_compressionServiceFactory);
    }
}

public class TokenModel : Token
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Message { get; set; }
}