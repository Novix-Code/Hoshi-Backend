using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;

namespace Hoshi.Repositories.ServiceService
{
    public interface IServiceService
    {
        Task<ResultDTO<List<ServiceCategoryGetDTO>>> searchServiceAsyn(string serviceName);
        Task<ResultDTO<ServiceGetDTO>> AddService(ServicePostDTO postDTO);
        Task<ResultDTO<ServiceGetDTO>> UpdateService(ServicePutDTO putDTO);

    }
}
