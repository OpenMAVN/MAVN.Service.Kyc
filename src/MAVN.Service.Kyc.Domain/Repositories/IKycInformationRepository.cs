using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Kyc.Domain.Models;

namespace MAVN.Service.Kyc.Domain.Repositories
{
    public interface IKycInformationRepository
    {
        Task AddIfNotExistsAsync(IKycInformation model);
        Task UpdateAsync(IKycInformation model);
        Task<IKycInformation> GetByPartnerId(Guid partnerId);
        Task<IReadOnlyList<IKycInformation>> GetByPartnerIds(Guid[] partnerIds);
    }
}
