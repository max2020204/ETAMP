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
using Microsoft.Extensions.Logging;

#endregion

namespace ETAMP.Extension.ServiceCollection;

public static class ETAMPServiceCollectionExtensions
{
    public static void AddETAMPServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        AddBaseServices(services, false);
        AddCompositionServices(services, false);
        AddEncryptionServices(services, false);
        AddValidationServices(services, false);
        AddWrapperServices(services, false);
    }

    public static void AddValidationServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<IStructureValidator, StructureValidator>();
        services.AddScoped<ITokenValidator, TokenValidator>();
        services.AddScoped<ISignatureValidator, SignatureValidator>();
        services.AddScoped<IETAMPValidator, ETAMPValidator>();
    }

    public static void AddCompositionServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<DeflateCompressionService>();
        services.AddScoped<GZipCompressionService>();
        services.AddScoped<ICompressionServiceFactory, CompressionServiceFactory>();
    }

    public static void AddWrapperServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<ISignWrapper, SignWrapper>();
        services.AddScoped<IVerifyWrapper, VerifyWrapper>();
    }

    public static void AddEncryptionServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<IEncryptionService, AESEncryptionService>();
        services.AddScoped<IECIESEncryptionService, ECIESEncryptionService>();
        services.AddScoped<IECDSAControl, ECDSAControl>();
        services.AddSingleton<IECDSAStore, ECDSAStore>();
        services.AddScoped<IPemKeyCleaner, PemKeyCleaner>();
    }

    public static void AddBaseServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<IETAMPBase, ETAMPProtocol>();
        services.AddSingleton<VersionInfo>(_ =>
        {
            var versionInfo = new VersionInfo();
            versionInfo.GetVersionInfo();
            return versionInfo;
        });
    }

    private static void AddLogging(IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        if (addlogger) services.AddLogging(configureLogging);
    }
}