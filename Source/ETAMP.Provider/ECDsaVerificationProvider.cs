using System.Security.Cryptography;
using ETAMP.Core.Utils;
using ETAMP.Wrapper.Interfaces;
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
    ///     Verifies the signature of string data.
    /// </summary>
    /// <param name="data">Data to verify, in string format.</param>
    /// <param name="signature">Base64-encoded signature to verify against.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public bool VerifyData(Stream data, string signature)
    {
        ValidateState();
        EnsureStreamIsReadable(data);

        _logger.LogDebug("Verifying data stream of length {Length}", data.Length);

        var isValid = _ecdsa!.VerifyData(data, Base64UrlEncoder.DecodeBytes(signature), _algorithmName);

        if (!isValid)
            _logger.LogWarning("Signature verification failed for data stream.");

        return isValid;
    }

    /// <summary>
    ///     Verifies the signature of byte array data.
    /// </summary>
    /// <param name="data">Data to verify, as a byte array.</param>
    /// <param name="signature">Signature to verify against, as a byte array.</param>
    /// <returns>True if the signature is valid; otherwise, false.</returns>
    public bool VerifyData(Stream data, byte[] signature)
    {
        ValidateState();
        EnsureStreamIsReadable(data);

        _logger.LogDebug("Verifying data stream of length {Length}", data.Length);
        var isValid = _ecdsa!.VerifyData(data, signature, _algorithmName);

        if (!isValid)
            _logger.LogWarning("Signature verification failed for data stream.");

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

    private static void EnsureStreamIsReadable(Stream stream)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Stream is not readable.", nameof(stream));

        if (stream.CanSeek)
            stream.Position = 0;
    }
}