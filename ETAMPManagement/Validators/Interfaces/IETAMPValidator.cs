#region

using ETAMPManagement.Models;

#endregion

namespace ETAMPManagement.Validators.Interfaces;

public interface IETAMPValidator
{
    ValidationResult ValidateETAMP<T>(ETAMPModel<T> etamp, bool validateLite) where T : Token;
}