using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;

namespace Hoshi.Repositories.NotificationService
{
    /// <summary>
    /// Contract for sending and retrieving notifications for admins, clients, and workers.
    /// </summary>
    public interface INotificationServiceHandler
    {
        /// <summary>
        /// Send a notification to all admins and persist an admin notification per admin.
        /// </summary>
        Task sendMessagetoAdmin(string messge , int subId);
        /// <summary>
        /// Send a notification to a specific client and persist a user notification.
        /// </summary>
        Task sendMessagetoClient(string message , int Id);
        /// <summary>
        /// Send a notification to a specific worker and persist a user notification.
        /// </summary>
        Task sendMessagetoWorker(string message , int Id);
        /// <summary>
        /// Get notifications for the current admin user.
        /// </summary>
        Task<ResultDTO<object>> getNotificationsAdminAsync();
        /// <summary>
        /// Get notifications for the current client/worker user.
        /// </summary>
        Task<ResultDTO<object>> getNotificationsClientAndWorkerAsync();

    }
}
