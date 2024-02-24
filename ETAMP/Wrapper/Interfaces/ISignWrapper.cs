using ETAMP.Models;

namespace ETAMP.Wrapper.Interfaces
{
    internal interface ISignWrapper
    {
        byte[] Sign(byte[] data);

        byte[] Sign(Stream data);

        string SignEtamp(string jsonEtamp);

        string SignEtampModel(ETAMPModel etamp);
    }
}