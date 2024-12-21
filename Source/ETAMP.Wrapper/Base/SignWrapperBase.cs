using ETAMP.Core.Models;
using ETAMP.Wrapper.Helper;
using ETAMP.Wrapper.Interfaces;

namespace ETAMP.Wrapper.Base;

/// <summary>
///     Provides a base class for sign wrapper implementation.
/// </summary>
public abstract class SignWrapperBase : InitializeWrapper, ISignWrapper
{
    /// <summary>
    ///     Signs the provided ETAMP model by generating a signature for the model's token and message.
    ///     Must be implemented by derived classes.
    /// </summary>
    public abstract ETAMPModel<T> SignEtampModel<T>(ETAMPModel<T> etamp) where T : Token;
}