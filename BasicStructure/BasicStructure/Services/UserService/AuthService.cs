using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BasicStructure.Services.UserService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole<int>> _role;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> user,
            RoleManager<IdentityRole<int>> role,
            IConfiguration config)
        {
            _user = user;
            _role = role;
            _config = config;
        }
        public async Task<bool> RegisterUser(RegisterUserDTO user, int roleId)
        {
            var role = await _role.FindByIdAsync(roleId.ToString());
            if (role != null)
            {
                var identityUser = new ApplicationUser
                {
                    Email = user.Email,
                    UserName = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                var result = await _user.CreateAsync(identityUser, user.Password);
                if (result.Succeeded)
                {
                    result = await _user.AddToRoleAsync(identityUser, role.Name);
                }
                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> LoginUser(Models.LoginUser user)
        {
            var identityUser = await _user.FindByEmailAsync(user.Email);
            if (identityUser == null)
                return false;
            return await _user.CheckPasswordAsync(identityUser, user.Password);
        }

        public string GenerateTokenString(Models.LoginUser user)
        {
            IEnumerable<System.Security.Claims.Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
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
