using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MAVN.Service.Kyc.Client.Models.Requests;
using MAVN.Service.Kyc.Client.Models.Responses;
using Refit;

namespace MAVN.Service.Kyc.Client
{
    // This is an example of service controller interfaces.
    // Actual interface methods must be placed here (not in IKycClient interface).

    /// <summary>
    /// Kyc client API interface.
    /// </summary>
    [PublicAPI]
    public interface IKycApi
    {
        /// <summary>
        /// Get current kyc info
        /// </summary>
        /// <param name="partnerId"></param>
        [Get("/api/kyc/current")]
        Task<KycInformationResponse> GetCurrentByPartnerIdAsync([Query] Guid partnerId);

        /// <summary>
        /// Get history of kyc info
        /// </summary>
        /// <param name="partnerId"></param>
        [Get("/api/kyc/history")]
        Task<IReadOnlyList<KycStatusChangeResponse>> GetKycStatusChangeHistoryByPartnerIdAsync([Query] Guid partnerId);

        /// <summary>
        /// update  kyc info
        /// </summary>
        /// <param name="request"></param>
        [Put("/api/kyc")]
        Task<KycUpdateResponse> UpdateKycInfoAsync([Body] KycUpdateRequest request);
    }
}
