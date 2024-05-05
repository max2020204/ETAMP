#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace ETAMPManagement.Services;

/// <summary>
///     Provides signing credentials using the ECDsa cryptographic algorithm.
///     This provider facilitates the creation of signing credentials that can be used for digital signatures.
/// </summary>
public class ECDsaSigningCredentialsProvider : ISigningCredentialsProvider
{
    /// <summary>
    ///     Provides signing credentials using the ECDsa cryptographic algorithm.
    ///     This provider facilitates the creation of signing credentials that can be used for digital signatures.
    /// </summary>
    private readonly ECDsa _ecdsa;

    /// <summary>
    ///     Initializes a new instance of the ECDsaSigningCredentialsProvider class using a provider for ECDsa instances and a
    ///     security algorithm.
    /// </summary>
    /// <param name="ecdsaProvider">The provider for obtaining an ECDsa instance to use for signing.</param>
    public ECDsaSigningCredentialsProvider(IECDsaProvider ecdsaProvider)
    {
        ArgumentNullException.ThrowIfNull(ecdsaProvider);
        _ecdsa = ecdsaProvider.GetECDsa()
                 ?? throw new InvalidOperationException("ECDsa instance cannot be null.");
        SecurityAlgorithm = SecurityAlgorithms.EcdsaSha256Signature;
    }

    /// <summary>
    ///     Represents the security algorithm used for signing credentials.
    /// </summary>
    public string SecurityAlgorithm { get; set; }

    /// <summary>
    ///     Creates and returns SigningCredentials based on the ECDsa instance and security algorithm provided during
    ///     initialization.
    ///     This method encapsulates the ECDsa instance and the chosen security algorithm into a SigningCredentials object,
    ///     suitable for use in signing tokens.
    /// </summary>
    /// <returns>A new SigningCredentials instance configured with an ECDsaSecurityKey and the specified security algorithm.</returns>
    public SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(new ECDsaSecurityKey(_ecdsa), SecurityAlgorithm);
    }
}