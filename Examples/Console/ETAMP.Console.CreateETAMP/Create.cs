using ETAMP.Console.CreateETAMP.Models;
using ETAMP.Core;
using ETAMP.Core.Management;
using ETAMP.Core.Models;


namespace ETAMP.Console.CreateETAMP;

public class Create
{
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