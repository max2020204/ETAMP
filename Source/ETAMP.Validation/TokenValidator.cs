#region

using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;

#endregion

namespace ETAMP.Validation;

/// <summary>
///     Represents a token validator.
/// </summary>
public class TokenValidator : ITokenValidator
{
    /// <summary>
    ///     Validates a token by performing several checks on the provided model.
    /// </summary>
    /// <typeparam name="T">The type of token to validate.</typeparam>
    /// <param name="model">The ETAMP model containing the token to validate.</param>
    /// <returns>The validation result, indicating whether the token is valid or not.</returns>
    public ValidationResult ValidateToken<T>(ETAMPModel<T> model) where T : Token
    {
        if (model.CompressionType == null)
            return new ValidationResult(false, "ETAMPBuilder type is null");

        if (model.Token == null)
            return new ValidationResult(false, "Token is null.");

        if (model.Id != model.Token.MessageId)
            return new ValidationResult(false, "MessageId does not match the model Id.");

        return model.Token.Id == Guid.Empty
            ? new ValidationResult(false, "Token Id cannot be empty.")
            : new ValidationResult(true);
    }
}