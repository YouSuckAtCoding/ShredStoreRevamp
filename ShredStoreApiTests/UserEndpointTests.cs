using Application;
using Application.Models;
using Application.Services.UserServices;
using Contracts.Request;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShredStore;
using ShredStoreApiTests.TestDoubles;
using ShredStoreTests.Fake;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ShredStoreApiTests
{
    public class UserEndpointTests
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };

        private const int userId = 2;
        private const string password = "123456789";
        private const string failPassword = "6374521878";
        private const string email = "admin@teste.com";
        

        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            var response = await client.GetAsync(ApiEndpoints.UserEndpoints.GetAll);
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
       
        [Fact]
        public void Should_Return_200_From_Login_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            var user = FakeDataFactory.FakeUser();

            HttpRequestMessage requestMessage = SetLogin(client);
            var response = client.SendAsync(requestMessage).ConfigureAwait(false);

            var responseInfo = response.GetAwaiter().GetResult();
            responseInfo.StatusCode.Should().Be(HttpStatusCode.OK);
            
        }
      
        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            string url = Utility.SetGet_Or_DeleteUrl(userId, ApiEndpoints.UserEndpoints.Get);
            var response = await client.GetAsync(url);
            response.Should().HaveStatusCode(HttpStatusCode.OK);

        }

       [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            string url = Utility.SetGet_Or_DeleteUrl(userId, ApiEndpoints.UserEndpoints.Delete);
            var response = await client.DeleteAsync(url);
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_200_From_ResetPassword_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            HttpRequestMessage requestMessage = SetResetPassword(client, FakeDataFactory.FakeCreateUserRequest());
            var response = await client.PutAsync(ApiEndpoints.UserEndpoints.ResetPassword, requestMessage.Content);
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        private static HttpRequestMessage SetResetPassword(HttpClient client, CreateUserRequest user)
        {
            ResetPasswordUserRequest request = new ResetPasswordUserRequest
            {
                Email = user.Email,
                Password = user.Password,
                NewPassword = password
            };
            var json = JsonSerializer.Serialize(request);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress + ApiEndpoints.UserEndpoints.Login),
                Content = new StringContent(json, encoding: Encoding.UTF8, Utility.content_Type)
            };
            return requestMessage;
        }

        private static HttpRequestMessage SetLogin(HttpClient client, bool fail = false)
        {
            LoginUserRequest request = new LoginUserRequest
            {
                Email = email,
                Password = password
            };


            if (fail) request.Password = failPassword;

            var json = JsonSerializer.Serialize(request);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress + ApiEndpoints.UserEndpoints.Login),
                Content = new StringContent(json, encoding: Encoding.UTF8, Utility.content_Type)
            };
            return requestMessage;
        }

  
        private ApiFactory CreateApiWithUserRepository<T>()
           where T : class, IUserService
        {
            var api = new ApiFactory(services =>
            {
                services.RemoveAll(typeof(IUserService));
               
                services.TryAddScoped<IUserService, T>();

            });
            return api;
        }
    }
}
