using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;
using Hoshi.Models.OrderModels;

namespace Hoshi.Repositories.WorkerHomeService
{
    public interface IWorkerHomeService
    {
        Task<ResultDTO<WorkerOrderDetailsDto>> GetWorkerHomeAsync(int workerId);
        Task<ResultDTO<List<OrderGetDTO>>> SearchOrdersAsync(OrderSearchRequestDto searchRequest);
    }
}
