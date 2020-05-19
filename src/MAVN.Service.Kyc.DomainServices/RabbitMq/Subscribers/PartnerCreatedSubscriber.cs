using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using MAVN.Service.Kyc.Domain.Models;
using MAVN.Service.Kyc.Domain.Services;
using MAVN.Service.PartnerManagement.Contract;

namespace MAVN.Service.Kyc.DomainServices.RabbitMq.Subscribers
{
    public class PartnerCreatedSubscriber : JsonRabbitSubscriber<PartnerCreatedEvent>
    {
        private readonly IKycService _kycService;
        private readonly ILog _log;

        public PartnerCreatedSubscriber(
            IKycService kycService,
            string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _kycService = kycService;
            _log = logFactory.CreateLog(this);
        }

        protected override async Task ProcessMessageAsync(PartnerCreatedEvent message)
        {
            await _kycService.AddKycInfoAsync(new KycInformation
            {
                PartnerId = message.PartnerId, AdminUserId = message.CreatedBy,
            });

            _log.Info("Processed PartnerCreatedEvent", context:message);
        }
    }
}
