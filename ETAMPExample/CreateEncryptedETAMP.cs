using System.Security.Cryptography;
using ETAMPManagement.Encryption.Base;
using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.ETAMP.Interfaces;
using ETAMPManagement.Extensions.Builder;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Management;
using ETAMPManagement.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPExample;

public class CreateEncryptedETAMP
{
    private readonly ServiceProvider _provider;

    public CreateEncryptedETAMP(ServiceProvider provider)
    {
        _provider = provider;
    }


    /// <summary>
    ///     Creates an encrypted ETAMP message.
    /// </summary>
    /// <returns>The encrypted ETAMP message.</returns>
    public string CreateEncryptedETAMPMessage()
    {
        var etampBase = _provider.GetService<IETAMPBase>();
        var ecies = _provider.GetService<ECIESEncryptionServiceBase>();
        var compression = _provider.GetService<ICompressionServiceFactory>();
        var keyExchanger = _provider.GetService<KeyExchangerBase>();
        var keyPairProvider = _provider.GetService<KeyPairProviderBase>();
        var aes = _provider.GetService<IEncryptionService>();

        keyPairProvider.Initialize(ECDiffieHellman.Create());
        keyExchanger.Initialize(keyPairProvider);
        keyExchanger.DeriveKey(ECDiffieHellman.Create().PublicKey);
        ecies.Initialize(keyExchanger, aes);
        var token = CreateToken("SomeDataInJson");
        var etamp = etampBase.CreateETAMPModel("Message", token, CompressionNames.Deflate);

        return etamp.EncryptData(ecies).Build(compression);
    }


    /// <summary>
    ///     Create a token object with the specified data.
    /// </summary>
    /// <param name="data">The data for the token.</param>
    /// <returns>The created token object.</returns>
    private Token CreateToken(string data)
    {
        return new Token
        {
            Data = data
        };
    }
}