using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;

namespace Hoshi.Repositories.WorkerHomeService
{
    /// <summary>
    /// Worker home data: lists of offers, upcoming orders, nearby orders, and search.
    /// </summary>
    public interface IWorkerHomeService
    {
        /// <summary>
        /// Get worker home details including offers, upcoming and nearby orders.
        /// </summary>
        Task<ResultDTO<object>> GetWorkerHomeAsync(int workerId);
        /// <summary>
        /// Search published orders by service and city names.
        /// </summary>
        Task<ResultDTO<List<OrderBasicDTO>>> SearchOrdersAsync(OrderSearchRequestDto searchRequest);
    }
}
