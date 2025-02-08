using System.IO.Pipelines;

namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Defines an interface for encryption and decryption services.
///     Provides methods for transforming data streams securely using a given cryptographic key.
/// </summary>
public interface IEncryptionService
{
    Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, byte[] key, CancellationToken cancellationToken);

    Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, byte[] key, CancellationToken cancellationToken);
}