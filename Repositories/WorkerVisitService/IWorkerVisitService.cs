using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.Enums;
using Hoshi.Models.OrderModels;

namespace Hoshi.Repositories.WorkerVisitService
{
    public interface IWorkerVisitService
    {
        Task<ResultDTO<OrderVisitGetDTO>> AddVisitAsync(OrderVisitPostDTO dto);
        Task<ResultDTO<bool>> CompleteVisitAsync(int visitId);
        Task<ResultDTO<bool>> CancelVisitAsync(int visitId);
        Task<(double main, double min, double max)> GetFeeAsync(FeeType type, Order order);
        double Clamp(double value, double min, double max);

    }
}
