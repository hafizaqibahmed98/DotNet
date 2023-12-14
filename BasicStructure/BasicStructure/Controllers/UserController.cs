using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BasicFlowApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class UserController : ControllerBase
    {
        private readonly InterfaceUserService _userService;

        public UserController(InterfaceUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetUserDTO>>>> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("id")]
        public async Task<ActionResult<ServiceResponse<GetUserDTO>>> GetUserById(int id)
        {
            return Ok(await _userService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetUserDTO>>>> AddUser(AddUserDTO user)
        {
            return Ok(await _userService.AddUser(user));
        }

        [HttpPut("id")]
        public async Task<ActionResult<ServiceResponse<List<GetUserDTO>>>> UpdateUserById(int id, AddUserDTO user)
        {
            return Ok(await _userService.UpdateUser(id, user));
        }

        [HttpDelete("id")]
        public async Task<ActionResult<ServiceResponse<List<GetUserDTO>>>> DeleteUserById(int id)
        {
            return Ok(await _userService.DeleteUser(id));
        }

    }
}
