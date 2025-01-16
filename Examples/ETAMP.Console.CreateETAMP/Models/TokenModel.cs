#region

using ETAMP.Core.Models;

#endregion

namespace ETAMP.Console.CreateETAMP.Models;

public class TokenModel : Token
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Message { get; set; }
}