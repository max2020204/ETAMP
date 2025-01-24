#region

using System.Security.Cryptography;
using System.Text;
using ETAMP.Core.Utils;
using ETAMP.Wrapper.Interfaces;

#endregion

namespace ETAMP.Wrapper;

/// <summary>
///     Provides cryptographic verification using ECDsa, supporting both string and byte array data formats.
/// </summary>
/// >
public sealed class VerifyWrapper : IVerifyWrapper
{
    private HashAlgorithmName _algorithmName;
    private ECDsa? _ecdsa;

    /// <summary>
    ///     Verifies the signature of string data.
    /// </summary>
    /// <param name="data">Data to verify, in string format.</param>
    /// <param name="signature">Base64-encoded signature to verify against.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public bool VerifyData(string data, string signature)
    {
        return _ecdsa!.VerifyData(Encoding.UTF8.GetBytes(data), Base64UrlEncoder.DecodeBytes(signature),
            _algorithmName);
    }

    /// <summary>
    ///     Verifies the signature of byte array data.
    /// </summary>
    /// <param name="data">Data to verify, as a byte array.</param>
    /// <param name="signature">Signature to verify against, as a byte array.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public bool VerifyData(byte[] data, byte[] signature)
    {
        return _ecdsa!.VerifyData(data, signature, _algorithmName);
    }

    /// <summary>
    ///     Disposes the underlying ECDsa instance, releasing all associated resources.
    /// </summary>
    public void Dispose()
    {
        _ecdsa?.Dispose();
    }

    public void Initialize(ECDsa provider, HashAlgorithmName algorithmName)
    {
        _ecdsa = provider;
        _algorithmName = algorithmName;
    }
}