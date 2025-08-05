using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;

namespace Hoshi.Repositories.OrderImageService
{
    public interface IOrderImageService
    {
        Task<ResultDTO<List<OrderImageGetDTO>>> AddImages(int orderId, List<IFormFile> images);
    }
}
