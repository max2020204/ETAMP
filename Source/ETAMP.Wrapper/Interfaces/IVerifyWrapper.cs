#region

using ETAMP.Core.Interfaces;

#endregion

namespace ETAMP.Wrapper.Interfaces;

/// <summary>
///     Verifies signatures using ECDsa.
/// </summary>
public interface IVerifyWrapper : IInitialize
{
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