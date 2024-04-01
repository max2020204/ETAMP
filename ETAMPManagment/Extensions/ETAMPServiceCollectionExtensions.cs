using ETAMPManagment.Encryption;
using ETAMPManagment.Encryption.ECDsaManager;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Interfaces;
using ETAMPManagment.Services;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Utils;
using ETAMPManagment.Validators;
using ETAMPManagment.Validators.Interfaces;
using ETAMPManagment.Wrapper;
using ETAMPManagment.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPManagment.Extensions
{
    /// <summary>
    /// Extension methods for setting up ETAMP services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ETAMPServiceCollectionExtensions
    {
        /// <summary>
        /// Adds ETAMP services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The original <see cref="IServiceCollection"/> with ETAMP services added.</returns>
        public static IServiceCollection AddETAMPServices(this IServiceCollection services)
        {
            // Register key management services
            services.AddTransient<IKeyExchanger, KeyExchanger>();
            services.AddTransient<IKeyPairProvider, KeyPairProvider>();

            // Register cryptographic services
            services.AddScoped<IAesEncryptionService, AesEncryptionService>();
            services.AddScoped<IEciesEncryptionService, EciesEncryptionService>();

            // Register wrapper services for cryptographic operations
            services.AddScoped<IEcdsaCreator, EcdsaCreator>();
            services.AddScoped<IEcdsaKeyManager, EcdsaKeyManager>();
            services.AddScoped<IECDsaProvider, ECDsaProvider>();
            services.AddScoped<IECDsaRegistrar, ECDsaProvider>();
            services.AddScoped<IPemKeyCleaner, PemKeyCleaner>();

            // Register ETAMP processing services
            services.AddScoped<IETAMPBase, ETAMPBase>();
            services.AddScoped<IETAMPData, ETAMPData>();
            services.AddScoped<ETAMPSign>();

            // Register token and encryption services
            services.AddScoped<IEncryptToken, EncryptToken>();
            services.AddScoped<IETAMPEncrypted, ETAMPEncrypted>();
            services.AddScoped<ETAMPEncryptedSigned>();

            // Register compression services
            services.AddScoped<ICompressionService, DeflateCompressionService>();
            services.AddScoped<ICompressionService, GZipCompressionService>();

            // Register signing and validation services
            services.AddTransient<ISigningCredentialsProvider, ECDsaSigningCredentialsProvider>();
            services.AddScoped<IETAMPValidator, ETAMPValidator>();
            services.AddScoped<IJwtValidator, JwtValidator>();
            services.AddScoped<ISignatureValidator, SignatureValidator>();
            services.AddScoped<IStructureValidator, StructureValidator>();

            services.AddScoped<ISignWrapper, SignWrapper>();
            services.AddScoped<IVerifyWrapper, VerifyWrapper>();

            // Register the ETAMP builder service
            services.AddScoped<IETAMPBuilder<ETAMPType>, ETAMPBuilder>();

            return services;
        }
    }
}