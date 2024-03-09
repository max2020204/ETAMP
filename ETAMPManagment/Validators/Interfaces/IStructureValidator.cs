using ETAMPManagment.Models;

namespace ETAMPManagment.Validators.Interfaces
{
    public interface IStructureValidator
    {
        (bool isValid, ETAMPModel model) IsValidEtampFormat(string etamp);

        bool ValidateIdConsistency(string etamp);

        bool ValidateETAMPStructure(string etamp);

        bool ValidateETAMPStructure(ETAMPModel model);

        bool ValidateETAMPStructureLite(string etamp);

        bool ValidateETAMPStructureLite(ETAMPModel model);
    }
}