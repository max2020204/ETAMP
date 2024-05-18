#region

using System.Security.Cryptography;
using System.Text;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.Helper;
using ETAMPManagement.Models;
using ETAMPManagement.Wrapper.Interfaces;
using Newtonsoft.Json;

#endregion

namespace ETAMPManagement.Wrapper;

/// <summary>
///     Signs data using Elliptic Curve Digital Signature Algorithm (ECDsa).
/// </summary>
public sealed class SignWrapper : ISignWrapper
{
    private HashAlgorithmName _algorithmName;
    private ECDsa? _ecdsa;

    /// <summary>
    ///     Initializes the <see cref="SignWrapper" /> with an ECDsa instance and a hash algorithm.
    ///     This method should be called before performing any signature operations.
    /// </summary>
    /// <param name="ecdsaProvider">The provider for ECDsa instances.</param>
    /// <param name="algorithmName">The hash algorithm to use for signing.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="ecdsaProvider" /> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if ECDsa instance cannot be obtained from the provider.</exception>
    public void Initialize(IECDsaProvider ecdsaProvider, HashAlgorithmName algorithmName)
    {
        if (ecdsaProvider == null)
            throw new ArgumentNullException(nameof(ecdsaProvider), "IECDsaProvider instance cannot be null.");

        _ecdsa = ecdsaProvider.GetECDsa()
                 ?? throw new InvalidOperationException(
                     "ECDsa instance cannot be null after extraction from IECDsaProvider.");
        _algorithmName = algorithmName;
    }

    /// <summary>
    ///     SignEtampModel method signs an ETAMPModel instance and updates the signature fields.
    /// </summary>
    /// <typeparam name="T">The type of the token.</typeparam>
    /// <param name="etamp">The ETAMPModel instance to sign.</param>
    /// <returns>The signed ETAMPModel instance.</returns>
    /// <exception cref="ArgumentException">Thrown if etamp.Token is null.</exception>
    public ETAMPModel<T> SignEtampModel<T>(ETAMPModel<T> etamp) where T : Token
    {
        if (etamp.Token == null) throw new ArgumentException(nameof(etamp.Token));

        etamp.SignatureToken =
            Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(etamp.Token))));
        etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(
            Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
        return etamp;
    }

    private byte[] Sign(byte[] data)
    {
        return _ecdsa!.SignData(data, _algorithmName);
    }
}