using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;

namespace Hoshi.Repositories.OrderVisitService
{
    public interface IOrderVisitService
    {
        Task sendNoificationforclient(int orderId , string message);
        Task sendNoificationforclient2(int VisitId, string message);
        Task<ResultDTO<object>> addVisitAsync(OrderVisitPostDTO orderVisit);
    }
}
