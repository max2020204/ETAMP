using ETAMPManagment.ETAMP.Base.Interfaces;

namespace ETAMPManagment.Factories.Interfaces
{
    public interface IETAMPFactory<Type>
    {
        Dictionary<Type, Func<IETAMPBase>> Factory { get; }

        void RegisterGenerator(Type type, Func<IETAMPBase> generator);

        IETAMPBase CreateGenerator(Type type);

        bool UnregisterETAMPGenerator(Type type);
    }
}