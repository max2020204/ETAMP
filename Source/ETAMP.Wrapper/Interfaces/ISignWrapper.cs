using ETAMP.Core.Interfaces;
using ETAMP.Core.Models;

namespace ETAMP.Wrapper.Interfaces;

/// <summary>
///     Provides functionality for signing ETAMP messages.
/// </summary>
public interface ISignWrapper : IInitialize
{
    /// <summary>
    /// Signs an ETAMP model using the provided signing implementation.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the token contained within the ETAMP model. Must inherit from <see cref="Token"/>.
    /// </typeparam>
    /// <param name="etamp">
    /// The ETAMP model to be signed. This model includes the token and associated message data.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to signal the cancellation of the asynchronous operation. Defaults to <see cref="CancellationToken.None"/> if none is provided.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the signed <see cref="ETAMPModel{T}"/> object.
    /// </returns>
    Task<ETAMPModel<T>> SignEtampModel<T>(ETAMPModel<T> etamp, CancellationToken cancellationToken = default)
        where T : Token;
}