namespace ETAMP.Encryption.Interfaces.ECDSAManager;

public interface IECDsaControl
{
    bool Remove(Guid id);
    bool Remove(string name);
}