using System.Security.Cryptography;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;

namespace ETAMPManagment.Wrapper.Interfaces;

/// <summary>
///     Verifies signatures using ECDsa.
/// </summary>
public interface IVerifyWrapper : IDisposable
{
    /// Initializes the verifier with an ECDsa instance and a hash algorithm.
    /// This method should be called before performing any verification operations.
    /// </summary>
    /// <param name="ecdsaProvider">The provider for obtaining the ECDsa instance.</param>
    /// <param name="algorithm">The hash algorithm to use for signature verification.</param>
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