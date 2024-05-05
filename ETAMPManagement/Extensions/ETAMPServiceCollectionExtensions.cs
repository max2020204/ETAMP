#region

using ETAMPManagement.Encryption;
using ETAMPManagement.Encryption.ECDsaManager;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.ETAMP.Base;
using ETAMPManagement.ETAMP.Base.Interfaces;
using ETAMPManagement.ETAMP.Encrypted;
using ETAMPManagement.ETAMP.Encrypted.Interfaces;
using ETAMPManagement.Factory;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Services;
using ETAMPManagement.Services.Interfaces;
using ETAMPManagement.Validators;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper;
using ETAMPManagement.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ETAMPManagement.Extensions;

/// <summary>
///     Extension methods for setting up ETAMP services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ETAMPServiceCollectionExtensions
{
    /// <summary>
    ///     Adds ETAMP services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The original <see cref="IServiceCollection" /> with ETAMP services added.</returns>
    public static IServiceCollection AddETAMPServices(this IServiceCollection services)
    {
        // Register key management services
        services.AddTransient<IKeyExchanger, KeyExchanger>();
        services.AddTransient<IKeyPairProvider, KeyPairProvider>();

        // Register cryptographic services
        services.AddScoped<IEncryptionService, AesEncryptionService>();
        services.AddScoped<IEciesEncryptionService, EciesEncryptionService>();

        // Register wrapper services for cryptographic operations
        services.AddScoped<IECDsaRegistrar, ECDsaProvider>();
        services.AddScoped<IECDsaProvider, ECDsaProvider>();
        services.AddScoped<IECDsaCreator, ECDsaCreator>();
        services.AddScoped<IECDsaKeyManager, ECDsaKeyManager>();
        services.AddScoped<IPemKeyCleaner, PemKeyCleaner>();

        // Register ETAMP processing services
        services.AddScoped<IETAMPBase, ETAMPBase>();
        services.AddScoped<IETAMPData, ETAMPData>();

        // Register token and encryption services
        services.AddScoped<IEncryptToken, EncryptToken>();

        // Register compression services
        services.AddScoped<DeflateCompressionService>();
        services.AddScoped<GZipCompressionService>();

        //Factory
        services.AddScoped<ICompressionServiceFactory, CompressionServiceFactory>();
        services.AddScoped<IKeyPairProviderFactory, KeyPairProviderFactory>();

        // Register signing and validation services
        services.AddTransient<ISigningCredentialsProvider, ECDsaSigningCredentialsProvider>();
        services.AddScoped<IETAMPValidator, ETAMPValidator>();
        services.AddScoped<IJwtValidator, JwtValidator>();
        services.AddScoped<ISignatureValidator, SignatureValidator>();
        services.AddScoped<IStructureValidator, StructureValidator>();

        services.AddScoped<ISignWrapper, SignWrapper>();
        services.AddScoped<IVerifyWrapper, VerifyWrapper>();

        return services;
    }
}