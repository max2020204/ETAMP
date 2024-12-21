using System.Text;
using ETAMP.Core;
using ETAMP.Core.Models;
using ETAMP.Wrapper.Base;
using Newtonsoft.Json;

namespace ETAMP.Wrapper;

/// <summary>
///     Signs data using Elliptic Curve Digital Signature Algorithm (ECDsa).
/// </summary>
public sealed class SignWrapper : SignWrapperBase
{
    /// <summary>
    ///     SignEtampModel method signs an ETAMPModel instance and updates the signature fields.
    /// </summary>
    /// <typeparam name="T">The type of the token.</typeparam>
    /// <param name="etamp">The ETAMPModel instance to sign.</param>
    /// <returns>The signed ETAMPModel instance.</returns>
    /// <exception cref="ArgumentException">Thrown if etamp.Token is null.</exception>
    public override ETAMPModel<T> SignEtampModel<T>(ETAMPModel<T> etamp)
    {
        ArgumentNullException.ThrowIfNull(etamp.Token);

        var token = JsonConvert.SerializeObject(etamp.Token);
        etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(
            Encoding.UTF8.GetBytes(
                $"{etamp.Id}{etamp.Version}{token}{etamp.UpdateType}{etamp.CompressionType}")));
        return etamp;
    }

    private byte[] Sign(byte[] data)
    {
        return Ecdsa!.SignData(data, AlgorithmName);
    }
}