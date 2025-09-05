using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;

namespace Hoshi.Repositories.OrderVisitService
{
    /// <summary>
    /// Contract for creating order visits and sending related notifications.
    /// </summary>
    public interface IOrderVisitService
    {
        /// <summary>
        /// Send a message to the client related to an order.
        /// </summary>
        Task sendNoificationforclient(int orderId , string message);
        /// <summary>
        /// Send a message to the client related to a visit.
        /// </summary>
        Task sendNoificationforclient2(int VisitId, string message);
        /// <summary>
        /// Create a visit for an order and initialize its invoice data.
        /// </summary>
        Task<ResultDTO<object>> addVisitAsync(OrderVisitPostDTO orderVisit);
    }
}
