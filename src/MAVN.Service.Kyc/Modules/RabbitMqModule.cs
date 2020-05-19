using Autofac;
using JetBrains.Annotations;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.SettingsReader;
using MAVN.Service.Kyc.DomainServices.RabbitMq.Subscribers;
using MAVN.Service.Kyc.Settings;
using MAVN.Service.PartnerManagement.Contract;

namespace MAVN.Service.Kyc.Modules
{
    [UsedImplicitly]
    public class RabbitMqModule : Module
    {
        private const string DefaultQueueName = "kyc";
        private const string PartnerCreatedExchangeName = "lykke.customer.partnercreated";

        private readonly RabbitMqSettings _settings;

        public RabbitMqModule(IReloadingManager<AppSettings> settingsManager)
        {
            _settings = settingsManager.CurrentValue.KycService.Rabbit;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterRabbitMqPublishers(builder);
            RegisterRabbitMqSubscribers(builder);
        }

        private void RegisterRabbitMqPublishers(ContainerBuilder builder)
        {
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitSubscriber<PartnerCreatedSubscriber, PartnerCreatedEvent>(
                _settings.Subscribers.ConnectionString,
                PartnerCreatedExchangeName, DefaultQueueName);
        }
    }
}
