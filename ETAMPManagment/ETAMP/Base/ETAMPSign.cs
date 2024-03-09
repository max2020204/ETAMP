using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ETAMPManagment.ETAMP.Base
{
    /// <summary>
    /// Extends ETAMPBase to include digital signature functionality using a provided signature wrapper.
    /// </summary>
    public class ETAMPSign : ETAMPBase
    {
        private readonly ISignWrapper _signWrapper;

        /// <summary>
        /// Initializes a new instance of the ETAMPSign class with a specified ECDSA, signature wrapper, and security algorithm.
        /// </summary>
        /// <param name="ecdsa">The ECDSA instance for signing tokens.</param>
        /// <param name="signWrapper">The signature wrapper for signing ETAMP models.</param>
        /// <param name="securityAlgorithm">The security algorithm identifier, defaulting to EcdsaSha512Signature.</param>
        public ETAMPSign(ECDsa ecdsa, ISignWrapper signWrapper, string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature) : base(ecdsa, securityAlgorithm)
        {
            _signWrapper = signWrapper;
        }

        /// <summary>
        /// Creates a serialized ETAMP token with digital signature.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP token.</param>
        /// <param name="payload">The payload to include in the token.</param>
        /// <param name="version">The version of the protocol, defaulting to 1.</param>
        /// <returns>A serialized string representation of the signed ETAMP token.</returns>
        public override string CreateETAMP<T>(string updateType, T payload, double version = 1)
        {
            return JsonConvert.SerializeObject(CreateETAMPModel(updateType, payload, version));
        }

        /// <summary>
        /// Creates a signed ETAMP model.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload to include in the model.</param>
        /// <param name="version">The version of the protocol, defaulting to 1.</param>
        /// <returns>A signed ETAMPModel instance.</returns>
        public override ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1)
        {
            return _signWrapper.SignEtampModel(base.CreateETAMPModel(updateType, payload, version));
        }
    }
}