using ScraperAPI.CustomExceptions;
using ScraperAPI.Enumerations;
using ScraperAPI.ErrorModels;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScraperAPI
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _client;

        public HttpClientWrapper()
        {
            _client = new HttpClient();
        }

        private async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        }

        private async Task<string> GetStringAsync(string url)
        {
            return await _client.GetStringAsync(url);
        }

        private async Task<HttpResponseMessage> PostAsync(string url, string json)
        {
            return await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, HttpResponseContentType.JSON));
        }

        public async Task<string> GetHtmlViaHttpRequest(HttpRequestType methodType, string encodedUrl, string json = "")
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            switch (methodType)
            {
                case HttpRequestType.POST:
                    httpResponse = await PostAsync(encodedUrl, json);
                    break;

                case HttpRequestType.GET:
                    httpResponse = await GetAsync(encodedUrl);
                    break;
            }

            if ((int)httpResponse.StatusCode != 200)
                throw new HttpStatusCodeResponseException(httpResponse.StatusCode, new InnerError
                {
                    Code = ((int) httpResponse.StatusCode).ToString(),
                    Message = $"{methodType} Request to {encodedUrl} failed."
                });

            string result = await httpResponse.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<byte[]> GetBytesViaHttpRequest(HttpRequestType methodType, string encodedUrl, string json = "")
        {
            HttpResponseMessage httpResponse = await GetAsync(encodedUrl);

            if ((int)httpResponse.StatusCode != 200)
                throw new HttpStatusCodeResponseException(httpResponse.StatusCode, new InnerError
                {
                    Code = ((int)httpResponse.StatusCode).ToString(),
                    Message = $"{methodType} Request to {encodedUrl} failed."
                });

            byte[] responseBytes = await httpResponse.Content.ReadAsByteArrayAsync();

            return responseBytes;
        }
    }
}
