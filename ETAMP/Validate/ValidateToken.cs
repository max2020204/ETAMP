using ETAMP.Models;
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
    }
}