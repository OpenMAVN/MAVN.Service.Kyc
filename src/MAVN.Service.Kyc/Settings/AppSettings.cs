using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using MAVN.Service.AdminManagement.Client;
using MAVN.Service.PartnerManagement.Client;

namespace MAVN.Service.Kyc.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public KycSettings KycService { get; set; }

        public AdminManagementServiceClientSettings AdminManagementService { get; set; }

        public PartnerManagementServiceClientSettings PartnerManagementService { get; set; }
    }
}
