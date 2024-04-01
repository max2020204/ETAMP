using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.ECDsaManager
{
    public class EcdsaCreator : IEcdsaCreator
    {
        private readonly IECDsaRegistrar _ecdsaRegistrar;

        internal EcdsaCreator(IECDsaRegistrar ecdsaRegistrar)
        {
            _ecdsaRegistrar = ecdsaRegistrar;
        }

        public IECDsaProvider CreateECDsa()
        {
            ECDsa ecdsa = ECDsa.Create();
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        public IECDsaProvider CreateECDsa(ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        public IECDsaProvider CreateECDsa(string publicKey, ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }

        public IECDsaProvider CreateECDsa(byte[] publicKey, ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
            return _ecdsaRegistrar.RegisterEcdsa(ecdsa);
        }
    }
}