#region

using ETAMPManagement.Models;

#endregion

namespace ETAMPManagementTests.Models;

public class Data : BasePayload
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
}