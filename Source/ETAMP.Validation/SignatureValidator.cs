#region

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;
using ETAMP.Wrapper.Interfaces;
using Microsoft.Extensions.Logging;

#endregion

namespace ETAMP.Validation;

/// <summary>
/// Provides functionality to validate signatures in ETAMP messages.
/// </summary>
public sealed class SignatureValidator : ISignatureValidator
{
    /// <summary>
    /// Logs information, warnings, and errors for the signature validation process of ETAMP messages.
    /// </summary>
    private readonly ILogger<SignatureValidator> _logger;

    /// <summary>
    /// Responsible for validating the structure and consistency of ETAMP tokens and models.
    /// </summary>
    private readonly IStructureValidator _structureValidator;

    /// <summary>
    ///     Serves as a wrapper for verifying data and signatures using ECDsa.
    /// </summary>
    private readonly IVerifyWrapper _verifyWrapper;

    /// <summary>
    ///     Represents an ECDsa cryptographic provider used for signature verification of data in ETAMP messages.
    /// </summary>
    private ECDsa _ecdsa;


    /// <summary>
    /// SignatureValidator is a class that implements the ISignatureValidator interface.
    /// It validates the signatures of ETAMP messages by utilizing a structure validator,
    /// a verification wrapper, and a logging mechanism.
    /// </summary>
    public SignatureValidator(IVerifyWrapper verifyWrapper, IStructureValidator structureValidator,
        ILogger<SignatureValidator> logger)
    {
        _verifyWrapper = verifyWrapper;
        _structureValidator = structureValidator
                              ?? throw new ArgumentNullException(nameof(structureValidator));
        _logger = logger;
    }


    /// <summary>
    ///     Validates the provided ETAMP message by ensuring its structure
    ///     and signature are correct.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the token within the ETAMP model. It must derive from the <see cref="Token" /> class.
    /// </typeparam>
    /// <param name="etamp">
    ///     The ETAMP model to be validated. This includes information such as
    ///     the token, signature message, and other related fields.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="ValidationResult" /> indicating whether the
    ///     validation was successful or not, along with an appropriate message.
    /// </returns>
    public async Task<ValidationResult> ValidateETAMPMessageAsync<T>(ETAMPModel<T> etamp,
        CancellationToken cancellationToken = default) where T : Token
    {
        ArgumentNullException.ThrowIfNull(etamp);
        ArgumentNullException.ThrowIfNull(etamp.Token);
        var structureValidationResult = _structureValidator.ValidateETAMP(etamp);
        if (!structureValidationResult.IsValid)
        {
            _logger.LogError(structureValidationResult.ErrorMessage);
            return new ValidationResult(false, "Invalid ETAMP Model.");
        }

        if (string.IsNullOrWhiteSpace(etamp.SignatureMessage))
        {
            _logger.LogError("SignatureMessage is missing in the ETAMP model.");
            return new ValidationResult(false, "SignatureMessage is missing in the ETAMP model.");
        }

        string token;
        await using (var memoryStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(memoryStream, etamp.Token, cancellationToken: cancellationToken);

            token = Encoding.UTF8.GetString(memoryStream.ToArray());
        }


        var dataToVerify = $"{etamp.Id}{etamp.Version}{token}{etamp.UpdateType}{etamp.CompressionType}";
        var isVerified = _verifyWrapper.VerifyData(dataToVerify, etamp.SignatureMessage);

        if (isVerified) return new ValidationResult(true);

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
    public void Initialize(ECDsa provider, HashAlgorithmName algorithmName)
    {
        _ecdsa = provider;
        _verifyWrapper.Initialize(provider, algorithmName);
    }

    /// <summary>
    ///     Disposes the resources used by the SignatureValidator instance, including the verify wrapper
    ///     and elliptic curve digital signature algorithm (ECDsa) resources.
    /// </summary>
    public void Dispose()
    {
        _verifyWrapper.Dispose();
        _ecdsa.Dispose();
    }
}