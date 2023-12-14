using Microsoft.AspNetCore.Mvc;

namespace BasicStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthUserController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("Register")]
        public async Task<bool> RegisterUser(RegisterUserDTO user)
        {
            return await _authService.RegisterUser(user);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(Models.LoginUser user)
        {
            var result =  await _authService.LoginUser(user);
            if(result == true)
            {
                var tokenString = _authService.GenerateTokenString(user);
                return Ok(tokenString);
            }
            return BadRequest(result);

        }
    }
}
