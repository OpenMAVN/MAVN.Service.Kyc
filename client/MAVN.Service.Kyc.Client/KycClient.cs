using Lykke.HttpClientGenerator;

namespace MAVN.Service.Kyc.Client
{
    /// <summary>
    /// Kyc API aggregating interface.
    /// </summary>
    public class KycClient : IKycClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to Kyc Api.</summary>
        public IKycApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public KycClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IKycApi>();
        }
    }
}
