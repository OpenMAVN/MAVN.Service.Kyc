using MAVN.Service.Kyc.Client.Models.Enums;

namespace MAVN.Service.Kyc.Client.Models.Responses
{
    /// <summary>
    /// Response model for KYC update
    /// </summary>
    public class KycUpdateResponse
    {
        /// <summary>
        /// Holds error code
        /// </summary>
        public UpdateKycErrorCodes Error { get; set; }
    }
}
