namespace ETAMP.Services.Interfaces
{
    public interface IVerifyWrapper
    {
        bool VerifyData(string data, string signature);

        bool VerifyData(byte[] data, byte[] signature);

        bool VerifyData(string data, byte[] signature);

        bool VerifyData(byte[] data, string signature);
    }
}