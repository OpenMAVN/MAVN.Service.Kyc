using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Kyc.Domain.Enums;
using MAVN.Service.Kyc.Domain.Models;

namespace MAVN.Service.Kyc.Domain.Services
{
    public interface IKycService
    {
        Task AddKycInfoAsync(KycInformation model);
        Task<UpdateKycStatusErrorCode> UpdateKycInfoAsync(KycInformation model);
        Task<IReadOnlyList<IKycInformationStatusChange>> GetKycStatusChangesAsync(Guid partnerId);
        Task<IKycInformation> GetCurrentKycStatusAsync(Guid partnerId);
    }
}
