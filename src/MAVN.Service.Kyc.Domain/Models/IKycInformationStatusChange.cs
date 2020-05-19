using System;
using MAVN.Service.Kyc.Domain.Enums;

namespace MAVN.Service.Kyc.Domain.Models
{
    public interface IKycInformationStatusChange
    {
        public Guid PartnerId { get; set; }
        public Guid AdminUserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Comment { get; set; }
        public KycStatus? OldKycStatus { get; set; }
        public KycStatus NewKycStatus { get; set; }
    }
}
