using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Models.UserModels;

namespace Hoshi.Repositories.EmailServiceFold
{
    public interface IEmailService
    {
        public Task<ResultDTO<string>> SendEmail(string email, string AdminCode, string DefaultPassword, string userName);
        public Task<ResultDTO<string>> SendOTP(string email);
        Task<ResultDTO<string>> SendVerifivationCode(string email);
        Task<ResultDTO<object>> checkOTPVerfication(string otp, string userId);
        Task<ResultDTO<object>> ReSetOtp(string email);
        Task<bool> CanConnectToMailServerAsync(string email);
    }
}
