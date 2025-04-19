using ETAMP.Core.Interfaces;

namespace ETAMP.Provider.Interfaces;

/// <summary>
///     Verifies signatures using ECDsa.
/// </summary>
public interface IECDsaVerificationProvider : IInitialize
{
    /// <summary>
    ///     Verifies the signature of the provided data using ECDsa.
    /// </summary>
    /// <param name="data">The data to be verified, represented as a read-only byte span.</param>
    /// <param name="signature">The signature to validate the data against, represented as a read-only byte span.</param>
    /// <returns>A boolean value indicating whether the signature validation is successful (true) or not (false).</returns>
    bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature);
}