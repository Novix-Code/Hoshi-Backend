using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;

namespace Hoshi.Repositories.NotificationService
{
    public interface INotificationServiceHandler
    {
        Task sendMessagetoAdmin(string messge , int subId);
        Task sendMessagetoClient(string message , int Id);
        Task sendMessagetoWorker(string message , int Id);
        Task<ResultDTO<object>> getNotificationsAdminAsync();
        Task<ResultDTO<object>> getNotificationsClientAndWorkerAsync();

    }
}
