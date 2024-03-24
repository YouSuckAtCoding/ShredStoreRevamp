using Contracts.Request;
using Contracts.Response.UserResponses;

namespace ShredStorePresentation.Services.UserService
{
    public interface IUserHttpService
    {
        Task<bool> Create(CreateUserRequest user);
        Task<bool> Delete(int sessionId);
        Task<UserResponse> EditUser(UpdateUserRequest userEdit);
        Task<UserResponse> GetById(int id);
        Task<UserResponse> Login(LoginUserRequest user);
        Task<bool> ResetUserPassword(ResetPasswordUserRequest newPassword);
        Task<IEnumerable<UserResponse>> GetAll();
    }
}