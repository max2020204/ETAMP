#region

using System.Security.Cryptography;

#endregion

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Interface defining methods for handling ECDSA registration.
/// </summary>
public interface IECDSARegistrar
{
    /// <summary>
    ///     Registers an ECDsa instance with a backing store and retrieves its unique identifier
    ///     along with the status of the registration.
    /// </summary>
    /// <param name="ecdsa">The ECDsa cryptographic instance to be registered.</param>
    /// <returns>
    ///     A tuple containing a unique identifier (Guid) for the registered ECDsa and a boolean indicating if the
    ///     registration was successful.
    /// </returns>
    (Guid Id, bool status) Registrar(ECDsa ecdsa);

    /// <summary>
    ///     Registers an ECDsa instance in the system.
    /// </summary>
    /// <param name="ecdsa">The ECDsa cryptographic object to register.</param>
    /// <returns>
    ///     A tuple where the first element is the unique identifier (Guid) of the registered ECDsa instance, and the
    ///     second element is a boolean indicating the success status of the registration.
    /// </returns>
    bool Registrar(ECDsa ecdsa, string name);
}