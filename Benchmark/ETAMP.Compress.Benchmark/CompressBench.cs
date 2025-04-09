using BenchmarkDotNet.Attributes;
using ETAMP.Compression.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Compress.Benchmark;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[GcServer]
//[HardwareCounters(HardwareCounter.CacheMisses, HardwareCounter.BranchMispredictions)]
public class CompressBench
{
    private ETAMPModelBuilder _builder;
    private ICompressionManager _compressionManager;
    private ETAMPModel<Token> _model;

    [GlobalSetup]
    public async Task Setup()
    {
        ServiceCollection services = new();
        services.AddCompositionServices();
        var provider = services.BuildServiceProvider();
        _compressionManager = provider.GetRequiredService<ICompressionManager>();
        _model = new ETAMPModel<Token>
        {
            Version = 2,
            CompressionType = CompressionNames.Deflate,
            Token = new Token
            {
                Data = "string"
            }
        };
        _builder = await _compressionManager.CompressAsync(_model);
    }

    [Benchmark]
    public async Task Compress()
    {
        await _compressionManager.CompressAsync(_model);
    }

    [Benchmark]
    public async Task Decompress()
    {
        await _compressionManager.DecompressAsync<Token>(_builder);
    }
}