
namespace BasicStructure.Services.UserService
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(Models.LoginUser user);

        Task<bool> LoginUser(Models.LoginUser user);

        string GenerateTokenString(Models.LoginUser user);

    }
}