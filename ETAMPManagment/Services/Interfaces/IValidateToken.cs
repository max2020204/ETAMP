using ETAMPManagment.Wrapper.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Services.Interfaces
{
    public interface IValidateToken
    {
        Task<bool> FullVerify(string etamp, string audience, string issuer);

        Task<bool> FullVerifyLite(string etamp, ECCurve curve, string publicKey, IEcdsaWrapper ecdsaWrapper);

        Task<bool> FullVerifyLite(string etamp, ECDsa ecdsa);

        Task<bool> FullVerifyWithTokenSignature(string etamp, string audience, string issuer, ECCurve curve, string publicKey);

        Task<bool> FullVerifyWithTokenSignature(string etamp, string audience, string issuer, ECDsa ecdsa);

        bool VerifyETAMP(string etamp);

        Task<bool> VerifyLifeTime(string token);
    }
}