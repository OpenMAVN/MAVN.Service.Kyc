using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.Kyc.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class KycSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSettings Rabbit { get; set; }
    }
}
