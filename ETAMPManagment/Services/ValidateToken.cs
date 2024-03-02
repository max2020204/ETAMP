using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace ETAMPManagment.Services
{
    public class ValidateToken : IValidateToken
    {
        private readonly IVerifyWrapper _verifyWrapper;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        /// <summary>
        /// Initializes a new instance of the ValidateToken class with a specified verification wrapper.
        /// This constructor sets up the class with a default JwtSecurityTokenHandler and uses the provided IVerifyWrapper
        /// instance for cryptographic verification tasks.
        /// </summary>
        /// <param name="verifyWrapper">The IVerifyWrapper instance to be used for cryptographic verification.</param>
        public ValidateToken(IVerifyWrapper verifyWrapper)
        {
            _verifyWrapper = verifyWrapper;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Initializes a new instance of the ValidateToken class with specified verification wrapper and JWT security token handler.
        /// This constructor allows for custom configuration of both the verification process and the JWT token handling.
        /// </summary>
        /// <param name="verifyWrapper">The IVerifyWrapper instance to be used for cryptographic verification.</param>
        /// <param name="jwtSecurityTokenHandler">The JwtSecurityTokenHandler instance to be used for handling JWT tokens.</param>
        public ValidateToken(IVerifyWrapper verifyWrapper, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _verifyWrapper = verifyWrapper;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        /// <summary>
        /// Verifies the integrity and authenticity of an ETAMP token.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a string to be verified.</param>
        public virtual bool VerifyETAMP(string etamp)
        {
            if (string.IsNullOrEmpty(etamp))
            {
                return false;
            }
            ETAMPModel? ETAMPModel = JsonConvert.DeserializeObject<ETAMPModel>(etamp);
            if (ETAMPModel?.Id == Guid.Empty)
            {
                return false;
            }
            var tokenData = _jwtSecurityTokenHandler.ReadJwtToken(ETAMPModel?.Token).Payload.ToDictionary();

            if (!tokenData.ContainsKey("MessageId") || tokenData["MessageId"].ToString() != ETAMPModel?.Id.ToString())
            {
                return false;
            }
            if (ETAMPModel?.SignatureToken != null && ETAMPModel?.Token != null && !ValidateSignature(ETAMPModel?.Token, ETAMPModel?.SignatureToken))
            {
                return false;
            }

            if (ETAMPModel != null)
            {
                string verificationString = $"{ETAMPModel.Id}{ETAMPModel.Version}{ETAMPModel.Token}{ETAMPModel.UpdateType}{ETAMPModel.SignatureToken}";
                if (ETAMPModel.SignatureMessage != null && !ValidateSignature(verificationString, ETAMPModel.SignatureMessage))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidateSignature(string data, string sign)
        {
            return _verifyWrapper.VerifyData(data, Convert.FromBase64String(sign));
        }

        /// <summary>
        /// Fully verifies an ETAMP using JWT token.
        /// This method first checks the validity of the ETAMP, then deserializes it into an ETAMPModel,
        /// and finally validates the JWT token using provided audience and issuer parameters.
        /// </summary>
        /// <param name="etamp">The ETAMP string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <returns>True if the ETAMP and JWT token are valid, false otherwise.</returns>
        /// <exception cref="Exception">Throws an exception if token validation fails.</exception>
        public virtual async Task<bool> FullVerify(string etamp, string audience, string issuer)
        {
            if (!VerifyAndDeserializeETAMP(etamp, out ETAMPModel model))
            {
                return false;
            }
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;
            TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(model.Token,
              TokenValidationParameters(audience, issuer));
            if (result.IsValid)
            {
                return true;
            }
            throw result.Exception;
        }

        /// <summary>
        /// Fully verifies an ETAMP using a JWT token with an ECDSA signature.
        /// This method first checks the validity of the ETAMP, deserializes it into an ETAMPModel,
        /// and then validates the JWT token using the provided audience, issuer, ECDSA curve, and public key.
        /// </summary>
        /// <param name="etamp">The ETAMP string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <param name="curve">The ECCurve used for the ECDSA public key.</param>
        /// <param name="publicKey">The public key in Base64 format used for token signature validation.</param>
        /// <returns>True if the ETAMP and JWT token are valid, false otherwise.</returns>
        /// <exception cref="Exception">Throws an exception if token validation fails.</exception>
        public virtual async Task<bool> FullVerifyWithTokenSignature(string etamp, string audience, string issuer, ECCurve curve, string publicKey)
        {
            if (!VerifyAndDeserializeETAMP(etamp, out ETAMPModel model))
            {
                return false;
            }
            using (ECDsa ecdsa = ECDsa.Create(curve))
            {
                ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
                TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(model.Token,
                    TokenValidationParametersWithSignature(audience, issuer, new ECDsaSecurityKey(ecdsa)));
                if (result.IsValid)
                {
                    return true;
                }
                throw result.Exception;
            }
        }

        /// <summary>
        /// Performs a complete verification of an ETAMP using a JWT token with ECDSA signing.
        /// This method validates the ETAMP, deserializes it into an ETAMPModel, and then checks the JWT token's integrity
        /// and authenticity using the default ECDSA instance and hash algorithm.
        /// </summary>
        /// <param name="etamp">The ETAMP string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <returns>True if the ETAMP and JWT token are valid, false otherwise.</returns>
        /// <exception cref="Exception">Throws an exception if token validation fails.</exception>

        public virtual async Task<bool> FullVerifyWithTokenSignature(string etamp, string audience, string issuer, ECDsa ecdsa)
        {
            if (!VerifyAndDeserializeETAMP(etamp, out ETAMPModel model))
            {
                return false;
            }
            TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(model.Token,
                TokenValidationParametersWithSignature(audience, issuer, new ECDsaSecurityKey(ecdsa)));
            if (result.IsValid)
            {
                return true;
            }
            throw result.Exception;
        }

        /// <summary>
        /// Asynchronously verifies an ETAMP with its associated JWT token signature. The method first attempts to
        /// verify and deserialize the provided ETAMP. If successful, it then validates the JWT token extracted
        /// from the ETAMP, ensuring its integrity and authenticity. The token validation is performed using ECDSA
        /// signing with audience and issuer claims.
        /// </summary>
        /// <param name="etamp">The ETAMP string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <returns>True if both the ETAMP and its JWT token are valid, false if the ETAMP is invalid.</returns>
        /// <exception cref="Exception">Throws an exception if JWT token validation fails.</exception>
        public virtual async Task<bool> FullVerifyWithTokenSignature(string etamp, string audience, string issuer)
        {
            if (!VerifyAndDeserializeETAMP(etamp, out ETAMPModel model))
            {
                return false;
            }
            TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(model.Token,
                TokenValidationParametersWithSignature(audience, issuer, new ECDsaSecurityKey(_verifyWrapper.ECDsa)));
            if (result.IsValid)
            {
                return true;
            }
            throw result.Exception;
        }

        /// <summary>
        /// Performs a lightweight verification of an ETAMP using a JWT token with ECDSA signing.
        /// This method checks the ETAMP's validity, deserializes it into an ETAMPModel, and then validates the JWT token
        /// focusing primarily on the signature's integrity and the token's lifetime, using a custom ECDSA curve and public key.
        /// </summary>
        /// <param name="etamp">The ETAMP string to be validated.</param>
        /// <param name="curve">The ECCurve used for the ECDSA public key.</param>
        /// <param name="publicKey">The public key in Base64 format used for token signature validation.</param>
        /// <returns>True if the ETAMP and JWT token are valid, false otherwise.</returns>
        public virtual async Task<bool> FullVerifyLite(string etamp, ECCurve curve, string publicKey, IEcdsaWrapper ecdsaWrapper)
        {
            if (!VerifyAndDeserializeETAMP(etamp, out ETAMPModel model))
            {
                return false;
            }

            using (ECDsa ecdsa = ecdsaWrapper.CreateECDsa(publicKey, curve))
            {
                TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(model.Token,
                    TokenValidationParametersWithSignature(new ECDsaSecurityKey(ecdsa)));
                if (result.IsValid)
                {
                    return true;
                }
                throw result.Exception;
            }
        }

        /// <summary>
        /// Performs a lightweight verification of an ETAMP using a JWT token with ECDSA signing.
        /// This method checks the ETAMP's validity, deserializes it into an ETAMPModel, and then validates the JWT token
        /// focusing primarily on the signature's integrity and the token's lifetime, using the default ECDSA instance and hash algorithm.
        /// </summary>
        /// <param name="etamp">The ETAMP string to be validated.</param>
        /// <returns>True if the ETAMP and JWT token are valid, false otherwise.</returns>
        public virtual async Task<bool> FullVerifyLite(string etamp, ECDsa ecdsa)
        {
            if (!VerifyAndDeserializeETAMP(etamp, out ETAMPModel model))
            {
                return false;
            }
            TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(model.Token,
                TokenValidationParametersWithSignature(securityKey: new ECDsaSecurityKey(ecdsa)));
            if (result.IsValid)
            {
                return true;
            }
            throw result.Exception;
        }

        /// <summary>
        /// Validates the lifetime of a given JWT token.
        /// This method checks if the token is currently valid based on its 'nbf' (not before) and 'exp' (expiration time) claims.
        /// </summary>
        /// <param name="token">The JWT token to be validated.</param>
        /// <returns>True if the token is within its valid time frame, false otherwise.</returns>
        /// <exception cref="Exception">Throws an exception if token validation fails.</exception>
        public virtual async Task<bool> VerifyLifeTime(string token)
        {
            var validationParametr = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                SignatureValidator = (encodedString, parameters) =>
                {
                    return new JwtSecurityToken(encodedString);
                }
            };

            TokenValidationResult result = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, validationParametr);
            if (result.IsValid)
            {
                return true;
            }
            throw result.Exception;
        }

        private TokenValidationParameters TokenValidationParameters(string audience, string issuer)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = !string.IsNullOrEmpty(issuer),
                ValidateAudience = !string.IsNullOrEmpty(audience),
                ValidAudience = audience,
                ValidIssuer = issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                SignatureValidator = (token, parameters) => new JwtSecurityToken(token)
            };
            return parameters;
        }

        private TokenValidationParameters TokenValidationParametersWithSignature(string audience, string issuer, ECDsaSecurityKey securityKey)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey
            };
            return parameters;
        }

        private TokenValidationParameters TokenValidationParametersWithSignature(ECDsaSecurityKey securityKey)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey
            };
            return parameters;
        }

        private bool VerifyAndDeserializeETAMP(string etamp, out ETAMPModel model)
        {
            if (!VerifyETAMP(etamp))
            {
                model = null;
                return false;
            }

            model = JsonConvert.DeserializeObject<ETAMPModel>(etamp);
            return true;
        }
    }
}