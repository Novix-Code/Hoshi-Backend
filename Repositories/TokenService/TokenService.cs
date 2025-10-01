using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hoshi.Repositories.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly HoshiDbContext context;

        /// <summary>
        /// Issues JWTs and maintains an in-memory blacklist of invalidated tokens using the token's JTI.
        /// Note: Blacklist is memory-scoped; consider a distributed cache for multi-instance deployments.
        /// </summary>
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenService(
            HoshiDbContext context,
            IMemoryCache cache,
            IConfiguration configuration,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _configuration = configuration;
            _userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
            _cache = cache;

        }

        public async Task<string> CreateTokenAsync(User applicationUser)
        {
            //1-Set claims
            List<Claim> claimsList = new List<Claim>
            {
                new Claim("email", applicationUser.Email),
                new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            //Get user roles
            var roles = await _userManager.GetRolesAsync(applicationUser);
            foreach (var role in roles)
                claimsList.Add(new Claim("role", role));

            //2-Create secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //3-Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(2),
                claims: claimsList,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            //4-Return the token
            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // update user token if exist
            var updateUserToken = 
                await context.UserTokens
                    .Where(t => t.UserId == applicationUser.Id)
                    .ExecuteUpdateAsync(ut => ut.SetProperty(ut => ut.Value, accessToken));

            // if not exist add new one
            if (updateUserToken <= 0)
                await context.UserTokens.AddAsync(new IdentityUserToken<int>
                {
                    Name = "AccessToken",
                    LoginProvider = "Application",
                    UserId = applicationUser.Id,
                    Value = accessToken
                });

            await context.SaveChangesAsync();

            return accessToken;
        }

        public async Task<ResultDTO<string>> RefrshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);

            var nameIdentifier = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (nameIdentifier == null) return ResultDTO<string>.Unauthorized();

            int userId = int.Parse(nameIdentifier);

            var checkToken = await context.UserTokens.AsNoTracking().FirstOrDefaultAsync(t => t.Value == token && t.UserId == userId);
            if (checkToken == null)
                return ResultDTO<string>.BadRequest(new ErrorDTO
                {
                    ErrorAr = "التوكين غير صحيح. بالرجاء اعادة تسجيل الدخول من جديد.",
                    ErrorEn = "The token is invalid. Please log in again."
                });

            var applicationUser = await _userManager.FindByIdAsync(nameIdentifier);
            if (applicationUser == null) return ResultDTO<string>.Unauthorized();

            string newToken = await CreateTokenAsync(applicationUser);

            return ResultDTO<string>.Success(null, token: newToken);
        }

        // In your TokenService
        public async Task InvalidateTokenAsync(string token)
        {
            // Just store the JWT's unique ID ("jti") with expiry
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var jti = jwt.Id; // Unique token identifier

            _cache.Set($"invalidated_jti:{jti}", true, jwt.ValidTo);
            // Suggested improvement (use distributed cache for scalability):
            // _distributedCache.SetString($"invalidated_jti:{jti}", "1", new DistributedCacheEntryOptions { AbsoluteExpiration = jwt.ValidTo });
        }

        public async Task<bool> IsTokenInvalidated(string token)
        {
            if (string.IsNullOrEmpty(token))
                return true;

            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var jti = jwt.Id;
                return _cache.TryGetValue($"invalidated_jti:{jti}", out _);
            }
            catch
            {
                return true; // Consider invalid if we can't read the token
            }
        }


    }
}
