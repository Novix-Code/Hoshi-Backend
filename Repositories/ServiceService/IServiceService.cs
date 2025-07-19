using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.ClientDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;

namespace Hoshi.Repositories.ServiceService
{
    public interface IServiceService
    {
        Task<ResultDTO<List<ServiceCategoryGetDTO>>> searchServiceAsyn(string serviceName);
    }
}
