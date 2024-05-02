#region

using System.Security.Cryptography;
using System.Text;
using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

#endregion

namespace ETAMPManagment.Wrapper;

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
    ///     Signs the ETAMP data given as a JSON string and updates its signature fields.
    /// </summary>
    /// <param name="jsonEtamp">The ETAMP data in JSON format.</param>
    /// <returns>A JSON string of the ETAMP data with updated signature fields.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the input JSON string is null, empty,
    ///     or cannot be deserialized into an ETAMP model.
    /// </exception>
    public string SignEtamp(string jsonEtamp)
    {
        var etamp = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);

        ArgumentNullException.ThrowIfNull(etamp);
        ArgumentException.ThrowIfNullOrEmpty(etamp.Token);

        etamp.SignatureToken = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
        etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(
            Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
        return JsonConvert.SerializeObject(etamp);
    }

    /// <summary>
    ///     Signs the ETAMP model and updates its signature fields.
    /// </summary>
    /// <param name="etamp">The ETAMP model to be signed.</param>
    /// <returns>A JSON string of the ETAMP data with updated signature fields.</returns>
    public string SignEtamp(ETAMPModel etamp)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(etamp.Token);

        etamp.SignatureToken = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
        etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(
            Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
        return JsonConvert.SerializeObject(etamp);
    }

    /// <summary>
    ///     Signs the ETAMP model and returns the model with updated signature fields.
    /// </summary>
    /// <param name="etamp">The ETAMP model to be signed.</param>
    /// <returns>The ETAMP model with updated signature fields.</returns>
    public ETAMPModel SignEtampModel(ETAMPModel etamp)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(etamp.Token);

        etamp.SignatureToken = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
        etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(
            Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
        return etamp;
    }

    /// <summary>
    ///     Signs the provided ETAMP model by encoding and appending a signature token and signature message to it.
    /// </summary>
    /// <param name="etamp">The ETAMP model to be signed.</param>
    /// <returns>The signed ETAMP model as a JSON string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="etamp" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the Token property of <paramref name="etamp" /> is null or empty.</exception>
    private byte[] Sign(byte[] data)
    {
        return _ecdsa!.SignData(data, _algorithmName);
    }
}