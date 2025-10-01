using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;

namespace Hoshi.Repositories.ServiceService
{
    /// <summary>
    /// Service-related operations: creation/update with image handling, search, and dashboards.
    /// </summary>
    public interface IServiceService
    {
        /// <summary>
        /// Search services by name and group under categories.
        /// </summary>
        Task<ResultDTO<List<ServiceCategoryGetDTO>>> searchServiceAsyn(string serviceName);
        /// <summary>
        /// Create a new service with image upload.
        /// </summary>
        Task<ResultDTO<ServiceGetDTO>> AddService(ServicePostDTO postDTO);
        /// <summary>
        /// Update a service and optionally its image.
        /// </summary>
        Task<ResultDTO<ServiceGetDTO>> UpdateService(ServicePutDTO putDTO);
    }
}
