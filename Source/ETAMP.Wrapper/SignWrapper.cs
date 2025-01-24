#region

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;
using ETAMP.Wrapper.Interfaces;

#endregion

namespace ETAMP.Wrapper;

/// <summary>
///     Signs data using Elliptic Curve Digital Signature Algorithm (ECDsa).
/// </summary>
public sealed class SignWrapper : ISignWrapper
{
    private HashAlgorithmName _algorithmName;
    private ECDsa? _ecdsa;

    /// <summary>
    ///     SignEtampModel method signs an ETAMPModel instance and updates the signature fields.
    /// </summary>
    /// <typeparam name="T">The type of the token.</typeparam>
    /// <param name="etamp">The ETAMPModel instance to sign.</param>
    /// <returns>The signed ETAMPModel instance.</returns>
    /// <exception cref="ArgumentException">Thrown if etamp.Token is null.</exception>
    public ETAMPModel<T> SignEtampModel<T>(ETAMPModel<T> etamp) where T : Token
    {
        ArgumentNullException.ThrowIfNull(etamp.Token, nameof(etamp.Token));

        var token = JsonSerializer.Serialize(etamp.Token);
        etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(
            Encoding.UTF8.GetBytes(
                $"{etamp.Id}{etamp.Version}{token}{etamp.UpdateType}{etamp.CompressionType}")));
        return etamp;
    }

    public void Initialize(ECDsa provider, HashAlgorithmName algorithmName)
    {
        _ecdsa = provider;
        _algorithmName = algorithmName;
    }

    public void Dispose()
    {
        _ecdsa?.Dispose();
    }

    private byte[] Sign(byte[] data)
    {
        return _ecdsa!.SignData(data, _algorithmName);
    }
}