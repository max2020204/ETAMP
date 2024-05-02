#region

using ETAMPManagment.Models;

#endregion

namespace ETAMPManagmentTests.Models;

public class Data : BasePayload
{
    public string Name { get; set; }
    public string Surname { get; set; }
}