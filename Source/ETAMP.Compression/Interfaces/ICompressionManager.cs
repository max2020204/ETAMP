using ETAMP.Core.Models;

namespace ETAMP.Compression.Interfaces;

/// <summary>
///     Interface for managing compression and decompression operations.
///     Provides methods to compress and decompress models.
/// </summary>
public interface ICompressionManager
{
    /// <summary>
    ///     Compresses the specified ETAMP model asynchronously and returns a corresponding ETAMPModelBuilder instance.
    /// </summary>
    /// <typeparam name="T">The type of token contained in the ETAMP model, which must inherit from <see cref="Token" />.</typeparam>
    /// <param name="model">The instance of <see cref="ETAMPModel{T}" /> to be compressed.</param>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None" /> if not provided.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an <see cref="ETAMPModelBuilder" />
    ///     representing the compressed model.
    /// </returns>
    Task<ETAMPModelBuilder> CompressAsync<T>(ETAMPModel<T> model, CancellationToken cancellationToken = default)
        where T : Token;

    /// <summary>
    ///     Converts a compressed ETAMPModelBuilder back into an ETAMPModel of a specified token type.
    /// </summary>
    /// <typeparam name="T">The type of token associated with the ETAMP model, constrained to inherit from Token.</typeparam>
    /// <param name="model">The builder representation of the compressed model to be decompressed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>Returns the decompressed ETAMPModel of the specified token type.</returns>
    Task<ETAMPModel<T>> DecompressAsync<T>(ETAMPModelBuilder model, CancellationToken cancellationToken = default)
        where T : Token;
}