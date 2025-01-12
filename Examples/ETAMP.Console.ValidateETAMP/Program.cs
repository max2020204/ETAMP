#region

using System.Security.Cryptography;
using ETAMP.Encryption.ECDsaManager;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Validation;
using ETAMP.Wrapper;
using ETAMP.Wrapper.Base;

#endregion

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
Console.WriteLine(result.IsValid);