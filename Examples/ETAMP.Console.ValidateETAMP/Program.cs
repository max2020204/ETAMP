#region

#endregion

internal class ValidateETAMP
{
    private static void Main(string[] args)
    {
    }
}
/*
var ecdsa = ECDsa.Create();
IECDsaRegistrar registrar = new ECDsaProvider();
IECDsaProvider provider = new ECDsaProvider();
registrar.RegisterECDsa(ecdsa);
provider.GetECDsa()
SignWrapperBase signWrapperBase = new SignWrapper();
signWrapperBase.Initialize(, HashAlgorithmName.SHA512);
var etamp = ETAMPSignProgram.SignETAMP();

var structureValidator = new StructureValidator();
var result = structureValidator.ValidateETAMP(etamp);
Console.WriteLine(result.IsValid);*/