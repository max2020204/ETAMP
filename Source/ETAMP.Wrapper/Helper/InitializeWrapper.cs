using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

namespace ETAMP.Wrapper.Helper;

/// <summary>
///     Represents a wrapper class for initializing cryptographic providers and algorithms.
/// </summary>
public class InitializeWrapper
{
    /// <summary>
    ///     Represents the Ecdsa class for cryptographic operations using Elliptic Curve Digital Signature Algorithm (ECDSA).
    /// </summary>
    protected ECDsa? Ecdsa { get; private set; }

    /// <summary>
    ///     This class is responsible for initializing the ECDsa instance and setting the algorithm name for cryptographic
    ///     operations.
    /// </summary>
    protected HashAlgorithmName AlgorithmName { get; private set; }

    /// <summary>
    ///     Initializes the ECDsa instance and sets the hash algorithm name.
    /// </summary>
    /// <param name="ecdsaProvider">The provider for managing and accessing the ECDsa instance.</param>
    /// <param name="algorithm">The hash algorithm name.</param>
    public void Initialize(IECDsaProvider ecdsaProvider, HashAlgorithmName algorithm)
    {
        if (ecdsaProvider == null)
            throw new ArgumentNullException(nameof(ecdsaProvider), "IECDsaProvider instance cannot be null.");

        Ecdsa = ecdsaProvider.GetECDsa()
                ?? throw new InvalidOperationException(
                    "ECDsa instance cannot be null after extraction from IECDsaProvider.");
        AlgorithmName = algorithm;
    }
}