using System.Security.Cryptography;
using ETAMPManagement.Encryption.ECDsaManager.Interfaces;
using ETAMPManagement.ETAMP.Interfaces;
using ETAMPManagement.Extensions.Builder;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Management;
using ETAMPManagement.Models;
using ETAMPManagement.Wrapper.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPExample;

/// <summary>
///     The <see cref="CreateSignETAMP" /> class provides functionality to create and sign ETAMP messages.
/// </summary>
public class CreateSignETAMP
{
    /// <summary>
    ///     Represents a provider used for managing dependencies and providing instances of services.
    /// </summary>
    private readonly ServiceProvider _provider;

    /// <summary>
    ///     The CreateSignETAMP class is responsible for creating and signing ETAMP messages.
    /// </summary>
    public CreateSignETAMP(ServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    ///     Provides an interface for managing and accessing an ECDsa instance for cryptographic operations.
    /// </summary>
    public IECDsaProvider ECDsaProvider { get; private set; }

    /// <summary>
    ///     Represents the hash algorithm used for signing and validation.
    /// </summary>
    public HashAlgorithmName HashAlgorithm { get; private set; }

    /// <summary>
    ///     Creates and signs an ETAMP message.
    /// </summary>
    /// <returns>The signed ETAMP message.</returns>
    public string CreateAndSignETAMPMessage()
    {
        var etampBase = _provider.GetService<IETAMPBase>();
        var sign = _provider.GetService<SignWrapperBase>();
        var creator = _provider.GetService<IECDsaCreator>();
        var compression = _provider.GetService<ICompressionServiceFactory>();

        InitializeSignature(sign, creator);

        var token = CreateToken("SomeDataInJson");
        var etamp = etampBase.CreateETAMPModel("Message", token, CompressionNames.Deflate);

        return etamp.Sign(sign).Build(compression);
    }

    /// <summary>
    ///     Initializes the signature by setting up the sign wrapper and the ECCdsa provider.
    /// </summary>
    /// <param name="sign">The sign wrapper instance.</param>
    /// <param name="creator">The ECCdsa creator instance.</param>
    private void InitializeSignature(SignWrapperBase sign, IECDsaCreator creator)
    {
        ECDsaProvider = creator.CreateECDsa();
        HashAlgorithm = HashAlgorithmName.SHA512;
        sign.Initialize(ECDsaProvider, HashAlgorithm);
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