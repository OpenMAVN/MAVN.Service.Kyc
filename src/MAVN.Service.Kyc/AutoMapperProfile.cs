using AutoMapper;
using MAVN.Service.Kyc.Client.Models.Requests;
using MAVN.Service.Kyc.Client.Models.Responses;
using MAVN.Service.Kyc.Domain.Models;

namespace MAVN.Service.Kyc
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<KycUpdateRequest, KycInformation>()
                .ForMember(x => x.Timestamp, opt => opt.Ignore());

            CreateMap<IKycInformationStatusChange, KycStatusChangeResponse>();
            CreateMap<IKycInformation, KycInformationResponse>();

        }
    }
}
