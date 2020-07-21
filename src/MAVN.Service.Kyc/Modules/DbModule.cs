using Autofac;
using AutoMapper;
using AzureStorage.Blob;
using AzureStorage.Tables;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.SettingsReader;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.Kyc.Domain.Repositories;
using MAVN.Service.Kyc.MsSqlRepositories;
using MAVN.Service.Kyc.MsSqlRepositories.Repositories;
using MAVN.Service.Kyc.Settings;

namespace MAVN.Service.Kyc.Modules
{
    [UsedImplicitly]
    public class DbModule : Module
    {

        private readonly string _connectionString;

        public DbModule(IReloadingManager<AppSettings> appSettings)
        {
            _connectionString = appSettings.CurrentValue.KycService.Db.DbConnString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterPostgreSQL(
                _connectionString,
                connString => new KycContext(connString, false),
                dbConn => new KycContext(dbConn));

            builder.RegisterType<KycInformationRepository>()
                .As<IKycInformationRepository>()
                .SingleInstance();

            builder.RegisterType<KycStatusChangeRepository>()
                .As<IKycStatusChangeRepository>()
                .SingleInstance();

        }
    }
}
