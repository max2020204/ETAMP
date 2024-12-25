using ETAMP.Console.CreateETAMP;
using ETAMP.Console.CreateETAMP.Models;
using ETAMP.Core.Models;
using ETAMP.Validation;

var create = new Create();
var etamp = create.CreateETAMP();


var structureValidator = new StructureValidator();
var result = structureValidator.ValidateETAMP(etamp, true);
Console.WriteLine(result.IsValid);