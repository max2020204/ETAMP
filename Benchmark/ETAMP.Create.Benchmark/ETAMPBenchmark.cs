#region

using BenchmarkDotNet.Attributes;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

#endregion

namespace ETAMP.Create.Benchmark;

[MemoryDiagnoser]
public class ETAMPBenchmark
{
    private ICompressionServiceFactory _compressionServiceFactory;
    private IETAMPBase _etampBase;
    private IServiceProvider _provider;

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
        serviceCollection.AddCompositionServices(false);
        _provider = serviceCollection.BuildServiceProvider();
        _etampBase = _provider.GetService<IETAMPBase>();
        _compressionServiceFactory = _provider.GetService<ICompressionServiceFactory>();

        if (_etampBase == null)
        {
            throw new InvalidOperationException("Failed to resolve IETAMPBase");
        }

        if (_compressionServiceFactory == null)
        {
            throw new InvalidOperationException("Failed to resolve ICompressionServiceFactory");
        }
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

        await _etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip)
            .ToJsonAsync();
    }

    [Benchmark]
    public async Task CreateETAMPBuild()
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

        await _etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip)
            .BuildAsync(_compressionServiceFactory);
    }

    [Benchmark]
    public async Task CreateETAMP_LargeData()
    {
        var largeData = new string('A', 10_000); // Строка размером 10KB
        var tokenModel = new TokenModel
        {
            Message = largeData,
            Email = "<EMAIL>",
            Data = largeData,
            IsEncrypted = false,
            LastName = "Last",
            Name = "Name",
            Phone = "+1234567890"
        };

        await _etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip)
            .ToJsonAsync();
    }

    [Benchmark]
    public async Task CreateETAMPBuild_LargeData()
    {
        var largeData = new string('A', 10_000); // Строка размером 10KB
        var tokenModel = new TokenModel
        {
            Message = largeData,
            Email = "<EMAIL>",
            Data = largeData,
            IsEncrypted = false,
            LastName = "Last",
            Name = "Name",
            Phone = "+1234567890"
        };

        await _etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip)
            .BuildAsync(_compressionServiceFactory);
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