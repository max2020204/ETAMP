#region

using System.Security.Cryptography;
using ETAMP.Console.CreateETAMP;
using ETAMP.Core.Models;
using ETAMP.Encryption.Base;
using ETAMP.Encryption.ECDsaManager;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Extension.Builder;
using ETAMP.Wrapper;
using ETAMP.Wrapper.Base;

#endregion

public static class ETAMPSignProgram
{
    private static ECDsaProviderBase? _providerBase;
    private static SignWrapperBase _signWrapperBase;
    private static IECDsaStore _ecdsaStore;


    private static void Main(string[] args)
    {
        Initialize(ref _signWrapperBase);

        //Sign
        var etamp = SignETAMP(_signWrapperBase);
        Console.WriteLine(etamp.ToJson());
    }

    private static void Initialize(ref SignWrapperBase sign)
    {
        sign = new SignWrapper();
        _ecdsaStore = new ECDsaStore();
        var ecDsaProvider = new ECDsaProvider(_ecdsaStore);
        _providerBase = ecDsaProvider;

        ECDsaRegistrationBase registrationBase = new ECDsaRegistration(_ecdsaStore, _providerBase);
        registrationBase.Registrar(ECDsa.Create());
        sign.Initialize(_providerBase, HashAlgorithmName.SHA512);
    }

    public static ETAMPModel<TokenModel> SignETAMP(SignWrapperBase signWrapperBase)
    {
        var create = new Create();
        return create.CreateETAMP().Sign(signWrapperBase);
    }
}