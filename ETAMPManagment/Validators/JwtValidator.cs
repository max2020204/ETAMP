using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace ETAMPManagment.Validators
{
    /// <summary>
    /// Provides functionality for validating JSON Web Tokens (JWT), including checks for structure, lifetime, and claims validation.
    /// </summary>
    public class JwtValidator : IJwtValidator
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtValidator()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Determines whether the provided JWT token is valid based on its structure and header information.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>A JwtValidationResult indicating whether the token is a well-formed JWT and containing the error message if it's not.</returns>
        public virtual ValidationResult IsValidJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return new ValidationResult(false, "The token cannot be null or empty");

            var parts = token.Split('.');

            if (parts.Length != 3)
                return new ValidationResult(false, "JWT must consist of three parts: header, payload, and signature");

            try
            {
                var headerJson = Base64UrlEncoder.Decode(parts[0]);
                var headerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(headerJson);

                if (headerData == null || !headerData.TryGetValue("typ", out string? value) || value != "ETAMP")
                    return new ValidationResult(false, "The JWT header is invalid or the expected type 'ETAMP' is missing");
            }
            catch (JsonException ex)
            {
                return new ValidationResult(false, $"Error deserializing the JWT header: {ex.Message}");
            }
            catch (FormatException ex)
            {
                return new ValidationResult(false, $"Format error decoding from Base64Url: {ex.Message}");
            }

            return new ValidationResult(true);
        }

        /// <summary>
        /// Asynchronously validates the lifetime of the provided JWT token using a specified ECDsa security key.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <param name="securityKey">The ECDsaSecurityKey used for signature validation.</param>
        /// <returns>A task that represents the asynchronous validation operation, yielding a <see cref="ValidationResult"/> that contains the validation outcome.</returns>
        /// <remarks>
        /// This method first checks if the token is well-formed and has a valid structure using <see cref="IsValidJwtToken"/>.
        /// Then it validates the token's lifetime against the provided security key.
        /// The method returns a detailed validation result, indicating whether the token's lifetime is valid.
        /// Specific lifetime-related issues are reported in the validation result.
        /// </remarks>
        public virtual async Task<ValidationResult> ValidateLifeTime(string token, ECDsaSecurityKey securityKey)
        {
            var structuralValidation = IsValidJwtToken(token);
            if (!structuralValidation.IsValid)
                return await Task.FromResult(structuralValidation);

            try
            {
                TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, GetValidationParameters(securityKey));
                return await Task.FromResult(new ValidationResult(result.IsValid, result.IsValid ? null : "Token's lifetime validation failed."));
            }
            catch (SecurityTokenExpiredException ex)
            {
                return await Task.FromResult(new ValidationResult(false, $"Token has expired: {ex.Message}"));
            }
            catch (SecurityTokenNotYetValidException ex)
            {
                return await Task.FromResult(new ValidationResult(false, $"Token is not yet valid: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ValidationResult(false, $"Token validation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Asynchronously validates a JWT token, including its lifetime, audience, and issuer claims, using a specified ECDsa security key.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <param name="securityKey">The ECDsaSecurityKey used for token signature validation.</param>
        /// <returns>A task that represents the asynchronous validation operation, yielding a <see cref="ValidationResult"/> that contains the validation outcome.</returns>
        /// <remarks>
        /// This method first performs a structural validation of the JWT token to ensure it is well-formed.
        /// If the token passes this initial check, it then proceeds to a comprehensive validation against the provided audience, issuer, and signature.
        /// Specific checks include validating the token's lifetime, audience, and issuer claims, as well as verifying the token's signature using the provided ECDsa security key.
        /// The method returns a detailed validation result, indicating whether all aspects of the token are valid or not, and provides specific error messages for different types of validation failures.
        /// </remarks>
        public virtual async Task<ValidationResult> ValidateToken(string token, string audience, string issuer, ECDsaSecurityKey securityKey)
        {
            var structuralValidation = IsValidJwtToken(token);
            if (!structuralValidation.IsValid)
                return await Task.FromResult(structuralValidation);

            try
            {
                TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, GetValidationParameters(securityKey, audience, issuer));
                return await Task.FromResult(new ValidationResult(result.IsValid, result.IsValid ? null : "Invalid JWT token claims."));
            }
            catch (SecurityTokenExpiredException ex)
            {
                return await Task.FromResult(new ValidationResult(false, $"Token is expired: {ex.Message}"));
            }
            catch (SecurityTokenNotYetValidException ex)
            {
                return await Task.FromResult(new ValidationResult(false, $"Token is not yet valid: {ex.Message}"));
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                return await Task.FromResult(new ValidationResult(false, $"Invalid token signature: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ValidationResult(false, $"Token validation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Constructs the TokenValidationParameters used for JWT validation.
        /// </summary>
        /// <param name="issuerSigningKey">The ECDsaSecurityKey used for validating the issuer's signing key.</param>
        /// <param name="validAudience">The expected audience value.</param>
        /// <param name="validIssuer">The expected issuer value.</param>
        /// <returns>The constructed TokenValidationParameters.</returns>
        private TokenValidationParameters GetValidationParameters(ECDsaSecurityKey issuerSigningKey, string? validAudience = null, string? validIssuer = null)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = issuerSigningKey,
                ValidateIssuer = !string.IsNullOrEmpty(validIssuer),
                ValidIssuer = validIssuer,
                ValidateAudience = !string.IsNullOrEmpty(validAudience),
                ValidAudience = validAudience,
                ValidateLifetime = true
            };
        }
    }
}