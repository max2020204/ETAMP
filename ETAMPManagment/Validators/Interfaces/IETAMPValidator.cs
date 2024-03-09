using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Validators.Interfaces
{
    /// <summary>
    /// Defines methods for validating ETAMP (Encrypted Token And Message Protocol) tokens against security and integrity checks.
    /// </summary>
    public interface IETAMPValidator
    {
        /// <summary>
        /// Validates an ETAMP token including checks against expected audience and issuer, using a specified ECDsa security key for signature validation.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key used for token signature validation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the ETAMP token is valid; otherwise, <c>false</c>.</returns>
        Task<bool> ValidateETAMP(string etamp, string audience, string issuer, ECDsaSecurityKey tokenSecurityKey);

        /// <summary>
        /// Validates an ETAMP token using a specified ECDsa security key for signature validation without checking the audience and issuer claims.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key used for token signature validation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the ETAMP token is valid; otherwise, <c>false</c>.</returns>
        Task<bool> ValidateETAMP(string etamp, ECDsaSecurityKey tokenSecurityKey);

        /// <summary>
        /// Performs a lightweight validation of an ETAMP token using a specified ECDsa security key, focusing on signature validation only.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key used for token signature validation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the ETAMP token's signature is valid; otherwise, <c>false</c>.</returns>
        Task<bool> ValidateETAMPLite(string etamp, ECDsaSecurityKey tokenSecurityKey);
    }
}