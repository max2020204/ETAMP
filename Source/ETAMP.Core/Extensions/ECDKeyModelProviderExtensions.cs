using ETAMP.Core.Models;

namespace ETAMP.Core.Extensions;

/// <summary>
///     Provides extension methods for the <see cref="ECDKeyModelProvider" /> class.
/// </summary>
public static class ECDKeyModelProviderExtensions
{
    /// <summary>
    ///     Removes PEM formatting from both public and private keys by stripping prefix, suffix, and line breaks.
    /// </summary>
    /// <param name="model">The key model provider containing the PEM-formatted public and private keys.</param>
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