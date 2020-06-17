using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;
using MAVN.Service.Kyc.Settings.EmailSettings;

namespace MAVN.Service.Kyc.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class KycSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSettings Rabbit { get; set; }

        public KycApprovedEmail KycApprovedEmail { get; set; }

        public KycRejectedEmail KycRejectedEmail { get; set; }

        public string BackOfficeLink { get; set; }
    }
}
