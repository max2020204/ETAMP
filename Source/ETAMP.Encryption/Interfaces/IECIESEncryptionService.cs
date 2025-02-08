using System.IO.Pipelines;
using System.Security.Cryptography;

namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Defines a service for encrypting and decrypting messages using Elliptic Curve Integrated ETAMPEncryption Scheme
///     (ECIES).
/// </summary>
public interface IECIESEncryptionService
{
    Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey,
        CancellationToken cancellationToken = default);

    Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey, byte[] publicKey,
        CancellationToken cancellationToken = default);

    Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default);

    Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, ECDiffieHellman privateKey, byte[] publicKey,
        CancellationToken cancellationToken = default);
}