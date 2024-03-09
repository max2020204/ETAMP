using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Validators.Interfaces
{
    /// <summary>
    /// Defines methods for validating JSON Web Tokens (JWT) for their lifetime, structure, and signature.
    /// </summary>
    public interface IJwtValidator
    {
        /// <summary>
        /// Asynchronously validates the lifetime of a JWT token using a specified ECDsa security key.
        /// </summary>
        /// <param name="token">The JWT token as a string to be validated for its lifetime.</param>
        /// <param name="securityKey">The ECDsa security key used for token signature validation.</param>
        /// <returns>A task that represents the asynchronous validation operation. The task result contains <c>true</c> if the JWT token's lifetime is valid; otherwise, <c>false</c>.</returns>
        Task<bool> ValidateLifeTime(string token, ECDsaSecurityKey securityKey);

        /// <summary>
        /// Checks if a JWT token is valid based on its structure and signature without validating its claims or lifetime.
        /// </summary>
        /// <param name="token">The JWT token as a string to be validated.</param>
        /// <returns><c>true</c> if the JWT token has a valid structure and signature; otherwise, <c>false</c>.</returns>
        bool IsValidJwtToken(string token);

        /// <summary>
        /// Asynchronously validates a JWT token including its audience and issuer claims using a specified ECDsa security key.
        /// </summary>
        /// <param name="token">The JWT token as a string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <param name="securityKey">The ECDsa security key used for token signature validation.</param>
        /// <returns>A task that represents the asynchronous validation operation. The task result contains <c>true</c> if the JWT token is valid; otherwise, <c>false</c>.</returns>
        Task<bool> ValidateToken(string token, string audience, string issuer, ECDsaSecurityKey securityKey);
    }
}