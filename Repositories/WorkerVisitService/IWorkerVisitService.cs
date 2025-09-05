using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.Enums;
using Hoshi.Models.OrderModels;

namespace Hoshi.Repositories.WorkerVisitService
{
    /// <summary>
    /// Worker visit operations: create, complete, cancel; fee computations; utilities.
    /// </summary>
    public interface IWorkerVisitService
    {
        /// <summary>
        /// Create a visit for an order and prepare temp invoice with fees.
        /// </summary>
        Task<ResultDTO<OrderVisitGetDTO>> AddVisitAsync(OrderVisitPostDTO dto);
        /// <summary>
        /// Complete a visit, apply financial updates, and log wallet history.
        /// </summary>
        Task<ResultDTO<bool>> CompleteVisitAsync(int visitId);
        /// <summary>
        /// Cancel a visit and deduct cancellation fee when applicable.
        /// </summary>
        Task<ResultDTO<bool>> CancelVisitAsync(int visitId);
        /// <summary>
        /// Get fee tuple (main, min, max) for a service/fee type.
        /// </summary>
        Task<(double main, double min, double max)> GetFeeAsync(FeeType type, Order order);
        /// <summary>
        /// Clamp a value to min/max if applicable.
        /// </summary>
        double Clamp(double value, double min, double max);

    }
}
