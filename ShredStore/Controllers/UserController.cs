using Application.Models;
using Application.Repositories.UserStorage;
using Application.Services.UserServices;
using Contracts.Request;
using Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Mapping;
using static ShredStore.ApiEndpoints;

namespace ShredStore.Controllers
{
    
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(ApiEndpoints.UserEndpoints.Login)]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginUserRequest user, CancellationToken token)
        {

            var users = await _userService.Login(user, token);
            if (users != null)
            {
                UserResponse result = users.MapToUserResponse();
                return Ok(result);
            }
            return Unauthorized();

        }

        [HttpGet(ApiEndpoints.UserEndpoints.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var result = await _userService.GetUsers(token);
            return Ok(result);
        }

        [HttpPost(ApiEndpoints.UserEndpoints.Create)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken token)
        {
            User user = request.MapToUser();
            
            bool result = await _userService.InsertUser(user, token);
            if(result)
                return Ok(result);

            return BadRequest();

        }
        [HttpGet(ApiEndpoints.UserEndpoints.Get)]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken token)
        {
            User? user = await _userService.GetUser(id, token);
            if (user is not null)
            {
                var result = user.MapToUserResponse();
                return Ok(result);
            }
            return NotFound();
            
        }

        [HttpPut(ApiEndpoints.UserEndpoints.Update)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest request, CancellationToken token)
        {
            User user = request.MapToUser();

            User updated = await _userService.UpdateUser(user, token);

            return Ok(updated);

                            
        }

        [HttpDelete(ApiEndpoints.UserEndpoints.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
        {
            var result = await _userService.DeleteUser(id, token);
            return result ? Ok() : BadRequest();
        }
    }
}
