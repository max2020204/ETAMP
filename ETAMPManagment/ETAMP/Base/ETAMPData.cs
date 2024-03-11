using ETAMPManagment.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ETAMPManagment.ETAMP.Base
{
    public class ETAMPData : IETAMPData
    {
        /// <summary>
        /// The ECDsa instance used for digital signature operations.
        /// </summary>
        private readonly ECDsa _ecdsa;

        /// <summary>
        /// The security algorithm identifier used for signing.
        /// </summary>
        private readonly string _securityAlgorithm;

        /// <summary>
        /// Initializes a new instance of the ETAMPData class with a specified ECDsa instance and security algorithm.
        /// </summary>
        /// <param name="ecdsa">The ECDsa instance for digital signature operations.</param>
        /// <param name="securityAlgorithm">The security algorithm identifier used for signing.</param>
        public ETAMPData(ECDsa ecdsa, string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature)
        {
            _ecdsa = ecdsa;
            _securityAlgorithm = securityAlgorithm;
        }

        /// <summary>
        /// Initializes a new instance of the ETAMPData class with a specified security algorithm, creating a new ECDsa instance internally.
        /// </summary>
        /// <param name="securityAlgorithm">The security algorithm identifier used for signing. Defaults to EcdsaSha512Signature.</param>
        public ETAMPData(string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature)
        {
            _ecdsa = ECDsa.Create();
            _securityAlgorithm = securityAlgorithm;
        }

        /// <summary>
        /// Generates the token data for an ETAMP payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="messageId">The message identifier for the ETAMP token.</param>
        /// <param name="payload">The payload to include in the ETAMP token.</param>
        /// <returns>A string representing the serialized token data.</returns>
        public string CreateEtampData<T>(string messageId, T payload) where T : BasePaylaod
        {
            var handler = new JwtSecurityTokenHandler();
            string payloadJson = JsonConvert.SerializeObject(payload);
            var claimsDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);

            var descriptor = new SecurityTokenDescriptor
            {
                TokenType = "ETAMP",
                Claims = claimsDictionary,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("MessageId", messageId),
                    new Claim("Timestamp", DateTimeOffset.UtcNow.ToString())
                }),
                SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(_ecdsa), _securityAlgorithm)
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}