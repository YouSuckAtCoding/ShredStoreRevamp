using Application.Models;
using Contracts.Request;
using Contracts.Response.UserResponses;

namespace ShredStore.Mapping
{
    public static class UserMapping
    {
        public static User MapToUser(this CreateUserRequest request)
        {
            return new User
            {
                Name = request.Name,
                Cpf = request.Cpf,
                Address = request.Address,
                Age = request.Age,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            };
        }

        public static User MapToUser(this UpdateUserRequest request)
        {
            return new User
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address,
                Role = request.Role

            };
        }

        public static UserResponse MapToUserResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Address = user.Address,
                Email = user.Email,
                Role = user.Role,
                Cpf = user.Cpf
            };
            
        }
        public static LoginUserRequest MapToLoginUserRequest(this ResetPasswordUserRequest user)
        {
            return new LoginUserRequest
            {
                Email = user.Email,
                Password = user.Password
            };
        }

        
    }
}
