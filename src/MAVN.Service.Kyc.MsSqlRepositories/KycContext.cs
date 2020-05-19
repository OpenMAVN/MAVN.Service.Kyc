using System;
using System.Data.Common;
using JetBrains.Annotations;
using MAVN.Common.MsSql;
using MAVN.Service.Kyc.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Kyc.MsSqlRepositories
{
    public class KycContext : MsSqlContext
    {
        private const string Schema = "kyc";

        public DbSet<KycInformationEntity> KycInformation { get; set; }
        public DbSet<KycInformationStatusChangeEntity> KycInformationStatusChange { get; set; }

        // empty constructor needed for EF migrations
        [UsedImplicitly]
        public KycContext()
            : base(Schema)
        {
        }

        public KycContext(string connectionString, bool isTraceEnabled)
            : base(Schema, connectionString, isTraceEnabled)
        {
        }

        //Needed constructor for using InMemoryDatabase for tests
        public KycContext(DbContextOptions options)
            : base(Schema, options)
        {
        }

        public KycContext(DbConnection dbConnection)
            : base(Schema, dbConnection)
        {
        }

        protected override void OnLykkeModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
