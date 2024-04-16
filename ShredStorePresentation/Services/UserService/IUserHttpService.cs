using Contracts.Request;
using Contracts.Response.UserResponses;

namespace ShredStorePresentation.Services.UserService
{
    public interface IUserHttpService
    {
        Task<bool> Create(CreateUserRequest user);
        Task<bool> Delete(int sessionId, string token);
        Task<UserResponse> EditUser(UpdateUserRequest userEdit, string token);
        Task<UserResponse> GetById(int id, string token);
        Task<UserResponse> Login(LoginUserRequest user);
        Task<bool> ResetUserPassword(ResetPasswordUserRequest newPassword, string token);
        Task<IEnumerable<UserResponse>> GetAll(string token);
    }
}