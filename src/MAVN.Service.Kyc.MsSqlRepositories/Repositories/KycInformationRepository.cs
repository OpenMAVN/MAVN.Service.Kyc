using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVN.Common.MsSql;
using MAVN.Service.Kyc.Domain.Models;
using MAVN.Service.Kyc.Domain.Repositories;
using MAVN.Service.Kyc.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Kyc.MsSqlRepositories.Repositories
{
    public class KycInformationRepository : IKycInformationRepository
    {
        private readonly IDbContextFactory<KycContext> _contextFactory;

        public KycInformationRepository(IDbContextFactory<KycContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddIfNotExistsAsync(IKycInformation model)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                if (await context.KycInformation.FindAsync(model.PartnerId) != null)
                    return;

                var kycInformationEntity = KycInformationEntity.Create(model);
                var statusChangeEntity = KycInformationStatusChangeEntity.Create(model.PartnerId, model.AdminUserId,
                    model.Timestamp, model.Comment, null, model.KycStatus);

                context.KycInformation.Add(kycInformationEntity);
                context.KycInformationStatusChange.Add(statusChangeEntity);

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(IKycInformation model)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.KycInformation.FindAsync(model.PartnerId);
                var statusChangeEntity = KycInformationStatusChangeEntity.Create(model.PartnerId, model.AdminUserId,
                    model.Timestamp, model.Comment, entity.KycStatus, model.KycStatus);

                entity.KycStatus = model.KycStatus;
                entity.Comment = model.Comment;
                entity.Timestamp = model.Timestamp;
                entity.AdminUserId = model.AdminUserId;

                context.KycInformation.Update(entity);
                context.KycInformationStatusChange.Add(statusChangeEntity);

                await context.SaveChangesAsync();
            }
        }

        public async Task<IKycInformation> GetByPartnerId(Guid partnerId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.KycInformation.FindAsync(partnerId);

                return entity;
            }
        }

        public async Task<IReadOnlyList<IKycInformation>> GetByPartnerIds(Guid[] partnerIds)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var result = await context.KycInformation
                    .Where(k => partnerIds.Contains(k.PartnerId))
                    .ToListAsync();

                return result;
            }
        }
    }
}
