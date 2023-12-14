using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BasicStructure.Services.UserService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _user;
        private readonly IConfiguration _config;

        public AuthService(UserManager<IdentityUser> user, IConfiguration config)
        {
            _user = user;
            _config = config;
        }
        public async Task<bool> RegisterUser(Models.LoginUser user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Username,
                Email = user.Username,
            };
            var result = await _user.CreateAsync(identityUser, user.Password);
            return result.Succeeded;
        }

        public async Task<bool> LoginUser(Models.LoginUser user)
        {
            var identityUser = await _user.FindByEmailAsync(user.Username);
            if (identityUser == null)
                return false;
            return await _user.CheckPasswordAsync(identityUser, user.Password);
        }

        public string GenerateTokenString(Models.LoginUser user)
        {
            IEnumerable<System.Security.Claims.Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Username),
            };
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes
                (_config.GetSection("JWT:Key").Value)
                );
            SigningCredentials SigningCred = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256
                );
            var securityoken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("JWT:Issuer").Value,
                audience: _config.GetSection("JWT:Audience").Value,
                signingCredentials: SigningCred
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityoken);
            return tokenString;
        }
    }
}
