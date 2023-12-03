using ETAMP.Models;
using System.Security.Cryptography;

namespace ETAMP.Interfaces
{
    public interface IEtamp
    {
        ECCurve Curve { get; }
        ECDsa Ecdsa { get; }
        HashAlgorithmName HashAlgorithm { get; }
        string PrivateKey { get; }
        string PublicKey { get; }
        string SecurityAlgorithm { get; }

        string CreateETAMP<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod;

        string CreateETAMPWithoutSignature<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod;
    }
}