using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPManagment
{
    public class ETAMPBuilder : IETAMPBuilder<ETAMPType>
    {
        private ETAMPModel? _model;
        private readonly IServiceProvider _serviceProvider;

        public ETAMPBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public ETAMPModel Build()
        {
            if (_model == null)
            {
                throw new InvalidOperationException("ETAMP model is not created yet.");
            }

            return _model;
        }

        public virtual ETAMPBuilder CreateETAMP<T>(ETAMPType type, string updateType, T payload, double version = 1) where T : BasePayload
        {
            switch (type)
            {
                case ETAMPType.Base:
                    IETAMPBase etampBase = _serviceProvider.GetRequiredService<IETAMPBase>();
                    ArgumentNullException.ThrowIfNull(etampBase, nameof(etampBase));
                    _model = etampBase.CreateETAMPModel(updateType, payload, version);
                    break;

                case ETAMPType.Sign:
                    ETAMPSign sign = _serviceProvider.GetRequiredService<ETAMPSign>();
                    ArgumentNullException.ThrowIfNull(sign, nameof(sign));
                    _model = sign.CreateETAMPModel(updateType, payload, version);
                    break;

                case ETAMPType.Encrypted:
                    IETAMPEncrypted encrypted = _serviceProvider.GetRequiredService<IETAMPEncrypted>();
                    ArgumentNullException.ThrowIfNull(encrypted, nameof(encrypted));
                    _model = encrypted.CreateEncryptETAMPModel(updateType, payload, version);

                    break;

                case ETAMPType.EncryptedSign:
                    ETAMPEncryptedSigned encryptedSigned = _serviceProvider.GetRequiredService<ETAMPEncryptedSigned>();
                    ArgumentNullException.ThrowIfNull(encryptedSigned, nameof(encryptedSigned));
                    _model = encryptedSigned.CreateEncryptETAMPModel(updateType, payload, version);

                    break;

                default:
                    throw new ArgumentException($"Unsupported ETAMP type: {type}");
            }
            return this;
        }
    }
}