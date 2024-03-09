using ETAMPManagment.Models;

namespace ETAMPManagment.Wrapper.Interfaces
{
    public interface ISignWrapper
    {
        string SignEtamp(string jsonEtamp);

        string SignEtamp(ETAMPModel etamp);

        ETAMPModel SignEtampModel(ETAMPModel etamp);
    }
}