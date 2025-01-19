#region

using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

/// <summary>
///     The ECDSAStore class provides a storage mechanism for managing ECDsaProviderBase instances.
///     This class allows adding, retrieving, and removing ECDSA providers using either a Guid or a string as the key.
///     Implements the IECDSAStore interface.
/// </summary>
public class ECDSAStore : IECDSAStore
{
    /// <summary>
    ///     A private dictionary field that stores mappings between GUIDs and instances of ECDsaProviderBase.
    ///     Used to manage and retrieve ECDSA cryptographic providers based on unique GUID identifiers.
    /// </summary>
    private readonly Dictionary<Guid, ECDSAProviderBase> _guidStore;

    /// <summary>
    ///     A private dictionary used to store ECDsaProviderBase instances, where the key is a string identifier.
    ///     Enables searching, adding, and removing providers based on their designated string names.
    ///     Primarily used for efficiently managing and accessing cryptographic providers by name.
    /// </summary>
    private readonly Dictionary<string, ECDSAProviderBase> _nameStore;

    /// <summary>
    ///     The ECDSAStore class provides an implementation for managing elliptic curve digital signature algorithm (ECDSA)
    ///     providers.
    /// </summary>
    /// <remarks>
    ///     This class maintains internal storage for ECDSA providers organized by unique identifiers (GUIDs) and names.
    ///     It supports adding, removing, and retrieving ECDSA providers from the store.
    /// </remarks>
    public ECDSAStore()
    {
        _guidStore = new Dictionary<Guid, ECDSAProviderBase>();
        _nameStore = new Dictionary<string, ECDSAProviderBase>();
    }

    /// <summary>
    ///     Adds a new ECDSA provider to the store using a unique identifier.
    /// </summary>
    /// <param name="id">A globally unique identifier (GUID) used to identify the ECDSA provider.</param>
    /// <param name="provider">
    ///     The ECDSA provider to be added to the store. Must be an instance of
    ///     <see cref="ECDSAProviderBase" />.
    /// </param>
    /// <returns>
    ///     A boolean value indicating whether the addition was successful.
    ///     Returns <c>true</c> if the provider is successfully added; otherwise, <c>false</c>.
    /// </returns>
    public bool Add(Guid id, ECDSAProviderBase provider)
    {
        return _guidStore.TryAdd(id, provider);
    }

    /// <summary>
    ///     Adds a new ECDsa provider to the store associated with the specified name.
    /// </summary>
    /// <param name="name">The unique name associated with the ECDsa provider to be added.</param>
    /// <param name="provider">The ECDsaProviderBase instance to be added to the store.</param>
    /// <returns>
    ///     A boolean value indicating whether the ECDsa provider was successfully added to the store.
    ///     Returns true if the addition was successful, or false if a provider with the same name already exists.
    /// </returns>
    public bool Add(string name, ECDSAProviderBase provider)
    {
        return _nameStore.TryAdd(name, provider);
    }

    /// <summary>
    ///     Removes an ECDsa provider from the store using its associated <see cref="Guid" /> identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ECDsa provider to be removed.</param>
    /// <returns>
    ///     True if the provider was successfully removed; otherwise, false.
    /// </returns>
    public bool Remove(Guid id)
    {
        return _guidStore.Remove(id);
    }

    /// <summary>
    ///     Removes an ECDsa provider from the store using the specified name as the key.
    /// </summary>
    /// <param name="name">The name of the ECDsa provider to be removed.</param>
    /// <returns>
    ///     Returns <c>true</c> if the provider with the specified name was successfully removed;
    ///     otherwise, <c>false</c>, indicating that the provider was not found in the store.
    /// </returns>
    public bool Remove(string name)
    {
        return _nameStore.Remove(name);
    }

    /// <summary>
    ///     Retrieves the ECDsa provider associated with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ECDsa provider to retrieve.</param>
    /// <returns>
    ///     The <see cref="ECDSAProviderBase" /> instance associated with the specified identifier,
    ///     or <c>null</c> if no provider is found.
    /// </returns>
    public ECDSAProviderBase? Get(Guid id)
    {
        return _guidStore.TryGetValue(id, out var provider) ? provider : null;
    }

    /// <summary>
    ///     Retrieves an ECDsaProviderBase instance associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the ECDSA provider to retrieve.</param>
    /// <returns>
    ///     An instance of <see cref="ECDSAProviderBase" /> if the provider with the given name exists;
    ///     otherwise, null.
    /// </returns>
    public ECDSAProviderBase? Get(string name)
    {
        return _nameStore.TryGetValue(name, out var provider) ? provider : null;
    }
}