using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPManagment
{
    /// <summary>
    /// Builds ETAMP (Encrypted Token And Message Protocol) models based on specified parameters and types.
    /// </summary>
    public class ETAMPBuilder : IETAMPBuilder<ETAMPType>
    {
        private ETAMPModel? _model;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the ETAMPBuilder with a service provider for obtaining ETAMP services.
        /// </summary>
        /// <param name="serviceProvider">The service provider for resolving ETAMP related services.</param>
        public ETAMPBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider
                ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Finalizes the building process and returns the constructed ETAMP model.
        /// </summary>
        /// <returns>The built ETAMP model.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the ETAMP model has not been created.</exception>
        public ETAMPModel Build()
        {
            return _model ?? throw new InvalidOperationException("ETAMP model has not been created.");
        }

        /// <summary>
        /// Begins the creation of an ETAMP model based on the specified type and details.
        /// </summary>
        /// <param name="type">The type of ETAMP to create.</param>
        /// <param name="updateType">The update type of the ETAMP model.</param>
        /// <param name="payload">The payload of the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol.</param>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <returns>The builder instance for fluent configuration.</returns>
        /// <exception cref="ArgumentException">Thrown if the ETAMP type is unsupported.</exception>
        public virtual IETAMPBuilder<ETAMPType> CreateETAMP<T>(ETAMPType type, string updateType, T payload, double version = 1) where T : BasePayload
        {
            switch (type)
            {
                case ETAMPType.Base:
                    IETAMPBase etampBase = _serviceProvider.GetRequiredService<IETAMPBase>();
                    ArgumentNullException.ThrowIfNull(etampBase);
                    _model = etampBase.CreateETAMPModel(updateType, payload, version);
                    break;

                case ETAMPType.Encrypted:
                    IETAMPEncrypted encrypted = _serviceProvider.GetRequiredService<IETAMPEncrypted>();
                    ArgumentNullException.ThrowIfNull(encrypted);
                    _model = encrypted.CreateEncryptETAMPModel(updateType, payload, version);

                    break;

                default:
                    throw new ArgumentException($"Unsupported ETAMP type: {type}");
            }
            return this;
        }
    }
}