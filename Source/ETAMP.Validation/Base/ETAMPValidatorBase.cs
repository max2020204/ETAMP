#region

using System.Security.Cryptography;
using ETAMP.Core.Models;
using ETAMP.Encryption.Base;
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
    ///     Validates an ETAMP model using the provided token type and a specified validation mode.
    /// </summary>
    /// <typeparam name="T">The type of the token that the ETAMP model contains.</typeparam>
    /// <param name="etamp">The ETAMP model to validate.</param>
    /// <param name="validateLite">If true, performs a lightweight validation; otherwise, performs a full validation.</param>
    /// <returns>A task that represents the asynchronous validation operation. The task result contains the validation result.</returns>
    public abstract Task<ValidationResult> ValidateETAMPAsync<T>(ETAMPModel<T> etamp, bool validateLite)
        where T : Token;

    /// <summary>
    ///     Initializes the ETAMPValidatorBase by providing the ECDsa provider and the hash algorithm name.
    /// </summary>
    /// <param name="provider">The IECDSAProvider instance to be used for cryptographic operations.</param>
    /// <param name="algorithmName">The hash algorithm name to be used for cryptographic operations.</param>
    public void Initialize(ECDSAProviderBase provider, HashAlgorithmName algorithmName)
    {
        signutureValidatorAbstract.Initialize(provider, algorithmName);
    }
}