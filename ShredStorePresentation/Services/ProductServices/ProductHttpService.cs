using Contracts.Request.HttpRequests;
using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using ShredStorePresentation.Extensions;
using ShredStorePresentation.Models;
using System.Net;
using System.Text.Json;

namespace ShredStorePresentation.Services.ProductServices
{
    public class ProductHttpService : IProductHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };

        public ProductHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri")!);
        }

        public async Task<ProductResponse> Create(CreateProductRequest product, string token)
        {
            string json = JsonSerializer.Serialize(product);

            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.ProductEndpoints.Create);

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Post, token: token, content: json);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            var created = new ProductResponse();

            if (result is not null)
                created = JsonSerializer.Deserialize<ProductResponse>(result, jsonSerializerOptions);

            return created;
        }

        public async Task Delete(int id, string token)
        {
            var httpResponseMessage = await httpClient.DeleteAsync(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.ProductEndpoints.Delete));
            httpResponseMessage.EnsureSuccessStatusCode();

        }
        public async Task<ProductResponse> Edit(UpdateProductRequest product, string token)
        {
            string json = JsonSerializer.Serialize(product);
            
            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.ProductEndpoints.Update);

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Put, token: token, content: json);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            var edited = new ProductResponse();

            if (result is not null)
                edited = JsonSerializer.Deserialize<ProductResponse>(result, jsonSerializerOptions);

            return edited;
        }
        public async Task<IEnumerable<ProductResponse>> GetAll()
        {
            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.ProductEndpoints.GetAll);

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Get);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            var products = Enumerable.Empty<ProductResponse>();

            if (result is not null)
                products = JsonSerializer.Deserialize<IEnumerable<ProductResponse>>(result, jsonSerializerOptions);
            
            return products;

        }
        public async Task<IEnumerable<ProductResponse>> GetAllByCategory(string Category)
        {
            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UrlGenerator.SetUrlParameters(Category, ApiEndpoints.ProductEndpoints.GetByCategory));

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Get);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            var products = Enumerable.Empty<ProductResponse>();

            if (result is not null)
                products = JsonSerializer.Deserialize<IEnumerable<ProductResponse>>(result, jsonSerializerOptions);

            return products;

        }
        public async Task<IEnumerable<ProductResponse>> GetAllByUserId(int UserId, string token)
        {
            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UrlGenerator.SetUrlParameters(UserId, ApiEndpoints.ProductEndpoints.GetByUserId));

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Get, token: token);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            var products = Enumerable.Empty<ProductResponse>();

            if (result is not null)
                products = JsonSerializer.Deserialize<IEnumerable<ProductResponse>>(result, jsonSerializerOptions);

            return products;


        }

      
        public async Task<IEnumerable<ProductCartItemResponse>> GetAllByCartId(int cartId, string token)
        {

            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UrlGenerator.SetUrlParameters(cartId, ApiEndpoints.ProductEndpoints.GetByCartId));

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Get, token: token);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            var products = Enumerable.Empty<ProductCartItemResponse>();

            if (result is not null)
                products = JsonSerializer.Deserialize<IEnumerable<ProductCartItemResponse>>(result, jsonSerializerOptions);

            return products;

        }
        public async Task<ProductResponse> GetById(int id)
        {

            Uri uri = new Uri(httpClient.BaseAddress + ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.ProductEndpoints.GetByCartId));

            HttpCreateRequestMessageRequest request = new HttpCreateRequestMessageRequest();
            request = request.GenerateCreateMessageRequest(uri, HttpMethod.Get);

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage = requestMessage.GenerateRequestMessage(request);

            var response = await httpClient.SendAsync(requestMessage);

            string? result = await response.EvaluteResponseStatucCode_ReturnContent();

            var product = new ProductResponse();

            if (result is not null)
                product = JsonSerializer.Deserialize<ProductResponse>(result, jsonSerializerOptions);

            return product;

        }
    }

}
