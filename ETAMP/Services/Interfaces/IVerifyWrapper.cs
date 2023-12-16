using System.Security.Cryptography;

namespace ETAMP.Services.Interfaces
{
    public interface IVerifyWrapper : IDisposable
    {
        ECDsa ECDsa { get; }

        bool VerifyData(string data, string signature);

        bool VerifyData(byte[] data, byte[] signature);

        bool VerifyData(string data, byte[] signature);

        bool VerifyData(byte[] data, string signature);
    }
}