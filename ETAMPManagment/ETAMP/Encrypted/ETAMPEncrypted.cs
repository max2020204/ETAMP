using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;

namespace ETAMPManagment.ETAMP.Encrypted
{
    /// <summary>
    /// Extends <see cref="ETAMPBase"/> to incorporate encryption functionalities into ETAMP (Encrypted Token And Message Protocol) tokens,
    /// using the Elliptic Curve Integrated Encryption Scheme (ECIES). This class allows for the creation of ETAMP tokens that are not only signed but also encrypted,
    /// enhancing confidentiality alongside the existing integrity and authentication provided by the base functionality.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ETAMPEncrypted"/> class with an encryption service and a signing credentials provider.
    /// This constructor is suited for scenarios where both encryption of ETAMP tokens and specific signing capabilities are required.
    /// </remarks>
    /// <param name="eciesEncryptionService">The service used to encrypt ETAMP tokens.</param>
    /// <param name="signingCredentialsProvider">The provider used to create signing credentials for digital signature operations.</param>
    public class ETAMPEncrypted(IEciesEncryptionService eciesEncryptionService,
                                ISigningCredentialsProvider signingCredentialsProvider) : ETAMPBase(signingCredentialsProvider), IETAMPEncrypted
    {
        /// <summary>
        /// Creates an ETAMP model with encryption based on the specified update type, payload, and protocol version.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload to be encrypted and included in the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol, defaulting to 1.</param>
        /// <returns>An instance of <see cref="ETAMPModel"/> containing the encrypted payload.</returns>
        public virtual ETAMPModel CreateEncryptETAMPModel<T>(string updateType, T payload, double version = 1) where T : BasePayload
        {
            ETAMPModel model = CreateETAMPModel(updateType, payload, version);
            model.Token = eciesEncryptionService.Encrypt(model.Token);
            return model;
        }
    }
}