using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;

namespace Hoshi.Repositories.OrderService
{
    public interface IOrderService
    {
        Task<ResultDTO<SubmittedOrderDetailsDto>> GetSubmittedOrderDetailsAsync(int orderId);
        Task<ResultDTO<OrderClientDetailsDto>> GetOrderClientDetailsAsync(int orderId);
        Task<ResultDTO<bool>> CompleteOrderAsync(int orderId);
        Task<ResultDTO<object>> GetAssignedOrderAsync(int orderId);
    }
}
