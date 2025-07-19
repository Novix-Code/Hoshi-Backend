using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;

namespace Hoshi.Repositories.WorkerVisitService
{
    public interface IWorkerVisitService
    {
        Task<ResultDTO<OrderVisitGetDTO>> AddVisitAsync(OrderVisitPostDTO dto);
        Task<ResultDTO<bool>> CompleteVisitAsync(int visitId);
        Task<ResultDTO<bool>> CancelVisitAsync(int visitId);
    }
}
