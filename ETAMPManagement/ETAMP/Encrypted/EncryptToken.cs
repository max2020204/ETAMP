#region

using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.ETAMP.Encrypted.Interfaces;
using ETAMPManagement.Models;
using ETAMPManagement.Validators.Interfaces;

#endregion

namespace ETAMPManagement.ETAMP.Encrypted;

/// <summary>
///     Provides functionalities for encrypting ETAMP tokens using the Elliptic Curve Integrated Encryption Scheme (ECIES).
/// </summary>
public class EncryptToken : IEncryptToken
{
    /// <summary>
    ///     Provides functionalities for encrypting ETAMP tokens using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    private readonly IEciesEncryptionService _eciesEncryptionService;

    /// <summary>
    ///     Handles the validation of the structure and consistency of ETAMP tokens and models.
    /// </summary>
    private readonly IStructureValidator _structureValidator;

    /// <summary>
    ///     Provides functionalities for encrypting ETAMP tokens using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    public EncryptToken(IStructureValidator structureValidator, IEciesEncryptionService eciesEncryptionService)
    {
        _structureValidator = structureValidator;
        _eciesEncryptionService = eciesEncryptionService;
    }

    /// <summary>
    ///     Encrypts an ETAMP token and returns the encrypted token as an ETAMPModel.
    /// </summary>
    /// <param name="jsonEtamp">The JSON string representation of an ETAMP token to be encrypted.</param>
    /// <returns>An ETAMPModel instance containing the encrypted token.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the ETAMP data is invalid.</exception>
    public ETAMPModel EncryptETAMP(string jsonEtamp)
    {
        var model = _structureValidator.IsValidEtampFormat(jsonEtamp);
        ArgumentException.ThrowIfNullOrWhiteSpace(model.Token);
        model.Token = _eciesEncryptionService.Encrypt(model.Token);
        return model;
    }
}