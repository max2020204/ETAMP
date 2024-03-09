using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Factories.Interfaces;
using ETAMPManagment.Utils;

namespace ETAMPManagment.Factories
{
    public class ETAMPFactory : IETAMPFactory<ETAMPType>
    {
        public Dictionary<ETAMPType, Func<IETAMPBase>> Factory { get; }

        public ETAMPFactory()
        {
            Factory = new Dictionary<ETAMPType, Func<IETAMPBase>>();
        }

        public virtual IETAMPBase CreateGenerator(ETAMPType type)
        {
            if (Factory.TryGetValue(type, out var func))
            {
                return func();
            }
            throw new ArgumentException($"Unsupported ETAMP generator type: {type}", nameof(type));
        }

        public virtual void RegisterGenerator(ETAMPType type, Func<IETAMPBase> generator)
        {
            Factory.Add(type, generator);
        }

        public virtual bool UnregisterETAMPGenerator(ETAMPType type)
        {
            return Factory.Remove(type);
        }
    }
}