using Application.Models;
using Application.Repositories.UserStorage;
using Contracts.Request;
using FluentValidation;

namespace Application.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _userValidator;
        private readonly IValidator<LoginUserRequest> _loginValidator;

        public UserService(IUserRepository userRepository, IValidator<User> userValidator, IValidator<LoginUserRequest> loginValidator)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
            _loginValidator = loginValidator;
        }

        public async Task<bool> DeleteUser(int id, CancellationToken token)
        {

            var result = await _userRepository.GetUser(id, token);
            if (result is not null)
            {
                await _userRepository.DeleteUser(id, token);
                return await Task.FromResult(true);
            }
            else
                return await Task.FromResult(false);
        }

        public async Task<User?> GetUser(int id, CancellationToken token)
        {
            var result = await _userRepository.GetUser(id, token);
            return result is null ? null : result;
        }

        public async Task<IEnumerable<User>> GetUsers(CancellationToken token)
        {
            IEnumerable<User> users = await _userRepository.GetUsers(token);
            return users;
        }

        public async Task<bool> InsertUser(User user, CancellationToken token)
        {
            await _userValidator.ValidateAndThrowAsync(user, token);
            await _userRepository.InsertUser(user, token);
            return await Task.FromResult(true);
        }

        public async Task<User?> Login(LoginUserRequest user, CancellationToken token)
        {
            await _loginValidator.ValidateAndThrowAsync(user, token);
            var result = await _userRepository.Login(user, token);
            return result is null ? null : result;
        }

        public async Task<User> UpdateUser(User request, CancellationToken token)
        {
            await _userRepository.UpdateUser(request, token);
            return await _userRepository.GetUser(request.Id, token);

        }
    }
}
