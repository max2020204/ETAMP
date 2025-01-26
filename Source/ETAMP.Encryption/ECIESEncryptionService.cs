#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;
using Microsoft.Extensions.Logging;

#endregion

namespace ETAMP.Encryption;

/// <summary>
///     Represents a service implementing the Elliptic Curve Integrated Encryption Scheme (ECIES)
///     for secure data encryption and decryption. This class integrates elliptic curve cryptography
///     with symmetric encryption to provide a reliable encryption mechanism.
/// </summary>
public sealed class ECIESEncryptionService : IECIESEncryptionService
{
    private readonly IEncryptionService? _encryptionService;
    private readonly ILogger<ECIESEncryptionService> _logger;

    /// <summary>
    /// Provides functionality for encrypting and decrypting data using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// Combines elliptic curve cryptography with symmetric encryption to ensure secure data transmission.
    /// </summary>
    public ECIESEncryptionService(IEncryptionService encryptionService, ILogger<ECIESEncryptionService> logger)
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        _logger = logger;
    }


    /// Encrypts a given message stream using ECIES (Elliptic Curve Integrated Encryption Scheme).
    /// <param name="message">The stream representing the message to be encrypted.</param>
    /// <param name="privateKey">The private key of the entity encrypting the message.</param>
    /// <param name="publicKey">The public key of the recipient.</param>
    /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
    /// <returns>An encrypted stream of the message.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the message stream is null.</exception>
    public async Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        return await _encryptionService!.EncryptAsync(message, sharedSecret, cancellationToken);
    }


    /// <summary>
    /// Encrypts a given input stream using ECDH-based shared secret and an encryption service.
    /// </summary>
    /// <param name="message">The input stream containing the data to encrypt.</param>
    /// <param name="privateKey">The private ECDiffieHellman key to derive the shared secret.</param>
    /// <param name="publicKey">The public key of the counterpart used to derive the shared secret.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the operation to complete.</param>
    /// <return>
    /// A stream containing the encrypted data.
    /// </return>
    public async Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey, byte[] publicKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        return await _encryptionService!.EncryptAsync(message, sharedSecret, cancellationToken);
    }


    /// <summary>
    ///     Decrypts an encrypted message using the ECIES (Elliptic Curve Integrated Encryption Scheme) approach.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message in base64 format as a stream.</param>
    /// <param name="privateKey">The ECDiffieHellman private key of the recipient used to derive the shared secret.</param>
    /// <param name="publicKey">The ECDiffieHellman public key of the sender used to derive the shared secret.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing the decrypted message as a stream.</returns>
    public async Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(encryptedMessageBase64, nameof(encryptedMessageBase64));
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        return await _encryptionService!.DecryptAsync(encryptedMessageBase64, sharedSecret, cancellationToken);
    }


    /// <summary>
    /// Decrypts an encrypted message using ECIES (Elliptic Curve Integrated Encryption Scheme) with the provided
    /// private key and public key.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded stream.</param>
    /// <param name="privateKey">The ECDiffieHellman private key used for decryption.</param>
    /// <param name="publicKey">The byte array representation of the public key used in the decryption process.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A stream containing the decrypted message.</returns>
    public async Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey,
        byte[] publicKey, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(encryptedMessageBase64, nameof(encryptedMessageBase64));
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        return await _encryptionService!.DecryptAsync(encryptedMessageBase64, sharedSecret, cancellationToken);
    }


    /// <summary>
    /// Derives a shared secret for encryption or decryption using Elliptic Curve Diffie-Hellman (ECDH).
    /// </summary>
    /// <param name="privateKey">The ECDiffieHellman private key used for the shared secret derivation.</param>
    /// <param name="publicKey">The ECDiffieHellmanPublicKey used for the shared secret derivation.</param>
    /// <returns>A byte array representing the derived shared secret.</returns>
    private byte[] DeriveSharedSecret(ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));
        return privateKey.DeriveKeyMaterial(publicKey);
    }


    /// <summary>
    /// Derives a shared secret using the private key and the provided public key.
    /// This method computes the symmetric key material based on elliptic curve Diffie-Hellman (ECDH) key exchange.
    /// </summary>
    /// <param name="privateKey">
    /// The elliptic curve Diffie-Hellman (ECDH) private key used in generating the shared secret.
    /// </param>
    /// <param name="publicKey">
    /// The public key in byte array format to derive the shared secret.
    /// </param>
    /// <returns>
    /// A byte array representing the derived shared secret.
    /// </returns>
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