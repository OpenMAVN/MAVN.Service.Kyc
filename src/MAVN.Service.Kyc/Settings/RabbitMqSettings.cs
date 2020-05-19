using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.Kyc.Settings
{
    public class RabbitMqSettings
    {
        public RabbitMqExchangeSettings Publishers { get; set; }
        public RabbitMqExchangeSettings Subscribers { get; set; }
    }

    public class RabbitMqExchangeSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }
    }
}
