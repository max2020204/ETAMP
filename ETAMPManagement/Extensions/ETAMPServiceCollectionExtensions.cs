using ETAMPManagement.Codec;
using ETAMPManagement.Encryption;
using ETAMPManagement.Encryption.Base;
using ETAMPManagement.Encryption.ECDsaManager;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.ETAMP;
using ETAMPManagement.ETAMP.Interfaces;
using ETAMPManagement.Factory;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Helper;
using ETAMPManagement.Validators;
using ETAMPManagement.Validators.Base;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper;
using ETAMPManagement.Wrapper.Base;
using ETAMPManagement.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
    public static void AddETAMPServices(this IServiceCollection services)
    {
        // Register key management services
        services.AddTransient<IKeyExchanger, KeyExchanger>();
        services.AddTransient<KeyExchangerBase, KeyExchanger>();
        services.AddTransient<IKeyPairProvider, KeyPairProvider>();
        services.AddTransient<KeyPairProviderBase, KeyPairProvider>();

        // Register cryptographic services
        services.AddScoped<IEncryptionService, AESEncryptionService>();
        services.AddScoped<IECIESEncryptionService, ECIESEncryptionService>();
        services.AddScoped<ECIESEncryptionServiceBase, ECIESEncryptionService>();

        // Register wrapper services for cryptographic operations
        services.AddScoped<IECDsaRegistrar, ECDsaProvider>();
        services.AddScoped<IECDsaProvider, ECDsaProvider>();
        services.AddScoped<IECDsaCreator, ECDsaCreator>();
        services.AddScoped<IECDsaKeyManager, ECDsaKeyManager>();
        services.AddScoped<IPemKeyCleaner, PemKeyCleaner>();

        // Register ETAMP processing services
        services.AddScoped<IETAMPBase, ETAMPBase>();


        // Register compression services
        services.AddScoped<DeflateCompressionService>();
        services.AddScoped<GZipCompressionService>();

        //Factory
        services.AddScoped<ICompressionServiceFactory, CompressionServiceFactory>();
        services.AddScoped<IKeyPairProviderFactory, KeyPairProviderFactory>();

        // Register signing and validation services
        services.AddScoped<IETAMPValidator, ETAMPValidator>();
        services.AddScoped<ETAMPValidatorBase, ETAMPValidator>();
        services.AddScoped<ISignatureValidator, SignatureValidator>();
        services.AddScoped<SignatureValidatorBase, SignatureValidator>();
        services.AddScoped<IStructureValidator, StructureValidator>();
        services.AddScoped<ITokenValidator, TokenValidator>();


        services.AddScoped<ISignWrapper, SignWrapper>();
        services.AddScoped<SignWrapperBase, SignWrapper>();
        services.AddScoped<IVerifyWrapper, VerifyWrapper>();
        services.AddScoped<VerifyWrapperBase, VerifyWrapper>();

        services.AddSingleton<VersionInfo>(_ =>
        {
            var versionInfo = new VersionInfo();
            versionInfo.GetVersionInfo();
            return versionInfo;
        });
    }
}