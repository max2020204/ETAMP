using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Interfaces;
using ETAMPManagment.Managment;
using ETAMPManagment.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPManagment
{
    /// <summary>
    /// Builds ETAMP (Encrypted Token And Message Protocol) models based on specified parameters and types.
    /// This builder facilitates the creation of various ETAMP model types, leveraging dependency injection
    /// to obtain the specific services needed for the model construction.
    /// </summary>
    public class ETAMPBuilder : IETAMPBuilder<string>
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
        /// Starts the creation process of an ETAMP model based on the specified type and details.
        /// This method configures the builder with the type-specific settings required to build the ETAMP model.
        /// </summary>
        /// <param name="type">The type of ETAMP to create, determining the specific ETAMP service to use.</param>
        /// <param name="updateType">The update type of the ETAMP model, defining how the model will be updated.</param>
        /// <param name="payload">The data payload of the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol to be used (default is 1).</param>
        /// <typeparam name="T">The type of the payload, must inherit from BasePayload.</typeparam>
        /// <returns>The builder instance for fluent configuration, allowing additional modifications.</returns>
        /// <exception cref="ArgumentException">Thrown if the specified ETAMP type is unsupported or invalid.</exception>
        public virtual IETAMPBuilder<string> CreateETAMP<T>(string type, string updateType, T payload, double version = 1) where T : BasePayload
        {
            switch (type)
            {
                case ETAMPTypeNames.Base:
                    IETAMPBase etampBase = _serviceProvider.GetRequiredService<IETAMPBase>();
                    ArgumentNullException.ThrowIfNull(etampBase);
                    _model = etampBase.CreateETAMPModel(updateType, payload, version);
                    break;

                case ETAMPTypeNames.Encrypted:
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