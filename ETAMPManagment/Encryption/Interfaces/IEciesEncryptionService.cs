using ETAMPManagment.Wrapper.Interfaces;

namespace ETAMPManagment.Encryption.Interfaces
{
    public interface IEciesEncryptionService
    {
        string Encrypt(string message);

        string Decrypt(string encryptedMessageBase64, byte[] publicKey);
    }
}