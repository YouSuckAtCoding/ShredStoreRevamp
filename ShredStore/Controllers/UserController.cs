using Application.Models;
using Application.Services.JwtServices;
using Application.Services.UserServices;
using Contracts.Request;
using Contracts.Response.JwtResponses;
using Contracts.Response.UserResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShredStore.Jwt;
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

        [HttpPost(ApiEndpoints.UserEndpoints.Loginv2)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> LoginPost([FromBody] LoginUserRequest user, CancellationToken token)
        {

            var users = await _userService.Login(user, token);
            if (users != null)
            {
                UserResponse result = users.MapToUserResponse();
                return Ok(result);
            }
            return Unauthorized();

        }


        [HttpGet(ApiEndpoints.UserEndpoints.Login)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpGet(ApiEndpoints.UserEndpoints.GetAll)]
        [ProducesResponseType(typeof(UsersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var result = await _userService.GetUsers(token);
            return Ok(result);

        }

        [HttpPost(ApiEndpoints.UserEndpoints.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken token)
        {
            User user = request.MapToUser();

            bool result = await _userService.InsertUser(user, token);
            if (result)
                return Created(Constants.CreatedReponseUri, user);

            return BadRequest();

        }

        [Authorize(AuthConstants.CustomerPolicyName)]
        [HttpGet(ApiEndpoints.UserEndpoints.Get)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [Authorize(AuthConstants.CustomerPolicyName)]
        [HttpPut(ApiEndpoints.UserEndpoints.Update)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] UpdateUserRequest request, CancellationToken token)
        {
            User user = request.MapToUser();

            User updated = await _userService.UpdateUser(user, token);

            return updated.Id > 0 ? Ok(updated.MapToUserResponse()) : NotFound();
        }

        [Authorize(AuthConstants.CustomerPolicyName)]
        [HttpPut(ApiEndpoints.UserEndpoints.ResetPassword)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordUserRequest request, CancellationToken token)
        {
            LoginUserRequest login = request.MapToLoginUserRequest();

            var result = await _userService.Login(login, token);

            if (result.Id > 0)
            {
                await _userService.ResetPassword(request, token);
                return Ok();
            }
            return NotFound();
        }

        [Authorize(AuthConstants.CustomerPolicyName)]
        [HttpDelete(ApiEndpoints.UserEndpoints.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
        {
            var result = await _userService.DeleteUser(id, token);
            return result ? Ok() : NotFound();
        }
    }
}
