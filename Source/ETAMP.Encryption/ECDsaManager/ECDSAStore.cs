using System.Collections.Concurrent;
using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using Microsoft.Extensions.Logging;

namespace ETAMP.Encryption.ECDsaManager;

/// <summary>
///     Manages storage of ECDSA providers, allowing registration, retrieval, and removal
///     by unique identifiers or names.
/// </summary>
public class ECDSAStore : IECDSAStore
{
    /// <summary>
    ///     A concurrent dictionary for storing ECDSAProviderBase instances indexed by their unique Guid identifiers.
    ///     Used internally for managing providers by their associated Guid keys.
    /// </summary>
    private readonly ConcurrentDictionary<Guid, ECDsa> _guidStore;

    private readonly ILogger<ECDSAStore> _logger;

    /// <summary>
    ///     A private dictionary that maps string-based names to instances of <see cref="ECDSAProviderBase" />.
    ///     This collection is used to store and manage ECDSA cryptographic providers by their associated names.
    /// </summary>
    private readonly ConcurrentDictionary<string, ECDsa> _nameStore;

    /// <summary>
    ///     An implementation of the <see cref="IECDSAStore" /> interface.
    ///     Provides functionality to manage and store instances of ECDSA cryptographic providers
    ///     using unique identifiers or string-based names.
    /// </summary>
    public ECDSAStore(ILogger<ECDSAStore> logger)
    {
        _logger = logger;
        _guidStore = new ConcurrentDictionary<Guid, ECDsa>();
        _nameStore = new ConcurrentDictionary<string, ECDsa>();
    }

    /// <summary>
    ///     Adds an ECDSA provider to the store using a unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier associated with the ECDSA provider.</param>
    /// <param name="provider">The ECDSA provider to be added to the store.</param>
    /// <returns>
    ///     True if the provider was successfully added; otherwise, false if the identifier
    ///     already exists in the store.
    /// </returns>
    public bool Add(Guid id, ECDsa provider)
    {
        _logger.LogDebug($"Adding ECDSA provider with id {id}");
        return _guidStore.TryAdd(id, provider);
    }

    /// <summary>
    ///     Adds a new ECDSA provider to the store with the specified name.
    /// </summary>
    /// <param name="name">The name associated with the ECDSA provider to be added.</param>
    /// <param name="provider">The instance of the ECDSA provider to be added to the store.</param>
    /// <returns>
    ///     Returns true if the provider was successfully added to the store; otherwise, false.
    /// </returns>
    public bool Add(string name, ECDsa provider)
    {
        _logger.LogDebug($"Adding ECDSA provider with name {name}");
        return _nameStore.TryAdd(name, provider);
    }

    /// <summary>
    ///     Removes an ECDSA provider associated with the specified unique identifier from the store.
    /// </summary>
    /// <param name="id">The unique identifier of the ECDSA provider to be removed.</param>
    /// <returns>
    ///     A boolean value indicating whether the removal was successful.
    ///     Returns true if the provider was successfully removed; otherwise, false.
    /// </returns>
    public bool Remove(Guid id)
    {
        _logger.LogDebug($"Removing ECDSA provider with id {id}");
        return _guidStore.TryRemove(id, out _);
    }

    /// <summary>
    ///     Removes an ECDSA provider from the store using its name.
    /// </summary>
    /// <param name="name">The name of the ECDSA provider to be removed.</param>
    /// <returns>True if the provider was successfully removed; otherwise, false.</returns>
    public bool Remove(string name)
    {
        _logger.LogDebug($"Removing ECDSA provider with name {name}");
        return _nameStore.TryRemove(name, out _);
    }

    /// <summary>
    ///     Retrieves an instance of <see cref="ECDSAProviderBase" /> for the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="ECDSAProviderBase" /> to retrieve.</param>
    /// <returns>
    ///     The <see cref="ECDSAProviderBase" /> instance associated with the specified unique identifier,
    ///     or null if no matching instance exists in the store.
    /// </returns>
    public ECDsa? Get(Guid id)
    {
        _logger.LogDebug($"Retrieving ECDSA provider with id {id}");
        return _guidStore.GetValueOrDefault(id);
    }

    /// <summary>
    ///     Retrieves an ECDSA provider by its name.
    /// </summary>
    /// <param name="name">The name of the ECDSA provider to retrieve.</param>
    /// <returns>
    ///     The <see cref="ECDSAProviderBase" /> instance associated with the provided name,
    ///     or null if no provider is found.
    /// </returns>
    public ECDsa? Get(string name)
    {
        _logger.LogDebug($"Retrieving ECDSA provider with name {name}");
        return _nameStore.GetValueOrDefault(name);
    }
}