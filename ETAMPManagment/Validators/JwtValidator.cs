using ETAMPManagment.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace ETAMPManagment.Validators
{
    public class JwtValidator : IJwtValidator
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtValidator(JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public JwtValidator()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

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

        public virtual async Task<bool> ValidateLifeTime(string token, ECDsaSecurityKey securityKey)
        {
            if (IsValidJwtToken(token))
            {
                TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, GetValidationParameters(securityKey));
                return result.IsValid;
            }
            return false;
        }

        public virtual async Task<bool> ValidateToken(string token, string audience, string issuer, ECDsaSecurityKey securityKey)
        {
            if (IsValidJwtToken(token))
            {
                TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, GetValidationParameters(securityKey, audience, issuer));
                return result.IsValid;
            }
            return false;
        }

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