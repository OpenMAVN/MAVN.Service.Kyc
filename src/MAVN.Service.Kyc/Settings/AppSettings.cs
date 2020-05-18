using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace MAVN.Service.Kyc.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public KycSettings KycService { get; set; }
    }
}
