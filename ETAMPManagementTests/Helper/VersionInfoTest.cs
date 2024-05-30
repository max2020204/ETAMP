using System.Reflection;
using ETAMPManagement.Attributes;
using ETAMPManagement.Helper;
using JetBrains.Annotations;
using Xunit;

namespace ETAMPManagementTests.Helper;

[TestSubject(typeof(VersionInfo))]
public class VersionInfoTest
{
    [Fact]
    public void GetVersionInfo_ShouldSetCorrectValues()
    {
        var assembly = Assembly.GetAssembly(typeof(VersionInfo));
        var version = assembly!.GetName().Version;
        var protocolVersion =
            (ProtocolVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(ProtocolVersionAttribute))!;

        var versionInfo = new VersionInfo();
        versionInfo.GetVersionInfo();

        Assert.Equal(double.Parse(protocolVersion.ProtocolVersion), versionInfo.ProtocolVersion);
        Assert.Equal($"{version!.Major}.{version.Minor}.{version.Build}", versionInfo.FullVersion);
    }
}