using ETAMP.Wrapper.Interfaces;

namespace ETAMP.Services.Interfaces
{
    public interface IEciesEncryptionService
    {
        IEcdhKeyWrapper EcdhKeyWrapper { get; }
        IEncryptionService EncryptionService { get; }

        string Encrypt(string message);

        string Decrypt(string encryptedMessageBase64, byte[] publicKey);
    }
}