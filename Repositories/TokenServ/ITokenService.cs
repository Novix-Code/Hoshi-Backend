using Hoshi.Models.UserModels;

namespace Hoshi.Repositories.TokenServ
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User applicationUser);
        Task InvalidateTokenAsync(string token);      // New: For logout
        Task<bool> IsTokenInvalidated(string token);  // For validation checks

    }
}
