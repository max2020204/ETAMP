using ETAMPManagment.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace ETAMPManagment.Validators
{
    /// <summary>
    /// Provides functionality for validating JSON Web Tokens (JWT), including structure, lifetime, and claims validation.
    /// </summary>
    public class JwtValidator : IJwtValidator
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        /// <summary>
        /// Initializes a new instance of the JwtValidator class with a custom JwtSecurityTokenHandler.
        /// </summary>
        /// <param name="jwtSecurityTokenHandler">The JwtSecurityTokenHandler used for processing JWT tokens.</param>
        public JwtValidator(JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        /// <summary>
        /// Initializes a new instance of the JwtValidator class with a default JwtSecurityTokenHandler.
        /// </summary>
        public JwtValidator()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Determines whether the provided JWT token is valid based on its structure and header information.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns><c>true</c> if the token is a well-formed JWT; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown if the token is null or empty.</exception>
        /// <exception cref="FormatException">Thrown if the token does not consist of three parts or is not in a valid Base64Url format.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the JWT header is invalid or missing expected values.</exception>
        public virtual bool IsValidJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("The token cannot be null or empty.", nameof(token));
            }

            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                throw new FormatException("JWT must consist of three parts: header, payload, and signature.");
            }

            try
            {
                var headerJson = Base64UrlEncoder.Decode(parts[0]);
                var headerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(headerJson);

                if (headerData == null || !headerData.TryGetValue("typ", out string? value) || value != "ETAMP")
                {
                    throw new InvalidOperationException("The JWT header is invalid or the expected type 'ETAMP' is missing.");
                }
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Error deserializing the JWT header.", ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("Format error decoding from Base64Url.", ex);
            }

            return true;
        }

        /// <summary>
        /// Validates the lifetime of the provided JWT token using a specified ECDsaSecurityKey.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <param name="securityKey">The ECDsaSecurityKey used for signature validation.</param>
        /// <returns>A task that represents the asynchronous operation, yielding true if the token's lifetime is valid; otherwise, false.</returns>
        public virtual async Task<bool> ValidateLifeTime(string token, ECDsaSecurityKey securityKey)
        {
            if (IsValidJwtToken(token))
            {
                TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, GetValidationParameters(securityKey));
                return result.IsValid;
            }
            return false;
        }

        /// <summary>
        /// Validates a JWT token for its lifetime, audience, and issuer claims using a specified ECDsaSecurityKey.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <param name="audience">The expected audience (aud) claim.</param>
        /// <param name="issuer">The expected issuer (iss) claim.</param>
        /// <param name="securityKey">The ECDsaSecurityKey used for signature validation.</param>
        /// <returns>A task that represents the asynchronous operation, yielding true if the token is valid; otherwise, false.</returns>
        public virtual async Task<bool> ValidateToken(string token, string audience, string issuer, ECDsaSecurityKey securityKey)
        {
            if (IsValidJwtToken(token))
            {
                TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, GetValidationParameters(securityKey, audience, issuer));
                return result.IsValid;
            }
            return false;
        }

        /// <summary>
        /// Constructs the TokenValidationParameters used for JWT validation.
        /// </summary>
        /// <param name="issuerSigningKey">The ECDsaSecurityKey used for validating the issuer's signing key.</param>
        /// <param name="validAudience">The expected audience value.</param>
        /// <param name="validIssuer">The expected issuer value.</param>
        /// <returns>The constructed TokenValidationParameters.</returns>
        private TokenValidationParameters GetValidationParameters(ECDsaSecurityKey? issuerSigningKey = null, string? validAudience = null, string? validIssuer = null)
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