using ETAMPManagment.Models;
using System.Diagnostics.CodeAnalysis;

namespace ETAMPManagment.Compares
{
    /// <summary>
    /// Provides methods to compare two instances of <see cref="ETAMPModel"/> for equality.
    /// This comparison is based on the parameters of the ETAMP models, ensuring that two instances
    /// are considered equal if all their relevant data fields match.
    /// </summary>
    public class ETAMPCompare : IEqualityComparer<ETAMPModel>
    {
        /// <summary>
        /// Compares two instances of <see cref="ETAMPModel"/> for equality based on their properties.
        /// </summary>
        /// <param name="x">The first <see cref="ETAMPModel"/> instance to compare.</param>
        /// <param name="y">The second <see cref="ETAMPModel"/> instance to compare.</param>
        /// <returns>Returns <c>true</c> if both instances are considered equal; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Two instances are considered equal if their Id, Version, UpdateType, Token, SignatureToken,
        /// and SignatureMessage properties are the same. This method is thorough in comparing each relevant field,
        /// ensuring a comprehensive equality check.
        /// </remarks>
        public bool Equals(ETAMPModel? x, ETAMPModel? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            List<bool> states = new List<bool>()
            {
                Equals(x.Id, y.Id),
                Equals(x.Version, y.Version),
                string.Equals(x.UpdateType, y.UpdateType,StringComparison.Ordinal),
                string.Equals(x.Token, y.Token,StringComparison.Ordinal),
                string.Equals(x.SignatureToken, y.SignatureToken,StringComparison.Ordinal),
                string.Equals(x.SignatureMessage, y.SignatureMessage,StringComparison.Ordinal)
            };
            return states.TrueForAll(t => t);
        }

        /// <summary>
        /// Returns a hash code for the specified <see cref="ETAMPModel"/> instance.
        /// </summary>
        /// <param name="obj">The <see cref="ETAMPModel"/> instance for which to get a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <remarks>
        /// The hash code is computed by combining the hash codes of the Id, Version, Token, UpdateType,
        /// SignatureToken, and SignatureMessage properties. This ensures that the hash code reflects the
        /// content of the relevant data fields.
        /// </remarks>
        public int GetHashCode([DisallowNull] ETAMPModel obj)
        {
            return HashCode.Combine(obj.Id, obj.Version, obj.Token, obj.UpdateType, obj.SignatureToken, obj.SignatureMessage);
        }
    }
}