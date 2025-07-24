using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;

namespace Hoshi.Repositories.EmailServiceFold
{
    public interface IEmailService
    {
        public Task<ResultDTO<string>> SendEmail(string email, string AdminCode, string DefaultPassword, string userName);
        public Task<ResultDTO<string>> SendOTP(string email);
        Task<ResultDTO<string>> SendVerifivationCode(string email);
    }
}
