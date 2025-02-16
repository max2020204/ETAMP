using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

public static class ECDKeyModelProviderExtensions
{
    public static void ClearPemFormatting(this ECDKeyModelProvider model)
    {
        model.PrivateKey.Replace("-----BEGIN PRIVATE KEY-----", "")
            .Replace("-----END PRIVATE KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "");
        model.PublicKey.Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "");
    }
}