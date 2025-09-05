using Hoshi.Models.UserModels;

namespace Hoshi.Repositories.TokenService
{
    public interface ITokenService
    {
        /// <summary>
        /// Create a signed JWT for the specified user (includes roles and JTI).
        /// </summary>
        Task<string> CreateTokenAsync(User applicationUser);
        /// <summary>
        /// Invalidate a token by blacklisting its JTI until its expiration.
        /// </summary>
        Task InvalidateTokenAsync(string token);
        /// <summary>
        /// Check whether a token is blacklisted/invalidated.
        /// </summary>
        Task<bool> IsTokenInvalidated(string token);

    }
}
