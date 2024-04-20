namespace ETAMPManagment.Managment
{
    /// <summary>
    /// Defines the constants for different types of ETAMP (Encrypted Token And Message Protocol) models.
    /// These constants are used throughout the application to specify and distinguish between the various ETAMP model types.
    /// </summary>
    public class ETAMPTypeNames
    {
        /// <summary>
        /// Represents the base type of ETAMP model.
        /// This constant is used when a basic ETAMP model is required without additional encryption or specialized processing.
        /// </summary>
        public const string Base = "Base";

        /// <summary>
        /// Represents the encrypted type of ETAMP model.
        /// This constant is used to specify that the ETAMP model should be encrypted, providing additional security for the contained data.
        /// </summary>
        public const string Encrypted = "Encrypted";
    }
}