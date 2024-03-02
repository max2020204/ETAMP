namespace ETAMPManagment.Services.Interfaces
{
    public interface IEncryptionServiceFactory
    {
        Dictionary<string, Func<IEncryptionService>> Services { get; }

        void RegisterEncryptionService(string name, Func<IEncryptionService> serviceCreator);

        IEncryptionService CreateEncryptionService(string name);
    }
}