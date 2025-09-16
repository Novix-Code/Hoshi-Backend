using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;

namespace Hoshi.Repositories.AuthService
{
    public interface IAuthService
    {
        Task<ResultDTO<UserGetDTO>> Register(
            UserType userType,
            ApplicationUserRegisterRequestDto registerRequestDto,
            bool canAddAdmin = false
        );
        Task<ResultDTO<UserGetDTO>> Login(ApplicationUserLoginRequestDto loginRequestDto);
        Task<ResultDTO<string>> Delete(string id);
        Task<ResultDTO<string>> Edit(ApplicationUserEditRequestDto userEditRequestDto);
        Task<ResultDTO<object>> GetAllUsers();
        Task<ResultDTO<string>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
        ResultDTO<object> GetCurrentUserId();
        Task<ResultDTO<string>> CreateResetPasswordTokenAsync(string email);
        Task<ResultDTO<string>> Logout();
        Task<ResultDTO<string>> BeWorkerAsync(BeWorkerRequestDTO request);
    }
}
