using ETAMPManagement.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Xunit;

namespace ETAMPManagementTests.Models;

[TestSubject(typeof(Token))]
public class TokenTest
{
    // Test for default values
    [Fact]
    public void DefaultConstructor_Sets_DefaultValues()
    {
        var token = new Token();
        Assert.NotEqual(Guid.Empty, token.Id);
        Assert.Equal(Guid.Empty, token.MessageId);
        Assert.False(token.IsEncrypted);
        Assert.Null(token.Data);
        Assert.True(Math.Abs((DateTimeOffset.UtcNow - token.TimeStamp).TotalSeconds) < 1);
    }

    [Fact]
    public void SetData_CanSerializeObject()
    {
        var token = new Token();
        var input = new { Property = "Test data" };
        token.SetData(input);
        Assert.NotNull(token.Data);

        var result = JsonConvert.DeserializeObject<dynamic>(token.Data);

        Assert.NotNull(result);
        string res = result["Property"];
        Assert.Equal(input.Property, res);
    }

    [Fact]
    public void GetData_ReturnsNull_IfDataIsNull()
    {
        var token = new Token();
        var result = token.GetData<dynamic>();
        Assert.Null(result);
    }

    [Fact]
    public void GetData_ReturnsNull_IfDataIsInvalidJson()
    {
        var token = new Token();
        token.Data = "Invalid JSON";
        var result = token.GetData<dynamic>();
        Assert.Null(result);
    }

    [Fact]
    public void GetData_ReturnsObject_IfDataIsValidJson()
    {
        var token = new Token();
        token.SetData(new { Property = "Test data" });

        var result = token.GetData<dynamic>();
        Assert.NotNull(result);
    }
}