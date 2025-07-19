using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;

namespace Hoshi.Repositories.UserService
{
    public interface IUserService
    {
        Task<ResultDTO<object>> Register(ApplicationUserRegisterRequestDto registerRequestDto);
        Task<ResultDTO<object>> Login(ApplicationUserLoginRequestDto loginRequestDto);
        Task<ResultDTO<string>> Delete(string id);
        Task<ResultDTO<string>> Edit(ApplicationUserEditRequestDto userEditRequestDto);
        Task<ResultDTO<UserGetDTO>> GetById(string id);
        Task<ResultDTO<object>> GetAllUsers();
        Task<ResultDTO<string>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
        ResultDTO<object> GetCurrentUserId();
        Task<ResultDTO<string>> CreateResetPasswordTokenAsync(string email);

        //Task<ResultDTO<string>> BanUserAsync(string userId);
        //Task<ResultDTO<string>> UnbanUserAsync(string userId);
        Task<ResultDTO<string>> Logout(string userId = null);
    }
}
