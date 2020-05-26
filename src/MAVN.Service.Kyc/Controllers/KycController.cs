using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Service.Kyc.Client;
using MAVN.Service.Kyc.Client.Models.Enums;
using MAVN.Service.Kyc.Client.Models.Requests;
using MAVN.Service.Kyc.Client.Models.Responses;
using MAVN.Service.Kyc.Domain.Models;
using MAVN.Service.Kyc.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.Kyc.Controllers
{
    [ApiController]
    [Route("api/kyc")]
    public class KycController : ControllerBase, IKycApi
    {
        private readonly IKycService _kycService;
        private readonly IMapper _mapper;

        public KycController(IKycService kycService, IMapper mapper)
        {
            _kycService = kycService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get current kyc info
        /// </summary>
        /// <param name="partnerId"></param>
        [HttpGet("current")]
        [ProducesResponseType(typeof(KycInformationResponse), (int)HttpStatusCode.OK)]
        public async Task<KycInformationResponse> GetCurrentByPartnerIdAsync([FromQuery]Guid partnerId)
        {
            var result = await _kycService.GetCurrentKycStatusAsync(partnerId);

            return _mapper.Map<KycInformationResponse>(result);
        }

        /// <summary>
        /// Get current kyc info for list of partners
        /// </summary>
        /// <param name="partnerIds"></param>
        [HttpPost("list")]
        [ProducesResponseType(typeof(KycInformationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<KycInformationResponse>> GetCurrentByPartnerIdsAsync([FromBody][Required] Guid[] partnerIds)
        {
            var result = await _kycService.GetCurrentKycStatusByPartnerIdsAsync(partnerIds);

            return _mapper.Map<IReadOnlyList<KycInformationResponse>>(result);
        }

        /// <summary>
        /// Get history of kyc info
        /// </summary>
        /// <param name="partnerId"></param>
        [HttpGet("history")]
        [ProducesResponseType(typeof(KycStatusChangeResponse), (int)HttpStatusCode.OK)]
        public async Task<IReadOnlyList<KycStatusChangeResponse>> GetKycStatusChangeHistoryByPartnerIdAsync([FromQuery]Guid partnerId)
        {
            var result = await _kycService.GetKycStatusChangesAsync(partnerId);

            return _mapper.Map<IReadOnlyList<KycStatusChangeResponse>>(result);
        }

        /// <summary>
        /// update  kyc info
        /// </summary>
        /// <param name="request"></param>
        [HttpPut]
        [ProducesResponseType(typeof(KycUpdateResponse), (int)HttpStatusCode.OK)]
        public async Task<KycUpdateResponse> UpdateKycInfoAsync([FromBody]KycUpdateRequest request)
        {
            var model = _mapper.Map<KycInformation>(request);
            var result = await _kycService.UpdateKycInfoAsync(model);

            return new KycUpdateResponse { Error = (UpdateKycErrorCodes)result };
        }
    }
}
