using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.Models;

namespace ETAMPManagement.Extensions;

public static class Encryption
{
    /// <summary>
    ///     Encrypts the data in the given ETAMPModel using the provided IEciesEncryptionService.
    /// </summary>
    /// <typeparam name="T">The type of token contained in the ETAMPModel.</typeparam>
    /// <param name="model">The ETAMPModel to be encrypted.</param>
    /// <param name="eciesEncryptionService">The IEciesEncryptionService used to encrypt the data.</param>
    /// <returns>The encrypted ETAMPModel with the data encrypted in the token.</returns>
    public static ETAMPModel<T> EncryptData<T>(this ETAMPModel<T> model, IEciesEncryptionService eciesEncryptionService)
        where T : Token
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Token.Data);
        ArgumentNullException.ThrowIfNull(eciesEncryptionService);
        model.Token.Data = eciesEncryptionService.Encrypt(model.Token.Data);
        return model;
    }
}