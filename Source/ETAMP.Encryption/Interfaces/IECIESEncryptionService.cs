using System.IO.Pipelines;
using System.Security.Cryptography;

namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Represents a contract for services implementing encryption and decryption using the Elliptic Curve Integrated
///     Encryption Scheme (ECIES). Provides asynchronous methods for handling secure data streams.
/// </summary>
public interface IECIESEncryptionService
{
    /// <summary>
    ///     Asynchronously encrypts data from the input reader and writes the encrypted data to the output writer
    ///     using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    /// <param name="inputReader">The <see cref="PipeReader" /> from which the plaintext data is read.</param>
    /// <param name="outputWriter">The <see cref="PipeWriter" /> to which the encrypted data is written.</param>
    /// <param name="privateKey">The <see cref="ECDiffieHellman" /> private key used for encryption operations.</param>
    /// <param name="publicKey">
    ///     The <see cref="ECDiffieHellmanPublicKey" /> of the recipient used for key agreement during
    ///     encryption.
    /// </param>
    /// <param name="cancellationToken">
    ///     An optional <see cref="CancellationToken" /> to cancel the asynchronous encryption
    ///     operation.
    /// </param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation of encrypting the data.</returns>
    Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Encrypts data asynchronously using ECIES (Elliptic Curve Integrated Encryption Scheme) with the provided private
    ///     key
    ///     and public key. Reads the input data from the specified <see cref="PipeReader" /> and writes the encrypted data
    ///     to the specified <see cref="PipeWriter" />.
    /// </summary>
    /// <param name="inputReader">The <see cref="PipeReader" /> from which the plaintext data is read.</param>
    /// <param name="outputWriter">The <see cref="PipeWriter" /> to which the encrypted data is written.</param>
    /// <param name="privateKey">
    ///     The private key of type <see cref="ECDiffieHellman" /> used in encryption to generate a shared
    ///     secret.
    /// </param>
    /// <param name="publicKey">The public key as a byte array used in encryption to establish a shared secret.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to observe cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation of encrypting the data.</returns>
    Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey, byte[] publicKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously decrypts data from the input stream and writes the decrypted data to the output stream
    ///     using the given private key and peer's public key for the ECIES scheme.
    /// </summary>
    /// <param name="inputReader">The reader for the encrypted input stream.</param>
    /// <param name="outputWriter">The writer for the decrypted output stream.</param>
    /// <param name="privateKey">The ECDiffieHellman private key for decryption.</param>
    /// <param name="publicKey">The ECDiffieHellmanPublicKey from the peer for shared key computation.</param>
    /// <param name="cancellationToken">The token used to cancel the decryption operation, if needed.</param>
    /// <returns>A Task representing the asynchronous decryption operation.</returns>
    Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Decrypts the data read from the input reader using the specified private key and the public key,
    ///     and writes the decrypted data to the output writer.
    /// </summary>
    /// <param name="inputReader">The <see cref="PipeReader" /> to read the encrypted data from.</param>
    /// <param name="outputWriter">The <see cref="PipeWriter" /> to write the decrypted data to.</param>
    /// <param name="privateKey">The <see cref="ECDiffieHellman" /> private key used for decryption.</param>
    /// <param name="publicKey">The public key as a byte array used for key agreement during decryption.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey, byte[] publicKey,
        CancellationToken cancellationToken = default);
}