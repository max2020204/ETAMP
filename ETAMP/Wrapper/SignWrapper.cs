using ETAMP.Models;
using ETAMP.Wrapper.Interfaces;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ETAMP.Wrapper
{
    internal class SignWrapper : ISignWrapper
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

        public byte[] Sign(byte[] data)
        {
            return _ecdsa.SignData(data, _algorithmName);
        }

        public byte[] Sign(Stream data)
        {
            return _ecdsa.SignData(data, _algorithmName);
        }

        public string SignEtamp(string jsonEtamp)
        {
            ETAMPModel etamp = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }

        public string SignEtampModel(ETAMPModel etamp)
        {
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }
    }
}