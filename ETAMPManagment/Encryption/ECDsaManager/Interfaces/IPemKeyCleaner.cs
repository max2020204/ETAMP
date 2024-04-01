using ETAMPManagment.Models;

namespace ETAMPManagment.Encryption.ECDsaManager.Interfaces
{
    public interface IPemKeyCleaner
    {
        ECDKeyModelProvider KeyModelProvider { get; }

        IPemKeyCleaner ClearPEMPrivateKey(string privateKey);

        IPemKeyCleaner ClearPEMPublicKey(string publicKey);
    }
}