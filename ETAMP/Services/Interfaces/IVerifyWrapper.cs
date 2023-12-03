namespace ETAMP.Services.Interfaces
{
    public interface IVerifyWrapper : IDisposable
    {
        bool VerifyData(string data, string signature);

        bool VerifyData(byte[] data, byte[] signature);

        bool VerifyData(string data, byte[] signature);

        bool VerifyData(byte[] data, string signature);
    }
}