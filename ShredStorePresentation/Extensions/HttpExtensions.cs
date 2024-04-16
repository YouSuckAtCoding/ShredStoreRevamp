using Contracts.Request.HttpRequests;
using System.Net;

namespace ShredStorePresentation.Extensions
{
    public static class HttpExtensions
    {
        private const string Content_Type = "application/json";
        public static HttpRequestMessage GenerateRequestMessage(this HttpRequestMessage requestMessage, HttpCreateRequestMessageRequest request)
        {
            requestMessage.Method = request.Method;
            requestMessage.RequestUri = request.route;

            if (!string.IsNullOrEmpty(request.Content))
                requestMessage.Content = new StringContent(request.Content, encoding: System.Text.Encoding.UTF8, Content_Type);

            if (!string.IsNullOrEmpty(request.Token))
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.Token);

            return requestMessage;
        }
        public static HttpCreateRequestMessageRequest GenerateCreateMessageRequest(this HttpCreateRequestMessageRequest request, Uri uri, HttpMethod httpMethod, string content = "", string token = "")
        {

            request.route = uri;
            request.Method = httpMethod;
            request.Token = token;
            request.Content = content;

            return request;
        }

        public static async Task<string?> EvaluteResponseStatucCode_ReturnContent(this HttpResponseMessage responseInfo)
        {
            return responseInfo.StatusCode switch
            {
                HttpStatusCode.OK => await responseInfo.Content.ReadAsStringAsync(),
                HttpStatusCode.Unauthorized => null,
                HttpStatusCode.BadRequest => null,
                _ => null
            };
        }

        public static bool EvaluteResponseStatusCode_ReturnBool(this HttpResponseMessage responseInfo)
        {
            return responseInfo.StatusCode switch
            {
                HttpStatusCode.OK => true,
                HttpStatusCode.Unauthorized => false,
                HttpStatusCode.BadRequest => false,
                _ => false
            };
        }
    }
}
