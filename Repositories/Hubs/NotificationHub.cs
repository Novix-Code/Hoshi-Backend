
using Microsoft.AspNetCore.SignalR;

namespace Hoshi.Repositories.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
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

                await Clients.Caller.ReceiveMessage("✔️ تم إضافتك للجروب حسب دورك");
            }
            else
            {
                //await Clients.Caller.ReceiveMessage("❌ غير مصرح لك بالاتصال");
                //Context.Abort(); // إنهاء الاتصال لو مش متسجل
            }

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage($"{message}");
        }

    }
}
