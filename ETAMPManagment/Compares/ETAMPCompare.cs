using ETAMPManagment.Models;
using System.Diagnostics.CodeAnalysis;

namespace ETAMPManagment.Compares
{
    /// <summary>
    /// Provides methods to compare two instances of <see cref="ETAMPModel"/> for equality.
    /// Ensures that two instances are considered equal if all their significant data fields match,
    /// specifically Id, Version, UpdateType, Token, SignatureToken, and SignatureMessage.
    /// </summary>
    public class ETAMPCompare : IEqualityComparer<ETAMPModel>
    {
        /// <summary>
        /// Compares two instances of <see cref="ETAMPModel"/> for equality based on their properties.
        /// </summary>
        /// <param name="x">The first <see cref="ETAMPModel"/> instance to compare.</param>
        /// <param name="y">The second <see cref="ETAMPModel"/> instance to compare.</param>
        /// <returns>Returns <c>true</c> if both instances have the same values for their significant fields; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method compares each relevant field, including Id, Version, UpdateType, Token, SignatureToken, and SignatureMessage,
        /// to determine equality. It handles null values, considering two null objects as equal but a null and a non-null object as unequal.
        /// </remarks>
        public bool Equals(ETAMPModel? x, ETAMPModel? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return new List<bool>
            {
                Equals(x.Id, y.Id),
                Equals(x.Version, y.Version),
                string.Equals(x.UpdateType, y.UpdateType, StringComparison.Ordinal),
                string.Equals(x.Token, y.Token, StringComparison.Ordinal),
                string.Equals(x.SignatureToken, y.SignatureToken, StringComparison.Ordinal),
                string.Equals(x.SignatureMessage, y.SignatureMessage, StringComparison.Ordinal)
            }.TrueForAll(equals => equals);
        }

        /// <summary>
        /// Returns a hash code for the specified <see cref="ETAMPModel"/> instance.
        /// </summary>
        /// <param name="obj">The <see cref="ETAMPModel"/> instance for which to get a hash code.</param>
        /// <returns>A hash code for the specified object, reflecting its significant fields.</returns>
        /// <remarks>
        /// Computes the hash code by combining the hash codes of the Id, Version, UpdateType, Token, SignatureToken, and SignatureMessage properties.
        /// This method safely handles null values in these properties.
        /// </remarks>
        public int GetHashCode([DisallowNull] ETAMPModel obj)
        {
            return HashCode.Combine(obj.Id, obj.Version, obj.Token, obj.UpdateType, obj.SignatureToken, obj.SignatureMessage);
        }
    }
}