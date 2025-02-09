using ETAMP.Compression.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Compress.Console;

internal class Program
{
    private static ICompressionManager _compressionManager;
    private static ETAMPModel<Token> _model;

    private static async Task Main(string[] args)
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

        await _compressionManager.CompressAsync(_model);
    }
}