using ETAMP.Core.Models;

namespace ETAMP.Validation.Interfaces;

/// <summary>
///     Represents a validator for the ETAMP (Encrypted Token And Message Protocol) structure.
/// </summary>
/// <remarks>
///     The ETAMPValidator can be used to validate the structure of an ETAMP model.
/// </remarks>
public interface IETAMPValidator
{
    /// <summary>
    ///     Validates the ETAMP (Encrypted Token And Message Protocol) structure.
    /// </summary>
    /// <typeparam name="T">The type of the token.</typeparam>
    /// <param name="etamp">The ETAMP model to validate.</param>
    /// <param name="validateLite">Specify whether to perform a lite validation.</param>
    /// <returns>The validation result.</returns>
    ValidationResult ValidateETAMP<T>(ETAMPModel<T> etamp, bool validateLite) where T : Token;
}