using Microsoft.AspNetCore.SignalR;

namespace Team_07_PRN222_A02.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string title)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, title, message);
        }

    }
}
