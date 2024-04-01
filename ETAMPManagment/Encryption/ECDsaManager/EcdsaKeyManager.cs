using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.ECDsaManager
{
    public class EcdsaKeyManager : IEcdsaKeyManager
    {
        private readonly IECDsaRegistrar _ecdsaRegistrar;

        internal EcdsaKeyManager(IECDsaRegistrar ecdsaRegistrar)
        {
            _ecdsaRegistrar = ecdsaRegistrar;
        }

        public IECDsaProvider ImportECDsa(byte[] privateKey, ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        public IECDsaProvider ImportECDsa(byte[] privateKey)
        {
            ECDsa ecdsa = ECDsa.Create();
            ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        public IECDsaProvider ImportECDsa(string privateKey, ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        public IECDsaProvider ImportECDsa(string privateKey)
        {
            ECDsa ecdsa = ECDsa.Create();
            ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }
    }
}