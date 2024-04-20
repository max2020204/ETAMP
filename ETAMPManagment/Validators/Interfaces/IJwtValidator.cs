using ETAMPManagment.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ETAMPManagment.Validators.Interfaces
{
    /// <summary>
    /// Defines methods for validating JSON Web Tokens (JWT), including checks for their structure, lifetime, and claims.
    /// </summary>
    public interface IJwtValidator
    {
        /// <summary>
        /// Asynchronously validates the lifetime of a JWT token using a specified ECDsa security key.
        /// </summary>
        /// <param name="token">The JWT token as a string to be validated for its lifetime.</param>
        /// <param name="securityKey">The ECDsa security key used for token signature validation.</param>
        /// <returns>A task that represents the asynchronous validation operation, yielding a JwtValidationResult that describes the outcome of the validation.</returns>
        Task<ValidationResult> ValidateLifeTime(string token, ECDsaSecurityKey securityKey);

        /// <summary>
        /// Validates whether a JWT token is well-formed and has a valid signature.
        /// </summary>
        /// <param name="token">The JWT token as a string to be validated.</param>
        /// <returns>A JwtValidationResult indicating whether the JWT token is valid based on its structure and signature.</returns>
        ValidationResult IsValidJwtToken(string token);

        /// <summary>
        /// Asynchronously validates a JWT token, including its audience and issuer claims, using a specified ECDsa security key.
        /// </summary>
        /// <param name="token">The JWT token as a string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <param name="securityKey">The ECDsa security key used for token signature validation.</param>
        /// <returns>A task that represents the asynchronous validation operation, yielding a JwtValidationResult that describes the outcome of the validation.</returns>
        Task<ValidationResult> ValidateToken(string token, string audience, string issuer, ECDsaSecurityKey securityKey);
    }
}