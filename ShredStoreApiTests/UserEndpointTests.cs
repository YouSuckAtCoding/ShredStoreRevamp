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
        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            var response = await client.GetAsync(ApiEndpointsTest.User.GetAll);
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Should_Return_All_Users_Real_GetAll_Endpoint()
        {
            using var client = CreateOfficialApi().CreateClient();
            var response = await client.GetAsync(ApiEndpointsTest.User.GetAll);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(result, jsonSerializerOptions);

            users.Should().NotBeEmpty();
        }
        [Fact]
        public async Task Should_Return_200_From_Insert_User()
        {
            using var client = CreateOfficialApi().CreateClient();
            var request = FakeDataFactory.FakeCreateUserRequest();

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiEndpointsTest.User.Create, httpContent);
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
        public async Task Should_Return_User_From_Login_Real_Endpoint()
        {
            using var client = CreateOfficialApi().CreateClient();
            HttpRequestMessage requestMessage = SetLogin(client);

            var response = client.SendAsync(requestMessage).ConfigureAwait(false);

            var responseInfo = response.GetAwaiter().GetResult();
            var result = await responseInfo.Content.ReadAsStringAsync();

            var loggeduser = JsonSerializer.Deserialize<User>(result, jsonSerializerOptions);
            loggeduser.Id.Should().BeGreaterThan(0);
        }
        [Fact]
        public async Task Should_Return_401_From_Real_Login_Endpoint()
        {
            using var client = CreateOfficialApi().CreateClient();
            HttpRequestMessage requestMessage = SetLogin(client, true);

            var response = client.SendAsync(requestMessage).ConfigureAwait(false);

            var responseInfo = response.GetAwaiter().GetResult();
            responseInfo.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        }

        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            string url = SetGetUrl(2);
            var response = await client.GetAsync(url);
            response.Should().HaveStatusCode(HttpStatusCode.OK);

        }

        [Fact]
        public async Task Should_Return_User_From_Get_Endpoint()
        {
            using var client = CreateOfficialApi().CreateClient();
            string url = SetGetUrl(4);
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var user = JsonSerializer.Deserialize<User>(result, jsonSerializerOptions);
            user!.Id.Should().Be(4);

        }

        [Fact]
        public async Task Should_Update_User_Name_Real_Update_Endpoint()
        {
            using var client = CreateOfficialApi().CreateClient();

            string url = SetGetUrl(4);
            var getResponse = await client.GetAsync(url);
            var getResult = await getResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var returned = JsonSerializer.Deserialize<User>(getResult, jsonSerializerOptions)!;


            returned.Name = "KappaClaus";

            UpdateUserRequest request = new UpdateUserRequest
            {
                Id = returned.Id,
                Name = returned.Name,
                Age = returned.Age,
                Cpf = returned.Cpf,
                Address = returned.Address,
                Email = returned.Email
            };          

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(ApiEndpointsTest.User.Update, httpContent);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var user = JsonSerializer.Deserialize<User>(result, jsonSerializerOptions);
            user!.Name.Should().Be("KappaClaus");

        }


        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            string url = SetDeleteUrl(2);
            var response = await client.DeleteAsync(url);
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Delete_User_Real_Delete_Endpoint()
        {
            using var client = CreateOfficialApi().CreateClient();
            string url = SetDeleteUrl(2);
            await client.DeleteAsync(url);

            var response = await client.GetAsync(ApiEndpointsTest.User.GetAll);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(result, jsonSerializerOptions);

            users.Should().NotContain(x => x.Id == 2);

        }

        [Fact]
        public async Task Should_Return_200_From_ResetPassword_Endpoint()
        {
            using var client = CreateApiWithUserRepository<StubSuccessUserRepository>().CreateClient();
            HttpRequestMessage requestMessage = SetResetPassword(client, FakeDataFactory.FakeCreateUserRequest());
            var response = await client.PutAsync(ApiEndpointsTest.User.ResetPassword, requestMessage.Content);
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Reset_User_Passwrod_ResetPassword_Endpoint()
        {
            using var client = CreateOfficialApi().CreateClient();

            var newUser = FakeDataFactory.FakeCreateUserRequest();
            var jsonString = JsonSerializer.Serialize(newUser);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await client.PostAsync(ApiEndpointsTest.User.Create, httpContent);

            HttpRequestMessage requestMessage = SetResetPassword(client, newUser);
            var response = await client.PutAsync(ApiEndpointsTest.User.ResetPassword, requestMessage.Content);

            LoginUserRequest request = new LoginUserRequest
            {
                Email = newUser.Email,
                Password = "123456789"
            };

            var json = JsonSerializer.Serialize(request);
            requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress + ApiEndpointsTest.User.Login),
                Content = new StringContent(json, encoding: Encoding.UTF8, "application/json")
            };

            response = await client.SendAsync(requestMessage).ConfigureAwait(false);
            var responseInfo = response.Content.ReadAsStringAsync().Result;
            var loggeduser = JsonSerializer.Deserialize<User>(responseInfo, jsonSerializerOptions);
            loggeduser.Id.Should().BeGreaterThan(0);

        }

        private static string SetDeleteUrl(int id)
        {
            string url = ApiEndpointsTest.User.Get;
            url = url.Replace("{id}", $"{id}");
            return url;
        }
        private static string SetGetUrl(int id)
        {
            string url = ApiEndpointsTest.User.Get;
            url = url.Replace("{id}", $"{id}");
            return url;
        }
        private static HttpRequestMessage SetResetPassword(HttpClient client, CreateUserRequest user)
        {
            ResetPasswordUserRequest request = new ResetPasswordUserRequest
            {
                Email = user.Email,
                Password = user.Password,
                NewPassword = "123456789"
            };
            var json = JsonSerializer.Serialize(request);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress + ApiEndpointsTest.User.Login),
                Content = new StringContent(json, encoding: Encoding.UTF8, "application/json")
            };
            return requestMessage;
        }

        private static HttpRequestMessage SetLogin(HttpClient client, bool fail = false)
        {
            LoginUserRequest request = new LoginUserRequest
            {
                Email = "teste@gmail.com",
                Password = "123456789"
            };


            if (fail) request.Password = "12365478965";

            var json = JsonSerializer.Serialize(request);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress + ApiEndpointsTest.User.Login),
                Content = new StringContent(json, encoding: Encoding.UTF8, "application/json")
            };
            return requestMessage;
        }

        private ApiFactory CreateOfficialApi()
        {
            var api = new ApiFactory(services =>
            {
                services.AddApplication();
            });
            return api;
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
