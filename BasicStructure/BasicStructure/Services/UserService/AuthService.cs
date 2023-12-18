using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace BasicStructure.Services.UserService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole<int>> _role;
        private readonly IConfiguration _config;
        public DataContext _context { get; }

        public AuthService(UserManager<ApplicationUser> user,
            RoleManager<IdentityRole<int>> role,
            IConfiguration config,
            DataContext context)
        {
            _user = user;
            _role = role;
            _config = config;
            _context = context;
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
            var identityUser = _user.FindByEmailAsync(user.Email).Result;
            var roleId = "dimmy";
            if (identityUser != null)
            {
                // Get user roles
                var roles = _user.GetRolesAsync(identityUser).Result;
                foreach (var roleName in roles)
                {
                    var role = _role.FindByNameAsync(roleName);

                    if (role != null)
                    {
                        // Access the role ID using role.Id
                        roleId = role.Id.ToString();
                        // Do something with roleId
                    }
                }
                // Add roles as claims
            }
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", identityUser.Id.ToString()),
                new Claim("roleId", roleId),
            };
            IEnumerable<System.Security.Claims.Claim> claims = userClaims;
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

        public async Task<bool> CheckPermission(User user, int apiId)
        {
            var userId = user?.Id;
            var roleId = user?.RoleId;
            if(userId != null && roleId != null)
            {
                var dbPermissions = await _context.Permissions.ToListAsync();
                foreach (var permission in dbPermissions)
                {
                    if(permission.APIId == apiId && permission.IdentityRoleId == roleId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public User DecodeToken(string token)
        {
            // Decode the token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var roleClaims = jsonToken?.Claims.Where(claim => claim.Type == "roleId").Select(claim => claim.Value).ToList();
            var idClaims = jsonToken?.Claims.Where(claim => claim.Type == "UserId").Select(claim => claim.Value).ToList();
            User dataObject = new User()
            {
                Id = int.Parse(idClaims[0]),
                RoleId = int.Parse(roleClaims[0]),
            };
            return dataObject;
        }
    }
}
