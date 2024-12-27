using ETAMP.Core;
using ETAMP.Core.Management;
using ETAMP.Core.Models;

namespace ETAMP.Console.CreateETAMP;

public class Create
{
    /// <summary>
    /// Creates an ETAMP model by initializing a protocol instance and a token model. The token model
    /// is populated with predefined data and used as the payload for the resulting ETAMP model.
    /// </summary>
    /// <returns>
    /// Returns an instance of <see cref="ETAMP.Core.Models.ETAMPModel{T}"/> where the generic type is <see cref="ETAMP.Console.CreateETAMP.Models.TokenModel"/>.
    /// The returned model contains information such as update type, compression type, and the token payload.
    /// </returns>
    public ETAMPModel<TokenModel> CreateETAMP()
    {
        var protocol = new ETAMPProtocol();
        var tokenModel = new TokenModel
        {
            Message = "Hello World!",
            Email = "<EMAIL>",
            Data = "Some data",
            IsEncrypted = false,
            LastName = "Last",
            Name = "Name",
            Phone = "+1234567890"
        };

        return protocol.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip);
    }
}