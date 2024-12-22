using ETAMP.Console.CreateETAMP;
using ETAMP.Console.CreateETAMP.Models;
using ETAMP.Core.Models;
using ETAMP.Validation;

Create create = new Create();
ETAMPModel<TokenModel>? etamp = create.CreateETAMP();


StructureValidator structureValidator = new StructureValidator();
var result = structureValidator.ValidateETAMP(etamp, true);
Console.WriteLine(result.IsValid);