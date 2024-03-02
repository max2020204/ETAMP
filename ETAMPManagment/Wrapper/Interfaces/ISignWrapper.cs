using ETAMPManagment.Models;

namespace ETAMPManagment.Wrapper.Interfaces
{
    internal interface ISignWrapper
    {
        byte[] Sign(byte[] data);

        byte[] Sign(Stream data);

        string SignEtamp(string jsonEtamp);

        string SignEtampModel(ETAMPModel etamp);
    }
}