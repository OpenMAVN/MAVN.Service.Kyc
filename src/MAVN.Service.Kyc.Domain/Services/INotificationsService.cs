using System.Threading.Tasks;

namespace MAVN.Service.Kyc.Domain.Services
{
    public interface INotificationsService
    {
        Task NotifyKycApprovedAsync(string adminUserId, string adminUserEmail, string adminUserName,
            string partnerName);

        Task NotifyKycRejectedAsync(string adminUserId, string adminUserEmail, string adminUserName, string partnerName,
            string rejectionComment);
    }
}
