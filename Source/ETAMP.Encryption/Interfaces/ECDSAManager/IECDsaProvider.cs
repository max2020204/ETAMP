#region

using System.Security.Cryptography;

#endregion

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Provides an interface for managing and accessing an ECDsa instance for cryptographic operations.
/// </summary>
public interface IECDsaProvider
{
    ECDsa GetECDsa(Guid id);
    ECDsa GetECDsa(string name);
}