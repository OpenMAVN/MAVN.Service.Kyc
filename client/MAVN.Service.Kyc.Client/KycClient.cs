using Lykke.HttpClientGenerator;

namespace MAVN.Service.Kyc.Client
{
    /// <summary>
    /// Kyc API aggregating interface.
    /// </summary>
    public class KycClient : IKycClient
    {
        // Note: Add similar KycApi properties for each new service controller

        /// <summary>Interface to Kyc KycApi.</summary>
        public IKycApi KycApi { get; private set; }

        /// <summary>C-tor</summary>
        public KycClient(IHttpClientGenerator httpClientGenerator)
        {
            KycApi = httpClientGenerator.Generate<IKycApi>();
        }
    }
}
