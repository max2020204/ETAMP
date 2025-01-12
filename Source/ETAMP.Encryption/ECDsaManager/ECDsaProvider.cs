#region

using System.Security.Cryptography;
using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

public class ECDsaProvider : ECDsaProviderBase
{
    public ECDsaProvider(IECDsaStore store) : base(store)
    {
    }

    /// <inheritdoc />
    public override ECDsa? GetECDsa(Guid id)
    {
        var provider = Store.Get(id);
        return provider?.CurrentEcdsa;
    }

    /// <inheritdoc />
    public override ECDsa? GetECDsa(string name)
    {
        var provider = Store.Get(name);
        return provider?.CurrentEcdsa;
    }

   
}