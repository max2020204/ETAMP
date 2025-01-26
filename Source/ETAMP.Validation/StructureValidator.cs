#region

using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Validation.Interfaces;
using Microsoft.Extensions.Logging;

#endregion

namespace ETAMP.Validation;

/// <summary>
///     Provides functionality to validate the structure of an ETAMP model.
/// </summary>
public sealed class StructureValidator : IStructureValidator
{
    private readonly ICompressionServiceFactory _compressionServiceFactory;
    private readonly ILogger<StructureValidator> _logger;

    public StructureValidator(ICompressionServiceFactory compressionServiceFactory, ILogger<StructureValidator> logger)
    {
        _compressionServiceFactory = compressionServiceFactory;
        _logger = logger;
    }

    /// <summary>
    ///     Validates an ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of token.</typeparam>
    /// <param name="model">The ETAMP model to validate.</param>
    /// <param name="validateLite">Optional. Specifies whether to perform lite validation. Default is false.</param>
    /// <returns>A ValidationResult object indicating whether the model is valid or not.</returns>
    public ValidationResult ValidateETAMP<T>(ETAMPModel<T> model, bool validateLite = false) where T : Token
    {
        if (EqualityComparer<ETAMPModel<T>>.Default.Equals(model, default))
        {
            _logger.LogError("ETAMP model is uninitialized (default).");
            return new ValidationResult(false, "ETAMP model is uninitialized (default).");
        }

        if (model.Id != Guid.Empty &&
            !string.IsNullOrWhiteSpace(model.UpdateType) &&
            !string.IsNullOrWhiteSpace(model.CompressionType) &&
            model.Token != null &&
            (validateLite || !string.IsNullOrWhiteSpace(model.SignatureMessage)))
            return new ValidationResult(true);

        _logger.LogError("ETAMP model has empty/missing fields or contains invalid values.");
        return new ValidationResult(false, "ETAMP model has empty/missing fields or contains invalid values.");
    }


    /// <summary>
    /// Asynchronously validates an ETAMP model from its JSON representation.
    /// </summary>
    /// <typeparam name="T">The type of token.</typeparam>
    /// <param name="etampJson">The JSON representation of the ETAMP model to validate.</param>
    /// <param name="validateLite">Optional. Specifies whether to perform lite validation. Default is false.</param>
    /// <param name="cancellationToken">Optional. A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A Task that represents the asynchronous operation, containing a ValidationResult object indicating whether the model is valid or not.</returns>
    public async Task<ValidationResult> ValidateETAMPAsync<T>(string etampJson, bool validateLite = false,
        CancellationToken cancellationToken = default)
        where T : Token
    {
        var model = await etampJson.DeconstructETAMPAsync<T>(_compressionServiceFactory, cancellationToken);
        return ValidateETAMP(model, validateLite);
    }
}