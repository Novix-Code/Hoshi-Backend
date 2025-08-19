using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.DTOs.UserDTOs.SuspendedUserDTOs;

namespace Hoshi.Repositories.UserService
{
    public interface IUserService
    {
        Task<Tuple<bool, string>> AddUserImage(int id, IFormFile image, bool isUpdate);
        Task<ResultDTO<object>> overViewPage();
        Task<ResultDTO<object>> Clientpage();
        Task<ResultDTO<object>> ClientDetails(int Id);
        Task<ResultDTO<object>> WorkerDetails(int Id);
        Task<ResultDTO<object>> BeWorkerApproved(int Id);
        Task<ResultDTO<object>> BeWorkerReject(int Id , string rejectResoun);
        Task<ResultDTO<object>> WorkerPage();
        Task<ResultDTO<object>> DashbordWorkerDetails(int id);
        Task<ResultDTO<Object>> OrderPage();
        Task<ResultDTO<Object>> OrderDetails(int id);
        Task<ResultDTO<List<AdminWithRolesAndPermissionsDTO>>> GetAllAdminsWithRolesAndPermissionsAsync();

        Task<ResultDTO<object>> SuspendUser(SuspendedUserPostDTO suspendDTO);
    }
}
