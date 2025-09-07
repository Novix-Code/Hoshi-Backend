using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;

namespace Hoshi.Repositories.ClientOrderService
{
    /// <summary>
    /// Abstraction for client-facing order operations: create, list, details, delete.
    /// </summary>
    public interface IClientOrderService
    {
        /// <summary>
        /// Create a new order and initialize related records (status history, invoice, promotions, images).
        /// </summary>
        Task<ResultDTO<object>> AddOrderAsync(OrderPostDTO dto);
        /// <summary>
        /// Get all non-cancelled orders for clients with minimal projection.
        /// </summary>
        Task<ResultDTO<List<OrderGetAllDto>>> GetAllClientsAsync();
        /// <summary>
        /// Get order details including visits, offers, and invoice.
        /// </summary>
        Task<ResultDTO<OrderGetDetailsDto>> GetOrderDetails(int orderId);
        /// <summary>
        /// Cancel/delete order with business-time-window dependent fees and notifications.
        /// </summary>
        Task<ResultDTO<string>> DeleteOrder(int orderId);

    }
}
