using System.Security.Cryptography;

namespace ETAMP.Core.Interfaces;

/// <summary>
///     Provides an interface for components that require initialization with cryptographic providers and algorithms.
/// </summary>
/// <remarks>
///     Classes implementing this interface can be initialized with an optional ECDsa cryptographic provider and a
///     specified
///     hash algorithm name. The interface also extends <see cref="IDisposable" /> to ensure proper disposal of resources
///     used during initialization.
/// </remarks>
public interface IInitialize : IDisposable
{
    /// <summary>
    ///     Initializes the cryptographic provider and hash algorithm.
    /// </summary>
    /// <param name="provider">The ECDsa provider to be used for cryptographic operations.</param>
    /// <param name="algorithmName">The hash algorithm name to use with the provider.</param>
    void Initialize(ECDsa? provider, HashAlgorithmName algorithmName);
}