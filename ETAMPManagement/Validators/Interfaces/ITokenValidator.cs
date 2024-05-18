using ETAMPManagement.Models;

namespace ETAMPManagement.Validators.Interfaces;

public interface ITokenValidator
{
    ValidationResult ValidateToken<T>(ETAMPModel<T> model) where T : Token;
}