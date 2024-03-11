using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Factories.Interfaces;
using ETAMPManagment.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Utils;

namespace ETAMPManagment
{
    /// <summary>
    /// A builder class for creating and configuring ETAMP (Encrypted Token And Message Protocol) models.
    /// Utilizes a factory pattern to dynamically generate ETAMP models based on specified types,
    /// such as base, signed, encrypted, and encrypted and signed models.
    /// </summary>
    public class ETAMPBuilder : IETAMPBuilder
    {
        private ETAMPModel _model;
        private readonly IETAMPFactory<ETAMPType> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPBuilder"/> class using a factory to generate ETAMP models.
        /// </summary>
        /// <param name="factory">The factory responsible for creating ETAMP data generators.</param>
        public ETAMPBuilder(IETAMPFactory<ETAMPType> factory)
        {
            _factory = factory;
            _model = new ETAMPModel();
        }

        /// <summary>
        /// Creates a basic ETAMP model with the specified update type, payload, and version.
        /// </summary>
        /// <typeparam name="T">The type of the payload included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload data to be included in the model.</param>
        /// <param name="version">The version number of the ETAMP protocol to use.</param>
        /// <returns>The <see cref="ETAMPBuilder"/> instance for chaining further configuration calls.</returns>
        public virtual ETAMPBuilder CreateETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            var generator = _factory.CreateGenerator(ETAMPType.Base) as IETAMPBase;
            _model = generator?.CreateETAMPModel(updateType, payload, version);
            return this;
        }

        /// <summary>
        /// Creates a signed ETAMP model using the specified update type, payload, version, and a signing wrapper.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier.</param>
        /// <param name="payload">The payload data.</param>
        /// <param name="version">The protocol version.</param>
        /// <returns>The builder instance for chaining.</returns>
        public virtual ETAMPBuilder CreateSignETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            var generator = _factory.CreateGenerator(ETAMPType.Sign) as ETAMPSign;
            _model = generator?.CreateETAMPModel(updateType, payload, version);
            return this;
        }

        /// <summary>
        /// Creates an encrypted ETAMP model for enhanced security, specifying the update type, payload, version, and encryption service.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier.</param>
        /// <param name="payload">The payload data.</param>
        /// <param name="version">The protocol version.</param>
        /// <returns>The builder instance for chaining.</returns>
        public virtual ETAMPBuilder CreateEncryptedETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            var generator = _factory.CreateGenerator(ETAMPType.Sign) as IETAMPEncrypted;
            _model = generator?.CreateEncryptETAMPModel(updateType, payload, version);
            return this;
        }

        /// <summary>
        /// Creates a model that is both encrypted and signed, using provided services for encryption and signing, along with update type, payload, and version.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier.</param>
        /// <param name="payload">The payload data.</param>
        /// <param name="version">The protocol version.</param>
        /// <returns>The builder instance for chaining.</returns>
        public virtual ETAMPBuilder CreateEncryptedSignETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            var generator = _factory.CreateGenerator(ETAMPType.Sign) as ETAMPEncryptedSigned;
            _model = generator?.CreateEncryptETAMPModel(updateType, payload, version);
            return this;
        }

        /// <summary>
        /// Finalizes the building process and returns the configured ETAMP model.
        /// </summary>
        /// <returns>The configured ETAMP model.</returns>
        public virtual ETAMPModel Build()
        {
            return _model;
        }
    }
}