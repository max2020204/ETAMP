#region

using ETAMPManagement.Models;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace ETAMPManagement.Validators.Interfaces;

/// <summary>
///     Defines methods for validating ETAMP (Encrypted Token And Message Protocol) tokens against security and integrity
///     checks.
/// </summary>
public interface IETAMPValidator
{
    /// <summary>
    ///     Validates an ETAMP token, including checks against expected audience and issuer, using a specified ECDsa security
    ///     key for signature validation.
    /// </summary>
    /// <param name="etamp">The ETAMP token model to be validated.</param>
    /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
    /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
    /// <param name="tokenSecurityKey">The ECDsa security key used for token signature validation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, yielding true if the ETAMP token is valid; otherwise,
    ///     false.
    /// </returns>
    Task<bool> ValidateETAMP(ETAMPModel etamp, string audience, string issuer, ECDsaSecurityKey tokenSecurityKey);

    /// <summary>
    ///     Validates an ETAMP token for structure and signature, and checks the token's lifetime using a specified ECDsa
    ///     security key.
    /// </summary>
    /// <param name="etamp">The ETAMP token model to be validated.</param>
    /// <param name="tokenSecurityKey">The ECDsa security key used for token signature validation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, yielding true if the ETAMP token is valid; otherwise,
    ///     false.
    /// </returns>
    Task<bool> ValidateETAMP(ETAMPModel etamp, ECDsaSecurityKey tokenSecurityKey);

    /// <summary>
    ///     Performs a lightweight validation of an ETAMP token, focusing primarily on structure and signature checks using a
    ///     specified ECDsa security key.
    /// </summary>
    /// <param name="etamp">The ETAMP token model to be validated.</param>
    /// <param name="tokenSecurityKey">The ECDsa security key used for validating the token's signature.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, yielding true if the ETAMP token's structure and signature
    ///     are valid; otherwise, false.
    /// </returns>
    Task<bool> ValidateETAMPLite(ETAMPModel etamp, ECDsaSecurityKey tokenSecurityKey);
}