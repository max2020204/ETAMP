using System.Text;
using ETAMP.Core;
using ETAMP.Wrapper.Base;

namespace ETAMP.Wrapper;

/// <summary>
///     Provides cryptographic verification using ECDsa, supporting both string and byte array data formats.
/// </summary>
/// >
public sealed class VerifyWrapper : VerifyWrapperBase
{
    /// <summary>
    ///     Verifies the signature of string data.
    /// </summary>
    /// <param name="data">Data to verify, in string format.</param>
    /// <param name="signature">Base64-encoded signature to verify against.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public override bool VerifyData(string data, string signature)
    {
        return Ecdsa!.VerifyData(Encoding.UTF8.GetBytes(data), Base64UrlEncoder.DecodeBytes(signature), AlgorithmName);
    }

    /// <summary>
    ///     Verifies the signature of byte array data.
    /// </summary>
    /// <param name="data">Data to verify, as a byte array.</param>
    /// <param name="signature">Signature to verify against, as a byte array.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public override bool VerifyData(byte[] data, byte[] signature)
    {
        return Ecdsa!.VerifyData(data, signature, AlgorithmName);
    }

    /// <summary>
    ///     Disposes the underlying ECDsa instance, releasing all associated resources.
    /// </summary>
    public override void Dispose()
    {
        Ecdsa?.Dispose();
    }
}