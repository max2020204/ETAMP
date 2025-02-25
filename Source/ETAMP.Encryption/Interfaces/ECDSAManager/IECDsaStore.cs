using System.Security.Cryptography;

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Represents a storage mechanism for managing ECDsa cryptographic providers.
///     This interface defines the contract for adding, retrieving, and removing
///     ECDsa providers using unique identifiers or names.
/// </summary>
public interface IECDsaStore
{
    /// <summary>
    ///     Adds a new ECDSA provider to the store.
    /// </summary>
    /// <param name="id">The unique identifier associated with the ECDSA provider.</param>
    /// <param name="provider">The ECDSA provider to be added to the store.</param>
    /// <returns>
    ///     True if the provider was successfully added; otherwise, false if the identifier already exists in the store.
    /// </returns>
    bool Add(Guid id, ECDsa provider);

    /// <summary>
    ///     Adds a new ECDSA provider to the store with the specified name.
    /// </summary>
    /// <param name="name">The name associated with the ECDSA provider to be added.</param>
    /// <param name="provider">The instance of the ECDSA provider to be added to the store.</param>
    /// <returns>
    ///     Returns true if the provider was successfully added to the store; otherwise, false if a provider with the same name
    ///     already exists.
    /// </returns>
    bool Add(string name, ECDsa provider);

    /// <summary>
    ///     Removes an entry associated with the specified unique identifier from the store.
    /// </summary>
    /// <param name="id">The unique identifier of the entry to remove.</param>
    /// <returns>
    ///     A boolean value indicating the success of the removal operation.
    ///     Returns true if the entry was successfully removed, otherwise false.
    /// </returns>
    bool Remove(Guid id);

    /// <summary>
    ///     Removes an ECDSA provider from the store using its name.
    /// </summary>
    /// <param name="name">The name of the ECDSA provider to be removed.</param>
    /// <returns>True if the provider was successfully removed; otherwise, false.</returns>
    bool Remove(string name);

    /// <summary>
    ///     Retrieves an <see cref="ECDsa" /> instance associated with the specified unique identifier.
    /// </summary>
    /// <param name="id">The GUID representing the unique identifier of the <see cref="ECDsa" /> to retrieve.</param>
    /// <returns>
    ///     The <see cref="ECDsa" /> instance if found; otherwise, null if no matching instance exists in the store.
    /// </returns>
    ECDsa? Get(Guid id);

    /// <summary>
    ///     Retrieves an ECDSA provider by its name.
    /// </summary>
    /// <param name="name">The name of the ECDSA provider to retrieve.</param>
    /// <returns>
    ///     An instance of <see cref="ECDsa" /> associated with the provided name,
    ///     or null if no provider is found.
    /// </returns>
    ECDsa? Get(string name);
}