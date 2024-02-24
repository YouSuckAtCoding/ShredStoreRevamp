using ShredStorePresentation.Models.User.Request;
using ShredStorePresentation.Models.User.Response;

namespace ShredStorePresentation.Services.UserService
{
    public interface IUserHttpService
    {
        Task<bool> Create(UserRegistrationViewRequest user);
        Task<bool> Delete(int sessionId);
        Task<UserViewResponse> EditUser(UserUpdateViewRequest userEdit);
        Task<UserViewResponse> GetById(int id);
        Task<UserViewResponse> Login(UserLoginViewRequest user);
        Task<bool> ResetUserPassword(UserResetPasswordViewRequest newPassword);
    }
}