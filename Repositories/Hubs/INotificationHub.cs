namespace Hoshi.Repositories.Hubs
{
    /// <summary>
    /// Client interface that the NotificationHub can call on connected clients.
    /// </summary>
    public interface INotificationHub
    {
        /// <summary>
        /// Receive a plain text notification message.
        /// </summary>
        Task ReceiveMessage(string message);
        
    }
}
