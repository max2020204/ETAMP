#region

using Microsoft.IdentityModel.Tokens;

#endregion

namespace ETAMPManagement.Services.Interfaces;

/// <summary>
///     Defines a provider for creating signing credentials used in the authentication process.
/// </summary>
public interface ISigningCredentialsProvider
{
    public string SecurityAlgorithm { get; set; }

    /// <summary>
    ///     Creates signing credentials for use in signing a token.
    /// </summary>
    /// <returns>The signing credentials.</returns>
    SigningCredentials CreateSigningCredentials();
}