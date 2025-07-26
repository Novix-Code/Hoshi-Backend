using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;

namespace Hoshi.Repositories.UserService
{
    public interface IUserService
    {

        Task<ResultDTO<object>> overViewPage();
        Task<ResultDTO<object>> Clientpage();
        Task<ResultDTO<object>> ClientDetails(int Id);
        Task<ResultDTO<object>> WorkerPage();
            
    }
}
