#region

using ETAMP.Wrapper.Helper;
using ETAMP.Wrapper.Interfaces;

#endregion

namespace ETAMP.Wrapper.Base;

/// <summary>
///     Provides a wrapper class for verifying data using ECDsa cryptographic algorithm.
/// </summary>
public abstract class VerifyWrapperBase : InitializeWrapper, IVerifyWrapper
{
    /// <summary>
    ///     Disposes the underlying ECDsa instance, releasing all associated resources.
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    ///     Verifies the data using the specified signature.
    /// </summary>
    /// <param name="data">The data to be verified.</param>
    /// <param name="signature">The signature to be used for verification.</param>
    /// <returns>Returns true if the data is verified successfully; otherwise, false.</returns>
    public abstract bool VerifyData(string data, string signature);

    /// <summary>
    ///     Verifies the signature of data using ECDsa cryptographic algorithm.
    /// </summary>
    /// <param name="data">The data to verify.</param>
    /// <param name="signature">The signature to check against.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public abstract bool VerifyData(byte[] data, byte[] signature);
}