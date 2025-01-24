#region

using ETAMP.Compression.Codec;
using ETAMP.Compression.Factory;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Utils;
using ETAMP.Encryption;
using ETAMP.Encryption.ECDsaManager;
using ETAMP.Encryption.Interfaces;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Validation;
using ETAMP.Validation.Interfaces;
using ETAMP.Wrapper;
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

        services.AddScoped<IECDSAControl, ECDSAControl>();
        services.AddSingleton<IECDSAStore, ECDSAStore>();
        services.AddScoped<IPemKeyCleaner, PemKeyCleaner>();

        services.AddScoped<IETAMPBase, ETAMPProtocol>();

        services.AddScoped<DeflateCompressionService>();
        services.AddScoped<GZipCompressionService>();

        services.AddScoped<ICompressionServiceFactory, CompressionServiceFactory>();

        services.AddScoped<ISignWrapper, SignWrapper>();
        services.AddScoped<IVerifyWrapper, VerifyWrapper>();


        services.AddScoped<IStructureValidator, StructureValidator>();
        services.AddScoped<ITokenValidator, TokenValidator>();
        services.AddScoped<ISignatureValidator, SignatureValidator>();
        services.AddScoped<IETAMPValidator, ETAMPValidator>();


        services.AddSingleton<VersionInfo>(_ =>
        {
            var versionInfo = new VersionInfo();
            versionInfo.GetVersionInfo();
            return versionInfo;
        });
    }
}