using ETAMP.Core.Models;
using ETAMP.Encryption.Interfaces;

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
    public static ETAMPModel<T> EncryptData<T>(this ETAMPModel<T> model, IECIESEncryptionService eciesEncryptionService)
        where T : Token
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Token.Data);
        ArgumentNullException.ThrowIfNull(eciesEncryptionService);
        model.Token.Data = eciesEncryptionService.Encrypt(model.Token.Data);
        model.Token.IsEncrypted = true;
        return model;
    }
}