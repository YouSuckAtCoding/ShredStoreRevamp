using Application.Models;
using Contracts.Request;
using Contracts.Response;

namespace ShredStore.Mapping
{
    public static class ContractMapping
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
                Password = request.Password

            };
        }
        public static User MapToUser(this UpdateUserRequest request)
        {
            return new User
            {
                Id = request.Id,
                Name = request.Name,
                Cpf = request.Cpf,
                Address = request.Address,
                Age = request.Age,
                Email = request.Email

            };
        }
        public static UserResponse MapToUserResponse(this User user)
        {
            UserResponse response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Address = user.Address,
                Email = user.Email,
                Role = user.Role,
                Cpf = user.Cpf
            };
            return response;
        }

        
    }
}
