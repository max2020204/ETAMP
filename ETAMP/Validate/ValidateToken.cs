using ETAMP.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace ETAMP.Validate
{
    public class ValidateToken
    {
        private readonly ECDsa _ecdsa;
        private readonly HashAlgorithmName _algorithm;

        /// <summary>
        /// Initializes a new instance of the ValidateToken class.
        /// </summary>
        /// <param name="ecdsa">The ECDSA instance used for cryptographic verification.</param>
        /// <param name="hash">The hash algorithm used in the cryptographic process.</param>
        public ValidateToken(ECDsa ecdsa, HashAlgorithmName hash)
        {
            _ecdsa = ecdsa;
            _algorithm = hash;
        }
        public ValidateToken()
        {

        }

        /// <summary>
        /// Verifies the given data against the specified signature.
        /// </summary>
        /// <param name="data">The data string to verify.</param>
        /// <param name="signature">The signature string to verify against.</param>
        public bool VerifyData(string data, string signature)
        {
            return _ecdsa.VerifyData(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(signature), _algorithm);
        }

        /// <summary>
        /// Verifies the given data against the specified signature.
        /// </summary>
        /// <param name="data">The data as a byte array to verify.</param>
        /// <param name="signature">The signature as a byte array to verify against.</param>
        public bool VerifyData(byte[] data, byte[] signature)
        {
            return _ecdsa.VerifyData(data, signature, _algorithm);
        }

        /// <summary>
        /// Verifies the given data string against the specified signature byte array.
        /// </summary>
        /// <param name="data">The data string to verify.</param>
        /// <param name="signature">The signature as a byte array to verify against.</param>
        public bool VerifyData(string data, byte[] signature)
        {
            return _ecdsa.VerifyData(Encoding.UTF8.GetBytes(data), signature, _algorithm);
        }

        /// <summary>
        /// Verifies the given data byte array against the specified signature string.
        /// </summary>
        /// <param name="data">The data as a byte array to verify.</param>
        /// <param name="signature">The signature string to verify against.</param>
        public bool VerifyData(byte[] data, string signature)
        {
            return _ecdsa.VerifyData(data, Encoding.UTF8.GetBytes(signature), _algorithm);
        }

        /// <summary>
        /// Verifies the integrity and authenticity of an ETAMP token.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a string to be verified.</param>
        public bool VerifyETAMP(string etamp)
        {
            if (string.IsNullOrEmpty(etamp))
            {
                return false;
            }
            EtampModel? etampModel = JsonConvert.DeserializeObject<EtampModel>(etamp);
            if (etampModel == null)
            {
                return false;
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenData = tokenHandler.ReadJwtToken(etampModel.Token).Payload.ToDictionary();
            if (tokenData["messageId"].ToString() != etampModel.Id.ToString())
            {
                return false;
            }

            byte[] signatureTokenBytes = Convert.FromBase64String(etampModel.SignatureToken);
            if (!VerifyData(etampModel.Token, signatureTokenBytes))
            {
                return false;
            }

            string verificationString = $"{etampModel.Id}{etampModel.Version}{etampModel.Token}{etampModel.UpdateType}{etampModel.SignatureToken}";
            byte[] signatureMessageBytes = Convert.FromBase64String(etampModel.SignatureMessage);
            if (!VerifyData(verificationString, signatureMessageBytes))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Fully verifies an ETAMP using JWT token.
        /// This method first checks the validity of the ETAMP, then deserializes it into an EtampModel,
        /// and finally validates the JWT token using provided audience and issuer parameters.
        /// </summary>
        /// <param name="etamp">The ETAMP string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <returns>True if the ETAMP and JWT token are valid, false otherwise.</returns>
        /// <exception cref="Exception">Throws an exception if token validation fails.</exception>
        public async Task<bool> FullVerify(string etamp, string audience, string issuer)
        {
            if (!VerifyETAMP(etamp))
            {
                return false;
            }
            EtampModel? etampModel = JsonConvert.DeserializeObject<EtampModel>(etamp);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var validationParametr = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                ValidateLifetime = true,
            };
            try
            {
                TokenValidationResult result = await tokenHandler.ValidateTokenAsync(etampModel.Token, validationParametr);
                return result.IsValid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Fully verifies an ETAMP using a JWT token with an ECDSA signature.
        /// This method first checks the validity of the ETAMP, deserializes it into an EtampModel,
        /// and then validates the JWT token using the provided audience, issuer, ECDSA curve, and public key.
        /// </summary>
        /// <param name="etamp">The ETAMP string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim in the JWT token.</param>
        /// <param name="issuer">The expected issuer (iss) claim in the JWT token.</param>
        /// <param name="curve">The ECCurve used for the ECDSA public key.</param>
        /// <param name="publicKey">The public key in Base64 format used for token signature validation.</param>
        /// <returns>True if the ETAMP and JWT token are valid, false otherwise.</returns>
        /// <exception cref="Exception">Throws an exception if token validation fails.</exception>
        public async Task<bool> FullVerifyWithTokenSignature(string etamp, string audience, string issuer, ECCurve curve, string publicKey)
        {
            if (!VerifyETAMP(etamp))
            {
                return false;
            }
            EtampModel? etampModel = JsonConvert.DeserializeObject<EtampModel>(etamp);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            using (ECDsa ecdsa = ECDsa.Create(curve))
            {
                ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);

                var validationParametr = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new ECDsaSecurityKey(ecdsa),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                };
                try
                {
                    TokenValidationResult result = await tokenHandler.ValidateTokenAsync(etampModel.Token, validationParametr);
                    return result.IsValid;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        /// <summary>
        /// Validates the lifetime of a given JWT token.
        /// This method checks if the token is currently valid based on its 'nbf' (not before) and 'exp' (expiration time) claims.
        /// </summary>
        /// <param name="token">The JWT token to be validated.</param>
        /// <returns>True if the token is within its valid time frame, false otherwise.</returns>
        /// <exception cref="Exception">Throws an exception if token validation fails.</exception>
        public async Task<bool> VerifyLifeTime(string token)
        {
            var validationParametr = new TokenValidationParameters()
            {
                ValidateLifetime = true,
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                TokenValidationResult result = await tokenHandler.ValidateTokenAsync(token, validationParametr);
                return result.IsValid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}