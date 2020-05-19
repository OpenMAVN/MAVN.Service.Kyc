using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using MAVN.Service.Kyc.Domain.Enums;
using MAVN.Service.Kyc.Domain.Models;
using MAVN.Service.Kyc.Domain.Repositories;
using MAVN.Service.Kyc.Domain.Services;

namespace MAVN.Service.Kyc.DomainServices
{
    public class KycService : IKycService
    {
        private readonly IKycInformationRepository _kycInformationRepository;
        private readonly IKycStatusChangeRepository _kycStatusChangeRepository;
        private readonly ILog _log;

        public KycService(IKycInformationRepository kycInformationRepository, IKycStatusChangeRepository kycStatusChangeRepository, ILogFactory logFactory)
        {
            _kycInformationRepository = kycInformationRepository;
            _kycStatusChangeRepository = kycStatusChangeRepository;
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

            var current = await _kycInformationRepository.GeyByPartnerId(model.PartnerId);

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

            return UpdateKycStatusErrorCode.None;
        }

        public Task<IReadOnlyList<IKycInformationStatusChange>> GetKycStatusChangesAsync(Guid partnerId)
            => _kycStatusChangeRepository.GetByPartnerIdAsync(partnerId);

        public Task<IKycInformation> GetCurrentKycStatusAsync(Guid partnerId)
           => _kycInformationRepository.GeyByPartnerId(partnerId);
    }
}
