using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ETAMPManagment.Wrapper
{
    /// <summary>
    /// Provides functionality for signing data using Elliptic Curve Digital Signature Algorithm (ECDsa).
    /// </summary>
    /// <remarks>
    /// This class enables the creation of digital signatures using ECDsa, facilitating multiple initialization methods,
    /// including direct from an ECDsa instance or via an IEcdsaWrapper for enhanced flexibility and custom implementation scenarios.
    /// It supports signing operations on byte arrays, streams, and JSON representations of models.
    /// </remarks>
    public class SignWrapper : ISignWrapper
    {
        private readonly ECDsa? _ecdsa;
        private readonly HashAlgorithmName _algorithmName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignWrapper"/> class using an IECDsaProvider.
        /// The ECDsa instance for signing is obtained from the provided IECDsaProvider.
        /// </summary>
        /// <param name="ecdsaProvider">The IECDsaProvider to use for obtaining the ECDsa instance.</param>
        /// <param name="algorithmName">The hash algorithm to use for signing.</param>
        public SignWrapper(IECDsaProvider ecdsaProvider, HashAlgorithmName algorithmName)
        {
            if (ecdsaProvider == null)
                throw new ArgumentNullException(nameof(ecdsaProvider), "IECDsaProvider instance cannot be null.");

            _ecdsa = ecdsaProvider.GetECDsa();
            if (_ecdsa == null)
                throw new InvalidOperationException("ECDsa instance cannot be null after extraction from IECDsaProvider.");

            _algorithmName = algorithmName;
        }

        private byte[] Sign(byte[] data)
        {
            return _ecdsa.SignData(data, _algorithmName);
        }

        /// <summary>
        /// Signs the ETAMP data given as a JSON string and updates its signature fields.
        /// </summary>
        /// <param name="jsonEtamp">The ETAMP data in JSON format.</param>
        /// <returns>A JSON string of the ETAMP data with updated signature fields.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input JSON string is null, empty,
        /// or cannot be deserialized into an ETAMP model.</exception>
        public virtual string SignEtamp(string jsonEtamp)
        {
            ETAMPModel? etamp = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);
            ArgumentNullException.ThrowIfNull(etamp);

            etamp.SignatureToken = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }

        /// <summary>
        /// Signs the ETAMP model and updates its signature fields.
        /// </summary>
        /// <param name="etamp">The ETAMP model to be signed.</param>
        /// <returns>A JSON string of the ETAMP data with updated signature fields.</returns>
        public virtual string SignEtamp(ETAMPModel etamp)
        {
            etamp.SignatureToken = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }

        /// <summary>
        /// Signs the ETAMP model and returns the model with updated signature fields.
        /// </summary>
        /// <param name="etamp">The ETAMP model to be signed.</param>
        /// <returns>The ETAMP model with updated signature fields.</returns>
        public virtual ETAMPModel SignEtampModel(ETAMPModel etamp)
        {
            etamp.SignatureToken = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Base64UrlEncoder.Encode(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return etamp;
        }
    }
}