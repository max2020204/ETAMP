using System.Security.Cryptography;
using System.Text;
using ETAMP.Core.Extensions;
using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;
using ETAMP.Wrapper.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Validation;

/// <summary>
/// Provides functionality to validate signatures in ETAMP messages.
/// </summary>
public sealed class SignatureValidator : ISignatureValidator
{
    private readonly IECDsaVerificationProvider _iecDsaVerificationProvider;
    private readonly ILogger<SignatureValidator> _logger;
    private readonly IStructureValidator _structureValidator;
    private ECDsa? _ecdsa;


    /// <summary>
    /// Provides functionality to validate signatures in ETAMP messages.
    /// </summary>
    public SignatureValidator(IECDsaVerificationProvider iecDsaVerificationProvider,
        IStructureValidator structureValidator,
        ILogger<SignatureValidator> logger)
    {
        _iecDsaVerificationProvider = iecDsaVerificationProvider;
        _structureValidator = structureValidator
                              ?? throw new ArgumentNullException(nameof(structureValidator));
        _logger = logger;
    }


    /// <summary>
    /// Asynchronously validates the ETAMP message by checking the structure of the ETAMP model,
    /// verifying the presence of required fields, and validating the signature of the message.
    /// </summary>
    /// <typeparam name="T">The type of the token associated with the ETAMP model, which must derive from the <see cref="Token"/> class.</typeparam>
    /// <param name="etamp">The ETAMP model containing the token, signature message, and related data to be validated.</param>
    /// <param name="cancellationToken">An optional token to observe while waiting for the task to complete. Defaults to <see cref="default"/>.</param>
    /// <returns>A task that represents the asynchronous validation operation. The task result is a <see cref="ValidationResult"/> indicating whether the validation succeeded or failed, along with error details if applicable.</returns>
    public async Task<ValidationResult> ValidateETAMPMessageAsync<T>(ETAMPModel<T> etamp,
        CancellationToken cancellationToken = default) where T : Token
    {
        ArgumentNullException.ThrowIfNull(etamp.Token);
        var structureValidationResult = _structureValidator.ValidateETAMP(etamp);
        if (!structureValidationResult.IsValid)
        {
            _logger.LogError("Invalid ETAMP Model.");
            return new ValidationResult(false, "Invalid ETAMP Model.");
        }

        if (string.IsNullOrWhiteSpace(etamp.SignatureMessage))
        {
            _logger.LogError("SignatureMessage is missing in the ETAMP model.");
            return new ValidationResult(false, "SignatureMessage is missing in the ETAMP model.");
        }

        await using (var stream = new MemoryStream())
        {
            await using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                await writer.WriteAsync(etamp.Id.ToString());
                await writer.WriteAsync(etamp.Version.ToString());
                await writer.WriteAsync(await etamp.Token.ToJsonAsync());
                await writer.WriteAsync(etamp.UpdateType);
                await writer.WriteAsync(etamp.CompressionType);
            }

            var isVerified = _iecDsaVerificationProvider.VerifyData(stream, etamp.SignatureMessage);

            if (isVerified)
                return new ValidationResult(true);
        }

        _logger.LogError("Failed to verify data.");
        return new ValidationResult(false, "Failed to verify data.");
    }

    /// <summary>
    ///     Initializes the SignatureValidator by assigning a cryptographic provider and algorithm.
    /// </summary>
    /// <param name="provider">
    ///     The ECDsa cryptographic provider used for signature validation.
    /// </param>
    /// <param name="algorithmName">
    ///     The hash algorithm name to be used during the validation process.
    /// </param>
    public void Initialize(ECDsa? provider, HashAlgorithmName algorithmName)
    {
        _ecdsa = provider;
        _iecDsaVerificationProvider.Initialize(provider, algorithmName);
    }

    /// <summary>
    ///     Disposes the resources used by the SignatureValidator instance, including the verify wrapper
    ///     and elliptic curve digital signature algorithm (ECDsa) resources.
    /// </summary>
    public void Dispose()
    {
        _iecDsaVerificationProvider.Dispose();
        _ecdsa?.Dispose();
    }
}