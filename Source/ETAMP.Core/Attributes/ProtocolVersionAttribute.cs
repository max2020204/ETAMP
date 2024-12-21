namespace ETAMP.Core.Attributes;

/// <summary>
///     Represents an attribute used to specify the protocol version of an assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public class ProtocolVersionAttribute(string protocolVersion) : Attribute
{
    /// <summary>
    ///     Represents version information for the application.
    /// </summary>
    public string ProtocolVersion { get; } = protocolVersion;
}