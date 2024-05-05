#region

using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace ETAMPManagement.Services.Interfaces;

/// <summary>
///     Defines a provider for creating signing credentials used in the authentication process.
/// </summary>
public interface ISigningCredentialsProvider
{
    /// <summary>
    /// The SecurityAlgorithm property is used to specify the cryptographic algorithm for creating signing credentials.
    /// </summary>
    /// <value>
    /// The security algorithm.
    /// </value>
    public string SecurityAlgorithm { get; set; }


    /// <summary>
    /// Initializes the ECDsaSigningCredentialsProvider by setting the ECDsa instance and the security algorithm.
    /// </summary>
    /// <param name="ecdsaProvider">An instance of IECDsaProvider used to obtain the ECDsa instance.</param>
    void Initialize(IECDsaProvider provider);


    /// <summary>
    /// Creates and returns SigningCredentials based on the ECDsa instance and security algorithm provided during
    /// initialization.
    /// This method encapsulates the ECDsa instance and the chosen security algorithm into a SigningCredentials object,
    /// suitable for use in signing tokens.
    /// </summary>
    /// <param name="verify">Determines whether to verify the created SigningCredentials. Default value is true.</param>
    /// <returns>A new SigningCredentials instance configured with an ECDsaSecurityKey and the specified security algorithm.</returns>
    SigningCredentials CreateSigningCredentials(bool verify = true);
}