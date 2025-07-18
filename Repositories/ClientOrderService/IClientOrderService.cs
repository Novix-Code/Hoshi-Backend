using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;

namespace Hoshi.Repositories.ClientOrderService
{
    public interface IClientOrderService
    {
        Task<ResultDTO<string>> AddOrderAsync(OrderPostDTO dto);
        Task<ResultDTO<List<OrderGetAllDto>>> GetAllClientsAsync();
        Task<ResultDTO<OrderGetDetailsDto>> GetOrderDetails(int orderId);
        Task<ResultDTO<string>> DeleteOrder(int orderId);

    }
}
