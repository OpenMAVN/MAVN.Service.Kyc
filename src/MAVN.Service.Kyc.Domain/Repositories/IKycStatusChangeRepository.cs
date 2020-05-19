using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Kyc.Domain.Models;

namespace MAVN.Service.Kyc.Domain.Repositories
{
    public interface IKycStatusChangeRepository
    {
        Task<IReadOnlyList<IKycInformationStatusChange>> GetByPartnerIdAsync(Guid partnerId);
    }
}
