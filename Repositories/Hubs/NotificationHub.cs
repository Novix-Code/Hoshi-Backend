
using Microsoft.AspNetCore.SignalR;

namespace Hoshi.Repositories.Hubs
{
    /// <summary>
    /// SignalR hub for broadcasting role-based notifications.
    /// Adds authenticated connections to role groups and exposes a simple broadcast method.
    /// </summary>
    public class NotificationHub : Hub<INotificationHub>
    {
        /// <summary>
        /// On connect, add the connection to a group based on user role.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            if (user.Identity.IsAuthenticated)
            {
                if (user.IsInRole("Admin"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
                }
                else if (user.IsInRole("Client"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Client");
                }
                else if (user.IsInRole("Worker"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Worker");
                }

                await Clients.Caller.ReceiveMessage("تم إضافتك للجروب حسب دورك");
            }
            else
            {
                await Clients.Caller.ReceiveMessage("غير مصرح لك بالاتصال");
                Context.Abort(); // إنهاء الاتصال لو مش متسجل
            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Broadcast a message to all connections.
        /// </summary>
        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage($"{message}");
        }

    }
}
