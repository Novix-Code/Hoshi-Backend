using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;

namespace Hoshi.Repositories.OrderImageService
{
    /// <summary>
    /// Contract for adding images to orders.
    /// </summary>
    public interface IOrderImageService
    {
        /// <summary>
        /// Save and associate a list of images with an order.
        /// </summary>
        Task<ResultDTO<List<OrderImageGetDTO>>> AddImages(int orderId, List<IFormFile> images);
    }
}
