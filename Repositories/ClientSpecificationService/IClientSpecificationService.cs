using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Repositories.ClientSpecificationService
{
    public interface IClientSpecificationService
    {
        Task<ResultDTO<ClientSpecificationGetDTO>> GetById(int userId);
        Task<ResultDTO<ClientSpecificationGetDTO>> Update(ClientSpecificationPutDTO putDTO);
    }
}
