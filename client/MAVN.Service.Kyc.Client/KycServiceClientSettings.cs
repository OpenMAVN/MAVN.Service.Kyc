using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.Kyc.Client 
{
    /// <summary>
    /// Kyc client settings.
    /// </summary>
    public class KycServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
