#region

using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;

#endregion

namespace ETAMPManagment.ETAMP.Base;

/// <summary>
///     This class represents the base implementation of the ETAMP (Encrypted Token And Message Protocol) functionality.
/// </summary>
public sealed class ETAMPBase : IETAMPBase
{
    /// <summary>
    ///     Represents the ETAMP data processing for creating digitally signed ETAMP token data.
    /// </summary>
    /// <remarks>
    ///     This class provides the functionality to create ETAMP tokens and models,
    ///     supporting dynamic encryption and digital signing capabilities.
    /// </remarks>
    private readonly IETAMPData _etampData;

    /// <summary>
    ///     Represents a provider for creating signing credentials used in the authentication process.
    /// </summary>
    private readonly ISigningCredentialsProvider _signingCredentialsProvider;


    /// <summary>
    ///     Represents the ETAMPBase class.
    /// </summary>
    public ETAMPBase(ISigningCredentialsProvider signingCredentialsProvider, IETAMPData etampData)
    {
        _signingCredentialsProvider = signingCredentialsProvider ??
                                      throw new ArgumentNullException(nameof(signingCredentialsProvider));
        _etampData = etampData ?? throw new ArgumentNullException(nameof(etampData));
    }


    /// <summary>
    ///     Creates an ETAMPModel based on the specified update type, payload, version, and signing credentials provider.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    /// <param name="updateType">The update type.</param>
    /// <param name="payload">The payload.</param>
    /// <param name="version">The version of the ETAMPModel. Default is 1.</param>
    /// <param name="provider">The signing credentials provider. Optional.</param>
    /// <returns>An instance of the ETAMPModel class.</returns>
    public ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1,
        ISigningCredentialsProvider? provider = null) where T : BasePayload
    {
        var credentialsProvider = provider ?? _signingCredentialsProvider;
        var messageId = Guid.NewGuid();
        return new ETAMPModel
        {
            Id = messageId,
            Version = version,
            Token = _etampData.CreateEtampData(messageId.ToString(), payload, credentialsProvider),
            UpdateType = updateType
        };
    }
}