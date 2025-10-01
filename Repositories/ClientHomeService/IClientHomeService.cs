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
    }
}
