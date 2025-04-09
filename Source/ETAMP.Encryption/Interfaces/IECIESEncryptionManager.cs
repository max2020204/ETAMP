using System.Security.Cryptography;

namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Defines methods for encrypting and decrypting data using the Elliptic Curve Integrated Encryption Scheme (ECIES).
///     Provides support for both string and byte array data formats with asynchronous operations.
/// </summary>
public interface IECIESEncryptionManager
{
    /// <summary>
    ///     Asynchronously encrypts the provided data using Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    /// <param name="data">The input data to be encrypted as a string.</param>
    /// <param name="privateKey">The private key for encryption, represented as an instance of ECDiffieHellman.</param>
    /// <param name="publicKey">The recipient's public key, represented as an instance of ECDiffieHellmanPublicKey.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the encrypted data as a UTF-8 encoded
    ///     string.
    /// </returns>
    Task<string> EncryptAsync(string data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey);

    /// <summary>
    ///     Asynchronously encrypts the provided data using the specified private and public keys.
    /// </summary>
    /// <param name="data">The data to be encrypted, represented as a string.</param>
    /// <param name="privateKey">The private key used for the encryption process.</param>
    /// <param name="publicKey">The public key used for the encryption process.</param>
    /// <returns>
    ///     A task representing the asynchronous encryption operation. The result contains the encrypted data as a
    ///     base64-encoded string.
    /// </returns>
    Task<byte[]> EncryptAsync(byte[] data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey);

    /// <summary>
    ///     Asynchronously decrypts a string using ECIES (Elliptic Curve Integrated Encryption Scheme) with the specified
    ///     private and public keys.
    /// </summary>
    /// <param name="data">The encrypted data as a string to be decrypted.</param>
    /// <param name="privateKey">The <see cref="ECDiffieHellman" /> private key used for decryption.</param>
    /// <param name="publicKey">The <see cref="ECDiffieHellmanPublicKey" /> public key used for decryption.</param>
    /// <returns>Returns the decrypted string.</returns>
    Task<string> DecryptAsync(string data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey);

    /// Asynchronously decrypts the specified byte array using ECIES (Elliptic Curve Integrated Encryption Scheme) with the provided private and public keys.
    /// <param name="data">The encrypted data as a byte array to be decrypted.</param>
    /// <param name="privateKey">The private key used in decrypting the data.</param>
    /// <param name="publicKey">The public key associated with the encrypted data.</param>
    /// <returns>
    ///     A task that represents the asynchronous decryption operation. The task result contains the decrypted data as a
    ///     byte array.
    /// </returns>
    Task<byte[]> DecryptAsync(byte[] data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey);
}