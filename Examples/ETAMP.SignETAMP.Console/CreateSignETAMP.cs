#region

using System.Security.Cryptography;
using ETAMP.Console.CreateETAMP.Models;
using ETAMP.Core.Models;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Extension.Builder;
using ETAMP.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class CreateSignETAMP
{
    private static ServiceProvider _provider;

    private static ECDsa _ecdsaInstance;

    public static string PublicKey { get; private set; }

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
        var (sign, pemCleaner) = GetServices(provider);

        InitializeSigning(sign, pemCleaner);

        var etampModel = CreateETAMP.InitializeEtampModel(provider);
        etampModel.Sign(sign);
        return etampModel;
    }

    private static (ISignWrapper?, IPemKeyCleaner?) GetServices(IServiceProvider provider)
    {
        return (
            provider.GetService<ISignWrapper>(),
            provider.GetService<IPemKeyCleaner>()
        );
    }

    private static void InitializeSigning(ISignWrapper sign, IPemKeyCleaner pemCleaner)
    {
        var publicKeyPem = pemCleaner.ClearPemPublicKey(_ecdsaInstance.ExportSubjectPublicKeyInfoPem());
        PublicKey = publicKeyPem.KeyModelProvider.PublicKey;
        sign.Initialize(_ecdsaInstance, HashAlgorithmName.SHA512);
    }
}