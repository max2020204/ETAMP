#region

using ETAMPManagement.Models;
using Xunit;

#endregion

namespace ETAMPManagementTests.Models;

public class BasePayloadTests
{
    [Fact]
    public void Constructor_WithSpecificValues_InitializesPropertiesCorrectly()
    {
        var expectedJTI = Guid.NewGuid();
        var expectedIssuedAt = DateTime.UtcNow;
        var expectedExpires = expectedIssuedAt.AddHours(1);

        var payload = new BasePayload(expectedJTI, expectedIssuedAt, expectedExpires);

        Assert.Equal(expectedJTI, payload.JTI);
        Assert.Equal(expectedIssuedAt, payload.IssuedAt.UtcDateTime);
        Assert.Equal(expectedExpires, payload.Expires.UtcDateTime);
    }
}