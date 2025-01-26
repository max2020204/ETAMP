#region

using System.Security.Cryptography;
using System.Text;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;
using ETAMP.Wrapper.Interfaces;
using Microsoft.Extensions.Logging;

#endregion

namespace ETAMP.Wrapper;

/// <summary>
///     Signs data using Elliptic Curve Digital Signature Algorithm (ECDsa).
/// </summary>
public sealed class SignWrapper : ISignWrapper
{
    private HashAlgorithmName _algorithmName;
    private ECDsa? _ecdsa;

    private readonly ILogger<SignWrapper> _logger;

    public SignWrapper(ILogger<SignWrapper> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     SignEtampModel method signs an ETAMPModel instance and updates the signature fields.
    /// </summary>
    /// <typeparam name="T">The type of the token.</typeparam>
    /// <param name="etamp">The ETAMPModel instance to sign.</param>
    /// <returns>The signed ETAMPModel instance.</returns>
    /// <exception cref="ArgumentException">Thrown if etamp.Token is null.</exception>
    public async Task<ETAMPModel<T>> SignEtampModel<T>(ETAMPModel<T> etamp) where T : Token
    {
        ArgumentNullException.ThrowIfNull(etamp.Token, nameof(etamp.Token));


        _logger.LogInformation("Serialize Token");
        var token = await etamp.Token.ToJsonAsync();
        _logger.LogInformation("Token was serialized");
        var dataToSign = $"{etamp.Id}{etamp.Version}{token}{etamp.UpdateType}{etamp.CompressionType}";
        _logger.LogInformation("Data to sign was generated");
        var signature = Sign(Encoding.UTF8.GetBytes(dataToSign));
        _logger.LogInformation("Signature was generated");
        etamp.SignatureMessage = Base64UrlEncoder.Encode(signature);
        _logger.LogInformation("Signature was encoded");
        return etamp;
    }

    /// <summary>
    ///     Initializes the signing provider with the specified ECDsa and hash algorithm.
    /// </summary>
    /// <param name="provider">The ECDsa provider to perform signing operations.</param>
    /// <param name="algorithmName">The name of the hash algorithm to use for signing.</param>
    public void Initialize(ECDsa provider, HashAlgorithmName algorithmName)
    {
        _logger.LogInformation("Initialize");
        _ecdsa = provider;
        _algorithmName = algorithmName;
    }

    /// <summary>
    ///     Releases all resources used by the current instance of the SignWrapper class.
    /// </summary>
    public void Dispose()
    {
        _logger.LogInformation("Dispose");
        _ecdsa?.Dispose();
    }

    /// <summary>
    ///     Signs the provided data using the initialized ECDsa provider and hash algorithm.
    /// </summary>
    /// <param name="data">The byte array representing the data to be signed.</param>
    /// <returns>A byte array containing the generated digital signature.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the ECDsa provider is not initialized.</exception>
    private byte[] Sign(byte[] data)
    {
        _logger.LogInformation("Sign");
        return _ecdsa!.SignData(data, _algorithmName);
    }
}