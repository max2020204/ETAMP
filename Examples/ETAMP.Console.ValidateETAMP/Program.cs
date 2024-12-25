using ETAMP.Validation;

var create = new Create();
var etamp = create.CreateETAMP();


var structureValidator = new StructureValidator();
var result = structureValidator.ValidateETAMP(etamp, true);
Console.WriteLine(result.IsValid);