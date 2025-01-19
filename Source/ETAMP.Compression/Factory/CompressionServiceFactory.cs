#region

using System.Collections.Concurrent;
using ETAMP.Compression.Codec;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Management;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ETAMP.Compression.Factory;

/// <summary>
///     Factory for creating compression service instances based on the specified compression type.
///     Utilizes the service provider to resolve the requested compression service dynamically at runtime.
/// </summary>
public sealed class CompressionServiceFactory : ICompressionServiceFactory
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CompressionServiceFactory" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependency injection services.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="serviceProvider" /> is null.</exception>
    public CompressionServiceFactory(IServiceProvider serviceProvider)
    {
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
        if (Factory.TryGetValue(compressionType, out var serviceFactory))
            return serviceFactory;

        throw new KeyNotFoundException($"ETAMPBuilder service '{compressionType}' not recognized.");
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(compressionType, nameof(compressionType));

        return Factory.TryRemove(compressionType, out _);
    }
}