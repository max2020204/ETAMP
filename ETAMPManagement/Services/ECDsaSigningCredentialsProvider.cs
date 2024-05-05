#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Helper;
using ETAMPManagement.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace ETAMPManagement.Services;

/// <summary>
///     Provides signing credentials using the ECDsa cryptographic algorithm.
///     This provider facilitates the creation of signing credentials that can be used for digital signatures.
/// </summary>
public class ECDsaSigningCredentialsProvider : InitializeBase, ISigningCredentialsProvider
{
    /// <summary>
    ///     Provides signing credentials using the ECDsa cryptographic algorithm.
    ///     This provider facilitates the creation of signing credentials that can be used for digital signatures.
    /// </summary>
    private ECDsa _ecdsa;

    /// <summary>
    /// Provides signing credentials using the ECDsa cryptographic algorithm.
    /// This provider facilitates the creation of signing credentials that can be used for digital signatures.
    /// </summary>
    public ECDsaSigningCredentialsProvider()
    {
        _ecdsa = ECDsa.Create();
        SecurityAlgorithm = SecurityAlgorithms.EcdsaSha256Signature;
    }

    /// <summary>
    /// Initializes the ECDsaSigningCredentialsProvider by setting the ECDsa instance and the security algorithm.
    /// </summary>
    /// <param name="ecdsaProvider">An instance of IECDsaProvider used to obtain the ECDsa instance.</param>
    public void Initialize(IECDsaProvider ecdsaProvider)
    {
        _init = true;
        ArgumentNullException.ThrowIfNull(ecdsaProvider);
        _ecdsa = ecdsaProvider.GetECDsa()
                 ?? throw new InvalidOperationException("ECDsa instance cannot be null.");
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
    public SigningCredentials CreateSigningCredentials(bool verify = true)
    {
        if (verify)
        {
            CheckInitialization();
        }
        return new SigningCredentials(new ECDsaSecurityKey(_ecdsa), SecurityAlgorithm);
    }
}