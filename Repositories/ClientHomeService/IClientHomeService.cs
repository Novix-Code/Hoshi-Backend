using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.ClientDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Repositories.ClientHomeService
{
    public interface IClientHomeService
    {
        Task<ResultDTO<ClientHomeDto>> GetClientWithServiceById(int ClientId);
        Task<ResultDTO<List<GetAllHomeServiceDTO>>> GetAllClientWithServiceAsync();  
    }
}
