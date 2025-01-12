#region

using System.Security.Cryptography;
using ETAMP.Console.CreateETAMPService.Models;
using ETAMP.Core.Models;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Extension.Builder;
using ETAMP.Wrapper.Base;
using Microsoft.Extensions.DependencyInjection;

#endregion

internal class Program
{
    private static ServiceProvider _provider;

    private static void Main(string[] args)
    {
        _provider = Service.ConfigureServices();
        Console.WriteLine(SignETAMP(_provider).ToJson());
    }

    public static ETAMPModel<TokenModel> SignETAMP(ServiceProvider provider)
    {
        var sign = provider.GetService<SignWrapperBase>();
        var creator = provider.GetService<IECDsaCreator>();
        sign.Initialize(creator.CreateECDsa(), HashAlgorithmName.SHA512);
        var etamp = Service.CreateETAMP(_provider);
        etamp.Sign(sign);
        return etamp;
    }
}