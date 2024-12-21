using System.Reflection;
using ETAMP.Core.Attributes;

namespace ETAMP.Core;

/// <summary>
///     Represents version information for the application.
/// </summary>
public class VersionInfo
{
    /// <summary>
    ///     Gets the protocol version of the ETAMP.
    /// </summary>
    /// <remarks>
    ///     This property is used to retrieve the protocol version number of the ETAMP.
    ///     The protocol version number is specified using the ProtocolVersionAttribute in the executing assembly.
    /// </remarks>
    public double ProtocolVersion { get; private set; }

    /// <summary>
    ///     Gets the full version of the ETAMP framework.
    /// </summary>
    /// <remarks>
    ///     This property is used to retrieve the full version number of the ETAMP framework.
    ///     The full version number is calculated from the Major, Minor, and Compress components of the version information.
    /// </remarks>
    public string? FullVersion { get; private set; }

    public void GetVersionInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        if (version != null)
            FullVersion = $"{version.Major}.{version.Minor}.{version.Build}";
        var protocolVersion =
            (ProtocolVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(ProtocolVersionAttribute))!;
        ProtocolVersion = double.Parse(protocolVersion.ProtocolVersion);
    }
}