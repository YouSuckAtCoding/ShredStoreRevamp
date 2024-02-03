using Application.Models;
using Application.Services.UserServices;
using Contracts.Request;
using Contracts.Response.UserResponses;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Mapping;

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
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(typeof(UsersResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var result = await _userService.GetUsers(token);
            return Ok(result);
        }

        [HttpPost(ApiEndpoints.UserEndpoints.Create)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken token)
        {
            User user = request.MapToUser();
            
            bool result = await _userService.InsertUser(user, token);
            if(result)
                return Ok(result);

            return BadRequest();

        }
        [HttpGet(ApiEndpoints.UserEndpoints.Get)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest request, CancellationToken token)
        {
            User user = request.MapToUser();

            User updated = await _userService.UpdateUser(user, token);

            return Ok(updated.MapToUserResponse());         
        }

        [HttpPut(ApiEndpoints.UserEndpoints.ResetPassword)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordUserRequest request, CancellationToken token)
        {
            LoginUserRequest login = request.MapToLoginUserRequest();
            var result = await _userService.Login(login, token);
            if(result is not null)
            {
                await _userService.ResetPassword(request, token);
                return Ok();
            }
            return NotFound();
            
            
        }

        [HttpDelete(ApiEndpoints.UserEndpoints.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
        {
            var result = await _userService.DeleteUser(id, token);
            return result ? Ok() : NotFound();
        }
    }
}
