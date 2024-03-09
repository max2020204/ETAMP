using System.Security.Cryptography;
using ETAMPManagment.Encryption.Interfaces;

namespace ETAMPManagment.Encryption
{
    public class KeyPairProvider : IKeyPairProvider
    {
        public string PrivateKey { get; }

        public string PublicKey { get; }
        private readonly ECDiffieHellman _eCDiffieHellman;
        public ECDiffieHellmanPublicKey HellmanPublicKey => _eCDiffieHellman.PublicKey;

        public KeyPairProvider()
        {
            _eCDiffieHellman = ECDiffieHellman.Create();
            PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem();
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        public KeyPairProvider(ECDiffieHellman ecDiffieHellman)
        {
            _eCDiffieHellman = ecDiffieHellman;
            PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem();
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        public KeyPairProvider(ECParameters parameters)
        {
            _eCDiffieHellman = ECDiffieHellman.Create(parameters);
            PrivateKey = _eCDiffieHellman.ExportECPrivateKeyPem();
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        public KeyPairProvider(ECCurve curve)
        {
            _eCDiffieHellman = ECDiffieHellman.Create(curve);
            PrivateKey = _eCDiffieHellman.ExportPkcs8PrivateKeyPem();
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem();
        }

        public virtual ECDiffieHellman GetECDiffieHellman()
        {
            return _eCDiffieHellman;
        }

        public virtual void Dispose()
        {
            _eCDiffieHellman.Dispose();
        }
    }
}