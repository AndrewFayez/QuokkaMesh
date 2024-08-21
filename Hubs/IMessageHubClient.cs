

using Microsoft.AspNetCore.SignalR;
using QuokkaMesh.Models.DataModels.Notify;

namespace QuokkaMesh.Hubs
{
    public interface IMessageHubClient 
    {
        Task SendNotification(NotificationDTO notification);
    }
}
