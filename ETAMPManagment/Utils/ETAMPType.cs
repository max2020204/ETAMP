namespace ETAMPManagment.Utils
{
    /// <summary>
    /// Defines the types of ETAMP generators based on their functionality.
    /// </summary>
    public enum ETAMPType
    {
        /// <summary>
        /// Represents a basic ETAMP generator without additional security features like signing or encryption.
        /// </summary>
        Base,

        /// <summary>
        /// Represents an ETAMP generator that encrypts the token for confidentiality.
        /// </summary>
        Encrypted,
    }
}