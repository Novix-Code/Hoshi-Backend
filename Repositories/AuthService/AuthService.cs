using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;

namespace Hoshi.Repositories.AuthService
{
    public class AuthService : IAuthService
    {
        public Task<ResultDTO<string>> CreateResetPasswordTokenAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO<string>> Edit(ApplicationUserEditRequestDto userEditRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO<object>> Login(ApplicationUserLoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO<string>> Logout(string userId = null)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO<object>> Register(ApplicationUserRegisterRequestDto registerRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO<string>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
