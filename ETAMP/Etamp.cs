using ETAMP.Interfaces;
using ETAMP.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ETAMP
{
    public class Etamp : IEtamp
    {
        /// <summary>
        /// Gets the elliptic curve used in cryptographic operations. Defaults to NIST P-521 curve if not specified.
        /// </summary>
        public ECCurve Curve { get; }

        /// <summary>
        /// Gets the hash algorithm used for cryptographic signatures. Default is SHA-512.
        /// </summary>
        public HashAlgorithmName HashAlgorithm { get; }

        /// <summary>
        /// Gets the JWT security algorithm, defaulting to EcdsaSha512Signature.
        /// </summary>
        public string SecurityAlgorithm { get; }

        /// <summary>
        /// Gets the private key in PEM format, generated from the ECDSA object.
        /// </summary>
        public string PrivateKey { get; }

        /// <summary>
        /// Gets the public key in PEM format, extracted from the ECDSA object.
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        /// Gets the instance of the elliptic curve digital signature algorithm used for cryptographic operations.
        /// </summary>
        public ECDsa Ecdsa { get; }

        /// <summary>
        /// Initializes a new instance of the Etamp class with specified ECDSA, elliptic curve, security algorithm, and hash algorithm.
        /// Sets up the cryptographic parameters and generates the private and public keys in PEM format.
        /// </summary>
        /// <param name="ecdsa">The ECDSA instance to use. If null, a new instance will be created.</param>
        /// <param name="curve">The elliptic curve to use. Defaults to NIST P-521 curve if not specified.</param>
        /// <param name="securityAlgorithm">The security algorithm for JWT. Defaults to EcdsaSha512Signature.</param>
        /// <param name="hash">The hash algorithm to use. Default is SHA-512.</param>
        public Etamp(ECDsa? ecdsa = null, ECCurve curve = default, string securityAlgorthm = SecurityAlgorithms.EcdsaSha512Signature, HashAlgorithmName hash = default)
        {
            Curve = curve.IsNamed == default ? ECCurve.NamedCurves.nistP521 : curve;
            HashAlgorithm = hash == default ? HashAlgorithmName.SHA512 : hash;
            Ecdsa = ecdsa == null ? ECDsa.Create() : ecdsa;
            SecurityAlgorithm = securityAlgorthm;
            PrivateKey = Ecdsa.ExportECPrivateKeyPem();
            PublicKey = Ecdsa.ExportSubjectPublicKeyInfoPem();
        }

        /// <summary>
        /// A private method used internally to create JWT tokens with ETAMP-specific data and optional signatures.
        /// </summary>
        /// <param name="messageId">The unique message ID for the ETAMP token.</param>
        /// <param name="payload">The payload to be included in the token.</param>
        /// <param name="signature">Indicates whether the token should be signed.</param>
        /// <typeparam name="T">The type of the payload.</typeparam>
        private string CreateEtampData<T>(string messageId, T payload, bool signature) where T : BasePaylaod
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
            };

            if (signature)
            {
                descriptor.SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(Ecdsa), SecurityAlgorithm);
            }

            return handler.WriteToken(handler.CreateToken(descriptor));
        }

        /// <summary>
        /// Creates an ETAMP token with a unique message ID, payload, and optional signature.
        /// Serializes the token, signs it, and constructs an ETAMP object with all relevant data.
        /// </summary>
        /// <param name="updateType">The type of update this ETAMP represents.</param>
        /// <param name="payload">The payload to be included in the ETAMP token.</param>
        /// <param name="signToken">Indicates whether the token should be signed. Default is true.</param>
        /// <param name="version">The version of the ETAMP. Default is 1.0.</param>
        /// <typeparam name="T">The type of the payload inherited of BasePaylaod.</typeparam>
        public virtual string CreateETAMP<T>(string updateType, T payload, bool signToken = true, double version = 1.0) where T : BasePaylaod
        {
            Guid messageId = Guid.NewGuid();
            string token = CreateEtampData(messageId.ToString(), payload, signToken);
            string signatureToken = SignData(token);
            string signMessage = SignData($"{messageId}{version}{token}{updateType}{signatureToken}");
            EtampModel etamp = new()
            {
                Id = messageId,
                Version = version,
                Token = token,
                UpdateType = updateType,
                SignatureToken = signatureToken,
                SignatureMessage = signMessage
            };
            return JsonConvert.SerializeObject(etamp);
        }

        /// <summary>
        /// Creates an ETAMP token with a unique message ID and payload. This version of the method does not include a signature.
        /// Constructs an ETAMP object with the essential data without signing it.
        /// </summary>
        /// <param name="updateType">The type of update this ETAMP represents.</param>
        /// <param name="payload">The payload to be included in the ETAMP token.</param>
        /// <param name="signToken">Indicates whether the token should be signed. This parameter is present for compatibility but not used in this method. Default is true.</param>
        /// <param name="version">The version of the ETAMP. Default is 1.0.</param>
        /// <typeparam name="T">The type of the payload inherited from BasePayload.</typeparam>
        /// <returns>A JSON-serialized string representation of the ETAMP object without a signature.</returns>
        public virtual string CreateETAMPWithoutSignature<T>(string updateType, T payload, bool signToken = true, double version = 1.0) where T : BasePaylaod
        {
            Guid messageId = Guid.NewGuid();
            string token = CreateEtampData(messageId.ToString(), payload, signToken);
            EtampModel etamp = new()
            {
                Id = messageId,
                Version = version,
                Token = token,
                UpdateType = updateType
            };
            return JsonConvert.SerializeObject(etamp);
        }

        /// <summary>
        /// Signs the given string data using the ECDSA instance and the specified hash algorithm.
        /// Returns the base64-encoded signature.
        /// </summary>
        /// <param name="data">The data to be signed.</param>
        private string SignData(string data)
        {
            return Convert.ToBase64String(Ecdsa.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithm));
        }
    }
}