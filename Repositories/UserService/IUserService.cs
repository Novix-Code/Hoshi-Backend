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
        Task<ResultDTO<object>> WorkerDetails(int Id);
        Task<ResultDTO<object>> BeWorkerApproved(int Id);
        Task<ResultDTO<object>> BeWorkerReject(int Id , string rejectResoun);
        Task<ResultDTO<object>> WorkerPage();
        Task<ResultDTO<object>> DashbordWorkerDetails(int id);


            
    }
}
