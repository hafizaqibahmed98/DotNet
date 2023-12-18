
namespace BasicStructure.Services.UserService
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(RegisterUserDTO user, int role);

        Task<bool> LoginUser(Models.LoginUser user);

        string GenerateTokenString(Models.LoginUser user);

    }
}