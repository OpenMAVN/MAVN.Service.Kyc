using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MAVN.Service.Kyc.Domain.Enums;
using MAVN.Service.Kyc.Domain.Models;

namespace MAVN.Service.Kyc.MsSqlRepositories.Entities
{
    [Table("kyc_status_change")]
    public class KycInformationStatusChangeEntity : IKycInformationStatusChange
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

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

        [Column("old_status")]
        public KycStatus? OldKycStatus { get; set; }

        [Required]
        [Column("new_status")]
        public KycStatus NewKycStatus { get; set; }

        public static KycInformationStatusChangeEntity Create(Guid partnerId, Guid adminId, DateTime timestamp, string comment, KycStatus? oldStatus, KycStatus newStatus)
        {
            return new KycInformationStatusChangeEntity
            {
                PartnerId = partnerId,
                AdminUserId = adminId,
                Comment = comment,
                Timestamp = timestamp,
                NewKycStatus = newStatus,
                OldKycStatus = oldStatus,
            };
        }
    }
}
