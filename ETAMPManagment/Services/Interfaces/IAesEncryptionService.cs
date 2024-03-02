namespace ETAMPManagment.Services.Interfaces
{
    public interface IAesEncryptionService : IEncryptionService
    {
        byte[] IV { get; }
    }
}