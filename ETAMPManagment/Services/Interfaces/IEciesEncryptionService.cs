using ETAMPManagment.Wrapper.Interfaces;

namespace ETAMPManagment.Services.Interfaces
{
    public interface IEciesEncryptionService
    {
        IEcdhKeyWrapper EcdhKeyWrapper { get; }
        IEncryptionService EncryptionService { get; }

        string Encrypt(string message);

        string Decrypt(string encryptedMessageBase64, byte[] publicKey);
    }
}