using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.ClientDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Repositories.ClientHomeService
{
    public interface IClientHomeService
    {
        Task<ResultDTO<ClientHomeDto>> GetByIdServiceAsync(int Id);
        Task<ResultDTO<List<GetAllHomeServiceDTO>>> GetAllServiceAsync();  
    }
}
