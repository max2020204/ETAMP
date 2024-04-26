using System.Security.Cryptography;
using System.Text;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;

namespace ETAMPManagment.Wrapper;

/// <summary>
///     Provides cryptographic verification using ECDSA, supporting both string and byte array data formats.
/// </summary>
/// >
public sealed class VerifyWrapper : IVerifyWrapper
{
    private HashAlgorithmName _algorithm;
    private ECDsa? _ecdsa;

    /// <summary>
    ///     Initializes the <see cref="VerifyWrapper" /> with an ECDsa instance and a hash algorithm.
    ///     This method should be called before performing any verification operations.
    /// </summary>
    /// <param name="ecdsaProvider">Provider to obtain the ECDsa instance.</param>
    /// <param name="algorithm">Hashing algorithm for verification.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="ecdsaProvider" /> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if ECDsa instance cannot be obtained from the provider.</exception>
    public void Initialize(IECDsaProvider ecdsaProvider, HashAlgorithmName algorithm)
    {
        if (ecdsaProvider == null)
            throw new ArgumentNullException(nameof(ecdsaProvider), "IECDsaProvider instance cannot be null.");

        _ecdsa = ecdsaProvider.GetECDsa() ??
                 throw new InvalidOperationException(
                     "ECDsa instance cannot be null after extraction from IECDsaProvider.");
        _algorithm = algorithm;
    }

    /// <summary>
    ///     Verifies the signature of string data.
    /// </summary>
    /// <param name="data">Data to verify, in string format.</param>
    /// <param name="signature">Base64-encoded signature to verify against.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public bool VerifyData(string data, string signature)
    {
        return _ecdsa!.VerifyData(Encoding.UTF8.GetBytes(data), Convert.FromBase64String(signature), _algorithm);
    }

    /// <summary>
    ///     Verifies the signature of byte array data.
    /// </summary>
    /// <param name="data">Data to verify, as a byte array.</param>
    /// <param name="signature">Signature to verify against, as a byte array.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public bool VerifyData(byte[] data, byte[] signature)
    {
        return _ecdsa!.VerifyData(data, signature, _algorithm);
    }

    /// <summary>
    ///     Disposes the underlying ECDsa instance, releasing all associated resources.
    /// </summary>
    public void Dispose()
    {
        _ecdsa?.Dispose();
    }
}