using Hoshi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hoshi.Repositories.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(
            IMemoryCache cache,
            IConfiguration configuration,
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
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
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // In your TokenService
        public async Task InvalidateTokenAsync(string token)
        {
            // Just store the JWT's unique ID ("jti") with expiry
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var jti = jwt.Id; // Unique token identifier

            _cache.Set($"invalidated_jti:{jti}", true, jwt.ValidTo);
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
