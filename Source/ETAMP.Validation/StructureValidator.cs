﻿using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Validation;

/// <summary>
///     Provides functionality to validate the structure of an ETAMP model.
/// </summary>
public sealed class StructureValidator : IStructureValidator
{
    private readonly ILogger<StructureValidator> _logger;

    public StructureValidator(ILogger<StructureValidator> logger)
    {
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

        var hasValidId = model.Id != Guid.Empty;
        var hasValidUpdateType = !string.IsNullOrWhiteSpace(model.UpdateType);
        var hasValidCompressionType = !string.IsNullOrWhiteSpace(model.CompressionType);
        var hasToken = model.Token != null;
        var hasValidSignature = !string.IsNullOrWhiteSpace(model.SignatureMessage);

        if (hasValidId && hasValidUpdateType && hasValidCompressionType && hasToken &&
            (validateLite || hasValidSignature))
            return new ValidationResult(true);


        _logger.LogError("ETAMP model has empty/missing fields or contains invalid values.");
        return new ValidationResult(false, "ETAMP model has empty/missing fields or contains invalid values.");
    }
}