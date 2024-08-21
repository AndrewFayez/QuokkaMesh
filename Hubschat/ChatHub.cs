using Microsoft.AspNetCore.SignalR;

namespace ChatWebAPI.Hubs
{
    public class ChatHub :Hub<IChatClient>
    {
        public async Task SendMessage(string fromUser, string toUser, string message)
        {
            await Clients.User(toUser).ReceiveMessage(fromUser, message);
        }

        public async Task JoinRoom(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task LeaveRoom(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
    }
}

public interface IChatClient
{
    Task ReceiveMessage(string fromUser, string message);
}