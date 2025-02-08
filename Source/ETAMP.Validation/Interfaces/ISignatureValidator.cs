using ETAMP.Core.Interfaces;
using ETAMP.Core.Models;

namespace ETAMP.Validation.Interfaces;

/// <summary>
///     Interface for validating signatures in ETAMP messages.
/// </summary>
public interface ISignatureValidator : IInitialize
{
    /// <summary>
    /// Asynchronously validates the signature of an ETAMP message.
    /// </summary>
    /// <typeparam name="T">The type of token associated with the ETAMP message.</typeparam>
    /// <param name="etamp">The ETAMP model containing the message and signature to validate.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation if needed. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="ValidationResult"/> that indicates whether the validation was successful or not.</returns>
    Task<ValidationResult> ValidateETAMPMessageAsync<T>(ETAMPModel<T> etamp,
        CancellationToken cancellationToken = default) where T : Token;
}