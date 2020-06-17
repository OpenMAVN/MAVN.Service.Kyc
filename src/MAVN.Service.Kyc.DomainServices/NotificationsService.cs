using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Common;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.Kyc.Domain.Services;
using MAVN.Service.NotificationSystem.SubscriberContract;

namespace MAVN.Service.Kyc.DomainServices
{
    public class NotificationsService : INotificationsService
    {
        private readonly IRabbitPublisher<EmailMessageEvent> _emailsPublisher;
        private readonly string _backOfficeUrl;
        private readonly string _kycApprovedEmailTemplateId;
        private readonly string _kycApprovedEmailSubjectTemplateId;
        private readonly string _kycRejectedEmailTemplateId;
        private readonly string _kycRejectedEmailSubjectTemplateId;
        private readonly string _voucherManagerUrl;

        public NotificationsService(
            IRabbitPublisher<EmailMessageEvent> emailsPublisher,
            string backOfficeUrl,
            string kycApprovedEmailTemplateId,
            string kycApprovedEmailSubjectTemplateId,
            string kycRejectedEmailTemplateId,
            string kycRejectedEmailSubjectTemplateId,
            string voucherManagerUrl)
        {
            _emailsPublisher = emailsPublisher;
            _backOfficeUrl = backOfficeUrl;
            _kycApprovedEmailTemplateId = kycApprovedEmailTemplateId;
            _kycApprovedEmailSubjectTemplateId = kycApprovedEmailSubjectTemplateId;
            _kycRejectedEmailTemplateId = kycRejectedEmailTemplateId;
            _kycRejectedEmailSubjectTemplateId = kycRejectedEmailSubjectTemplateId;
            _voucherManagerUrl = voucherManagerUrl;
        }

        public async Task NotifyKycApprovedAsync(string adminUserId, string adminUserEmail, string adminUserName, string partnerName)
        {
            var values = new Dictionary<string, string>
            {
                {"BusinessName", partnerName},
                {"VoucherManagerUrl", _backOfficeUrl + _voucherManagerUrl},
                {"AdminUserName", $" {adminUserName}"},
            };

            await SendEmailAsync(adminUserId, adminUserEmail, values, _kycApprovedEmailTemplateId,
                _kycApprovedEmailSubjectTemplateId);
        }

        public async Task NotifyKycRejectedAsync(string adminUserId, string adminUserEmail, string adminUserName, string partnerName, string rejectionComment)
        {
            var values = new Dictionary<string, string>
            {
                {"BusinessName", partnerName},
                {"RejectionComment", rejectionComment},
                {"AdminUserName", $" {adminUserName}"},
            };

            await SendEmailAsync(adminUserId, adminUserEmail, values, _kycRejectedEmailTemplateId,
                _kycRejectedEmailSubjectTemplateId);
        }

        private async Task SendEmailAsync(string customerId, string destination, Dictionary<string, string> values, string emailTemplateId, string subjectTemplateId)
        {
            if (!string.IsNullOrWhiteSpace(destination))
                values["target_email"] = destination;

            await _emailsPublisher.PublishAsync(new EmailMessageEvent
            {
                CustomerId = customerId,
                MessageTemplateId = emailTemplateId,
                SubjectTemplateId = subjectTemplateId,
                TemplateParameters = values,
                Source = $"{AppEnvironment.Name} - {AppEnvironment.Version}"
            });
        }
    }
}
