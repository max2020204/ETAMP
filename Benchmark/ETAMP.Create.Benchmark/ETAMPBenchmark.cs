#region

using BenchmarkDotNet.Attributes;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace ETAMP.Create.Benchmark;

[MemoryDiagnoser]
public class ETAMPBenchmark
{
    private IETAMPBase _etampBase;
    private IServiceProvider _provider;

    [GlobalSetup]
    public void Setup()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddETAMPServices();
        _provider = serviceCollection.BuildServiceProvider();
        _etampBase = _provider.GetService<IETAMPBase>();
    }

    [Benchmark]
    public void CreateETAMP()
    {
        var tokenModel = new TokenModel(_provider.GetService<ILogger<Token>>())
        {
            Message = "Hello World!",
            Email = "<EMAIL>",
            Data = "Some data",
            IsEncrypted = false,
            LastName = "Last",
            Name = "Name",
            Phone = "+1234567890"
        };

        _etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip);
    }
}

public class TokenModel : Token
{
    public TokenModel(ILogger<Token>? logger) : base(logger)
    {
    }

    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Message { get; set; }
}