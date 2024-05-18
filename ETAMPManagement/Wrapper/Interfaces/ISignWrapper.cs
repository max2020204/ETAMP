#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Models;

#endregion

namespace ETAMPManagement.Wrapper.Interfaces;

/// <summary>
///     Provides functionality for signing ETAMP messages.
/// </summary>
public interface ISignWrapper
{
    /// <summary>
    ///     Initializes the verifier with an ECDsa instance and a hash algorithm.
    ///     This method should be called before performing any verification operations.
    /// </summary>
    /// <param name="ecdsaProvider">The provider for obtaining the ECDsa instance.</param>
    /// <param name="algorithmName">The hash algorithm to use for signature verification.</param>
    void Initialize(IECDsaProvider ecdsaProvider, HashAlgorithmName algorithmName);

    /// <summary>
    ///     Signs the provided ETAMP model by generating a signature for the model's token and message.
    /// </summary>
    /// <typeparam name="T">The type of the token contained in the ETAMP model.</typeparam>
    /// <param name="etamp">The ETAMP model to sign.</param>
    /// <returns>The signed ETAMP model with the generated signature for the token and message.</returns>
    ETAMPModel<T> SignEtampModel<T>(ETAMPModel<T> etamp) where T : Token;
}