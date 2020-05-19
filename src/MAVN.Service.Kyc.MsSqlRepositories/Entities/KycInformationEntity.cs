using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MAVN.Service.Kyc.Domain.Enums;
using MAVN.Service.Kyc.Domain.Models;

namespace MAVN.Service.Kyc.MsSqlRepositories.Entities
{
    [Table("kyc_information")]
    public class KycInformationEntity : IKycInformation
    {
        [Key]
        [Required]
        [Column("partner_id")]
        public Guid PartnerId { get; set; }

        [Required]
        [Column("admin_user_id")]
        public Guid AdminUserId { get; set; }

        [Required]
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        [Required]
        [Column("status")]
        public KycStatus KycStatus { get; set; }

        public static KycInformationEntity Create(IKycInformation model)
        {
            return new KycInformationEntity
            {
                KycStatus = model.KycStatus,
                PartnerId = model.PartnerId,
                AdminUserId = model.AdminUserId,
                Comment = model.Comment,
                Timestamp = model.Timestamp,
            };
        }
    }
}
