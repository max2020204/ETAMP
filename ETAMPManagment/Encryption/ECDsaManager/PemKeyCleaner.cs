using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Models;

namespace ETAMPManagment.Encryption.ECDsaManager
{
    public class PemKeyCleaner : IPemKeyCleaner
    {
        public ECDKeyModelProvider KeyModelProvider { get; }

        public PemKeyCleaner()
        {
            KeyModelProvider = new ECDKeyModelProvider();
        }

        public IPemKeyCleaner ClearPEMPrivateKey(string privateKey)
        {
            KeyModelProvider.PrivateKey = ClearPEMFormatting(privateKey);
            return this;
        }

        public IPemKeyCleaner ClearPEMPublicKey(string publicKey)
        {
            KeyModelProvider.PublicKey = ClearPEMFormatting(publicKey);
            return this;
        }

        private static string ClearPEMFormatting(string key)
        {
            return key.Replace("-----BEGIN PRIVATE KEY-----", "")
                      .Replace("-----END PRIVATE KEY-----", "")
                      .Replace("-----BEGIN PUBLIC KEY-----", "")
                      .Replace("-----END PUBLIC KEY-----", "")
                      .Replace("\n", "")
                      .Replace("\r", "");
        }
    }
}