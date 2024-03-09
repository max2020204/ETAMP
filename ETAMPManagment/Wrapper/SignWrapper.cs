using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ETAMPManagment.Wrapper
{
    /// <summary>
    /// Provides functionality for signing data using Elliptic Curve Digital Signature Algorithm (ECDsa).
    /// </summary>
    /// <remarks>
    /// This class allows for the creation of digital signatures using ECDsa, supporting multiple initializations
    /// including from a private key, an <see cref="ECDsa"/> instance, or an <see cref="IEcdsaWrapper"/> for custom
    /// implementation scenarios. It supports signing byte arrays, streams, and JSON representations of specific models.
    /// </remarks>
    public class SignWrapper : ISignWrapper
    {
        private readonly ECDsa _ecdsa;
        private readonly HashAlgorithmName _algorithmName;

        public SignWrapper(ECDsa ecdsa, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsa;
            _algorithmName = algorithmName;
        }

        public SignWrapper(string privateKey, HashAlgorithmName algorithmName)
        {
            _ecdsa = ECDsa.Create();
            _ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
            _algorithmName = algorithmName;
        }

        public SignWrapper(IEcdsaWrapper ecdsaWrapper, string privateKey, ECCurve curve, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsaWrapper.ImportECDsa(privateKey, curve);
            _algorithmName = algorithmName;
        }

        public SignWrapper(IEcdsaWrapper ecdsaWrapper, byte[] privateKey, ECCurve curve, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsaWrapper.ImportECDsa(privateKey, curve);
            _algorithmName = algorithmName;
        }

        private byte[] Sign(byte[] data)
        {
            return _ecdsa.SignData(data, _algorithmName);
        }

        public virtual string SignEtamp(string jsonEtamp)
        {
            ETAMPModel etamp = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }

        public virtual string SignEtamp(ETAMPModel etamp)
        {
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }

        public virtual ETAMPModel SignEtampModel(ETAMPModel etamp)
        {
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return etamp;
        }
    }
}