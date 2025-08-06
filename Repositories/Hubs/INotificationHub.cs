namespace Hoshi.Repositories.Hubs
{
    public interface INotificationHub
    {
        Task ReceiveMessage(string message);
        
    }
}
