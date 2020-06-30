using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using MAVN.Service.AdminManagement.Client;
using MAVN.Service.AdminManagement.Client.Models.Enums;
using MAVN.Service.AdminManagement.Client.Models.Requests;
using MAVN.Service.Kyc.Domain.Enums;
using MAVN.Service.Kyc.Domain.Models;
using MAVN.Service.Kyc.Domain.Repositories;
using MAVN.Service.Kyc.Domain.Services;
using MAVN.Service.PartnerManagement.Client;

namespace MAVN.Service.Kyc.DomainServices
{
    public class KycService : IKycService
    {
        private readonly IKycInformationRepository _kycInformationRepository;
        private readonly IKycStatusChangeRepository _kycStatusChangeRepository;
        private readonly INotificationsService _notificationsService;
        private IPartnerManagementClient _partnerManagementClient;
        private readonly IAdminManagementServiceClient _adminManagementClient;
        private readonly ILog _log;

        public KycService(
            IKycInformationRepository kycInformationRepository,
            IKycStatusChangeRepository kycStatusChangeRepository,
            INotificationsService notificationsService,
            IPartnerManagementClient partnerManagementClient,
            IAdminManagementServiceClient adminManagementClient,
            ILogFactory logFactory)
        {
            _kycInformationRepository = kycInformationRepository;
            _kycStatusChangeRepository = kycStatusChangeRepository;
            _notificationsService = notificationsService;
            _partnerManagementClient = partnerManagementClient;
            _adminManagementClient = adminManagementClient;
            _log = logFactory.CreateLog(this);
        }

        public async Task AddKycInfoAsync(KycInformation model)
        {
            if (model.PartnerId == Guid.Empty || model.AdminUserId == Guid.Empty)
            {
                _log.Warning("Request to add KycInfo with empty partner or admin id", context: model);
                return;
            }

            model.Timestamp = DateTime.UtcNow;
            model.KycStatus = KycStatus.Pending;

            await _kycInformationRepository.AddIfNotExistsAsync(model);
        }

        public async Task<UpdateKycStatusErrorCode> UpdateKycInfoAsync(KycInformation model)
        {
            if (model.KycStatus == KycStatus.Rejected && string.IsNullOrEmpty(model.Comment))
                return UpdateKycStatusErrorCode.CommentRequired;

            var current = await _kycInformationRepository.GetByPartnerId(model.PartnerId);

            if (current == null)
                return UpdateKycStatusErrorCode.KycDoesNotExist;

            model.Timestamp = DateTime.UtcNow;

            switch (current.KycStatus)
            {
                case KycStatus.Pending when model.KycStatus != KycStatus.InReview:
                    return UpdateKycStatusErrorCode.InvalidStatus;
                case KycStatus.InReview when model.KycStatus != KycStatus.Accepted && model.KycStatus != KycStatus.Rejected && model.KycStatus != KycStatus.RequiresData:
                    return UpdateKycStatusErrorCode.InvalidStatus;
                case KycStatus.Rejected when model.KycStatus != KycStatus.InReview:
                    return UpdateKycStatusErrorCode.InvalidStatus;
                case KycStatus.Rejected when model.KycStatus == KycStatus.InReview:
                    _log.Warning("Returning KYC from Rejected to InReview", context: model);
                    break;
                case KycStatus.Accepted when model.KycStatus != KycStatus.InReview:
                    return UpdateKycStatusErrorCode.InvalidStatus;
                case KycStatus.Accepted when model.KycStatus == KycStatus.InReview:
                    _log.Warning("Returning KYC from Accepted to InReview", context: model);
                    break;
                case KycStatus.RequiresData when model.KycStatus != KycStatus.InReview:
                    return UpdateKycStatusErrorCode.InvalidStatus;
            }

            await _kycInformationRepository.UpdateAsync(model);

            if (model.KycStatus != KycStatus.Accepted && model.KycStatus != KycStatus.Rejected)
                return UpdateKycStatusErrorCode.None;

            var partner = await _partnerManagementClient.Partners.GetByIdAsync(model.PartnerId);

            if (partner == null)
            {
                _log.Warning("Missing partner when trying to send KYC notification", context: model.PartnerId);
                return UpdateKycStatusErrorCode.None;
            }

            var admin = await _adminManagementClient.AdminsApi.GetByIdAsync(
                new GetAdminByIdRequestModel { AdminUserId = partner.CreatedBy.ToString() });

            if (admin.Error != AdminUserResponseErrorCodes.None || admin.Profile == null)
            {
                _log.Warning("Missing admin when trying to send KYC notification", context: new { PartnerAdminId = partner.CreatedBy });
                return UpdateKycStatusErrorCode.None;
            }

            if (model.KycStatus == KycStatus.Accepted)
                await _notificationsService.NotifyKycApprovedAsync(admin.Profile.AdminUserId,
                    admin.Profile.Email,
                    admin.Profile.FirstName, partner.Name);
            else
                await _notificationsService.NotifyKycRejectedAsync(admin.Profile.AdminUserId,
                    admin.Profile.Email, admin.Profile.FirstName, partner.Name, model.Comment);

            return UpdateKycStatusErrorCode.None;
        }

        public Task<IReadOnlyList<IKycInformationStatusChange>> GetKycStatusChangesAsync(Guid partnerId)
            => _kycStatusChangeRepository.GetByPartnerIdAsync(partnerId);

        public Task<IKycInformation> GetCurrentKycStatusAsync(Guid partnerId)
           => _kycInformationRepository.GetByPartnerId(partnerId);

        public Task<IReadOnlyList<IKycInformation>> GetCurrentKycStatusByPartnerIdsAsync(Guid[] partnerIds)
            => _kycInformationRepository.GetByPartnerIds(partnerIds);
    }
}
