using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.ClientDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Repositories.ClientHomeService
{
    /// <summary>
    /// Contract for building client home views (promotions and active categories/services).
    /// </summary>
    public interface IClientHomeService
    {
        /// <summary>
        /// Get a client's available promotions and active categories/services.
        /// </summary>
        Task<ResultDTO<object>> ClientHomePage(int clientId);
        
        /// <summary>
        /// Build home data for all clients (non-taken promos and active services per client).
        /// </summary>
        Task<ResultDTO<List<GetAllHomeServiceDTO>>> GetAllClientWithServiceAsync();

        Task<ResultDTO<object>> ClientHomePageForGuest();

        /// <summary>
        /// Deletes a service by its ID
        /// </summary>
        /// <param name="serviceId">The ID of the service to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<ResultDTO<object>> DeleteService(int serviceId);

        /// <summary>
        /// Deletes a category by its ID
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<ResultDTO<object>> DeleteCategory(int categoryId);
    }
}
