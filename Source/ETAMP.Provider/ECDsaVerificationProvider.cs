using System.Security.Cryptography;
using ETAMP.Core.Utils;
using ETAMP.Provider.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Provider;

/// <summary>
///     Provides cryptographic verification using ECDsa, supporting both string and byte array data formats.
/// </summary>
/// >
public sealed class ECDsaVerificationProvider : IECDsaVerificationProvider
{
    private readonly ILogger<ECDsaVerificationProvider> _logger;
    private HashAlgorithmName _algorithmName;
    private ECDsa? _ecdsa;

    public ECDsaVerificationProvider(ILogger<ECDsaVerificationProvider> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Verifies the specified data against a signature using the current ECDsa and hash algorithm.
    /// </summary>
    /// <param name="data">
    /// The data to verify, provided as a read-only span of bytes.
    /// </param>
    /// <param name="signature">
    /// The signature to verify against, provided as a read-only span of bytes.
    /// </param>
    /// <returns>
    /// Returns true if the signature is valid for the provided data; otherwise, false.
    /// </returns>
    public bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature)
    {
        ValidateState();

        _logger.LogDebug("Verifying data with signature.");
        var isValid = _ecdsa!.VerifyData(data, signature, _algorithmName);

        if (!isValid)
            _logger.LogWarning("Signature verification failed.");

        return isValid;
    }

    /// <summary>
    ///     Disposes the underlying ECDsa instance, releasing all associated resources.
    /// </summary>
    public void Dispose()
    {
        _logger.LogDebug("Disposing ECDsa instance.");
        _ecdsa?.Dispose();
    }

    /// <summary>
    ///     Initializes the ECDsa provider and hash algorithm for cryptographic operations.
    /// </summary>
    /// <param name="provider">The ECDsa provider to use for cryptographic operations.</param>
    /// <param name="algorithmName">The hash algorithm to associate with the ECDsa provider.</param>
    public void Initialize(ECDsa? provider, HashAlgorithmName algorithmName)
    {
        _logger.LogInformation("Initializing ECDsa with algorithm {Algorithm}", algorithmName);
        _ecdsa = provider;
        _algorithmName = algorithmName;
    }

    private void ValidateState()
    {
        if (_ecdsa != null)
            return;
        _logger.LogError("ECDsa is not initialized. Call Initialize before verifying data.");
        throw new InvalidOperationException("ECDsa is not initialized.");
    }
}