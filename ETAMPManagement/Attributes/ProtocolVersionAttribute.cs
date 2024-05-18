namespace ETAMPManagement.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ProtocolVersionAttribute(string protocolVersion) : Attribute
{
    public string ProtocolVersion { get; } = protocolVersion;
}