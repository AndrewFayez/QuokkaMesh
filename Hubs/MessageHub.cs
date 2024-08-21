

using Microsoft.AspNetCore.SignalR;
using QuokkaMesh.Models.DataModels.Notify;

namespace QuokkaMesh.Hubs
{
    public class MessageHub : Hub<IMessageHubClient> 
    {
        public async Task SendNotification( NotificationDTO notification)
        {
            await Clients.All.SendNotification(notification);
        }
    }
}
