using AutoMapper;
using MAVN.Service.Kyc.Client;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.Kyc.Controllers
{
    [Route("api/Kyc")] // TODO fix route
    public class KycController : Controller, IKycApi
    {
        private readonly IMapper _mapper;

        public KycController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
