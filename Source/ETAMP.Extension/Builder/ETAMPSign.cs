#region

using ETAMP.Core.Models;
using ETAMP.Wrapper.Interfaces;

#endregion

namespace ETAMP.Extension.Builder;

/// <summary>
///     Provides functionality to digitally sign an ETAMPModel using a specified signature wrapper.
/// </summary>
public static class ETAMPSign
{
    /// <summary>
    ///     Provides functionality to digitally sign an ETAMPModel using a specified signature wrapper.
    /// </summary>
    /// <typeparam name="T">The type of Token.</typeparam>
    /// <param name="model">The ETAMPModel to be signed.</param>
    /// <param name="sign">The instance of ISignWrapper used for signing.</param>
    /// <returns>The signed ETAMPModel.</returns>
    public static async Task<ETAMPModel<T>> Sign<T>(this ETAMPModel<T> model, ISignWrapper sign) where T : Token
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(sign);

        return await sign.SignEtampModel(model);
    }
}