using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.Kyc.Domain.Models;
using MAVN.Service.Kyc.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Kyc.MsSqlRepositories.Repositories
{
    public class KycStatusChangeRepository : IKycStatusChangeRepository
    {
        private readonly IDbContextFactory<KycContext> _contextFactory;

        public KycStatusChangeRepository(IDbContextFactory<KycContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IReadOnlyList<IKycInformationStatusChange>> GetByPartnerIdAsync(Guid partnerId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var result = await context.KycInformationStatusChange
                    .Where(x => x.PartnerId == partnerId)
                    .ToListAsync();

                return result;
            }
        }
    }
}
