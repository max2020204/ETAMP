using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ETAMPManagment.ETAMP.Base
{
    /// <summary>
    /// Extends <see cref="ETAMPBase"/> to include digital signature functionality for ETAMP (Encrypted Token And Message Protocol) tokens
    /// using a provided signature wrapper. This enhancement ensures the integrity and authenticity of the ETAMP tokens by digitally signing them.
    /// </summary>
    public class ETAMPSign : ETAMPBase
    {
        private readonly ISignWrapper _signWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPSign"/> class with specified ECDsa instance, signature wrapper, and security algorithm.
        /// This constructor is suited for scenarios where an existing ECDsa instance is preferred for signing operations.
        /// </summary>
        /// <param name="ecdsa">The ECDsa instance to be used for signing tokens.</param>
        /// <param name="signWrapper">The wrapper that provides the digital signing functionality.</param>
        /// <param name="securityAlgorithm">The identifier for the security algorithm to use, defaulting to EcdsaSha512Signature.</param>
        public ETAMPSign(ECDsa ecdsa, ISignWrapper signWrapper, string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature) : base(ecdsa, securityAlgorithm)
        {
            _signWrapper = signWrapper;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPSign"/> class with a specified signature wrapper and security algorithm,
        /// creating a new ECDsa instance internally. This constructor is ideal for situations where a specific ECDsa configuration is not required.
        /// </summary>
        /// <param name="signWrapper">The wrapper that provides the digital signing functionality.</param>
        /// <param name="securityAlgorithm">The identifier for the security algorithm to use, defaulting to EcdsaSha512Signature.</param>
        public ETAMPSign(ISignWrapper signWrapper, string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature) : base(securityAlgorithm)
        {
            _signWrapper = signWrapper;
        }

        /// <summary>
        /// Creates a serialized ETAMP token with a digital signature based on the specified update type, payload, and protocol version.
        /// This method enhances the security of the ETAMP token by not only creating it but also signing it to ensure its integrity and authenticity.
        /// </summary>
        /// <typeparam name="T">The type of the payload to be included in the ETAMP token.</typeparam>
        /// <param name="updateType">The update type identifier indicating the purpose or action associated with the ETAMP token.</param>
        /// <param name="payload">The payload to be included and signed within the ETAMP token.</param>
        /// <param name="version">The version of the ETAMP protocol, defaulting to 1.</param>
        /// <returns>A serialized string representation of the ETAMP token, including its digital signature.</returns>
        public override string CreateETAMP<T>(string updateType, T payload, double version = 1)
        {
            return JsonConvert.SerializeObject(CreateETAMPModel(updateType, payload, version));
        }

        /// <summary>
        /// Creates a signed ETAMP model based on the specified update type, payload, and protocol version.
        /// This method allows for the creation of an ETAMP model that is both constructed and signed, ready for serialization or further processing.
        /// </summary>
        /// <typeparam name="T">The type of the payload to be included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier indicating the purpose or action associated with the ETAMP model.</param>
        /// <param name="payload">The payload to be included and signed within the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol, defaulting to 1.</param>
        /// <returns>An instance of <see cref="ETAMPModel"/> that has been digitally signed.</returns>
        public override ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1)
        {
            return _signWrapper.SignEtampModel(base.CreateETAMPModel(updateType, payload, version));
        }
    }
}