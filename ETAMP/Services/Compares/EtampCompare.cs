using ETAMP.Models;
using System.Diagnostics.CodeAnalysis;

namespace ETAMP.Services.Compares
{
    public class EtampCompare : IEqualityComparer<EtampModel>
    {
        /// <summary>
        /// Compares two etamp models by their parameters.
        /// </summary>
        /// <param name="x">First etamp to compare</param>
        /// <param name="y">Second etamp to compare</param>
        /// <returns>Boolean if there are same</returns>
        public bool Equals(EtampModel? x, EtampModel? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null && y != null)
            {
                return false;
            }
            if (x != null && y == null)
            {
                return false;
            }
            List<bool> states = new List<bool>()
            {
                Equals(x.Id, y.Id),
                Equals(x.Version, y.Version),
                string.Equals(x.UpdateType, y.UpdateType,StringComparison.Ordinal),
                string.Equals(x.Token, x.Token,StringComparison.Ordinal),
                string.Equals(x.SignatureToken, y.SignatureToken,StringComparison.Ordinal),
                string.Equals(x.SignatureMessage, y.SignatureMessage,StringComparison.Ordinal)
            };
            return states.TrueForAll(t => t);
        }

        public int GetHashCode([DisallowNull] EtampModel obj)
        {
            return HashCode.Combine(obj.Id, obj.Version, obj.Token, obj.UpdateType, obj.SignatureToken, obj.SignatureMessage);
        }
    }
}
