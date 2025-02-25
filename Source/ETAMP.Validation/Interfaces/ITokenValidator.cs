using ETAMP.Core.Models;

namespace ETAMP.Validation.Interfaces;

/// <summary>
///     Represents a token validator that validates tokens in the ETAMP system.
/// </summary>
public interface ITokenValidator
{
    /// <summary>
    ///     Validates the token of the given ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of token.</typeparam>
    /// <param name="model">The ETAMP model to validate.</param>
    /// <returns>A ValidationResult object indicating the result of the token validation.</returns>
    ValidationResult ValidateToken<T>(ETAMPModel<T> model) where T : Token;
}