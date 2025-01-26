#region

using System.Security.Cryptography;
using ETAMP.Console.CreateETAMP.Models;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class CreateSignETAMP
{
    private static ServiceProvider _provider;

    private static ECDsa _ecdsaInstance;

    public static ECDKeyModelProvider KeyModelProvider { get; private set; }

    public static ETAMPModel<TokenModel> ETAMP { get; private set; }

    public static void Main()
    {
        _ecdsaInstance = ECDsa.Create();
        _provider = CreateETAMP.ConfigureServices();
        ETAMP = SignETAMP(_provider);
        Console.WriteLine(ETAMP.ToJson());
    }

    public static ETAMPModel<TokenModel> SignETAMP(IServiceProvider provider)
    {
        var sign = provider.GetService<ISignWrapper>();

        InitializeSigning(sign);

        var etampModel = CreateETAMP.InitializeEtampModel(provider);
        etampModel.Sign(sign);
        return etampModel;
    }

    private static void InitializeSigning(ISignWrapper sign)
    {
        KeyModelProvider = new ECDKeyModelProvider
        {
            PublicKey = ECDKeyModelProvider.ClearPemFormatting(_ecdsaInstance.ExportSubjectPublicKeyInfoPem())
        };
        sign.Initialize(_ecdsaInstance, HashAlgorithmName.SHA512);
    }
}