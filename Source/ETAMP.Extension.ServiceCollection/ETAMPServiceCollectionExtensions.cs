#region

using ETAMP.Compression.Codec;
using ETAMP.Compression.Factory;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Utils;
using ETAMP.Encryption;
using ETAMP.Encryption.Base;
using ETAMP.Encryption.ECDsaManager;
using ETAMP.Encryption.Interfaces;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Validation;
using ETAMP.Validation.Base;
using ETAMP.Validation.Interfaces;
using ETAMP.Wrapper;
using ETAMP.Wrapper.Base;
using ETAMP.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ETAMP.Extension.ServiceCollection;

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
        // Register cryptographic services
        services.AddScoped<IEncryptionService, AESEncryptionService>();
        services.AddScoped<IECIESEncryptionService, ECIESEncryptionService>();
        services.AddScoped<ECIESEncryptionServiceBase, ECIESEncryptionService>();

        // Register wrapper services for cryptographic operations
        services.AddScoped<IECDSAControl, ECDSAControl>();
        services.AddSingleton<IECDSAStore, ECDSAStore>();
        services.AddScoped<IPemKeyCleaner, PemKeyCleaner>();

        // Register ETAMP processing services
        services.AddScoped<IETAMPBase, ETAMPProtocol>();


        // Register compression services
        services.AddScoped<DeflateCompressionService>();
        services.AddScoped<GZipCompressionService>();

        //Factory
        services.AddScoped<ICompressionServiceFactory, CompressionServiceFactory>();

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