using Application.Models;
using Application.Repositories.UserStorage;
using Application.Services.UserServices;
using Contracts.Request;
using ShredStoreTests.Fake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreApiTests.TestDoubles
{
    public class StubSuccessUserRepository : IUserService
    {
        public Task<bool> DeleteUser(int id, CancellationToken token)
        {
            return Task.FromResult(true)!;
        }

        public Task<User?> GetUser(int id, CancellationToken token)
        {
            var res = FakeDataFactory.FakeUser();
            return Task.FromResult(res)!;
        }

        public Task<IEnumerable<User>> GetUsers(CancellationToken token)
        {
            var users = FakeDataFactory.FakeUsers();
            return Task.FromResult(users);
        }

        public Task<bool> InsertUser(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<User?> Login(LoginUserRequest user, CancellationToken token)
        {
            var res = FakeDataFactory.FakeUser();
            return Task.FromResult(res)!;
        }

        public Task<User> UpdateUser(User request, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
