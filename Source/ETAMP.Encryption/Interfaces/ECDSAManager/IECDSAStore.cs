#region

using ETAMP.Encryption.Base;

#endregion

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Represents an interface for storing and managing instances of ECDsaProviderBase.
///     Provides methods to add, remove, and retrieve ECDsa providers based on both GUID and string identifiers.
/// </summary>
public interface IECDSAStore
{
    /// <summary>
    ///     Adds a new entry to the ECDSA store with the specified identifier and cryptographic provider.
    /// </summary>
    /// <param name="id">The unique identifier associated with the ECDSA provider to be added.</param>
    /// <param name="provider">The ECDSA cryptographic provider instance to be added to the store.</param>
    /// <returns>
    ///     Returns true if the entry was successfully added to the store; otherwise, false.
    /// </returns>
    bool Add(Guid id, ECDSAProviderBase provider);

    /// <summary>
    ///     Adds a new ECDSA provider to the store using a specified name as the key.
    /// </summary>
    /// <param name="name">The name key used to identify and store the ECDSA provider.</param>
    /// <param name="provider">The ECDSA provider instance to be added to the store.</param>
    /// <returns>Returns true if the provider was successfully added to the store; otherwise, false.</returns>
    bool Add(string name, ECDSAProviderBase provider);

    /// <summary>
    ///     Removes an ECDsa provider from the store identified by the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the ECDsa provider to remove.</param>
    /// <returns>
    ///     Returns true if the ECDsa provider was successfully removed; otherwise, false.
    /// </returns>
    bool Remove(Guid id);

    /// <summary>
    ///     Removes an ECDsa provider from the store using the specified name.
    /// </summary>
    /// <param name="name">The name used to identify the ECDsa provider to be removed.</param>
    /// <returns>
    ///     A boolean value indicating whether the removal was successful.
    ///     Returns true if the provider was successfully removed; otherwise, false.
    /// </returns>
    bool Remove(string name);

    /// <summary>
    ///     Retrieves the ECDsaProviderBase instance associated with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the ECDsa provider to retrieve.</param>
    /// <returns>An instance of <see cref="ECDSAProviderBase" /> if found; otherwise, null.</returns>
    ECDSAProviderBase? Get(Guid id);

    /// <summary>
    ///     Retrieves the ECDsaProviderBase instance associated with the specified name.
    /// </summary>
    /// <param name="name">The unique name identifier for the ECDsaProviderBase instance to retrieve.</param>
    /// <returns>The ECDsaProviderBase instance if found; otherwise, null.</returns>
    ECDSAProviderBase? Get(string name);
}