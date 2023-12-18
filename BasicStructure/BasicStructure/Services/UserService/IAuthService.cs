
using Microsoft.AspNetCore.Mvc;

namespace BasicStructure.Services.UserService
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(RegisterUserDTO user, int role);

        Task<bool> LoginUser(Models.LoginUser user);

        string GenerateTokenString(Models.LoginUser user);

        Task<bool> CheckPermission(User user, int apiId);

        User DecodeToken(string token);

    }
}