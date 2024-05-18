#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;

#endregion

namespace ETAMPManagement.Wrapper.Interfaces;

/// <summary>
///     Verifies signatures using ECDsa.
/// </summary>
public interface IVerifyWrapper : IDisposable
{
    /// <summary>
    ///     Initializes the <see cref="VerifyWrapper" /> with an ECDsa instance and a hash algorithm.
    ///     This method should be called before performing any verification operations.
    /// </summary>
    /// <param name="ecdsaProvider">The provider to obtain the ECDsa instance.</param>
    /// <param name="algorithm">The hashing algorithm for verification.</param>
    void Initialize(IECDsaProvider ecdsaProvider, HashAlgorithmName algorithm);

    /// <summary>
    ///     Verifies the signature of string data.
    /// </summary>
    /// <param name="data">The data to verify.</param>
    /// <param name="signature">The signature to check against.</param>
    /// <returns>True if valid; otherwise, false.</returns>
    bool VerifyData(string data, string signature);

    /// <summary>
    ///     Verifies the signature of byte array data.
    /// </summary>
    /// <param name="data">The data to verify.</param>
    /// <param name="signature">The signature to check against.</param>
    /// <returns>True if valid; otherwise, false.</returns>
    bool VerifyData(byte[] data, byte[] signature);
}