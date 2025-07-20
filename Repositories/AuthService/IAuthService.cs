using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;

namespace Hoshi.Repositories.AuthService
{
    public interface IAuthService
    {
        Task<ResultDTO<object>> Register(ApplicationUserRegisterRequestDto registerRequestDto);
        Task<ResultDTO<object>> Login(ApplicationUserLoginRequestDto loginRequestDto);
        Task<ResultDTO<string>> Edit(ApplicationUserEditRequestDto userEditRequestDto);
        Task<ResultDTO<string>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<ResultDTO<string>> CreateResetPasswordTokenAsync(string email);
        Task<ResultDTO<string>> Logout(string userId = null);

    }
}
