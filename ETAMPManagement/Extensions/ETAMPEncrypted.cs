#region

using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.Models;

#endregion

namespace ETAMPManagement.Extensions;

/// <summary>
///     Provides extensions for the ETAMPModel class to enhance security features, including data encryption.
/// </summary>
public static class ETAMPEncrypted
{
    /// <summary>
    ///     Encrypts the token in the ETAMPModel using the provided IEciesEncryptionService.
    /// </summary>
    /// <param name="model">The ETAMPModel instance.</param>
    /// <param name="eciesEncryptionService">The IEciesEncryptionService instance used for encryption.</param>
    /// <returns>The ETAMPModel instance with the encrypted token.</returns>
    public static ETAMPModel EncryptToken(this ETAMPModel model, IEciesEncryptionService eciesEncryptionService)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Token);
        ArgumentNullException.ThrowIfNull(eciesEncryptionService);
        model.Token = eciesEncryptionService.Encrypt(model.Token);
        return model;
    }
}