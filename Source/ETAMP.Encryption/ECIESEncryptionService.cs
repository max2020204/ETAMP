using System.IO.Pipelines;
using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Encryption;

/// <summary>
/// A service for performing encryption and decryption using Elliptic Curve Integrated Encryption Scheme (ECIES).
/// Implements asynchronous methods for encrypting and decrypting data streams.
/// </summary>
public sealed class ECIESEncryptionService : IECIESEncryptionService
{
    /// <summary>
    ///     An instance of the encryption service implementing the <see cref="IEncryptionService" /> interface.
    ///     Used to perform encryption and decryption operations with cryptographic keys derived during the ECIES process.
    /// </summary>
    private readonly IEncryptionService? _encryptionService;

    /// <summary>
    ///     The logger instance used for logging messages, errors, and informational data
    ///     during the operation of the <see cref="ECIESEncryptionService" /> class.
    /// </summary>
    /// <remarks>
    ///     This logger is specifically configured for the <see cref="ECIESEncryptionService" />.
    ///     It is used to track application execution, assist in debugging, and report runtime
    ///     information related to encryption and decryption processes.
    /// </remarks>
    private readonly ILogger<ECIESEncryptionService> _logger;

    /// <summary>
    /// Provides an implementation of the IECIESEncryptionService interface, enabling ECIES
    /// (Elliptic Curve Integrated Encryption Scheme) encryption and decryption functionality.
    /// This service uses elliptic curve cryptography to securely encrypt and decrypt data streams.
    /// </summary>
    public ECIESEncryptionService(IEncryptionService encryptionService, ILogger<ECIESEncryptionService> logger)
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        _logger = logger;
    }


    /// <summary>
    ///     Asynchronously encrypts data from the input reader and writes the encrypted data to the output writer
    ///     using the ECIES (Elliptic Curve Integrated Encryption Scheme).
    /// </summary>
    /// <param name="inputReader">The <see cref="PipeReader" /> from which the plaintext data is read.</param>
    /// <param name="outputWriter">The <see cref="PipeWriter" /> to which the encrypted data is written.</param>
    /// <param name="privateKey">The <see cref="ECDiffieHellman" /> private key used for encryption purposes.</param>
    /// <param name="publicKey">The <see cref="ECDiffieHellmanPublicKey" /> of the recipient used for key agreement.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default)
    {
        EnsureArgumentsAreNotNull(inputReader, outputWriter);
        await GetSharedKey(inputReader, outputWriter, privateKey, publicKey, cancellationToken);
    }

    /// <summary>
    ///     Encrypts data from the specified <see cref="PipeReader" /> and writes the encrypted data to the specified
    ///     <see cref="PipeWriter" />
    ///     using the provided private key and public key.
    /// </summary>
    /// <param name="inputReader">The <see cref="PipeReader" /> from which the data to be encrypted is read.</param>
    /// <param name="outputWriter">The <see cref="PipeWriter" /> to which the encrypted data is written.</param>
    /// <param name="privateKey">The private key of type <see cref="ECDiffieHellman" /> used for encryption.</param>
    /// <param name="publicKey">The public key as a byte array used for encryption.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to observe cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous encryption operation.</returns>
    public async Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        byte[] publicKey,
        CancellationToken cancellationToken = default)
    {
        EnsureArgumentsAreNotNull(inputReader, outputWriter);
        await GetSharedKey(inputReader, outputWriter, privateKey, publicKey, cancellationToken);
    }

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
    public async Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default)
    {
        EnsureArgumentsAreNotNull(inputReader, outputWriter);
        await GetSharedKey(inputReader, outputWriter, privateKey, publicKey, cancellationToken);
    }

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
    public async Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        byte[] publicKey, CancellationToken cancellationToken = default)
    {
        EnsureArgumentsAreNotNull(inputReader, outputWriter);
        await GetSharedKey(inputReader, outputWriter, privateKey, publicKey, cancellationToken);
    }

    /// <summary>
    ///     Derives a shared key using Elliptic Curve Diffie-Hellman (ECDH) algorithm and decrypts data using the shared key.
    /// </summary>
    /// <param name="inputReader">The <see cref="PipeReader" /> to read encrypted input data.</param>
    /// <param name="outputWriter">The <see cref="PipeWriter" /> to write decrypted output data.</param>
    /// <param name="privateKey">The private key used for deriving the shared key.</param>
    /// <param name="publicKey">The public key used for deriving the shared key.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
    private async Task GetSharedKey(PipeReader inputReader, PipeWriter outputWriter,
        ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default)
    {
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        await _encryptionService!.DecryptAsync(inputReader, outputWriter, sharedSecret, cancellationToken);
    }

    /// <summary>
    ///     Derives a shared secret key using an ECDiffieHellman private key and a provided public key,
    ///     then uses the derived shared key to decrypt data from the input reader to the output writer.
    /// </summary>
    /// <param name="inputReader">The <see cref="PipeReader" /> instance to read encrypted data from.</param>
    /// <param name="outputWriter">The <see cref="PipeWriter" /> instance to write the decrypted data to.</param>
    /// <param name="privateKey">The private key used to derive the shared secret.</param>
    /// <param name="publicKey">The public key used to derive the shared secret in conjunction with the private key.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task GetSharedKey(PipeReader inputReader, PipeWriter outputWriter,
        ECDiffieHellman privateKey, byte[] publicKey, CancellationToken cancellationToken = default)
    {
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        await _encryptionService!.DecryptAsync(inputReader, outputWriter, sharedSecret, cancellationToken);
    }

    /// <summary>
    ///     Ensures that the provided arguments are not null.
    /// </summary>
    /// <param name="inputReader">The <see cref="PipeReader" /> to be checked for null.</param>
    /// <param name="outputWriter">The <see cref="PipeWriter" /> to be checked for null.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the arguments are null.</exception>
    private static void EnsureArgumentsAreNotNull(PipeReader inputReader, PipeWriter outputWriter)
    {
        ArgumentNullException.ThrowIfNull(inputReader, nameof(inputReader));
        ArgumentNullException.ThrowIfNull(outputWriter, nameof(outputWriter));
    }

    /// Derives a shared secret using Elliptic Curve Diffie-Hellman (ECDH) techniques.
    /// <param name="privateKey">
    ///     The ECDiffieHellman private key for the key exchange.
    /// </param>
    /// <param name="publicKey">
    ///     The ECDiffieHellmanPublicKey from the intended recipient.
    /// </param>
    /// <returns>
    ///     A byte array representing the derived shared secret for secure communication.
    /// </returns>
    private byte[] DeriveSharedSecret(ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));
        return privateKey.DeriveKeyMaterial(publicKey);
    }

    /// <summary>
    ///     Derives a shared secret using the private key and the provided public key.
    /// </summary>
    /// <param name="privateKey">The ECDiffieHellman private key used for deriving the shared secret.</param>
    /// <param name="publicKey">The public key represented as a byte array used in the shared secret derivation.</param>
    /// <returns>A byte array containing the derived shared secret.</returns>
    private byte[] DeriveSharedSecret(ECDiffieHellman privateKey, byte[] publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));
        _logger.LogInformation("Creating ECDiffieHellman instance...");
        using var ecdh = ECDiffieHellman.Create();
        _logger.LogInformation("Importing public key...");
        ecdh.ImportSubjectPublicKeyInfo(publicKey, out _);
        _logger.LogInformation("Deriving shared secret...");
        return DeriveSharedSecret(privateKey, ecdh.PublicKey);
    }
}