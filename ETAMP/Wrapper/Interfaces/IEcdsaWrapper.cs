using System.Security.Cryptography;

namespace ETAMP.Wrapper.Interfaces
{
    public interface IEcdsaWrapper
    {
        ECDsa CreateECDsa();

        ECDsa CreateECDsa(ECCurve curve);

        ECDsa CreateECDsa(string publicKey, ECCurve curve);

        ECDsa CreateECDsa(byte[] publicKey, ECCurve curve);

        ECDsa ImportECDsa(byte[] privateKey, ECCurve curve);

        ECDsa ImportECDsa(string privateKey, ECCurve curve);

        string ClearPEMPrivateKey(string privateKey);

        string ClearPEMPublicKey(string publicKey);
    }
}