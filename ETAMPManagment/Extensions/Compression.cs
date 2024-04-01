using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Newtonsoft.Json;

namespace ETAMPManagment.Extensions
{
    /// <summary>
    /// Provides compression and decompression utilities for ETAMPModel objects.
    /// </summary>
    public static class Compression
    {
        /// <summary>
        /// Compresses the ETAMPModel instance into a compressed string format.
        /// </summary>
        /// <param name="model">The ETAMPModel instance to be compressed.</param>
        /// <param name="compressionServiceFactory">A factory method that creates an instance of ICompressionService.</param>
        /// <returns>A compressed string representation of the ETAMPModel.</returns>
        public static string Compress(this ETAMPModel model, Func<ICompressionService> compressionServiceFactory)
        {
            // Utilizes the provided compression service to compress the model's string representation
            ArgumentNullException.ThrowIfNull(nameof(model));
            ArgumentNullException.ThrowIfNull(nameof(compressionServiceFactory));

            return compressionServiceFactory().CompressString(model.ToString());
        }

        /// <summary>
        /// Decompresses a string into an ETAMPModel instance.
        /// </summary>
        /// <param name="jsonEtamp">The compressed string representation of the ETAMPModel.</param>
        /// <param name="compressionServiceFactory">A factory method that creates an instance of ICompressionService.</param>
        /// <returns>The decompressed ETAMPModel instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided jsonEtamp string is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if the decompressed string is not in a valid JSON format.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the compression service fails to decompress the string.</exception>
        public static ETAMPModel Decompress(this string jsonEtamp, Func<ICompressionService> compressionServiceFactory)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(jsonEtamp));
            ArgumentNullException.ThrowIfNull(nameof(compressionServiceFactory));

            var compressionService = compressionServiceFactory() ?? throw new InvalidOperationException("Failed to create a compression service instance.");

            string decompressedString;
            try
            {
                decompressedString = compressionService.DecompressString(jsonEtamp);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Decompression of the ETAMP string failed.", ex);
            }

            try
            {
                return JsonConvert.DeserializeObject<ETAMPModel>(decompressedString)
                    ?? throw new InvalidOperationException("Decompressed string resulted in a null ETAMPModel.");
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("The decompressed string is not in a valid JSON format.", nameof(jsonEtamp), ex);
            }
        }
    }
}