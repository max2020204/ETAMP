using System.IO.Pipelines;
using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;
using Microsoft.Extensions.Logging;

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


    public async Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default)
    {
        EnsureArgumentsAreNotNull(inputReader, outputWriter);
        await GetSharedKey(inputReader, outputWriter, privateKey, publicKey, cancellationToken);
    }

    public async Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        byte[] publicKey,
        CancellationToken cancellationToken = default)
    {
        EnsureArgumentsAreNotNull(inputReader, outputWriter);
        await GetSharedKey(inputReader, outputWriter, privateKey, publicKey, cancellationToken);
    }

    public async Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default)
    {
        EnsureArgumentsAreNotNull(inputReader, outputWriter);
        await GetSharedKey(inputReader, outputWriter, privateKey, publicKey, cancellationToken);
    }

    public async Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        byte[] publicKey, CancellationToken cancellationToken = default)
    {
        EnsureArgumentsAreNotNull(inputReader, outputWriter);
        await GetSharedKey(inputReader, outputWriter, privateKey, publicKey, cancellationToken);
    }

    private async Task GetSharedKey(PipeReader inputReader, PipeWriter outputWriter,
        ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default)
    {
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        await _encryptionService!.DecryptAsync(inputReader, outputWriter, sharedSecret, cancellationToken);
    }

    private async Task GetSharedKey(PipeReader inputReader, PipeWriter outputWriter,
        ECDiffieHellman privateKey, byte[] publicKey, CancellationToken cancellationToken = default)
    {
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        await _encryptionService!.DecryptAsync(inputReader, outputWriter, sharedSecret, cancellationToken);
    }

    private static void EnsureArgumentsAreNotNull(PipeReader inputReader, PipeWriter outputWriter)
    {
        ArgumentNullException.ThrowIfNull(inputReader, nameof(inputReader));
        ArgumentNullException.ThrowIfNull(outputWriter, nameof(outputWriter));
    }

    private byte[] DeriveSharedSecret(ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));
        return privateKey.DeriveKeyMaterial(publicKey);
    }

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