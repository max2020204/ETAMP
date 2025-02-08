using ETAMP.Core.Models;

namespace ETAMP.Compression.Interfaces;

public interface ICompressionManager
{
    Task<ETAMPModelBuilder> CompressAsync<T>(ETAMPModel<T> model, CancellationToken cancellationToken = default)
        where T : Token;

    Task<ETAMPModel<T>> DecompressAsync<T>(ETAMPModelBuilder model, CancellationToken cancellationToken = default)
        where T : Token;
}