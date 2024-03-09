using ETAMPManagment.Encryption.Interfaces;

namespace ETAMPManagment.Factories.Interfaces
{
    public interface IEncryptionServiceFactory
    {
        Dictionary<string, Func<IEncryptionService>> Services { get; }

        void RegisterEncryptionService(string name, Func<IEncryptionService> serviceCreator);

        IEncryptionService CreateEncryptionService(string name);

        bool UnregisterEncryptionService(string name);
    }
}