#region

using System.Security.Cryptography;
using ETAMP.Core.Models;
using ETAMP.Encryption.Interfaces;

#endregion

namespace ETAMP.Extension.Builder;

public static class ETAMPEncryption
{
    /// <summary>
    ///     Encrypts the data in the given ETAMPModel using the provided IECIESEncryptionService.
    /// </summary>
    /// <typeparam name="T">The type of token contained in the ETAMPModel.</typeparam>
    /// <param name="model">The ETAMPModel to be encrypted.</param>
    /// <param name="eciesEncryptionService">The IECIESEncryptionService used to encrypt the data.</param>
    /// <returns>The encrypted ETAMPModel with the data encrypted in the token.</returns>
    public static async Task<ETAMPModel<T>> EncryptData<T>(this ETAMPModel<T> model,
        IECIESEncryptionService eciesEncryptionService, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
        where T : Token
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Token.Data);
        ArgumentNullException.ThrowIfNull(eciesEncryptionService);
        var stream =
            await eciesEncryptionService.EncryptAsync(await GenerateStreamFromString(model.Token.Data), privateKey,
                publicKey);
        StreamReader reader = new(stream);
        model.Token.Data = await reader.ReadToEndAsync();
        model.Token.IsEncrypted = true;
        return model;
    }

    /// <summary>
    ///     Encrypts the data in the specified ETAMPModel using the provided IECIESEncryptionService.
    /// </summary>
    /// <typeparam name="T">The type of token contained in the ETAMPModel.</typeparam>
    /// <param name="model">The ETAMPModel to be encrypted.</param>
    /// <param name="eciesEncryptionService">The IECIESEncryptionService used to perform the encryption.</param>
    /// <param name="publicKey">The public key used in the encryption process.</param>
    /// <returns>The encrypted ETAMPModel with the token's data encrypted.</returns>
    public static async Task<ETAMPModel<T>> EncryptData<T>(this ETAMPModel<T> model,
        IECIESEncryptionService eciesEncryptionService, ECDiffieHellman privateKey, byte[] publicKey)
        where T : Token
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Token.Data);
        ArgumentNullException.ThrowIfNull(eciesEncryptionService);
        var stream =
            await eciesEncryptionService.EncryptAsync(await GenerateStreamFromString(model.Token.Data), privateKey,
                publicKey);
        StreamReader reader = new(stream);
        model.Token.Data = await reader.ReadToEndAsync();
        model.Token.IsEncrypted = true;
        return model;
    }

    private static async Task<MemoryStream> GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(s);
        await writer.FlushAsync();
        stream.Position = 0;

        return stream;
    }
}