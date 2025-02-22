using System.Security.Cryptography;
using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;
using Microsoft.Extensions.Logging;
using ValidationResult = ETAMP.Core.Models.ValidationResult;


namespace ETAMP.Validation;

/// <summary>
///     Provides validation services for the ETAMP model including structure validation,
///     token validation, and signature validation.
/// </summary>
/// <remarks>
///     This class implements the IETAMPValidator interface and encapsulates
///     the process of validating an ETAMP model in a sequential manner: structure validation,
///     token validation, and signature validation.
/// </remarks>
public sealed class ETAMPValidator : IETAMPValidator
{
    /// <summary>
    ///     Represents the logger instance used for logging messages within the ETAMPValidator class.
    /// </summary>
    /// <remarks>
    ///     This logger is used to log various information, warnings, and error messages during the
    ///     validation process of ETAMP models, including token validation, structure validation,
    ///     and signature validation. It aids in monitoring and troubleshooting the validation workflow.
    /// </remarks>
    private readonly ILogger<ETAMPValidator> _logger;

    /// <summary>
    ///     Represents a signature validator dependency used for validating the signatures
    ///     in ETAMP messages. This variable is an instance of the <see cref="ISignatureValidator" /> interface.
    /// </summary>
    /// <remarks>
    ///     The <c>_signature</c> field is utilized in methods such as <see cref="ValidateETAMPAsync{T}" />
    ///     for signature validation and <see cref="Initialize(ECDsa, HashAlgorithmName)" /> for initializing
    ///     the signature validator with specific cryptographic settings. Additionally, it is disposed of
    ///     in the <see cref="Dispose" /> method to release any unmanaged resources.
    /// </remarks>
    private readonly ISignatureValidator _signature;

    /// <summary>
    ///     A private read-only instance of the IStructureValidator interface used to validate the structure
    ///     and consistency of ETAMP models and tokens within the ETAMPValidator.
    /// </summary>
    private readonly IStructureValidator _structureValidator;

    /// <summary>
    ///     Represents the token validator dependency used within the ETAMPValidator for validating tokens.
    /// </summary>
    /// <remarks>
    ///     An instance of this is injected into the ETAMPValidator to handle the validation logic for token validation.
    ///     It implements the ITokenValidator interface, ensuring adherence to a contract for token validation operations.
    /// </remarks>
    private readonly ITokenValidator _tokenValidator;

    /// <summary>
    ///     ETAMPValidator provides the functionality to validate ETAMP models, tokens, and their associated signatures
    ///     using the injected validators and logger. Implements the IETAMPValidator interface along with IDisposable and
    ///     IInitialize.
    /// </summary>
    public ETAMPValidator(ITokenValidator tokenValidator, IStructureValidator structureValidator,
        ISignatureValidator signature, ILogger<ETAMPValidator> logger)
    {
        _tokenValidator = tokenValidator;
        _structureValidator = structureValidator;
        _signature = signature;
        _logger = logger;
    }


    /// <summary>
    ///     Asynchronously validates an ETAMP model, including its structure, token, and associated signature.
    ///     Combines multiple validation steps to ensure all parts of the ETAMP model are valid before returning
    ///     the overall validation result.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the token associated with the ETAMP model. Must inherit from the <see cref="Token" /> class.
    /// </typeparam>
    /// <param name="etamp">The ETAMP model to be validated, which contains the necessary data and token.</param>
    /// <param name="validateLite">
    ///     A boolean value indicating whether a lightweight validation should be performed. When set to true,
    ///     fewer validation checks may be conducted.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests. Allows the asynchronous operation to be canceled if requested.
    /// </param>
    /// <returns>
    ///     A <see cref="ValidationResult" /> instance representing the outcome of the validation. Contains details about
    ///     whether the ETAMP model is valid and any associated error messages.
    /// </returns>
    public async Task<ValidationResult> ValidateETAMPAsync<T>(ETAMPModel<T> etamp, bool validateLite,
        CancellationToken cancellationToken = default) where T : Token
    {
        var structureValidationResult = _structureValidator.ValidateETAMP(etamp, validateLite);
        if (!structureValidationResult.IsValid)
        {
            _logger.LogError(structureValidationResult.ErrorMessage);
            return structureValidationResult;
        }

        var tokenValidationResult = _tokenValidator.ValidateToken(etamp);
        if (!tokenValidationResult.IsValid)
        {
            _logger.LogError(tokenValidationResult.ErrorMessage);
            return tokenValidationResult;
        }


        var signatureValidationResult = await _signature.ValidateETAMPMessageAsync(etamp, cancellationToken);
        _logger.LogInformation(signatureValidationResult.IsValid ? "Signature is valid" : "Signature is invalid");

        return !signatureValidationResult.IsValid
            ? signatureValidationResult
            : new ValidationResult(true);
    }

    /// <summary>
    ///     Releases all resources used by the ETAMPValidator instance.
    /// </summary>
    /// <remarks>
    ///     This method should be called when the validator is no longer needed, to free up resources such as logging and
    ///     signature validation dependencies.
    ///     It disposes of any resources held by the internal ISignatureValidator and performs necessary cleanup operations.
    /// </remarks>
    public void Dispose()
    {
        _signature.Dispose();
    }

    /// <summary>
    ///     Initializes the signature validator with the specified cryptographic provider
    ///     and hashing algorithm.
    /// </summary>
    /// <param name="provider">The ECDsa provider to use for cryptographic operations.</param>
    /// <param name="algorithmName">The hash algorithm name to be used for signing or verification.</param>
    public void Initialize(ECDsa? provider, HashAlgorithmName algorithmName)
    {
        _signature.Initialize(provider, algorithmName);
    }
}