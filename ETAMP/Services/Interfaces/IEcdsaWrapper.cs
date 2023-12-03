using System.Security.Cryptography;

namespace ETAMP.Services.Interfaces
{
    public interface IEcdsaWrapper
    {
        ECDsa CreateECDsa();

        ECDsa CreateECDsa(ECCurve curve);

        ECDsa CreateECDsa(string publicKey, ECCurve curve);

        ECDsa CreateECDsa(byte[] publicKey, ECCurve curve);
    }
}