#region

using ETAMP.Encryption.Base;

#endregion

namespace ETAMP.Encryption;

/// <summary>
///     Provides encryption and decryption functionalities using ECIES encryption algorithm.
/// </summary>
public sealed class ECIESEncryptionService : ECIESEncryptionServiceBase
{
    public override async Task<string> EncryptAsync(MemoryStream message)
    {
        CheckInitialization();
        ValidateSecret();

        var sharedSecret = KeyExchanger!.GetSharedSecret()!;
        var encryptedMessage = await EncryptionService!.EncryptAsync(message, sharedSecret);

        using var reader = new StreamReader(encryptedMessage);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    ///     Decrypts an encrypted message back to its plain text form using ECIES asynchronously.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded string.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the decrypted plain text message.</returns>
    public override async Task<string> DecryptAsync(MemoryStream encryptedMessageBase64)
    {
        CheckInitialization();
        ValidateSecret();

        var sharedSecret = KeyExchanger!.GetSharedSecret()!;
        var decryptedMessage = await EncryptionService!.DecryptAsync(encryptedMessageBase64, sharedSecret);
        using var reader = new StreamReader(decryptedMessage);

        return await reader.ReadToEndAsync();
    }
}