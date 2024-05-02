#region

using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;

#endregion

namespace ETAMPManagment.Extensions;

/// <summary>
///     Provides functionality to digitally sign an ETAMPModel using a specified signature wrapper.
/// </summary>
public static class ETAMPSign
{
    /// <summary>
    ///     Signs an ETAMPModel using the provided signature wrapper.
    /// </summary>
    /// <param name="model">The ETAMPModel to be signed.</param>
    /// <param name="sign">The ISignWrapper used to apply the digital signature to the model.</param>
    /// <returns>A new ETAMPModel instance that includes the digital signature.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="model" /> or <paramref name="sign" /> is null.</exception>
    public static ETAMPModel Sign(this ETAMPModel model, ISignWrapper sign)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(sign);

        return sign.SignEtampModel(model);
    }
}