using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Compression.Factory;

public sealed record CompressionServiceFactory : ICompressionServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CompressionServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICompressionService? Get(string compressionType)
    {
        return _serviceProvider.GetKeyedService<ICompressionService>(compressionType);
    }
}