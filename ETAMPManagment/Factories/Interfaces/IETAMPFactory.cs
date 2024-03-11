using ETAMPManagment.ETAMP.Base;

namespace ETAMPManagment.Factories.Interfaces
{
    public interface IETAMPFactory<Type>
    {
        Dictionary<Type, Func<IETAMPData>> Factory { get; }

        void RegisterGenerator(Type type, Func<IETAMPData> generator);

        IETAMPData CreateGenerator(Type type);

        bool UnregisterETAMPGenerator(Type type);
    }
}