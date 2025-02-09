namespace ETAMP.Core.Models;

/// <summary>
///     Represents a data model builder for the ETAMP framework.
///     This class is used to encapsulate the properties required for building or reconstructing ETAMP-related data models.
/// </summary>
public record ETAMPModelBuilder
{
    /// <summary>
    ///     Represents a unique identifier that is used to distinguish individual instances of the type.
    /// </summary>
    public Guid Id { get; set; }


    /// <summary>
    ///     Represents the version of the ETAMP model or builder instance.
    ///     This property is used to specify and track the version details of the data structure.
    /// </summary>
    public double Version { get; set; }


    /// <summary>
    ///     Represents a fundamental entity that encapsulates required data, typically serialized as part of ETAMP processes.
    ///     This class is used as a core component for transmitting or storing structured information.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    ///     Represents the type of update associated with the model.
    /// </summary>
    /// <remarks>
    ///     This property is used to indicate the nature of the update or change
    ///     applied to the model. It can help identify the purpose or context of
    ///     the update operation.
    /// </remarks>
    public string? UpdateType { get; set; }


    /// <summary>
    ///     Specifies the type of compression applied to data during serialization or deserialization.
    ///     This property is used to determine the appropriate compression or decompression mechanism.
    /// </summary>
    public string? CompressionType { get; set; }

    /// <summary>
    ///     Represents a message or signature used for data validation, authentication,
    ///     or other integrity purposes within the ETAMP framework.
    ///     This property may store information such as cryptographic signatures
    ///     or other forms of validation/verification messages.
    /// </summary>
    public string? SignatureMessage { get; set; }
}