using ETAMP.Compression.Interfaces;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static ICompressionManager _compressionManager;
    private static ETAMPModel<Token> _model;

    private static async Task Main(string[] args)
    {
        ServiceCollection services = new();
        services.AddCompositionServices();
        services.AddBaseServices();
        var provider = services.BuildServiceProvider();
        _compressionManager = provider.GetRequiredService<ICompressionManager>();

        var protocol = provider.GetRequiredService<IETAMPBase>();
        var token = new Token
        {
            Data = "string"
        };
        _model = protocol.CreateETAMPModel("update", token, CompressionNames.Deflate);
        var model = await _compressionManager.CompressAsync(_model);
        var etamp = await _compressionManager.DecompressAsync<Token>(model);
        Console.WriteLine(model);
    }
}