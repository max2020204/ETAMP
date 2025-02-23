using System.Buffers;
using System.IO.Pipelines;
using System.Security.Cryptography;
using System.Text;
using ETAMP.Core.Utils;
using ETAMP.Encryption.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Encryption;

/// <summary>
/// Manages encryption and decryption using the Elliptic Curve Integrated Encryption Scheme (ECIES).
/// Provides asynchronous methods for encrypting and decrypting data in both string and byte array formats.
/// </summary>
public class ECIESEncryptionManager : IECIESEncryptionManager
{
    /// <summary>
    /// Represents a private dependency of type <see cref="IECIESEncryptionService"/> used to perform
    /// Elliptic Curve Integrated Encryption Scheme (ECIES) operations, such as encryption and decryption of data streams.
    /// </summary>
    private readonly IECIESEncryptionService _ecies;

    private ILogger<ECIESEncryptionManager> _logger;

    /// <summary>
    /// Provides encryption and decryption functionalities using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// This class serves as a manager to handle operations for encrypting and decrypting data using ECIES with
    /// support for both string and byte array formats asynchronously.
    /// Implements the <see cref="IECIESEncryptionManager"/> interface.
    /// </summary>
    public ECIESEncryptionManager(IECIESEncryptionService ecies, ILogger<ECIESEncryptionManager> logger)
    {
        _ecies = ecies;
        _logger = logger;
    }


    /// <summary>
    /// Encrypts data asynchronously using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// Supports encryption of string or byte array data using the specified private and public keys.
    /// </summary>
    /// <param name="data">The input string data to encrypt.</param>
    /// <param name="privateKey">The ECDiffieHellman private key to use for encryption.</param>
    /// <param name="publicKey">The ECDiffieHellmanPublicKey public key to use for encryption.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the encrypted string data.</returns>
    public async Task<string> EncryptAsync(string data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        return Encoding.UTF8.GetString(await EncryptData(Encoding.UTF8.GetBytes(data), privateKey, publicKey));
    }

    /// <summary>
    /// Encrypts data asynchronously using ECIES (Elliptic Curve Integrated Encryption Scheme).
    /// </summary>
    /// <param name="data">The byte array representing the plaintext data to be encrypted.</param>
    /// <param name="privateKey">The ECDiffieHellman private key to be used for encryption.</param>
    /// <param name="publicKey">The ECDiffieHellmanPublicKey public key to be used for encryption.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the encrypted byte array.</returns>
    public async Task<byte[]> EncryptAsync(byte[] data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        return await EncryptData(data, privateKey, publicKey);
    }

    /// <summary>
    /// Decrypts the specified encrypted data string using the provided private and public keys asynchronously.
    /// </summary>
    /// <param name="data">The encrypted data string to be decrypted.</param>
    /// <param name="privateKey">The private ECDiffieHellman key for decryption.</param>
    /// <param name="publicKey">The public ECDiffieHellman key associated with the sender.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the decrypted string.</returns>
    public async Task<string> DecryptAsync(string data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        return Encoding.UTF8.GetString(await DecryptData(Encoding.UTF8.GetBytes(data), privateKey, publicKey));
    }

    /// <summary>
    /// Decrypts the specified byte array using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// The operation is performed asynchronously and requires the private key and public key for decryption.
    /// </summary>
    /// <param name="data">The encrypted data to be decrypted in byte array format.</param>
    /// <param name="privateKey">The ECDiffieHellman private key used for decryption.</param>
    /// <param name="publicKey">The ECDiffieHellman public key used for decryption.</param>
    /// <returns>A task representing the asynchronous operation, containing the decrypted data as a byte array.</returns>
    public async Task<byte[]> DecryptAsync(byte[] data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        return await DecryptData(data, privateKey, publicKey);
    }

    /// <summary>
    /// Encrypts data using the specified private and public ECDH keys and returns the encrypted data as a byte array.
    /// </summary>
    /// <param name="data">The byte array containing the data to be encrypted.</param>
    /// <param name="privateKey">The private ECDiffieHellman key used for encryption.</param>
    /// <param name="publicKey">The public ECDiffieHellman key used for encryption.</param>
    /// <returns>A task representing the asynchronous operation,
    /// with a byte array containing the encrypted data as the result.</returns>
    private async Task<byte[]> EncryptData(byte[] data, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey)
    {
        _logger.LogInformation("Encrypting data");
        var dataPipe = new Pipe();
        var outputPipe = new Pipe();

        var base64 = Base64UrlEncoder.EncodeBytes(data);
        await dataPipe.Writer.WriteAsync(base64);
        await dataPipe.Writer.CompleteAsync();
        _logger.LogInformation("Data written to pipe");

        await _ecies.EncryptAsync(dataPipe.Reader, outputPipe.Writer, privateKey, publicKey);
        _logger.LogInformation("Data encrypted");

        var result = await outputPipe.Reader.ReadAsync();
        _logger.LogInformation("Data read from pipe");
        var encodeBytes = Base64UrlEncoder.EncodeBytes(result.Buffer.ToArray());
        outputPipe.Reader.AdvanceTo(result.Buffer.End);
        await outputPipe.Reader.CompleteAsync();
        _logger.LogInformation("Data complete");
        return encodeBytes;
    }

    /// Decrypts the provided encrypted data using the specified private key and public key.
    /// <param name="data">The encrypted data to be decrypted as a byte array.</param>
    /// <param name="privateKey">The ECDiffieHellman private key used for decryption.</param>
    /// <param name="publicKey">The ECDiffieHellman public key of the counterpart used for decryption.</param>
    /// <returns>A task representing the asynchronous operation that returns the decrypted data as a byte array.</returns>
    private async Task<byte[]> DecryptData(byte[] data, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        _logger.LogInformation("Decrypting data");
        var decryptionPipe = new Pipe();
        var decryptionOutputPipe = new Pipe();

        await decryptionPipe.Writer.WriteAsync(Base64UrlEncoder.DecodeBytes(data));
        await decryptionPipe.Writer.CompleteAsync();
        _logger.LogInformation("Data written to pipe");

        await _ecies.DecryptAsync(decryptionPipe.Reader, decryptionOutputPipe.Writer, privateKey, publicKey);
        _logger.LogInformation("Data decrypted");

        var result = await decryptionOutputPipe.Reader.ReadAsync();
        _logger.LogInformation("Data read from pipe");
        var decryptedData = result.Buffer.ToArray();
        decryptionOutputPipe.Reader.AdvanceTo(result.Buffer.End);
        await decryptionOutputPipe.Reader.CompleteAsync();
        _logger.LogInformation("Data complete");
        return decryptedData;
    }
}