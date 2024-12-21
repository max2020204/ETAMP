using ETAMP.Core.Models;

namespace ETAMP.Validation.Interfaces;

/// <summary>
///     Interface for validating signatures in ETAMP messages.
/// </summary>
public interface ISignatureValidator
{
    /// <summary>
    ///     Validates the ETAMP (Encrypted Token And Message Protocol) message.
    /// </summary>
    /// <typeparam name="T">The type of the token in the ETAMPModel.</typeparam>
    /// <param name="etamp">The ETAMPModel object to be validated.</param>
    /// <returns>True if the ETAMP message is valid; otherwise, false.</returns>
    ValidationResult ValidateETAMPMessage<T>(ETAMPModel<T> etamp) where T : Token;
}