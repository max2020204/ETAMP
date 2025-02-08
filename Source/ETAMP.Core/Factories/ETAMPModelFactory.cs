using ETAMP.Core.Interfaces;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;

namespace ETAMP.Core.Factories;

public class ETAMPModelFactory : IETAMPBase
{
    private readonly VersionInfo _version;

    public ETAMPModelFactory(VersionInfo version)
    {
        _version = version;
    }

    public ETAMPModel<T> CreateETAMPModel<T>(string updateType, T payload, string compressionType) where T : Token
    {
        var messageId = Guid.NewGuid();
        payload.MessageId = messageId;
        return new ETAMPModel<T>
        {
            Id = messageId,
            Version = _version.ProtocolVersion,
            Token = payload,
            UpdateType = updateType,
            CompressionType = compressionType
        };
    }
}