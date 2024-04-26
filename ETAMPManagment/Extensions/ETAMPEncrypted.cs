using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;

namespace ETAMPManagment.Extensions;

/// <summary>
///     Provides extensions for the ETAMPModel class to enhance security features, including data encryption.
/// </summary>
public static class ETAMPEncrypted
{
    /// <summary>
    ///     Encrypts the token within an ETAMPModel instance using the provided ECIES encryption service.
    /// </summary>
    /// <param name="model">
    ///     The ETAMPModel instance whose token is to be encrypted. This model must already contain a valid
    ///     token.
    /// </param>
    /// <param name="eciesEncryptionService">
    ///     The encryption service used to encrypt the token. The service must implement the
    ///     IEciesEncryptionService interface.
    /// </param>
    /// <returns>
    ///     The ETAMPModel instance with the encrypted token. The original model is modified in-place, and the same
    ///     reference is returned.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the token in the model is null or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the provided ECIES encryption service is null.</exception>
    /// <remarks>
    ///     This method extends the ETAMPModel to include encryption of its token, providing an additional layer of security by
    ///     ensuring that the token contents are not readable without proper decryption. This is particularly useful in
    ///     scenarios where sensitive information needs to be securely transmitted over unsecured channels.
    /// </remarks>
    public static ETAMPModel EncryptToken(this ETAMPModel model, IEciesEncryptionService eciesEncryptionService)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Token);
        ArgumentNullException.ThrowIfNull(eciesEncryptionService);
        model.Token = eciesEncryptionService.Encrypt(model.Token);
        return model;
    }
}