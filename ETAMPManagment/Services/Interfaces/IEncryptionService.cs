namespace ETAMPManagment.Services.Interfaces
{
    public interface IEncryptionService
    {
        byte[] Encrypt(byte[] data, byte[] key);

        byte[] Decrypt(byte[] data, byte[] key);
    }
}