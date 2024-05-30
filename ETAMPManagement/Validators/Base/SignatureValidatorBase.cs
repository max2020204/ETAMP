using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Models;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper.Base;

namespace ETAMPManagement.Validators.Base;

/// <summary>
///     Represents the base class for signature validators.
/// </summary>
public abstract class SignatureValidatorBase : ISignatureValidator
{
    /// <summary>
    ///     Provides a wrapper class for verifying data using ECDsa cryptographic algorithm.
    /// </summary>
    protected VerifyWrapperBase VerifyWrapper;

    /// <summary>
    ///     Base class for signature validators.
    /// </summary>
    public SignatureValidatorBase(VerifyWrapperBase verifyWrapper)
    {
        VerifyWrapper = verifyWrapper;
    }

    /// <summary>
    ///     Validates the ETAMP message.
    /// </summary>
    /// <typeparam name="T">The type of token.</typeparam>
    /// <param name="etamp">The ETAMPModel instance.</param>
    /// <returns>The ValidationResult instance.</returns>
    public abstract ValidationResult ValidateETAMPMessage<T>(ETAMPModel<T> etamp) where T : Token;

    /// <summary>
    ///     Initializes the VerifyWrapper by setting the ECDsa instance and hash algorithm name.
    /// </summary>
    /// <param name="provider">The provider for managing and accessing the ECDsa instance.</param>
    /// <param name="algorithmName">The hash algorithm name.</param>
    public void Initialize(IECDsaProvider provider, HashAlgorithmName algorithmName)
    {
        VerifyWrapper.Initialize(provider, algorithmName);
    }
}