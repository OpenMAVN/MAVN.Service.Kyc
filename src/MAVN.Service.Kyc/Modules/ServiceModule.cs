using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Sdk.Health;
using Lykke.SettingsReader;
using MAVN.Service.AdminManagement.Client;
using MAVN.Service.Kyc.Domain.Services;
using MAVN.Service.Kyc.DomainServices;
using MAVN.Service.Kyc.Services;
using MAVN.Service.Kyc.Settings;
using MAVN.Service.PartnerManagement.Client;

namespace MAVN.Service.Kyc.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate()
                .SingleInstance();

            builder.RegisterType<KycService>()
                .As<IKycService>()
                .SingleInstance();

            builder.RegisterType<NotificationsService>()
                .As<INotificationsService>()
                .WithParameter("backOfficeUrl", _appSettings.CurrentValue.KycService.BackOfficeLink)
                .WithParameter("kycApprovedEmailTemplateId", _appSettings.CurrentValue.KycService.KycApprovedEmail.EmailTemplateId)
                .WithParameter("kycApprovedEmailSubjectTemplateId", _appSettings.CurrentValue.KycService.KycApprovedEmail.SubjectTemplateId)
                .WithParameter("kycRejectedEmailTemplateId", _appSettings.CurrentValue.KycService.KycRejectedEmail.EmailTemplateId)
                .WithParameter("kycRejectedEmailSubjectTemplateId", _appSettings.CurrentValue.KycService.KycRejectedEmail.SubjectTemplateId)
                .WithParameter("voucherManagerUrl", _appSettings.CurrentValue.KycService.KycApprovedEmail.VoucherManagerUrl)
                .SingleInstance();

            builder.RegisterAdminManagementClient(_appSettings.CurrentValue.AdminManagementService);
            builder.RegisterPartnerManagementClient(_appSettings.CurrentValue.PartnerManagementService);
        }
    }
}
