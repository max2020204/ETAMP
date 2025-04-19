namespace ETAMP.Core.Management;

/// <summary>
/// Provides constant string values representing encryption algorithm names.
/// </summary>
/// <remarks>
/// The constants defined in this class serve as identifiers for encryption algorithms.
/// They are used in conjunction with dependency injection to select the appropriate
/// encryption service implementations.
/// </remarks>
public class EncryptionNames
{
    /// <summary>
    /// Represents the Advanced Encryption Standard (AES) algorithm name as a string constant.
    /// </summary>
    /// <remarks>
    /// This constant is used as an identifier for the AES encryption algorithm and is
    /// commonly utilized in service configuration to register the appropriate encryption
    /// service implementation.
    /// </remarks>
    public const string AES = "AES";

    /// <summary>
    /// Represents a constant string value for the encryption algorithm "AESGcm".
    /// </summary>
    /// <remarks>
    /// This constant is used to specify the AES-GCM (Galois/Counter Mode) encryption method.
    /// It serves as an identifier for selecting the appropriate encryption service
    /// implementation within the dependency injection framework.
    /// </remarks>
    public const string AESGcm = "AESGcm";
}