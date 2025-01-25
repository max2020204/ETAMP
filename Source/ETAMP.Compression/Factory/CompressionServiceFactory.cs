#region

using System.Collections.Concurrent;
using ETAMP.Compression.Codec;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Management;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

#endregion

namespace ETAMP.Compression.Factory;

/// <summary>
///     Factory for creating compression service instances based on the specified compression type.
///     Utilizes the service provider to resolve the requested compression service dynamically at runtime.
/// </summary>
public sealed class CompressionServiceFactory : ICompressionServiceFactory
{
    private readonly ILogger<CompressionServiceFactory> _logger;


    /// <summary>
    /// Provides a factory for managing and creating instances of compression services. This factory supports
    /// registering and retrieving compression services by a specified type.
    /// </summary>
    public CompressionServiceFactory(IServiceProvider serviceProvider,
        ILogger<CompressionServiceFactory>? logger = null)
    {
        _logger = logger ?? NullLogger<CompressionServiceFactory>.Instance;
        Factory = new ConcurrentDictionary<string, ICompressionService>();
        Factory.TryAdd(CompressionNames.Deflate, serviceProvider.GetRequiredService<DeflateCompressionService>());
        Factory.TryAdd(CompressionNames.GZip, serviceProvider.GetRequiredService<GZipCompressionService>());
    }

    /// <summary>
    ///     Gets the dictionary mapping compression types to their corresponding compression service instances.
    /// </summary>
    public ConcurrentDictionary<string, ICompressionService> Factory { get; }

    /// <summary>
    ///     Creates and returns an instance of a compression service based on the specified compression type.
    /// </summary>
    /// <param name="compressionType">
    ///     The type of compression service to create. This should match one of the predefined
    ///     constants in <see cref="CompressionNames" />.
    /// </param>
    /// <returns>An instance of the requested compression service.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the specified compression type is not recognized or supported.</exception>
    public ICompressionService Create(string compressionType)
    {
        _logger.LogDebug("Creating compression service for type '{0}'", compressionType);
        if (!Factory.TryGetValue(compressionType, out var serviceFactory))
        {
            _logger.LogError("ETAMPBuilder service '{0}' not recognized.", compressionType);
            throw new KeyNotFoundException($"ETAMPBuilder service '{compressionType}' not recognized.");
        }

        _logger.LogDebug("Compression service for type '{0}' created successfully", compressionType);
        return serviceFactory;
    }


    /// <summary>
    ///     Registers a new compression service under a specified compression type.
    /// </summary>
    /// <param name="compressionType">The type of compression to register.</param>
    /// <param name="serviceFactory">The compression service instance to be registered.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either <paramref name="compressionType" /> or
    ///     <paramref name="serviceFactory" /> is null.
    /// </exception>
    public void RegisterCompressionService(string compressionType, ICompressionService serviceFactory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(compressionType, nameof(compressionType));
        ArgumentNullException.ThrowIfNull(serviceFactory, nameof(serviceFactory));
        _logger.LogDebug("Registering compression service for type '{0}'", compressionType);
        Factory.TryAdd(compressionType, serviceFactory);
    }

    /// <summary>
    ///     Unregisters an existing compression service for a specified compression type.
    /// </summary>
    /// <param name="compressionType">The type of compression to unregister.</param>
    /// <returns>True if the service was successfully unregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="compressionType" /> is null.</exception>
    public bool UnregisterCompressionService(string compressionType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(compressionType, nameof(compressionType));
        _logger.LogDebug("Unregistering compression service for type '{0}'", compressionType);
        return Factory.TryRemove(compressionType, out _);
    }
}