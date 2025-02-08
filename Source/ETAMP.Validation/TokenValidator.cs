using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Validation;

/// <summary>
///     Represents a token validator.
/// </summary>
public class TokenValidator : ITokenValidator
{
    private readonly ILogger<TokenValidator> _logger;

    public TokenValidator(ILogger<TokenValidator> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Validates a token by performing several checks on the provided model.
    /// </summary>
    /// <typeparam name="T">The type of token to validate.</typeparam>
    /// <param name="model">The ETAMP model containing the token to validate.</param>
    /// <returns>The validation result, indicating whether the token is valid or not.</returns>
    public ValidationResult ValidateToken<T>(ETAMPModel<T> model) where T : Token
    {
        if (model.CompressionType == null)
        {
            _logger.LogError("Compression type is null");
            return new ValidationResult(false, "ETAMPBuilder type is null");
        }

        if (model.Token == null)
        {
            _logger.LogError("Token is null");
            return new ValidationResult(false, "Token is null.");
        }

        if (model.Id != model.Token.MessageId)
        {
            _logger.LogError("MessageId does not match the model Id.");
            return new ValidationResult(false, "MessageId does not match the model Id.");
        }

        if (model.Token.Id != Guid.Empty)
            return new ValidationResult(true);

        _logger.LogError("Token Id cannot be empty.");
        return new ValidationResult(false, "Token Id cannot be empty.");
    }
}