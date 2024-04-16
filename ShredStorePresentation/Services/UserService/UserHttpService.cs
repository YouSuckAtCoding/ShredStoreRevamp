using Contracts.Request;
using Contracts.Request.HttpRequests;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using ShredStorePresentation.Extensions;
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
        public async Task<bool> Create(CreateUserRequest user)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(ApiEndpoints.UserEndpoints.Create, user);

            return httpResponseMessage.StatusCode == HttpStatusCode.Created ? true : false;

        }
        public async Task<UserResponse> Login(LoginUserRequest user)
        {
            var json = JsonSerializer.Serialize(user);

            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UserEndpoints.Login);

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Get, content: json);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            var responseInfo = response.GetAwaiter().GetResult();

            var result = await responseInfo.EvaluteResponseStatucCode_ReturnContent();

            if (result is null)
                return new UserResponse();

            var loggeduser = JsonSerializer.Deserialize<UserResponse>(result, jsonSerializerOptions);

            return loggeduser;

        }
        public async Task<UserResponse> EditUser(UpdateUserRequest user, string token)
        {
            string json = JsonSerializer.Serialize(user);

            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UserEndpoints.Update);

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Put, token: token, content: json);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            if (result is null)
                return new UserResponse();

            var editedUser = JsonSerializer.Deserialize<UserResponse>(result, jsonSerializerOptions);

            return editedUser;
            
        }
        public async Task<IEnumerable<UserResponse>> GetAll(string token)
        {

            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UserEndpoints.GetAll);

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Get, token: token);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            if (result is null)
                return Enumerable.Empty<UserResponse>();

            var users = JsonSerializer.Deserialize<IEnumerable<UserResponse>>(result, jsonSerializerOptions);

            return users;


        }
        public async Task<UserResponse> GetById(int id, string token)
        {

            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.UserEndpoints.Get));

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Get,token: token);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            if (result is null)
                return new UserResponse();

            var loggeduser = JsonSerializer.Deserialize<UserResponse>(result, jsonSerializerOptions);

            return loggeduser;
        }
        public async Task<bool> Delete(int userId, string token)
        {
            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UrlGenerator.SetUrlParameters(userId, ApiEndpoints.UserEndpoints.Delete));

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Delete, token: token);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            var result = response.EvaluteResponseStatusCode_ReturnBool();

            return result;
        }
        public async Task<bool> ResetUserPassword(ResetPasswordUserRequest request, string token)
        {
            string json = JsonSerializer.Serialize(request);

            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UserEndpoints.ResetPassword);

            HttpCreateRequestMessageRequest httpRequest = new HttpCreateRequestMessageRequest();
            httpRequest = httpRequest.GenerateCreateMessageRequest(uri, HttpMethod.Put, token: token, content: json);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(httpRequest);

            var response = await httpClient.SendAsync(requestMessage);

            bool result = response.EvaluteResponseStatusCode_ReturnBool();

            return result;

        }


    }
}
