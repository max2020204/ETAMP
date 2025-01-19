namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Defines methods for managing ECDSA providers.
///     Provides functionality to remove ECDSA providers using unique identifiers or names.
/// </summary>
public interface IECDSAControl
{
    /// <summary>
    ///     Removes an entry associated with the specified identifier from the underlying store.
    /// </summary>
    /// <param name="id">The unique identifier of the entry to be removed.</param>
    /// <returns>True if the entry was successfully removed; otherwise, false.</returns>
    bool Remove(Guid id);

    /// <summary>
    ///     Removes a key or entry associated with the provided name.
    /// </summary>
    /// <param name="name">The name associated with the key or entry to be removed.</param>
    /// <returns>
    ///     Returns true if the key or entry was successfully removed; otherwise, false.
    /// </returns>
    bool Remove(string name);
}