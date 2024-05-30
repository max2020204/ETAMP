using ETAMPManagement.ETAMP.Interfaces;
using ETAMPManagement.Extensions.Builder;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Management;
using ETAMPManagement.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPExample;

/// <summary>
///     The CreateETAMP class is responsible for creating an ETAMP message.
///     It uses the IETAMPBase interface to create an ETAMP model and the ICompressionServiceFactory interface for
///     compression.
/// </summary>
public class CreateETAMP
{
    /// <summary>
    ///     Represents the provider used in the CreateETAMP class.
    /// </summary>
    /// <remarks>
    ///     The provider is responsible for providing instances of various services required by the CreateETAMP class.
    /// </remarks>
    private readonly ServiceProvider _provider;

    /// <summary>
    ///     Represents a class for creating an ETAMP message.
    /// </summary>
    public CreateETAMP(ServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    ///     This method creates an ETAMP message.
    /// </summary>
    /// <returns>The ETAMP message string.</returns>
    public string CreateETAMPMessage()
    {
        var etampBase = _provider.GetService<IETAMPBase>();
        var compression = _provider.GetService<ICompressionServiceFactory>();

        var token = CreateToken("SomeDataInJson");
        var etamp = etampBase.CreateETAMPModel("Message", token, CompressionNames.Deflate);

        return etamp.Build(compression);
    }

    /// <summary>
    ///     Creates a token object with the given data.
    /// </summary>
    /// <param name="data">The data to be stored in the token.</param>
    /// <returns>The created token object.</returns>
    private Token CreateToken(string data)
    {
        return new Token
        {
            Data = data
        };
    }
}