using System.IO.Pipelines;

namespace ETAMP.Encryption.Interfaces;

/// <summary>
/// Represents a service for performing encryption and decryption operations.
/// Defines methods for processing data streams using a specified cryptographic algorithm and key.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    ///     Encrypts the provided input data stream using AES encryption and writes the encrypted data to the output stream.
    /// </summary>
    /// <param name="inputReader">The <c>PipeReader</c> to read the unencrypted input data from.</param>
    /// <param name="outputWriter">The <c>PipeWriter</c> to write the encrypted output data to.</param>
    /// <param name="key">The encryption key as a byte array. The key must be 128, 192, or 256 bits in length.</param>
    /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <c>Task</c> that represents the asynchronous operation.</returns>
    Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, byte[] key, CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously decrypts data from the input stream and writes the decrypted data to the output stream using the
    ///     specified cryptographic key.
    /// </summary>
    /// <param name="inputReader">The PipeReader instance from which the encrypted data will be read.</param>
    /// <param name="outputWriter">The PipeWriter instance to which the decrypted data will be written.</param>
    /// <param name="key">The cryptographic key used for decryption.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous decryption operation.</returns>
    Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, byte[] key, CancellationToken cancellationToken);
}