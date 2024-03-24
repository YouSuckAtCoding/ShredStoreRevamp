using Contracts.Request;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using System.Net;
using System.Text.Json;

namespace ShredStorePresentation.Services.UserService
{
    public class UserHttpService : IUserHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };

        public UserHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri")!);
        }

        public async Task<UserResponse> Login(LoginUserRequest user)
        {
            var json = JsonSerializer.Serialize(user);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(httpClient.BaseAddress + ApiEndpoints.UserEndpoints.Login),
                Content = new StringContent(json, encoding: System.Text.Encoding.UTF8, "application/json")
            };

            var response = httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            var responseInfo = response.GetAwaiter().GetResult();

            var result = await responseInfo.Content.ReadAsStringAsync();

            var loggeduser = JsonSerializer.Deserialize<UserResponse>(result, jsonSerializerOptions);

            return loggeduser;

        }

        public async Task<bool> Create(CreateUserRequest user)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(ApiEndpoints.UserEndpoints.Create, user);

            return httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Created ? true : false;

        }


        public async Task<UserResponse> EditUser(UpdateUserRequest user)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync(ApiEndpoints.UrlGenerator.SetUrlParameters(user.Id, ApiEndpoints.UserEndpoints.Update), user);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var edited = await JsonSerializer.DeserializeAsync<UserResponse>(contentStream, jsonSerializerOptions);

            return edited;
        }
        public async Task<IEnumerable<UserResponse>> GetAll()
        {
            var uses = await httpClient.GetFromJsonAsync<IEnumerable<UserResponse>>(ApiEndpoints.UserEndpoints.GetAll, cancellationToken: default);
            return uses;

        }
        public async Task<UserResponse> GetById(int id)
        {
            var result = await httpClient.GetFromJsonAsync<UserResponse>(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.UserEndpoints.Get));
            return result;
        }

        public async Task<bool> Delete(int sessionId)
        {
            var httpResponseMessage = await httpClient.DeleteAsync(ApiEndpoints.UrlGenerator.SetUrlParameters(sessionId, ApiEndpoints.UserEndpoints.Delete));
            return httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK ? true : false;
        }

        public async Task<bool> ResetUserPassword(ResetPasswordUserRequest request)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync(ApiEndpoints.UserEndpoints.ResetPassword, request);

            return httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK ? true : false;

        }
    }
}
