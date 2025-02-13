using ETAMP.Compression;
using ETAMP.Compression.Codec;
using ETAMP.Compression.Factory;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Factories;
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
using Serilog;

namespace ETAMP.Extension.ServiceCollection;

/// <summary>
///     Provides extension methods to register ETAMP services into an instance of
///     <see cref="IServiceCollection" />. It includes methods for adding services
///     related to logging, validation, composition, encryption, and wrappers.
/// </summary>
public static class ETAMPServiceCollectionExtensions
{
    /// <summary>
    ///     Registers the ETAMP services, which include logging, base services,
    ///     composition services, encryption services, validation services,
    ///     and wrapper services, into the provided <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> to which the services will be added.
    /// </param>
    /// <param name="addlogger">
    ///     Specifies whether to enable logging services. Defaults to true.
    /// </param>
    /// <param name="configureLogging">
    ///     An optional action to configure logging using <see cref="ILoggingBuilder" />.
    ///     If not provided, the default logging configuration is applied.
    /// </param>
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

    /// <summary>
    ///     Registers validation services for ETAMP into the specified <see cref="IServiceCollection" />.
    ///     These services include structure validation, token validation, signature validation,
    ///     and ETAMP-specific validation.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> to which the validation services will be added.
    /// </param>
    /// <param name="addlogger">
    ///     Specifies whether to enable logging services. Defaults to true.
    /// </param>
    /// <param name="configureLogging">
    ///     An optional action to configure logging using <see cref="ILoggingBuilder" />.
    ///     If not provided, the default logging configuration is applied.
    /// </param>
    public static void AddValidationServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<IStructureValidator, StructureValidator>();
        services.AddScoped<ITokenValidator, TokenValidator>();
        services.AddScoped<ISignatureValidator, SignatureValidator>();
        services.AddScoped<IETAMPValidator, ETAMPValidator>();
    }

    /// <summary>
    ///     Registers the composition-related services, including compression services
    ///     and the compression service factory, into the provided <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> to which the composition services will be added.
    /// </param>
    /// <param name="addlogger">
    ///     Specifies whether to enable logging services. Defaults to true.
    /// </param>
    /// <param name="configureLogging">
    ///     An optional action to configure logging using <see cref="ILoggingBuilder" />.
    ///     If not provided, the default logging configuration is applied.
    /// </param>
    public static void AddCompositionServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<DeflateCompressionService>();
        services.AddScoped<GZipCompressionService>();
        services.AddScoped<ICompressionServiceFactory, CompressionServiceFactory>();
        services.AddScoped<ICompressionManager, CompressionManager>();
    }

    /// <summary>
    ///     Registers wrapper services, including signing and verification functionalities,
    ///     into the provided <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> to which the wrapper services will be added.
    /// </param>
    /// <param name="addlogger">
    ///     Specifies whether to enable logging services. Defaults to true.
    /// </param>
    /// <param name="configureLogging">
    ///     An optional action to configure logging using <see cref="ILoggingBuilder" />.
    ///     If not provided, the default logging configuration is applied.
    /// </param>
    public static void AddWrapperServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<ISignWrapper, SignWrapper>();
        services.AddScoped<IVerifyWrapper, VerifyWrapper>();
    }

    /// <summary>
    ///     Registers the encryption services, including AES encryption services,
    ///     ECIES encryption services, and ECDSA store, into the provided
    ///     <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> to which the services will be added.
    /// </param>
    /// <param name="addlogger">
    ///     Specifies whether to enable logging services. Defaults to true.
    /// </param>
    /// <param name="configureLogging">
    ///     An optional action to configure logging using <see cref="ILoggingBuilder" />.
    ///     If not provided, the default logging configuration is applied.
    /// </param>
    public static void AddEncryptionServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<IEncryptionService, AESEncryptionService>();
        services.AddScoped<IECIESEncryptionService, ECIESEncryptionService>();
        services.AddSingleton<IECDSAStore, ECDSAStore>();
    }

    /// <summary>
    ///     Adds the base services required for the ETAMP framework, including logging support,
    ///     the ETAMP protocol service, and version information service, into the provided
    ///     <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> to which the base services will be added.
    /// </param>
    /// <param name="addlogger">
    ///     Specifies whether to enable logging services. Defaults to true.
    /// </param>
    /// <param name="configureLogging">
    ///     An optional action to configure logging using <see cref="ILoggingBuilder" />.
    ///     If not provided, the default logging configuration is applied.
    /// </param>
    public static void AddBaseServices(this IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        AddLogging(services, addlogger, configureLogging);
        services.AddScoped<IETAMPBase, ETAMPModelFactory>();
        services.AddSingleton<VersionInfo>(_ =>
        {
            var versionInfo = new VersionInfo();
            versionInfo.GetVersionInfo();
            return versionInfo;
        });
    }

    /// <summary>
    ///     Configures logging services within the specified <see cref="IServiceCollection" />.
    ///     Allows customization of logging behavior and logging providers, including
    ///     console and debug output.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection" /> to which logging services will be added.
    /// </param>
    /// <param name="addlogger">
    ///     Indicates whether logging services should be added. If set to false, no logging
    ///     services will be registered. Defaults to true.
    /// </param>
    /// <param name="configureLogging">
    ///     An optional action for customizing the logging configuration via <see cref="ILoggingBuilder" />.
    ///     When provided, this configuration overrides the default logging setup.
    /// </param>
    private static void AddLogging(IServiceCollection services, bool addlogger = true,
        Action<ILoggingBuilder>? configureLogging = null)
    {
        if (!addlogger) return;
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.Async(a => a.Console(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
            .CreateLogger();
        services.AddLogging(builder =>
        {
            if (configureLogging != null)
            {
                configureLogging(builder);
                return;
            }

            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
            configureLogging?.Invoke(builder);
        });
    }
}