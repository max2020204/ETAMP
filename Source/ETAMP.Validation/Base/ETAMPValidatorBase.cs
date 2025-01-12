#region

using System.Security.Cryptography;
using ETAMP.Core.Models;
using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Validation.Interfaces;

#endregion

namespace ETAMP.Validation.Base;

/// <summary>
///     Base class for ETAMP validators.
/// </summary>
public abstract class ETAMPValidatorBase : IETAMPValidator
{
    /// <summary>
    ///     Abstract base class for signature validators.
    /// </summary>
    protected SignatureValidatorBase signutureValidatorAbstract;

    /// <summary>
    ///     Base abstract class for ETAMP validators.
    /// </summary>
    public ETAMPValidatorBase(SignatureValidatorBase signatureValidatorBase)
    {
        signutureValidatorAbstract = signatureValidatorBase;
    }

    /// <summary>
    ///     Validates the ETAMP (Encrypted Token And Message Protocol) structure.
    /// </summary>
    /// <typeparam name="T">The type of the token.</typeparam>
    /// <param name="etamp">The ETAMP model to validate.</param>
    /// <param name="validateLite">Specify whether to perform a lite validation.</param>
    /// <returns>The validation result.</returns>
    public abstract ValidationResult ValidateETAMP<T>(ETAMPModel<T> etamp, bool validateLite) where T : Token;

    /// <summary>
    ///     Initializes the ETAMPValidatorBase by providing the ECDsa provider and the hash algorithm name.
    /// </summary>
    /// <param name="provider">The IECDsaProvider instance to be used for cryptographic operations.</param>
    /// <param name="algorithmName">The hash algorithm name to be used for cryptographic operations.</param>
    public void Initialize(ECDsaProviderBase provider, HashAlgorithmName algorithmName)
    {
        signutureValidatorAbstract.Initialize(provider, algorithmName);
    }
}