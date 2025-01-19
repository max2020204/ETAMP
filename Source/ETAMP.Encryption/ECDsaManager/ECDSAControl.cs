#region

using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

/// <summary>
///     Provides functionality to manage ECDSA key stores, including operations for removing ECDSA entries by their
///     identifier or name.
/// </summary>
public class ECDSAControl : IECDSAControl
{
    /// <summary>
    ///     Represents the private storage instance utilized for managing ECDSA providers.
    ///     This field works with an implementation of the <see cref="IECDSAStore" /> interface.
    ///     Facilitates operations such as adding, removing, and retrieving cryptographic providers.
    /// </summary>
    private readonly IECDSAStore _store;

    /// <summary>
    ///     Provides an implementation of the interface <see cref="IECDSAControl" /> for managing ECDSA providers via an
    ///     underlying store.
    ///     Enables the removal of ECDSA providers from the store by both unique GUID identifiers and string names.
    /// </summary>
    public ECDSAControl(IECDSAStore store)
    {
        _store = store;
    }

    /// <summary>
    ///     Removes an entry associated with the specified identifier from the underlying store.
    /// </summary>
    /// <param name="id">The unique identifier of the entry to be removed.</param>
    /// <returns>True if the entry was successfully removed; otherwise, false.</returns>
    public bool Remove(Guid id)
    {
        return _store.Remove(id);
    }

    /// <summary>
    ///     Removes a key or entry associated with the provided name.
    /// </summary>
    /// <param name="name">The name associated with the key or entry to be removed.</param>
    /// <returns>
    ///     Returns true if the key or entry was successfully removed; otherwise, false.
    /// </returns>
    public bool Remove(string name)
    {
        return _store.Remove(name);
    }
}